using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Infrastructure;

public sealed class ServerDatProvisioner
{
    private const long FreeSpaceSafetyMargin = 64L * 1024 * 1024;
    private readonly LauncherLog log;

    public ServerDatProvisioner(LauncherLog log, string? targetDirectory = null)
    {
        this.log = log;
        TargetDirectory = targetDirectory ?? GetDefaultDirectory();
    }

    public string TargetDirectory { get; }

    public static string GetDefaultDirectory() => Path.Combine(ApplicationPaths.LocalRoot, "ServerData");

    public bool RequiresRefresh(LauncherSettings settings)
    {
        var sourceDirectory = SetupValidator.DetectDatDirectory(settings.ClientExePath);
        if (sourceDirectory is null)
            return true;

        return SetupValidator.RequiredClientDatFiles.Any(file =>
            !FilesMatch(Path.Combine(sourceDirectory, file), Path.Combine(TargetDirectory, file)));
    }

    public async Task<string> EnsureAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var sourceDirectory = SetupValidator.DetectDatDirectory(settings.ClientExePath)
            ?? throw new InvalidOperationException(
                "The selected Asheron's Call client is missing one or more required DAT files.");
        var sourceFullPath = Path.GetFullPath(sourceDirectory);
        var targetFullPath = Path.GetFullPath(TargetDirectory);
        if (string.Equals(sourceFullPath.TrimEnd(Path.DirectorySeparatorChar),
                targetFullPath.TrimEnd(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The private server DAT directory must be separate from the game client directory.");

        Directory.CreateDirectory(targetFullPath);
        FilePermissionHardener.RestrictDirectoryToCurrentUser(targetFullPath);

        var pending = SetupValidator.RequiredClientDatFiles
            .Select(file => new DatCopy(
                file,
                Path.Combine(sourceFullPath, file),
                Path.Combine(targetFullPath, file)))
            .Where(copy => !FilesMatch(copy.SourcePath, copy.TargetPath))
            .ToArray();
        EnsureFreeSpace(targetFullPath, pending);

        try
        {
            foreach (var copy in pending)
            {
                cancellationToken.ThrowIfCancellationRequested();
                log.Write($"Copying {copy.FileName} into the private ACE.Server data directory. The original client file is unchanged.");
                await CopyAtomicallyAsync(copy.SourcePath, copy.TargetPath, cancellationToken);
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new IOException(
                $"OpenDereth could not prepare its private server data files in '{targetFullPath}'. " +
                "Close any running ACE.Server process and make sure the drive has enough free space. " + ex.Message,
                ex);
        }

        log.Write($"ACE.Server will use the private DAT copy at '{targetFullPath}'. The game client will use its original files at '{sourceFullPath}'.");
        return targetFullPath;
    }

    private static async Task CopyAtomicallyAsync(string sourcePath, string targetPath, CancellationToken cancellationToken)
    {
        var temporaryPath = targetPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            var sourceInfo = new FileInfo(sourcePath);
            await using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read,
                             1024 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan))
            await using (var target = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                             1024 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await source.CopyToAsync(target, 1024 * 1024, cancellationToken);
                await target.FlushAsync(cancellationToken);
            }

            if (new FileInfo(temporaryPath).Length != sourceInfo.Length)
                throw new IOException($"The private copy of {Path.GetFileName(sourcePath)} was incomplete.");

            File.SetLastWriteTimeUtc(temporaryPath, sourceInfo.LastWriteTimeUtc);
            File.Move(temporaryPath, targetPath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static bool FilesMatch(string sourcePath, string targetPath)
    {
        if (!File.Exists(sourcePath) || !File.Exists(targetPath))
            return false;

        var source = new FileInfo(sourcePath);
        var target = new FileInfo(targetPath);
        return source.Length == target.Length && source.LastWriteTimeUtc == target.LastWriteTimeUtc;
    }

    private static void EnsureFreeSpace(string targetDirectory, IReadOnlyCollection<DatCopy> pending)
    {
        if (pending.Count == 0)
            return;

        var required = pending.Sum(copy => new FileInfo(copy.SourcePath).Length) + FreeSpaceSafetyMargin;
        var root = Path.GetPathRoot(targetDirectory);
        if (string.IsNullOrWhiteSpace(root))
            return;

        var drive = new DriveInfo(root);
        if (drive.AvailableFreeSpace >= required)
            return;

        throw new IOException(
            $"The private ACE.Server DAT copy needs about {FormatBytes(required)} free on {drive.Name}, " +
            $"but only {FormatBytes(drive.AvailableFreeSpace)} is available.");
    }

    private static string FormatBytes(long bytes) => $"{bytes / 1024d / 1024d / 1024d:0.00} GB";

    private sealed record DatCopy(string FileName, string SourcePath, string TargetPath);
}
