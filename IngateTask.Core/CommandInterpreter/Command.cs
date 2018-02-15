using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.CommandInterpreter
{
    public abstract class Command
    {
        protected readonly ILogProvider _logProvider;
        protected readonly Client _clientLink;
        abstract public string CommandName { get;}
        public Func<bool> InvokeRequarement { get; set; }
        public Func<bool> ResumeRequarement { get; set; }
        public Func<string> OnFailFunc { get; set; }
        public string Parameter { get; set; }

        public Command(ILogProvider logProvider, Client clientLink)
        {
            _logProvider = logProvider;
            _clientLink = clientLink;
        }

        abstract public bool CommandAction();

    }
}
