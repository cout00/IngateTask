using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Service.WCF.Logger
{
    public class ConsoleWCFDispatcher :ILogProvider
    {
        ServiceCallBackDispatcher serviceCallBackDispatcher;
        FileWriterLogger fileWriterLogger;
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
                case LogMessages.Event:
                    {
                        serviceCallBackDispatcher.SendStatusMessage(mgsStatus, msg);
                        break;
                    }
            }
        }
    }
}
