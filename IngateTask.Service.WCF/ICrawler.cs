using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IngateTask.Service.WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "ICrawler" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(ICrawlerCallBack))]
    public interface ICrawler
    {
        [OperationContract]
        void DoWork(string msg);
        [OperationContract]
        void OpenSession();
    }
}
