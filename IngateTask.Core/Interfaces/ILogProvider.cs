using IngateTask.Core.Loggers;

namespace IngateTask.Core.Interfaces
{
    public interface ILogProvider
    {
        void SendNonStatusMessage(string msg);
        void SendStatusMessage(LogMessages mgsStatus, string msg);
    }
}