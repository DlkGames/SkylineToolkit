using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging
{
    public struct DebugConsoleLogMessage
    {
        private string _text;
        private string _formatted;

        private DebugConsoleMessageType _type;
        private Color _color;

        public Color Color
        {
            get {
                return this._color;
            }
        }

        public DebugConsoleLogMessage(object messageObject)
            : this(messageObject, DebugConsoleMessageType.Normal, DebugConsole.msgDefaultColor) { }

        public DebugConsoleLogMessage(object messageObject, Color displayColor)
            : this(messageObject, DebugConsoleMessageType.Normal, displayColor) { }

        public DebugConsoleLogMessage(object messageObject, DebugConsoleMessageType messageType)
            : this(messageObject, messageType, DebugConsole.msgDefaultColor)
        {
            switch (messageType)
            {
                case DebugConsoleMessageType.Error:
                case DebugConsoleMessageType.Exception:
                    this._color = DebugConsole.msgErrorColor;
                    break;
                case DebugConsoleMessageType.System:
                    this._color = DebugConsole.msgSystemColor;
                    break;
                case DebugConsoleMessageType.Warning:
                    this._color = DebugConsole.msgWarningColor;
                    break;
                case DebugConsoleMessageType.Output:
                    this._color = DebugConsole.msgOutputColor;
                    break;
                case DebugConsoleMessageType.Input:
                    this._color = DebugConsole.msgInputColor;
                    break;
            }
        }

        public DebugConsoleLogMessage(object messageObject, DebugConsoleMessageType messageType, Color displayColor)
        {
            this._text = messageObject == null ? "<null>" : messageObject.ToString();

            this._formatted = string.Empty;
            this._type = messageType;
            this._color = displayColor;
        }

        public override string ToString()
        {
            switch (this._type)
            {
                case DebugConsoleMessageType.Error:
                case DebugConsoleMessageType.Warning:
                    return string.Format("[{0}] {1}", this._type, this._text);
                default:
                    return ToGuiString();
            }
        }

        public string ToGuiString()
        {
            if (!string.IsNullOrEmpty(this._formatted))
            {
                return this._formatted;
            }

            switch (this._type)
            {
                case DebugConsoleMessageType.Input:
                    this._formatted = string.Format("=> {0}", this._text);
                    break;
                case DebugConsoleMessageType.Output:
                    var lines = this._text.Trim('\n').Split('\n');
                    var output = new StringBuilder();

                    foreach (var line in lines)
                    {
                        output.AppendFormat("= {0}\n", line);
                    }

                    this._formatted = output.ToString();
                    break;
                case DebugConsoleMessageType.System:
                    this._formatted = string.Format("# {0}", this._text);
                    break;
                case DebugConsoleMessageType.Warning:
                    this._formatted = string.Format("* {0}", this._text);
                    break;
                case DebugConsoleMessageType.Error:
                    this._formatted = string.Format("** {0}", this._text);
                    break;
                default:
                    this._formatted = this._text;
                    break;
            }

            return this._formatted;
        }
    }
}
