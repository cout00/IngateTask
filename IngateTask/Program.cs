using System;
using IngateTask.Core.Clients;
using IngateTask.Core.Loggers;

namespace IngateTask.Local
{
    internal class Program
    {
        private static readonly bool cancelConsole = false;

        private static void Main(string[] args)
        {
            var stringCombiner = new ServerStringCombiner();
            var consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            var logMessanger = new LogMessanger();
            logMessanger.Add("consoleWriter", consoleWriterLogger);
            var clientConsole = new ClientConsole("me", consoleWriterLogger);
            //Directory.CreateDirectory(@"D:\\me\\www.speedtest.net");
            clientConsole.InitInterpreter();
            while (!cancelConsole)
                clientConsole.Interpreter.Interpret(Console.ReadLine());
        }
    }
}