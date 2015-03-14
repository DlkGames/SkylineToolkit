using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework.UI;
using ColossalFramework.Plugins;
using ColossalFramework;

using ColossalMessageType = ColossalFramework.Plugins.PluginManager.MessageType;

namespace SkylineToolkit
{
    public static class DebugPanel
    {
        private static DebugOutputPanel panel;

        public static DebugOutputPanel Panel
        {
            get
            {
                if (panel == null && UIView.library != null)
                {
                    DebugOutputPanel foundPanel = UIView.library.Get<DebugOutputPanel>("DebugOutputPanel");

                    if (foundPanel != null)
                    {
                        panel = (DebugOutputPanel)foundPanel;
                    }
                }

                return panel;
            }
        }

        public static void LogMessage(string message, MessageType type = MessageType.Message)
        {
            ColossalMessageType colossalType = type.ToColossalType();

            string formattedMessage = DebugPanel.GetFormattedMessage(message, type);

            try
            {
                DebugOutputPanel.AddMessage(colossalType, formattedMessage);
            }
            catch (Exception ex)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, ex.GetType().ToString() + " Message: " + ex.Message);
            }
        }

        public static void Show()
        {
            try
            {
                DebugOutputPanel.Show();
            }
            catch (Exception ex)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, ex.GetType().ToString() + " Message: " + ex.Message);
            }
        }

        public static void Hide()
        {
            try
            {
                DebugOutputPanel.Hide();
            }
            catch (Exception ex)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, ex.GetType().ToString() + " Message: " + ex.Message);
            }
        }

        private static string GetFormattedMessage(string message, MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return String.Format("[Error] {0}");
                case MessageType.Warning:
                    return String.Format("[Warning] {0}");
                case MessageType.Message:
                    return String.Format("[Message] {0}");
                case MessageType.Info:
                    return String.Format("[Info] {0}");
                case MessageType.Verbose:
                    return String.Format("[Verbose] {0}");
                case MessageType.Debug:
                    return String.Format("[Debug] {0}");
                case MessageType.Profile:
                    return String.Format("[Profile] {0}");
                case MessageType.Critical:
                    return String.Format("[Critical] {0}");
                case MessageType.Exception:
                    return String.Format("[Exception] {0}");
                default:
                    return message;
            }
        }
    }
}
