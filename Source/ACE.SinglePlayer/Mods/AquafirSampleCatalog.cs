namespace ACE.SinglePlayer.Mods;

public static class AquafirSampleCatalog
{
    private const string BaseUrl = "https://github.com/aquafir/ACE.BaseMod/tree/master/Samples/";
    private const string PortBaseUrl = "https://github.com/titaniumweiner/OpenDereth/tree/main/Source/ACE.SinglePlayer.Mods.";

    public static IReadOnlyList<ModCatalogEntry> Entries { get; } = new ModCatalogEntry[]
    {
        PortRequired(
            "aquafir.access-db", "AccessDb",
            "Adds administrator examples for moving online or offline characters, listing player locations, and querying creature/login counts from the ACE databases.",
            "This is primarily a server-administration and mod-development sample. Its moveall command can permanently change character login locations.",
            ModDataImpact.CharacterData, ModRemovalPolicy.ChangesRemain,
            "Turning it off does not undo character moves already performed."),
        PortRequired(
            "aquafir.auto-loot", "AutoLoot",
            "Automatically loots creature corpses for a character using selectable Virindi Tank-style loot profiles and a /loot command.",
            "Loot profiles choose which generated items a character takes from corpses. It changes server-side looting behavior and does not require Decal.",
            ModDataImpact.SettingsOnly, ModRemovalPolicy.Safe,
            "Safe to turn off; items already looted remain in character inventories."),
        PortRequired(
            "aquafir.balance", "Balance",
            "Lets you replace many combat, healing, experience, level-cost, critical-hit, rending, accuracy, and damage formulas from Settings.json.",
            "Each formula patch can be enabled separately. Because this can change experience awards and progression costs, a character may keep gains made under a previous formula.",
            ModDataImpact.CharacterData, ModRemovalPolicy.BackupRequired,
            "Back up first. Disabling restores ACE formulas but cannot reverse experience, levels, or item outcomes already earned."),
        PortRequired(
            "aquafir.bank", "Bank",
            "Adds a virtual bank for configured items, pyreals, trade notes, luminance, direct vendor deposits, and optional death-drop rules.",
            "Balances are stored on characters. Players can deposit and withdraw configured items or currency with /bank, /cash, and /lum commands.",
            ModDataImpact.CharacterData, ModRemovalPolicy.DoNotRemove,
            "Do not turn this off while anything is deposited; stored balances could become inaccessible."),
        PortRequired(
            "aquafir.chat-filter", "ChatFilter",
            "Filters chat and tells with configurable blacklists/whitelists, censorship, shadow bans, gags, and temporary bans.",
            "This is mainly useful on multiplayer servers. It requires the Profanity.Detector dependency and careful moderation settings.",
            ModDataImpact.CharacterData, ModRemovalPolicy.ChangesRemain,
            "Existing gag or ban state may remain after the mod is disabled."),
        PortRequired(
            "aquafir.connection-limit", "ConnectionLimit",
            "Limits simultaneous connections while allowing configured IP addresses or landblocks to be exempt for bots or mule characters.",
            "This is a multiplayer administration tool and normally has no benefit in a private one-player world.",
            ModDataImpact.SettingsOnly, ModRemovalPolicy.Safe,
            "Safe to remove, but not recommended for ordinary single-player use.",
            availability: ModCatalogAvailability.NotRecommended),
        new ModCatalogEntry(
            "aquafir.critical-override", "CriticalOverride", "aquafir",
            "Overrides physical and magic critical-hit chances against non-player creatures with two simple settings.",
            "The OpenDereth port keeps player-versus-player calculations unchanged. Restart the local server after changing Settings.json.",
            BaseUrl + "CriticalOverride", ModCatalogAvailability.Ready, ModDataImpact.SettingsOnly, ModRemovalPolicy.Safe,
            "Safe to turn off. Existing combat results are not recalculated.",
            TargetAceVersion: "ACE.Server 1.1 / OpenDereth",
            TargetFramework: ".NET 10 port",
            PackageRelativePath: @"Packages\aquafir.critical-override-1.0.0-sp1.zip",
            PortSourceUrl: PortBaseUrl + "CriticalOverride"),
        PortRequired(
            "aquafir.custom-spells", "CustomSpells",
            "Creates or edits spells and equipment-set spell tiers from Settings.json or an Excel spreadsheet.",
            "It can add custom spell IDs, change names and effects, alter stacking categories, and customize set bonuses. Incorrect projectile IDs can crash the client.",
            ModDataImpact.WorldData, ModRemovalPolicy.DoNotRemove,
            "High risk: saved enchantments or items may refer to custom spell IDs. Do not disable after using custom content without a tested migration."),
        PortRequired(
            "aquafir.discord", "Discord",
            "Relays chat and tells between ACE and Discord and lets approved Discord users run configured server commands.",
            "Requires a Discord bot, token, channel IDs, Discord.Net, and a deliberate security setup. It is intended for hosted multiplayer servers.",
            ModDataImpact.SettingsOnly, ModRemovalPolicy.Safe,
            "Never place a Discord bot token in a public mod package or log.",
            availability: ModCatalogAvailability.NotRecommended),
        PortRequired(
            "aquafir.easy-enlightenment", "EasyEnlightenment",
            "Makes enlightenment requirements, resets, bonuses, skill credits, luminance limits, and maximum enlightenment count configurable.",
            "Optional skill, attribute, and vital bonuses rely on Expansion's BonusStats feature. It can convert characters to a new persistent progression system.",
            ModDataImpact.CharacterData, ModRemovalPolicy.DoNotRemove,
            "High risk: converted characters and persistent bonuses may depend on this mod.",
            dependencyIds: new[] { "aquafir.expansion" }),
        PortRequired(
            "aquafir.expansion", "Expansion",
            "A large loot and creature expansion framework that can mutate generated items, add bonus properties, sets, slayers, procs, creature behaviors, and other systems.",
            "This is an experimental foundation for major gameplay overhauls. Some generated items require its runtime features to keep working as designed.",
            ModDataImpact.WorldData, ModRemovalPolicy.DoNotRemove,
            "High risk: once it has generated modified loot, removing it can leave dependent items in the saved world."),
        new ModCatalogEntry(
            "aquafir.hello-command", "HelloCommand", "aquafir",
            "A small developer sample that adds /hello and /bye commands to demonstrate how ACE server-mod commands are registered.",
            "It is useful for mod authors but adds little to normal gameplay.",
            BaseUrl + "HelloCommand", ModCatalogAvailability.Preview, ModDataImpact.None, ModRemovalPolicy.Safe,
            "Safe to turn off or remove.",
            TargetAceVersion: "ACE.Server 1.1 / OpenDereth",
            TargetFramework: ".NET 10 preview port",
            PackageRelativePath: @"Packages\aquafir.hello-command-1.0.0-sp1.zip",
            PortSourceUrl: PortBaseUrl + "HelloCommand"),
        PortRequired(
            "aquafir.imgui-hud", "ImGuiHud",
            "An experimental server-side ImGui overlay sample with pickers, filters, textures, tables, and on-screen tools.",
            "The upstream README is unfinished. It opens native desktop UI from the server process and needs extra graphics dependencies and focused testing.",
            ModDataImpact.SettingsOnly, ModRemovalPolicy.Safe,
            "Experimental desktop integration; it is not a Decal in-game plugin.",
            availability: ModCatalogAvailability.Experimental),
        PortRequired(
            "aquafir.ironman", "Ironman",
            "Adds opt-in ironman/hardcore rule sets, character templates, item restrictions, fellowship/allegiance restrictions, and progression flags.",
            "The upstream documentation is unfinished, but the source enrolls characters and stores custom state used by its restrictions.",
            ModDataImpact.CharacterData, ModRemovalPolicy.DoNotRemove,
            "High risk: enrolled characters and flagged items may rely on the mod. Test recovery before enabling it on a real save.",
            availability: ModCatalogAvailability.Experimental),
        PortRequired(
            "aquafir.player-save", "PlayerSave",
            "Exports character snapshots and can import them as new characters or into another account.",
            "The author labels this work in progress. It rewrites extensive character, inventory, and relationship data and does not yet fully handle houses, allegiances, or cross-server IDs.",
            ModDataImpact.CharacterData, ModRemovalPolicy.BackupRequired,
            "Experimental database operation: always make a full private-database backup before loading a snapshot.",
            availability: ModCatalogAvailability.Experimental),
        PortRequired(
            "aquafir.quality-of-life", "QualityOfLife",
            "Bundles configurable convenience changes for fellowships, animations, augment limits, property defaults, tailoring, recklessness, and admin commands.",
            "Individual feature groups are selected in Settings.json. Some options can permanently affect augmentations or tailored items.",
            ModDataImpact.CharacterData, ModRemovalPolicy.BackupRequired,
            "Back up before enabling progression or tailoring changes; turning them off does not undo earlier character or item changes."),
        PortRequired(
            "aquafir.quest-bonus", "QuestBonus",
            "Tracks completed-quest points on each character and multiplies earned experience by a configurable quest-completion bonus.",
            "The bonus is recalculated at login and when quests complete, then stored on the character in a configured property.",
            ModDataImpact.CharacterData, ModRemovalPolicy.ChangesRemain,
            "Turning it off stops future bonus experience but does not remove experience already gained."),
        PortRequired(
            "aquafir.raise", "Raise",
            "Raises the level cap and adds alternate or effectively infinite advancement for levels, skills, attributes, vitals, and ratings.",
            "Custom progression counts and spent resources are stored in character properties, with refund and inspection commands.",
            ModDataImpact.CharacterData, ModRemovalPolicy.DoNotRemove,
            "High risk: characters advanced beyond normal ACE limits may become inconsistent or lose access to progression if the mod is removed."),
        PortRequired(
            "aquafir.selective-startup", "SelectiveStartup",
            "A developer sample that can skip selected ACE startup systems such as networking, world loading, houses, players, or events.",
            "This can deliberately create a partial server for testing. Disabling the wrong subsystem can prevent the launcher from reaching a playable world.",
            ModDataImpact.WorldData, ModRemovalPolicy.Safe,
            "Dangerous for normal play; the launcher will not offer one-click installation.",
            availability: ModCatalogAvailability.NotRecommended),
        new ModCatalogEntry(
            "aquafir.society-tailoring", "SocietyTailoring", "aquafir",
            "Allows Society armor to be used in tailoring while keeping inventory and retained-item checks.",
            "Tailoring changes the resulting item permanently; disabling the mod stops new uses but does not reverse completed tailoring.",
            BaseUrl + "SocietyTailoring", ModCatalogAvailability.Preview, ModDataImpact.CharacterData, ModRemovalPolicy.ChangesRemain,
            "Back up first. Existing tailored items remain after the mod is turned off.",
            TargetAceVersion: "ACE.Server 1.1 / OpenDereth",
            TargetFramework: ".NET 10 preview port",
            PackageRelativePath: @"Packages\aquafir.society-tailoring-1.0.0-sp1.zip",
            PortSourceUrl: PortBaseUrl + "SocietyTailoring"),
        PortRequired(
            "aquafir.tinkering", "Tinkering",
            "Reworks tinkering requirement checks, maximum attempts, difficulty scaling, and handling of additional imbue effects.",
            "Items can be tinkered under rules that differ from stock ACE, so their saved tinkering counts may exceed normal expectations.",
            ModDataImpact.CharacterData, ModRemovalPolicy.BackupRequired,
            "Back up first. Turning it off cannot undo tinkers or imbues already applied to items."),
        PortRequired(
            "aquafir.tower", "Tower",
            "An experimental game-mode overhaul combining tower floors, hardcore play, banking, custom loot, aetheria, melee magic, PvP, offline progress, and speedrun features.",
            "The upstream README is incomplete and the source contains several interdependent systems with persistent character and item state.",
            ModDataImpact.WorldData, ModRemovalPolicy.DoNotRemove,
            "High risk and unfinished: use only on a separate test world after it is ported and validated.",
            availability: ModCatalogAvailability.Experimental)
    };

    private static ModCatalogEntry PortRequired(
        string id,
        string name,
        string description,
        string details,
        ModDataImpact dataImpact,
        ModRemovalPolicy removalPolicy,
        string safetyNotice,
        IReadOnlyList<string>? dependencyIds = null,
        IReadOnlyList<string>? conflictIds = null,
        ModCatalogAvailability availability = ModCatalogAvailability.NeedsPort) => new(
            id,
            name,
            "aquafir",
            description,
            details,
            BaseUrl + name,
            availability,
            dataImpact,
            removalPolicy,
            safetyNotice,
            DependencyIds: dependencyIds,
            ConflictIds: conflictIds);
}
