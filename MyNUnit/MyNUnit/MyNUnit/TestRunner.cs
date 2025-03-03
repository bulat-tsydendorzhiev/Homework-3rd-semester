// <copyright file="TestRunner.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit;

using System.Reflection;
using MyNUnit.TestComponents;

/// <summary>
/// Allows to run tests and make report.
/// </summary>
public static class TestRunner
{
    /// <summary>
    /// Runs tests located in the assembly at the specified path.
    /// </summary>
    /// <param name="path">Path to directory with assemblies.</param>
    public static IEnumerable<TestAssemblyResult> RunTests(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException("There is no directory with such path.");
        }

        var assemblies = Directory.GetFiles(path, "*.dll")
            .Select(fileName => new TestAssembly(Assembly.LoadFrom(fileName)))
            .ToList();

        var result = new List<TestAssemblyResult>();

        Parallel.ForEach(assemblies, assembly => result.Add(assembly.RunTests()));

        return result.ToList();
    }
}
