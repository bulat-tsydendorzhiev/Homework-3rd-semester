// <copyright file="Index.cshtml.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnit;
using MyNUnitWeb.Data;

/// <summary>
/// Represents a model for uploading and removing files and test running.
/// </summary>
public class IndexModel(TestRunDbContext context) : PageModel
{
    /// <summary>
    /// Gets the name of the directory with assemblies with tests.
    /// </summary>
    public string TestsDirectoryPath => "TestsDirectory";

    /// <summary>
    /// Upload assembly files with tests to a dedicated tests directory.
    /// </summary>
    /// <param name="files">Assembly files with tests.</param>
    /// <returns>A task representing the upload assembly files completion.</returns>
    public async Task<IActionResult> OnPostUploadAsync(IEnumerable<IFormFile> files)
    {
        await UploadFilesAsync(files);
        return Page();
    }

    /// <summary>
    /// Runs tests located in a tests directory.
    /// </summary>
    /// <returns>A task representing tests run completion.</returns>
    public async Task<IActionResult> OnPostRunAsync()
    {
        if (Directory.GetFiles(TestsDirectoryPath).Length == 0)
        {
            return Page();
        }

        var testsResults = TestRunner.RunTests(TestsDirectoryPath);

        var testRun = new TestRunResult(testsResults);
        await context.TestRunResults.AddAsync(testRun);
        await context.SaveChangesAsync();

        foreach (var testAssemblyResult in testsResults)
        {
            foreach (var testClassResult in testAssemblyResult.ClassesTestResults)
            {
                foreach (var testMethodResult in testClassResult.TestResults)
                {
                    var testResult = new TestResult()
                    {
                        TestRunId = testRun.Id,
                        Name = testMethodResult.Name,
                        Status = (int)testMethodResult.Status,
                        Duration = testMethodResult.Duration,
                        Message = testMethodResult.IgnoreMessage ?? testMethodResult.ErrorMessage ?? string.Empty,
                    };

                    await context.TestResults.AddAsync(testResult);
                }
            }
        }

        await context.SaveChangesAsync();

        return new RedirectResult($"./TestResult?id={testRun.Id}");
    }

    /// <summary>
    /// Removes assembly file from directory.
    /// </summary>
    /// <param name="filePath">Path to the file that should be removed from directory.</param>
    /// <returns>A page without a file with a specialized path.</returns>
    public IActionResult OnPostRemove(string filePath)
    {
        System.IO.File.Delete(filePath);
        return Page();
    }

    private async Task UploadFilesAsync(IEnumerable<IFormFile> files)
    {
        foreach (var file in files)
        {
            var path = Path.Combine(TestsDirectoryPath, file.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}
