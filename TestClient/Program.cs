using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestClient.ServiceReference1;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new CallBack());
            CrawlerClient client = new CrawlerClient(context);
            bool close_flag = true;
            client.OpenSession();
            while (close_flag)
            {
                client.DoWork(Console.ReadLine());
            }


        }
    }
}
