// <copyright file="InvalidMatrixException.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when incorrect matrix or its data was given from file.
/// </summary>
public class InvalidMatrixException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMatrixException"/> class.
    /// </summary>
    public InvalidMatrixException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMatrixException"/> class.
    /// </summary>
    /// <param name="message">Exception Message.</param>
    public InvalidMatrixException(string message)
        : base(message)
    {
    }
}