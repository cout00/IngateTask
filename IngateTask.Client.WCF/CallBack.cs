using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IngateTask.Client.WCF.CrawlerService;
using IngateTask.PortableLibrary.Classes;

namespace IngateTask.Client.WCF
{
    /// <summary>
    /// обратный вызов с сервера
    /// </summary>
    public class CallBack : ICrawlerCallback
    {
        public CrawlerClient CrawlerClient { get; set; }

        /// <summary>
        /// вызов на получение данных и их серилизация
        /// </summary>
        /// <param name="sendMeData"></param>
        /// <param name="filePath"></param>
        public void GetClientData(bool sendMeData, string filePath)
        {
            if (sendMeData)
            {
                ConsoleWriterLogger _logProvider = new ConsoleWriterLogger(new SimpleStringCombiner());
                InputLocalFileParser fileParser = new InputLocalFileParser(filePath, _logProvider);
                List<InputFields> array = fileParser.GetParsedArray();
                if (array != null)
                {
                    Task.Run(() => CrawlerClient.SetInputData(array.ToArray()));
                }
            }
        }

        /// <summary>
        /// выводит сообщения клиенту
        /// </summary>
        /// <param name="msg"></param>
        public void OnCallBack(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}