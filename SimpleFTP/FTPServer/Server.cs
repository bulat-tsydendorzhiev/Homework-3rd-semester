// <copyright file="Server.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace FTPServer;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Server that allows to make listing of files in directory on it and to download file.
/// </summary>
public class Server
{
    private readonly CancellationTokenSource _cts;

    private TcpListener _listener;

    /// <summary>
    /// Initializes a new instance of the <see cref="Server"/> class.
    /// </summary>
    /// <param name="port">Port of FTP server.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws when port value is invalid.</exception>
    public Server(int port)
    {
        if (port < 0 || port > 65535)
        {
            throw new ArgumentOutOfRangeException("Port value must be more than 0 and less than 65536");
        }

        _listener = new (IPAddress.Any, port);
        _cts = new ();
    }

    /// <summary>
    /// Starts server work.
    /// </summary>
    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"Server started work with port {((IPEndPoint)_listener.LocalEndpoint).Port}.");

        var remainingTasks = new List<Task>();

        while (!_cts.Token.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(_cts.Token);

            var task = Task.Run(async () =>
            {
                await using var stream = client.GetStream();

                using var reader = new StreamReader(stream);
                using var writer = new StreamWriter(stream) { AutoFlush = true };

                var request = await reader.ReadLineAsync(_cts.Token);

                if (request != null)
                {
                    if (request[0] == '1')
                    {
                        await ListAsync(request[2..], writer);
                    }
                    else if (request[0] == '2')
                    {
                        await GetAsync(request[2..], writer);
                    }
                }

                client.Close();
            });

            remainingTasks.Add(task);
        }

        await Task.WhenAll([.. remainingTasks]);

        _listener.Stop();
    }

    /// <summary>
    /// Allows the remaining tasks to complete and after that stops server work.
    /// </summary>
    public void Stop()
        => _cts.Cancel();

    private static async Task ListAsync(string path, StreamWriter writer)
    {
        if (!Directory.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }

        var entries = Directory.GetFileSystemEntries(path).ToArray();
        Array.Sort(entries);

        await writer.WriteAsync($"{entries.Length}");

        foreach (var entry in entries)
        {
            await writer.WriteAsync($" {entry} {Directory.Exists(entry)}");
        }

        await writer.WriteLineAsync();
    }

    private static async Task GetAsync(string path, StreamWriter writer)
    {
        if (!File.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }

        var content = await File.ReadAllBytesAsync(path);

        await writer.WriteLineAsync($"{content.LongLength} {Encoding.UTF8.GetString(content)}");
    }
}