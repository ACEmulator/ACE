using System.Diagnostics;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.ClientLaunch;

public sealed class DirectClientLaunchProvider : IClientLaunchProvider
{
    public ClientLaunchMode Mode => ClientLaunchMode.Vanilla;
    public string DisplayName => "Vanilla";

    public bool IsAvailable(out string reason)
    {
        reason = string.Empty;
        return true;
    }

    public Task<Process> LaunchAsync(ClientLaunchRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var process = Process.Start(CreateStartInfo(request))
            ?? throw new InvalidOperationException("Windows did not start acclient.exe.");
        return Task.FromResult(process);
    }

    public static ProcessStartInfo CreateStartInfo(ClientLaunchRequest request)
    {
        var settings = request.Settings;
        var startInfo = new ProcessStartInfo
        {
            FileName = settings.ClientExePath,
            WorkingDirectory = Path.GetDirectoryName(settings.ClientExePath)!,
            UseShellExecute = false
        };
        AddAceArguments(startInfo.ArgumentList, settings.AccountName, request.AccountPassword, settings.Host, settings.Port);
        return startInfo;
    }

    public static void AddAceArguments(IList<string> arguments, string account, string password, string host, ushort port)
    {
        arguments.Add("-a");
        arguments.Add(account);
        arguments.Add("-v");
        arguments.Add(password);
        arguments.Add("-h");
        arguments.Add($"{host}:{port}");
        arguments.Add("-rodat");
        // ACE.Server keeps the client DATs open with shared-read access. The client must
        // also open them read-only or Windows rejects its write request while the server runs.
        arguments.Add("on");
    }
}
