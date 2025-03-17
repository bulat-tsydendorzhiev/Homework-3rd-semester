// <copyright file="TestResult.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Data;

/// <summary>
/// Represents the result of the test method.
/// </summary>
public class TestResult
{
    /// <summary>
    /// Gets or sets primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets test run id test result belongs to.
    /// </summary>
    public int TestRunId { get; set; }

    /// <summary>
    /// Gets or sets name of the test method.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets test status.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets duration of the test method running.
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    /// Gets or sets message in case of ignored, failed or cancelled test.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
