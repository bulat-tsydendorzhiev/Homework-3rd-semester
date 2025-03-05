// <copyright file="TestMethod.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.TestComponents;

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MyNUnit.Assertion;
using MyNUnit.Attributes;

/// <summary>
/// Represents a method that has test attribute.
/// </summary>
public class TestMethod
{
    private readonly object _instance;

    private readonly IEnumerable<MethodInfo> _beforeMethods;
    private readonly MethodInfo _test;
    private readonly IEnumerable<MethodInfo> _afterMethods;

    private readonly string? _ignoreMessage = null;
    private readonly Type? _expectedExceptionType = null;

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

        var testAttribute = test.GetCustomAttribute<MyTestAttribute>()!;
        _ignoreMessage = testAttribute.IgnoreMessage;
        _expectedExceptionType = testAttribute.ExpectedExceptionType;
    }

    /// <summary>
    /// Runs the test.
    /// </summary>
    /// <returns>The test result.</returns>
    public TestMethodResult Run()
    {
        if (_ignoreMessage is not null)
        {
            return new TestMethodResult(_test.Name, MyTestStatus.Ignored, 0, IgnoreMessage: _ignoreMessage);
        }

        var stopWatch = new Stopwatch();
        string? errorMessage = null;
        var status = MyTestStatus.Passed;

        try
        {
            InvokeMethods(_beforeMethods);
        }
        catch (Exception e)
        {
            errorMessage = $"Test was cancelled due to \"Before\" method's {e.InnerException?.InnerException?.GetType()}";
            return new TestMethodResult(_test.Name, MyTestStatus.Canceled, 0, ErrorMessage: errorMessage);
        }

        stopWatch.Start();

        try
        {
            _test.Invoke(_instance, null);
            stopWatch.Stop();
        }
        catch (Exception e)
        {
            stopWatch.Stop();

            var occuredExceptionType = e.InnerException?.GetType();

            errorMessage = occuredExceptionType == typeof(MyAssertException)
                                ? e.InnerException?.Message
                                : _expectedExceptionType is not null && occuredExceptionType != _expectedExceptionType
                                ? $"Expected {_expectedExceptionType} but {occuredExceptionType} occured."
                                : occuredExceptionType != _expectedExceptionType
                                ? $"Unexpected exception: {occuredExceptionType}"
                                : "no errors";
            status = errorMessage == "no errors"
                ? MyTestStatus.Passed
                : MyTestStatus.Failed;
        }
        finally
        {
            try
            {
                InvokeMethods(_afterMethods);
            }
            catch (Exception e)
            {
                errorMessage = $"Test was cancelled due to \"After\" method's {e.InnerException?.InnerException?.GetType()}";
                status = MyTestStatus.Canceled;
            }
        }

        if (_expectedExceptionType is not null && errorMessage is null)
        {
            errorMessage = $"Expected {_expectedExceptionType} but it didn't occur";
            return new TestMethodResult(_test.Name, MyTestStatus.Failed, stopWatch.ElapsedMilliseconds, ErrorMessage: errorMessage);
        }

        if (status == MyTestStatus.Passed)
        {
            errorMessage = null;
        }

        return new TestMethodResult(_test.Name, status, stopWatch.ElapsedMilliseconds, ErrorMessage: errorMessage);
    }

    private void InvokeMethods(IEnumerable<MethodInfo> methods)
        => Parallel.ForEach(methods, method => method.Invoke(_instance, null));
}