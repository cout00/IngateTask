using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using System.IO;

namespace IngateTask.Core.Parsers
{
    public class RobotsFileStub :IRequest
    {
        public List<string> GetFileFromDomain(string uri)
        {
            List<string> list = new List<string>();
            using (StreamReader stream = new StreamReader(File.OpenRead(uri)))
            {
                foreach (var str in stream.ReadToEnd().Split('\r','\n'))
                {
                    list.Add(str.ToLower());
                }
            }
            return list;
        }
    }
}
