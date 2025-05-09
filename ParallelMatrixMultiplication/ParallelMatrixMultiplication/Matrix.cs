// <copyright file="Matrix.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

using System.Text;

/// <summary>
/// Class that implements matrix.
/// </summary>
public class Matrix
{
    private readonly int[,] _matrix;

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="path">The path to the file with rectangle matrix.</param>
    /// <exception cref="FileNotFoundException">Throws when file with specified path does not exist.</exception>
    /// <exception cref="InvalidMatrixException">Throws when matrix is empty or its values are invalid, or number in each row is different.</exception>
    public Matrix(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("There is no file with such path.");
        }

        var rows = File.ReadLines(path).ToList();
        if (rows.Count == 0)
        {
            throw new InvalidMatrixException("Matrix cannot be empty.");
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
                    throw new InvalidMatrixException("Invalid matrix data was given.");
                }

                rowValues.Add(number);
            }

            if (i > 0 && rowValues.Count != matrix[i - 1].Length)
            {
                throw new InvalidMatrixException("Incorrect matrix was given.");
            }

            matrix.Add([..rowValues]);
        }

        _matrix = new int[matrix.Count, matrix[0].Length];

        for (var rowNumber = 0; rowNumber < matrix.Count; ++rowNumber)
        {
            for (var columnNumber = 0; columnNumber < matrix[0].Length; ++columnNumber)
            {
                _matrix[rowNumber, columnNumber] = matrix[rowNumber][columnNumber];
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="newMatrix">Matrix with the rectangle form.</param>
    /// <exception cref="ArgumentNullException">Throws when <see cref="newMatrix"/> is null.</exception>
    /// <exception cref="InvalidMatrixException">Throws when <see cref="newMatrix"/> .</exception>
    public Matrix(int[,] newMatrix)
    {
        ArgumentNullException.ThrowIfNull(newMatrix);

        if (newMatrix.GetLength(0) == 0 || newMatrix.GetLength(1) == 0)
        {
            throw new InvalidMatrixException("Invalid matrix was given.");
        }

        _matrix = new int[newMatrix.GetLength(0), newMatrix.GetLength(1)];

        for (var i = 0; i < newMatrix.GetLength(0); i++)
        {
            for (var j = 0; j < newMatrix.GetLength(1); j++)
            {
                _matrix[i, j] = newMatrix[i, j];
            }
        }
    }

    /// <summary>
    /// Gets number of rows in matrix.
    /// </summary>
    public int RowsNumber => _matrix.GetLength(0);

    /// <summary>
    /// Gets number of columns in matrix.
    /// </summary>
    public int ColumnsNumber => _matrix.GetLength(1);

    /// <summary>
    /// Gets the value by its indices.
    /// </summary>
    /// <param name="rowIndex">Number of row.</param>
    /// <param name="columnIndex">Number of column.</param>
    /// <returns>Value by specified incices.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws when <see cref="rowIndex"/> and <see cref="columnIndex"/> out of range.</exception>
    public int this[int rowIndex, int columnIndex]
    {
        get
        {
            if (!AreValidIndices(rowIndex, columnIndex))
            {
                throw new ArgumentOutOfRangeException();
            }

            return _matrix[rowIndex, columnIndex];
        }
    }

    /// <summary>
    /// Write matrix to the file.
    /// </summary>
    /// <param name="outputPath">Path where matrix will be located.</param>
    public void WriteToFile(string outputPath)
    {
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

        for (var i = 0; i < RowsNumber; ++i)
        {
            for (var j = 0; j < ColumnsNumber; ++j)
            {
                writer.Write(_matrix[i, j]);
                writer.Write(' ');
            }

            writer.Write('\n');
        }
    }

    /// <summary>
    /// Returns a value indicating this instance is equal to a specified <see cref="Matrix"/> instance.
    /// </summary>
    /// <param name="otherMatrix">Rectangle matrix.</param>
    /// <returns>true if <see cref="otherMatrix"/> values equals this matrix values; otherwise, false.</returns>
    public bool Equals(Matrix otherMatrix)
    {
        if (otherMatrix is null)
        {
            return false;
        }

        if (otherMatrix.RowsNumber != RowsNumber || otherMatrix.ColumnsNumber != ColumnsNumber)
        {
            return false;
        }

        for (var i = 0; i < RowsNumber; ++i)
        {
            for (var j = 0; j < ColumnsNumber; ++j)
            {
                if (otherMatrix[i, j] != _matrix[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool AreValidIndices(int rowIndex, int columnIndex)
        => rowIndex >= 0 && rowIndex < RowsNumber && columnIndex >= 0 && columnIndex < ColumnsNumber;
}