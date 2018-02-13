using IngateTask.Core.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Core.Crawler;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Parsers;
using IngateTask.Core.ParallelQueue;

namespace IngateTask
{
    class Program
    {
        Dictionary<string, IUserAgent> inputDictionary = new Dictionary<string, IUserAgent>();

        private static string outDirectory = "";
        private static int threadCount = 1;
        private static LogMessanger _logMessanger=new LogMessanger();

        static bool ParseArgs(List<string> args)
        {
            switch (args.Count)
            {
                case 1:
                    {
                        if (args.First() == "-help")
                        {
                            Common.ShowHelp();
                            return true;
                        }
                        return false;
                    }
                case 6:
                    {
                        if (args[0] == "-input" && args[2] == "-output" && args[4] == "-thread_count")
                        {
                            //все ошибки парсинга заранее пишем в лог
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }
            return false;
        }

        static void Main(string[] args)
        {
            SimpleStringCombiner stringCombiner = new SimpleStringCombiner();
            FileWriterLogger fileWriterLogger = new FileWriterLogger(Path.Combine(@"D:\", "log.txt"), stringCombiner);
            ConsoleWriterLogger consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            InputLocalFileParser fileParser = new InputLocalFileParser(@"D:\input.txt", fileWriterLogger);
            LogMessanger logMessanger=new LogMessanger();
            logMessanger.Add("consoleWriter",consoleWriterLogger);
            logMessanger.Add("filewriter",fileWriterLogger);
            var array = fileParser.GetParsedArray();
            RobotsFileDownloader robotsFileDownloader=new RobotsFileDownloader();
            ParallelQueue parallelQueue=new ParallelQueue(2);
            ConcurrentBag<RobotsParser> concurrentBag = new ConcurrentBag<RobotsParser>();
            foreach (var domains in array)
            {
                RobotsParser robotsParser=new RobotsParser(domains, logMessanger,robotsFileDownloader);
                concurrentBag.Add(robotsParser);
                parallelQueue.Queue(() => robotsParser.ParseFileAsync());
            }
            parallelQueue.Process().Wait();
            parallelQueue = new ParallelQueue(2);
            RegularExpressionHttpParser parser=new RegularExpressionHttpParser();
            foreach (var robotsParser in concurrentBag)
            {
                Crawler crawler=new Crawler(robotsParser.GetResult(),logMessanger,parser,"");
                parallelQueue.Queue(() =>crawler.CrawAsync());
            }
            parallelQueue.Process().Wait();
            fileWriterLogger.CloseLogger();
            Console.ReadLine();
        }



    }
}
