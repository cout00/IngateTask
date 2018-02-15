using System;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class SimpleStringCombiner : ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss") + ": " + inputString;
        }
    }
}