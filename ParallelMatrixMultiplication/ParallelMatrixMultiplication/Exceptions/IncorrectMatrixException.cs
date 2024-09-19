// <copyright file="IncorrectMatrixException.cs" company="bulat-tsydendorzhiev">
// Copyright (c) bulat-tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when incorrect matrix or its data was given from file.
/// </summary>
public class IncorrectMatrixException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectMatrixException"> class.
    /// </summary>
    public IncorrectMatrixException()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectMatrixException"> class.
    /// </summary>
    public IncorrectMatrixException(string message) : base(message)
    {
    }
}