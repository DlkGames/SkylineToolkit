using SkylineToolkit.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        void OnGUI()
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.Title = String.Format(windowTitleFormat, debugger.FpsCounter.Current);
        }
    }
}
