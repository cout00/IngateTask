using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Baseclasses;

namespace IngateTask.Loggers
{
    class SimpleLogger :Logger
    {
        public SimpleLogger(string outputPath) : base(outputPath)
        {
        }

        protected override string fileName
        {
            get { return "CommonLog.txt"; }
            set { value = "CommonLog.txt"; }
        }

        protected override string GetMessageString()
        {
            return "";
        }
    }
}
