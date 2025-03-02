// <copyright file="BeforeAttribute.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.Attributes;

/// <summary>
/// Identifies a method to be called immediately before each test is run.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class BeforeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeforeAttribute"/> class.
    /// </summary>
    public BeforeAttribute()
    {
    }
}
