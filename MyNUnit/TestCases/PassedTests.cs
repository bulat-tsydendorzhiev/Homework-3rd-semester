// <copyright file="PassedTests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace TestCases;

using MyNUnit.Assertion;
using MyNUnit.Attributes;

/// <summary>
/// Test class with passed methods.
/// </summary>
public class PassedTests
{
    public static volatile int beforeClassMethodInvokesCount;
    public static volatile int beforeMethodInvokesCount;
    public static volatile int afterMethodInvokesCount;
    public static volatile int afterClassMethodInvokesCount;

    [BeforeClass]
    public static void BeforeClass_ShouldBeCalled_OneTime()
    {
        Interlocked.Increment(ref beforeClassMethodInvokesCount);
    }

    [Before]
    public void Before_ShouldBeCalled_BeforeEveryTest()
    {
        Interlocked.Increment(ref beforeMethodInvokesCount);
    }

    [After]
    public void After_ShouldBeCalled_AfterEveryTest()
    {
        Interlocked.Increment(ref afterMethodInvokesCount);
    }

    [AfterClass]
    public static void AfterClass_ShouldBeCalled_OneTime()
    {
        Interlocked.Increment(ref afterClassMethodInvokesCount);
    }

    //[MyTest(typeof(ArgumentException))]
    //public void TestWithException()
    //{
    //    throw new ArgumentException();
    //}

    [MyTest]
    public void AssertThatTest()
    {
        MyAssert.That(1 + 1 == 2);
    }
}
