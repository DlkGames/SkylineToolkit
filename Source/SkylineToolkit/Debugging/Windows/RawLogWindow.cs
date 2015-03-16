using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging.Windows
{
    internal class RawLogWindow : IDebugConsoleWindow
    {
        private Vector2 scrollPosition = Vector2.zero;

        private Rect messageLine;
        private Rect innerRect = new Rect(0, 0, 0, 0);

        private GUIContent guiContent = new GUIContent();

        private StringBuilder displayString = new StringBuilder();

        public UnityEngine.Vector2 ScrollPosition
        {
            get;
            set;
        }

        public void SetupWindow(int windowId, DebugConsole debugConsole)
        {
            debugConsole.ConsoleContentChanged += ConsoleContentChanged_Handler;
            debugConsole.RectsRecalculated += RectsRecalculated_Handler;

            this.messageLine = new Rect(4, 0, debugConsole.WindowRect.width - 36, 20);

            this.innerRect.width = messageLine.width;
        }

        public void DrawWindow(int windowId, DebugConsole debugConsole)
        {
            this.guiContent.text = this.GetDisplayString(debugConsole);

            float calcHeight = GUI.skin.textArea.CalcHeight(this.guiContent, this.messageLine.width);

            this.innerRect.height = calcHeight < debugConsole.ScrollRect.height ? debugConsole.ScrollRect.height : calcHeight;

            this.scrollPosition = GUI.BeginScrollView(debugConsole.ScrollRect, this.scrollPosition, this.innerRect, false, true);

            GUI.TextArea(this.innerRect, this.guiContent.text);

            GUI.EndScrollView();
        }

        private void RectsRecalculated_Handler(object sender, EventArgs e)
        {
            this.messageLine.Set(4, 0, ((DebugConsole)sender).WindowRect.width - 36, 20);
        }

        private void ConsoleContentChanged_Handler(object sender, EventArgs e)
        {
            this.scrollPosition.y = 50000.0f;

            this.GenerateDisplayString((DebugConsole)sender);
        }

        private string GetDisplayString(DebugConsole debugConsole)
        {
            if (debugConsole.Messages == null)
            {
                return String.Empty;
            }

            return this.displayString.ToString();
        }

        private void GenerateDisplayString(DebugConsole debugConsole)
        {
            this.displayString.Length = 0;

            foreach (DebugConsoleLogMessage msg in debugConsole.Messages)
            {
                this.displayString.AppendLine(msg.ToString());
            }
        }
    }
}
