using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class SimpleStringCombiner :ILogStringCombiner
    {
        public string GetCombinedString(string inputString)
        {
            return inputString;
        }
    }
}
