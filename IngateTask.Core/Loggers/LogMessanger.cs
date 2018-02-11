using System.Collections.Generic;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public enum LogMessages
    {
        Error,
        Exceptions,
        Warning,
        Update,
    }

    public class LogMessanger:List<ILogProvider>
    {
        public void PostMessage(string msg)
        {
            foreach (var logger in this)
            {
                logger.SendNonStatusMessage(msg);
            }
        }
        public void PostStatusMessage(LogMessages logMessages, string msg)
        {
            foreach (var logger in this)
            {
                logger.SendStatusMessage(logMessages,msg);
            }
        }
    }
}