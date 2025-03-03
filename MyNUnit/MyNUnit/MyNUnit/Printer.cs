// <copyright file="Printer.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
using MyNUnit.TestClasses;

namespace MyNUnit;

/// <summary>
/// Represents a class that prints report for tests.
/// </summary>
public static class Printer
{
    /// <summary>
    /// Prints report for assembly testing result.
    /// </summary>
    /// <param name="assemblyTestResult">Assembly testing result.</param>
    /// <param name="writer">Writer where will be report.</param>
    public static async Task PrintReportForAssemblyAsync(TestAssemblyResult assemblyTestResult, TextWriter writer)
    {
        await writer.WriteLineAsync("================================================================");
        await writer.WriteLineAsync($"Assembly name: {assemblyTestResult.Name}");
        await writer.WriteLineAsync($"Total duration of tests in test classes: {assemblyTestResult.TotalDuration}");

        foreach (var classResult in assemblyTestResult.TestResults)
        {
            await PrintReportForClassAsync(classResult, writer);
        }

        await writer.WriteLineAsync("================================================================");
    }

    private static async Task PrintReportForClassAsync(TestClassResult classTestResult, TextWriter writer)
    {
        await writer.WriteLineAsync("----------------------------------------------------------------");
        await writer.WriteLineAsync($"\tClass name: {classTestResult.Name}");
        await writer.WriteLineAsync($"\tTotal duration of tests: {classTestResult.TotalDuration}");

        foreach (var testResult in classTestResult.TestResults)
        {
            await PrintReportForTestAsync(testResult, writer);
        }
    }

    private static async Task PrintReportForTestAsync(TestMethodResult testResult, TextWriter writer)
    {
        await writer.WriteLineAsync($"\t\tTest name: {testResult.Name}");
        await writer.WriteLineAsync($"\t\tDuration: {testResult.Duration}");

        switch (testResult.Status)
        {
            case MyTestStatus.Passed:
                await writer.WriteLineAsync("\t\tTest result: passed");
                break;

            case MyTestStatus.Failed:
                await writer.WriteLineAsync("\t\tTest result: failed");
                await writer.WriteLineAsync($"\t\tTest error: {testResult.ErrorMessage}");
                break;

            case MyTestStatus.Ignored:
                await writer.WriteLineAsync("\t\tTest result: ignored");
                await writer.WriteLineAsync($"\t\tIgnore reason: {testResult.IgnoreMessage}");
                break;
        }

        await writer.WriteLineAsync();
    }
}
