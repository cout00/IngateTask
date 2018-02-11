using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Service.WCF;

namespace CrawlerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var host=new ServiceHost(typeof(Crawler));
            host.Open();
            Console.WriteLine("service started...");
            Console.ReadLine();
            host.Close();
        }
    }
}
