// <copyright file="Journal.cshtml.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnitWeb.Data;

/// <summary>
/// Represents a model of a journal of tests running.
/// </summary>
/// <param name="context">Database context with test results.</param>
[BindProperties]
public class JournalModel(TestRunDbContext context)
    : PageModel
{
    /// <summary>
    /// Gets the test runs.
    /// </summary>
    public IList<TestRunResult> TestRuns { get; private set; } = [];

    /// <summary>
    /// Gets the test runs from database.
    /// </summary>
    public void OnGet()
    {
        TestRuns = context.TestRunResults
            .OrderBy(testRun => testRun.Id)
            .ToList();
    }
}
