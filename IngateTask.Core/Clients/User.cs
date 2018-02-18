using System.Collections.Generic;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Core.Clients
{
    public abstract class User
    {
        public readonly ILogProvider _logProvider;
        protected readonly string _name;

        protected User(string name, ILogProvider logProvider)
        {
            _name = name;
            _logProvider = logProvider;
        }

        public string InputFilePath { get; set; } = "";
        public string OutPutPath { get; set; } = "";
        public int ThreadNumber { get; set; } = 3;
        public List<InputFields> InputFieldses { get; set; }
    }
}