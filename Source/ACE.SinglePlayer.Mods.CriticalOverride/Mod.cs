using System.Text.Json;

using ACE.Server.Mods;

using HarmonyLib;

namespace CriticalOverride;

public sealed class Mod : IHarmonyMod
{
    private const string HarmonyId = "aquafir.CriticalOverride.ace-single-player";
    private readonly Harmony harmony = new(HarmonyId);

    internal static CriticalOverrideSettings Settings { get; private set; } = new();

    public void Initialize()
    {
        Settings = LoadSettings(Path.Combine(this.GetFolder(), "Settings.json"));
        harmony.PatchAll(typeof(Mod).Assembly);
        ModManager.Log($"CriticalOverride configured physical crit chance {Settings.CritChance:P0} and magic crit chance {Settings.MagicCritChance:P0} against creatures.");
    }

    public void Dispose()
    {
        harmony.UnpatchAll(HarmonyId);
    }

    private static CriticalOverrideSettings LoadSettings(string path)
    {
        if (!File.Exists(path))
            return new CriticalOverrideSettings();

        var settings = JsonSerializer.Deserialize<CriticalOverrideSettings>(File.ReadAllText(path), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        }) ?? new CriticalOverrideSettings();
        if (settings.CritChance is < 0 or > 1 || settings.MagicCritChance is < 0 or > 1)
            throw new InvalidDataException("CriticalOverride chances must be between 0 and 1.");
        return settings;
    }
}

public sealed class CriticalOverrideSettings
{
    public float CritChance { get; set; } = 0.5f;
    public float MagicCritChance { get; set; } = 0.05f;
}
