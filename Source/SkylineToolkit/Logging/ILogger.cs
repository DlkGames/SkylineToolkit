using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Logging
{
    public interface ILogger
    {
        void LogMessage(string module, string message, MessageType type);
    }
}
