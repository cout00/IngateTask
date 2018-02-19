using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Loggers
{
    /// <summary>
    /// комбинирует строку чтобы рядом была надпись сервер
    /// </summary>
    public class ServerStringCombiner : ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return "Server -> " + inputString;
        }
    }
}