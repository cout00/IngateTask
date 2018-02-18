using IngateTask.PortableLibrary.Interfaces;
using System;

namespace IngateTask.PortableLibrary.Classes
{
    public class SimpleStringCombiner : ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss") + ": " + inputString;
        }
    }
}