// <copyright file="BeforeClassAttribute.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Attributes;

/// <summary>
/// Identifies a method that is called once before any child tests are run.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class BeforeClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeforeClassAttribute"/> class.
    /// </summary>
    public BeforeClassAttribute()
    {
    }
}
