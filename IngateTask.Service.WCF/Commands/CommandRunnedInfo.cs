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
                       
            StringBuilder infoBuilder=new StringBuilder();
            infoBuilder.AppendLine(Environment.NewLine);
            infoBuilder.AppendLine("________INFO_______");
            infoBuilder.AppendLine($"runned {ClientConsoleLink._parallelQueue.GetRunningCount().ToString()} threads");
            infoBuilder.AppendLine("task names:");
            infoBuilder.AppendLine(string.Join(Environment.NewLine, ClientConsoleLink._parallelQueue.GetRunnedTasksName().
                    Select(pair => $"Key: {pair.Key} Readed mbytes: {Math.Round(pair.Value.ReadedMbytes)} Parsed: {pair.Value.SavedPages}").ToList()
                ));
            infoBuilder.AppendLine("___________________");
            _logProvider.SendNonStatusMessage(infoBuilder.ToString());
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
