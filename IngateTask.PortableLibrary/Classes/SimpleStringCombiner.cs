using System;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.PortableLibrary.Classes
{
    /// <summary>
    /// прост строк с датой
    /// </summary>
    public class SimpleStringCombiner : ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss") + ": " + inputString;
        }
    }
}