namespace ACE.SinglePlayer.Mods;

public sealed class ChorizitePluginProvider : IModProvider
{
    public ModType Type => ModType.ChorizitePlugin;
    public IReadOnlyList<ModRecord> Scan() => Array.Empty<ModRecord>();
}

public sealed class AceContentPackProvider : IModProvider
{
    public ModType Type => ModType.WorldContent;
    public IReadOnlyList<ModRecord> Scan() => Array.Empty<ModRecord>();
}
