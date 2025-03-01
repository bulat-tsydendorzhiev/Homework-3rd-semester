// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using FTPClient;

if (args.Length != 2)
{
    Console.WriteLine("Invalid number of arguments. Enter \"dotnet run -help\" for more information.");
    return;
}

if (args[0] == "-help")
{
    Console.WriteLine("""
    This programme allows to get file content or to make listing of a local server directory.
    
    In order to use it enter: dotnet run <request> <path>
    "request" is number(1 or 2) that shows which option you want to use:
    1 - make listing
    2 - get file content
    "path" is specified path to directory/file if "request" is 1/2
    
    Enjoy ;)
    """);
    return;
}

if (!int.TryParse(args[0], out int request) && (request != 1 || request != 2))
{
    Console.WriteLine("Invalid request value.");
    return;
}

var path = args[1];
try
{
    var client = new Client("localhost", 12345);

    switch (request)
    {
        case 1:
        {
            var entries = await client.ListAsync(path);

            Console.Write(entries.Count);
            foreach (var (name, isDirectory) in entries)
            {
                Console.Write($" {name} {isDirectory}");
            }

            Console.WriteLine();
            break;
        }

        case 2:
        {
            var content = await client.GetAsync(path);

            Console.WriteLine($"{content.LongLength} {content}");
            break;
        }
    }
}
catch (DirectoryNotFoundException e)
{
    Console.WriteLine(e.Message);
}
catch (FileNotFoundException e)
{
    Console.WriteLine(e.Message);
}