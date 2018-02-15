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
using IngateTask.Core.Clients;
using IngateTask.Core.Crawler;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Parsers;
using IngateTask.Core.ParallelQueue;

namespace IngateTask
{
    class Program
    {
        private static bool cancelConsole = false;


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

        static async Task Process()
        {


            //RobotsFileDownloader robotsFileDownloader = new RobotsFileDownloader();
            //ParallelQueue parallelQueue = new ParallelQueue(2);
            //ConcurrentBag<RobotsParser> concurrentBag = new ConcurrentBag<RobotsParser>();
            //foreach (var domains in array)
            //{
            //    RobotsParser robotsParser = new RobotsParser(domains, logMessanger, robotsFileDownloader);
            //    concurrentBag.Add(robotsParser);
            //    parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("", () => robotsParser.ParseFileAsync()));
            //}
            //parallelQueue.Process().Wait();
            //parallelQueue = new ParallelQueue(2);
            //RegularExpressionHttpParser parser = new RegularExpressionHttpParser();
            //foreach (var robotsParser in concurrentBag)
            //{
            //    Crawler crawler = new Crawler(robotsParser.GetResult(), logMessanger, parser, "");
            //    parallelQueue.Queue(new KeyValuePair<string, Func<Task>>("me", () => crawler.CrawAsync(parallelQueue.GetTokenByName("me"))));
            //}
            //parallelQueue.Process();
            ////fileWriterLogger.CloseLogger();
            //Console.ReadLine();
        }


        static void Main(string[] args)
        {
            SimpleStringCombiner stringCombiner = new SimpleStringCombiner();
            //FileWriterLogger fileWriterLogger = new FileWriterLogger(Path.Combine(@"D:\", "log.txt"), stringCombiner);
            ConsoleWriterLogger consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            LogMessanger logMessanger = new LogMessanger();
            logMessanger.Add("consoleWriter", consoleWriterLogger);
            //Console.WriteLine("Hello, put ur input file at line below");
            //Console.WriteLine("input file looks like:");
            //Console.WriteLine("<line>::=<domain><user_agent>|<crawl-delay>");
            //Console.WriteLine("example:");
            //Console.WriteLine("https://stackoverflow.com yandex\r\nhttp://theory.phphtml.net google\r\nhttp://www.mkyong.com yandex\r\nhttp://2coders.ru 300\r\nhttps://habrahabr.ru google");
            Client client=new Client("me",consoleWriterLogger);
            client.InitInterpreter();
            while (!cancelConsole)
            {
                client.Interpreter.Interpret(Console.ReadLine());
            }
        }



    }
}
