using System;
using System.Linq;

namespace SkylineToolkit.Debugging.Commands
{
    internal abstract class DebugCommand : IDebugCommand
    {
        public abstract int Execute(ICommandContext ctx, string[] args, char[] flags);

        public abstract bool CanExecute(ICommandContext ctx);

        public abstract string GetHelp();

        public abstract string GetUsage();

        public virtual void PrintUsage(ICommandContext ctx)
        {
            ctx.SendOutput(String.Format("\tUsage:\t\t{0}", this.GetUsage()));
        }

        protected bool VerifyArgsCount(string[] args, int minArgs, int maxArgs)
        {
            if (args.Length < minArgs | args.Length > maxArgs)
            {
                return false;
            }

            return true;
        }

        protected bool HasFlag(char[] flags, char flag)
        {
            return flags.Any(f => f.Equals(flag));
        }
    }
}
