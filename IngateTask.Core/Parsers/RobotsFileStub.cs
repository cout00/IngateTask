using System.Collections.Generic;
using System.IO;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Parsers
{
    public class RobotsFileStub : IRequest
    {
        public List<string> GetFileFromDomain(string uri)
        {
            var list = new List<string>();
            using (var stream = new StreamReader(File.OpenRead(uri)))
            {
                foreach (var str in stream.ReadToEnd().Split('\r', '\n'))
                    list.Add(str.ToLower());
            }
            return list;
        }
    }
}