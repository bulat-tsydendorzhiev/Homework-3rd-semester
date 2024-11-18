// <copyright file="DimensionsMismatchException.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when dimensions mismatch occurs during matrices multiplying.
/// </summary>
public class DimensionsMismatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"/> class.
    /// </summary>
    public DimensionsMismatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"/> class.
    /// </summary>
    /// <param name="message">Exception Message.</param>
    public DimensionsMismatchException(string message)
        : base(message)
    {
    }
}