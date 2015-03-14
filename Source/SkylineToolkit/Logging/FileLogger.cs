using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Logging
{
    public class FileLogger : ILogger
    {
        public string LogFile { get; set; }

        public FileLogger(string file)
        {
            FileInfo info = new FileInfo(file);

            if (!info.Directory.Exists)
            {
                Directory.CreateDirectory(info.Directory.FullName);
            }

            this.LogFile = file;
        }

        public void LogMessage(string message, MessageType type)
        {
            if (String.IsNullOrEmpty(LogFile))
            {
                return;
            }

            using (FileStream stream = File.Open(LogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine(GetFormattedMessage(message, type));

                writer.Flush();
            }
        }

        private static string GetFormattedMessage(string message, MessageType type)
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
                    return String.Format("{0} [{1}] {2}", timestamp, type, message);
                default:
                    return message;
            }
        }
    }
}
