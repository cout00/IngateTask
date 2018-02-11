using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Interfaces;

namespace IngateTask.Loggers
{
    public class SimpleStringCombiner :ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return inputString;
        }
    }
}
