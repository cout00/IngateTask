using IngateTask.Core.Interfaces;

namespace IngateTask.Core.UserAgents
{
    public class YandexBot : IUserAgent
    {
        public int GetCrawlDelay { get; set; }

        public string GetUserAgentFullName()
        {
            return "Mozilla/5.0 (compatible; YandexBot/3.0)";
        }

        public string GetUserAgentName()
        {
            return "Yandex";
        }
    }
}