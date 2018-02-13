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
        private readonly ConcurrentQueue<Func<Task>> _processingQueue = new ConcurrentQueue<Func<Task>>();
        private readonly ConcurrentDictionary<int, Task> _runningTasks = new ConcurrentDictionary<int, Task>();
        private readonly int _maxRunningTasks;
        private TaskCompletionSource<bool> _tscQueue = new TaskCompletionSource<bool>();

        public ParallelQueue(int maxRunningTasks)
        {
            _maxRunningTasks = maxRunningTasks;
        }

        public void Queue(Func<Task> futureTask)
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
                Func<Task> futureTask;
                if (!_processingQueue.TryDequeue(out futureTask))
                {
                    break;
                }
                var t = Task.Run(futureTask);
                _runningTasks.TryAdd(t.GetHashCode(), t);
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
