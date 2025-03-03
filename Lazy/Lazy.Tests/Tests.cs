// <copyright file="Tests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace Lazy.Tests;

using Lazy;

public class Tests
{
    private static int _invokesCounter = 0;

    [TearDown]
    public void CleanUp()
        => _invokesCounter = 0;

    [Test]
    public void ArgumentNullException_ShouldBeThrown_WithNullFunc()
    {
        Assert.Throws<ArgumentNullException>(() => new SingleThreadedLazy<object>(null!));
        Assert.Throws<ArgumentNullException>(() => new MultiThreadedLazy<object>(null!));
    }

    [Test]
    public void ArgumentNullException_ShouldBeThrown_WithNullReturningFunc()
    {
        var singleThreadedLazy = new SingleThreadedLazy<object>(() => null!);
        var multiThreadedLazy = new MultiThreadedLazy<object>(() => null!);

        Assert.That(singleThreadedLazy.Get(), Is.Null);
        Assert.That(multiThreadedLazy.Get(), Is.Null);
    }

    [TestCaseSource(nameof(ValidTestCases))]
    public void Get_ShouldReturn_TheSameValue(ILazy<int> lazy)
    {
        var result = lazy.Get();
        Assert.That(lazy.Get(), Is.EqualTo(result));
    }

    [Test]
    public void MultiThreadedLazyTestWithRaces()
    {
        var mre = new ManualResetEvent(false);

        const int ExpectedResult = 1;
        const int TestNumberOfThreads = 6;

        var lazy = new MultiThreadedLazy<int>(TestMethod);
        var threads = new Thread[TestNumberOfThreads];
        var result = new int[TestNumberOfThreads];

        for (var i = 0; i < threads.Length; ++i)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                mre.WaitOne();
                result[localI] = lazy.Get();
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        Thread.Sleep(100);
        mre.Set();

        foreach (var thread in threads)
        {
            thread.Join();
        }

        foreach (var item in result)
        {
            Assert.That(item, Is.EqualTo(ExpectedResult));
        }
    }

    private static int TestMethod()
    {
        Interlocked.Increment(ref _invokesCounter);
        return _invokesCounter;
    }

    private static ILazy<int>[] ValidTestCases = 
    {
        new SingleThreadedLazy<int>(TestMethod),
        new MultiThreadedLazy<int>(TestMethod)
    };
}