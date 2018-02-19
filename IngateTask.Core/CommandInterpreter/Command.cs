using System.Threading.Tasks;
using IngateTask.Core.Clients;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.CommandInterpreter
{
    /// <summary>
    /// команда патерн "команда" только адаптированный под эту задачу
    /// </summary>
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

        public string Parameter { get; set; }
        public abstract string CommandDiscription { get; }

        /// <summary>
        ///     устанавливает свойство
        /// </summary>
        public abstract bool PropertySetter(object obj);

        /// <summary>
        ///     происходит при попытке вызова команды
        /// </summary>
        public abstract bool InvokeRequarement();

        /// <summary>
        ///     происходит если команда требует продолжения
        /// </summary>
        public abstract bool ResumeRequarement();

        /// <summary>
        ///     происходит если вызов не удался
        /// </summary>
        public abstract string OnFailFunc();

        /// <summary>
        /// само действие при команде
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> CommandAction();
    }
}