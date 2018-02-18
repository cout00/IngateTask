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

namespace IngateTask.Service.WCF
{
    public class WCFConsoleClient :ClientConsole
    {
        public ICrawlerCallBack _crawlerCallBack;

        public WCFConsoleClient(string name, ILogProvider logProvider, ICrawlerCallBack crawlerCallBack) : base(name, logProvider)
        {
            _crawlerCallBack = crawlerCallBack;
        }

        public override void InitInterpreter()
        {
            base.InitInterpreter();
            //Interpreter.Remove(Interpreter.Find(
            //    command =>command.GetType()==typeof(CommandStartCrawl)));
            
            Interpreter.Remove(Interpreter.Find(
                command => command.GetType() == typeof(CommandReadInputFile)));

            

            CommandReadInputFileToServer commandReadInputFile =new CommandReadInputFileToServer(_logProvider, this);
                        
            CommandRunnedInfo commandRunnedInfo=new CommandRunnedInfo(_logProvider,this);

            Interpreter.Add(commandRunnedInfo);
            Interpreter.Add(commandReadInputFile);
        }
    }
}
