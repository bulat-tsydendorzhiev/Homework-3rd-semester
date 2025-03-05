// <copyright file="TestAssemblyResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of the classes test methods.
/// </summary>
/// <param name="Name">Gets the test name of assembly with the classes with the tests.</param>
/// <param name="ClassesTestResults">Gets the results of all tests of all classes in this assembly.</param>
/// <param name="TotalDuration">Gets the time spent on all tests of all classes in this assembly in milliseconds.</param>
public record TestAssemblyResult(string Name, IEnumerable<TestClassResult> ClassesTestResults, long TotalDuration)
{
}
