// <copyright file="IMyTask.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace MyThreadPool;

/// <summary>
/// Task that will be executed by thread pool.
/// </summary>
/// <typeparam name="TResult">The type of task result.</typeparam>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Gets a value indicating whether the task has completed.
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    /// Gets a value indicating whether the result of the task.
    /// </summary>
    /// <exception cref="AggregateException">Throws when the function corresponding to the task has terminated with an exception.</exception>
    public TResult Result { get; }

    /// <summary>
    /// Applies to the result of a given task and returns a new task accepted for execution.
    /// </summary>
    /// <typeparam name="TNewResult">The type of new result.</typeparam>
    /// <param name="func">A function used to continue work with the result.</param>
    /// <returns>New task for executing.</returns>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
}