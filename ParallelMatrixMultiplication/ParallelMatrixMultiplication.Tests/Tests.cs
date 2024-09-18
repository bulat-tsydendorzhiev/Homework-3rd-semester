namespace ParallelMatrixMultiplication.Tests;

using ParallelMatrixMultiplication;

public class Tests
{
    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.InvalidTestCases))]
    public void IncorrectMatrixException_ShouldBeThrown_DuringMatrixGetting(string filePath)
    {
        Assert.Throws<IncorrectMatrixException>(() => new Matrix(filePath));
    }

    [TestCase("../../../TestFiles/EmptyFile.txt")]
    public void FileNotFoundException_ShouldBeThrown_DuringMatrixGetting(string invalidFilePath)
    {
        Assert.Throws<FileNotFoundException>(() => new Matrix(invalidFilePath));
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.ValidTestCases))]
    public void SequentialMatrixMultiplication_ShouldReturn_RightAnswer((string FirstMatrixData, string SecondMatrixData, Matrix Expected) testData)
    {
        var firstMatrix = new Matrix(testData.FirstMatrixData);
        var secondMatrix = new Matrix(testData.SecondMatrixData);
        var result = MatrixMultiplier.Multiply(firstMatrix, secondMatrix);

        Assert.That(result.Equals(testData.Expected), Is.True);
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.ValidTestCases))]
    public void ParallelMatrixMultiplication_ShouldReturn_RightAnswer((string FirstMatrixData, string SecondMatrixData, Matrix Expected) testData)
    {
        var firstMatrix = new Matrix(testData.FirstMatrixData);
        var secondMatrix = new Matrix(testData.SecondMatrixData);
        var result = MatrixMultiplier.MultiplyInParallel(firstMatrix, secondMatrix);

        Assert.That(result.Equals(testData.Expected), Is.True);
    }

    private class TestDataClass
    {
        public static string[] InvalidTestCases =
        {
            "../../../TestFiles/Empty.txt",
            "../../../TestFiles/InvalidMatrix.txt",
            "../../../TestFiles/MatrixWithInvalidData.txt"
        };
        
        public static (string, string, Matrix)[] ValidTestCases = 
        {
            ("../../../TestFiles/1stMatrix.txt",
            "../../../TestFiles/2ndMatrix.txt",
            new Matrix("../../../TestFiles/ExpectedResult1.txt")),
            ("../../../TestFiles/1stMatrix.txt",
            "../../../TestFiles/3rdMatrix.txt",
            new Matrix("../../../TestFiles/3rdMatrix.txt")),
            ("../../../TestFiles/4thMatrix.txt",
            "../../../TestFiles/1stMatrix.txt",
            new Matrix("../../../TestFiles/ExpectedResult2.txt")),
            ("../../../TestFiles/4thMatrix.txt",
            "../../../TestFiles/3rdMatrix.txt",
            new Matrix("../../../TestFiles/ExpectedResult3.txt"))
        };
    }
}