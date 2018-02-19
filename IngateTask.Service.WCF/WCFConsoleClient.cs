using IngateTask.Core.Clients;
using IngateTask.Core.CommandInterpreter.CommandsList;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Service.WCF.Commands;

namespace IngateTask.Service.WCF
{
    /// <summary>
    /// объект клиента на стороне сервиса, уже со своей реализацией
    /// </summary>
    public class WCFConsoleClient : ClientConsole
    {
        public ICrawlerCallBack _crawlerCallBack;

        public WCFConsoleClient(string name, ILogProvider userLogProvider,
            ICrawlerCallBack crawlerCallBack) : base(name, userLogProvider)
        {
            _crawlerCallBack = crawlerCallBack;
        }

        public override void InitInterpreter()
        {
            base.InitInterpreter();
            Interpreter.Remove(Interpreter.Find(
                command => command.GetType() == typeof(CommandStartCrawl)));

            Interpreter.Remove(Interpreter.Find(
                command => command.GetType() == typeof(CommandReadInputFile)));


            CommandReadInputFileToServer commandReadInputFile = new CommandReadInputFileToServer(UserLogProvider, this);
            CommandStartCrawlAtServer commandStartCrawlAtServer = new CommandStartCrawlAtServer(UserLogProvider, this);
            CommandRunnedInfo commandRunnedInfo = new CommandRunnedInfo(UserLogProvider, this);
            CommandAbortCrawl commandAbortCrawl = new CommandAbortCrawl(UserLogProvider, this);

            Interpreter.Add(commandStartCrawlAtServer);
            Interpreter.Add(commandRunnedInfo);
            Interpreter.Add(commandReadInputFile);
            Interpreter.Add(commandAbortCrawl);
        }
    }
}