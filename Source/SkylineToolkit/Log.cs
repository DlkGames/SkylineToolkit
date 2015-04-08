using ColossalFramework;
using ColossalFramework.Plugins;
using SkylineToolkit.Debugging;
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
            HookToUnityDebug();
        }

        public static MessageType LogLevel { get; set; }

        /// <summary>
        /// Note: MessageManager sends it's messages to chirper. So the log will receive all messages, which are shown by Chirper.
        /// </summary>
        private static void SubscribeToMessageManager()
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
        private static void SubscribeToPluginManager()
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

        private static void HookToUnityDebug()
        {
            try
            {
                UnityDebug.Enable();
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

        #region Logging

        #region Without module

        #region Format

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
            if (LogLevel < MessageType.Verbose)
            {
                return;
            }

            Message(message, MessageType.Verbose, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Message(message, MessageType.Warning, args);
        }

        #endregion

        #region Without format

        public static void Message(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Message);
        }

        public static void Message(object obj, MessageType type = MessageType.Message)
        {
            OnLogMessage("Global", obj == null ? "<null>" : obj.ToString(), type);
        }

        public static void Critical(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Critical);
        }

        public static void Debug(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Debug);
        }

        public static void Error(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Error);
        }

        public static void Exception(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            Message(ex.ToString(), MessageType.Exception);
        }

        public static void Info(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Info);
        }

        public static void Profile(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Profile);
        }

        public static void Verbose(object obj)
        {
            if (LogLevel < MessageType.Verbose)
            {
                return;
            }

            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Verbose);
        }

        public static void Warning(object obj)
        {
            Message(obj == null ? "<null>" : obj.ToString(), MessageType.Warning);
        }

        #endregion

        #endregion

        #region With module

        #region Format

        public static void Message(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Message, args);
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
            if (LogLevel < MessageType.Verbose)
            {
                return;
            }

            Message(module, message, MessageType.Verbose, args);
        }

        public static void Warning(string module, string message, params object[] args)
        {
            Message(module, message, MessageType.Warning, args);
        }

        #endregion

        #region Without format

        public static void Message(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Message);
        }

        public static void Message(string module, object obj, MessageType type = MessageType.Message)
        {
            OnLogMessage(module, obj == null ? "<null>" : obj.ToString(), type);
        }

        public static void Critical(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Critical);
        }

        public static void Debug(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Debug);
        }

        public static void Error(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Error);
        }

        public static void Exception(string module, Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            Message(ex.ToString(), MessageType.Exception);
        }

        public static void Info(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Info);
        }

        public static void Profile(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Profile);
        }

        public static void Verbose(string module, object obj)
        {
            if (LogLevel < MessageType.Verbose)
            {
                return;
            }

            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Verbose);
        }

        public static void Warning(string module, object obj)
        {
            Message(module, obj == null ? "<null>" : obj.ToString(), MessageType.Warning);
        }

        #endregion

        #endregion

        #endregion

        private static void OnLogMessage(string module, string message, MessageType type)
        {
            if (message == null)
            {
                return;
            }

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
