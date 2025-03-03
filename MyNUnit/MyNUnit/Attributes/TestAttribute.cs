// <copyright file="TestAttribute.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Attributes;

/// <summary>
/// Marks the method as test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class MyTestAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyTestAttribute"/> class.
    /// </summary>
    /// <param name="ignore">Ignore message to cancel the launch and indicate the reason.</param>
    /// <param name="expected">Exception type that is expected as test result.</param>
    public MyTestAttribute(string? ignore = null, Type? expected = null)
    {
        ExpectedExceptionType = expected;
        IgnoreMessage = ignore;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTestAttribute"/> class.
    /// </summary>
    /// <param name="expected">Exception type that is expected as test result.</param>
    public MyTestAttribute(Type expected)
    {
        ExpectedExceptionType = expected;
    }

    /// <summary>
    /// Gets ignore message.
    /// </summary>
    public string? IgnoreMessage { get; private set; }

    /// <summary>
    /// Gets expected exception type.
    /// </summary>
    public Type? ExpectedExceptionType { get; private set; }
}
