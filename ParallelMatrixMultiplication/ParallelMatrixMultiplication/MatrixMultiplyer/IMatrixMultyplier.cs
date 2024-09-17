namespace ParallelMatrixMultiplication;

/// <summary>
/// Interface that defines methods for matrix multiplication.
/// </summary>
public interface IMatrixMultyplier
{
    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    /// <param name="firstMatrix">First matrix.</param>
    /// <param name="secondMatrix">Second matrix.</param>
    /// <returns>Matrix with 1st matrix rows number and 2nd matrix columns number.</returns>
    /// <exception cref="DimensionsMismatchException">
    /// Throws when dimensions mismatch occurs during matrices multiplying.
    /// </exception>
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix);
}