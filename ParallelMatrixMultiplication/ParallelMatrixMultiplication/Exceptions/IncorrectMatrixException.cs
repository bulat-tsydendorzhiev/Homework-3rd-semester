namespace ParallelMatrixMultiplication;

/// <summary>
/// Throws when incorrect matrix or its data was given from file.
/// </summary>
public class IncorrectMatrixException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectMatrixException"> class.
    /// </summary>
    public IncorrectMatrixException()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectMatrixException"> class.
    /// </summary>
    public IncorrectMatrixException(string message) : base(message)
    {
    }
}