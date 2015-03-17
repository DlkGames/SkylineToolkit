using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Debugging.Commands
{
    internal class EvalCommand : DebugCommand
    {
        public override int Execute(ICommandContext ctx, string[] args, char[] flags)
        {
            int position = ctx.RawCommand.IndexOf(' ');

            if (position == -1)
            {
                ctx.SendOutput("No code specified. Type 'help eval' for more information.");

                return 1;
            }

            string code = ctx.RawCommand.Substring(position);

            ctx.SendOutput("Evaluating code...");

            object result = null;

            try
            {
                result = Debug.Evaluate(code);
            }
            catch (Exception ex)
            {
                ctx.SendOutput("Error while evaluating the code.");
                Log.Exception(ex);

                return 1;
            }

            if (result != null)
            {
                Log.Info(result.ToString());

                ctx.SendOutput("Output was sent to log");
            }
            else
            {
                ctx.SendOutput("<null>");
            }

            return 0;
        }

        public override bool CanExecute(ICommandContext ctx)
        {
            return true;
        }

        public override string GetHelp()
        {
            return "Evaluates the given code.";
        }

        public override string GetUsage()
        {
            return "eval <code>";
        }
    }
}
