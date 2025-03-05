// <copyright file="TestMethodResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of a test method.
/// </summary>
/// <param name="Name">Test name.</param>
/// <param name="Status">Status of the test.</param>
/// <param name="Duration">Time spent on the test in milliseconds.</param>
/// <param name="IgnoreMessage">Message why test was ignored.</param>
/// <param name="ErrorMessage">Message about error in the test.</param>
public record TestMethodResult(string Name, MyTestStatus Status, long Duration, string? IgnoreMessage = null, string? ErrorMessage = null)
{
    /// <summary>
    /// Gets or sets the test name.
    /// </summary>
    public string Name { get; set; } = Name;

    /// <summary>
    /// Gets or sets the status of the test.
    /// </summary>
    public MyTestStatus Status { get; set; } = Status;

    /// <summary>
    /// Gets or sets the time spent on the test in milliseconds.
    /// </summary>
    public long Duration { get; set; } = Duration;

    /// <summary>
    /// Gets or sets the message why test was ignored.
    /// </summary>
    public string? IgnoreMessage { get; set; } = IgnoreMessage;

    /// <summary>
    /// Gets or sets the message about error in the test.
    /// </summary>
    public string? ErrorMessage { get; set; } = ErrorMessage;
}