using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Interfaces;

namespace IngateTask.Loggers
{
    public class ConsoleWriterLogger :ILogProvider
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
            Console.WriteLine($"{mgsStatus.ToString()}: {_stringCombiner.GetCombinedString(msg)}");
        }
    }
}
