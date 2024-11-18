// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using FTPClient;

var client = new Client("localhost", 7777);

var path = "../";

var entries = await client.ListAsync(path);

Console.Write(entries.Count);
foreach (var (name, isDirectory) in entries)
{
    Console.Write($" {name} {isDirectory}");
}

Console.WriteLine();
