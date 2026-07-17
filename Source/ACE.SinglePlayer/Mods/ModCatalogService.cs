namespace ACE.SinglePlayer.Mods;

public sealed class ModCatalogService
{
    private readonly IReadOnlyList<ModCatalogEntry> catalog;
    private readonly string applicationDirectory;

    public ModCatalogService(IEnumerable<ModCatalogEntry> catalog, string applicationDirectory)
    {
        this.catalog = catalog.ToArray();
        this.applicationDirectory = Path.GetFullPath(applicationDirectory);
    }

    public IReadOnlyList<ModListItem> Merge(IEnumerable<ModRecord> installedRecords)
    {
        var installed = installedRecords.ToArray();
        var installedByName = installed
            .GroupBy(record => record.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        var catalogByName = catalog.ToDictionary(entry => entry.Name, StringComparer.OrdinalIgnoreCase);
        var installedIds = catalog
            .Where(entry => installedByName.ContainsKey(entry.Name))
            .Select(entry => entry.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var result = catalog.Select(entry =>
        {
            installedByName.TryGetValue(entry.Name, out var installedRecord);
            var packagePath = ResolvePackagePath(entry.PackageRelativePath);
            var (status, message) = Evaluate(entry, installedRecord, installedIds, packagePath);
            return new ModListItem
            {
                Catalog = entry,
                Installed = installedRecord,
                CompatibilityStatus = status,
                CompatibilityMessage = message,
                PackagePath = packagePath
            };
        }).ToList();

        foreach (var record in installed.Where(record => !catalogByName.ContainsKey(record.Name)))
        {
            var description = string.IsNullOrWhiteSpace(record.Description)
                ? "Installed manually. No curated description is available."
                : record.Description;
            var entry = new ModCatalogEntry(
                "installed." + NormalizeId(record.Name),
                record.Name,
                string.IsNullOrWhiteSpace(record.Author) ? "Unknown" : record.Author,
                description,
                "This mod was found in the local Mods folder but is not part of the curated catalog. The launcher cannot verify its dependencies, conflicts, or saved-data behavior.",
                string.Empty,
                ModCatalogAvailability.Experimental,
                ModDataImpact.WorldData,
                ModRemovalPolicy.BackupRequired,
                "Unknown mod: make a backup before changing or removing it.",
                record.TargetAceVersion,
                record.TargetFramework);
            result.Add(new ModListItem
            {
                Catalog = entry,
                Installed = record,
                CompatibilityStatus = record.IsMalformed ? CompatibilityStatus.LoadFailed : CompatibilityStatus.Unknown,
                CompatibilityMessage = record.IsMalformed ? record.LastLoadError : "Installed manually; compatibility has not been verified."
            });
        }

        return result
            .OrderBy(GetDisplayRank)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static int GetDisplayRank(ModListItem item) => item switch
    {
        { Installed: not null } => 0,
        { CompatibilityStatus: CompatibilityStatus.Compatible } => 1,
        _ => 2
    };

    private (CompatibilityStatus Status, string Message) Evaluate(
        ModCatalogEntry entry,
        ModRecord? installed,
        IReadOnlySet<string> installedIds,
        string packagePath)
    {
        if (installed is not null)
        {
            if (installed.IsMalformed)
                return (CompatibilityStatus.LoadFailed, installed.LastLoadError);
            var installedMessage = installed.Enabled
                ? "Installed and enabled. A server restart is required after changes."
                : "Installed but disabled. A server restart is required after changes.";
            if (entry.Availability == ModCatalogAvailability.Preview)
                installedMessage += " " + GetPreviewNotice(entry);
            return (CompatibilityStatus.Compatible, installedMessage);
        }

        if (entry.Availability is not (ModCatalogAvailability.Ready or ModCatalogAvailability.Preview))
        {
            var message = entry.Availability switch
            {
                ModCatalogAvailability.Experimental => "The upstream sample is unfinished or high-risk and has not been ported to this ACE build.",
                ModCatalogAvailability.NotRecommended => "This sample is not intended for ordinary single-player use and has not been ported.",
                _ => "The upstream .NET 8 sample must be rebuilt and tested against this exact ACE version before installation."
            };
            return (CompatibilityStatus.NeedsSourcePort, message);
        }

        var missing = entry.Dependencies.Where(id => !installedIds.Contains(id)).ToArray();
        if (missing.Length > 0)
            return (CompatibilityStatus.MissingDependency, "Install first: " + FriendlyNames(missing));

        var conflicts = entry.Conflicts.Where(installedIds.Contains).ToArray();
        if (conflicts.Length > 0)
            return (CompatibilityStatus.Conflict, "Conflicts with: " + FriendlyNames(conflicts));

        if (string.IsNullOrWhiteSpace(packagePath) || !File.Exists(packagePath))
            return (CompatibilityStatus.PackageMissing, "This build does not contain the validated mod package.");

        return entry.Availability == ModCatalogAvailability.Preview
            ? (CompatibilityStatus.Compatible, GetPreviewNotice(entry))
            : (CompatibilityStatus.Compatible, "Validated package is included and ready to install.");
    }

    private static string GetPreviewNotice(ModCatalogEntry entry) =>
        string.IsNullOrWhiteSpace(entry.PreviewNotice)
            ? "Preview package is included. It builds and passes automated load, patch/registration, checksum, and package checks, but it has not received thorough in-game testing. Back up before use."
            : entry.PreviewNotice;

    private string FriendlyNames(IEnumerable<string> ids) => string.Join(", ", ids.Select(id =>
        catalog.FirstOrDefault(entry => string.Equals(entry.Id, id, StringComparison.OrdinalIgnoreCase))?.Name ?? id));

    private string ResolvePackagePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return string.Empty;
        var path = Path.GetFullPath(Path.Combine(applicationDirectory, relativePath));
        var prefix = applicationDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        return path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? path : string.Empty;
    }

    private static string NormalizeId(string value) => string.Concat(value
        .ToLowerInvariant()
        .Select(character => char.IsLetterOrDigit(character) ? character : '-'));
}
