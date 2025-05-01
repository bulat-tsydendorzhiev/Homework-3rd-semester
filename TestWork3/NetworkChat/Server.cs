// <copyright file="Server.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace NetworkChat.Server;

using System.Net;
using System.Net.Sockets;
using ChatMember;

/// <summary>
/// Server that sends/gets messages for/from the client.
/// </summary>
public class ChatServer : ChatMember
{
    private TcpListener _listener;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatServer"/> class.
    /// </summary>
    /// <param name="port">Port for connecting to the server.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws when port value is invalid.</exception>
    public ChatServer(int port) : base("Client")
    {
        _listener = new (IPAddress.Any, port);
    }

    /// <summary>
    /// Starts chat with client.
    /// </summary>
    /// <returns>Task that represents this method.</returns>
    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine($"Server started work.");

        var client = await _listener.AcceptTcpClientAsync(_cts.Token);
        Console.WriteLine("Client connected.");

        using var clientStream = client.GetStream();
        using var reader = new StreamReader(clientStream);
        using var writer = new StreamWriter(clientStream) { AutoFlush = true };

        var readerTask = Task.Run(async () => await ReadAsync(reader));
        var writerTask = Task.Run(async () => await WriteAsync(writer));

        await Task.WhenAny(readerTask, writerTask);

        _listener.Stop();
    }
}