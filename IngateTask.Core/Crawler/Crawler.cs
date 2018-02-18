using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.UserAgents;

namespace IngateTask.Core.Crawler
{
    public class Crawler
    {
        private readonly IHttpParser _httpParser;
        private readonly KeyValuePair<Uri, IUserAgent> _inpParams;
        private readonly ILogProvider _logMessanger;
        private readonly List<string> visitedList = new List<string>();
        private string _output;
        public double ReadedBytes { get; private set; } = 0;    

        public Crawler(KeyValuePair<Uri, IUserAgent> inpParams, ILogProvider logMessanger, IHttpParser httpParser,
            string output)
        {
            _inpParams = inpParams;
            _logMessanger = logMessanger;
            _httpParser = httpParser;
            _output = output;
        }

        private IEnumerable<Uri> ParsePage(string page, Uri currentUri)
        {
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

        private void DirectRecursion(Uri nextPage, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                _logMessanger.SendStatusMessage(LogMessages.Warning,
                    $"Domain {_inpParams.Key.OriginalString} crawling was canseled by user");
                return;
            }
            try
            {
                visitedList.Add(nextPage.OriginalString);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(nextPage);
                request.UserAgent = _inpParams.Value.GetUserAgentFullName();
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
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
                            using (StreamWriter outFile = File.CreateText(Path.Combine(_output, nextPage.ToFilePath())))
                            {
                                outFile.WriteAsync(data).Wait();
                                ReadedBytes +=Math.Round((data.Length*(double)sizeof(char))/(1024*1024));
                                //outFile.Close();
                            }
                            readStream.Close();
                            _logMessanger.SendStatusMessage(LogMessages.Event, $"I parsed {nextPage.OriginalString}");
                            foreach (Uri page in ParsePage(data, nextPage))
                            {
                                Task.Delay(_inpParams.Value.GetCrawlDelay).Wait();
                                DirectRecursion(page, token);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logMessanger.SendStatusMessage(LogMessages.Exceptions,
                    $"Crawler exception: {e.Message} domain {nextPage.OriginalString}");
            }
        }


        public async Task CrawAsync(CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    _output = Path.Combine(_output, _inpParams.Key.Host);
                    Directory.CreateDirectory(_output);
                    _logMessanger.SendStatusMessage(LogMessages.Warning,
                        $"Domain {_inpParams.Key.OriginalString} start crawling use -help for more info");
                    DirectRecursion(_inpParams.Key, token);
                    _logMessanger.SendStatusMessage(LogMessages.Warning,
                        $"Domain {_inpParams.Key.OriginalString} Parsed!");
                }
                catch (Exception e)
                {
                    _logMessanger.SendStatusMessage(LogMessages.Exceptions, e.Message);
                }
            });
        }
    }
}