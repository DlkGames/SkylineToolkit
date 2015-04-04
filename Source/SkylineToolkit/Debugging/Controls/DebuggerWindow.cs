using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging.Controls
{
    internal class DebuggerWindow : WindowControl
    {
        private static string windowTitleFormat = "SkylineToolkit Debugger\t\tFPS: {0:00.0}";

        private Debugger debugger;

        internal Debugger Debugger
        {
            get { return debugger; }
            set { debugger = value; }
        }

        #region Unity Engine callbacks

        protected override void Awake()
        {
            base.Awake();

            SetupGUI();
        }

        protected virtual void OnGUI()
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.Title = String.Format(windowTitleFormat, debugger.FpsCounter.Current);
        }

        #endregion

        private void SetupGUI()
        {
            TextField text = new TextField("test_textfield", "Initial content", new Vector3(100, 100, 3));

            text.Width = 200;

            this.WindowPanel.AttachControl(text);

            Button testButton = new Button("test_button", "Testbutton Label", new Vector3(310, 100, 3));
            testButton.Width = 140;

            this.WindowPanel.AttachControl(testButton);
        }
    }
}
