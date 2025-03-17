// <copyright file="TestResult.cshtml.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnitWeb.Data;

/// <summary>
/// Represents a model of a result of the tests.
/// </summary>
/// <param name="context">Database context with test results.</param>
[BindProperties]
public class TestResultModel(TestRunDbContext context)
    : PageModel
{
    /// <summary>
    /// Gets the tests result.
    /// </summary>
    public IList<TestResult> Tests { get; private set; } = [];

    /// <summary>
    /// Gets the test result from database by its id.
    /// </summary>
    /// <param name="id">Id of the test in the database.</param>
    public void OnGet(int id)
    {
        Tests = context.TestResults
            .OrderBy(test => test.Id)
            .Where(test => test.TestRunId == id)
            .ToList();
    }
}
