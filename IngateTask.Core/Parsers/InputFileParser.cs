using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using IngateTask.Core.UserAgents;

namespace IngateTask.Core.Parsers
{
    /// <summary>
    ///     тут можно спорить вечно но по рихтеру они не боксятся, сам не проверял
    /// </summary>
    [Serializable]
    public struct InputFields
    {
        public string Domain { get; set; }

        /// <summary>
        ///     памяти и так много
        /// </summary>
        public dynamic UserAgent { get; set; }
    }


    public class InputLocalFileParser
    {
        private readonly ILogProvider _logProvider;
        private readonly string _path;
        private readonly List<InputFields> _fieldses = new List<InputFields>();
        private readonly List<string> allowedUserAgents;

        public InputLocalFileParser(string path, ILogProvider logProvider)
        {
            _path = path;
            _logProvider = logProvider;
            allowedUserAgents = typeof(IUserAgent).GetAssignedType
                (type => type != typeof(IUserAgent) && type != typeof(CustomAgent))
                .Select(type => type.Name.ToUpper())
                .ToList();
        }

        public bool FileIsValid { get; set; } = true;

        public List<InputFields> GetParsedArray()
        {
            var stringArray = new string[1] {" "};
            try
            {
                stringArray = File.ReadAllLines(_path);
            }
            catch (Exception e)
            {
                _logProvider.SendStatusMessage(LogMessages.Error,
                    $"Sorry, path {_path} is wrong try againe {e.Message}");
                return null;
            }
            for (var i = 0; i < stringArray.Length; i++)
            {
                var subString = stringArray[i].Trim().Split(' ');
                if (subString.Length > 2)
                {
                    _logProvider.SendStatusMessage(LogMessages.Error,
                        $"Wrong Parsing at {i + 1} line. Line will be skipped");
                    FileIsValid = false;
                    continue;
                }
                if (subString[0].Last() != '/')
                    subString[0] = subString[0] += '/';
                var tryConvertDelay = 0;
                if (int.TryParse(subString[1], out tryConvertDelay))
                {
                    _fieldses.Add(new InputFields {Domain = subString[0], UserAgent = tryConvertDelay});
                }
                else
                {
                    if (allowedUserAgents.Contains(subString[1].ToUpper()))
                    {
                        _fieldses.Add(new InputFields {Domain = subString[0], UserAgent = subString[1]});
                    }
                    else
                    {
                        _logProvider.SendStatusMessage(LogMessages.Error,
                            $"Wrong Parsing at {i + 1} line. Line will be skipped. {Environment.NewLine}\"{subString[1]}\" not valid User Agent");
                        FileIsValid = false;
                    }
                }
            }
            return _fieldses;
        }
    }
}