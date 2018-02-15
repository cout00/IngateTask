using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;

namespace IngateTask.Core.Crawler
{
    public class Crawler
    {
        private readonly IHttpParser _httpParser;
        private readonly KeyValuePair<Uri, IUserAgent> _inpParams;
        private readonly LogMessanger _logMessanger;
        private string _output;
        private readonly List<string> visitedList = new List<string>();

        public Crawler(KeyValuePair<Uri, IUserAgent> inpParams, LogMessanger logMessanger, IHttpParser httpParser,
            string output)
        {
            _inpParams = inpParams;
            _logMessanger = logMessanger;
            _httpParser = httpParser;
            _output = output;
        }

        private IEnumerable<Uri> ParsePage(string page, Uri currentUri)
        {
            foreach (var uri in _httpParser.GetNestedUri(page, currentUri))
                if (!visitedList.Contains(uri.OriginalString)
                    && uri.UriHaveSameDomens(_inpParams.Key)
                    && !uri.UriIsAnchorOrRedirect())
                    yield return uri;
        }

        private void DirectRecursion(Uri nextPage, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;
            try
            {
                visitedList.Add(nextPage.OriginalString);
                var request = (HttpWebRequest) WebRequest.Create(nextPage);
                request.UserAgent = _inpParams.Value.GetUserAgentFullName();
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.ContentType.IsSubStringOf(DefaultParams.MEMETextList.ToArray()))
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;
                            if (response.CharacterSet == null)
                                readStream = new StreamReader(receiveStream);
                            else
                                readStream =
                                    new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            var data = readStream.ReadToEnd();
                            using (var outFile = File.CreateText(Path.Combine(_output, nextPage.ToFilePath())))
                            {
                                outFile.WriteAsync(data).Wait();
                                //outFile.Close();
                            }
                            readStream.Close();
                            _logMessanger.PostStatusMessage(LogMessages.Event, $"I parsed {nextPage.OriginalString}");
                            foreach (var page in ParsePage(data, nextPage))
                            {
                                Task.Delay(_inpParams.Value.GetCrawlDelay).Wait();
                                DirectRecursion(page, token);
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


        public async Task CrawAsync(CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    _output = Path.Combine(_output, _inpParams.Key.Host);
                    Directory.CreateDirectory(_output);
                    DirectRecursion(_inpParams.Key, token);
                    _logMessanger.PostStatusMessage(LogMessages.Event,
                        $"Domain {_inpParams.Key.OriginalString} Parsed!");
                }
                catch (Exception e)
                {
                    _logMessanger.PostStatusMessage(LogMessages.Exceptions, e.Message);
                }
            });
        }
    }
}