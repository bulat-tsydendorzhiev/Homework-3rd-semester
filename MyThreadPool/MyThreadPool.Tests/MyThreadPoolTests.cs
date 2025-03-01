// <copyright file="MyThreadPoolTests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace Tests;

using MyThreadPool;

public class Tests
{
    private MyThreadPool _threadPool;

    private readonly int _numberOfThreads = Environment.ProcessorCount;

    [SetUp]
    public void Setup()
        => _threadPool = new (_numberOfThreads); 

    [TearDown]
    public void Teardown()
        => _threadPool.Shutdown();

    [Test]
    public void ThreadPool_ShouldHave_AtLeast_InitializedInConstructor_NumberOfWorkingThreads()
    {
        const int ExpectedResult = 1;
        var tasks = new IMyTask<int>[_numberOfThreads];

        for (var i = 0; i < _numberOfThreads; i++)
        {
            tasks[i] = _threadPool.Submit(() => ExpectedResult);
        }

        foreach (var task in tasks)
        {
            Assert.That(task.Result, Is.EqualTo(ExpectedResult));
            Assert.That(task.IsCompleted, Is.True);
        }
    }

    [Test, Timeout(6000)]
    public void Submit_ShouldBe_ThreadSafe_WithShutdown()
    {
        var submitThread = new Thread(() =>
        {
            const int ExpectedResult = 1;

            var task = _threadPool.Submit(() =>
            {
                Thread.Sleep(5000);

                return ExpectedResult;
            });

            Assert.That(task.Result, Is.EqualTo(ExpectedResult));
        });

        var shutdownThread = new Thread(() =>
        {
            Thread.Sleep(100);
            _threadPool.Shutdown();
        });

        submitThread.Start();
        shutdownThread.Start();

        submitThread.Join();
        shutdownThread.Join();
    }

    [Test, Timeout(6000)]
    public void ContinueWith_ShouldBe_ThreadSafe_WithShutdown()
    {
        var continueWithThread = new Thread(() =>
        {
            const int ExpectedResult = 1;

            var task = _threadPool.Submit(() => ExpectedResult);
            var newTask = task.ContinueWith(value =>
            {
                Thread.Sleep(5000);

                return value;
            });

            Assert.That(newTask.Result, Is.EqualTo(ExpectedResult));
        });

        var shutdownThread = new Thread(() =>
        {
            Thread.Sleep(100);
            _threadPool.Shutdown();
        });

        continueWithThread.Start();
        shutdownThread.Start();

        continueWithThread.Join();
        shutdownThread.Join();
    }

    [Test]
    public void ResultAfter_MultipleCallsOfContinueWith_ShouldBe_ExpectedValue()
    {
        var task = _threadPool.Submit(() => 2 * 2)
                    .ContinueWith(x => x.ToString())
                    .ContinueWith(x => x + "44")
                    .ContinueWith(int.Parse);

        Assert.That(task.Result, Is.EqualTo(444));
    }

    [Test, Timeout(1000)]
    public void Tasks_ShouldNotBeAccepted_AftedShutdown()
    {
        var task = _threadPool.Submit(() => 123 * 123);

        Thread.Sleep(100);
        _threadPool.Shutdown();

        Assert.That(task.Result, Is.EqualTo(123 * 123));
        Assert.Throws<InvalidOperationException>(() => task.ContinueWith(x => x + 1));
        Assert.Throws<InvalidOperationException>(() => _threadPool.Submit(() => 123 * 123));
    }

    [Test, Timeout(2000)]
    public void AggregateException_ShouldBeThrown_WithInvalidSupplier()
    {
        var task = _threadPool.Submit(() =>
        {
            var array = new int[1];
            return array[123];
        });

        Assert.Throws<AggregateException>(() => { _ = task.Result; });
    }

    [Test]
    public void ArgumentNullException_ShouldBeThrown_WithNullSupplierCase()
    {
        Func<int> func1 = null!;
        Func<int, int> func2 = null!;
        
        Assert.Throws<ArgumentNullException>(() => _threadPool.Submit(func1));
        Assert.Throws<ArgumentNullException>(() => _threadPool.Submit(() => 1).ContinueWith(func2));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void ArgumentException_ShouldBeThrown_WithInvalidNumberOfThreads(int invalidNumberOfThreads)
        => Assert.Throws<ArgumentException>(() => _ = new MyThreadPool(invalidNumberOfThreads));
}