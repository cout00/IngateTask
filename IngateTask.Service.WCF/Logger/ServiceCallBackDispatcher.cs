using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.Core.Interfaces;

namespace IngateTask.Service.WCF.Logger
{
    public class ServiceCallBackDispatcher :ILogProvider
    {
        private readonly ICrawlerCallBack _crawlerCallBack;
        private readonly ILogStringCombiner _stringCombiner;

        public ServiceCallBackDispatcher(ICrawlerCallBack crawlerCallBack, ILogStringCombiner stringCombiner)
        {
            _crawlerCallBack = crawlerCallBack;
            _stringCombiner = stringCombiner;
        }

        public void SendNonStatusMessage(string msg)
        {
            _crawlerCallBack.OnCallBack(_stringCombiner.GetCombinedString(msg));
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
           _crawlerCallBack.OnCallBack($"{mgsStatus}: {_stringCombiner.GetCombinedString(msg)}");
        }
    }
}
