using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Networking;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class ReadyAndProcessSimulationTests
{
    [TestMethod]
    public void ReadyFileValidationRejectsAnotherProcess()
    {
        var json = $$"""{"ProcessId":42,"WorldName":"Test","Host":"127.0.0.1","Port":9000,"ReadyAtUtc":"{{DateTime.UtcNow:O}}"}""";
        Assert.ThrowsExactly<InvalidDataException>(() => ReadyFileMonitor.Validate(json, 43, IPAddress.Loopback, 9000));
    }

    [TestMethod]
    public async Task SimulatorWritesReadyFileAndBindsExpectedUdpPort()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        using var process = StartHost("--server-ready", Path.Combine(directory, "ready.json"), GetFreeUdpPort().ToString());
        try
        {
            var port = int.Parse(process.StartInfo.ArgumentList[2]);
            var result = await new ReadyFileMonitor(new UdpPortProbe()).WaitAsync(
                process.StartInfo.ArgumentList[1], process, IPAddress.Loopback, port, TimeSpan.FromSeconds(5), CancellationToken.None);
            Assert.AreEqual(process.Id, result.ProcessId);
            await process.StandardInput.WriteLineAsync(string.Empty);
            await process.WaitForExitAsync();
        }
        finally
        {
            if (!process.HasExited) process.Kill(true);
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task SimulatorExitBeforeReadyIsReported()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        using var process = StartHost("--server-exit-before-ready");
        try
        {
            await AssertThrowsAsync<InvalidOperationException>(() => new ReadyFileMonitor(new UdpPortProbe()).WaitAsync(
                Path.Combine(directory, "ready.json"), process, IPAddress.Loopback, GetFreeUdpPort(), TimeSpan.FromSeconds(5), CancellationToken.None));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task SimulatorTimeoutAndPortProbeTimeoutAreDeterministic()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        using var process = StartHost("--server-timeout");
        var port = GetFreeUdpPort();
        try
        {
            await AssertThrowsAsync<TimeoutException>(() => new ReadyFileMonitor(new UdpPortProbe()).WaitAsync(
                Path.Combine(directory, "ready.json"), process, IPAddress.Loopback, port, TimeSpan.FromMilliseconds(250), CancellationToken.None));
            await AssertThrowsAsync<TimeoutException>(() => new UdpPortProbe().WaitUntilListeningAsync(
                IPAddress.Loopback, port, TimeSpan.FromMilliseconds(150), CancellationToken.None));
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(true);
                await process.WaitForExitAsync();
            }
            Directory.Delete(directory, true);
        }
    }

    private static Process StartHost(params string[] arguments)
    {
        var info = new ProcessStartInfo
        {
            FileName = TestPaths.TestHost,
            WorkingDirectory = Path.GetDirectoryName(TestPaths.TestHost)!,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
            info.ArgumentList.Add(argument);
        return Process.Start(info) ?? throw new InvalidOperationException("Test host did not start.");
    }

    private static int GetFreeUdpPort()
    {
        using var socket = new UdpClient(new IPEndPoint(IPAddress.Loopback, 0));
        return ((IPEndPoint)socket.Client.LocalEndPoint!).Port;
    }

    private static async Task AssertThrowsAsync<T>(Func<Task> action) where T : Exception
    {
        try
        {
            await action();
            Assert.Fail($"Expected {typeof(T).Name}.");
        }
        catch (T)
        {
        }
    }
}
