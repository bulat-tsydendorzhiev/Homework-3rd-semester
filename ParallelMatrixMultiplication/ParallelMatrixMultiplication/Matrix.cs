using System.Text;

namespace ParallelMatrixMultiplication;

/// <summary>
/// Class, which has methods for matrix multiplication.
/// </summary>
public class Matrix
{
    /// <summary>
    /// Gets number of columns in matrix.
    /// </summary>
    public int ColumnsNumber {get; }

    /// <summary>
    /// Gets number of rows in matrix.
    /// </summary>
    public int RowsNumber {get; }

    private List<int[]> _matrix;
    
    private IMatrixMultyplier _matrixMultyplier;

    /// <summary>
    /// Initializes a new instance of <see cref="Matrix"/>.
    /// </summary>
    /// <param name="path">Path to file with matrix.</param>
    /// <exception cref="FileNotFoundException">Throws when file with given path doesn't exist.</exception>
    /// <exception cref="IncorrectMatrixException">Throws when empty file was given.</exception>
    /// <exception cref="IncorrectMatrixException">Throws when incorrect matrix or its data was given.</exception>
    public Matrix(string path, IMatrixMultyplier matrixMultyplier)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("There is no file with such path.");
        }

        var rows = File.ReadLines(path).ToList();
        if (rows.Count == 0)
        {
            throw new IncorrectMatrixException("Matrix cannot be empty.");
        }

        var matrix = new List<int[]>();

        for (var i = 0; i < rows.Count; ++i)
        {
            var values = rows[i].Split();
            var rowValues = new List<int>();

            foreach (var value in values)
            {
                if (!int.TryParse(value, out int number))
                {
                    throw new IncorrectMatrixException("Incorrect matrix data was given.");
                }

                rowValues.Add(number);
            }
            
            if (i > 0 && rowValues.Count != matrix[i - 1].Length)
            {
                throw new IncorrectMatrixException("Incorrect matrix was given.");
            }
            
            matrix.Add([..rowValues]);
        }

        RowsNumber = rows.Count;
        ColumnsNumber = matrix[0].Length;
        _matrix = matrix;
        _matrixMultyplier = matrixMultyplier;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Matrix"/>.
    /// </summary>
    /// <param name="newMatrix">New matrix.</param>
    public Matrix(List<int[]> newMatrix)
    {
        ArgumentNullException.ThrowIfNull(newMatrix);

        RowsNumber = newMatrix.Count;
        ColumnsNumber = newMatrix[0].Length;
        _matrix = newMatrix;
    }

    /// <summary>
    /// Gets the value by its indices.
    /// </summary>
    /// <param name="rowIndex">Number of row.</param>
    /// <param name="columnIndex">Number of column.</param>
    /// <returns>Value by specified incices.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws when indices out of range.</exception>
    public int this[int rowIndex, int columnIndex]
    {
        get
        {
            if (!AreValidIndices(rowIndex, columnIndex))
            {
                throw new ArgumentOutOfRangeException("Index out of range.");
            }

            return _matrix[rowIndex][columnIndex];
        }
    }

    /// <summary>
    /// Multiply this matrix by given.
    /// </summary>
    /// <param name="matrix">Matrix to multiply by.</param>
    /// <returns>Matrix with 1st matrix rows number and 2nd matrix columns number.</returns>
    /// <exception cref="DimensionsMismatchException">
    /// Throws when dimensions mismatch occurs during matrices multiplying.
    /// </exception>
    public Matrix MultiplyBy(Matrix matrix)
    {
        return _matrixMultyplier.Multiply(this, matrix);
    }

    /// <summary>
    /// Write matrix to the file.
    /// </summary>
    /// <param name="outputPath">Path where matrix will be located.</param>
    public void WriteToFile(string outputPath)
    {
        using(var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
        {
            for (var i = 0; i < RowsNumber; ++i)
            {
                writer.Write(string.Join(' ', _matrix[i]));
                writer.Write("\n");
            }
        }
    }

    private bool AreValidIndices(int rowIndex, int columnIndex)
        => rowIndex >= 0 && rowIndex < RowsNumber && columnIndex >= 0 && columnIndex < ColumnsNumber;
}