// <copyright file="DimensionsMismatchException.cs" company="bulat-tsydendorzhiev">
// Copyright (c) bulat-tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when dimensions mismatch occurs during matrices multiplying.
/// </summary>
public class DimensionsMismatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"> exception.
    /// </summary>
    public DimensionsMismatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"> exception.
    /// </summary>
    /// <param name="message">Throwing message.</param>
    public DimensionsMismatchException(string message) : base(message)
    {
    }
}