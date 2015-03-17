using System;
using UnityEngine;

namespace SkylineToolkit.Debugging.Windows
{
    internal class LogWindow : IDebugConsoleWindow
    {
        private int lineOffset = -4;
        private int innerHeight = 0;

        private Vector2 scrollPosition = Vector2.zero;

        private Rect messageLine;
        private Rect innerRect = new Rect(0, 0, 0, 0);

        private GUIContent guiContent = new GUIContent();
        private GUIStyle labelStyle;

        public void SetupWindow(int windowId, DebugConsole debugConsole)
        {
            debugConsole.ConsoleContentChanged += ConsoleContentChanged_Handler;
            debugConsole.RectsRecalculated += RectsRecalculated_Handler;

            this.messageLine = new Rect(4, 0, debugConsole.WindowRect.width - 36, 20);

            this.innerRect.width = messageLine.width;
        }

        public void DrawWindow(int windowId, DebugConsole debugConsole)
        {
            labelStyle = GUI.skin.label;

            this.innerRect.height = this.innerHeight < debugConsole.ScrollRect.height ? debugConsole.ScrollRect.height : this.innerHeight;

            this.scrollPosition = GUI.BeginScrollView(debugConsole.ScrollRect, this.scrollPosition, this.innerRect, false, true);

            if (debugConsole.Messages != null || debugConsole.Messages.Count > 0)
            {
                Color tmpColor = GUI.contentColor;

                this.messageLine.y = 0;

                foreach (DebugConsoleLogMessage msg in debugConsole.Messages)
                {
                    GUI.contentColor = msg.Color;

                    this.guiContent.text = msg.ToGuiString();

                    this.messageLine.height = this.labelStyle.CalcHeight(this.guiContent, this.messageLine.width);

                    GUI.Label(this.messageLine, this.guiContent);

                    this.messageLine.y += this.messageLine.height + this.lineOffset;

                    this.innerHeight = this.messageLine.y > debugConsole.ScrollRect.height ? (int)this.messageLine.y : (int)debugConsole.ScrollRect.height;
                }

                GUI.contentColor = tmpColor;
            }

            GUI.EndScrollView();
        }

        private void RectsRecalculated_Handler(object sender, EventArgs e)
        {
            this.messageLine.Set(4, 0, ((DebugConsole)sender).WindowRect.width - 36, 20);
        }

        private void ConsoleContentChanged_Handler(object sender, EventArgs e)
        {
            this.scrollPosition.y = 50000.0f;
        }
    }
}
