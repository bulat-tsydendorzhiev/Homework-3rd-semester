// <copyright file="Index.cshtml.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace MyNUnitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// Model for uploading files and .
/// </summary>
public class IndexModel : PageModel
{
    /// <summary>
    /// Gets the name of the directory with assemblies with tests.
    /// </summary>
    public string TestsDirectoryPath => "TestsDirectory";

    public void OnGet()
    {
    }
}
