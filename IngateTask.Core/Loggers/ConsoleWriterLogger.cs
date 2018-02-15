using System;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class ConsoleWriterLogger : ILogProvider
    {
        private readonly ILogStringCombiner _stringCombiner;

        public ConsoleWriterLogger(ILogStringCombiner stringCombiner)
        {
            _stringCombiner = stringCombiner;
        }

        public void SendNonStatusMessage(string msg)
        {
            Console.WriteLine(_stringCombiner.GetCombinedString(msg));
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
            Console.WriteLine($"{mgsStatus}: {_stringCombiner.GetCombinedString(msg)}");
        }
    }
}