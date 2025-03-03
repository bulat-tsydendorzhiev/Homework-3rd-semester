// <copyright file = "CancelledTests.cs" company = "Bulat Tsydendorzhiev">
// Copyright(c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace TestCases;

using MyNUnit.Assertion;
using MyNUnit.Attributes;

public class CancelledTestsBecauseOfBefore
{
    [Before]
    public void Before()
    {
        throw new ArgumentNullException();
    }

    [MyTest]
    public void SimpleTest()
    {
        MyAssert.That(1 + 1 == 2);
    }
}

public class CancelledTestsBecauseOfAfter
{
    [After]
    public void After()
    {
        throw new Exception();
    }

    [MyTest]
    public void SimpleTest()
    {
        MyAssert.That(1 + 1 == 2);
    }
}
