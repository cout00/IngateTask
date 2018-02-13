using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using System.Net;
using System.IO;
using System.Threading;
using System.Web;

namespace IngateTask.Core.Crawler
{
    public class Crawler
    {
        private readonly KeyValuePair<Uri, IUserAgent> _inpParams;
        private readonly LogMessanger _logMessanger;
        private readonly IHttpParser _httpParser;
        private readonly string _output;
        List<string> visitedList = new List<string>();

        public Crawler(KeyValuePair<Uri, IUserAgent> inpParams, LogMessanger logMessanger, IHttpParser httpParser, string output)
        {
            _inpParams = inpParams;
            _logMessanger = logMessanger;
            _httpParser = httpParser;
            _output = output;
        }

        IEnumerable<Uri> ParsePage(string page, Uri currentUri)
        {
            if (currentUri.OriginalString.Contains("jpg"))
            {
                var i = 0;
            }
            foreach (Uri uri in _httpParser.GetNestedUri(page, currentUri))
            {
                if (!visitedList.Contains(uri.OriginalString)                    
                    && uri.UriHaveSameDomens(_inpParams.Key)
                    && !uri.UriIsAnchorOrRedirect())
                {
                    yield return uri;
                }
            }


        }

        private void DirectRecursion(Uri nextPage)
        {            
            try
            {
                visitedList.Add(nextPage.OriginalString);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(nextPage);
                request.UserAgent = _inpParams.Value.GetUserAgentFullName();
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentType.IsSubStringOf(DefaultParams.MEMETextList.ToArray()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {

                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;
                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream =
                                    new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }
                            string data = readStream.ReadToEnd();
                            readStream.Close();
                            _logMessanger.PostStatusMessage(LogMessages.Event, $"I parsed {nextPage.OriginalString}");
                            foreach (var page in ParsePage(data, nextPage))
                            {
                                Task.Delay(_inpParams.Value.GetCrawlDelay).Wait();
                                DirectRecursion(page);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logMessanger.PostStatusMessage(LogMessages.Exceptions,
                    $"Crawler exception: {e.Message} domain {nextPage.OriginalString}");
            }
        }


        public async Task CrawAsync()
        {
            await Task.Run(() =>
            {
                //Directory.CreateDirectory("");

                DirectRecursion(_inpParams.Key);
                _logMessanger.PostStatusMessage(LogMessages.Event,
                    $"Domain {_inpParams.Key.OriginalString} Parsed!");
            });
        }










    }
}
