using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.UserAgents
{
    public class CustomAgent :IUserAgent
    {
        public string GetUserAgentFullName() => "CustomBot";
        public string GetUserAgentName() => "CustomBot";

    }
}
