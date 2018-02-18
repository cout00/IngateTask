namespace IngateTask.PortableLibrary.UserAgents
{
    public class GoogleBot : IUserAgent
    {
        public int GetCrawlDelay { get; set; }

        public string GetUserAgentFullName()
        {
            return "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
        }

        public string GetUserAgentName()
        {
            return "Googlebot";
        }
    }
}