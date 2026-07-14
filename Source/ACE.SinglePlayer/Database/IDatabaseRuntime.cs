using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed record DatabaseValidationResult(bool IsValid, string Message);

public interface IDatabaseRuntime : IAsyncDisposable
{
    bool IsManaged { get; }
    Task StartAsync(LauncherSettings settings, CancellationToken cancellationToken);
    Task<DatabaseValidationResult> ValidateAsync(LauncherSettings settings, CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}
