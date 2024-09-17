namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when dimensions mismatch occurs during matrices multiplying.
/// </summary>
public class DimensionsMismatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"> class.
    /// </summary>
    public DimensionsMismatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionsMismatchException"> class.
    /// </summary>
    public DimensionsMismatchException(string message) : base(message)
    {
    }
}