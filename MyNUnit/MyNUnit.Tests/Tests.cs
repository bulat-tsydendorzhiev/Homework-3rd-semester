// <copyright file="Tests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit.Tests;

using MyNUnit;
using MyNUnit.Attributes;
using MyNUnit.TestClasses;
using System.Reflection;

public class Tests
{
    private const string path = "../../../../TestCases/bin/Debug/net9.0/TestCases.dll";
    private Assembly _assembly;
    private TestAssemblyResult _assemblyResults;

    [OneTimeSetUp]
    public void Setup()
    {
        _assembly = Assembly.LoadFrom(path);
        
        _assemblyResults = new TestAssembly(_assembly).RunTests();
    }

    [Test]
    public void Before_And_After_ShouldBe_ExpectedValue()
    {
        var passedType = _assembly.DefinedTypes.First(t => t.Name == "PassedTests");

        var expectedNumberOfInvokes = passedType.GetMethods().Where(m => Attribute.IsDefined(m, typeof(MyTestAttribute))).ToList().Count;
        var beforeMethodInvokesCount = passedType.GetField("beforeMethodInvokesCount")!.GetValue(null);
        var afterMethodInvokesCount = passedType.GetField("afterMethodInvokesCount")!.GetValue(null);

        Assert.That(beforeMethodInvokesCount, Is.EqualTo(expectedNumberOfInvokes));
        Assert.That(afterMethodInvokesCount, Is.EqualTo(1));
    }

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

    [TestCase("FailedTests", MyTestStatus.Failed)]
    [TestCase("IgnoredTests", MyTestStatus.Ignored)]
    [TestCase("PassedTests", MyTestStatus.Passed)]
    public void TestCases_ShouldHave_ExpectedStatus(string className, MyTestStatus expectedStatus)
    {
        foreach (var classTestResult in _assemblyResults.TestResults)
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
