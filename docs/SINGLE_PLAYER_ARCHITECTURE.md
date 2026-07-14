# ACE Single Player architecture

## Process boundary

`ACE.SinglePlayer.exe` is a WinForms orchestrator, not an offline rewrite of ACE. It owns and monitors these optional child processes:

1. A user-supplied `mariadbd.exe` only in experimental managed mode.
2. The published `ACE.Server.exe`, started with `--config <absolute path>` and `--ready-file <absolute path>`.
3. `acclient.exe`, launched directly in Vanilla mode or through the package's isolated x86 Decal helper.

The original server behavior is unchanged when the two new arguments are omitted. The normal cross-platform `Source/ACE.sln` is unchanged; Windows packaging uses `Source/ACE.SinglePlayer.slnx`.

The release package publishes the launcher and server self-contained for Windows x64. This avoids a first-run dependency on Windows' .NET runtime installer. The isolated Decal helper remains self-contained Windows x86.

## Configuration and readiness

The launcher references `ACE.Common` and serializes the current `MasterConfiguration`. It does not edit JavaScript with regular expressions. Defaults are `127.0.0.1`, port 9000, four sessions, automatic account creation, no world precache, automatic ACE schema updates, and no automatic world-database download/update.

At startup ACE.Server deletes a stale requested ready file. After all managers and commands are initialized and `WorldManager.Open` has made logins possible, it atomically writes process ID, world name, host, port, and UTC timestamp. Normal process exit removes the file. The launcher requires a matching PID, loopback host, expected port, a live process, and an active local UDP listener. ACE uses UDP 9000/9001, so a TCP probe would be incorrect.

## Persistence and security

The same account name and randomly generated password are passed on every launch. Windows DPAPI protects stored account and database passwords for the current user. Settings and generated server configuration receive a current-user-only Windows ACL. The database credential must be present in the generated ACE Config.js while the server runs; that file is ignored by Git and excluded from packages.

Process arguments are added individually. Arguments and configuration content are never logged. Process ownership records contain PID, start time, and executable path; shutdown never selects a process by name alone. ACE's supported `stop-now` console command is used before a bounded forced stop.

## Databases

`IDatabaseRuntime` separates external and managed MariaDB. External mode checks authentication, all three schema names, and core tables (`account`, `character`, and `weenie`). `DatabaseBootstrapper` uses the packaged ACE authentication/shard base SQL and a user-selected world SQL package. It applies a base only when the target schema is entirely absent and refuses partial or existing incomplete schemas.

Managed mode is deliberately experimental. It accepts a specific `mariadbd.exe`, binds it to loopback, and uses `Runtime\Database`, but requires the matching distribution to have initialized that data directory already.

## Client providers

`IClientLaunchProvider` isolates client choices. `DirectClientLaunchProvider` uses `ProcessStartInfo.ArgumentList` for `-a`, `-v`, `-h`, and `-rodat off`; it has no Decal, Thwarg, Chorizite, or injection dependency.

`DecalClientLaunchProvider` detects `SOFTWARE\Decal\Agent\AgentPath` in 32/64-bit HKLM/HKCU views and validates `Inject.dll`. A separately built, self-contained x86 helper creates the 32-bit client suspended, loads the installed `Inject.dll`, invokes its exported `DecalStartup`, resumes the client, and returns a diagnostic PID. It bundles no Decal or third-party injector binaries and does not use ThwargLauncher or ThwargFilter. Invocation is diagnosed, but actual in-game Decal/plugin loading was not end-to-end tested in this repository environment.

## Mods

ACE server mods remain owned by the existing `ModManager`, `ModContainer`, `IHarmonyMod`, and `Meta.json` flow. The launcher only inventories metadata and safely changes `Enabled` while preserving unknown JSON fields. Editing is disabled while the server runs.

Decal registry inventory is read-only and clearly typed as client-side. Chorizite and world-content providers are empty future boundaries. A DLL's presence never establishes compatibility; ACE.Server output remains the source for load errors.

## Known Phase-1 limitations

- A real client, DATs, MariaDB service, and populated world package are user-supplied.
- Managed MariaDB does not perform distribution-specific first initialization.
- The client and live character-selection screen were not available for end-to-end testing here.
- Decal's startup export is invoked and diagnosed, but in-game injection/plugin behavior requires a machine with a working Decal/client installation.
- Chorizite launch and package installation are reserved for future work.
