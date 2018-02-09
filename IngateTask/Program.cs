using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Interfaces;
using IngateTask.Loggers;

namespace IngateTask
{
    class Program
    {
        Dictionary<string, IUserAgent> inputDictionary=new Dictionary<string, IUserAgent>();

        private static string outDirectory = "";
        private static int threadCount = 1;
        private static SimpleLogger _simpleLogger;

        static bool ParseArgs(List<string> args)
        {
            switch (args.Count)
            {
                case 1:
                {
                    if (args.First()=="-help")
                    {
                        Common.ShowHelp();
                        return true;
                    }
                    return false;
                }
                case 6:
                {
                    if (args[0]== "-input"&& args[2] == "-output" && args[4] == "-thread_count")
                    {
                        //все ошибки парсинга заранее пишем в лог
                    }
                    break;
                }
                default:
                {
                    return  false;
                }
            }
            return false;
        }

        static void test1()
        {
            for (int i = 0; i < 100; i++)
            {
                _simpleLogger.AddToLogAsync( $"task1: {i.ToString()}");
            }
        }

        static void test2()
        {
            for (int i = 0; i < 100; i++)
            {
                _simpleLogger.AddToLogAsync($"task2: {i.ToString()}");
            }
        }

        static void test3()
        {
            for (int i = 0; i < 100; i++)
            {
                _simpleLogger.AddToLogAsync($"task3: {i.ToString()}");
            }
        }


        static void Main(string[] args)
        {
            _simpleLogger=new SimpleLogger(@"D:\");
            Task.Factory.StartNew(test1);
            Task.Factory.StartNew(test2);
            Task.Factory.StartNew(test3);

            Console.ReadLine();
            _simpleLogger.CloseLog();
        }

        public async static Task test()
        {
            string link = @"https://habrahabr.ru/robots.txt";
            WebClient webClient=new WebClient();
            webClient.Encoding=Encoding.UTF8;                    
            await webClient.DownloadFileTaskAsync(new Uri(link), @"D:\map1.txt");
        }


    }
}
