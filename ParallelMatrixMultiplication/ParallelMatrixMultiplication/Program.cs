using ParallelMatrixMultiplication;

if (args.Length > 0 && args[0] == "-help")
{
    var message = """
    This is a program multiplies two matrices and write the result into the file.
    
    Usage:
    dotnet run <1st matrix file> <2nd matrix file>
    
    """;

    Console.WriteLine(message);
    return 0;
}

if (args.Length != 2)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Incorrect number of data. Use -help to learn more.");
    return 0;
}

var path1 = args[0];
var path2 = args[1];

try
{
    var firstMatrix = new Matrix(path1);
    var secondMatrix = new Matrix(path2);
    var result = MatrixMultiplier.Multiply(firstMatrix, secondMatrix); 
    
    result.WriteToFile("ResultOfMultiplication");
    Console.WriteLine("Done.");
}
catch (DimensionsMismatchException ex)
{
    Console.WriteLine(ex.Message);
}
catch (IncorrectMatrixException ex)
{
    Console.WriteLine(ex.Message);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

return 0;