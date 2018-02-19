using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.CommandInterpreter.CommandsList;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Service.WCF.Commands
{
    internal class CommandReadInputFileToServer : CommandReadInputFile
    {
        public CommandReadInputFileToServer(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(
            logProvider, clientConsoleLink)
        {
        }

        public override bool PropertySetter(object obj)
        {
            ClientConsoleLink.InputFilePath = (string) obj;
            ((WCFConsoleClient) ClientConsoleLink)._crawlerCallBack.GetClientData(true, (string) obj);
            return true;
        }

        public override async Task<bool> CommandAction()
        {
            return true;
        }
    }
}