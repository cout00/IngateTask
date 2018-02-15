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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Crawler :ICrawler
    {
        private int test = 0;
        public static ICrawlerCallBack CrawlerCallBack;
        Timer Timer=new Timer();

        public void DoWork(string msg)
        {
            //CrawlerCallBack.OnCallBack($"Server: {msg}");
        }

        public void OpenSession()
        {
            CrawlerCallBack = OperationContext.Current.GetCallbackChannel<ICrawlerCallBack>();
            Timer.Elapsed += Timer_Elapsed;
            Timer.Interval = 1000;
            Timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CrawlerCallBack.OnCallBack($"Server: {test} {OperationContext.Current.SessionId}");
            test++;
        }
    }
}
