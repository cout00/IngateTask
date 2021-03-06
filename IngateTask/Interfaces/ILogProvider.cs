﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Loggers;

namespace IngateTask.Interfaces
{
    public interface ILogProvider
    {
        void SendNonStatusMessage(string msg);
        void SendStatusMessage(LogMessages mgsStatus, string msg);
    }
}
