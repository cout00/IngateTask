﻿namespace IngateTask.PortableLibrary.UserAgents
{
    public class CustomAgent : IUserAgent
    {
        public int GetCrawlDelay { get; set; }

        public string GetUserAgentFullName()
        {
            return "CustomBot";
        }

        public string GetUserAgentName()
        {
            return "CustomBot";
        }
    }
}