using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using IngateTask.Core.Clients;
using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Classes;
using IngateTask.Service.WCF.Logger;

namespace IngateTask.Service.WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Crawler" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, 
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class Crawler :ICrawler, IDisposable
    {
        public ICrawlerCallBack _crawlerCallBack;
        WCFConsoleClient client;
        public void Dispose()
        {
            var i = 1;
            //throw new NotImplementedException();
        }
        public async Task Interpret(string command)
        {
            await client.Interpreter.Interpret(command);
        }

        public void OpenSession(string userName)
        {
            _crawlerCallBack = OperationContext.Current.GetCallbackChannel<ICrawlerCallBack>();
            ServerStringCombiner stringCombiner = new ServerStringCombiner();
            ServiceCallBackDispatcher serviceCallBackDispatcher = new ServiceCallBackDispatcher(_crawlerCallBack, stringCombiner);
            serviceCallBackDispatcher.SendNonStatusMessage($"Hello {userName}");
            client = new WCFConsoleClient(userName, serviceCallBackDispatcher, _crawlerCallBack);
            client.InitInterpreter();
        }

        public void SetInputData(InputFields[] inputFieldses)
        {
            client.InputFieldses = inputFieldses.ToList();
        }
    }
}
