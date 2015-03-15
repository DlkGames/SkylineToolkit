
namespace SkylineToolkit.Debugging.Commands
{
    public interface ICommandContext
    {
        bool VerifyArgsCount(int minArgs, int maxArgs);

        bool HasFlag(char flag);

        void SendOutput(string message);
    }
}
