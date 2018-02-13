using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.UserAgents
{
    public class YandexBot:IUserAgent
    {
        public int GetCrawlDelay { get; set; }
        public string GetUserAgentFullName() => "Mozilla/5.0 (compatible; YandexBot/3.0)";
        public string GetUserAgentName() => "Yandex";

    }
}
