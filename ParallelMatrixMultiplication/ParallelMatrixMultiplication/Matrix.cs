// <copyright file="Matrix.cs" company="bulat-tsydendorzhiev">
// Copyright (c) bulat-tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace ParallelMatrixMultiplication;

using System.Text;

/// <summary>
/// Class, which has methods for matrix multiplication.
/// </summary>
public class Matrix
{
    /// <summary>
    /// Gets number of rows in matrix.
    /// </summary>
    public int RowsNumber => this._matrix.GetLength(0);

    /// <summary>
    /// Gets number of columns in matrix.
    /// </summary>
    public int ColumnsNumber => this._matrix.GetLength(1);

    private int[,] _matrix;

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// Create new matrix from file.
    /// </summary>
    /// <param name="path">Path to file with rectangle matrix.</param>
    /// <exception cref="FileNotFoundException">Throws when file with given path doesn't exist.</exception>
    /// <exception cref="IncorrectMatrixException">Throws when empty file was given.</exception>
    /// <exception cref="IncorrectMatrixException">Throws when incorrect matrix or invalid data was given.</exception>
    public Matrix(string path)
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
                    throw new IncorrectMatrixException("Invalid matrix data was given.");
                }

                rowValues.Add(number);
            }

            if (i > 0 && rowValues.Count != matrix[i - 1].Length)
            {
                throw new IncorrectMatrixException("Incorrect matrix was given.");
            }

            matrix.Add([..rowValues]);
        }

        this._matrix = new int[matrix.Count, matrix[0].Length];

        for (var rowNumber = 0; rowNumber < matrix.Count; ++rowNumber)
        {
            for (var columnNumber = 0; columnNumber < matrix[0].Length; ++columnNumber)
            {
                this._matrix[rowNumber, columnNumber] = matrix[rowNumber][columnNumber];
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="newMatrix">Matrix with the ractangle form.</param>
    /// <exception cref="ArgumentNullException">Throws when <see cref="newMatrix"/> is null.</exception>
    public Matrix(int[,] newMatrix)
    {
        ArgumentNullException.ThrowIfNull(newMatrix);

        this._matrix = newMatrix;
    }

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
            if (!this.AreValidIndices(rowIndex, columnIndex))
            {
                throw new ArgumentOutOfRangeException("Index out of range.");
            }

            return this._matrix[rowIndex, columnIndex];
        }
    }

    /// <summary>
    /// Write matrix to the file.
    /// </summary>
    /// <param name="outputPath">Path where matrix will be located.</param>
    public void WriteToFile(string outputPath)
    {
        using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
        {
            for (var i = 0; i < this.RowsNumber; ++i)
            {
                for (var j = 0; j < this.ColumnsNumber; ++j)
                {
                    writer.Write(this._matrix[i, j]);
                    writer.Write(' ');
                }

                writer.Write("\n");
            }
        }
    }

    /// <summary>
    /// Determines whether given values equals with this.
    /// </summary>
    /// <param name="otherMatrix">Rectangle matrix.</param>
    /// <returns>true if <see cref="otherMatrix"/> values equals this matrix values; otherwise false.</returns>
    public bool Equals(Matrix otherMatrix)
    {
        if (otherMatrix is null)
        {
            return false;
        }

        if (otherMatrix.RowsNumber != this.RowsNumber || otherMatrix.ColumnsNumber != this.ColumnsNumber)
        {
            return false;
        }

        for (var i = 0; i < this.RowsNumber; ++i)
        {
            for (var j = 0; j < this.ColumnsNumber; ++j)
            {
                if (otherMatrix[i, j] != this._matrix[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool AreValidIndices(int rowIndex, int columnIndex)
        => rowIndex >= 0 && rowIndex < this.RowsNumber && columnIndex >= 0 && columnIndex < this.ColumnsNumber;
}