using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Service.WCF.Logger
{
    /// <summary>
    /// рассыльщик в файл и в колбек
    /// </summary>
    public class ConsoleWCFDispatcher : ILogProvider
    {
        private readonly FileWriterLogger fileWriterLogger;
        private readonly ServiceCallBackDispatcher serviceCallBackDispatcher;

        public ConsoleWCFDispatcher(string outPath, ICrawlerCallBack crawlerCallBack)
        {
            SimpleStringCombiner simpleStringCombiner = new SimpleStringCombiner();
            ServerStringCombiner stringCombiner = new ServerStringCombiner();
            fileWriterLogger = new FileWriterLogger(outPath, simpleStringCombiner);
            serviceCallBackDispatcher = new ServiceCallBackDispatcher(crawlerCallBack, stringCombiner);
        }

        public void SendNonStatusMessage(string msg)
        {
            fileWriterLogger.SendNonStatusMessage(msg);
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
            fileWriterLogger.SendStatusMessage(mgsStatus, msg);
            switch (mgsStatus)
            {
                case LogMessages.Error:
                case LogMessages.Exceptions:
                case LogMessages.Warning:
                {
                    serviceCallBackDispatcher.SendStatusMessage(mgsStatus, msg);
                    break;
                }
            }
        }
    }
}