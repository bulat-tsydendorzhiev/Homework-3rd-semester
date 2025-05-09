// <copyright file="SingleThreadedCheckSumCalculator.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace TestWork1;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Single-threaded calculator.
/// </summary>
public static class SingleThreadedCheckSumCalculator
{
    /// <summary>
    /// Calculates check-sum for directory.
    /// </summary>
    /// <param name="path">Path to directory.</param>
    /// <returns>Check sum.</returns>
    /// <exception cref="DirectoryNotFoundException">Throws when directory is not found.</exception>
    public static byte[] CalculateCheckSum(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException();
        }

        return CalculateDirectoryCheckSum(path);
    }

    private static byte[] CalculateFileCheckSum(string filePath) => MD5.HashData(File.ReadAllBytes(filePath));

    private static byte[] CalculateDirectoryCheckSum(string directoryPath)
    {
        var checkSum = MD5.HashData(Encoding.UTF8.GetBytes(directoryPath));

        var fileNames = Directory.GetFiles(directoryPath);
        var subDirectories = Directory.GetDirectories(directoryPath);

        Array.Sort(fileNames);
        Array.Sort(subDirectories);

        foreach (var fileName in fileNames)
        {
            checkSum.Concat(CalculateFileCheckSum(fileName));
        }

        foreach (var subDirectory in subDirectories)
        {
            checkSum.Concat(CalculateDirectoryCheckSum(subDirectory));
        }

        return checkSum;
    }
}