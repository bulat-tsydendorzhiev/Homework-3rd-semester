// <copyright file="AfterClassAttribute.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.Attributes;

/// <summary
/// Identifies a method that is called once after any child tests are run.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class AfterClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AfterClassAttribute"/> class.
    /// </summary>
    public AfterClassAttribute()
    {
    }
}
