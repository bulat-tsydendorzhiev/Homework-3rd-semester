// <copyright file="Program.cs" company="bulat-tsydendorzhiev">
// Copyright (c) bulat-tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ParallelMatrixMultiplication;

/// <summary>
/// Class for comparing work of two matrix multipliers.
/// </summary>
public class Benchmark
{
    /// <summary>
    /// Number of table columns (Matrix multiplier, size1 x size2, expected value, standart deviation).
    /// </summary>
    private const int NumberOfTableColumns = 4;

    private const int NumberOfLaunches = 5;

    private readonly Stopwatch stopwatch = new ();

    private readonly Random random = new ();

    private readonly List<(double ExpectedValue, double StandartDeviation)> sequentialResults = new ();

    private readonly List<(double ExpectedValue, double StandartDeviation)> parallelResults = new ();

    private readonly List<(int Rows, int Columns)> sizes = [(50, 50), (100, 100), (250, 300), (500, 500), (1000, 1000)];

    /// <summary>
    /// Runs benchmark to compare speed of sequential and parallel matrix multiplications.
    /// Saves results in .pdf file.
    /// </summary>
    /// <param name="outputFileName">File name for saving in .pdf format.</param>
    /// <exception cref="FileNotFoundException">Throws when file with specified path doesn't exist.</exception>
    public void Run(string outputFileName)
    {
        for (var i = 0; i < this.sizes.Count; ++i)
        {
            var sequentialTimeResults = new long[NumberOfLaunches];
            var parallelTimeResults = new long[NumberOfLaunches];

            for (var j = 0; j < NumberOfLaunches; ++j)
            {
                var firstMatrix = this.GenerateMatrix(this.sizes[i].Rows, this.sizes[i].Columns);
                var secondMatrix = this.GenerateMatrix(this.sizes[i].Columns, this.sizes[i].Rows);

                var sequentialTime = this.CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.Multiply);
                var parallelTime = this.CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.MultiplyInParallel);

                sequentialTimeResults[j] = sequentialTime;
                parallelTimeResults[j] = parallelTime;
            }

            this.sequentialResults.Add(GetExpectedValueAndStandartDeviation(sequentialTimeResults));
            this.parallelResults.Add(GetExpectedValueAndStandartDeviation(parallelTimeResults));
        }

        this.WriteDataToFile(outputFileName);
    }

    private static (double, double) GetExpectedValueAndStandartDeviation(long[] timeResults)
    {
        var expectedValue = timeResults.Sum() * 1d / timeResults.Length;
        var standartDeviation = Math.Sqrt(timeResults.Sum(t => (t - expectedValue) * (t - expectedValue)) / (timeResults.Length - 1));
        return (expectedValue, standartDeviation);
    }

    private Matrix GenerateMatrix(int numberOfRows, int numberOfColumns)
    {
        int[,] newMatrix = new int[numberOfRows, numberOfColumns];

        for (var i = 0; i < numberOfRows; ++i)
        {
            for (var j = 0; j < numberOfColumns; ++j)
            {
                newMatrix[i, j] = this.random.Next(-1000, 1000);
            }
        }

        return new Matrix(newMatrix);
    }

    private long CalculateTime(Matrix firstMatrix, Matrix secondMatrix, Func<Matrix, Matrix, Matrix> method)
    {
        this.stopwatch.Restart();
        method(firstMatrix, secondMatrix);
        this.stopwatch.Stop();

        return this.stopwatch.ElapsedMilliseconds;
    }

    private void WriteDataToFile(string outputFileName)
    {
        var baseFont = BaseFont.CreateFont("Helvetica", "Cp1252", false);
        var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
        var table = new PdfPTable(NumberOfTableColumns);

        List<PdfPCell> cells =
        [
            new PdfPCell(new Phrase("Matrix multiplier", font)),
            new PdfPCell(new Phrase("Size1 x Size2", font)),
            new PdfPCell(new Phrase("Expected value (ms)", font)),
            new PdfPCell(new Phrase("Standart deviation (ms)", font))
        ];

        for (var i = 0; i < this.sizes.Count; ++i)
        {
            this.AddRow(cells, "Sequential", i, font);
            this.AddRow(cells, "Parallel", i, font);
        }

        foreach (var cell in cells)
        {
            table.AddCell(cell);
        }

        var pdfDocument = new Document();
        PdfWriter.GetInstance(pdfDocument, new FileStream($"{outputFileName}.pdf", FileMode.OpenOrCreate));

        pdfDocument.Open();
        pdfDocument.Add(table);
        pdfDocument.Close();

        Console.WriteLine("Benchmark is done.");
    }

    private void AddRow(List<PdfPCell> cells, string matrixMultiplierName, int numberOfRow, Font font)
    {
        cells.Add(new PdfPCell(new Phrase(matrixMultiplierName, font)));
        cells.Add(new PdfPCell(new Phrase($"{this.sizes[numberOfRow].Rows} x {this.sizes[numberOfRow].Columns}", font)));

        if (matrixMultiplierName == "Parallel")
        {
            cells.Add(new PdfPCell(new Phrase(this.parallelResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(this.parallelResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
        else
        {
            cells.Add(new PdfPCell(new Phrase(this.sequentialResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(this.sequentialResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
    }
}