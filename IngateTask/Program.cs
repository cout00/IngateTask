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
            ServerStringCombiner stringCombiner = new ServerStringCombiner();
            ConsoleWriterLogger consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            LogMessanger logMessanger = new LogMessanger();
            logMessanger.Add(consoleWriterLogger);
            ClientConsole clientConsole = new ClientConsole("me", consoleWriterLogger);
            clientConsole.InitInterpreter();
            while (!cancelConsole)
            {
                clientConsole.Interpreter.Interpret(Console.ReadLine());
            }
        }
    }
}