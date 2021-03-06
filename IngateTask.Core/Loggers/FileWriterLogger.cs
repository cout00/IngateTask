﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using System.Collections.Concurrent;

namespace IngateTask.Core.Loggers
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
            _streamWriter = File.AppendText(path);
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
                    case LogMessages.Exceptions:
                        {
                            _streamWriter.WriteLine($"{mgsStatus.ToString()}:{_stringCombiner.GetCombinedString(msg)}");
                            _streamWriter.Close();
                            _streamWriter = File.AppendText(_path);
                            break;
                        }
                    case LogMessages.Event:
                    case LogMessages.Warning:
                        {
                            _streamWriter.WriteLine($"{mgsStatus.ToString()}:{_stringCombiner.GetCombinedString(msg)}");
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
    }
}
