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
            var link = $"{uri}/robots.txt";
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var list = new List<string>();
            using (var stream = new StreamReader(webClient.OpenRead(new Uri(link))))
            {
                foreach (var str in stream.ReadToEnd().Split('\r', '\n'))
                    list.Add(str.ToLower());
            }
            return list;
        }
    }
}