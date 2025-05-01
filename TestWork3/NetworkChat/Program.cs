// <copyright file="Program.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

using System.Net;
using NetworkChat.Client;
using NetworkChat.Server;

static void SendMessage()
{
    var message = """
    This program provides chat between server and client.
    
    In order to use this program, you should use command:
    dotnet run <port> [ip address]
    
    """;
    Console.WriteLine(message);
}

switch (args.Length)
{
    case 1:
    {
        if (!int.TryParse(args[0], out int port) || port < 0 || port > 65535)
        {
            Console.WriteLine("Incorrect port value.");
            SendMessage();
        }

        var server = new ChatServer(int.Parse(args[0]));

        await server.StartAsync();

        break;
    }
    case 2:
    {
        if (!int.TryParse(args[0], out int port) || port < 0 || port > 65535)
        {
            Console.WriteLine("Incorrect port value.");
            SendMessage();
            break;
        }

        if (!IPAddress.TryParse(args[1], out _))
        {
            Console.WriteLine("Incorrect ip address.");
            SendMessage();
            break;
        }

        var client = new ChatClient(int.Parse(args[0]), args[1]);

        await client.StartAsync();

        break;
    }
    default:
    {
        SendMessage();
        break;
    }
}