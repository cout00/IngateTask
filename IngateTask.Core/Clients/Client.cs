using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Parsers;

namespace IngateTask.Core.Clients
{
    public class Client
    {
        private readonly string _name;
        private readonly ILogProvider _logProvider;
        internal string InputFilePath { get; set; } = "";
        internal string OutPutPath { get; set; } = "";
        internal string ThreadNumber { get; set; } = "";
        public Interpreter Interpreter { get; set; }
        public List<InputFields> InputFieldses { get; set; }
       
        public Client(string name, ILogProvider logProvider)
        {
            _name = name;
            _logProvider = logProvider;
            Interpreter=new Interpreter(logProvider);
        }

        public virtual void InitInterpreter()
        {
            CommandReadInputFile commandReadInputFile=new CommandReadInputFile(_logProvider,this);
            commandReadInputFile.InvokeRequarement = () => { return InputFilePath.Length != 0; };
            commandReadInputFile.OnFailFunc = () => { return "need file name"; };
            commandReadInputFile.ResumeRequarement = () =>
            {
                _logProvider.SendNonStatusMessage("file have mistakes. Resume Y(yes) N(no)");
                var text = Console.ReadLine();
                if (text== "Y".ToLower())
                {
                    return true;
                }
                if (text == "N".ToLower())
                {
                    return false;
                }
                _logProvider.SendNonStatusMessage($"Unexpected symbol {text}");
                return false;
            };
            Interpreter.Add(commandReadInputFile);
        }



    }
}
