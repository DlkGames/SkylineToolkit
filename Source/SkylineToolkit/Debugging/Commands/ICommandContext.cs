
namespace SkylineToolkit.Debugging.Commands
{
    public interface ICommandContext
    {
        string[] Arguments { get; }

        char[] Flags { get; }

        string RawCommand { get; }

        bool VerifyArgsCount(int minArgs, int maxArgs);

        bool HasFlag(char flag);

        void SendOutput(string message);
    }
}
