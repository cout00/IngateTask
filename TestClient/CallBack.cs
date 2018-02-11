using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.ServiceReference1;

namespace TestClient
{
    public class CallBack :ICrawlerCallback
    {
        public void OnCallBack(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
