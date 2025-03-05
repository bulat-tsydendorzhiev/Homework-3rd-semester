// <copyright file="TestMethodResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of a test method.
/// </summary>
/// <param name="Name">Gets the test name.</param>
/// <param name="Status">Gets the status of the test.</param>
/// <param name="Duration">Gets the time spent on the test in milliseconds.</param>
/// <param name="IgnoreMessage">Gets the message why test was ignored.</param>
/// <param name="ErrorMessage">Gets the message about error in the test.</param>
public record TestMethodResult(string Name, MyTestStatus Status, long Duration, string? IgnoreMessage = null, string? ErrorMessage = null)
{
}