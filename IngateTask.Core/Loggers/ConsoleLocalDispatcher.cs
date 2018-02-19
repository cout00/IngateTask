using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Loggers
{
    /// <summary>
    /// груповой рассыльщик для локального интерфейса
    /// </summary>
    public class ConsoleLocalDispatcher : ILogProvider
    {
        private readonly LogMessanger logMessanger;

        public ConsoleLocalDispatcher(string path)
        {
            logMessanger = new LogMessanger();
            SimpleStringCombiner stringCombiner = new SimpleStringCombiner();
            FileWriterLogger fileWriterLogger = new FileWriterLogger(path,
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