// <copyright file="MyAssert.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Assertion;

/// <summary>
/// Represents assertion that should be true.
/// </summary>
public static class MyAssert
{
    /// <summary>
    /// Checks whether condition is met.
    /// </summary>
    /// <param name="condition">Condition that should be met.</param>
    /// <exception cref="MyAssertException">Throws when the condition is not met.</exception>
    public static void That(bool condition)
    {
        if (!condition)
        {
            throw new MyAssertException("Assertion has failed.");
        }
    }
}
