// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using MyNUnit;

var path = "../../../../TestCases/bin/Debug/net9.0/";

var result = TestRunner.RunTests(path);

foreach (var assemblyResult in result)
{
    await Printer.PrintReportForAssemblyAsync(assemblyResult, Console.Out);
}
