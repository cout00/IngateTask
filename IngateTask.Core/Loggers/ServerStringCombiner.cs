using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class ServerStringCombiner : ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return "Server -> " + inputString;
        }
    }
}