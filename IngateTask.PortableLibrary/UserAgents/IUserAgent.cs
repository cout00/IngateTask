namespace IngateTask.PortableLibrary.UserAgents
{
    /// <summary>
    /// от них зависит задержка и то что вернет сайт
    /// </summary>
    public interface IUserAgent
    {
        int GetCrawlDelay { get; set; }
        string GetUserAgentName();
        string GetUserAgentFullName();
    }
}