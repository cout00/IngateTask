using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.CommandInterpreter;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Service.WCF.Commands
{
    class CommandReadInputFileToServer :CommandReadInputFile
    {
        public CommandReadInputFileToServer(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider, clientConsoleLink)
        {

        }

        public override bool PropertySetter(object obj)
        {
            ClientConsoleLink.InputFilePath = (string)obj;
            ((WCFConsoleClient)ClientConsoleLink)._crawlerCallBack.GetClientData(true, (string)obj);
            return true;
        }

        public async override Task<bool> CommandAction()
        {
            return true;
        }
    }
}
