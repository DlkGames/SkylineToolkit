using ColossalFramework;
using ColossalFramework.Plugins;
using SkylineToolkit.Logging;
using System;
using ColossalMessageType = ColossalFramework.Plugins.PluginManager.MessageType;

namespace SkylineToolkit
{
    public static class Log
    {
        public delegate void LogMessageEventHandler(string module, string message, MessageType type);

        public static event LogMessageEventHandler LogMessage;

        static Log()
        {
            var fileLog = new FileLogger(@"C:\Temp\SkylinesLog.txt");

            LogLevel = MessageType.Verbose;

            LogMessage += DebugPanel.LogMessage;
            LogMessage += fileLog.LogMessage;
            LogMessage += Debugging.DebugConsole.Instance.LogMessage;
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

        #region Without module

        public static void Message(string message, params object[] args)
        {
            Message(message, MessageType.Message, args);
        }

        public static void Message(string message, MessageType type = MessageType.Message, params object[] args)
        {
            OnLogMessage("Global", String.Format(message, args), type);
        }

        public static void Critical(string message, params object[] args)
        {
            Message(message, MessageType.Critical, args);
        }

        public static void Debug(string message, params object[] args)
        {
            Message(message, MessageType.Debug, args);
        }

        public static void Error(string message, params object[] args)
        {
            Message(message, MessageType.Error, args);
        }

        public static void Exception(Exception ex)
        {
            Message(ex.ToString(), MessageType.Exception);
        }

        public static void Info(string message, params object[] args)
        {
            Message(message, MessageType.Info, args);
        }

        public static void Profile(string message, params object[] args)
        {
            Message(message, MessageType.Profile, args);
        }

        public static void Verbose(string message, params object[] args)
        {
            Message(message, MessageType.Verbose, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Message(message, MessageType.Warning, args);
        }

        #endregion

        #region With module

        public static void Message(string module, string message, params object[] args)
        {
            Message(message, MessageType.Message, args);
        }

        public static void Message(string module, string message, MessageType type = MessageType.Message, params object[] args)
        {
            OnLogMessage(module, String.Format(message, args), type);
        }

        public static void Critical(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Critical, args);
        }

        public static void Debug(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Debug, args);
        }

        public static void Error(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Error, args);
        }

        public static void Exception(string module, Exception ex)
        {
            Message(ex.ToString(), MessageType.Exception);
        }

        public static void Info(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Info, args);
        }

        public static void Profile(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Profile, args);
        }

        public static void Verbose(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Verbose, args);
        }

        public static void Warning(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Warning, args);
        }

        #endregion

        private static void OnLogMessage(string module, string message, MessageType type)
        {
            if (LogMessage != null)
            {
                if (type <= LogLevel)
                {
                    LogMessage(module, message, type);
                }
            }
        }
    }
}
