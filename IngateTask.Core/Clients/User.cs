using System.Collections.Concurrent;
using System.Collections.Generic;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Core.Clients
{
    public abstract class User
    {
        public readonly ILogProvider UserLogProvider;    
        protected readonly string _name;
        public ConcurrentBag<Crawler.Crawler> runnedCrawlers=new ConcurrentBag<Crawler.Crawler>();

        protected User(string name, ILogProvider userLogProvider)
        {
            _name = name;
            UserLogProvider = userLogProvider;
            userLogProvider.SendNonStatusMessage($"Hello user {name} use -help for more info");
        }
        public string InputFilePath { get; set; } = "";
        public string OutPutPath { get; set; } = "";
        public int ThreadNumber { get; set; } = 3;
        public List<InputFields> InputFieldses { get; set; }
    }
}