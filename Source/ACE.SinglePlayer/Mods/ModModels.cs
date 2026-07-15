namespace ACE.SinglePlayer.Mods;

public enum ModType
{
    AceServer,
    DecalPlugin,
    ChorizitePlugin,
    WorldContent
}

public enum CompatibilityStatus
{
    Compatible,
    CompatibleAfterRebuild,
    NeedsSourcePort,
    MissingDependency,
    Conflict,
    PackageMissing,
    WrongModType,
    Unknown,
    LoadFailed
}

public enum ModDataImpact
{
    None,
    SettingsOnly,
    CharacterData,
    WorldData,
    ClientData
}

public enum ModRemovalPolicy
{
    Safe,
    ChangesRemain,
    BackupRequired,
    DoNotRemove
}

public enum ModCatalogAvailability
{
    Ready,
    Preview,
    NeedsPort,
    Experimental,
    NotRecommended
}

public sealed class ModRecord
{
    public string CatalogId { get; init; } = string.Empty;
    public ModType Type { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string TargetAceVersion { get; init; } = string.Empty;
    public string TargetFramework { get; init; } = string.Empty;
    public string RequiredClientFramework { get; init; } = string.Empty;
    public string RequiredDependencies { get; init; } = string.Empty;
    public uint Priority { get; init; }
    public bool Enabled { get; set; }
    public string InstalledPath { get; init; } = string.Empty;
    public string MetadataPath { get; init; } = string.Empty;
    public string SettingsPath { get; init; } = string.Empty;
    public bool IsMalformed { get; init; }
    public string LastLoadError { get; init; } = string.Empty;
    public CompatibilityStatus CompatibilityStatus { get; init; } = CompatibilityStatus.Unknown;
    public bool CanToggle => Type == ModType.AceServer && !IsMalformed;
}

public sealed record ModCatalogEntry(
    string Id,
    string Name,
    string Author,
    string Description,
    string Details,
    string SourceUrl,
    ModCatalogAvailability Availability,
    ModDataImpact DataImpact,
    ModRemovalPolicy RemovalPolicy,
    string SafetyNotice,
    string TargetAceVersion = "Current ACE port required",
    string TargetFramework = ".NET 8 source sample",
    IReadOnlyList<string>? DependencyIds = null,
    IReadOnlyList<string>? ConflictIds = null,
    string PackageRelativePath = "",
    string PortSourceUrl = "")
{
    public IReadOnlyList<string> Dependencies { get; init; } = DependencyIds ?? Array.Empty<string>();
    public IReadOnlyList<string> Conflicts { get; init; } = ConflictIds ?? Array.Empty<string>();
}

public sealed class ModListItem
{
    public required ModCatalogEntry Catalog { get; init; }
    public ModRecord? Installed { get; init; }
    public required CompatibilityStatus CompatibilityStatus { get; init; }
    public string CompatibilityMessage { get; init; } = string.Empty;
    public string PackagePath { get; init; } = string.Empty;

    public string Name => Catalog.Name;
    public string Author => Catalog.Author;
    public string Description => Catalog.Description;
    public string Safety => Catalog.DataImpact switch
    {
        ModDataImpact.None => "No saved-data changes",
        ModDataImpact.SettingsOnly => "Settings only",
        ModDataImpact.CharacterData => "Changes characters",
        ModDataImpact.WorldData => "Changes the saved world",
        ModDataImpact.ClientData => "Changes client data",
        _ => Catalog.DataImpact.ToString()
    };
    public string Status => Installed?.IsMalformed == true ? "Install error"
        : Installed is not null
        ? Catalog.Availability == ModCatalogAvailability.Preview
            ? Installed.Enabled ? "Installed - on (preview)" : "Installed - off (preview)"
            : Installed.Enabled ? "Installed - on" : "Installed - off"
        : Catalog.Availability == ModCatalogAvailability.Preview ? "Preview - limited testing"
        : Catalog.Availability == ModCatalogAvailability.Experimental ? "Experimental"
        : Catalog.Availability == ModCatalogAvailability.NotRecommended ? "Not recommended"
        : CompatibilityStatus switch
        {
            CompatibilityStatus.Compatible => "Ready to install",
            CompatibilityStatus.NeedsSourcePort => "Needs port",
            CompatibilityStatus.MissingDependency => "Missing requirement",
            CompatibilityStatus.Conflict => "Conflicts",
            CompatibilityStatus.PackageMissing => "Package unavailable",
            _ => CompatibilityStatus.ToString()
        };
}

public interface IModProvider
{
    ModType Type { get; }
    IReadOnlyList<ModRecord> Scan();
}
