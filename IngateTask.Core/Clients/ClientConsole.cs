using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.CommandInterpreter.CommandsList;
using IngateTask.Core.ParallelThread;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Clients
{
    /// <summary>
    /// консольный клиент к нему добавился интерпретатор и ссылка на очередь задач
    /// </summary>
    public class ClientConsole : User
    {
        public ParallelQueue<Crawler.Crawler> _parallelQueue;

        public ClientConsole(string name, ILogProvider userLogProvider) : base(name, userLogProvider)
        {
            Interpreter = new Interpreter(userLogProvider);
        }

        public Interpreter Interpreter { get; set; }
        /// <summary>
        /// инициализирует интерпретатор командами которые доступны пользователю
        /// </summary>
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