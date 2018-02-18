namespace IngateTask.PortableLibrary.Interfaces
{
    public enum LogMessages
    {
        Error,
        Exceptions,
        Warning,
        Update,
        Event
    }

    public interface ILogProvider
    {
        void SendNonStatusMessage(string msg);
        void SendStatusMessage(LogMessages mgsStatus, string msg);
    }
}