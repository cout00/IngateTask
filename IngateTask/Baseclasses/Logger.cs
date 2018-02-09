using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngateTask.Baseclasses
{
    public enum LogMessages
    {
        Error,
        Exceptions,
        Warning,
    }

    public abstract class Logger
    {
        protected readonly string _outputPath;
        private object syncObject=new object();
        protected abstract string fileName {get;set;}
        private StreamWriter _streamWriter;

        protected Logger(string outputPath)
        {
            _outputPath = outputPath;
            try
            {
                _streamWriter= File.CreateText(Path.Combine(outputPath, fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Wrong Input {e.Message}");
            }
        }


        void AddToFile(string text)
        {
            //короче тут если пришло важное сообщение то закрываем файл. создаем очередь
            // ловим файл недоступен эксепшен и пишем в эту очередь сообщение. как 
            //только файл станет доступен вытаскиваем из очереди и пишем в него опять
            lock (syncObject)
            {
                _streamWriter.WriteLine(text);
            }
        }


        //async Task AddToFile(string text)
        //{
        //    await _streamWriter.WriteAsync(text);
        //}


        public void AddToLogAsync(string msg)
        {
            AddToFile($"{GetMessageString()} {msg}");
            //task.Wait();
        }

        public async void AddToLogAsync(LogMessages logMessages, string msg)
        {
            await _streamWriter.WriteAsync($"{logMessages.ToString()}: {GetMessageString()} {msg}");
        }

        public void CloseLog()
        {
            _streamWriter.Close();
        }

        protected abstract string GetMessageString();
    }
}
