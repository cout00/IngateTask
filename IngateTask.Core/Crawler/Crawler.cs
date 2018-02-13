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
        List<string> visitedList = new List<string>();
        

        public Crawler(KeyValuePair<Uri, IUserAgent> inpParams, LogMessanger logMessanger, IHttpParser httpParser)
        {
            _inpParams = inpParams;
            _logMessanger = logMessanger;
            _httpParser = httpParser;
        }

        IEnumerable<Uri> ParsePage(string page, Uri currentUri)
        {
            foreach (Uri uri in _httpParser.GetNestedUri(page,currentUri))
            {
                if (!visitedList.Contains(uri.OriginalString)
                    && !uri.OriginalString.In(DefaultParams.MemeNonTextList.ToArray())
                    && !uri.IsSubDomain()
                    && uri.UriHaveSameDomens(_inpParams.Key))
                {
                    yield return uri;
                }
            }

            
        }

        //^([A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?){2,})$

        private async Task<Uri>  DirectRecursion(Uri nextPage)
        {
            try
            {
                visitedList.Add(nextPage.OriginalString);                        
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(nextPage);
                request.UserAgent = _inpParams.Value.GetUserAgentFullName();
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {                   
                    await Task.Delay(_inpParams.Value.GetCrawlDelay);
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
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }
                            string data = readStream.ReadToEnd();
                            _logMessanger.PostStatusMessage(LogMessages.Event, $"I parsed {nextPage.OriginalString}");
                            foreach (var page in ParsePage(data, nextPage))
                            {
                                await DirectRecursion(page);
                            }
                            readStream.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logMessanger.PostStatusMessage(LogMessages.Exceptions, 
                    $"Crawler exception: {e.Message} domain {nextPage.OriginalString}");
            }
            return null;
        }


        public async Task CrawAsync()
        {
           await Task.Run(async () =>
            {
               await DirectRecursion(_inpParams.Key);
            });
        }










    }
}
