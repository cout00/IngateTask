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
    /// <summary>
    /// сам паук
    /// </summary>
    public class Crawler
    {
        private readonly IHttpParser _httpParser;
        private readonly KeyValuePair<Uri, IUserAgent> _inpParams;
        private readonly ILogProvider _logMessanger;
        private readonly List<string> visitedList = new List<string>();
        private string _output;

        public Crawler(KeyValuePair<Uri, IUserAgent> inpParams, ILogProvider logMessanger, IHttpParser httpParser,
            string output)
        {
            _inpParams = inpParams;
            _logMessanger = logMessanger;
            _httpParser = httpParser;
            _output = output;
        }

        public Crawler()
        {
        }

        public double ReadedMbytes { get; private set; }
        public double SavedPages { get; private set; }

        private IEnumerable<Uri> ParsePage(string page, Uri currentUri)
        {
            foreach (Uri uri in _httpParser.GetNestedUri(page, currentUri))
            {
                //фильтр на под домен то что мы уже были на это ссылке и то что ссылка не анкор и нередирект
                if (!visitedList.Contains(uri.OriginalString)
                    && uri.UriHaveSameDomens(_inpParams.Key)
                    && !uri.UriIsAnchorOrRedirect())
                {
                    yield return uri;
                }
            }
        }
        /// <summary>
        /// нисходящая рекурсия. почему нисходящая? смотрим как работает елд он тут повсюду
        /// </summary>
        /// <param name="nextPage"></param>
        /// <param name="token"></param>
        private void DirectRecursion(Uri nextPage, CancellationToken token)
        {
            try
            {
                visitedList.Add(nextPage.OriginalString);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(nextPage);
                request.UserAgent = _inpParams.Value.GetUserAgentFullName();
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    //список всех МИМЕ типов тут 
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
                                SavedPages++;
                                ReadedMbytes += data.Length * (double) sizeof(char) / (1024 * 1024);
                            }
                            readStream.Close();
                            _logMessanger.SendStatusMessage(LogMessages.Event, $"I parsed {nextPage.OriginalString}");
                            foreach (Uri page in ParsePage(data, nextPage))
                            {
                                Task.Delay(_inpParams.Value.GetCrawlDelay).Wait();
                                // для тех кто только заходит в нору за кроликом
                                if (token.IsCancellationRequested)
                                {
                                    return;
                                }
                                DirectRecursion(page, token);
                                // для тех кто его поймал
                                if (token.IsCancellationRequested)
                                {
                                    return;
                                }
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


        /// <summary>
        /// тут все понятно, едим токен и погнали
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
                    if (token.IsCancellationRequested)
                    {
                        _logMessanger.SendStatusMessage(LogMessages.Warning,
                            $"crawlin Domain {_inpParams.Key.OriginalString} was aborted");
                    }
                    else
                    {
                        _logMessanger.SendStatusMessage(LogMessages.Warning,
                            $"Domain {_inpParams.Key.OriginalString} Parsed!");
                    }
                }
                catch (Exception e)
                {
                    _logMessanger.SendStatusMessage(LogMessages.Exceptions, e.Message);
                }
            });
        }
    }
}