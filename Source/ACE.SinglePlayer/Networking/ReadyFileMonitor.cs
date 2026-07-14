using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ACE.SinglePlayer.Networking;

public sealed class ReadyFilePayload
{
    public int ProcessId { get; set; }
    public string WorldName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public ushort Port { get; set; }
    public DateTime ReadyAtUtc { get; set; }
}

public sealed class ReadyFileMonitor
{
    private readonly IPortProbe portProbe;

    public ReadyFileMonitor(IPortProbe portProbe)
    {
        this.portProbe = portProbe;
    }

    public static ReadyFilePayload Validate(string json, int expectedProcessId, IPAddress expectedHost, int expectedPort)
    {
        var payload = JsonSerializer.Deserialize<ReadyFilePayload>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidDataException("The ACE.Server ready file is empty.");

        if (payload.ProcessId != expectedProcessId)
            throw new InvalidDataException("The ready file belongs to a different ACE.Server process.");
        if (!IPAddress.TryParse(payload.Host, out var actualHost) || !actualHost.Equals(expectedHost))
            throw new InvalidDataException("The ready file reports an unexpected server address.");
        if (payload.Port != expectedPort)
            throw new InvalidDataException("The ready file reports an unexpected server port.");
        if (payload.ReadyAtUtc == default)
            throw new InvalidDataException("The ready file has no valid readiness timestamp.");

        return payload;
    }

    public async Task<ReadyFilePayload> WaitAsync(string path, Process serverProcess, IPAddress host, int port,
        TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutSource.CancelAfter(timeout);

        try
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (serverProcess.HasExited)
                    throw new InvalidOperationException($"ACE.Server exited before the world became ready (exit code {serverProcess.ExitCode}).");

                if (File.Exists(path))
                {
                    try
                    {
                        var payload = Validate(await File.ReadAllTextAsync(path, timeoutSource.Token), serverProcess.Id, host, port);
                        if (portProbe.IsListening(host, port))
                            return payload;
                    }
                    catch (IOException)
                    {
                        // The server may still be completing the atomic replacement.
                    }
                    catch (JsonException)
                    {
                        // A partially observed file is retried until timeout.
                    }
                }

                await Task.Delay(100, timeoutSource.Token);
            }
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException($"ACE.Server did not open the world within {timeout.TotalSeconds:N0} seconds.");
        }
    }
}
