// <copyright file="TestStatus.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.TestClasses;

/// <summary>
/// Identify test method status.
/// </summary>
public enum TestStatus
{
    /// <summary>
    /// Identify ignored test.
    /// </summary>
    Ignored = -1,

    /// <summary>
    /// Identify passed test.
    /// </summary>
    Passed = 0,

    /// <summary>
    /// Identify failed test.
    /// </summary>
    Failed = 1,
}