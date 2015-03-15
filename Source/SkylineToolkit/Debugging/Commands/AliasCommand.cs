using System;
using System.Collections.Generic;
using System.Linq;

namespace SkylineToolkit.Debugging.Commands
{
    internal class AliasCommand : DebugCommand
    {
        private IList<string> aliasCommands = new List<string>();

        public override int Execute(ICommandContext ctx, string[] args, char[] flags)
        {
            if (!this.VerifyArgsCount(args, 2, Int16.MaxValue))
            {
                this.PrintUsage(ctx);
                return 1;
            }

            IList<string> argsList = args.ToList();

            string alias = argsList[0];
            argsList.RemoveAt(0);
            
            if(argsList[0].ToLower().Equals("alias")) {
                throw new ArgumentException("Cannot register an alias for the alias command.", "args");
            }

            if (this.aliasCommands.Contains(argsList[0].ToLower()))
            {
                throw new ArgumentException("Cannot register an alias for another alias command.", "args");
            }

            if (argsList[0].ToLower().Equals("0"))
            {
                DebugConsole.UnregisterCommand(alias);
                this.aliasCommands.Remove(alias);

                ctx.SendOutput(String.Format("Removed alias {0}", alias));

                return 0;
            }

            string target = String.Join(" ", argsList.ToArray());

            if (flags.Length > 0)
            {
                target += " -" + String.Join(" -", flags.Select(f => f.ToString()).ToArray<string>());
            }

            DelegateCommand delCmd = new DelegateCommand((dCtx, dArgs, dFlags) =>
            {
                target += " " + String.Join(" ", dArgs);

                if (dFlags.Length > 0)
                {
                    target += " -" + String.Join(" -", dFlags.Select(f => f.ToString()).ToArray<string>());
                }

                dCtx.SendOutput(String.Format("Alias {0} executes {1}", alias, target));
                DebugConsole.Execute(target);

                return 0;
            }, true);

            delCmd.UsageText = String.Format("{0} [<AdditionalArg1> <AdditionalArg2> ...]", alias);
            delCmd.HelpText = String.Format("This is an alias command for {0}\n" 
                + "You can use this alias to execute the aliased command with default or if required with more arguments.", target);

            DebugConsole.RegisterCommand(alias, delCmd);

            if (!this.aliasCommands.Contains(alias))
            {
                this.aliasCommands.Add(alias);
            }

            ctx.SendOutput(String.Format("Registered command alias {0} for command {1}", alias, target));

            return 0;
        }

        public override bool CanExecute(ICommandContext ctx)
        {
            return true;
        }

        public override string GetHelp()
        {
            return "Registers an alias for another command (with arguments). Set the target command to 0 in order to remove an existing alias.";
        }

        public override string GetUsage()
        {
            return "alias <alias> <targetCommand> [<targetCommandArg1> <targetCommandArg2> ...]";
        }
    }
}
