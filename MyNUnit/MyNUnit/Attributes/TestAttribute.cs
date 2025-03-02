// <copyright file="TestAttribute.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.Attributes;

/// <summary>
/// Marks the method as test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class TestAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    /// <param name="expected">Exception that is expected as test result.</param>
    /// <param name="ignore">Ignore message to cancel the launch and indicate the reason.</param>
    public TestAttribute(Type? expected = null, string? ignore = null)
    {
        ExpectedException = expected;
        IgnoreMessage = ignore;
    }

    /// <summary>
    /// Gets expected exception.
    /// </summary>
    public Type? ExpectedException { get; }

    /// <summary>
    /// Gets ignore message.
    /// </summary>
    public string? IgnoreMessage { get; }
}
