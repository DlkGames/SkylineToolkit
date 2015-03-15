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

        public static void LogMessage(string module, string message, MessageType type = MessageType.Message)
        {
            ColossalMessageType colossalType = type.ToColossalType();

            string formattedMessage = DebugPanel.GetFormattedMessage(module, message, type);

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

        private static string GetFormattedMessage(string module, string message, MessageType type)
        {
            DateTime timestamp = DateTime.Now;

            switch (type)
            {
                case MessageType.Error:
                case MessageType.Warning:
                case MessageType.Message:
                case MessageType.Info:
                case MessageType.Verbose:
                case MessageType.Debug:
                case MessageType.Profile:
                case MessageType.Critical:
                case MessageType.Exception:
                    return String.Format("{0} [{1}] [{2}] {3}", timestamp, type, module, message);
                default:
                    return message;
            }
        }
    }
}
