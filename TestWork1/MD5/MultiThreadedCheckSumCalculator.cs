// <copyright file="MultiThreadedCheckSumCalculator.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace TestWork1;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Multi-threaded calculator.
/// </summary>
public class MultiThreadedCheckSumCalculator
{
    /// <summary>
    /// Calculate check-sum for directory.
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

    private static async Task<byte[]> CalculateFileCheckSum(string filePath)
    {
        var fileContent = await File.ReadAllBytesAsync(filePath);

        return MD5.HashData(fileContent);
    }

    private static byte[] CalculateDirectoryCheckSum(string directoryPath)
    {
        var checkSum = MD5.HashData(Encoding.UTF8.GetBytes(directoryPath));

        var fileNames = Directory.GetFiles(directoryPath);
        var subDirectories = Directory.GetDirectories(directoryPath);

        Array.Sort(fileNames);
        Array.Sort(subDirectories);

        foreach (var fileName in fileNames)
        {
            checkSum.Concat(CalculateFileCheckSum(fileName).Result);
        }

        foreach (var subDirectory in subDirectories)
        {
            checkSum.Concat(CalculateDirectoryCheckSum(subDirectory));
        }

        return checkSum;
    }
}