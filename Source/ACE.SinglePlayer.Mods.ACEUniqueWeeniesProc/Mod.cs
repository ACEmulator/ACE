using ACE.Server.Mods;

using HarmonyLib;

namespace ACEUniqueWeeniesProc;

public sealed class Mod : IHarmonyMod
{
    internal const string HarmonyId = "titaniumweiner.ACEUniqueWeeniesProc.ace-single-player";
    private readonly Harmony harmony = new(HarmonyId);

    public void Initialize()
    {
        harmony.PatchAll(typeof(Mod).Assembly);
        Console.WriteLine("[Expanded Cast on Strike] Enabled cast-on-strike procs for equipped items other than cloak weave proc type 1.");
    }

    public void Dispose() => harmony.UnpatchAll(HarmonyId);
}
