// <copyright file="TestClass.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.TestClasses;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using MyNunit.Attributes;

/// <summary>
/// Represents a class that has test attributes on its methods.
/// </summary>
public class TestClass
{
    private readonly List<MethodInfo> _beforeClassMethods;
    private readonly List<MethodInfo> _beforeMethods;
    private readonly List<TestMethod> _testMethods;
    private readonly List<MethodInfo> _afterMethods;
    private readonly List<MethodInfo> _afterClassMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestClass"/> class.
    /// </summary>
    /// <param name="testingClass">Class that has test methods.</param>
    public TestClass(Type testingClass)
    {
        Name = testingClass.Name;

        var methods = testingClass.GetMethods();
        _beforeClassMethods = GetMethodsWithAttribute<BeforeClassAttribute>(methods);
        _beforeMethods = GetMethodsWithAttribute<BeforeAttribute>(methods);
        _afterMethods = GetMethodsWithAttribute<AfterAttribute>(methods);
        _afterClassMethods = GetMethodsWithAttribute<AfterClassAttribute>(methods);

        _testMethods = GetMethodsWithAttribute<TestAttribute>(methods)
            .Select(method =>
            {
                var instance = Activator.CreateInstance(testingClass);

                ArgumentNullException.ThrowIfNull(instance);

                return new TestMethod(instance, method, _beforeMethods, _afterMethods);
            })
            .ToList();
    }

    /// <summary>
    /// Gets the class name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Runs test methods of the class.
    /// </summary>
    /// <returns>The classes' test methods results.</returns>
    public TestClassResult RunTests()
    {
        var result = new ConcurrentBag<TestMethodResult>();

        InvokeClassMethods(_beforeClassMethods);
        Parallel.ForEach(_testMethods, testMethod => result.Add(testMethod.Run()));
        InvokeClassMethods(_afterClassMethods);

        return new TestClassResult(Name, result.ToList(), result.Sum(testResult => testResult.Duration));
    }

    private static void InvokeClassMethods(IEnumerable<MethodInfo> classMethods)
        => Parallel.ForEach(classMethods, classMethod => classMethod.Invoke(null, null));

    private static List<MethodInfo> GetMethodsWithAttribute<T>(IEnumerable<MethodInfo> methods)
        where T : Attribute
            => methods.Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0).ToList();
}
