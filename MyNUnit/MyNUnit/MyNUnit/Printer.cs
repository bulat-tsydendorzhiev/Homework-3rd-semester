// <copyright file="Printer.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace MyNUnit;

using MyNUnit.TestComponents;

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
        await writer.WriteLineAsync($"Total duration of tests in test classes(ms): {assemblyTestResult.TotalDuration}");

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
        await writer.WriteLineAsync($"\tTotal duration of tests(ms): {classTestResult.TotalDuration}");

        if (classTestResult.InvalidMethodsName is not null)
        {
            await writer.WriteLineAsync("\n\tTests weren't invoked because of the following invalid methods:");

            foreach (var invalidMethodName in classTestResult.InvalidMethodsName)
            {
                await writer.WriteLineAsync($"\t- {invalidMethodName}");
            }

            return;
        }

        foreach (var testResult in classTestResult.TestResults)
        {
            await PrintReportForTestAsync(testResult, writer);
        }
    }

    private static async Task PrintReportForTestAsync(TestMethodResult testResult, TextWriter writer)
    {
        await writer.WriteLineAsync($"\t\tTest name: {testResult.Name}");
        await writer.WriteLineAsync($"\t\tDuration(ms): {testResult.Duration}");

        switch (testResult.Status)
        {
            case MyTestStatus.Passed:
                await writer.WriteLineAsync("\t\tTest result: passed");
                break;

            case MyTestStatus.Failed:
                await writer.WriteLineAsync("\t\tTest result: failed");
                await writer.WriteLineAsync($"\t\tError reason: {testResult.ErrorMessage}");
                break;

            case MyTestStatus.Ignored:
                await writer.WriteLineAsync("\t\tTest result: ignored");
                await writer.WriteLineAsync($"\t\tIgnore reason: {testResult.IgnoreMessage}");
                break;
            case MyTestStatus.Cancelled:
                await writer.WriteLineAsync("\t\tTest result: cancelled");
                await writer.WriteLineAsync($"\t\tReason of cancel: {testResult.ErrorMessage}");
                break;
        }

        await writer.WriteLineAsync();
    }
}
