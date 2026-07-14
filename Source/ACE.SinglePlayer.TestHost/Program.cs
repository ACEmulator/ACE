using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var argumentRecord = Environment.GetEnvironmentVariable("ACE_SINGLEPLAYER_TEST_RECORD_ARGUMENTS");
if (!string.IsNullOrWhiteSpace(argumentRecord))
{
    await File.WriteAllTextAsync(argumentRecord, JsonSerializer.Serialize(args));
    return 0;
}

if (args.Length == 0)
    return 2;

switch (args[0])
{
    case "--server-ready":
    {
        var readyPath = args[1];
        var port = int.Parse(args[2]);
        using var listener = new UdpClient(new IPEndPoint(IPAddress.Loopback, port));
        var payload = new
        {
            ProcessId = Environment.ProcessId,
            WorldName = "Test World",
            Host = "127.0.0.1",
            Port = port,
            ReadyAtUtc = DateTime.UtcNow
        };
        var temporaryPath = readyPath + ".tmp";
        await File.WriteAllTextAsync(temporaryPath, JsonSerializer.Serialize(payload));
        File.Move(temporaryPath, readyPath, true);
        await Console.In.ReadLineAsync();
        return 0;
    }
    case "--server-exit-before-ready":
        return 23;
    case "--server-timeout":
        await Task.Delay(TimeSpan.FromSeconds(30));
        return 0;
    default:
        return 3;
}
