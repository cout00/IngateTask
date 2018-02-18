using System.IO;
using IngateTask.Core.Interfaces;
using IngateTask.PortableLibrary.Interfaces;

namespace IngateTask.Core.Loggers
{
    public class FileWriterLogger : ILogProvider
    {
        private readonly string _path;
        private readonly ILogStringCombiner _stringCombiner;
        private readonly object syncObject = new object();
        private StreamWriter _streamWriter;

        public FileWriterLogger(string path, ILogStringCombiner stringCombiner)
        {
            _path = Path.Combine(path, "log.txt");
            _stringCombiner = stringCombiner;
            _streamWriter = File.AppendText(_path);
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