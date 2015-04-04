using SkylineToolkit.Debugging.Commands;
using SkylineToolkit.Debugging.Windows;
using SkylineToolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string DEBUG_CONSOLE_NAME = "___DebugConsole___";

        public static readonly Version Version = new Version(1, 0);

        public event EventHandler ConsoleContentChanged;

        public event EventHandler RectsRecalculated;

        #region Public Unity Component Options
        public bool showDebugMenu;
        public bool registerDefaultCommands = true;
        public KeyCode toggleKey = KeyCode.F11;
        public int maxDisplayedMessages = 100;
        #endregion

        #region Private Fields
        private static DebugConsole _instance;

        private IDictionary<int, string> windowNames = new Dictionary<int, string>();
        private IDictionary<int, IDebugConsoleWindow> windows = new Dictionary<int, IDebugConsoleWindow>();
        private IDictionary<string, IDebugCommand> commandsTable = new Dictionary<string, IDebugCommand>();
        private IDictionary<string, IWatchVar> watchVarTable = new Dictionary<string, IWatchVar>();

        private IList<string> coreCommands = new List<string>();

        private IList<DebugConsoleLogMessage> messages = new List<DebugConsoleLogMessage>();
        private CommandHistory commandHistory = new CommandHistory();

        private string commandInputString = String.Empty;

        private FpsCounter fpsCounter;
        #endregion

        #region GUI Elements
        private readonly string commandInputFieldName = "DebugConsoleInputField99";

        private bool consoleContentChanged = false;

        private Vector3 guiScale = Vector3.one;
        private Matrix4x4 restoreMatrix = Matrix4x4.identity;

        private bool scaled = false;

        private Rect windowRect = new Rect(30.0f, 30.0f, 400.0f, 500.0f);

        private Rect scrollRect;  // = new Rect(10, 20, 360, 362);
        private Rect inputRect;   // = new Rect(10, 388, 308, 24);
        private Rect enterRect;   // = new Rect(320, 388, 50, 24);
        private Rect toolbarRect; // = new Rect(16, 416, 346, 25);

        private string[] tabs = new string[] { "Log", "Raw Log", "Watch Vars" };
        private int toolbarIndex = 0;
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

                    GameObject console = new GameObject(DEBUG_CONSOLE_NAME);
                    DebugConsole._instance = console.AddComponent<DebugConsole>();
                }

                return DebugConsole._instance;
            }
        }

        public IList<DebugConsoleLogMessage> Messages
        {
            get
            {
                return this.messages;
            }
            private set
            {
                if (value == null)
                {
                    this.messages = new List<DebugConsoleLogMessage>();
                }

                this.messages = value;
            }
        }

        public IDictionary<string, IWatchVar> WatchVars
        {
            get
            {
                return this.watchVarTable;
            }
            private set
            {
                this.watchVarTable = value;
            }
        }

        public Rect ScrollRect
        {
            get
            {
                return this.scrollRect;
            }
        }

        public Rect WindowRect
        {
            get
            {
                return this.windowRect;
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

            this.AddWindow("Log", new LogWindow());
            this.AddWindow("Raw Log", new RawLogWindow());
            this.AddWindow("Watch Vars", new WatchVarsWindow());
        }

        void OnEnable()
        {
            float scale = Screen.dpi / 160.0f;

            if (scale != 0.0f & scale >= 1.1f)
            {
                this.scaled = true;
                this.guiScale.Set(scale, scale, scale);
            }

            this.fpsCounter = new FpsCounter();
            StartCoroutine(this.fpsCounter.Update());

            this.scrollRect = new Rect(10, 20, this.windowRect.width - 20, this.windowRect.height - 88);
            this.inputRect = new Rect(10, this.windowRect.height - 62, this.windowRect.width - 72, 24);
            this.enterRect = new Rect(this.windowRect.width - 60, this.windowRect.height - 62, 50, 24);
            this.toolbarRect = new Rect(16, this.windowRect.height - 34, this.windowRect.width - 32, 25);

            Application.logMessageReceived += HandleLog;

            this.AddMessage(new DebugConsoleLogMessage(String.Format("DLK Games Debug Console v{0}", DebugConsole.Version), DebugConsoleMessageType.System));
            this.AddMessage(new DebugConsoleLogMessage("Type help for a list of available commands.", DebugConsoleMessageType.System));
            this.AddMessage(new DebugConsoleLogMessage(String.Empty));
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void OnGUI()
        {
            Event e = Event.current;

            if (e.keyCode == toggleKey & e.type == EventType.keyUp)
            {
                this.showDebugMenu = !this.showDebugMenu;
            }

            if (!showDebugMenu)
            {
                return;
            }

            if (this.scaled)
            {
                this.restoreMatrix = GUI.matrix;

                GUI.matrix = GUI.matrix * Matrix4x4.Scale(guiScale);
            }

            this.RemoveMessageOverhead();
            this.RecalculateRects();

            this.windowRect = GUI.Window(-99999, this.windowRect, this.DrawWindow, String.Format("Debug Console\tFPS: {0:00.0}", this.fpsCounter.Current));
            GUI.BringWindowToFront(-99999);

            HandleUserInput(e);

            if (this.scaled)
            {
                GUI.matrix = restoreMatrix;
            }

            if (this.consoleContentChanged & e.type == EventType.Repaint)
            {
                this.OnConsoleContentChanged();

                this.consoleContentChanged = false;
            }
        }

        private void HandleUserInput(Event e)
        {
            if (GUI.GetNameOfFocusedControl() == this.commandInputFieldName)
            {
                if (e.isKey & e.type == EventType.keyUp)
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.Return:
                        case KeyCode.KeypadEnter:
                            this.EvaluateCommand(this.commandInputString);

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
        }

        private void RemoveMessageOverhead()
        {
            while (this.messages.Count > this.maxDisplayedMessages)
            {
                this.messages.RemoveAt(0);
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

            this.AddMessage(new DebugConsoleLogMessage(message, (DebugConsoleMessageType)type));
        }

        #endregion

        #region Public Logging Methods

        public static void Log(string format, DebugConsoleMessageType messageType, Color displayColor, params object[] args)
        {
            DebugConsole.Instance.AddMessage(new DebugConsoleLogMessage(String.Format(format, args), messageType, displayColor));
        }

        public static void Log(string message, DebugConsoleMessageType messageType)
        {
            DebugConsole.Instance.AddMessage(new DebugConsoleLogMessage(message, messageType));
        }

        public void LogMessage(string module, string message, MessageType type)
        {
            this.AddMessage(new DebugConsoleLogMessage(String.Format("[{0}] {1}", module, message), type.ToDebugConsoleType()));
        }
        
        #endregion

        #region Console API

        public static void Execute(string command)
        {
            DebugConsole.Instance.EvaluateCommand(command);
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

        public static void RegisterWindow(string name, IDebugConsoleWindow window)
        {
            DebugConsole.Instance.AddWindow(name, window);
        }

        public static void UnregisterWindow(string name)
        {
            DebugConsole.Instance.RemoveWindow(name);
        }

        #endregion

        #region Internal API

        internal void AddMessage(DebugConsoleLogMessage message)
        {
            this.messages.Add(message);

            this.consoleContentChanged = true;
        }

        internal void EvaluateCommand(string command)
        {
            command = command.Trim();

            if (string.IsNullOrEmpty(command))
            {
                this.AddMessage(new DebugConsoleLogMessage(String.Empty));
                return;
            }

            if (command.Equals("!"))
            {
                this.EvaluateCommand(this.commandHistory.GetLast());
                return;
            }

            this.commandHistory.Add(command);
            this.AddMessage(new DebugConsoleLogMessage(command, DebugConsoleMessageType.Input));

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
                    this.AddMessage(new DebugConsoleLogMessage(String.Join(" ", input.ToArray())));
                    break;
                default:
                    if (this.commandsTable.ContainsKey(cmd))
                    {
                        IDebugCommand dCmd = this.commandsTable[cmd];

                        CommandContext context = new CommandContext(command, dCmd, input.ToArray(), new char[0]);

                        if (!dCmd.CanExecute(context))
                        {
                            return;
                        }

                        int exitCode;
                        if ((exitCode = dCmd.Execute(context, input.ToArray(), new char[0])) != 0)
                        {
                            this.AddMessage(new DebugConsoleLogMessage(String.Format("Command ended with unproper exit code: {0}", exitCode)));
                        }
                    }
                    else
                    {
                        this.AddMessage(new DebugConsoleLogMessage(String.Format("Unknown Command: {0}", cmd)));
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

        internal void AddWindow(string name, IDebugConsoleWindow window)
        {
            int windowId = this.windowNames.Count;

            this.windowNames.Add(windowId, name);

            this.windows.Add(windowId, window);

            window.SetupWindow(windowId, this);
        }

        internal void RemoveWindow(string name)
        {
            int windowId = this.windowNames.Where(w => w.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase)).First().Key;

            this.windowNames.Remove(windowId);

            this.windows.Remove(windowId);
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
            this.RegisterCommandCallback("eval", new EvalCommand());
            //this.RegisterCommandCallback("pause", new PauseCommand());
        }

        #endregion

        #region Window Methods

        private void DrawWindow(int windowId)
        {
            IDebugConsoleWindow window = this.windows[this.toolbarIndex];

            GUI.Box(this.scrollRect, String.Empty);

            window.DrawWindow(windowId, this);

            this.DrawToolbar();
        }

        private void DrawToolbar()
        {
            GUI.SetNextControlName(this.commandInputFieldName);
            this.commandInputString = GUI.TextField(this.inputRect, this.commandInputString);

            if (GUI.Button(enterRect, "Enter"))
            {
                this.EvaluateCommand(this.commandInputString);
                this.commandInputString = String.Empty;
            }

            int index = GUI.Toolbar(this.toolbarRect, this.toolbarIndex, this.windowNames.Values.ToArray());

            if (index != this.toolbarIndex)
            {
                this.toolbarIndex = index;
            }

            GUI.DragWindow();
        }

        public void RecalculateRects()
        {
            this.scrollRect.Set(10, 20, this.windowRect.width - 20, this.windowRect.height - 88);
            this.inputRect.Set(10, this.windowRect.height - 62, this.windowRect.width - 72, 24);
            this.enterRect.Set(this.windowRect.width - 60, this.windowRect.height - 62, 50, 24);
            this.toolbarRect.Set(16, this.windowRect.height - 34, this.windowRect.width - 32, 25);

            this.OnRectsRecalculated();
        }

        #endregion

        #region On Events

        private void OnConsoleContentChanged()
        {
            if (this.ConsoleContentChanged != null)
            {
                this.ConsoleContentChanged(this, null);
            }
        }

        private void OnRectsRecalculated()
        {
            if (this.RectsRecalculated != null)
            {
                this.RectsRecalculated(this, null);
            }
        }

        #endregion
    }
}
