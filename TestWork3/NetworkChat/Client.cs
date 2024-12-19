// <copyright file="Client.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace NetworkChat.Client;

using System.Net;
using System.Net.Sockets;
using ChatMember;

/// <summary>
/// Client that sends/gets messages for/from the server.
/// </summary>
public class ChatClient : ChatMember
{
    private int _port;

    private string _hostName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatClient"/> class.
    /// </summary>
    /// <param name="port">Port for connecting to the server.</param>
    /// <param name="hostName">Host name.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws when port value is invalid.</exception>
    /// <exception cref="ArgumentException">Throws when host name is invalid.</exception>
    public ChatClient(int port, string hostName) : base("Server")
    {
        if (port < 0 || port > 65535)
        {
            throw new ArgumentOutOfRangeException("Port value must be more than 0 and less than 65536");
        }

        if (!IPAddress.TryParse(hostName, out _))
        {
            throw new ArgumentException("Incorrect host name.");
        }

        _hostName = hostName;
        _port = port;
    }

    /// <summary>
    /// Starts chat with server.
    /// </summary>
    /// <returns>Task that represents this method.</returns>
    public async Task StartAsync()
    {
        var client = new TcpClient();
        await client.ConnectAsync(_hostName, _port);

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream) { AutoFlush = true };

        var readerTask = Task.Run(async () => await ReadAsync(reader));
        var writerTask = Task.Run(async () => await WriteAsync(writer));

        await Task.WhenAny(readerTask, writerTask);
    }
}