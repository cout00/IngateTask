using System;
using System.IO;
using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.ParallelThread;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Core.CommandInterpreter.CommandsList;

namespace IngateTask.Core.Clients
{
    public class ClientConsole : User
    {
        public ClientConsole(string name, ILogProvider logProvider) : base(name, logProvider)
        {
            Interpreter = new Interpreter(logProvider);
        }

        public Interpreter Interpreter { get; set; }
        public ParallelQueue _parallelQueue;

        public virtual void InitInterpreter()
        {
            CommandReadInputFile commandReadInputFile = new CommandReadInputFile(_logProvider, this);

            CommandSetThreads commandSetThreads = new CommandSetThreads(_logProvider, this);

            CommandHelp commandHelp = new CommandHelp(_logProvider, this);

            CommandOutPutPath commandOutPutPath = new CommandOutPutPath(_logProvider, this);
                        
            CommandStartCrawl commandStartCrawl = new CommandStartCrawl(_logProvider, this);

            Interpreter.Add(commandStartCrawl);
            Interpreter.Add(commandOutPutPath);
            Interpreter.Add(commandHelp);
            Interpreter.Add(commandSetThreads);
            Interpreter.Add(commandReadInputFile);
        }
    }
}