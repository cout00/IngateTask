using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Client.WCF.CrawlerService;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Client.WCF
{
    class Program
    {
        static CrawlerClient client;
        static void Main(string[] args)
        {
            //try
            //{
                CallBack callBack = new CallBack();
                InstanceContext context = new InstanceContext(callBack);
                client = new CrawlerClient(context);
                callBack.CrawlerClient = client;
                bool close_flag = true;
                client.OpenSession("user");
                while (close_flag)
                {
                    Interpret();
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine("_RUN ME AS LOCAL!!! ADMIN___");
            //    Console.WriteLine("_________I_AM:______________");
            //    Console.WriteLine("___http://localhost:8733/___");
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine("____________________________");
            //    Console.WriteLine(e.Message);
            //}            
            Console.ReadLine();            
        }

        static async Task Interpret()
        {
            await client.InterpretAsync(Console.ReadLine());
        }
    }
}
