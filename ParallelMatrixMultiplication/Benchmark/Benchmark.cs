using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ParallelMatrixMultiplication;

/// <summary>
/// Class for comparing work of two matrix multipliers.
/// </summary>
public class Benchmark
{
    private Random _random = new();

    private List<(double ExpectedValue, double StandartDeviation)> _sequentialResults = []; 

    private List<(double ExpectedValue, double StandartDeviation)> _parallelResults = [];

    private List<(int Rows, int Columns)> _sizes = [(50, 50), (100, 100), (250, 300), (500, 500), (1000,1000)];

    private readonly Stopwatch _stopwatch = new();

    /// <summary>
    /// Number of table columns (Matrix multiplier, size1 x size2, expected value, standart deviation).
    /// </summary>
    private const int NumberOfTableColumns = 4;

    private const int NumberOfLaunches = 5;

    /// <summary>
    /// Runs benchmark to compare speed of sequential and parallel matrix multiplications.
    /// Saves results in .pdf file.
    /// </summary>
    /// <param name="outputFileName">File name for saving in .pdf format.</param>
    /// <exception cref="FileNotFoundException">Throws when file with specified path doesn't exist.</exception>
    public void Run(string outputFileName)
    {
        for (var i = 0; i < _sizes.Count; ++i)
        {
            var _sequentialTimeResults = new long[NumberOfLaunches];
            var _parallelTimeResults = new long[NumberOfLaunches];
            
            for (var j = 0; j < NumberOfLaunches; ++j)
            {
                var firstMatrix = GenerateMatrix(_sizes[i].Rows, _sizes[i].Columns);
                var secondMatrix = GenerateMatrix(_sizes[i].Columns, _sizes[i].Rows);

                var sequentialTime = CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.Multiply);
                var parallelTime = CalculateTime(firstMatrix, secondMatrix, MatrixMultiplier.MultiplyInParallel);

                _sequentialTimeResults[j] = sequentialTime;
                _parallelTimeResults[j] = parallelTime;
            }

            _sequentialResults.Add(GetExpectedValueAndStandartDeviation(_sequentialTimeResults));
            _parallelResults.Add(GetExpectedValueAndStandartDeviation(_parallelTimeResults));
        }

        WriteDataToFile(outputFileName);
    }

    private Matrix GenerateMatrix(int numberOfRows, int numberOfColumns)
    {
        int[,] newMatrix = new int[numberOfRows, numberOfColumns];
        
        for (var i = 0; i < numberOfRows; ++i)
        {
            for (var j = 0; j < numberOfColumns; ++j)
            {
                newMatrix[i, j] = _random.Next(-1000, 1000);
            }
        }

        return new Matrix(newMatrix);
    }

    private long CalculateTime(Matrix firstMatrix, Matrix secondMatrix, Func<Matrix, Matrix, Matrix> Multiply)
    {
        _stopwatch.Restart();
        Multiply(firstMatrix, secondMatrix);
        _stopwatch.Stop();

        return _stopwatch.ElapsedMilliseconds;
    }

    private static (double,double) GetExpectedValueAndStandartDeviation(long[] timeResults)
    {
        var expectedValue = timeResults.Sum() * 1d / timeResults.Length;
        var standartDeviation = Math.Sqrt(timeResults.Sum(t => (t - expectedValue) * (t - expectedValue)) / (timeResults.Length - 1));
        return (expectedValue, standartDeviation);
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

        for (var i = 0; i < _sizes.Count; ++i)
        {
            AddRow(cells, "Sequential", i, font);
            AddRow(cells, "Parallel", i, font);
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

    private void AddRow(List<PdfPCell> cells, string MatrixMultiplierName, int numberOfRow, Font font)
    {
        cells.Add(new PdfPCell(new Phrase(MatrixMultiplierName, font)));
        cells.Add(new PdfPCell(new Phrase($"{_sizes[numberOfRow].Rows} x {_sizes[numberOfRow].Columns}", font)));

        if (MatrixMultiplierName == "Parallel")
        {
            cells.Add(new PdfPCell(new Phrase(_parallelResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(_parallelResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
        else
        {
            cells.Add(new PdfPCell(new Phrase(_sequentialResults[numberOfRow].ExpectedValue.ToString(), font)));
            cells.Add(new PdfPCell(new Phrase(_sequentialResults[numberOfRow].StandartDeviation.ToString(), font)));
        }
    }
}