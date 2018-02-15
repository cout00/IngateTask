using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using IngateTask.Core.Interfaces;

[assembly: InternalsVisibleTo("IngateTask.Tests")]

namespace IngateTask.Core.CommandInterpreter
{
    public class Interpreter : List<Command>
    {
        private readonly ILogProvider _logProvider;

        public Interpreter(ILogProvider logProvider)
        {
            _logProvider = logProvider;
        }

        public bool Interpret(string inpCommand)
        {
            var command = Parse(inpCommand);
            if (command == null)
                return false;
            if (!command.PropertySetter(command.Parameter))
                return false;
            if (command.InvokeRequarement())
                if (command.CommandAction())
                {
                    _logProvider.SendNonStatusMessage("OK");
                    return true;
                }
                else
                {
                    return command.ResumeRequarement();
                }
            if (command.OnFailFunc != null)
                _logProvider.SendNonStatusMessage(command.OnFailFunc());
            return false;
        }

        internal Command FindCommand(string name)
        {
            foreach (var command in this)
                if (string.Compare(command.CommandName.ToLower(), name) == 0)
                    return command;
            return null;
        }

        internal Command Parse(string inpstr)
        {
            inpstr = inpstr.Trim(' ').ToLower();
            if (!inpstr.StartsWith("-"))
            {
                _logProvider.SendNonStatusMessage($"Parse error. command {inpstr} unknow");
                return null;
            }
            var splitRes = inpstr.Split(' ');
            if (splitRes.Length == 0)
                return null;
            Command command = null;
            command = FindCommand(splitRes.First());
            if (splitRes.Length == 1)
            {
                if (command == null)
                {
                    _logProvider.SendNonStatusMessage($"Parse error. command {inpstr} unknow");
                    return null;
                }
                if (command.GetType().GetCustomAttribute(typeof(ParametrelessAttribute)) == null)
                {
                    _logProvider.SendNonStatusMessage($"Parse error. command {inpstr} have no params");
                    return null;
                }
            }
            else
            {
                command.Parameter = string.Empty;
                for (var i = 1; i < splitRes.Length; i++)
                    command.Parameter += splitRes[i] + ' ';
                command.Parameter.Trim(' ');
            }
            return command;
        }
    }
}