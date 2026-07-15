using ACE.Server.Mods;

using HarmonyLib;

namespace SocietyTailoring;

public sealed class Mod : IHarmonyMod
{
    private const string HarmonyId = "aquafir.SocietyTailoring.ace-single-player";
    private readonly Harmony harmony = new(HarmonyId);

    public void Initialize()
    {
        harmony.PatchAll(typeof(Mod).Assembly);
        Console.WriteLine("[SocietyTailoring] Enabled Society armor tailoring while preserving ACE inventory and retained-item checks.");
    }

    public void Dispose() => harmony.UnpatchAll(HarmonyId);
}
