using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Parsers
{
    public class GrammaHttpParser : IHttpParser
    {
        private readonly WebBrowser _webBrowser = new WebBrowser();

        /// <summary>
        ///     не рабочий код. винформс может работать только в ста потоке
        /// </summary>
        /// <param name="page"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public IEnumerable<Uri> GetNestedUri(string page, Uri baseUri)
        {
            var document = _webBrowser.Document;
            document.Write(page);
            var elementCollection = document.GetElementsByTagName("a");
            for (var i = 0; i < elementCollection.Count; i++)
                if (elementCollection[i].GetAttribute("href").Length != 0)
                    yield return elementCollection[i].GetAttribute("href").ToUri().Merge(baseUri);
        }
    }
}