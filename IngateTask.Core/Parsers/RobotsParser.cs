using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.UserAgents;

namespace IngateTask.Core.Parsers
{
    /// <summary>
    /// парсит роботс тхт файл
    /// </summary>
    public class RobotsParser
    {
        private const string USER_AGENT_STR = "user-agent";
        private const string CRAWLER_DELAY_STR = "crawl-delay";

        private readonly InputFields _inputFields;
        private readonly ILogProvider _logProvider;
        private readonly IRequest _request;
        private double intOutValue = DefaultParams.DefaulDelay;

        public RobotsParser(InputFields inputFields, ILogProvider logProvider, IRequest request)
        {
            _inputFields = inputFields;
            _logProvider = logProvider;
            _request = request;
        }

        public RobotsParser()
        {
        }

        private void Parse()
        {
            try
            {
                if (_inputFields.UserAgent is int)
                {
                    intOutValue = _inputFields.UserAgent;
                    return;
                }
                List<string> test = _request.GetFileFromDomain(_inputFields.Domain);
                bool blockFinded = false;
                bool blockGenericFinded = false;
                Func<string, string, string> valueExtracter = (inpStr, patter) =>
                {
                    if (inpStr.Contains(patter))
                    {
                        string row = inpStr.Replace(patter, "");
                        string newRow = "";
                        for (int j = 0; j < row.Length; j++)
                        {
                            if (!row[j].In(':', ' '))
                            {
                                newRow += row[j];
                            }
                        }
                        return newRow;
                    }
                    return "";
                };

                for (int i = 0; i < test.Count; i++)
                {
                    string ruleSubStr = valueExtracter(test[i], USER_AGENT_STR);
                    if (ruleSubStr.Length > 0)
                    {
                        blockFinded = string.Compare(_inputFields.UserAgent, ruleSubStr, true) == 0;
                        blockGenericFinded = string.Compare("*", ruleSubStr, true) == 0;
                        continue;
                    }

                    if (blockFinded || blockGenericFinded)
                    {
                        string ruleValue = valueExtracter(test[i], CRAWLER_DELAY_STR);
                        if (ruleValue.Length > 0)
                        {
                            double value = 0;
                            if (double.TryParse(ruleValue, out value))
                            {
                                intOutValue = value * 1000;
                            }
                            if (blockFinded)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logProvider.SendStatusMessage(LogMessages.Exceptions,
                    $"Domain {_inputFields.Domain} throw {e.Message}");
            }
            finally
            {
                _logProvider.SendStatusMessage(LogMessages.Warning,
                    $"Domain {_inputFields.Domain} have {intOutValue} delay");
            }
        }

        public KeyValuePair<Uri, IUserAgent> GetResult()
        {
            if (_inputFields.UserAgent is int)
            {
                CustomAgent agent = new CustomAgent();
                agent.GetCrawlDelay = (int) intOutValue;
                return new KeyValuePair<Uri, IUserAgent>(_inputFields.Domain.ToUri(), agent);
            }
            foreach (Type type in Extensions.GetAgentsType())
            {
                if (string.Compare(type.Name, _inputFields.UserAgent, true) == 0)
                {
                    IUserAgent userAgent = (IUserAgent) Activator.CreateInstance(type);
                    userAgent.GetCrawlDelay = (int) intOutValue;
                    return new KeyValuePair<Uri, IUserAgent>(_inputFields.Domain.ToUri(), userAgent);
                }
            }
            return new KeyValuePair<Uri, IUserAgent>();
        }

        public void ParseFile()
        {
            Parse();
        }

        public async Task ParseFileAsync()
        {
            await Task.Factory.StartNew(() => { Parse(); });
            //await Task.Delay(4000);
        }
    }
}