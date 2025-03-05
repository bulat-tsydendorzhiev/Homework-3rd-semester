// <copyright file="MyTestStatus.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

/// <summary>
/// Identify test method status.
/// </summary>
public enum MyTestStatus
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

    /// <summary>
    /// Identify test whose result was canceled due to exception from "before"/"after" methods.
    /// </summary>
    Canceled = 2,
}