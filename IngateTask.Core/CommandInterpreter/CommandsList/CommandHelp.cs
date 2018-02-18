using System;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter.CommandsList
{
    [Parametreless]
    public class CommandHelp :Command
    {
        public CommandHelp(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription
        {
            get { return "show help"; }
        }

        public override string CommandName
        {
            get { return "-help"; }
        }

        public async override Task<bool> CommandAction()
        {
            string outString = Environment.NewLine;
            foreach (Command command in ClientConsoleLink.Interpreter)
            {
                outString += $" {command.CommandName}: {command.CommandDiscription}{Environment.NewLine}";
            }
            _logProvider.SendNonStatusMessage(outString.TrimEnd('\r', '\n'));
            return true;
        }

        public override bool InvokeRequarement()
        {
            return true;
        }

        public override string OnFailFunc()
        {
            return "";
        }

        public override bool PropertySetter(object obj)
        {
            return true;
        }

        public override bool ResumeRequarement()
        {
            return true;
        }
    }
}