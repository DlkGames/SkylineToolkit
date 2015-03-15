using SkylineToolkit.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalMessageType = ColossalFramework.Plugins.PluginManager.MessageType;

namespace SkylineToolkit
{
    public static class MessageTypeExtensions
    {
        public static ColossalMessageType ToColossalType(this MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                case MessageType.Critical:
                case MessageType.Exception:
                    return ColossalMessageType.Error;
                case MessageType.Warning:
                    return ColossalMessageType.Warning;
                case MessageType.Message:
                case MessageType.Info:
                case MessageType.Verbose:
                case MessageType.Debug:
                case MessageType.Profile:
                default:
                    return ColossalMessageType.Message;
            }
        }

        public static DebugConsoleMessageType ToDebugConsoleType(this MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                case MessageType.Critical:
                    return DebugConsoleMessageType.Error;
                case MessageType.Exception: 
                    return DebugConsoleMessageType.Exception;
                case MessageType.Warning:
                    return DebugConsoleMessageType.Warning;
                case MessageType.Message:
                case MessageType.Info:
                case MessageType.Verbose:
                case MessageType.Debug:
                case MessageType.Profile:
                default:
                    return DebugConsoleMessageType.Normal;
            }
        }
    }
}
