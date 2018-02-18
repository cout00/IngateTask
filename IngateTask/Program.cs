using System;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Local
{
    internal class Program
    {
        private static readonly bool cancelConsole = false;

        private static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                var stringCombiner = new ServerStringCombiner();
                var consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
                var logMessanger = new LogMessanger();
                logMessanger.Add(consoleWriterLogger);
                var clientConsole = new ClientConsole("me", consoleWriterLogger);
                clientConsole.InitInterpreter();
                while (!cancelConsole)
                    clientConsole.Interpreter.Interpret(Console.ReadLine());
            }).Wait();
            
            
        }
    }
}