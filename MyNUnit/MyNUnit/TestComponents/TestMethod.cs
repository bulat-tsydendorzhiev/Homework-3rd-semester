// <copyright file="TestMethod.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNunit.TestClasses;

using MyNunit.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Represents a method that has test attribute.
/// </summary>
public class TestMethod
{
    private readonly object _instance;

    private readonly IEnumerable<MethodInfo> _beforeMethods;

    private readonly MethodInfo _test;

    private readonly IEnumerable<MethodInfo> _afterMethods;

    private readonly string? _ignoreReason = null;

    private Type? _expectedException = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestMethod"/> class.
    /// </summary>
    /// <param name="instance">Instance on which the test is invoked.</param>
    /// <param name="test">Test method.</param>
    /// <param name="beforeMethods">Methods that should be invoked before test method.</param>
    /// <param name="afterMethods">Methods that should be invoked after test method.</param>
    public TestMethod(object instance, MethodInfo test, IEnumerable<MethodInfo> beforeMethods, IEnumerable<MethodInfo> afterMethods)
    {
        _instance = instance;
        _test = test;
        _beforeMethods = beforeMethods;
        _afterMethods = afterMethods;

        var testAttribute = test.GetCustomAttribute<TestAttribute>() !;
        _ignoreReason = testAttribute.IgnoreMessage;
        _expectedException = testAttribute.ExpectedException;
    }

    /// <summary>
    /// Runs the test.
    /// </summary>
    /// <returns>The test result.</returns>
    public TestMethodResult Run()
    {
        var stopWatch = new Stopwatch();
        Type? occuredException = null;
        var errorMessage = string.Empty;

        if (_ignoreReason is not null)
        {
            return new TestMethodResult(_test.Name, TestStatus.Ignored, 0, _ignoreReason);
        }

        InvokeMethods(_beforeMethods);
        stopWatch.Start();

        try
        {
            _test.Invoke(_instance, null);
            stopWatch.Stop();
        }
        catch (Exception e)
        {
            stopWatch.Stop();

            occuredException = e.InnerException?.GetType();

            errorMessage = occuredException != _expectedException
                            ? $"Expected {_expectedException}, but {occuredException} has occured."
                            : e.Message;
        }
        finally
        {
            InvokeMethods(_afterMethods);
        }

        return new TestMethodResult(_test.Name, TestStatus.Failed, stopWatch.ElapsedMilliseconds, errorMessage: errorMessage);
    }

    private static void InvokeMethods(IEnumerable<MethodInfo> methods)
        => Parallel.ForEach(methods, method => method.Invoke(null, null));
}
