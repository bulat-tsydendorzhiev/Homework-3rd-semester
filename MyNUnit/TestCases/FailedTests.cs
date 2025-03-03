// <copyright file="FailedTests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
//namespace TestCases;

//using MyNUnit.Attributes;
//using MyNUnit.Assertion;

///// <summary>
///// Test class with passed methods.
///// </summary>
//public class FailedTests
//{
//    [Test]
//    public void TestWithException()
//    {
//        throw new ArgumentNullException();
//    }

//    [Test(typeof(ArgumentException))]
//    public void TestWithIncorrectException()
//    {
//        throw new ArgumentNullException();
//    }

//    [Test]
//    public void TestWithAssertThat()
//    {
//        MyAssert.That(1 + 1 == 3);
//    }

//    [Test]
//    public void TestWithAssertThrows()
//    {
//        MyAssert.Throws<ArgumentException>(() => throw new ArgumentNullException());
//    }
//}
