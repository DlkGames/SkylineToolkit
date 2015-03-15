using System;

namespace SkylineToolkit.Debugging.Commands
{
    internal class DelegateCommand : DebugCommand
    {
        public delegate int ExecuteMethod(ICommandContext ctx, string[] args, char[] flags);

        private ExecuteMethod execMethod;

        private bool canExecute = false;

        public string HelpText
        {
            get;
            set;
        }

        public string UsageText
        {
            get;
            set;
        }

        public DelegateCommand(Func<ICommandContext, string[], char[], int> executeMethod, bool canExecute)
        {
            this.execMethod = new ExecuteMethod(executeMethod);
            this.canExecute = canExecute;
        }

        public override int Execute(ICommandContext ctx, string[] args, char[] flags)
        {
            if (this.execMethod != null)
            {
                return this.execMethod(ctx, args, flags);
            }

            return 0;
        }

        public override bool CanExecute(ICommandContext ctx)
        {
            return this.canExecute;
        }

        public override string GetHelp()
        {
            if (this.HelpText != null)
            {
                return this.HelpText;
            }

            return "Executes some automatically from code generated actions.";
        }

        public override string GetUsage()
        {
            if (this.UsageText != null)
            {
                return this.UsageText;
            }

            return String.Empty;
        }
    }
}
