// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using ParallelMatrixMultiplication;

if (args.Length > 0 && args[0] == "-help")
{
    var message = """
    This program multiplies two matrices and write the result into the file.
    
    Usage:
    dotnet run <1st matrix file> <2nd matrix file> <output file>
    
    """;

    Console.WriteLine(message);
    return;
}

if (args.Length != 3)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Incorrect number of data. Use -help to learn more.");
    return;
}

var path1 = args[0];
var path2 = args[1];
var outputPath = args[2];

try
{
    var firstMatrix = new Matrix(path1);
    var secondMatrix = new Matrix(path2);
    var result = MatrixMultiplier.Multiply(firstMatrix, secondMatrix);

    result.WriteToFile(outputPath);
    Console.WriteLine("Done.");
}
catch (DimensionsMismatchException ex)
{
    Console.WriteLine(ex.Message);
}
catch (InvalidMatrixException ex)
{
    Console.WriteLine(ex.Message);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}