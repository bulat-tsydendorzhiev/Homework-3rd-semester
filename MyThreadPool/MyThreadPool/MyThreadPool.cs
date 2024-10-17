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

    private readonly CancellationTokenSource _cts;

    private readonly ConcurrentQueue<Action> _tasks;

    private readonly object _lockObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class.
    /// </summary>
    /// <param name="threadsAmount">Amount of threads in thread pool.</param>
    /// <exception cref="ArgumentException">Throws when invalid amount of threads was given.</exception>
    public MyThreadPool(int threadsAmount)
    {
        if (threadsAmount < 1)
        {
            throw new ArgumentException("Amount of threads cannot be less that 1.");
        }

        _cts = new ();
        _tasks = new ();
        _lockObject = new ();
        _threads = new Thread[threadsAmount];

        for (var i = 0; i < threadsAmount; ++i)
        {
            _threads[i] = new Thread(ExecuteTask) { IsBackground = true };
            _threads[i].Start();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="func">Executable task.</param>
    /// <returns>A new task.</returns>
    /// <exception cref="InvalidOperationException">Throws when thread pool was shut down.</exception>
    public IMyTask<T> Submit<T>(Func<T> func)
    {
        if (_cts.IsCancellationRequested)
        {
            throw new InvalidOperationException("Thread pool was shut down.");
        }

        lock (_lockObject)
        {
            var task = new MyTask<T>(func, this);

            return task;
        }
    }

    /// <summary>
    /// Shuts down threads work.
    /// </summary>
    public void Shutdown()
    {
        if (_cts.Token.IsCancellationRequested)
        {
            return;
        }

        _cts.Cancel();

        foreach (var thread in _threads)
        {
            thread.Join();
        }
    }

    private void ExecuteTask()
    {
        
    }

    private class MyTask<T> : IMyTask<T>
    {
        private readonly object _lockObject;

        private readonly MyThreadPool _threadPool;

        private Exception? _exception;

        private Func<T>? _supplier;

        private T _result;

        public MyTask(Func<T> supplier, MyThreadPool threadPool)
        {
            _supplier = supplier;
            _threadPool = threadPool;
        }

        /// <inheritdoc/>
        public bool IsCompleted { get; private set; }

        /// <inheritdoc/>
        public T Result
        {
            get
            {
                if (_threadPool._cts.Token.IsCancellationRequested)
                {
                    throw new InvalidOperationException();
                }

                if (_exception is not null)
                {
                    throw new AggregateException(_exception);
                }

                return _result;
            }
        }

        /// <inheritdoc/>
        public IMyTask<TNew> ContinueWith<TNew>(Func<T, TNew> func)
        {
            
        }

        private void ExecuteNewTask()
        {
            try
            {
                _result = _supplier();
            }
            catch (Exception e)
            {
                _exception = e;
            }
            finally
            {
                _supplier = null;
                IsCompleted = true;
            }
        }
    }
}