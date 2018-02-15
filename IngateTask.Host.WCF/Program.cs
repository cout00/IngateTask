using IngateTask.Service.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Host.WCF
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Crawler));
            host.Open();
            Console.WriteLine("service started...");
            Console.ReadLine();
            host.Close();
        }
    }
}
