using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IngateTask.Core.ParallelThread
{
    /// <summary>
    /// контекст для потока
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public class ThreadContext<Type> where Type : class, new()
    {
        public string ThreadKey { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
        public Type AssisatedObject { get; set; }
        public Func<Task> MethodPointer { get; set; }
    }


    public class ParallelQueue<Type> where Type : class, new()
    {
        private readonly int _maxRunningTasks;

        /// <summary>
        ///     очеред которая ждет в данный момент
        /// </summary>
        private readonly ConcurrentQueue<ThreadContext<Type>> _processingQueue =
            new ConcurrentQueue<ThreadContext<Type>>();

        /// <summary>
        ///     соответствие хешь код - и контекста
        /// </summary>
        private readonly ConcurrentDictionary<int, ThreadContext<Type>> _runningTasks =
            new ConcurrentDictionary<int, ThreadContext<Type>>();

        /// <summary>
        ///     фабрика задач которым можно установить результат вручную. удобная вещь
        /// </summary>
        private TaskCompletionSource<bool> _tscQueue = new TaskCompletionSource<bool>();

        public ParallelQueue(int maxRunningTasks)
        {
            _maxRunningTasks = maxRunningTasks;
        }

        public void Queue(ThreadContext<Type> context)
        {
            _processingQueue.Enqueue(context);
        }

        public int GetQueueCount()
        {
            return _processingQueue.Count - GetRunningCount();
        }

        public int GetRunningCount()
        {
            return _runningTasks.Count;
        }
        /// <summary>
        /// нужна чтобы найти токен при вызове методов поддерживающих отмену
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CancellationToken GetTokenByName(string name)
        {
            foreach (KeyValuePair<int, ThreadContext<Type>> task in _runningTasks)
            {
                if (task.Value.ThreadKey == name)
                {
                    return task.Value.TokenSource.Token;
                }
            }
            return CancellationToken.None;
        }

        /// <summary>
        /// отменяет задачу по имени, имена в контексте
        /// </summary>
        /// <param name="name"></param>
        public void CanselTask(string name)
        {
            foreach (KeyValuePair<int, ThreadContext<Type>> task in _runningTasks)
            {
                if (task.Value.ThreadKey == name)
                {
                    task.Value.TokenSource.Cancel();
                }
            }
        }

        public Dictionary<string, Type> GetRunnedTasksName()
        {
            Dictionary<string, Type> temp = new Dictionary<string, Type>();
            foreach (KeyValuePair<int, ThreadContext<Type>> task in _runningTasks)
            {
                temp.Add(task.Value.ThreadKey, task.Value.AssisatedObject);
            }
            return temp;
        }

        public void CancelAll()
        {
            foreach (KeyValuePair<int, ThreadContext<Type>> task in _runningTasks)
            {
                task.Value.TokenSource.Cancel();
            }
        }
        /// <summary>
        /// сама точка входа
        /// </summary>
        /// <returns></returns>
        public async Task Process()
        {
            Task<bool> t = _tscQueue.Task;
            StartTasks();
            await t;
        }

        /// <summary>
        /// способ стар как мир, видел еще в 2015
        /// смысл в том что в фоне запущена большая задача которую взяли из из тасккомплшенсорс.
        /// вытаскиваем метод из контекста и ставим ему продолжение
        /// как только продолжение сработает
        /// оно вернет новый таск в нем метод вызовется опять и все повторится
        /// вконце стоит интерлокед эксченж пусть он тут просто будет
        /// </summary>

        private void StartTasks()
        {
            int startMaxCount = _maxRunningTasks - _runningTasks.Count;
            for (int i = 0; i < startMaxCount; i++)
            {
                ThreadContext<Type> context;
                if (!_processingQueue.TryDequeue(out context))
                {
                    break;
                }
                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                Task t = Task.Run(context.MethodPointer, cancellationToken.Token);
                context.TokenSource = cancellationToken;
                _runningTasks.TryAdd(t.GetHashCode(), context);
                t.ContinueWith(t2 =>
                {
                    ThreadContext<Type> _temp;
                    _runningTasks.TryRemove(t2.GetHashCode(), out _temp);
                    StartTasks();
                });
            }
            if (_processingQueue.IsEmpty && _runningTasks.IsEmpty)
            {
                TaskCompletionSource<bool> _oldQueue = Interlocked.Exchange(
                    ref _tscQueue, new TaskCompletionSource<bool>());
                _oldQueue.TrySetResult(true);
            }
        }
    }
}