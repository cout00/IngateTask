using System.Collections.Generic;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class LogMessanger : List<ILogProvider>
    {
        public void PostMessage(string msg)
        {
            foreach (ILogProvider logger in this)
            {
                logger.SendNonStatusMessage(msg);
            }
        }

        public void PostStatusMessage(LogMessages logMessages, string msg)
        {
            foreach (ILogProvider logger in this)
            {
                logger.SendStatusMessage(logMessages, msg);
            }
        }
    }
}