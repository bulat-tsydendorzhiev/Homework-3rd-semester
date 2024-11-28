// <copyright file="MyThreadPool.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace MyThreadPool;

using System.Collections.Concurrent;

/// <summary>
/// Provides a pool of threads that can be used to execute tasks.
/// </summary>
public class MyThreadPool
{
    private readonly Thread[] _threads;

    private readonly CancellationTokenSource _cts = new ();

    private readonly ConcurrentQueue<Action> _remainingTasks = new ();

    private readonly object _lockObject = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads in thread pool.</param>
    /// <exception cref="ArgumentException">Throws when invalid number of threads was given.</exception>
    public MyThreadPool(int numberOfThreads)
    {
        if (numberOfThreads < 1)
        {
            throw new ArgumentException("Number of threads cannot be less that 1.");
        }

        _threads = new Thread[numberOfThreads];

        for (var i = 0; i < numberOfThreads; ++i)
        {
            _threads[i] = new Thread(ExecuteTask) { IsBackground = true };
            _threads[i].Start();
        }
    }

    /// <summary>
    /// Shuts down thread pool work.
    /// New tasks are not allowed, running tasks are allowed to finish their work.
    /// </summary>
    public void Shutdown()
    {
        if (_cts.IsCancellationRequested)
        {
            return;
        }

        _cts.Cancel();

        lock (_lockObject)
        {
            Monitor.PulseAll(_lockObject);
        }

        foreach (var thread in _threads)
        {
            thread.Join();
        }
    }

    /// <summary>
    /// Submits the task.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="task">Executable task.</param>
    /// <returns>A new task.</returns>
    /// <exception cref="InvalidOperationException">Throws when thread pool was shut down.</exception>
    public IMyTask<T> Submit<T>(Func<T> task)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (_cts.IsCancellationRequested)
        {
            throw new InvalidOperationException("Thread pool was shut down.");
        }

        var newTask = new MyTask<T>(task, this);

        SubmitTask(newTask.Run);

        return newTask;
    }

    private void SubmitTask(Action task)
    {
        lock (_lockObject)
        {
            _remainingTasks.Enqueue(task);

            Monitor.Pulse(_lockObject);
        }
    }

    private void ExecuteTask()
    {
        while (!_cts.IsCancellationRequested)
        {
            Action task;

            lock (_lockObject)
            {
                while (!_remainingTasks.TryDequeue(out task!))
                {
                    if (_cts.IsCancellationRequested)
                    {
                        return;
                    }

                    Monitor.Wait(_lockObject);
                }
            }

            task();
        }
    }

    private class MyTask<T> : IMyTask<T>
    {
        private readonly object _taskLockObject = new ();

        private readonly MyThreadPool _threadPool;

        private readonly ConcurrentQueue<Action> _continuations = new ();

        private Exception? _occuredException;

        private Func<T>? _task;

        private T _result;

        public MyTask(Func<T> task, MyThreadPool threadPool)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentNullException.ThrowIfNull(threadPool);

            _task = task;
            _threadPool = threadPool;
        }

        /// <inheritdoc/>
        public bool IsCompleted { get; private set; }

        /// <inheritdoc/>
        public T Result
        {
            get
            {
                lock (_taskLockObject)
                {
                    while (!IsCompleted)
                    {
                        Monitor.Wait(_taskLockObject);
                    }

                    if (_occuredException is not null)
                    {
                        throw new AggregateException(_occuredException);
                    }

                    return _result;
                }
            }
        }

        /// <inheritdoc/>
        public IMyTask<TNew> ContinueWith<TNew>(Func<T, TNew> task)
        {
            ArgumentNullException.ThrowIfNull(task);
            if (_threadPool._cts.IsCancellationRequested)
            {
                throw new InvalidOperationException("Thread pool was shut down");
            }

            lock (_taskLockObject)
            {
                if (IsCompleted)
                {
                    return _threadPool.Submit(() => task(Result));
                }

                var newTask = new MyTask<TNew>(() => task.Invoke(Result), _threadPool);
                _continuations.Enqueue(newTask.Run);

                return newTask;
            }
        }

        /// <summary>
        /// Runs the task.
        /// </summary>
        public void Run()
        {
            lock (_taskLockObject)
            {
                try
                {
                    _result = _task!();
                }
                catch (Exception e)
                {
                    _occuredException = e;
                }
                finally
                {
                    _task = null;
                    IsCompleted = true;

                    Monitor.Pulse(_taskLockObject);

                    SubmitContinuations();
                }
            }
        }

        private void SubmitContinuations()
        {
            foreach (var task in _continuations)
            {
                _threadPool.SubmitTask(task);
            }
        }
    }
}