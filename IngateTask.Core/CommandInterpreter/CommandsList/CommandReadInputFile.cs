using System.Collections.Generic;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter.CommandsList
{
    public class CommandReadInputFile : Command
    {
        public CommandReadInputFile(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription
        {
            get { return "Load File to the system"; }
        }

        public override string CommandName
        {
            get { return "-load_file"; }
        }

        public override async Task<bool> CommandAction()
        {
            InputLocalFileParser fileParser = new InputLocalFileParser(Parameter, _logProvider);
            List<InputFields> array = fileParser.GetParsedArray();
            ClientConsoleLink.InputFieldses = array;
            if (array == null)
            {
                return false;
            }
            if (!fileParser.FileIsValid)
            {
                return false;
            }
            return true;
        }

        public override bool InvokeRequarement()
        {
            return ClientConsoleLink.InputFilePath.Length != 0 && ClientConsoleLink.ThreadNumber != 0;
        }

        public override string OnFailFunc()
        {
            return "need file name";
        }

        public override bool PropertySetter(object obj)
        {
            ClientConsoleLink.InputFilePath = (string) obj;
            return true;
        }

        public override bool ResumeRequarement()
        {
            _logProvider.SendNonStatusMessage("file have mistakes. some lines will be skipped");
            return true;
        }
    }
}