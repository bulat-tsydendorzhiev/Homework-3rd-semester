// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using MyNUnit;

if (args.Length != 1)
{
    Console.WriteLine("Invalid number of arguments. Use command \"dotnet run -help\" to learn more.");
    return;
}

if (args[0] == "-help")
{
    Console.WriteLine("""
        This command-line application is used to run tests contained in all assemblies located at path.

        In order to use it enter the command:
        dotnet run <path>

        "path" - path to the directory

        Enjoy ;)
        """);
    return;
}

var path = args[0];

try
{
    var result = TestRunner.RunTests(path);

    foreach (var assemblyResult in result)
    {
        await Printer.PrintReportForAssemblyAsync(assemblyResult, Console.Out);
    }
}
catch (DirectoryNotFoundException e)
{
    Console.WriteLine(e.Message);
}