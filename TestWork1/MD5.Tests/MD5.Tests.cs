namespace MD5.Tests;

using TestWork1;

public class Tests
{
    [TestCase("../../../TestDirectory", new byte[] {80, 194, 226, 230, 233, 35, 206, 40, 123, 81, 72, 19, 152, 149, 15, 57})]
    public void CheckSumCalculators_ShouldWorkCorrectly_WithRightCases(string path, byte[] expected)
    {
        var singleThreadedResult = SingleThreadedCheckSumCalculator.CalculateCheckSum(path);
        var multiThreadedResult = MultiThreadedCheckSumCalculator.CalculateCheckSum(path);

        Assert.That(singleThreadedResult, Is.EqualTo(multiThreadedResult));
        Assert.That(singleThreadedResult, Is.EqualTo(expected));
        Assert.That(multiThreadedResult, Is.EqualTo(expected));
    }
    
    [TestCase("")]
    [TestCase("ololo")]
    public void CalculatorsCheckSum_ShouldThrow_DirectoryNotFoundException_WithIncorrectPath(string path)
    {
        Assert.Throws<DirectoryNotFoundException>(() => SingleThreadedCheckSumCalculator.CalculateCheckSum(path));
        Assert.Throws<DirectoryNotFoundException>(() => MultiThreadedCheckSumCalculator.CalculateCheckSum(path));
    }
}