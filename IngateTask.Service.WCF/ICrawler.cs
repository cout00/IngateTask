using IngateTask.PortableLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Service.WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "ICrawler" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(ICrawlerCallBack))]
        
    public interface ICrawler
    {
        [OperationContract(IsOneWay = false)]
        void SetInputData(InputFields[] inputFieldses);
        [OperationContract(IsOneWay = false)]
        Task Interpret(string command);
        [OperationContract]
        void OpenSession(string userName);
    }
}
