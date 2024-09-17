namespace ParallelMatrixMultiplication.Tests;

using ParallelMatrixMultiplication;

public class Tests
{
    [TestCase("../../../TestFiles/Empty.txt")]
    [TestCase("../../../TestFiles/InvalidMatrix.txt")]
    [TestCase("../../../TestFiles/MatrixWithInvalidData.txt")]
    public void IncorrectMatrixException_ShouldBeThrown_During(string filePath)
    {
        Assert.Throws<IncorrectMatrixException>(() => new Matrix(filePath, new SequentialMatrixMultiplyer()));
    }

    [TestCase("../../../TestFiles/EmptyFile.txt")]
    public void FileNotFoundException_ShouldBeThrown_During(string invalidFilePath)
    {
        Assert.Throws<FileNotFoundException>(() => new Matrix(invalidFilePath, new SequentialMatrixMultiplyer()));
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.ValidTestCases))]
    public void SequentialMatrixMultiplication_ShouldReturn_RightAnswer((string Path1, string Path2, Matrix Expected) testData)
    {
        var firstMatrix = new Matrix(testData.Path1, new SequentialMatrixMultiplyer());
        var secondMatrix = new Matrix(testData.Path2, new SequentialMatrixMultiplyer());

        Assert.That(MatricesAreEqual(firstMatrix.MultiplyBy(secondMatrix), testData.Expected), Is.True);
    }

    private static bool MatricesAreEqual(Matrix given, Matrix expected)
    {
        if (given.RowsNumber != expected.RowsNumber || given.ColumnsNumber != expected.ColumnsNumber)
        {
            return false;
        }
        
        for (var i = 0; i < given.RowsNumber; ++i)
        {
            for (var j = 0; j < given.ColumnsNumber; ++j)
            {
                if (given[i, j] != expected[i, j])
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}

public class TestDataClass
{
    public static (string, string, Matrix)[] ValidTestCases = 
    {
        ("../../../TestFiles/1stMatrix.txt",
        "../../../TestFiles/2ndMatrix.txt",
        new Matrix("../../../TestFiles/ExpectedResult1.txt", new SequentialMatrixMultiplyer())),
        ("../../../TestFiles/1stMatrix.txt",
        "../../../TestFiles/3rdMatrix.txt",
        new Matrix("../../../TestFiles/3rdMatrix.txt", new SequentialMatrixMultiplyer())),
        ("../../../TestFiles/4thMatrix.txt",
        "../../../TestFiles/1stMatrix.txt",
        new Matrix("../../../TestFiles/ExpectedResult2.txt", new SequentialMatrixMultiplyer())),
        ("../../../TestFiles/4thMatrix.txt",
        "../../../TestFiles/3rdMatrix.txt",
        new Matrix([[0, 0, 0]]))
    };
}