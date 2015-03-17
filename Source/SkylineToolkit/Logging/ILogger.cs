
namespace SkylineToolkit.Logging
{
    public interface ILogger
    {
        void LogMessage(string module, string message, MessageType type);
    }
}
