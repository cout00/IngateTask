using System;
using System.ServiceModel;
using IngateTask.Service.WCF;

namespace IngateTask.Host.WCF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ServiceHost host = new ServiceHost(typeof(Crawler));

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