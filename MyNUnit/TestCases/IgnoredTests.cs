// <copyright file="IgnoredTests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace TestCases;

using MyNUnit.Assertion;
using MyNUnit.Attributes;

/// <summary>
/// Test class with passed methods.
/// </summary>
public class IgnoredTests
{
    [MyTest("skip")]
    public void SkipTestWithoutAnyGoodReason()
    {
    }

    [MyTest("skip", typeof(ArgumentException))]
    public void TestWithException()
    {
        throw new ArgumentNullException();
    }

    [MyTest("skip")]
    public void AssertTest()
    {
        MyAssert.That(1 == 2);
    }
}
