using System.Collections.Generic;
using System.Linq;

namespace SkylineToolkit.Debugging.Commands
{
    internal class CommandContext : ICommandContext
    {
        private string rawCommand;
        private IDebugCommand command;
        private string[] commandArgs;
        private char[] commandFlags;

        public CommandContext(string rawCommand, IDebugCommand command, string[] args, char[] flags)
        {
            this.rawCommand = rawCommand;
            this.command = command;
            this.commandArgs = args;
            this.commandFlags = flags;
        }

        public string[] Arguments
        {
            get
            {
                return this.commandArgs.ToArray();
            }
        }

        public char[] Flags
        {
            get
            {
                return this.commandFlags.ToArray();
            }
        }

        public string RawCommand
        {
            get
            {
                return this.rawCommand;
            }
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
