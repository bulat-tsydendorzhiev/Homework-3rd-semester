// <copyright file="TestAssemblyResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.TestClasses;

/// <summary>
/// Defines the result of the classes test methods.
/// </summary>
public record TestAssemblyResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAssemblyResult"/> class.
    /// </summary>
    /// <param name="name">Name of the test class.</param>
    /// <param name="classesTestResults">Results of the tests from this class.</param>
    /// <param name="totalDuration">Total time spent on the tests.</param>
    public TestAssemblyResult(string name, IEnumerable<TestClassResult> classesTestResults, long totalDuration)
    {
        Name = name;
        TestResults = classesTestResults;
        TotalDuration = totalDuration;
    }

    /// <summary>
    /// Gets the test name of assembly with the classes with the tests.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the time spent on all tests of all classes in this assembly in milliseconds.
    /// </summary>
    public long TotalDuration { get; }

    /// <summary>
    /// Gets the status of the test.
    /// </summary>
    public IEnumerable<TestClassResult> TestResults { get; }
}
