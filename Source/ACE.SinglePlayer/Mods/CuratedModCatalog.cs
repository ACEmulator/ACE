namespace ACE.SinglePlayer.Mods;

public static class CuratedModCatalog
{
    public static IReadOnlyList<ModCatalogEntry> Entries { get; } =
        AquafirSampleCatalog.Entries.Concat(new[]
        {
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
