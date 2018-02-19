using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using IngateTask.Core.Loggers;
using IngateTask.PortableLibrary.Classes;
using IngateTask.Service.WCF.Logger;

namespace IngateTask.Service.WCF
{
    /// <summary>
    /// сервис на сессиях с поодержкой многопоточности и ручной синхронизации. 
    /// без UseSynchronizationContext = false эт гнида выбивает дедлок 
    /// при попытке вызвать сервис из колбека
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class Crawler : ICrawler, IDisposable
    {
        public ICrawlerCallBack _crawlerCallBack;
        private WCFConsoleClient client;

        /// <summary>
        /// а тут костыль эта гнида не хочет преобразовывать сервис в асинхронный пока нет
        /// любого ассинхронного метода. ПС данный метод синхронный
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task Interpret(string command)
        {
            await client.Interpreter.Interpret(command);
        }
        /// <summary>
        /// инициализация сессии
        /// </summary>
        /// <param name="userName"></param>
        public void OpenSession(string userName)
        {
            _crawlerCallBack = OperationContext.Current.GetCallbackChannel<ICrawlerCallBack>();
            ServerStringCombiner stringCombiner = new ServerStringCombiner();
            ServiceCallBackDispatcher serviceCallBackDispatcher =
                new ServiceCallBackDispatcher(_crawlerCallBack, stringCombiner);
            client = new WCFConsoleClient(userName, serviceCallBackDispatcher, _crawlerCallBack);
            client.InitInterpreter();
        }

        /// <summary>
        /// десериализованные данные от клиента
        /// </summary>
        /// <param name="inputFieldses"></param>
        public void SetInputData(InputFields[] inputFieldses)
        {
            client.InputFieldses = inputFieldses.ToList();
        }

        /// <summary>
        /// прост костыль. он не хотел умирать
        /// </summary>
        public void Dispose()
        {
        }
    }
}