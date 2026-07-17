# How to make and import an OpenDereth mod

OpenDereth's Mod ZIP importer handles **server-code mods**: .NET DLLs that use ACE's `IHarmonyMod` interface. Per-weenie AceForge SQL uses the separate **Custom Weenies** screen described in [the Custom Weenies guide](CUSTOM_WEENIES.md). General world SQL packs and client DAT/landscape packs still need a different migration system and are not supported by the Mod ZIP importer.

The easiest starting point is the small [HelloCommand port](https://github.com/titaniumweiner/OpenDereth/tree/main/Source/ACE.SinglePlayer.Mods.HelloCommand). Copy that project, rename its assembly and namespace, and replace its command code with your own feature.

## 1. Create the project

Place the project beside `ACE.Server` under `Source` so it can compile against the exact server version bundled by the launcher:

```text
Source/
  ACE.Server/
  ACE.SinglePlayer.Mods.MyMod/
    ACE.SinglePlayer.Mods.MyMod.csproj
    Mod.cs
    Meta.json
    ace-mod.json
    README.md
```

Use this project file, changing `MyMod` to a simple name without spaces:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <AssemblyName>MyMod</AssemblyName>
    <RootNamespace>MyMod</RootNamespace>
    <PlatformTarget>x64</PlatformTarget>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Lib.Harmony" Version="2.4.2" ExcludeAssets="runtime" />
    <ProjectReference Include="..\ACE.Server\ACE.Server.csproj" Private="false" />
  </ItemGroup>
  <ItemGroup>
    <None Update="Meta.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
</Project>
```

## 2. Add the mod entry point

ACE requires a public `Mod` class in a namespace that matches the assembly name. The class implements `IHarmonyMod`:

```csharp
using ACE.Server.Mods;
using HarmonyLib;

namespace MyMod;

public sealed class Mod : IHarmonyMod
{
    private const string HarmonyId = "your-name.MyMod";
    private readonly Harmony harmony = new(HarmonyId);

    public void Initialize()
    {
        harmony.PatchAll(typeof(Mod).Assembly);
        ModManager.Log("MyMod loaded.");
    }

    public void Dispose() => harmony.UnpatchAll(HarmonyId);
}
```

Harmony patches must target methods that exist in this repository's current `ACE.Server` source. A DLL built for a different ACE release may compile against that release and still fail here because method names or parameters changed.

## 3. Describe the installed mod

Create `Meta.json`. The `Name` must match the assembly and folder name:

```json
{
  "Name": "MyMod",
  "Author": "Your name",
  "Description": "A plain-language explanation of what the mod changes.",
  "Version": "1.0.0",
  "Priority": 0,
  "Enabled": true,
  "HotReload": false,
  "RegisterCommands": false,
  "CatalogId": "your-name.my-mod",
  "TargetAceVersion": "ACE.Server 1.1 / .NET 10",
  "DataImpact": "SettingsOnly",
  "RemovalPolicy": "Safe"
}
```

Choose saved-data labels honestly:

- `None`: no saved data changes.
- `SettingsOnly`: runtime behavior changes, but saves do not depend on the mod.
- `CharacterData`: characters or items can be changed permanently.
- `WorldData`: database/world content can depend on the mod.
- `ClientData`: matching client files are required.

Use `Safe`, `ChangesRemain`, `BackupRequired`, or `DoNotRemove` for `RemovalPolicy`. The launcher treats unknown imported code conservatively even when these fields are present.

## 4. Add the package manifest

Create `ace-mod.json`:

```json
{
  "formatVersion": 1,
  "id": "your-name.my-mod",
  "name": "MyMod",
  "version": "1.0.0",
  "folderName": "MyMod",
  "entryAssembly": "MyMod.dll"
}
```

The package ID should be stable across updates. The name, folder, DLL, and `Meta.json` name must agree.

## 5. Build a checksummed import ZIP

From the repository root, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\package-mod.ps1 -ProjectDirectory .\Source\ACE.SinglePlayer.Mods.MyMod
```

The helper builds against the repository's ACE version and creates two files under `artifacts\Mods`:

```text
your-name.my-mod-1.0.0.zip
your-name.my-mod-1.0.0.zip.sha256
```

Internally, the ZIP has the format the launcher requires:

```text
ace-mod.json
mod/
  MyMod.dll
  Meta.json
  README.md
  LICENSE.txt
```

`Settings.json` is included automatically when the project has one. Keep the `.zip` and `.zip.sha256` files together when sharing the mod.

## 6. Import it

1. Stop the game and local server.
2. Open **Server Mods** in OpenDereth.
3. Click **Import a Mod ZIP...** at the top.
4. Choose the `.zip`; the launcher finds and verifies the adjacent `.zip.sha256` file.
5. Read the warning and confirm.
6. Click **Play** to start ACE with the mod.

The importer checks the checksum, manifest, required files, naming rules, archive paths, size limits, and atomic installation. It **cannot prove arbitrary DLL code is safe** and it cannot discover every conflict between two Harmony patches.

## Testing checklist before sharing

- Build using the exact OpenDereth branch you name as supported.
- Start the private server with only this mod enabled and inspect the server log for load errors.
- Exercise the changed behavior in game, including failure and boundary cases.
- Test a clean install, disable/re-enable, server restart, and removal when removal is claimed safe.
- Test with every declared dependency and likely patch conflict.
- Back up `%LOCALAPPDATA%\OpenDereth` before tests that change characters, items, or the world.
- Link to the complete source and license. Clearly label a package **Preview / not thoroughly tested** until real gameplay coverage exists.

Do not put Asheron's Call client files or DAT files in a mod ZIP. They are proprietary and the server-mod importer is not designed to switch client data safely.
