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

            CallBack callBack = new CallBack();
            InstanceContext context = new InstanceContext(callBack);
            client = new CrawlerClient(context);
            callBack.CrawlerClient = client;
            bool close_flag = true;
            client.OpenSession("user");
            //client.SetInputData(new InputFields[1] {new InputFields() {UserAgent = "me",Domain = "me"} });
            while (close_flag)
            {
                interpret();
            }
            //Console.ReadLine();            

        }

        static async Task interpret()
        {
            await client.InterpretAsync(Console.ReadLine());
        }
    }
}
