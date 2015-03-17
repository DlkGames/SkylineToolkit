using System;
using UnityEngine;

namespace SkylineToolkit.Debugging.Windows
{
    internal class WatchVarsWindow : IDebugConsoleWindow
    {
        private int innerHeight = 0;

        private Vector2 scrollPosition = Vector2.zero;

        private Rect messageLine;
        private Rect nameRect;
        private Rect valueRect;
        private Rect innerRect = new Rect(0, 0, 0, 0);

        private GUIStyle labelStyle;

        public void SetupWindow(int windowId, DebugConsole debugConsole)
        {
            debugConsole.RectsRecalculated += RectsRecalculated_Handler;

            this.messageLine = new Rect(4, 0, debugConsole.WindowRect.width - 36, 20);

            this.innerRect.width = messageLine.width;

            this.nameRect = messageLine;
            this.valueRect = messageLine;
        }

        public void DrawWindow(int windowId, DebugConsole debugConsole)
        {
            labelStyle = GUI.skin.label;

            this.innerRect.height = this.innerHeight < debugConsole.ScrollRect.height ? debugConsole.ScrollRect.height : this.innerHeight;

            this.scrollPosition = GUI.BeginScrollView(debugConsole.ScrollRect, this.scrollPosition, this.innerRect, false, true);

            this.nameRect.x = this.messageLine.x;
            this.nameRect.y = this.valueRect.y = 0;

            float totalWidth = this.messageLine.width - this.messageLine.x;

            float nameMin, nameMax, valueMin, valueMax, stepHeight;
            GUIStyle textAreaStyle = GUI.skin.textArea;

            foreach (var watchvar in debugConsole.WatchVars)
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

                this.innerHeight = this.valueRect.y > debugConsole.ScrollRect.height ? (int)this.valueRect.y : (int)debugConsole.ScrollRect.height;
            }

            GUI.EndScrollView();
        }

        private void RectsRecalculated_Handler(object sender, EventArgs e)
        {
            this.messageLine.Set(4, 0, ((DebugConsole)sender).WindowRect.width - 36, 20);
        }
    }
}
