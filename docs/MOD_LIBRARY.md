# Mod library and saved-game safety

OpenDereth includes a curated mod library. Open **Server Mods** from the launcher, select a mod, and read its description, requirements, compatibility status, and saved-game warning before changing anything.

## Current status

The library inventories all 22 samples in [aquafir/ACE.BaseMod](https://github.com/aquafir/ACE.BaseMod/tree/master/Samples). The upstream `Meta.json` files contain placeholder descriptions, so OpenDereth's descriptions and safety policies are curated from the actual READMEs and source.

`CriticalOverride` is curated for this repository's .NET 10 ACE build. `HelloCommand`, `SocietyTailoring`, and **Expanded Cast on Strike** are installable **Preview** ports. Their checksum-verified packages build and pass automated registration/Harmony-target, packaging, and launcher tests, but they have not received thorough in-game testing. OptimShi's `CustomClothingBase` v1.11 is also an installable Preview. Its unmodified official .NET 8 package is pinned by SHA-256 and passed an isolated load test in the exact .NET 10 ACE mod container, but custom clothing behavior and saved-world compatibility are not thoroughly tested. The launcher repeats those warnings before installation. The other samples remain visible but are marked **Needs port**, **Experimental**, or **Not recommended** until they compile against this exact ACE version and pass suitable tests.

### Expanded Cast on Strike

**Expanded Cast on Strike** implements the combat-filter change documented by [titaniumweiner/ACEUniqueWeenies](https://github.com/titaniumweiner/ACEUniqueWeenies). Stock ACE runs the equipped-item proc pass only for Aetheria. This mod instead accepts an equipped item when it has a proc, its `CloakWeaveProc` value is not `1`, and its self-target flag matches the current attack. It changes server behavior only; install the mod here, then import compatible repository SQL separately through **Custom Weenies**.

Turning the mod off is safe for the database: custom items remain, but their non-Aetheria equipped procs stop firing after the server restarts. The port intentionally matches the documented filter exactly and is marked Preview because its proc frequency and interactions with every custom item have not received thorough in-game testing. The reviewed repository commit is [`75d7036c`](https://github.com/titaniumweiner/ACEUniqueWeenies/commit/75d7036c5b281e70b23aba60732aca681ee443d6).

### CustomClothingBase

`CustomClothingBase` loads custom ClothingTable entries from JSON without requiring client DAT updates. After installation, place custom `*.json` definitions directly in its `json` folder under the private ACE Mods directory. It can export entries with `@clothingbase-export` and reload with `@clear-clothing-cache`. Custom ClothingBase IDs can become dependencies of saved items or world content, so the launcher blocks removal and recommends a full backup before use. The [original source and examples](https://github.com/OptimShi/CustomClothingBase) remain the authoritative documentation.

The upstream repository currently has no `LICENSE` file. The official v1.11 binaries are included unchanged based on OptimShi's permission reported by the OpenDereth project maintainer, with attribution and links preserved in the package and release notices.

## Importing a separately rebuilt package

**Import a Mod ZIP...** at the top of the Mods window accepts an OpenDereth mod ZIP after the game and server are stopped. Import is atomic and requires:

- `ace-mod.json` at the ZIP root;
- mod files under the ZIP's `mod/` directory;
- `mod/Meta.json` and the manifest's entry DLL;
- a matching `.zip.sha256` checksum file beside the ZIP.

The manifest format is:

```json
{
  "formatVersion": 1,
  "id": "author.mod-id",
  "name": "Mod Name",
  "version": "1.0.0",
  "folderName": "ModAssemblyName",
  "entryAssembly": "ModAssemblyName.dll"
}
```

Importing validates identity, checksum, archive paths, size, required files, and ACE's folder/assembly naming rule. It cannot prove that arbitrary DLL code is compatible or safe. In particular, downloading an original Aquafir .NET 8 source folder and putting it in a ZIP does **not** port it to this .NET 10 ACE build. A developer must rebuild and test that source first. The launcher gives an additional warning when a selected package corresponds to a catalog entry that still says **Needs port**.

See [How to make and import an OpenDereth mod](MOD_AUTHOR_GUIDE.md) for a working project skeleton, metadata examples, the package helper, testing checklist, and import steps.

## Install, turn off, and remove are different operations

- **Install** validates the package identity and checksum, rejects unsafe archive paths, stages all files, and only then moves the complete mod into the ACE `Mods` directory.
- **Turn off** changes the mod's `Enabled` value. It stops the mod code after the next server restart, but it does not erase changes already saved to characters, items, or the world.
- **Remove** moves a safe mod out of ACE into `%LOCALAPPDATA%\OpenDereth\Runtime\RemovedMods`. The files are retained for recovery instead of being deleted.

The launcher assigns every curated mod one of these saved-data policies:

- **Safe**: no persistent game data depends on the mod. It can be turned off or moved to recovery.
- **Changes remain**: the mod can be turned off, but completed actions such as item tailoring, experience awards, or character moves are not undone.
- **Backup required**: turning it off may leave unusual but loadable character/item state. Back up `%LOCALAPPDATA%\OpenDereth` first.
- **Do not remove**: characters, items, custom spells, balances, or world objects may require the mod. Removal is blocked until the package provides a tested cleanup migration.

All mod changes require the game and local server to be stopped. This prevents ACE from loading a half-copied assembly or saving data during a change.

## Dependencies and conflicts

Each curated package can declare required mods and incompatible mods. Before installation, the launcher checks that requirements are installed and conflicts are absent. This is a preflight check, not a guarantee that two arbitrary Harmony patches are behaviorally compatible. A curated package is only marked ready after it has been rebuilt for this ACE version and tested with the packages it declares compatible.

Package updates will eventually add ownership information for custom IDs, database migrations, and explicit upgrade/rollback steps. Unknown manually installed mods are shown conservatively because the launcher cannot infer their saved-data behavior from a DLL.

## New items, dungeons, towns, and landscape

These are not all the same kind of mod:

1. A server mod changes ACE behavior. It can reuse objects and art already known to the original client without changing client DAT files.
2. A world-content pack adds or changes weenies, encounters, quests, recipes, landblock spawns, and other database content. It needs versioned SQL/data migrations, ownership of custom ID ranges, and a database backup/rollback plan.
3. A client-content pack is required for genuinely new models, textures, icons, sounds, terrain, buildings, or landblock geometry. The client and server must use matching DAT content. These packs need checksums, a writable copied client, and an atomic way to switch the complete matched DAT set.

A new town or dungeon can often use existing client assets with server/world changes, but new terrain or art requires matched client DAT changes. For that reason, future **World Packs** will be managed separately from ordinary DLL mods and will not be presented as a harmless on/off checkbox.

## Backups

Private-world characters and world state live under `%LOCALAPPDATA%\OpenDereth\Database`. Until automatic per-mod snapshots are implemented, close the launcher and copy the entire `%LOCALAPPDATA%\OpenDereth` folder before enabling a mod labeled **Backup required** or **Do not remove**.
