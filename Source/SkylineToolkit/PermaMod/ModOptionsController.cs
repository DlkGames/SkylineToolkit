using SkylineToolkit.Events;
using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            InitializeOptionsWindow();
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        private void InitializeOptionsWindow()
        {
            Log.Debug("ModOptions", "Creating mod options window");

            optionsWindow = Window.Create("(SkyT) Mod Options", false);
            optionsWindow.IsVisible = false;

            optionsWindow.IsResizable = true;
            optionsWindow.Title = "Mod Options";
            optionsWindow.Close += optionsWindow_Close;

            GameObject.DontDestroyOnLoad(optionsWindow);
        }

        void optionsWindow_Close(object sender, CancellableEventArgs e)
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
