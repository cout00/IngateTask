using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using IngateTask.Core.ParallelThread;
using IngateTask.Core.Parsers;

namespace IngateTask.Core.CommandInterpreter
{
    [Parametreless]
    public class CommandHelp : Command
    {
        public CommandHelp(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription => "show help";

        public override string CommandName => "-help";

        public override bool CommandAction()
        {
            var outString = Environment.NewLine;
            foreach (var command in ClientConsoleLink.Interpreter)
                outString += $" {command.CommandName}: {command.CommandDiscription}{Environment.NewLine}";
            _logProvider.SendNonStatusMessage(outString.TrimEnd('\r', '\n'));
            return true;
        }
    }

    public class CommandReadInputFile : Command
    {
        public CommandReadInputFile(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription => "Load File to the system";

        public override string CommandName => "-load_file";

        public override bool CommandAction()
        {
            var fileParser = new InputLocalFileParser(Parameter, _logProvider);
            var array = fileParser.GetParsedArray();
            ClientConsoleLink.InputFieldses = array;
            if (array == null)
                return false;
            if (!fileParser.FileIsValid)
                return false;
            return true;
        }
    }

    public class CommandSetThreads : Command
    {
        public CommandSetThreads(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription => "set a number of execution thread";

        public override string CommandName => "-set_threads";

        public override bool CommandAction()
        {
            ClientConsoleLink.ThreadNumber = int.Parse(Parameter);
            return true;
        }
    }

    public class CommandOutPutPath : Command
    {
        public CommandOutPutPath(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription => "set output directory";

        public override string CommandName => "-out_dir";

        public override bool CommandAction()
        {
            ClientConsoleLink.OutPutPath = Parameter;
            return true;
        }
    }

    [Parametreless]
    public class CommandStartCrawl : Command
    {
        public CommandStartCrawl(ILogProvider logProvider, ClientConsole clientConsoleLink) : base(logProvider,
            clientConsoleLink)
        {
        }

        public override string CommandDiscription => "start crawling by inited params";

        public override string CommandName => "-start_crawl";

        public override bool CommandAction()
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
            var logMessanger = new LogMessanger();
            var stringCombiner = new SimpleStringCombiner();
            var fileWriterLogger = new FileWriterLogger(Path.Combine(ClientConsoleLink.OutPutPath, "log.txt"),
                stringCombiner);
            var consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            logMessanger.Add("me", fileWriterLogger);
            logMessanger.Add("me1", consoleWriterLogger);
            var robotsFileDownloader = new RobotsFileDownloader();
            var parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            var concurrentBag = new ConcurrentBag<RobotsParser>();
            foreach (var domains in ClientConsoleLink.InputFieldses)
            {
                var robotsParser = new RobotsParser(domains, logMessanger, robotsFileDownloader);
                concurrentBag.Add(robotsParser);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("", () => robotsParser.ParseFileAsync()));
            }
            parallelQueue.Process().Wait();
            parallelQueue = new ParallelQueue(ClientConsoleLink.ThreadNumber);
            var parser = new RegularExpressionHttpParser();
            foreach (var robotsParser in concurrentBag)
            {
                var crawler = new Crawler.Crawler(robotsParser.GetResult(), logMessanger, parser,
                    ClientConsoleLink.OutPutPath);
                parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("me",
                    () => crawler.CrawAsync(parallelQueue.GetTokenByName("me"))));
            }
            parallelQueue.Process().Wait();
            return true;
        }
    }
}