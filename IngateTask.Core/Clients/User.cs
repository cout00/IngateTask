using System.Collections.Generic;
using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Clients
{
    /// <summary>
    /// общий клиент
    /// </summary>
    public abstract class User
    {
        protected readonly string _name;
        public readonly ILogProvider UserLogProvider;

        protected User(string name, ILogProvider userLogProvider)
        {
            _name = name;
            UserLogProvider = userLogProvider;
            userLogProvider.SendNonStatusMessage($"Hello '{name}' use -help for more info");
        }

        public string InputFilePath { get; set; } = "";
        public string OutPutPath { get; set; } = "";
        public int ThreadNumber { get; set; } = 3;
        public List<InputFields> InputFieldses { get; set; }
    }
}