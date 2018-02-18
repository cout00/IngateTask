using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IngateTask.Client.WCF.CrawlerService;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Client.WCF
{
    public class CallBack :ICrawlerCallback
    {
        public CrawlerClient CrawlerClient { get; set; }

        public void GetClientData(bool sendMeData, string filePath)
        {
            if (sendMeData)
            {
                ConsoleWriterLogger _logProvider=new ConsoleWriterLogger(new SimpleStringCombiner());
                var fileParser = new InputLocalFileParser(filePath, _logProvider);
                var array = fileParser.GetParsedArray();
                if (array!=null)
                {                                        
                    Task.Run(() => CrawlerClient.SetInputData(array.ToArray()));
                }
            }
        }

        public void OnCallBack(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
