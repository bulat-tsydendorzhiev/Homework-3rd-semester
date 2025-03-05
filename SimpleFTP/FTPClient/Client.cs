// <copyright file="Client.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace FTPClient;

using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

/// <summary>
/// Client that allows to make list and get requests.
/// List request - listing files in a directory on the server;
/// Get request - downloading file from server.
/// </summary>
public class Client
{
    private readonly string _hostName;

    private readonly int _port;

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="hostName">Host name.</param>
    /// <param name="port">Port for connecting to the server.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws when port value is invalid.</exception>
    public Client(string hostName, int port)
    {
        if (port < 0 || port > 65535)
        {
            throw new ArgumentOutOfRangeException("Port value must be more than 0 and less than 65536");
        }

        _hostName = hostName;
        _port = port;
    }

    /// <summary>
    /// Lists files in server directory.
    /// </summary>
    /// <param name="path">Path to the directory on the server.</param>
    public async Task<List<(string name, bool isDirectory)>> ListAsync(string path)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_hostName, _port);

        await using var stream = client.GetStream();

        var request = $"1 {path}\n";

        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(request);
        await writer.FlushAsync();

        return await HandleListAsync(stream);
    }

    /// <summary>
    /// Gets file content from the server.
    /// </summary>
    /// <param name="path">Path to the file on the server.</param>
    public async Task<byte[]> GetAsync(string path)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_hostName, _port);

        await using var stream = client.GetStream();

        var request = $"2 {path}\n";

        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(request);
        await writer.FlushAsync();

        return HandleGet(stream);
    }

    private static async Task<List<(string name, bool isDirectory)>> HandleListAsync(NetworkStream stream)
    {
        using var reader = new StreamReader(stream);

        var response = await reader.ReadLineAsync() ?? throw new NetworkInformationException();
        var entries = response.Split();

        var entriesLength = int.Parse(entries[0]);
        if (entriesLength == -1)
        {
            throw new DirectoryNotFoundException("There is no such directory on the server.");
        }

        var result = new List<(string, bool)>();
        for (var i = 1; i <= entriesLength; i++)
        {
            result.Add((entries[(2 * i) - 1], bool.Parse(entries[2 * i])));
        }

        return result;
    }

    private static byte[] HandleGet(NetworkStream stream)
    {
        var reader = new BinaryReader(stream);
        var size = reader.ReadInt64();
        if (size == -1)
        {
            throw new FileNotFoundException("There is no such file in the server directory.");
        }

        var result = new byte[size];
        for (long i = 0; i < size; i++)
        {
            result[i] = (byte)stream.ReadByte();
        }

        return result;
    }
}