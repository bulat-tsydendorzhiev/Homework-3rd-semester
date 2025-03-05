// <copyright file="MyAssertException.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Assertion;

/// <summary>
/// Throws when the condition is not met.
/// </summary>
public class MyAssertException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyAssertException"/> class.
    /// </summary>
    public MyAssertException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyAssertException"/> class.
    /// </summary>
    /// <param name="message">Message for additional information.</param>
    public MyAssertException(string message)
        : base(message)
    {
    }
}