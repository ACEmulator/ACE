using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace ACE.SinglePlayer.Mods;

public sealed class ModPackageManifest
{
    public int FormatVersion { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string FolderName { get; set; } = string.Empty;
    public string EntryAssembly { get; set; } = string.Empty;
}

public sealed class ModPackageInstaller
{
    private const long MaximumExpandedSize = 100L * 1024 * 1024;
    private const int MaximumEntries = 500;

    public async Task<ModPackageManifest> InspectAsync(
        string packagePath,
        CancellationToken cancellationToken = default)
    {
        packagePath = Path.GetFullPath(packagePath);
        if (!File.Exists(packagePath))
            throw new FileNotFoundException("The mod package is missing.", packagePath);

        await VerifyHashSidecarAsync(packagePath, cancellationToken);
        using var archive = ZipFile.OpenRead(packagePath);
        ValidateArchiveSize(archive);
        var manifestEntry = FindManifest(archive);
        var manifest = await ReadManifestAsync(manifestEntry, cancellationToken);
        ValidateManifest(manifest, expectedCatalogId: null);
        return manifest;
    }

    public async Task<string> InstallAsync(
        string packagePath,
        string expectedCatalogId,
        string modsDirectory,
        string stagingRoot,
        CancellationToken cancellationToken = default)
    {
        packagePath = Path.GetFullPath(packagePath);
        if (!File.Exists(packagePath))
            throw new FileNotFoundException("The mod package is missing.", packagePath);

        await VerifyHashSidecarAsync(packagePath, cancellationToken);
        Directory.CreateDirectory(modsDirectory);
        Directory.CreateDirectory(stagingRoot);

        var operationDirectory = Path.Combine(stagingRoot, "mod-install-" + Guid.NewGuid().ToString("N"));
        var extractedModDirectory = Path.Combine(operationDirectory, "mod");
        Directory.CreateDirectory(extractedModDirectory);

        try
        {
            using var archive = ZipFile.OpenRead(packagePath);
            ValidateArchiveSize(archive);
            var manifestEntry = FindManifest(archive);
            var manifest = await ReadManifestAsync(manifestEntry, cancellationToken);
            ValidateManifest(manifest, expectedCatalogId);

            var seenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in archive.Entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var normalized = NormalizeEntry(entry.FullName);
                if (string.Equals(normalized, "ace-mod.json", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!normalized.StartsWith("mod/", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException($"Unexpected package entry: {entry.FullName}");

                var relative = normalized[4..];
                if (string.IsNullOrWhiteSpace(relative))
                    continue;
                ValidateRelativePath(relative);
                if (!seenPaths.Add(relative))
                    throw new InvalidDataException($"Duplicate package path: {relative}");

                var destination = Path.GetFullPath(Path.Combine(extractedModDirectory, relative.Replace('/', Path.DirectorySeparatorChar)));
                EnsureInside(extractedModDirectory, destination);
                if (normalized.EndsWith('/'))
                {
                    Directory.CreateDirectory(destination);
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                await using var source = entry.Open();
                await using var target = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous);
                await source.CopyToAsync(target, cancellationToken);
            }

            ValidateExtractedMod(extractedModDirectory, manifest);
            var destinationDirectory = Path.GetFullPath(Path.Combine(modsDirectory, manifest.FolderName));
            EnsureImmediateChild(modsDirectory, destinationDirectory);
            if (Directory.Exists(destinationDirectory) || File.Exists(destinationDirectory))
                throw new IOException($"{manifest.Name} is already installed.");

            Directory.Move(extractedModDirectory, destinationDirectory);
            return destinationDirectory;
        }
        finally
        {
            if (Directory.Exists(operationDirectory))
                Directory.Delete(operationDirectory, recursive: true);
        }
    }

    public string MoveToQuarantine(string installedDirectory, string modsDirectory, string quarantineRoot)
    {
        installedDirectory = Path.GetFullPath(installedDirectory);
        EnsureImmediateChild(modsDirectory, installedDirectory);
        if (!Directory.Exists(installedDirectory))
            throw new DirectoryNotFoundException("The installed mod folder no longer exists.");

        Directory.CreateDirectory(quarantineRoot);
        var baseName = Path.GetFileName(installedDirectory) + "-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        var destination = Path.Combine(quarantineRoot, baseName);
        for (var suffix = 2; Directory.Exists(destination); suffix++)
            destination = Path.Combine(quarantineRoot, baseName + "-" + suffix);

        Directory.Move(installedDirectory, destination);
        return destination;
    }

    private static async Task VerifyHashSidecarAsync(string packagePath, CancellationToken cancellationToken)
    {
        var sidecarPath = packagePath + ".sha256";
        if (!File.Exists(sidecarPath))
            throw new InvalidDataException("The package checksum is missing.");

        var expected = (await File.ReadAllTextAsync(sidecarPath, cancellationToken))
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? string.Empty;
        if (expected.Length != 64 || expected.Any(character => !Uri.IsHexDigit(character)))
            throw new InvalidDataException("The package checksum is malformed.");

        await using var stream = new FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, FileOptions.Asynchronous | FileOptions.SequentialScan);
        var actual = Convert.ToHexString(await SHA256.HashDataAsync(stream, cancellationToken));
        if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("The package checksum does not match. The mod was not installed.");
    }

    private static async Task<ModPackageManifest> ReadManifestAsync(ZipArchiveEntry entry, CancellationToken cancellationToken)
    {
        await using var stream = entry.Open();
        return await JsonSerializer.DeserializeAsync<ModPackageManifest>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }, cancellationToken) ?? throw new InvalidDataException("The package manifest is empty.");
    }

    private static ZipArchiveEntry FindManifest(ZipArchive archive) =>
        archive.Entries.SingleOrDefault(entry =>
            string.Equals(NormalizeEntry(entry.FullName), "ace-mod.json", StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidDataException("The package has no ace-mod.json manifest.");

    private static void ValidateArchiveSize(ZipArchive archive)
    {
        if (archive.Entries.Count > MaximumEntries)
            throw new InvalidDataException("The mod package contains too many files.");
        if (archive.Entries.Sum(entry => entry.Length) > MaximumExpandedSize)
            throw new InvalidDataException("The expanded mod package is larger than 100 MB.");
    }

    private static void ValidateManifest(ModPackageManifest manifest, string? expectedCatalogId)
    {
        if (manifest.FormatVersion != 1)
            throw new InvalidDataException("The package manifest version is not supported.");
        if (string.IsNullOrWhiteSpace(manifest.Id))
            throw new InvalidDataException("The package identity is missing.");
        if (!string.IsNullOrWhiteSpace(expectedCatalogId) &&
            !string.Equals(manifest.Id, expectedCatalogId, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("The package identity does not match the selected catalog mod.");
        if (string.IsNullOrWhiteSpace(manifest.Name) || string.IsNullOrWhiteSpace(manifest.Version))
            throw new InvalidDataException("The package name or version is missing.");
        ValidateSimpleName(manifest.FolderName, "folder");
        ValidateSimpleName(manifest.EntryAssembly, "entry assembly");
        if (!manifest.EntryAssembly.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("The package entry assembly must be a DLL.");
        if (!string.Equals(Path.GetFileNameWithoutExtension(manifest.EntryAssembly), manifest.FolderName, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("ACE requires the mod folder and entry assembly to have the same name.");
    }

    private static void ValidateExtractedMod(string directory, ModPackageManifest manifest)
    {
        var metadataPath = Path.Combine(directory, "Meta.json");
        var assemblyPath = Path.Combine(directory, manifest.EntryAssembly);
        if (!File.Exists(metadataPath))
            throw new InvalidDataException("The package does not contain mod/Meta.json.");
        if (!File.Exists(assemblyPath))
            throw new InvalidDataException($"The package does not contain mod/{manifest.EntryAssembly}.");

        var metadata = ModMetadataEditor.Parse(File.ReadAllText(metadataPath));
        var name = metadata.FirstOrDefault(property => string.Equals(property.Key, "Name", StringComparison.OrdinalIgnoreCase)).Value?.ToString();
        if (!string.Equals(name, manifest.Name, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Meta.json does not match the package manifest.");
    }

    private static string NormalizeEntry(string value)
    {
        var normalized = value.Replace('\\', '/');
        if (normalized.StartsWith('/') || Path.IsPathRooted(normalized))
            throw new InvalidDataException($"Absolute package path is not allowed: {value}");
        return normalized;
    }

    private static void ValidateRelativePath(string value)
    {
        var parts = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0 || parts.Any(part => part is "." or ".." || part.Contains(':')))
            throw new InvalidDataException($"Unsafe package path: {value}");
    }

    private static void ValidateSimpleName(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value) || value is "." or ".." ||
            value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || value.Contains('/') || value.Contains('\\'))
            throw new InvalidDataException($"The package {field} is invalid.");
    }

    private static void EnsureInside(string parent, string child)
    {
        var prefix = Path.GetFullPath(parent).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!child.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("A package entry attempted to leave its install directory.");
    }

    private static void EnsureImmediateChild(string parent, string child)
    {
        var expectedParent = Path.GetFullPath(parent).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var actualParent = Path.GetDirectoryName(Path.GetFullPath(child))?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (!string.Equals(expectedParent, actualParent, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("The mod folder is outside the configured Mods directory.");
    }
}
