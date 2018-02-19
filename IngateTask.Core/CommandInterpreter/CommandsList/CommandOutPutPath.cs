using System;
using System.IO;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter.CommandsList
{
    public class CommandOutPutPath : Command
    {
        public CommandOutPutPath(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription
        {
            get { return "set output directory"; }
        }

        public override string CommandName
        {
            get { return "-out_dir"; }
        }

        public override async Task<bool> CommandAction()
        {
            ClientConsoleLink.OutPutPath = Parameter;
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
            try
            {
                string newPath = (string) obj;
                if (!newPath.EndsWith("\\"))
                {
                    newPath += "\\";
                }
                ClientConsoleLink.OutPutPath = Path.GetDirectoryName(Path.GetFullPath(newPath));
                _logProvider.SendNonStatusMessage("u entered: " + ClientConsoleLink.OutPutPath);
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