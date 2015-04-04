using SkylineToolkit.Debugging.Controls;
using SkylineToolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging
{
    public sealed class Debugger : MonoBehaviour, ILogger
    {
        public const string DEBUGGER_NAME = "___SkyT_Debugger___";
        public const string DEBUGGER_WINDOW_NAME = "(SkyT) Debugger";

        public static KeyCode toggleKey = KeyCode.F10;

        private static bool isInitialized = false;
        private static Debugger instance;

        private DebuggerWindow window;
        private FpsCounter fpsCounter;

        #region Properties

        #region Static

        public static Debugger Instance
        {
            get
            {
                if (instance == null)
                {
                    Initialize();
                }

                return instance;
            }
        }

        #endregion

        #region GUI

        internal DebuggerWindow Window
        {
            get { return window; }
        }

        #endregion

        #region Internal Components

        internal FpsCounter FpsCounter
        {
            get { return fpsCounter; }
        }

        #endregion

        #region Game State

        public float CurrentFps
        {
            get { return fpsCounter.Current; }
        }

        #endregion

        #endregion

        #region Initialization

        internal static Debugger Initialize()
        {
            if (isInitialized)
            {
                return instance;
            }

            GameObject go = new GameObject(DEBUGGER_NAME);

            Debugger debugger = go.AddComponent<Debugger>();

            isInitialized = true;
            instance = debugger;

            return debugger;
        }

        #endregion

        #region Unity Engine Callbacks

        void Awake()
        {
            DontDestroyOnLoad(this);

            if (isInitialized)
            {
                try
                {
                    throw new InvalidOperationException("Debugger already initialized!");
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                finally
                {
                    this.enabled = false;
                }
            }

            this.fpsCounter = new Debugging.FpsCounter();

            // Hook into LoadingManager... we have to restore our window on new levels TODO needs another workaround
            ColossalFramework.Singleton<LoadingManager>.instance.m_levelLoaded += LoadingManager_LevelLoaded;

            if (window == null)
            {
                CreateDebuggerWindow();
            }
        }

        void LoadingManager_LevelLoaded(SimulationManager.UpdateMode updateMode)
        {
            if (window == null)
            {
                CreateDebuggerWindow();
            }
        }

        void OnEnable()
        {
            StartCoroutine(this.fpsCounter.Update());
        }

        void OnGUI()
        {
            Event e = Event.current;

            if (e.keyCode == toggleKey & e.type == EventType.keyUp)
            {
                window.IsVisible = !window.IsVisible;
            }
        }

        #endregion

        #region Methods

        #region Initialization

        private void CreateDebuggerWindow()
        {
            GameObject go = new GameObject(DEBUGGER_WINDOW_NAME);

            go.transform.parent = SkylineToolkit.UI.ColossalControl.ColossalUIView.transform;

            DebuggerWindow window = go.AddComponent<DebuggerWindow>();

            Vector2 size = new Vector2(800, 600);

            window.Size = size;
            window.Position = new Vector3((size.x / 2) * -1, (size.y / 2) * -1);
            window.MinimumSize = new Vector2(500, 300);
            window.IsResizable = true;
            window.IsVisible = false;

            this.window = window;
            window.Debugger = this;

            GameObject.DontDestroyOnLoad(window);
        }

        #endregion

        #endregion

        #region ILogger implementation

        public void LogMessage(string module, string message, MessageType type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
