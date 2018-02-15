using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IngateTask.Core.ParallelQueue
{
    public class ParallelQueue
    {
        private readonly ConcurrentQueue<KeyValuePair<string, Func<Task>>> _processingQueue = new ConcurrentQueue<KeyValuePair<string, Func<Task>>>();
        private readonly ConcurrentDictionary<int, Task> _runningTasks = new ConcurrentDictionary<int, Task>();
        private readonly ConcurrentDictionary<int,KeyValuePair<string, CancellationTokenSource>> _cancellationTokens=new ConcurrentDictionary<int, KeyValuePair<string, CancellationTokenSource>>();
        private readonly int _maxRunningTasks;
        private TaskCompletionSource<bool> _tscQueue = new TaskCompletionSource<bool>();

        public ParallelQueue(int maxRunningTasks)
        {
            _maxRunningTasks = maxRunningTasks;
        }

        public void Queue(KeyValuePair<string, Func<Task>> futureTask)
        {
           _processingQueue.Enqueue(futureTask);
        }

        public int GetQueueCount()
        {
            return _processingQueue.Count;
        }

        public int GetRunningCount()
        {
            return _runningTasks.Count;
        }

        public CancellationToken GetTokenByName(string name)
        {
            foreach (var token in _cancellationTokens)
            {
                if ( token.Value.Key==name)
                {
                    return token.Value.Value.Token;
                }
            }
            return CancellationToken.None;
        }

        public void CanselTask(string name)
        {
            foreach (var token in _cancellationTokens)
            {
                if (token.Value.Key == name)
                {
                    token.Value.Value.Cancel();
                }
            }
        }

        public async Task Process()
        {
            var t = _tscQueue.Task;
            StartTasks();
            await t;
        }

        private void StartTasks()
        {
            var startMaxCount = _maxRunningTasks - _runningTasks.Count;
            for (int i = 0; i < startMaxCount; i++)
            {
                KeyValuePair<string, Func<Task>> futureTask;
                if (!_processingQueue.TryDequeue(out futureTask))
                {
                    break;
                }
                CancellationTokenSource cancellationToken=new CancellationTokenSource();
                var t = Task.Run(futureTask.Value, cancellationToken.Token);                
                _runningTasks.TryAdd(t.GetHashCode(), t);
                _cancellationTokens.TryAdd(t.GetHashCode(),
                    new KeyValuePair<string, CancellationTokenSource>(futureTask.Key, cancellationToken));
                t.ContinueWith((t2) =>
                {
                    Task _temp;
                    _runningTasks.TryRemove(t2.GetHashCode(), out _temp);
                    StartTasks();
                });
            }
            if (_processingQueue.IsEmpty && _runningTasks.IsEmpty)
            {
                var _oldQueue = Interlocked.Exchange(
                    ref _tscQueue, new TaskCompletionSource<bool>());
                _oldQueue.TrySetResult(true);
            }
        }
    }

}
