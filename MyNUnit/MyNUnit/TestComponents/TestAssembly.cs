// <copyright file="TestAssembly.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

using System.Collections.Concurrent;
using System.Reflection;

/// <summary>
/// Represents an assembly that has classes with test attributes on their methods.
/// </summary>
public class TestAssembly
{
    private List<TestClass> _testClasses;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAssembly"/> class.
    /// </summary>
    /// <param name="assembly">Assembly where should be classes with test attributes.</param>
    public TestAssembly(Assembly assembly)
    {
        Name = assembly.FullName!;
        _testClasses = assembly.ExportedTypes
            .Where(t => t.IsClass)
            .Select(c => new TestClass(c))
            .ToList();
    }

    /// <summary>
    /// Gets the assembly name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Runs tests for each assembly.
    /// </summary>
    /// <returns>The assemblies test methods results.</returns>
    public TestAssemblyResult RunTests()
    {
        var result = new ConcurrentBag<TestClassResult>();

        Parallel.ForEach(_testClasses, testClass => result.Add(testClass.RunTests()));

        return new TestAssemblyResult(Name, result.ToList(), result.Sum(testClassResult => testClassResult.TotalDuration));
    }
}
