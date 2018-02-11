using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;

namespace IngateTask.Service.WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Crawler" в коде и файле конфигурации.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Crawler :ICrawler
    {
        public static ICrawlerCallBack CrawlerCallBack;

        public void DoWork(string msg)
        {
            CrawlerCallBack.OnCallBack($"Server: {msg}");
        }

        public void OpenSession()
        {
            CrawlerCallBack = OperationContext.Current.GetCallbackChannel<ICrawlerCallBack>();
        }

    }
}
