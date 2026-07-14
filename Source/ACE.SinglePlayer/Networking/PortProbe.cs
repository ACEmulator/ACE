using System.Net;
using System.Net.NetworkInformation;

namespace ACE.SinglePlayer.Networking;

public interface IPortProbe
{
    bool IsListening(IPAddress address, int port);
    Task WaitUntilListeningAsync(IPAddress address, int port, TimeSpan timeout, CancellationToken cancellationToken);
}

public sealed class UdpPortProbe : IPortProbe
{
    public bool IsListening(IPAddress address, int port)
    {
        return IPGlobalProperties.GetIPGlobalProperties()
            .GetActiveUdpListeners()
            .Any(endpoint => endpoint.Port == port &&
                (endpoint.Address.Equals(address) ||
                 (IPAddress.IsLoopback(address) && IPAddress.IsLoopback(endpoint.Address))));
    }

    public async Task WaitUntilListeningAsync(IPAddress address, int port, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutSource.CancelAfter(timeout);

        try
        {
            while (!IsListening(address, port))
                await Task.Delay(100, timeoutSource.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException($"ACE.Server did not bind UDP {address}:{port} within {timeout.TotalSeconds:N0} seconds.");
        }
    }
}
