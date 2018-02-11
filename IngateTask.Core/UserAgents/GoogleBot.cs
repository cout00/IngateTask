using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.UserAgents
{
    public class GoogleBot:IUserAgent
    {
        public string GetUserAgentFullName() => "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
        public string GetUserAgentName() => "Googlebot";
    }
}
