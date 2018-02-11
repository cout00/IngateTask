using IngateTask.Core.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Parsers;

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
            ConsoleWriterLogger consoleWriterLogger=new ConsoleWriterLogger(stringCombiner);
            InputLocalFileParser fileParser=new InputLocalFileParser(@"D:\input.txt", fileWriterLogger);
            var array= fileParser.GetParsedArray();
            if (!fileParser.FileIsValid)
            {
                Console.WriteLine("Input not Valid. Check the log. Skip?");
            }
            fileWriterLogger.CloseLogger();                            
        }

        public async static Task test()
        {
            
        }


    }
}
