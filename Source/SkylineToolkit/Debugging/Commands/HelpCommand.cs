using System;
using System.Collections.Generic;
using System.Linq;

namespace SkylineToolkit.Debugging.Commands
{
    internal class HelpCommand : DebugCommand
    {
        private IDictionary<string, IDebugCommand> commands;

        public HelpCommand(IDictionary<string, IDebugCommand> commands)
        {
            this.commands = commands;
        }

        public override int Execute(ICommandContext ctx, string[] args, char[] flags)
        {
            if (!this.VerifyArgsCount(args, 0, 1))
            {
                this.PrintUsage(ctx);
                return 1;
            }

            if (args.Length == 0)
            {
                IList<string> avaiCommands = this.commands.Keys.ToList();
                avaiCommands.Insert(0, "echo");
                avaiCommands.Insert(0, "clear");
                avaiCommands.Insert(0, "help");

                ctx.SendOutput(String.Join("\n", avaiCommands.ToArray()));
            }
            else if (args.Length == 1)
            {
                if (this.commands.ContainsKey(args[0].ToLower()))
                {
                    IDebugCommand cmd = this.commands[args[0].ToLower()];

                    ctx.SendOutput(String.Format("\tUsage:\t\t{0}\n{1}", cmd.GetUsage(), cmd.GetHelp()));
                    return 0;
                }

                ctx.SendOutput(String.Format("Cannot find any help information for debug command {0}.", args[0]));
                return 1;
            }

            return 0;
        }

        public override bool CanExecute(ICommandContext ctx)
        {
            return true;
        }

        public override string GetHelp()
        {
            return "Prints the availiable Commands or help to a specific command.";
        }

        public override string GetUsage()
        {
            return "help [<command-name>]";
        }
    }
}
