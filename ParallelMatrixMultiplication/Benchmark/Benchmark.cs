// <copyright file="Benchmark.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
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
    /// Number of table columns (Matrix multiplier, Number of rows x Number of columns, expected value, standart deviation).
    /// </summary>
    private const int NumberOfTableColumns = 4;

    private const int NumberOfLaunches = 10;

    private const int NumberOfFractionalDigits = 3;

    private readonly Stopwatch stopwatch = new ();

    private readonly Random random = new ();

    private readonly List<(double ExpectedValue, double StandartDeviation)> sequentialResults = new ();

    private readonly List<(double ExpectedValue, double StandartDeviation)> parallelResults = new ();

    private readonly List<(int Rows, int Columns)> sizes = new () { (50, 50), (100, 100), (250, 300), (500, 500), (1000, 1000) };

    /// <summary>
    /// Runs benchmark to compare speed of sequential and parallel matrix multiplications.
    /// Saves results in "<see cref="outputFileName"/>".pdf file.
    /// </summary>
    /// <param name="outputFileName">File name for saving in PDF format.</param>
    public void Run(string outputFileName)
    {
        for (var i = 0; i < sizes.Count; ++i)
        {
            var sequentialTimeResults = new long[NumberOfLaunches];
            var parallelTimeResults = new long[NumberOfLaunches];

            for (var j = 0; j < NumberOfLaunches; ++j)
            {
                var firstMatrix = GenerateMatrix(sizes[i].Rows, sizes[i].Columns);
                var secondMatrix = GenerateMatrix(sizes[i].Columns, sizes[i].Rows);

                var sequentialTime = CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.Multiply);
                var parallelTime = CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.MultiplyInParallel);

                sequentialTimeResults[j] = sequentialTime;
                parallelTimeResults[j] = parallelTime;
            }

            sequentialResults.Add(GetExpectedValueAndStandartDeviation(sequentialTimeResults));
            parallelResults.Add(GetExpectedValueAndStandartDeviation(parallelTimeResults));
        }

        WriteDataToFile(outputFileName);

        Console.WriteLine("Benchmark is done.");
    }

    private static (double, double) GetExpectedValueAndStandartDeviation(long[] timeResults)
    {
        var expectedValue = timeResults.Sum() * 1d / timeResults.Length;
        var standartDeviation = Math.Sqrt(timeResults.Sum(t => (t - expectedValue) * (t - expectedValue)) / (timeResults.Length - 1));

        expectedValue = Math.Round(expectedValue / 1000, NumberOfFractionalDigits);
        standartDeviation = Math.Round(standartDeviation / 1000, NumberOfFractionalDigits) * 2;

        return (expectedValue, standartDeviation);
    }

    private Matrix GenerateMatrix(int numberOfRows, int numberOfColumns)
    {
        int[,] newMatrix = new int[numberOfRows, numberOfColumns];

        for (var i = 0; i < numberOfRows; ++i)
        {
            for (var j = 0; j < numberOfColumns; ++j)
            {
                newMatrix[i, j] = random.Next(-1000, 1000);
            }
        }

        return new Matrix(newMatrix);
    }

    private long CalculateTime(Matrix firstMatrix, Matrix secondMatrix, Func<Matrix, Matrix, Matrix> method)
    {
        stopwatch.Restart();
        method(firstMatrix, secondMatrix);
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    private void WriteDataToFile(string outputFileName)
    {
        var baseFont = BaseFont.CreateFont("Helvetica", "Cp1252", false);
        var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
        var table = new PdfPTable(NumberOfTableColumns);

        List<PdfPCell> cells =
        [
            new PdfPCell(new Phrase("Matrix multiplier", font)),
            new PdfPCell(new Phrase("Rows number x Columns number", font)),
            new PdfPCell(new Phrase("Expected value (sec)", font)),
            new PdfPCell(new Phrase("Standart deviation (sec)", font))
        ];

        for (var i = 0; i < sizes.Count; ++i)
        {
            AddRow(cells, false, i, font);
            AddRow(cells, true, i, font);
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
    }

    private void AddRow(List<PdfPCell> cells, bool isParallel, int numberOfRow, Font font)
    {
        cells.Add(new PdfPCell(new Phrase(isParallel ? "Parallel" : "Sequential", font)));
        cells.Add(new PdfPCell(new Phrase($"{sizes[numberOfRow].Rows} x {sizes[numberOfRow].Columns}", font)));

        if (isParallel)
        {
            cells.Add(new PdfPCell(new Phrase(parallelResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(parallelResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
        else
        {
            cells.Add(new PdfPCell(new Phrase(sequentialResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(sequentialResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
    }
}