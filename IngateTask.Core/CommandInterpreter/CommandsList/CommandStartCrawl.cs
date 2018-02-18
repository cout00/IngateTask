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
            try
            {
                Directory.CreateDirectory(ClientConsoleLink.OutPutPath);
            }
            catch (Exception e)
            {
                _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                return false;
            }
            RobotsFileDownloader robotsFileDownloader = new RobotsFileDownloader();
            ParallelQueue parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            ConcurrentBag<RobotsParser> concurrentBag = new ConcurrentBag<RobotsParser>();
            ConsoleLocalDispatcher consoleLocalDispatcher = new ConsoleLocalDispatcher(ClientConsoleLink.OutPutPath);
            foreach (var domains in ClientConsoleLink.InputFieldses)
            {
                RobotsParser robotsParser = new RobotsParser(domains, consoleLocalDispatcher, robotsFileDownloader);
                concurrentBag.Add(robotsParser);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("", () => robotsParser.ParseFileAsync()));
            }
            parallelQueue.Process().Wait();
            parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            ClientConsoleLink._parallelQueue = parallelQueue;
            RegularExpressionHttpParser parser = new RegularExpressionHttpParser();
            foreach (RobotsParser robotsParser in concurrentBag)
            {
                Crawler.Crawler crawler = new Crawler.Crawler(robotsParser.GetResult(), consoleLocalDispatcher, parser,
                    ClientConsoleLink.OutPutPath);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("me",
                    () => crawler.CrawAsync(parallelQueue.GetTokenByName("me"))));
            }
            await parallelQueue.Process();
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
            return true;
        }

        public override bool ResumeRequarement()
        {
            return true;
        }
    }
}