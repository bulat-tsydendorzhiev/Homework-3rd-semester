using System.Diagnostics;
using System.Text;
using ParallelMatrixMultiplication;

/// <summary>
/// Class for comparing work of two matrix multipliers.
/// </summary>
public class MatrixMultipliersBenchmark
{
    private Random _random = new();

    private List<(double ExpectedValue, double StandartDeviation)> _sequentialResults = []; 

    private List<(double ExpectedValue, double StandartDeviation)> _parallelResults = [];

    private List<(int Rows, int Columns)> _sizes = [(50, 50), (40, 80), (100, 100), (250, 300), (500, 500), (1000,1000)];

    private Stopwatch _stopwatch = new();

    private const int NumberOfLaunches = 5;

    /// <summary>
    /// Runs benchmark to compare speed of sequential and parallel matrix multiplications.
    /// </summary>
    /// <param name="outputPath">File where results will be written.</param>
    /// <exception cref="FileNotFoundException">Throws when file with specified path doesn't exist.</exception>
    public void RunBenchmark(string outputPath)
    {
        if (!File.Exists(outputPath))
        {
            throw new FileNotFoundException("There is no such file.");
        }
        
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

        WriteDataToFile(outputPath);
    }

    private long CalculateTime(Matrix firstMatrix, Matrix secondMatrix, Func<Matrix, Matrix, Matrix> Multiply)
    {
        _stopwatch.Restart();
        Multiply(firstMatrix, secondMatrix);
        _stopwatch.Stop();

        return _stopwatch.ElapsedMilliseconds;
    }

    private void WriteDataToFile(string outputPath)
    {
        using(var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
        {
            writer.WriteLine("Matrix multiplier | Size1 x Size2 | Expected value | Standart deviation");
            
            for (var i = 0; i < _sizes.Count; ++i)
            {
                writer.WriteLine($"Sequential  | {_sizes[i].Rows} x {_sizes[i].Columns} |" 
                + $" {_sequentialResults[i].ExpectedValue} | {_sequentialResults[i].StandartDeviation}");
                writer.WriteLine($"Parallel | {_sizes[i].Rows} x {_sizes[i].Columns} |" 
                + $" {_parallelResults[i].ExpectedValue} | {_parallelResults[i].StandartDeviation}");
            }
        }
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

    private static (double,double) GetExpectedValueAndStandartDeviation(long[] timeResults)
    {
        var expectedValue = timeResults.Sum() * 1d / timeResults.Length;
        var standartDeviation = Math.Sqrt(timeResults.Sum(t => (t - expectedValue) * (t - expectedValue)) / (timeResults.Length - 1));
        return (expectedValue, standartDeviation);
    }
}