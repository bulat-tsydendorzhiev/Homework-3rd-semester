namespace ParallelMatrixMultiplication;

/// <summary>
/// Class for multiplying two matrices.
/// </summary>
public static class MatrixMultiplier
{
    /// <summary>
    /// Nultiply matrices sequentially.
    /// </summary>
    /// <param name="firstMatrix">First rectangle matrix.</param>
    /// <param name="secondMatrix">Second rectangle matrix.</param>
    /// <returns>New rectangle matrix.</returns>
    /// <exception cref="DimensionsMismatchException">
    /// Throws when dimensions mismatch occurs during matrices multiplying.
    /// </exception>
    public static Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.ColumnsNumber != secondMatrix.RowsNumber)
        {
            var message = "Number of columns of the first matrix doesn't match with number of rows of the second one.";
            throw new DimensionsMismatchException(message);
        }

        var result = new int[firstMatrix.RowsNumber, secondMatrix.ColumnsNumber];

        for (var i = 0; i < firstMatrix.RowsNumber; ++i)
        {
            for (var j = 0; j < secondMatrix.ColumnsNumber; ++j)
            {
                for (var k = 0; k< firstMatrix.ColumnsNumber; ++k)
                {
                    result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        return new Matrix(result);
    }

    /// <summary>
    /// Nultiply matrices in parallel.
    /// </summary>
    /// <param name="firstMatrix">First rectangle matrix.</param>
    /// <param name="secondMatrix">Second rectangle matrix.</param>
    /// <returns>New rectangle matrix.</returns>
    /// <exception cref="DimensionsMismatchException">
    /// Throws when dimensions mismatch occurs during matrices multiplying.
    /// </exception>
    public static Matrix MultiplyInParallel(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.ColumnsNumber != secondMatrix.RowsNumber)
        {
            var message = "Number of columns of the first matrix doesn't match with number of rows of the second one.";
            throw new DimensionsMismatchException(message);
        }

        var result = new int[firstMatrix.RowsNumber, secondMatrix.ColumnsNumber];
        var threads = new Thread[Environment.ProcessorCount];
        
        var chunkSize = firstMatrix.RowsNumber / threads.Length + 1;
        
        for (var t = 0; t < threads.Length; ++t)
        {
            var localT = t;

            threads[t] = new Thread(() => {
                for (var i = localT * chunkSize; i < (localT + 1) * chunkSize && i < firstMatrix.RowsNumber; i += chunkSize)
                {
                    for (var j = 0; j < secondMatrix.ColumnsNumber; ++j)
                    {
                        for (var k = 0; k < firstMatrix.ColumnsNumber; ++k)
                        {
                            result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                        }
                    }
                }
            });
        }
        
        foreach (var thread in threads)
        {
            thread.Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        return new Matrix(result);
    }
}