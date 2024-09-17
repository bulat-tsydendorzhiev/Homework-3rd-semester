namespace ParallelMatrixMultiplication;

using System.Threading;

/// <summary>
/// Class that implements parallel matrix multiplication.
/// </summary>
public class ParallelMatrixMultiplyer : IMatrixMultyplier
{
    /// <inheritdoc/>
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        return new Matrix(new List<int[]>());
    }
}