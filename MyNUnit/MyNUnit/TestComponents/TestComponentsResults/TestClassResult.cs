// <copyright file="TestClassResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Defines the result of the tests of the class.
/// </summary>
public record TestClassResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestClassResult"/> class.
    /// </summary>
    /// <param name="name">Name of the test class.</param>
    /// <param name="testResults">Results of the tests from this class.</param>
    /// <param name="totalDuration">Total time spent on the tests.</param>
    public TestClassResult(string name, IEnumerable<TestMethodResult> testResults, long totalDuration)
    {
        Name = name;
        TestResults = testResults;
        TotalDuration = totalDuration;
    }

    /// <summary>
    /// Gets the test name of the class with tests.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the time spent on all tests in milliseconds.
    /// </summary>
    public long TotalDuration { get; private set; }

    /// <summary>
    /// Gets the status of the test.
    /// </summary>
    public IEnumerable<TestMethodResult> TestResults { get; private set; }
}