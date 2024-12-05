namespace SimpleFTP.Tests;

using FTPClient;
using FTPServer;

public class Tests
{
    private const int Port = 8888;

    private const string HostName = "localhost";

    private Server _server = new Server(Port);

    [OneTimeSetUp]
    public async Task Setup()
    {
        await _server.Start();
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

        Assert.That(CheckEntries(path, entries), Is.EqualTo(true));
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
    public void FileNotFoundException_ShouldBeThrown_WithInvalidDirectoryPath()
    {
        var client = new Client(HostName, Port);
        Assert.Throws<FileNotFoundException>(async () => await client.GetAsync("NonExistentFile.txt"));
    }

    [Test]
    public void DirectoryNotFoundException_ShouldBeThrown_WithInvalidDirectoryPath()
    {
        var client = new Client(HostName, Port);
        Assert.Throws<DirectoryNotFoundException>(async () => await client.ListAsync("../NonExistentDirectory"));
    }

    [TestCase(-1)]
    [TestCase(65536)]
    public void ArgumentOutOfRangeException_ShouldBeThrown_WithInvalidPortValue(int port)
        => Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Client(HostName, port));

    private static bool CheckEntries(string path, List<(string Name, bool IsDirectory)> entries)
    {
        var realEntries = Directory.GetFileSystemEntries(path);

        if (realEntries.Length != entries.Count)
        {
            return false;
        }

        Array.Sort(realEntries);

        for (var i = 0; i < realEntries.Length; ++i)
        {
            if (realEntries[i] != entries[i].Name || Directory.Exists(realEntries[i]) != entries[i].IsDirectory)
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
            "../../../TestDirectory/RickVisitsPissMaster.png"
        };
    }
}