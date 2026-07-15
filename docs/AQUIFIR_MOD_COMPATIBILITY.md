# Aquafir project compatibility with current ACE

Assessment updated: 2026-07-15. The portable release uses ACE.Server build `1.77.4782` from upstream commit `650c5b75` on .NET 10. The external repositories were inspected at their default branches; client-side projects were not executed as ACE server mods.

## ACE.BaseMod sample catalog update

The launcher now inventories all 22 folders under `aquafir/ACE.BaseMod/Samples` with curated descriptions and saved-game policies. The upstream samples target .NET 8, reference an installed ACE server, and use placeholder metadata, so they are not offered as compatible binaries merely because their source is public.

`CriticalOverride` is the curated one-click port. `HelloCommand` and `SocietyTailoring` are one-click **Preview** ports compiled against the same server and packaged with identity manifests and SHA-256 checksums. Automated tests cover HelloCommand command registration/removal and SocietyTailoring's current Harmony target/signature, but neither preview is represented as thoroughly gameplay-tested. Every remaining sample stays visible as **Needs port**, **Experimental**, or **Not recommended** until it receives suitable rebuild and test treatment. See `docs/MOD_LIBRARY.md` for install/removal behavior and persistent-data rules.

| Project | Type | Current target/dependencies | Builds against this ACE as-is? | Without Thwarg/Decal? | Status and recommendation |
|---|---|---|---|---|---|
| `aquafir/ACE.BaseMod` | ACE server-mod framework/templates | `ACE.Shared` net8.0 x64, Harmony 2.3.3, Krafs.Publicizer; package notes say ACE 1.64.4610 and references default to `C:\ACE\Server` | No checkout-to-checkout build path; it targets an older installed ACE output. Current ACE already contains the corresponding `ModManager`, `ModContainer`, `IHarmonyMod`, metadata, commands, and Harmony loader. | Yes, server-side | **Needs source port / do not integrate as a second loader.** Reuse current ACE's loader. Port only still-useful shared helpers or individual samples after license/API review. |
| `aquafir/LootAll` | ACE server mod | net8.0 x64, Harmony 2.3.2, `ACEmulator.ACE.Shared` 1.0.0, publicized/hardcoded `C:\ACE\Server` assemblies; sample-like Meta.json | No. It is not configured for current net10 ACE and was not proven against current APIs. | Yes | **Compatible after rebuild or needs source port.** Retarget net10, reference the exact current ACE publish, update shared/Harmony assumptions, then test load and gameplay. Place only a successful port in the ACE Mods directory. |
| `aquafir/AltLevels` | ACE server mod | net8.0 x64, Harmony 2.3.3, `ACEmulator.ACE.Shared` 1.0.0, EF/ACE assemblies from `C:\ACE\Server`; sample-like Meta.json | No. Same installed-old-ACE assumptions; current API compatibility is unverified. | Yes | **Needs source port.** Retarget/rebuild against current ACE and add behavior/data-migration tests before installation. |
| `aquafir/Chorizite` | Client plugin framework/launcher, not an ACE server mod | net8.0; x86 launcher; native bootstrapper, standalone loader, RmlUI/AC protocol and its own manifest format | Not applicable to ACE.Server; it is a separate client framework and build graph. | Yes without Thwarg and Decal; it uses its own bootstrap/injection path and may optionally load Decal | **Wrong mod type for ModsDirectory / future integration.** Implement the reserved `ChorizitePluginProvider` and launch provider only after its bootstrap licensing, packaging, and current client behavior are tested. |
| `aquafir/UtilityFace` | Decal client plugin and hot-reload network filter | .NET Framework 4.8 x86, Decal 2.9.8.2 interop, D3DService/DirectX, UtilityBelt.Service; some machine-specific reference paths | Not applicable to ACE.Server. Its project may need dependency-path cleanup to reproduce a build. | Works without Thwarg, but not without Decal and its dependencies | **Wrong mod type for ModsDirectory.** Inventory through the read-only Decal provider. Install/register through a tested Decal workflow; use only when Decal launch mode succeeds. |
| `aquafir/InterfaceReplacement` (`InventoryUI`) | Decal client UI plugin and hot-reload network filter | .NET Framework 4.8 x86, Decal Adapter/Interop 2.9.8.2, UtilityBelt.Service, NSIS | Not applicable to ACE.Server. It targets Decal rather than ACE APIs. | Works without Thwarg, but not without Decal and UtilityBelt.Service | **Wrong mod type for ModsDirectory.** Treat as a Decal plugin, keep registry management external/read-only for now, and test with the optional Decal provider. |

## Important distinctions

- **ACE server mods** run inside ACE.Server through its existing Harmony loader. They can work with the Vanilla client and never need Decal.
- **Decal plugins** run inside the 32-bit client after Decal injection. They never belong in ACE's server Mods directory.
- **Chorizite plugins** use Chorizite's bootstrap and manifest system. They are neither ACE Harmony mods nor Decal registrations.
- **World content** is database/content SQL and must be validated and backed up as data, not treated as an arbitrary DLL mod.

The launcher reports metadata and registration state, not binary compatibility. A successful rebuild plus a clean ACE.Server load and functional test is required before marking a server mod Compatible.
