using System;
using IngateTask.Core.Clients;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.CommandInterpreter
{
    public abstract class Command
    {
        protected readonly ILogProvider _logProvider;
        protected readonly ClientConsole ClientConsoleLink;


        public Command(ILogProvider logProvider, ClientConsole clientConsoleLink)
        {
            _logProvider = logProvider;
            ClientConsoleLink = clientConsoleLink;
        }

        public abstract string CommandName { get; }

        /// <summary>
        ///     устанавливает свойство
        /// </summary>
        public Func<object, bool> PropertySetter { get; set; }

        /// <summary>
        ///     происходит при попытке вызова команды
        /// </summary>
        public Func<bool> InvokeRequarement { get; set; }

        /// <summary>
        ///     происходит если команда требует продолжения
        /// </summary>
        public Func<bool> ResumeRequarement { get; set; }

        /// <summary>
        ///     происходит если вызов не удался
        /// </summary>
        public Func<string> OnFailFunc { get; set; }

        public string Parameter { get; set; }
        public abstract string CommandDiscription { get; }

        public abstract bool CommandAction();
    }
}