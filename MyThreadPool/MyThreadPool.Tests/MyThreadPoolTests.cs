namespace Tests;

using MyThreadPool;

public class Tests
{
    private MyThreadPool _threadPool;

    private readonly int _numberOfThreads = Environment.ProcessorCount;

    [SetUp]
    public void Setup()
    {
        _threadPool = new (_numberOfThreads);
    }

    [TearDown]
    public void Teardown()
    {
        _threadPool.Shutdown();
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

    [Test]
    public void Task_ShouldFinish_ItsWork_AfterShutdown()
    {
        var task = _threadPool.Submit(() => 1);

        var newTask = task.ContinueWith(value =>
        {
            Thread.Sleep(1000);

            return value + 1;
        });

        _threadPool.Shutdown();

        Assert.That(newTask.Result, Is.EqualTo(2));
    }

    [Test]
    public void Tasks_ShouldNotBeAccepted_AftedShutdown()
    {
        var task = _threadPool.Submit(() => 123 * 123);

        _threadPool.Shutdown();

        Assert.Throws<InvalidOperationException>(() => task.ContinueWith(x => x + 1));
        Assert.Throws<InvalidOperationException>(() => _threadPool.Submit(() => 123 * 123));
    }

    [Test]
    public void AggregateException_ShouldBeThrown_WithInvalidSupplier()
    {
        var task = _threadPool.Submit(() =>
        {
            var array = new int[1];
            return array[123];
        });

        Assert.Throws<AggregateException>(() => { var result = task.Result; });
    }

    [Test]
    public void ArgumentNullException_ShouldBeThrown_NullSupplier()
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