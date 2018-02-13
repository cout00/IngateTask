using System.Collections.Generic;
using System.Linq;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public enum LogMessages
    {
        Error,
        Exceptions,
        Warning,
        Update,
        Event,
    }

    public class LogMessanger:Dictionary<string,ILogProvider>
    {
        public void PostMessage(string msg)
        {
            foreach (var logger in this)
            {
                logger.Value.SendNonStatusMessage(msg);
            }
        }

        public void PostMessageDirectly(string msg, params string[] receiversList)
        {
            foreach (var logger in this)
            {
                if (logger.Key.In(receiversList.ToArray()))
                {
                logger.Value.SendNonStatusMessage(msg);
                    
                }
            }
        }

        public void PostStatusMessageDirectly(LogMessages logMessages, string msg, params string[] receiversList)
        {
            foreach (var logger in this)
            {
                if (logger.Key.In(receiversList.ToArray()))
                {
                    logger.Value.SendStatusMessage(logMessages, msg);
                }
            }
        }



        public void PostStatusMessage(LogMessages logMessages, string msg)
        {
            foreach (var logger in this)
            {
                logger.Value.SendStatusMessage(logMessages,msg);
            }
        }
    }
}