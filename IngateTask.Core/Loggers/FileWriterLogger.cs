using System.IO;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class FileWriterLogger : ILogProvider
    {
        private readonly ILogStringCombiner _stringCombiner;
        private readonly string _path;
        private StreamWriter _streamWriter;
        private readonly object syncObject = new object();

        public FileWriterLogger(string path, ILogStringCombiner stringCombiner)
        {
            _path = path;
            _stringCombiner = stringCombiner;
            _streamWriter = File.AppendText(path);
        }

        public void SendNonStatusMessage(string msg)
        {
            lock (syncObject)
            {
                _streamWriter.WriteLine(_stringCombiner.GetCombinedString(msg));
            }
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
            lock (syncObject)
            {
                switch (mgsStatus)
                {
                    case LogMessages.Error:
                    case LogMessages.Exceptions:
                    {
                        _streamWriter.WriteLine($"{mgsStatus}:{_stringCombiner.GetCombinedString(msg)}");
                        _streamWriter.Close();
                        _streamWriter = File.AppendText(_path);
                        break;
                    }
                    case LogMessages.Event:
                    case LogMessages.Warning:
                    {
                        _streamWriter.WriteLine($"{mgsStatus}:{_stringCombiner.GetCombinedString(msg)}");
                        _streamWriter.Close();
                        _streamWriter = File.AppendText(_path);
                        break;
                    }
                    case LogMessages.Update:
                    {
                        _streamWriter.Close();
                        _streamWriter = File.AppendText(_path);
                        break;
                    }
                }
            }
        }

        public void CloseLogger()
        {
            _streamWriter.Close();
        }
    }
}