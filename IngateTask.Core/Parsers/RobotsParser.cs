using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;

namespace IngateTask.Core.Parsers
{
    public class RobotsParser
    {
        private readonly InputFields _inputFields;
        private LogMessanger _logMessanger;
        private IRequest _request;

        public RobotsParser(InputFields inputFields, LogMessanger logMessanger, IRequest request)
        {
            _inputFields = inputFields;
            _logMessanger = logMessanger;
            _request = request;
        }

    }
}
