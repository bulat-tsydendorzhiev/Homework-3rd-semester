// <copyright file="TestMethodResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of a test method.
/// </summary>
public record TestMethodResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestMethodResult"/> class.
    /// </summary>
    /// <param name="name">Name of the test method.</param>
    /// <param name="status">Status of the test.</param>
    /// <param name="duration">Duration test was running.</param>
    /// <param name="ignoreMessage">Message that is actual if test is ignored.</param>
    /// <param name="errorMessage">Message that is actual if test is failed.</param>
    public TestMethodResult(string name, MyTestStatus status, long duration, string? ignoreMessage = null, string? errorMessage = null)
    {
        Name = name;
        Status = status;
        Duration = duration;
        IgnoreMessage = ignoreMessage;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Gets the test name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the status of the test.
    /// </summary>
    public MyTestStatus Status { get; private set; }

    /// <summary>
    /// Gets the time spent on the test in milliseconds.
    /// </summary>
    public long Duration { get; private set; }

    /// <summary>
    /// Gets the message why test was ignored.
    /// </summary>
    public string? IgnoreMessage { get; private set; }

    /// <summary>
    /// Gets the message about error in the test.
    /// </summary>
    public string? ErrorMessage { get; private set; }
}