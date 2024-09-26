namespace Lazy.Tests;

using Lazy;

public class Tests
{
    private static int _invokesCounter = 0;

    [TearDown]
    public void CleanUp()
    {
        _invokesCounter = 0;
    }

    [Test]
    public void ArgumentNullException_ShouldBeThrown_WithNullFunc()
    {
        Assert.Throws<ArgumentNullException>(() => new SingleThreadedLazy<object>(null));
        Assert.Throws<ArgumentNullException>(() => new MultiThreadedLazy<object>(null));
    }

    public void ArgumentNullException_ShouldBeThrown_WithNullReturningFunc()
    {
        var singleThreadedLazy = new SingleThreadedLazy<object>(() => null);
        var multiThreadedLazy = new MultiThreadedLazy<object>(() => null);

        Assert.Throws<ArgumentNullException>(() => singleThreadedLazy.Get());
        Assert.Throws<ArgumentNullException>(() => multiThreadedLazy.Get());
    }

    [TestCaseSource(nameof(ValidTestCases))]
    public void Get_ShouldReturn_TheSameValue(ILazy<int> lazy)
    {
        Assert.That(lazy.Get(), Is.EqualTo(lazy.Get()));
    }

    [Test]
    public void MultiThreadedLazyTestWithRaces()
    {
        const int ExpectedResult = 1;
        const int TestNumberOfThreads = 6;

        var lazy = new MultiThreadedLazy<int>(TestMethod);
        var threads = new Thread[TestNumberOfThreads];

        for (var i = 0; i < threads.Length; ++i)
        {
            threads[i] = new Thread(() =>
            {
                Assert.That(lazy.Get(), Is.EqualTo(ExpectedResult));
                Assert.That(lazy.Get(), Is.EqualTo(ExpectedResult));
            });
        }
        
        foreach (var thread in threads)
        {
            thread.Start();
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