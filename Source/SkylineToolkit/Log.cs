using ColossalFramework;
using ColossalFramework.Plugins;
using SkylineToolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalMessageType = ColossalFramework.Plugins.PluginManager.MessageType;

namespace SkylineToolkit
{
    public static class Log
    {
        public delegate void LogMessageEventHandler(string message, MessageType type);

        public static event LogMessageEventHandler LogMessage;

        static Log()
        {
            var fileLog = new FileLogger(@"C:\Temp\SkylinesLog.txt");

            LogLevel = MessageType.Verbose;

            LogMessage += DebugPanel.LogMessage;
            LogMessage += fileLog.LogMessage;
            //LogMessage += ChirpPanelControl.LogMessage;

            //SubscribeToMessageManager();
            SubscribeToPluginManager();
        }

        public static MessageType LogLevel { get; set; }

        /// <summary>
        /// Note: MessageManager sends it's messages to chirper. So the log will receive all messages, which are shown by Chirper.
        /// </summary>
        internal static void SubscribeToMessageManager()
        {
            try
            {
                Singleton<MessageManager>.instance.m_newMessages += ColossalChirperMessage;
            }
            catch (Exception ex)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, ex.GetType().ToString() + " Message: " + ex.Message);
            }
        }

        /// <summary>
        /// Note: Log will receive error messages from the PluginManager.
        /// </summary>
        internal static void SubscribeToPluginManager()
        {
            try
            {
                PluginManager.eventLogMessage += ColossalPluginManagerMessage;
            }
            catch (Exception ex)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, ex.GetType().ToString() + " Message: " + ex.Message);
            }
        }

        private static void ColossalChirperMessage(ICities.IChirperMessage message)
        {
            Message("MessageManager: {0} {1}: {2}", message.senderID, message.senderName, message.text);
        }

        private static void ColossalPluginManagerMessage(ColossalMessageType type, string message)
        {
            Message("PluginManager: {0}", (MessageType)type, message);
        }

        public static void Message(string message, params object[] args)
        {
            Message(message, MessageType.Message, args);
        }

        public static void Message(string message, MessageType type = MessageType.Message, params object[] args)
        {
            OnLogMessage(String.Format(message, args), type);
        }

        public static void Critical(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Critical);
        }

        public static void Debug(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Debug);
        }

        public static void Error(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Error);
        }

        public static void Exception(Exception ex)
        {
            Message(ex.ToString(), MessageType.Exception);
        }

        public static void Info(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Info);
        }

        public static void Profile(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Profile);
        }

        public static void Verbose(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Verbose);
        }

        public static void Warning(string message, params object[] args)
        {
            Message(String.Format(message, args), MessageType.Warning);
        }

        private static void OnLogMessage(string message, MessageType type)
        {
            if (LogMessage != null)
            {
                if (type <= LogLevel)
                {
                    LogMessage(message, type);
                }
            }
        }
    }
}
