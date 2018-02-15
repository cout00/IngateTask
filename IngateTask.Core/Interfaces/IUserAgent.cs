namespace IngateTask.Core.Interfaces
{
    public interface IUserAgent
    {
        int GetCrawlDelay { get; set; }
        string GetUserAgentName();
        string GetUserAgentFullName();
    }
}