// <copyright file="TestClass.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using MyNUnit.Attributes;

/// <summary>
/// Represents a class that has test attributes on its methods.
/// </summary>
public class TestClass
{
    private readonly List<MethodInfo> _beforeClassMethods;
    private readonly List<TestMethod> _testMethods;
    private readonly List<MethodInfo> _afterClassMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestClass"/> class.
    /// </summary>
    /// <param name="testingClass">Class that has test methods.</param>
    public TestClass(Type testingClass)
    {
        Name = testingClass.Name;

        var methods = testingClass.GetMethods().ToList();
        _beforeClassMethods = GetMethodsWithAttribute(typeof(BeforeClassAttribute), methods).Where(method => method.IsStatic).ToList();
        var beforeMethods = GetMethodsWithAttribute(typeof(BeforeAttribute), methods);
        var afterMethods = GetMethodsWithAttribute(typeof(AfterAttribute), methods);
        _afterClassMethods = GetMethodsWithAttribute(typeof(AfterClassAttribute), methods).Where(method => method.IsStatic).ToList();

        _testMethods = GetMethodsWithAttribute(typeof(MyTestAttribute), methods)
            .Select(method =>
            {
                var instance = Activator.CreateInstance(testingClass);

                ArgumentNullException.ThrowIfNull(instance);

                return new TestMethod(instance, method, beforeMethods, afterMethods);
            }).ToList();
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

    private static List<MethodInfo> GetMethodsWithAttribute(Type attributeType, IEnumerable<MethodInfo> methods)
        => methods.Where(m => Attribute.IsDefined(m, attributeType)).ToList();
}
