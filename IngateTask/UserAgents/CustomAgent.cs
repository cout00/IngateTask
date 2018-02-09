using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Interfaces;

namespace IngateTask.UserAgents
{
    public class CustomAgent :IUserAgent
    {
        public string GetUserAgentFullName() => "CustomBot";
        public string GetUserAgentName() => "CustomBot";

    }
}
