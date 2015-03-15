using System.Collections.Generic;
using System.Linq;

namespace SkylineToolkit.Debugging.Commands
{
    internal class CommandContext : ICommandContext
    {
        private IDebugCommand command;
        private IEnumerable<string> commandArgs;
        private IEnumerable<char> commandFlags;

        public CommandContext(IDebugCommand command, string[] args, char[] flags)
        {
            this.command = command;
            this.commandArgs = args;
            this.commandFlags = flags;
        }

        public bool VerifyArgsCount(int minArgs, int maxArgs)
        {
            if (this.commandArgs.Count() < minArgs | this.commandArgs.Count() > maxArgs)
            {
                return false;
            }

            return true;
        }

        public bool HasFlag(char flag)
        {
            return this.commandFlags.Any(f => f.Equals(flag));
        }

        public void SendOutput(string message)
        {
            DebugConsole.Log(message, DebugConsoleMessageType.Output);
        }
    }
}
