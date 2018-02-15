using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Parsers;

namespace IngateTask.Core.CommandInterpreter
{
    public class CommandHelp
    {
    }


    public class CommandReadInputFile :Command
    {
        public CommandReadInputFile(ILogProvider logProvider, Client clientLink) : base(logProvider, clientLink)
        {
        }

        public override string CommandName
        {
            get {
                return "-load_file";
            }
        }

        public override bool CommandAction()
        {
            InputLocalFileParser fileParser = new InputLocalFileParser(Parameter, _logProvider);
            var array = fileParser.GetParsedArray();
            _clientLink.InputFieldses = array;
            if (array==null)
            {
                return true;
            }
            if (!fileParser.FileIsValid)
            {
                return false;
            }
            return true;
        }
    }

    public class CommandSetThreads
    {

    }

    public class CommandOutPutPath
    {

    }


}
