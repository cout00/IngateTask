using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Client.WCF.CrawlerServices;

namespace IngateTask.Client.WCF
{
    public class CallBack :ICrawlerCallback
    {
        public void OnCallBack(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
