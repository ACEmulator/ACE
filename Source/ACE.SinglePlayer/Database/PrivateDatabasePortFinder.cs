using System.Net;
using System.Net.Sockets;

namespace ACE.SinglePlayer.Database;

public static class PrivateDatabasePortFinder
{
    public static ushort FindAvailablePort(ushort preferredPort = 3307, ushort lastPort = 3399)
    {
        if (preferredPort == 0 || lastPort < preferredPort)
            throw new ArgumentOutOfRangeException(nameof(preferredPort));

        for (var port = (int)preferredPort; port <= lastPort; port++)
            if (IsAvailable((ushort)port))
                return (ushort)port;

        throw new InvalidOperationException($"No free private-database port was found between {preferredPort} and {lastPort}.");
    }

    public static bool IsAvailable(ushort port)
    {
        TcpListener? listener = null;
        try
        {
            listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
        finally
        {
            listener?.Stop();
        }
    }
}
