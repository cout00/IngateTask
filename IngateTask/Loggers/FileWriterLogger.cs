using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Interfaces;
using System.Collections.Concurrent;

namespace IngateTask.Loggers
{
    public class FileWriterLogger :ILogProvider
    {
        private string _path;
        private readonly ILogStringCombiner _stringCombiner;
        private StreamWriter _streamWriter;
        private object syncObject = new object();

        public FileWriterLogger(string path, ILogStringCombiner stringCombiner)
        {
            _path = path;
            _stringCombiner = stringCombiner;
            _streamWriter = File.CreateText(path);
        }

        public void SendNonStatusMessage(string msg)
        {
            lock (syncObject)
            {
                _streamWriter.WriteLine(_stringCombiner.GetCombinedString(msg));
            }
        }

        public void CloseLogger()
        {
            _streamWriter.Close();
        }

        public void SendStatusMessage(LogMessages mgsStatus, string msg)
        {
            lock (syncObject)
            {
                switch (mgsStatus)
                {
                    case LogMessages.Error:
                        {
                            _streamWriter.WriteLine($"{mgsStatus.ToString()}:{_stringCombiner.GetCombinedString(msg)}");
                            _streamWriter.Close();
                            Task.Delay(50);
                            _streamWriter = File.AppendText(_path);
                            break;
                        }
                    case LogMessages.Exceptions:
                    case LogMessages.Warning:
                        {
                            _streamWriter.WriteLine($"{mgsStatus.ToString()}:{_stringCombiner.GetCombinedString(msg)}");
                            break;
                        }
                    case LogMessages.Update:
                        {
                            _streamWriter.Close();
                            Task.Delay(50);
                            _streamWriter = File.AppendText(_path);
                            break;
                        }
                }
            }
        }
    }
}
