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
            try
            {
                var host = new ServiceHost(typeof(Crawler));

                host.Open();

                Console.WriteLine("Service started!!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine("____________________________");
                Console.WriteLine("____________________________");
                Console.WriteLine("____________________________");
                Console.WriteLine("_RUN ME AS LOCAL!!! ADMIN___");
                Console.WriteLine("_________I_AM:______________");
                Console.WriteLine("___http://localhost:8092____");
                Console.WriteLine("____________________________");
                Console.WriteLine("____________________________");
                Console.WriteLine("____________________________");
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
