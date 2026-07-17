namespace ACE.SinglePlayer.Mods;

public static class CuratedModCatalog
{
    public static IReadOnlyList<ModCatalogEntry> Entries { get; } =
        AquafirSampleCatalog.Entries.Concat(new[]
        {
            new ModCatalogEntry(
                "titaniumweiner.ace-unique-weenies-proc",
                "Expanded Cast on Strike",
                "titaniumweiner",
                "Enables cast-on-strike procs on non-Aetheria equipped items such as the jewelry, armor, and weapons used by ACEUniqueWeenies content.",
                "Replaces ACE's Aetheria-only equipped-proc pass with the filter documented by ACEUniqueWeenies: the item must have a proc, must not be cloak weave proc type 1, and must match the current self-target mode. The package changes server combat behavior only; import compatible item SQL separately through Custom Weenies. It intentionally matches the documented filter exactly, including processing an active proc weapon again if that weapon is also present in EquippedObjects.",
                "https://github.com/titaniumweiner/ACEUniqueWeenies",
                ModCatalogAvailability.Preview,
                ModDataImpact.SettingsOnly,
                ModRemovalPolicy.Safe,
                "Safe to turn off after a server restart. Imported items remain in the world, but their non-Aetheria equipped procs stop working. Earlier combat results are not reversed.",
                TargetAceVersion: "ACE.Server 1.1 / ACE Single Player",
                TargetFramework: ".NET 10 preview port",
                PackageRelativePath: @"Packages\titaniumweiner.ace-unique-weenies-proc-1.0.0-sp1.zip",
                PortSourceUrl: "https://github.com/titaniumweiner/ACE-SinglePlayer/tree/main/Source/ACE.SinglePlayer.Mods.ACEUniqueWeeniesProc",
                PreviewNotice: "The package builds, loads, targets the pinned TryProcEquippedItems signature, and passes filter and removal tests. Its proc frequency and interactions with every custom item have not been thoroughly tested in game."),
            new ModCatalogEntry(
                "optimshi.custom-clothing-base",
                "CustomClothingBase",
                "OptimShi",
                "Loads custom server-side ClothingTable entries from JSON files, enabling new clothing colors and appearance combinations without client DAT updates.",
                "The official v1.11 package can merge or replace ClothingTable entries, export entries with @clothingbase-export, and reload its cache with @clear-clothing-cache. Place custom JSON files in the installed mod's json folder. The upstream repository currently has no LICENSE file; redistribution is based on OptimShi's permission reported by the ACE Single Player project maintainer.",
                "https://github.com/OptimShi/CustomClothingBase",
                ModCatalogAvailability.Preview,
                ModDataImpact.WorldData,
                ModRemovalPolicy.DoNotRemove,
                "Back up first. Do not disable or remove this mod while saved items or world content refer to custom ClothingBase IDs.",
                TargetAceVersion: "Official v1.11 binary load-tested with ACE.Server 1.1 / .NET 10",
                TargetFramework: ".NET 8 upstream binary hosted by .NET 10",
                PackageRelativePath: @"Packages\optimshi.custom-clothing-base-1.11-upstream.zip",
                PreviewNotice: "OptimShi's unmodified official v1.11 package passed an isolated load test with the bundled ACE server. Clothing behavior, custom JSON content, cache reloads, and saved-world compatibility have not been thoroughly tested in game.")
        }).ToArray();
}
