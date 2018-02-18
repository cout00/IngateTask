using System;
using System.IO;
using IngateTask.Core.Clients;
using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.CommandInterpreter.CommandsList;
using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Service.WCF.Logger;

namespace IngateTask.Service.WCF.Commands
{
    [Parametreless]
    public class CommandStartCrawlAtServer :CommandStartCrawl
    {
        public CommandStartCrawlAtServer(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider, clientConsoleLink)
        {
        }

        public override bool PropertySetter(object obj)
        {
            if (ClientConsoleLink.OutPutPath.Length != 0)
            {
                try
                {
                    Directory.CreateDirectory(ClientConsoleLink.OutPutPath);
                    ConsoleWCFDispatcher consoleWcfDispatcher = new ConsoleWCFDispatcher(ClientConsoleLink.OutPutPath, ((WCFConsoleClient)ClientConsoleLink)._crawlerCallBack);
                    _crawlLogProvider = consoleWcfDispatcher;
                }
                catch (Exception e)
                {
                    _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                    return false;
                }
                return true;
            }
            return true;
        }


    }
}