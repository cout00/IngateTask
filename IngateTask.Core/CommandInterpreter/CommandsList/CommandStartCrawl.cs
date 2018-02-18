using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.Loggers;
using IngateTask.Core.ParallelThread;
using IngateTask.Core.Parsers;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter.CommandsList
{
    [Parametreless]
    public class CommandStartCrawl :Command
    {

        public CommandStartCrawl(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {

        }

        protected ILogProvider _crawlLogProvider; 
        public override string CommandDiscription
        {
            get { return "start crawling by inited params"; }
        }

        public override string CommandName
        {
            get { return "-start_crawl"; }
        }

        public async override Task<bool> CommandAction()
        {            
            RobotsFileDownloader robotsFileDownloader = new RobotsFileDownloader();
            ParallelQueue parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            ConcurrentBag<RobotsParser> concurrentBag = new ConcurrentBag<RobotsParser>();

            foreach (var domains in ClientConsoleLink.InputFieldses)
            {
                RobotsParser robotsParser = new RobotsParser(domains, _crawlLogProvider, robotsFileDownloader);
                concurrentBag.Add(robotsParser);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("", () => robotsParser.ParseFileAsync()));
            }
            parallelQueue.Process().Wait();
            _crawlLogProvider.SendNonStatusMessage("_____________________________________");
            parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            ClientConsoleLink._parallelQueue = parallelQueue;
            RegularExpressionHttpParser parser = new RegularExpressionHttpParser();
            foreach (RobotsParser robotsParser in concurrentBag)
            {
                var result = robotsParser.GetResult();
                Crawler.Crawler crawler = new Crawler.Crawler(result, _crawlLogProvider, parser,
                    ClientConsoleLink.OutPutPath);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>(result.Key.Host,
                    () => crawler.CrawAsync(parallelQueue.GetTokenByName(result.Key.Host))));
            }
            _crawlLogProvider.SendNonStatusMessage("_____________________________________");
            parallelQueue.Process().Wait();
            _crawlLogProvider.SendStatusMessage(LogMessages.Update, "");
            return true;
        }

        public override bool InvokeRequarement()
        {
            if (ClientConsoleLink._parallelQueue != null)
            {
                _logProvider.SendStatusMessage(LogMessages.Error, "the process is started. use -help for more info");
                return false;
            }
            if (ClientConsoleLink.InputFieldses == null)
            {
                _logProvider.SendStatusMessage(LogMessages.Error, "input array are empty");
                return false;
            }
            if (ClientConsoleLink.OutPutPath.Length == 0)
            {
                _logProvider.SendStatusMessage(LogMessages.Error, "out put directory not seted");
                return false;
            }
            return true;
        }

        public override string OnFailFunc()
        {
            return "";
        }

        public override bool PropertySetter(object obj)
        {
            if (ClientConsoleLink.OutPutPath.Length!=0)
            {
                try
                {
                    Directory.CreateDirectory(ClientConsoleLink.OutPutPath);
                    ConsoleLocalDispatcher consoleLocalDispatcher=new ConsoleLocalDispatcher(ClientConsoleLink.OutPutPath);
                    _crawlLogProvider = consoleLocalDispatcher;
                }
                catch (Exception e)
                {
                    _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                    return false;
                }
                return true;    
            }
            return false;
        }

        public override bool ResumeRequarement()
        {
            return true;
        }
    }
}