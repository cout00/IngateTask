namespace IngateTask.PortableLibrary.Interfaces
{
    /// <summary>
    /// статусы логов
    /// </summary>
    public enum LogMessages
    {
        Error,
        Exceptions,
        Warning,
        Update,
        Event
    }
    /// <summary>
    /// контракт на логеры
    /// </summary>
    public interface ILogProvider
    {
        void SendNonStatusMessage(string msg);
        void SendStatusMessage(LogMessages mgsStatus, string msg);
    }
}