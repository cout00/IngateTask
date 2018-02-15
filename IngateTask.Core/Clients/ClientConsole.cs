using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.CommandInterpreter;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using IngateTask.Core.Parsers;

namespace IngateTask.Core.Clients
{
    public class ClientConsole:User
    {
        public Interpreter Interpreter { get; set; }
        public ClientConsole(string name, ILogProvider logProvider) : base(name, logProvider)
        {
            Interpreter=new Interpreter(logProvider);
        }
              
        public virtual void InitInterpreter()
        {
            CommandReadInputFile commandReadInputFile=new CommandReadInputFile(_logProvider,this);
            commandReadInputFile.PropertySetter = obj => { InputFilePath = (string) obj;
                return true;
            }; 
            commandReadInputFile.InvokeRequarement = () => { return InputFilePath.Length != 0 && ThreadNumber != 0; };
            commandReadInputFile.OnFailFunc = () => { return "need file name"; };
            commandReadInputFile.ResumeRequarement = () =>
            {
                _logProvider.SendNonStatusMessage("file have mistakes. some lines will be skipped");                
                return true;
            };

            CommandSetThreads commandSetThreads=new CommandSetThreads(_logProvider,this);
            commandSetThreads.PropertySetter = obj =>
            {
                try
                {
                    ThreadNumber = int.Parse((string)obj);
                }
                catch (Exception e)
                {
                    _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                    return false;
                }
                return true;
            };
            commandSetThreads.InvokeRequarement = () => { return ThreadNumber != 0; };
            commandSetThreads.OnFailFunc = () => { return "set threads number"; };
            commandSetThreads.ResumeRequarement = () => true;

            CommandHelp commandHelp=new CommandHelp(_logProvider,this);
            commandHelp.PropertySetter = obj => { return true; };
            commandHelp.InvokeRequarement = () => true;
            commandHelp.ResumeRequarement = () => true;

            CommandOutPutPath commandOutPutPath=new CommandOutPutPath(_logProvider,this);
            commandOutPutPath.PropertySetter = obj =>
            {
                try
                {
                    var newPath= (string)obj;
                    if (!newPath.EndsWith("\\"))
                    {
                        newPath += "\\";
                    }
                    OutPutPath = Path.GetDirectoryName(Path.GetFullPath(newPath));
                    _logProvider.SendNonStatusMessage("u entered: "+OutPutPath);
                }
                catch (Exception e)
                {
                    _logProvider.SendStatusMessage(LogMessages.Exceptions, e.Message);
                    return false;
                }
                return true;
            };
            commandOutPutPath.InvokeRequarement = () => true;
            commandOutPutPath.ResumeRequarement = () => true;

            CommandStartCrawl commandStartCrawl=new CommandStartCrawl(_logProvider,this);
            commandStartCrawl.PropertySetter = obj => { return true; };
            commandStartCrawl.InvokeRequarement = () =>
            {
                if (InputFieldses==null)
                {
                    _logProvider.SendStatusMessage(LogMessages.Error, "input array are empty");
                    return false;
                }
                if (OutPutPath.Length==0)
                {
                    _logProvider.SendStatusMessage(LogMessages.Error, "out put directory not seted");
                    return false;
                }
                return true;
            };
            commandStartCrawl.ResumeRequarement = () => true;

            Interpreter.Add(commandStartCrawl);
            Interpreter.Add(commandOutPutPath);
            Interpreter.Add(commandHelp);
            Interpreter.Add(commandSetThreads);
            Interpreter.Add(commandReadInputFile);
        }
    }
}
