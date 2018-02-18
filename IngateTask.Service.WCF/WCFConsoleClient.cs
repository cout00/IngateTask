using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.CommandInterpreter;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Core.Clients;
using IngateTask.Service.WCF.Commands;
using IngateTask.Core.CommandInterpreter.CommandsList;

namespace IngateTask.Service.WCF
{
    public class WCFConsoleClient :ClientConsole
    {
        public ICrawlerCallBack _crawlerCallBack;

        public WCFConsoleClient(string name, ILogProvider userLogProvider, ICrawlerCallBack crawlerCallBack) : base(name, userLogProvider)
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

            

            CommandReadInputFileToServer commandReadInputFile =new CommandReadInputFileToServer(UserLogProvider, this);
            CommandStartCrawlAtServer commandStartCrawlAtServer=new CommandStartCrawlAtServer(UserLogProvider,this);            
            CommandRunnedInfo commandRunnedInfo=new CommandRunnedInfo(UserLogProvider,this);
            CommandAbortCrawl commandAbortCrawl=new CommandAbortCrawl(UserLogProvider,this);

            Interpreter.Add(commandStartCrawlAtServer);
            Interpreter.Add(commandRunnedInfo);
            Interpreter.Add(commandReadInputFile);
            Interpreter.Add(commandAbortCrawl);
        }
    }
}
