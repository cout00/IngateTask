namespace IngateTask.PortableLibrary.UserAgents
{
    public interface IUserAgent
    {
        int GetCrawlDelay { get; set; }
        string GetUserAgentName();
        string GetUserAgentFullName();
    }
}