using System.ServiceModel;
using System.Threading.Tasks;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Service.WCF
{
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