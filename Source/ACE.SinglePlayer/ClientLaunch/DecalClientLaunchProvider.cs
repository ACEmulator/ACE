using System.Diagnostics;
using System.Text.Json;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.ClientLaunch;

public sealed class DecalClientLaunchProvider : IClientLaunchProvider
{
    private readonly LauncherLog log;

    public DecalClientLaunchProvider(LauncherLog log)
    {
        this.log = log;
    }

    public ClientLaunchMode Mode => ClientLaunchMode.Decal;
    public string DisplayName => "Decal through Thwarg";

    public bool IsAvailable(out string reason)
    {
        if (DecalDetector.Detect() is null)
        {
            reason = "Decal was not detected in the Windows registry, or its Inject.dll is missing.";
            return false;
        }

        if (ThwargDetector.Detect() is null)
        {
            reason = "ThwargLauncher was not detected, or its injector.dll is missing. Install ThwargLauncher to use Decal mode.";
            return false;
        }

        if (!File.Exists(FindHostPath()))
        {
            reason = "The OpenDereth Decal launch helper is not installed. Vanilla mode remains available.";
            return false;
        }

        reason = string.Empty;
        return true;
    }

    public async Task<Process> LaunchAsync(ClientLaunchRequest request, CancellationToken cancellationToken)
    {
        var installation = DecalDetector.Detect()
            ?? throw new InvalidOperationException("Decal is not installed or its Inject.dll is missing. Select Vanilla mode and retry.");
        var thwarg = ThwargDetector.Detect()
            ?? throw new InvalidOperationException("ThwargLauncher is not installed or its injector.dll is missing. Install ThwargLauncher or select Vanilla mode.");
        var hostPath = FindHostPath();
        if (!File.Exists(hostPath))
            throw new FileNotFoundException("The OpenDereth Decal launch helper is missing. Select Vanilla mode and retry.", hostPath);

        var startInfo = new ProcessStartInfo
        {
            FileName = hostPath,
            WorkingDirectory = Path.GetDirectoryName(hostPath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        Add(startInfo, "--client", request.Settings.ClientExePath);
        Add(startInfo, "--decal", installation.InjectDllPath);
        Add(startInfo, "--injector", thwarg.InjectorDllPath);
        Add(startInfo, "--account", request.Settings.AccountName);
        Add(startInfo, "--password", request.AccountPassword);
        Add(startInfo, "--host", request.Settings.Host);
        Add(startInfo, "--port", request.Settings.Port.ToString());

        using var host = Process.Start(startInfo) ?? throw new InvalidOperationException("The OpenDereth Decal launch helper did not start.");
        var outputTask = host.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = host.StandardError.ReadToEndAsync(cancellationToken);
        await host.WaitForExitAsync(cancellationToken);
        var output = await outputTask;
        var error = await errorTask;
        if (host.ExitCode != 0)
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(error)
                ? "Decal injection failed. Select Vanilla mode and retry."
                : "Decal injection failed: " + error.Trim());

        var result = JsonSerializer.Deserialize<DecalHostResult>(output.Trim(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidDataException("The Decal launch helper returned no diagnostic result.");
        if (!result.DecalStartupInvoked || result.ProcessId <= 0)
            throw new InvalidOperationException("Decal's startup entry point was not invoked. Select Vanilla mode and retry.");

        log.Write($"Started client process {result.ProcessId} with Decal through the installed Thwarg injector.");
        return Process.GetProcessById(result.ProcessId);
    }

    private static void Add(ProcessStartInfo info, string name, string value)
    {
        info.ArgumentList.Add(name);
        info.ArgumentList.Add(value);
    }

    private static string FindHostPath()
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Tools", "OpenDereth.DecalHost.exe"),
            Path.Combine(AppContext.BaseDirectory, "OpenDereth.DecalHost.exe"),
            Path.Combine(AppContext.BaseDirectory, "Tools", "ACE.SinglePlayer.DecalHost.exe"),
            Path.Combine(AppContext.BaseDirectory, "ACE.SinglePlayer.DecalHost.exe")
        };
        return candidates.FirstOrDefault(File.Exists) ?? candidates[0];
    }

    private sealed class DecalHostResult
    {
        public int ProcessId { get; set; }
        public bool DecalStartupInvoked { get; set; }
    }
}
