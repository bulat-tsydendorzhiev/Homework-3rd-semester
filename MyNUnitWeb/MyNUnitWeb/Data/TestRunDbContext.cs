// <copyright file="TestRunDbContext.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnitWeb.Data;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Test runs results database context.
/// </summary>
/// <param name="options">The options to be used by the database context.</param>
public class TestRunDbContext(DbContextOptions<TestRunDbContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Gets the test run result stored in database.
    /// </summary>
    public DbSet<TestRunResult> TestRunResults => Set<TestRunResult>();

    /// <summary>
    /// Gets the test method result stored in database.
    /// </summary>
    public DbSet<TestResult> TestResults => Set<TestResult>();
}