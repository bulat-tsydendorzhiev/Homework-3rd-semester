namespace ParallelMatrixMultiplication.Tests;

using ParallelMatrixMultiplication;

public class Tests
{
    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.InvalidTestCases))]
    public void InvalidMatrixException_ShouldBeThrown_DuringMatrixGetting(string filePath)
        => Assert.Throws<InvalidMatrixException>(() => new Matrix(filePath));

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.InvlidTestMatrices))]
    public void InvalidMatrixException_ShouldBeThrown_DuringMatrixGetting(int[,] matrix)
        => Assert.Throws<InvalidMatrixException>(() => new Matrix(matrix));

    [TestCase("../../../TestFiles/EmptyFile.txt")]
    public void FileNotFoundException_ShouldBeThrown_DuringMatrixGetting(string invalidFilePath)
        => Assert.Throws<FileNotFoundException>(() => new Matrix(invalidFilePath));

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.ValidTestCases))]
    public void Sequential_MatrixMultiplication_ShouldReturn_RightAnswer((Matrix FirstMatrix, Matrix SecondMatrix, Matrix Expected) testData)
    {
        var resultSequential = MatrixMultiplier.Multiply(testData.FirstMatrix, testData.SecondMatrix);

        Assert.That(resultSequential.Equals(testData.Expected), Is.True);
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.ValidTestCases))]
    public void Parallel_MatrixMultiplication_ShouldReturn_RightAnswer((Matrix FirstMatrix, Matrix SecondMatrix, Matrix Expected) testData)
    {
        var resultParallel = MatrixMultiplier.MultiplyInParallel(testData.FirstMatrix, testData.SecondMatrix);

        Assert.That(resultParallel.Equals(testData.Expected), Is.True);
    }

    private class TestDataClass
    {
        public static string[] InvalidTestCases =
        {
            "../../../TestFiles/Empty.txt",
            "../../../TestFiles/InvalidMatrix.txt",
            "../../../TestFiles/MatrixWithInvalidData.txt"
        };

        public static object[] InvlidTestMatrices =
        {
            new int[0, 1],
            new int[1, 0]
        };

        public static (Matrix, Matrix, Matrix)[] ValidTestCases = 
        {
            (new Matrix("../../../TestFiles/1stMatrix.txt"),
            new Matrix("../../../TestFiles/2ndMatrix.txt"),
            new Matrix("../../../TestFiles/ExpectedResult1.txt")),
            (new Matrix("../../../TestFiles/1stMatrix.txt"),
            new Matrix("../../../TestFiles/3rdMatrix.txt"),
            new Matrix("../../../TestFiles/3rdMatrix.txt")),
            (new Matrix("../../../TestFiles/4thMatrix.txt"),
            new Matrix("../../../TestFiles/1stMatrix.txt"),
            new Matrix("../../../TestFiles/ExpectedResult2.txt")),
            (new Matrix("../../../TestFiles/4thMatrix.txt"),
            new Matrix("../../../TestFiles/3rdMatrix.txt"),
            new Matrix("../../../TestFiles/ExpectedResult3.txt"))
        };
    }
}