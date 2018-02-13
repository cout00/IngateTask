using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using IngateTask.Core.UserAgents;

namespace IngateTask.Core.Parsers
{
    public class RobotsParser
    {
        private const string USER_AGENT_STR = "user-agent";
        private const string CRAWLER_DELAY_STR = "crawl-delay";

        private readonly InputFields _inputFields;
        private LogMessanger _logMessanger;
        private IRequest _request;
        private double intOutValue = DefaultParams.DefaulDelay;

        public RobotsParser(InputFields inputFields, LogMessanger logMessanger, IRequest request)
        {
            _inputFields = inputFields;
            _logMessanger = logMessanger;
            _request = request;
        }

        public RobotsParser()
        {

        }

        private void Parse()
        {
            try
            {
                var test = _request.GetFileFromDomain(_inputFields.Domain);
                if (_inputFields.UserAgent is int)
                {
                    intOutValue = _inputFields.UserAgent;
                    return;
                }
                bool blockFinded = false;
                bool blockGenericFinded = false;
                Func<string, string, string> valueExtracter = (inpStr, patter) => {
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
                    var ruleSubStr = valueExtracter(test[i], USER_AGENT_STR);
                    if (ruleSubStr.Length > 0)
                    {
                        blockFinded = string.Compare(_inputFields.UserAgent, ruleSubStr, true) == 0;
                        blockGenericFinded = string.Compare("*", ruleSubStr, true) == 0;
                        continue;
                    }

                    if (blockFinded || blockGenericFinded)
                    {
                        var ruleValue = valueExtracter(test[i], CRAWLER_DELAY_STR);
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
            finally
            {
                _logMessanger.PostStatusMessage(LogMessages.Warning, $"Domain {_inputFields.Domain} have {intOutValue} delay");
            }
        }

        public KeyValuePair<Uri, IUserAgent> GetResult()
        {
            if (_inputFields.UserAgent is int)
            {
                CustomAgent agent = new CustomAgent();
                agent.GetCrawlDelay = (int)intOutValue;
                return new KeyValuePair<Uri, IUserAgent>(_inputFields.Domain.ToUri(), agent);
            }
            foreach (Type type in Extensions.GetAgentsType())
            {
                if (string.Compare(type.Name, _inputFields.UserAgent, true) == 0)
                {
                    var userAgent = (IUserAgent)Activator.CreateInstance(type);
                    userAgent.GetCrawlDelay = (int)intOutValue;
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
            await Task.Factory.StartNew(() => {
                Parse();
            });
            //await Task.Delay(4000);
        }




    }
}
