﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Core.Interfaces
{
    public interface ILogStringCombiner
    {
        string GetCombinedString(string inputString);
    } 
}
