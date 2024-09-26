// <copyright file="SingleThreadedLazy.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace Lazy;

/// <summary>
/// A single-threaded implementation of the <see cref="ILazy"/> interface.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class SingleThreadedLazy<T> : ILazy<T>
{
    private Func<T>? _supplier;

    private bool _isValueCreated = false;

    private T? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleThreadedLazy"/> class.
    /// </summary>
    /// <param name="func">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
    /// <exception cref="ArgumentNullException">Throws if <see cref="func"/> is null.</exception>
    public SingleThreadedLazy(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        _supplier = func;
    }

    /// <inheritdoc/>
    public T Get()
    {
        if (!_isValueCreated)
        {
            _value = _supplier();
            _isValueCreated = true;
            _supplier = null;
        }

        return _value ?? throw new ArgumentNullException("The created value is null.");
    }
}