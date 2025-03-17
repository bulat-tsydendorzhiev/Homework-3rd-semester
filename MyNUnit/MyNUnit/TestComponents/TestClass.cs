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
    private readonly List<TestMethod> _testMethods = [];
    private readonly List<MethodInfo> _afterClassMethods;

    private readonly List<string> _invalidMethods = [];

    private readonly List<TestMethodResult> _cancelledResult = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TestClass"/> class.
    /// </summary>
    /// <param name="testingClass">Class that has test methods.</param>
    public TestClass(Type testingClass)
    {
        Name = testingClass.Name;

        var methods = testingClass.GetMethods().ToList();

        _beforeClassMethods = GetMethodsWithAttribute(typeof(BeforeClassAttribute), methods);
        var beforeMethods = GetMethodsWithAttribute(typeof(BeforeAttribute), methods);
        var testMethods = GetMethodsWithAttribute(typeof(MyTestAttribute), methods);
        var afterMethods = GetMethodsWithAttribute(typeof(AfterAttribute), methods);
        _afterClassMethods = GetMethodsWithAttribute(typeof(AfterClassAttribute), methods);

        CheckInvalidMethods(_beforeClassMethods);
        CheckInvalidMethods(beforeMethods);
        CheckInvalidMethods(testMethods);
        CheckInvalidMethods(afterMethods);
        CheckInvalidMethods(_afterClassMethods);

        CheckInvalidTestMethods(testMethods);
        CheckInvalidClassMethods(_beforeClassMethods);
        CheckInvalidClassMethods(_afterClassMethods);

        if (_invalidMethods.Count != 0)
        {
            return;
        }

        try
        {
            InvokeClassMethods(_beforeClassMethods);
        }
        catch (Exception e)
        {
            var errorMessage = $"Test was cancelled due to \"BeforeClass\" method's {e.InnerException?.InnerException?.GetType()}";
            _cancelledResult = testMethods.Select(method => new TestMethodResult(method.Name, MyTestStatus.Cancelled, 0, ErrorMessage: errorMessage)).ToList();
        }

        if (_cancelledResult.Count != 0)
        {
            return;
        }

        _testMethods = testMethods.Select(method =>
                                        {
                                            var instance = Activator.CreateInstance(testingClass) ?? throw new NullReferenceException();

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
        if (_testMethods.Count == 0)
        {
            return new TestClassResult(Name, [], 0);
        }

        if (_invalidMethods.Count > 0)
        {
            return new TestClassResult(Name, [], 0, _invalidMethods);
        }

        if (_cancelledResult.Count != 0)
        {
            return new TestClassResult(Name, _cancelledResult, 0);
        }

        var result = new ConcurrentBag<TestMethodResult>();
        Parallel.ForEach(_testMethods, testMethod => result.Add(testMethod.Run()));

        var totalDuration = result.Sum(testResult => testResult.Duration);

        try
        {
            InvokeClassMethods(_afterClassMethods);
        }
        catch (Exception e)
        {
            var errorMessage = $"Test was cancelled due to \"AfterClass\" method's {e.InnerException?.InnerException?.GetType()}";

            foreach (var testResult in result)
            {
                testResult.Status = MyTestStatus.Cancelled;
                testResult.ErrorMessage = errorMessage;
            }

            return new TestClassResult(Name, result, totalDuration);
        }

        return new TestClassResult(Name, result, totalDuration);
    }

    private static List<MethodInfo> GetMethodsWithAttribute(Type attributeType, IEnumerable<MethodInfo> methods)
        => methods.Where(m => Attribute.IsDefined(m, attributeType)).ToList();

    private static void InvokeClassMethods(IEnumerable<MethodInfo> classMethods)
        => Parallel.ForEach(classMethods, classMethod => classMethod.Invoke(null, null));

    private void CheckInvalidMethods(IEnumerable<MethodInfo> methods)
    {
        foreach (var method in methods)
        {
            if (method.ReturnType != typeof(void) || method.GetParameters().Length != 0)
            {
                _invalidMethods.Add(method.Name);
            }
        }
    }

    private void CheckInvalidTestMethods(IEnumerable<MethodInfo> classMethods)
    {
        foreach (var method in classMethods)
        {
            if (method.IsStatic)
            {
                _invalidMethods.Add(method.Name);
            }
        }
    }

    private void CheckInvalidClassMethods(IEnumerable<MethodInfo> classMethods)
    {
        foreach (var method in classMethods)
        {
            if (!method.IsStatic)
            {
                _invalidMethods.Add(method.Name);
            }
        }
    }
}
