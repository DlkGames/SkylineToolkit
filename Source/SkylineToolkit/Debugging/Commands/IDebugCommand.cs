
namespace SkylineToolkit.Debugging.Commands
{
    public interface IDebugCommand
    {
        int Execute(ICommandContext ctx, string[] args, char[] flags);

        bool CanExecute(ICommandContext ctx);

        string GetHelp();

        string GetUsage();
    }
}
