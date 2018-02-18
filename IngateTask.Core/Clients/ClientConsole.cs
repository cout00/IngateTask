using System;
using System.IO;
using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.ParallelThread;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Core.CommandInterpreter.CommandsList;
using IngateTask.Core.Loggers;

namespace IngateTask.Core.Clients
{
    public class ClientConsole : User
    {
        public ClientConsole(string name, ILogProvider userLogProvider) : base(name, userLogProvider)
        {
            Interpreter = new Interpreter(userLogProvider);
        }

        public Interpreter Interpreter { get; set; }
        public ParallelQueue<Crawler.Crawler> _parallelQueue;

        public virtual void InitInterpreter()
        {
            CommandReadInputFile commandReadInputFile = new CommandReadInputFile(UserLogProvider, this);

            CommandSetThreads commandSetThreads = new CommandSetThreads(UserLogProvider, this);

            CommandHelp commandHelp = new CommandHelp(UserLogProvider, this);

            CommandOutPutPath commandOutPutPath = new CommandOutPutPath(UserLogProvider, this);
            
            CommandStartCrawl commandStartCrawl = new CommandStartCrawl(UserLogProvider, this);

            Interpreter.Add(commandStartCrawl);
            Interpreter.Add(commandOutPutPath);
            Interpreter.Add(commandHelp);
            Interpreter.Add(commandSetThreads);
            Interpreter.Add(commandReadInputFile);
        }
    }
}