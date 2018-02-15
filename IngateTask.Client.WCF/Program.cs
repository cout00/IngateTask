using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Client.WCF.CrawlerServices;

namespace IngateTask.Client.WCF
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new CallBack());
            CrawlerServices.CrawlerClient client = new CrawlerServices.CrawlerClient(context);
            bool close_flag = true;
            client.OpenSession();
            while (close_flag)
            {
                client.DoWork(Console.ReadLine());
            }
        }
    }
}
