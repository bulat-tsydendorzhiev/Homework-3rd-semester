// <copyright file="TestClassResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of the tests of the class.
/// </summary>
/// <param name="Name">Gets the test name of the class with tests.</param>
/// <param name="TestResults">Gets the results of all tests of class.</param>
/// <param name="TotalDuration">Gets the time spent on all tests in milliseconds.</param>
/// <param name="InvalidMethodsName">Gets the invalid methods of the class.</param>
public record TestClassResult(string Name, IEnumerable<TestMethodResult> TestResults, long TotalDuration, List<string>? InvalidMethodsName = null)
{
}