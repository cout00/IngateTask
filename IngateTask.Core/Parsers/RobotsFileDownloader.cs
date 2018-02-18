using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Parsers
{
    public class RobotsFileDownloader : IRequest
    {
        public List<string> GetFileFromDomain(string uri)
        {
            string link = $"{uri}/robots.txt";
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            List<string> list = new List<string>();
            using (StreamReader stream = new StreamReader(webClient.OpenRead(new Uri(link))))
            {
                foreach (string str in stream.ReadToEnd().Split('\r', '\n'))
                {
                    list.Add(str.ToLower());
                }
            }
            return list;
        }
    }
}