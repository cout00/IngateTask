using System;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter.CommandsList
{
    public class CommandSetThreads : Command
    {
        public CommandSetThreads(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription
        {
            get { return "set a number of execution thread"; }
        }

        public override string CommandName
        {
            get { return "-set_threads"; }
        }

        public override async Task<bool> CommandAction()
        {
            ClientConsoleLink.ThreadNumber = int.Parse(Parameter);
            return true;
        }

        public override bool InvokeRequarement()
        {
            return ClientConsoleLink.ThreadNumber != 0;
        }

        public override string OnFailFunc()
        {
            return "set threads number";
        }

        public override bool PropertySetter(object obj)
        {
            try
            {
                ClientConsoleLink.ThreadNumber = int.Parse((string) obj);
            }
            catch (Exception e)
            {
                _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                return false;
            }
            return true;
        }

        public override bool ResumeRequarement()
        {
            return true;
        }
    }
}