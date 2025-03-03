 // <copyright file = "FailedTests.cs" company = "Bulat Tsydendorzhiev">
 // Copyright(c) Bulat Tsydendorzhiev. All Rights Reserved.
 // Licensed under the MIT License. See LICENSE in the repository root for license information.
 // </copyright>
namespace TestCases;

using MyNUnit.Attributes;
using MyNUnit.Assertion;

/// <summary>
/// Test class with passed methods.
/// </summary>
public class FailedTests
{
    [MyTest]
    public void TestWithoutExpectedException()
    {
        throw new ArgumentNullException();
    }

    [MyTest]
    public void TestWithoutExpectedException_WithExceptionThrowing()
    {
        throw new Exception();
    }

    [MyTest(typeof(ArgumentException))]
    public void TestWithIncorrectException()
    {
        throw new ArgumentNullException();
    }

    [MyTest]
    public void TestWithAssertThat()
    {
        MyAssert.That(1 + 1 == 3);
    }
}
