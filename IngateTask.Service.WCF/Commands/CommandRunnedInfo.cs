using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.ParallelThread;
using IngateTask.Core.CommandInterpreter;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Service.WCF.Commands
{
    [Parametreless]
    class CommandRunnedInfo :Command
    {
        public CommandRunnedInfo(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider, clientConsoleLink)
        {
        }

        public override string CommandName
        {
            get { return "-get_info"; }
        }

        public override string CommandDiscription
        {
            get { return "get info about runned tasks"; }
        }

        public override async Task<bool> CommandAction()
        {
            _logProvider.SendNonStatusMessage($"runned {ClientConsoleLink._parallelQueue.GetRunningCount().ToString()} threads");
            return true;
        }

        public override bool PropertySetter(object obj)
        {
            return true;
        }

        public override bool InvokeRequarement()
        {
            return ClientConsoleLink._parallelQueue != null;
        }

        public override bool ResumeRequarement()
        {
            return true;
        }

        public override string OnFailFunc()
        {
            return "process not started";
        }
    }
}
