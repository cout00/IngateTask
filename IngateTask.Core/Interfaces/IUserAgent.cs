using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Core.Interfaces
{
    public interface IUserAgent
    {
        string GetUserAgentName();
        string GetUserAgentFullName();
        int GetCrawlDelay { get; set; }
    }
}
