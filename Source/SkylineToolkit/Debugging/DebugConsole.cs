using SkylineToolkit.Debugging.Commands;
using SkylineToolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/* Note: This is a heavily modified version */
/************************************************************************
* DebugConsole.cs
* Copyright 2011 Calvin Rien
* (http://the.darktable.com)
*
* Derived from version 2.0 of Jeremy Hollingsworth's DebugConsole
*
* Copyright 2008-2010 By: Jeremy Hollingsworth
* (http://www.ennanzus-interactive.com)
*
* Licensed for commercial, non-commercial, and educational use.
*
* THIS PRODUCT IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND. THE
* LICENSOR MAKES NO WARRANTY REGARDING THE PRODUCT, EXPRESS OR IMPLIED.
* THE LICENSOR EXPRESSLY DISCLAIMS AND THE LICENSEE HEREBY WAIVES ALL
* WARRANTIES, EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, ALL
* IMPLIED WARRANTIES OF MERCHANTABILITY AND ALL IMPLIED WARRANTIES OF
* FITNESS FOR A PARTICULAR PURPOSE.
* ************************************************************************/ 
namespace SkylineToolkit.Debugging
{
    /// <summary>
    /// This is a heavily modified version of https://gist.github.com/darktable/1412228
    /// </summary>
    public class DebugConsole : MonoBehaviour, ILogger
    {
        public static readonly Version Version = new Version(1, 0);

        #region Public Unity Component Options
        public bool showDebugMenu;
        public bool registerDefaultCommands = true;
        public KeyCode toggleKey = KeyCode.F10;
        public int maxDisplayedMessages = 100;
        #endregion

        #region Private Fields
        private static DebugConsole _instance;

        private IDictionary<string, IDebugCommand> commandsTable = new Dictionary<string, IDebugCommand>();
        private IDictionary<string, IWatchVar> watchVarTable = new Dictionary<string, IWatchVar>();

        private IList<string> coreCommands = new List<string>();

        private IList<DebugConsoleLogMessage> messages = new List<DebugConsoleLogMessage>();
        private CommandHistory commandHistory = new CommandHistory();

        private string commandInputString = String.Empty;

        private FpsCounter fpsCounter;

        private StringBuilder displayString = new StringBuilder();
        #endregion

        #region GUI Elements
        private readonly string commandInputFieldName = "DebugConsoleInputField99";

        private bool consoleContentChanged = false;

        private Vector2 logScrollPos = Vector2.zero;
        private Vector2 rawLogScrollPos = Vector2.zero;
        private Vector2 watchVarsScrollPos = Vector2.zero;

        private Vector3 guiScale = Vector3.one;
        private Matrix4x4 restoreMatrix = Matrix4x4.identity;

        private bool scaled = false;

        public Rect windowRect = new Rect(30.0f, 30.0f, 380.0f, 450.0f);

        private Rect scrollRect;  // = new Rect(10, 20, 360, 362);
        private Rect inputRect;   // = new Rect(10, 388, 308, 24);
        private Rect enterRect;   // = new Rect(320, 388, 50, 24);
        private Rect toolbarRect; // = new Rect(16, 416, 346, 25);

        private Rect messageLine; // = new Rect(4, 0, 344, 20);
        private int lineOffset = -4;

        private string[] tabs = new string[] { "Log", "Raw Log", "Watch Vars" };

        private Rect nameRect;
        private Rect valueRect;
        private Rect innerRect = new Rect(0, 0, 0, 0);

        private int innerHeight = 0;
        private int toolbarIndex = 0;
        private GUIContent guiContent = new GUIContent();
        private GUI.WindowFunction[] windowMethods;
        private GUIStyle labelStyle;
        #endregion

        #region Log Message Colors
        public static Color msgDefaultColor = Color.white;
        public static Color msgWarningColor = Color.yellow;
        public static Color msgErrorColor = Color.red;
        public static Color msgSystemColor = Color.green;
        public static Color msgInputColor = Color.green;
        public static Color msgOutputColor = Color.cyan;
        #endregion

        #region Properties
        public bool IsOpen
        {
            get
            {
                return DebugConsole._instance.showDebugMenu;
            }
            set
            {
                DebugConsole._instance.showDebugMenu = value;
            }
        }

        public static DebugConsole Instance
        {
            get
            {
                if (DebugConsole._instance == null)
                {
                    DebugConsole._instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;

                    if (DebugConsole._instance != null)
                    {
                        return DebugConsole._instance;
                    }

                    GameObject console = new GameObject("___DebugConsole___");
                    DebugConsole._instance = console.AddComponent<DebugConsole>();
                }

                return DebugConsole._instance;
            }
        }
        #endregion

        private class CommandHistory
        {
            private IList<string> history = new List<string>();

            private string current;

            private int index = 0;

            public void Add(string command)
            {
                if (this.history.Count > 0 && this.history[this.history.Count - 1].Equals(command))
                {
                    this.index = 0;
                    return;
                }

                this.history.Add(command);

                this.index = 0;
            }

            public string Fetch(string current, bool next)
            {
                if (this.index == 0)
                {
                    this.current = current;
                }

                if (this.history.Count == 0)
                {
                    return current;
                }

                this.index += next ? -1 : 1;

                if (this.history.Count + this.index < 0 || this.history.Count + this.index > this.history.Count - 1)
                {
                    this.index = 0;

                    return this.current;
                }

                return this.history[history.Count + this.index];
            }

            public string GetLast()
            {
                if (this.history.Count > 0)
                {
                    return this.history[this.history.Count - 1];
                }

                return String.Empty;
            }
        }

        #region Unity Engine Callbacks
        void Awake()
        {
            DontDestroyOnLoad(this);

            if (DebugConsole._instance != null & DebugConsole._instance != this)
            {
                DestroyImmediate(this, true);
                return;
            }

            DebugConsole._instance = this;

            this.RegisterCoreCommands();
            
            if (this.registerDefaultCommands)
            {
                this.RegisterDefaultCommands();
            }
        }

        void OnEnable()
        {
            float scale = Screen.dpi / 160.0f;

            if (scale != 0.0f & scale >= 1.1f)
            {
                this.scaled = true;
                this.guiScale.Set(scale, scale, scale);
            }

            this.windowMethods = new GUI.WindowFunction[] { LogWindow, RawLogWindow, WatchVarWindow };

            this.fpsCounter = new FpsCounter();
            StartCoroutine(this.fpsCounter.Update());

            this.nameRect = messageLine;
            this.valueRect = messageLine;

            //this.RecalculateRects();
            this.scrollRect = new Rect(10, 20, this.windowRect.width - 20, this.windowRect.height - 88);
            this.inputRect = new Rect(10, this.windowRect.height - 62, this.windowRect.width - 72, 24);
            this.enterRect = new Rect(this.windowRect.width - 60, this.windowRect.height - 62, 50, 24);
            this.toolbarRect = new Rect(16, this.windowRect.height - 34, this.windowRect.width - 32, 25);
            this.messageLine = new Rect(4, 0, this.windowRect.width - 36, 20);

            Application.logMessageReceived += HandleLog;

            this.Log(new DebugConsoleLogMessage(String.Format("DLK Games Debug Console v{0}", DebugConsole.Version), DebugConsoleMessageType.System));
            this.Log(new DebugConsoleLogMessage("Type help for a list of available commands.", DebugConsoleMessageType.System));
            this.Log(new DebugConsoleLogMessage(String.Empty));
        }

        void OnDisable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnGUI()
        {
            Event e = Event.current;

            if (this.scaled)
            {
                this.restoreMatrix = GUI.matrix;

                GUI.matrix = GUI.matrix * Matrix4x4.Scale(guiScale);
            }

            while (this.messages.Count > this.maxDisplayedMessages)
            {
                this.messages.RemoveAt(0);
            }

            if (e.keyCode == toggleKey & e.type == EventType.keyUp)
            {
                this.showDebugMenu = !this.showDebugMenu;
            }

            if (!showDebugMenu)
            {
                return;
            }

            this.labelStyle = GUI.skin.label;

            this.RecalculateRects();

            this.innerRect.width = messageLine.width;

            this.windowRect = GUI.Window(-9999, this.windowRect, windowMethods[toolbarIndex], String.Format("Debug Console\tFPS: {0:00.0}", this.fpsCounter.Current));
            GUI.BringWindowToFront(-9999);

            if (GUI.GetNameOfFocusedControl() == this.commandInputFieldName)
            {
                if (e.isKey & e.type == EventType.keyUp)
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.Return:
                        case KeyCode.KeypadEnter:
                            this.EvalCommand(this.commandInputString);

                            this.commandInputString = String.Empty;
                            break;
                        case KeyCode.UpArrow:
                            this.commandInputString = this.commandHistory.Fetch(this.commandInputString, true);
                            break;
                        case KeyCode.DownArrow:
                            this.commandInputString = this.commandHistory.Fetch(this.commandInputString, false);
                            break;
                        case KeyCode.Escape:
                            this.showDebugMenu = false;
                            break;
                    }
                }
            }

            if (this.scaled)
            {
                GUI.matrix = restoreMatrix;
            }

            if (this.consoleContentChanged & e.type == EventType.Repaint)
            {
                this.logScrollPos.y = 50000.0f;
                this.rawLogScrollPos.y = 50000.0f;

                this.GenerateDisplayString();

                this.consoleContentChanged = false;
            }
        }

        void OnDestory()
        {
            this.StopAllCoroutines();
        }

        void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Exception && !String.IsNullOrEmpty(stackTrace))
            {
                message += "\n" + stackTrace;
            }

            this.Log(new DebugConsoleLogMessage(message, (DebugConsoleMessageType)type));
        }
        #endregion

        #region Public Logging Methods

        public static void Log(string format, DebugConsoleMessageType messageType, Color displayColor, params object[] args)
        {
            DebugConsole.Instance.Log(new DebugConsoleLogMessage(String.Format(format, args), messageType, displayColor));
        }

        public static void Log(string message, DebugConsoleMessageType messageType)
        {
            DebugConsole.Instance.Log(new DebugConsoleLogMessage(message, messageType));
        }

        public void LogMessage(string module, string message, MessageType type)
        {
            this.Log(new DebugConsoleLogMessage(String.Format("[{0}] {1}", module, message), type.ToDebugConsoleType()));
        }
        
        #endregion

        #region Console API
        public static void Execute(string command)
        {
            DebugConsole.Instance.EvalCommand(command);
        }

        public static void Clear()
        {
            DebugConsole.Instance.ClearLog();
        }

        public static void RegisterCommand(string commandName, IDebugCommand command)
        {
            DebugConsole.Instance.RegisterCommandCallback(commandName, command);
        }

        public static void UnregisterCommand(string commandName)
        {
            DebugConsole.Instance.UnregisterCommandCallback(commandName);
        }

        public static void RegisterWatchVar(IWatchVar watchVar)
        {
            DebugConsole.Instance.AddWatchVar(watchVar);
        }

        public static void UnregisterWatchVar(string name)
        {
            DebugConsole.Instance.RemoveWatchVar(name);
        }
        #endregion

        #region Internal API

        internal void Log(DebugConsoleLogMessage message)
        {
            this.messages.Add(message);

            this.consoleContentChanged = true;
        }

        internal void EvalCommand(string command)
        {
            command = command.Trim();

            if (string.IsNullOrEmpty(command))
            {
                this.Log(new DebugConsoleLogMessage(String.Empty));
                return;
            }

            if (command.Equals("!"))
            {
                this.EvalCommand(this.commandHistory.GetLast());
                return;
            }

            this.commandHistory.Add(command);
            this.Log(new DebugConsoleLogMessage(command, DebugConsoleMessageType.Input));

            IList<string> input = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = input.Select(str => str.ToLower()).ToList();

            string cmd = input[0];
            input.RemoveAt(0);

            switch (cmd)
            {
                case "clear":
                    this.ClearLog();
                    break;
                case "echo":
                    this.Log(new DebugConsoleLogMessage(String.Join(" ", input.ToArray())));
                    break;
                default:
                    if (this.commandsTable.ContainsKey(cmd))
                    {
                        IDebugCommand dCmd = this.commandsTable[cmd];

                        CommandContext context = new CommandContext(dCmd, input.ToArray(), new char[0]);

                        if (!dCmd.CanExecute(context))
                        {
                            return;
                        }

                        int exitCode;
                        if ((exitCode = dCmd.Execute(context, input.ToArray(), new char[0])) != 0)
                        {
                            this.Log(new DebugConsoleLogMessage(String.Format("Command ended with unproper exit code: {0}", exitCode)));
                        }
                    }
                    else
                    {
                        this.Log(new DebugConsoleLogMessage(String.Format("Unknown Command: {0}", cmd)));
                    }
                    break;
            }
        }

        internal void ClearLog()
        {
            this.messages.Clear();
        }

        internal void RegisterCommandCallback(string commandName, IDebugCommand command)
        {
            commandName = commandName.ToLower();

            if (this.coreCommands.Contains(commandName))
            {
                throw new ArgumentException("Cannot overwrite a core command.", "commandName");
            }

            if (this.commandsTable.ContainsKey(commandName))
            {
                this.commandsTable[commandName] = command;
                return;
            }

            this.commandsTable.Add(commandName.ToLower(), command);
        }

        internal void UnregisterCommandCallback(string commandName)
        {
            commandName = commandName.ToLower();

            if (this.coreCommands.Contains(commandName))
            {
                throw new ArgumentException("Cannot remove a core command.", "commandName");
            }

            if (this.commandsTable.ContainsKey(commandName))
            {
                this.commandsTable.Remove(commandName);
            }
        }

        internal void AddWatchVar(IWatchVar watchVar)
        {
            this.watchVarTable.Add(watchVar.Name, watchVar);
        }

        internal void RemoveWatchVar(string name)
        {
            if (this.watchVarTable.ContainsKey(name))
            {
                this.watchVarTable.Remove(name);
            }
        }

        private void RegisterCoreCommands()
        {
            this.coreCommands.Add("clear");
            this.coreCommands.Add("echo");

            this.RegisterCommandCallback("help", new HelpCommand(commandsTable));
            this.coreCommands.Add("help");

            this.RegisterCommandCallback("alias", new AliasCommand());
            this.coreCommands.Add("aliad");
        }

        private void RegisterDefaultCommands()
        {
            //this.RegisterCommandCallback("pause", new PauseCommand());
        }

        private string GetDisplayString()
        {
            if (this.messages == null)
            {
                return String.Empty;
            }

            return this.displayString.ToString();
        }

        private void GenerateDisplayString()
        {
            this.displayString.Length = 0;

            foreach (DebugConsoleLogMessage msg in this.messages)
            {
                this.displayString.AppendLine(msg.ToString());
            }
        }
        #endregion

        #region Window Methods
        private void LogWindow(int windowId)
        {
            GUI.Box(this.scrollRect, String.Empty);

            this.innerRect.height = this.innerHeight < this.scrollRect.height ? this.scrollRect.height : this.innerHeight;

            this.logScrollPos = GUI.BeginScrollView(this.scrollRect, this.logScrollPos, this.innerRect, false, true);

            if (this.messages != null || this.messages.Count > 0)
            {
                Color tmpColor = GUI.contentColor;

                this.messageLine.y = 0;

                foreach (DebugConsoleLogMessage msg in this.messages)
                {
                    GUI.contentColor = msg.Color;

                    this.guiContent.text = msg.ToGuiString();

                    this.messageLine.height = this.labelStyle.CalcHeight(this.guiContent, this.messageLine.width);

                    GUI.Label(this.messageLine, this.guiContent);

                    this.messageLine.y += this.messageLine.height + this.lineOffset;

                    this.innerHeight = this.messageLine.y > this.scrollRect.height ? (int)this.messageLine.y : (int)this.scrollRect.height;
                }

                GUI.contentColor = tmpColor;
            }

            GUI.EndScrollView();

            this.DrawToolbar();
        }

        private void RawLogWindow(int windowId)
        {
            this.guiContent.text = this.GetDisplayString();

            float calcHeight = GUI.skin.textArea.CalcHeight(this.guiContent, this.messageLine.width);

            this.innerRect.height = calcHeight < this.scrollRect.height ? this.scrollRect.height : calcHeight;

            this.rawLogScrollPos = GUI.BeginScrollView(this.scrollRect, this.rawLogScrollPos, this.innerRect, false, true);

            GUI.TextArea(this.innerRect, this.guiContent.text);

            GUI.EndScrollView();

            this.DrawToolbar();
        }

        private void WatchVarWindow(int windowId)
        {
            GUI.Box(this.scrollRect, String.Empty);

            this.innerRect.height = this.innerHeight < this.scrollRect.height ? this.scrollRect.height : this.innerHeight;

            this.watchVarsScrollPos = GUI.BeginScrollView(this.scrollRect, this.watchVarsScrollPos, this.innerRect, false, true);

            this.nameRect.x = this.messageLine.x;
            this.nameRect.y = this.valueRect.y = 0;

            float totalWidth = this.messageLine.width - this.messageLine.x;

            float nameMin, nameMax, valueMin, valueMax, stepHeight;
            GUIStyle textAreaStyle = GUI.skin.textArea;

            foreach (var watchvar in this.watchVarTable)
            {
                GUIContent nameContent = new GUIContent(String.Format("{0}:", watchvar.Value.Name));
                GUIContent valueContent = new GUIContent(watchvar.Value.ToString());

                this.labelStyle.CalcMinMaxWidth(nameContent, out nameMin, out nameMax);
                textAreaStyle.CalcMinMaxWidth(valueContent, out valueMin, out valueMax);

                if (nameMax > totalWidth)
                {
                    this.nameRect.width = totalWidth - valueMin;
                    this.valueRect.width = valueMin;
                }
                else if (valueMax + nameMax > totalWidth)
                {
                    this.nameRect.width = nameMin;
                    this.valueRect.width = totalWidth - nameMin;
                }
                else
                {
                    this.nameRect.width = nameMax;
                    this.valueRect.width = valueMax;
                }

                this.nameRect.height = this.labelStyle.CalcHeight(nameContent, this.nameRect.width);
                this.valueRect.height = textAreaStyle.CalcHeight(valueContent, this.valueRect.width);

                this.valueRect.x = totalWidth - this.valueRect.width + this.nameRect.x;

                GUI.Label(this.nameRect, nameContent);
                GUI.TextArea(this.valueRect, valueContent.text);

                stepHeight = Mathf.Max(this.nameRect.height, this.valueRect.height) + 4;

                this.nameRect.y += stepHeight;
                this.valueRect.y += stepHeight;

                this.innerHeight = this.valueRect.y > this.scrollRect.height ? (int)this.valueRect.y : (int)this.scrollRect.height;
            }

            GUI.EndScrollView();

            this.DrawToolbar();
        }

        private void DrawToolbar()
        {
            GUI.SetNextControlName(this.commandInputFieldName);
            this.commandInputString = GUI.TextField(this.inputRect, this.commandInputString);

            if (GUI.Button(enterRect, "Enter"))
            {
                this.EvalCommand(this.commandInputString);
                this.commandInputString = String.Empty;
            }

            int index = GUI.Toolbar(this.toolbarRect, this.toolbarIndex, tabs);

            if (index != this.toolbarIndex)
            {
                this.toolbarIndex = index;
            }

            GUI.DragWindow();
        }

        private void RecalculateRects()
        {
            this.scrollRect.Set(10, 20, this.windowRect.width - 20, this.windowRect.height - 88);
            this.inputRect.Set(10, this.windowRect.height - 62, this.windowRect.width - 72, 24);
            this.enterRect.Set(this.windowRect.width - 60, this.windowRect.height - 62, 50, 24);
            this.toolbarRect.Set(16, this.windowRect.height - 34, this.windowRect.width - 32, 25);
            this.messageLine.Set(4, 0, this.windowRect.width - 36, 20);
        }
        #endregion
    }
}
