using System.IO;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Core.Loggers
{
    public class ConsoleLocalDispatcher : ILogProvider
    {
        private readonly LogMessanger logMessanger;

        public ConsoleLocalDispatcher(string path)
        {
            logMessanger = new LogMessanger();
            SimpleStringCombiner stringCombiner = new SimpleStringCombiner();
            FileWriterLogger fileWriterLogger = new FileWriterLogger(Path.Combine(path, "log.txt"),
                stringCombiner);
            ConsoleWriterLogger consoleWriterLogger = new ConsoleWriterLogger(stringCombiner);
            logMessanger.Add(fileWriterLogger);
            logMessanger.Add(consoleWriterLogger);
        }

        public void SendNonStatusMessage(string msg)
        {
            logMessanger.PostMessage(msg);
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
            logMessanger.PostStatusMessage(mgsStatus, msg);
        }
    }
}