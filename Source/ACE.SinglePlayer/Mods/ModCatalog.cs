namespace ACE.SinglePlayer.Mods;

public sealed class ModCatalog
{
    private readonly IReadOnlyList<IModProvider> providers;

    public ModCatalog(IEnumerable<IModProvider> providers)
    {
        this.providers = providers.ToArray();
    }

    public IReadOnlyList<ModRecord> Scan() => providers.SelectMany(provider => provider.Scan()).ToArray();
}
