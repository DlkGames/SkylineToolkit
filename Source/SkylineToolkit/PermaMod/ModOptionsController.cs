using SkylineToolkit.Events;
using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public sealed class ModOptionsController : MonoBehaviour
    {
        private static ModOptionsController instance;

        private WindowControl optionsWindow;

        public static ModOptionsController Instance
        {
            get
            {
                return instance;
            }
        }

        internal WindowControl OptionsWindow
        {
            get { return optionsWindow; }
        }

        void Awake()
        {
            DontDestroyOnLoad(this);

            if (instance == null)
            {
                instance = this;
            }

            // Hook into LoadingManager... we have to restore our window on new levels TODO needs another workaround
            ColossalFramework.Singleton<LoadingManager>.instance.m_levelLoaded += LoadingManager_LevelLoaded;

            if (optionsWindow == null)
            {
                CreateOptionsWindow();
            }
        }

        void LoadingManager_LevelLoaded(SimulationManager.UpdateMode updateMode)
        {
            if (optionsWindow == null)
            {
                CreateOptionsWindow();
            }
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        private void CreateOptionsWindow()
        {
            Log.Debug("ModOptions", "Creating mod options window");

            optionsWindow = Window.Create("(SkyT) Mod Options", false);
            optionsWindow.IsVisible = false;

            optionsWindow.IsResizable = true;
            optionsWindow.Title = "Mod Options";
            optionsWindow.Close += optionsWindow_Close;

            GameObject.DontDestroyOnLoad(optionsWindow);
        }

        private void optionsWindow_Close(object sender, CancellableEventArgs e)
        {
            SaveModOptions();
        }

        private void SaveModOptions()
        {
            Log.Info("Saving mod options...");
        }

        public void ShowWindow()
        {
            this.optionsWindow.Show();
        }

        public void HideWindow()
        {
            this.optionsWindow.Hide();
        }

        public static void ShowOptionsWindow()
        {
            instance.ShowWindow();
        }

        public static void HideOptionsWindow()
        {
            instance.HideWindow();
        }
    }
}
