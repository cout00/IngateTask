using System.ServiceModel;

namespace IngateTask.Service.WCF
{
    public interface ICrawlerCallBack
    {
        [OperationContract]
        void OnCallBack(string msg);

        [OperationContract]
        void GetClientData(bool sendMeData, string filePath);
    }
}