// <copyright file="SimpleFTPTests.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>
namespace SimpleFTP.Tests;

using FTPClient;
using FTPServer;

public class Tests
{
    private const int Port = 8888;
    private const string HostName = "localhost";

    private Server _server = new (Port);

    [OneTimeSetUp]
    public void Setup()
    {
        Task.Run(_server.Start);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _server.Stop();
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.Directories))]
    public async Task ListAsync_ShouldReturn_ExpectedEntries(string path)
    {
        var client = new Client(HostName, Port);
        var entries = await client.ListAsync(path);

        Assert.That(CheckEntries(path, entries), Is.True);
    }

    [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.Files))]
    public async Task GetAsync_ShouldReturn_ExpectedFileContent(string path)
    {
        var client = new Client(HostName, Port);
        var fileContent = await client.GetAsync(path);

        var expectedFileContent = File.ReadAllBytes(path);

        Assert.That(fileContent, Is.EqualTo(expectedFileContent));
    }

    [Test]
    public void FileNotFoundException_ShouldBeThrown_WithInvalidFilePath()
    {
        var client = new Client(HostName, Port);
        Assert.ThrowsAsync<FileNotFoundException>(async () => await client.GetAsync("../NonExistentFile.txt"));
    }

    [Test]
    public void DirectoryNotFoundException_ShouldBeThrown_WithInvalidDirectoryPath()
    {
        var client = new Client(HostName, Port);
        Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await client.ListAsync("../NonExistentDirectory"));
    }

    [TestCase(-1)]
    [TestCase(65536)]
    public void ArgumentOutOfRangeException_ShouldBeThrown_WithInvalidPortValue(int port)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Server(port));
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Client(HostName, port));
    }

    private static bool CheckEntries(string path, List<(string Name, bool IsDirectory)> entries)
    {
        var actualEntries = Directory.GetFileSystemEntries(path);

        if (actualEntries.Length != entries.Count)
        {
            return false;
        }

        Array.Sort(actualEntries);

        for (var i = 0; i < actualEntries.Length; ++i)
        {
            if (actualEntries[i] != entries[i].Name || Directory.Exists(actualEntries[i]) != entries[i].IsDirectory)
            {
                return false;
            }
        }

        return true;
    }

    private class TestDataClass
    {
        public static string[] Directories =
        {
            "../../../TestDirectory",
            "../../../TestDirectory/Subdirectory",
            "../../../TestDirectory/Subdirectory/EmptyDirectory"
        };

        public static string[] Files =
        {
            "../../../TestDirectory/textFile.txt",
            "../../../TestDirectory/Subdirectory/emptyFile.txt",
            "../../../SimpleFTP.Tests.csproj"
        };
    }
}