// <copyright file="InvalidTestCases.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace TestCases;

using MyNUnit.Attributes;

public class InvalidTestCases
{
    public static volatile int beforeClassMethodInvokesCount;
    public static volatile int afterClassMethodInvokesCount;

    [BeforeClass]
    public void NonStaticBeforeClassMethod()
    {
        Interlocked.Increment(ref beforeClassMethodInvokesCount);
    }

    [AfterClass]
    public void NonStaticAfterClassMethod()
    {
        Interlocked.Increment(ref afterClassMethodInvokesCount);
    }

    [MyTest]
    public int TestMethod_WithNonVoid_ReturningType()
    {
        return 1;
    }

    [MyTest]
    public void TestMethod_WithParameters(int value)
    {
    }
}
