using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Interfaces;

namespace IngateTask.UserAgents
{
    public class YandexBot:IUserAgent
    {
        public string GetUserAgentFullName() => "Mozilla/5.0 (compatible; YandexBot/3.0)";
        public string GetUserAgentName() => "Yandex";

    }
}
