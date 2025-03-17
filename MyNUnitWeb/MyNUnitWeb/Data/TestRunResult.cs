// <copyright file="TestRunResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Data;

using MyNUnit.TestComponents;

/// <summary>
/// Represents the result of the test run.
/// </summary>
public class TestRunResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunResult"/> class.
    /// </summary>
    public TestRunResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunResult"/> class.
    /// </summary>
    /// <param name="testAssemblyResults">Results of the test methods.</param>
    public TestRunResult(IEnumerable<TestAssemblyResult> testAssemblyResults)
    {
        foreach (var testAssemblyResult in testAssemblyResults)
        {
            TotalDuration += testAssemblyResult.TotalDuration;
            foreach (var testClassResult in testAssemblyResult.ClassesTestResults)
            {
                foreach (var testResult in testClassResult.TestResults)
                {
                    ++NumberOfTests;
                    switch (testResult.Status)
                    {
                        case MyTestStatus.Passed:
                            ++NumberOfPassedTests;
                            break;
                        case MyTestStatus.Failed:
                            ++NumberOfFailedTests;
                            break;
                        case MyTestStatus.Ignored:
                            ++NumberOfIgnoredTests;
                            break;
                        case MyTestStatus.Cancelled:
                            ++NumberOfIgnoredTests;
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets total duration of all tests in a run.
    /// </summary>
    public long TotalDuration { get; set; }

    /// <summary>
    /// Gets or sets the number of tests in a run.
    /// </summary>
    public int NumberOfTests { get; set; }

    /// <summary>
    /// Gets or sets the number of the passed tests in a run.
    /// </summary>
    public int NumberOfPassedTests { get; set; }

    /// <summary>
    /// Gets or sets the number of the failed tests in a run.
    /// </summary>
    public int NumberOfFailedTests { get; set; }

    /// <summary>
    /// Gets or sets the number of the ignored tests in a run.
    /// </summary>
    public int NumberOfIgnoredTests { get; set; }

    /// <summary>
    /// Gets or sets the number of the cancelled tests in a run.
    /// </summary>
    public int NumberOfCancelledTests { get; set; }
}
