using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Parsers
{
    public class GrammaHttpParser :IHttpParser
    {
        WebBrowser _webBrowser=new WebBrowser();

        public GrammaHttpParser()
        {
            
        }

        /// <summary>
        /// не рабочий код. винформс может работать только в ста потоке
        /// </summary>
        /// <param name="page"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public IEnumerable<Uri> GetNestedUri(string page, Uri baseUri)
        {
            HtmlDocument document = _webBrowser.Document;
            document.Write(page);
            HtmlElementCollection elementCollection = document.GetElementsByTagName("a");
            for (int i = 0; i < elementCollection.Count; i++)
            {
                if (elementCollection[i].GetAttribute("href").Length!=0)
                {
                    yield return elementCollection[i].GetAttribute("href").ToUri().Merge(baseUri);
                }
            }
            
           
        }
    }
}
