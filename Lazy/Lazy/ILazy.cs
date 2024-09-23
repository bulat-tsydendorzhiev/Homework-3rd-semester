// <copyright file="ILazy.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace Lazy;

/// <summary>
/// An interface used for lazy evaluation.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public interface ILazy<T>
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    /// <exception cref="ArgumentNullException">Throws if supllier's value is null.</exception>
    public T Get();
}
