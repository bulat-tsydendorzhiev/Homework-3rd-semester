// <copyright file="Tests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Tests;

using System.Reflection;
using MyNUnit.Attributes;
using MyNUnit.TestComponents;

/// <summary>
/// Tests to check correctness of the MyNUnit work.
/// </summary>
public class Tests
{
    private const string Path = "../../../../TestCases/bin/Debug/net9.0/TestCases.dll";
    private Assembly _assembly;
    private TestAssemblyResult _assemblyResults;

    /// <summary>
    /// Makes Setup to work with tests.
    /// </summary>
    [OneTimeSetUp]
    public void Setup()
    {
        _assembly = Assembly.LoadFrom(Path);

        _assemblyResults = new TestAssembly(_assembly).RunTests();
    }

    /// <summary>
    /// Checks whether before and after methods are invoked correct number of times.
    /// </summary>
    [Test]
    public void Before_And_After_ShouldBe_ExpectedValue()
    {
        var passedType = _assembly.DefinedTypes.First(t => t.Name == "PassedTests");

        var expectedNumberOfInvokes = passedType.GetMethods().Where(m => Attribute.IsDefined(m, typeof(MyTestAttribute))).ToList().Count;
        var beforeMethodInvokesCount = passedType.GetField("beforeMethodInvokesCount")!.GetValue(null);
        var afterMethodInvokesCount = passedType.GetField("afterMethodInvokesCount")!.GetValue(null);

        Assert.That(beforeMethodInvokesCount, Is.EqualTo(expectedNumberOfInvokes));
        Assert.That(afterMethodInvokesCount, Is.EqualTo(expectedNumberOfInvokes));
    }

    /// <summary>
    /// Checks whether before and after class methods are invoked one time.
    /// </summary>
    [Test]
    public void Before_And_AfterClass_ShouldBe_ExpectedValue()
    {
        const int ExpectedNumberOfInvokes = 1;

        var testClass = _assembly.DefinedTypes.First(t => t.Name == "PassedTests");
        var beforeClassMethodInvokesCount = testClass.GetField("beforeClassMethodInvokesCount")!.GetValue(null);
        var afterClassMethodInvokesCount = testClass.GetField("afterClassMethodInvokesCount")!.GetValue(null);

        Assert.That(beforeClassMethodInvokesCount, Is.EqualTo(ExpectedNumberOfInvokes));
        Assert.That(afterClassMethodInvokesCount, Is.EqualTo(ExpectedNumberOfInvokes));
    }

    /// <summary>
    /// Checks whether non-static before and after class are invoked 0 times.
    /// </summary>
    [Test]
    public void Invalid_Before_And_AfterClass_ShouldBe_ExpectedValue()
    {
        const int ExpectedNumberOfInvokes = 0;

        var testClass = _assembly.DefinedTypes.First(t => t.Name == "InvalidTestCases");
        var beforeClassMethodInvokesCount = testClass.GetField("beforeClassMethodInvokesCount")!.GetValue(null);
        var afterClassMethodInvokesCount = testClass.GetField("afterClassMethodInvokesCount")!.GetValue(null);

        Assert.That(beforeClassMethodInvokesCount, Is.EqualTo(ExpectedNumberOfInvokes));
        Assert.That(afterClassMethodInvokesCount, Is.EqualTo(ExpectedNumberOfInvokes));
    }

    /// <summary>
    /// Checks whether test cases' test results returns expected status.
    /// </summary>
    /// <param name="className">Name of class in the assembly.</param>
    /// <param name="expectedStatus">Status that expects from specified scenario.</param>
    [TestCase("FailedTests", MyTestStatus.Failed)]
    [TestCase("IgnoredTests", MyTestStatus.Ignored)]
    [TestCase("PassedTests", MyTestStatus.Passed)]
    [TestCase("CancelledTestsBecauseOfBeforeClass", MyTestStatus.Canceled)]
    [TestCase("CancelledTestsBecauseOfBefore", MyTestStatus.Canceled)]
    [TestCase("CancelledTestsBecauseOfAfter", MyTestStatus.Canceled)]
    [TestCase("CancelledTestsBecauseOfAfterClass", MyTestStatus.Canceled)]
    public void TestCases_ShouldHave_ExpectedStatus(string className, MyTestStatus expectedStatus)
    {
        foreach (var classTestResult in _assemblyResults.ClassesTestResults)
        {
            if (classTestResult.Name == className)
            {
                foreach (var testResult in classTestResult.TestResults)
                {
                    Assert.That(testResult.Status, Is.EqualTo(expectedStatus));
                }
            }
        }
    }
}
