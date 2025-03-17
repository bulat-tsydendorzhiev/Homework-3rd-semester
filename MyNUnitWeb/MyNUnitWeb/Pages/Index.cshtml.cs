// <copyright file="Index.cshtml.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace MyNUnitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// Model for uploading files.
/// </summary>
public class IndexModel : PageModel
{
    /// <summary>
    /// Gets the name of the directory with assemblies with tests.
    /// </summary>
    public string TestsDirectoryPath => "TestsDirectory";

    /// <summary>
    /// Upload assembly files with tests to a dedicated tests directory.
    /// </summary>
    /// <param name="files">Assembly files with tests.</param>
    /// <returns>A task .</returns>
    public async Task<IActionResult> OnPostUploadAsync(IEnumerable<IFormFile> files)
    {
        await UploadFilesAsync(files);
        return Page();
    }

    /// <summary>
    /// Removes assembly file from directory.
    /// </summary>
    /// <param name="filePath">Path to the file that should be removed from directory.</param>
    /// <returns>A task .</returns>
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
