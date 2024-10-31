// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using System.Diagnostics;
using TestWork1;

if (args.Length != 1)
{
    Console.WriteLine("Incorrect number of items.\n" +
                      "Enter path to directory.");

    return;
}

var stopwatch = new Stopwatch();
var path = args[0];

stopwatch.Start();
SingleThreadedCheckSumCalculator.CalculateCheckSum(path);
stopwatch.Stop();

var singleThreadedCheckSumCalculatorTime = stopwatch.ElapsedMilliseconds;

stopwatch.Restart();
MultiThreadedCheckSumCalculator.CalculateCheckSum(path);
stopwatch.Stop();

var multiThreadedCheckSumCalculatorTime = stopwatch.ElapsedMilliseconds;

var difference = multiThreadedCheckSumCalculatorTime - singleThreadedCheckSumCalculatorTime;

Console.WriteLine($"Difference between time of multi- and single- threaded check sum calculators (ms) = {difference}");