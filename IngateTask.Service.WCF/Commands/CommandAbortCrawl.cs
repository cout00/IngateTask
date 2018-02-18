using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.CommandInterpreter;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Service.WCF.Commands
{
    public class CommandAbortCrawl :Command
    {
        public CommandAbortCrawl(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider, clientConsoleLink)
        {
        }

        public override string CommandDiscription
        {
            get { return "abort selected task. use -get_info to learn about task keys"; }
        }

        public override string CommandName
        {
            get { return "-abort"; }
        }

        public async override Task<bool> CommandAction()
        {
            if (ClientConsoleLink._parallelQueue.GetRunnedTasksName().Contains(Parameter))
            {
                _logProvider.SendStatusMessage(LogMessages.Warning, $"Please wait! this may take some time");
                ClientConsoleLink._parallelQueue.CanselTask(Parameter);
                return true;
            }
            _logProvider.SendStatusMessage(LogMessages.Warning, $"not task with key {Parameter}");
            return false;

        }

        public override bool InvokeRequarement()
        {
            return ClientConsoleLink._parallelQueue!=null&&ClientConsoleLink._parallelQueue.GetRunningCount() != 0;
        }

        public override string OnFailFunc()
        {
            return "no one runned threads";
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
