using System.Diagnostics;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.ClientLaunch;

public sealed record ClientLaunchRequest(LauncherSettings Settings, string AccountPassword);

public interface IClientLaunchProvider
{
    ClientLaunchMode Mode { get; }
    string DisplayName { get; }
    bool IsAvailable(out string reason);
    Task<Process> LaunchAsync(ClientLaunchRequest request, CancellationToken cancellationToken);
}
