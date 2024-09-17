namespace ParallelMatrixMultiplication;

/// <summary>
/// Class that implements sequential matrix multiplication.
/// </summary>
public class SequentialMatrixMultiplyer : IMatrixMultyplier
{
    /// <inheritdoc/>
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.ColumnsNumber != secondMatrix.RowsNumber)
        {
            var message = "Number of columns of the first matrix doesn't match with number of rows of the second one.";
            throw new DimensionsMismatchException(message);
        }

        var result = new List<int[]>(firstMatrix.RowsNumber);

        for (var i = 0; i < firstMatrix.RowsNumber; ++i)
        {
            var temp = new int[secondMatrix.ColumnsNumber];
            for (var j = 0; j < secondMatrix.ColumnsNumber; ++j)
            {
                for (var k = 0; k < secondMatrix.RowsNumber; ++k)
                {
                    temp[j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }

            result.Add(temp);
        }

        return new Matrix(result);
    }
}