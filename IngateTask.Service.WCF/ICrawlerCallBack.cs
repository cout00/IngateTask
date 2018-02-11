using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Service.WCF
{
    public interface ICrawlerCallBack
    {
        [OperationContract]
        void OnCallBack(string msg);
    }
}
