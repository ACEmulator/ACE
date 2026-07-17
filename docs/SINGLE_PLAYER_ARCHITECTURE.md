# OpenDereth architecture

## Process boundary

`ACE.SinglePlayer.exe` is a WinForms orchestrator, not an offline rewrite of ACE. It owns and monitors these optional child processes:

1. The release's pinned `Dependencies\MariaDB\bin\mariadbd.exe` in automatic private-database mode.
2. The published `ACE.Server.exe`, started with `--config <absolute path>` and `--ready-file <absolute path>`.
3. `acclient.exe`, launched directly in Vanilla mode or through the package's isolated x86 bridge to an installed ThwargLauncher injector.

The original server behavior is unchanged when the two new arguments are omitted. The normal cross-platform `Source/ACE.sln` is unchanged; Windows packaging uses `Source/ACE.SinglePlayer.slnx`.

The release package publishes the launcher and server self-contained for Windows x64. This avoids a first-run dependency on Windows' .NET runtime installer. The isolated Decal helper remains self-contained Windows x86. New setups place operational Runtime files and the private MariaDB data under `%LOCALAPPDATA%\OpenDereth` so cloud-sync tools do not lock database files or copy short-lived configuration credentials. A one-time startup migration moves the complete legacy `%LOCALAPPDATA%\ACESinglePlayer` tree and repairs saved absolute runtime and database paths.

## Configuration and readiness

The launcher references `ACE.Common` and serializes the current `MasterConfiguration`. It does not edit JavaScript with regular expressions. Defaults are `127.0.0.1`, port 9000, four sessions, automatic account creation, no world precache, automatic ACE schema updates, and no automatic world-database download/update.

At startup ACE.Server deletes a stale requested ready file. After all managers and commands are initialized and `WorldManager.Open` has made logins possible, it atomically writes process ID, world name, host, port, and UTC timestamp. Normal process exit removes the file. The launcher requires a matching PID, loopback host, expected port, a live process, and an active local UDP listener. ACE uses UDP 9000/9001, so a TCP probe would be incorrect.

## Persistence and security

The same account name and randomly generated password are passed on every launch. Windows DPAPI protects stored account and database passwords for the current user. Settings and generated server configuration receive a current-user-only Windows ACL. The database credential must be present in the generated ACE Config.js while the server runs; that file is ignored by Git and excluded from packages.

Process arguments are added individually. Arguments and configuration content are never logged. Process ownership records contain PID, start time, and executable path; shutdown never selects a process by name alone. ACE's supported `stop-now` console command is used before a bounded forced stop.

## Databases

`IDatabaseRuntime` separates an advanced external connection from the default private MariaDB. Validation checks authentication, all three schema names, core tables (`account`, `character`, and `weenie`), and the required Human world record (`weenie.class_Id = 1`). `DatabaseBootstrapper` uses the packaged ACE authentication/shard base SQL and the pinned complete world SQL under `Dependencies\World`. It rejects schema-only world data. External databases remain conservative and are never rewritten when incomplete; an isolated private database with empty world tables can be repaired by streaming the bundled full dump through MariaDB's local import client.

Private mode prefers the pinned bundled `mariadbd.exe` and requires its matching initializer and import client. First initialization occurs in a unique staging directory and is promoted to `%LOCALAPPDATA%\OpenDereth\Database` only after MariaDB's system tables exist. Keeping database data outside the release folder prevents cloud-sync locks and allows application upgrades without replacing characters. Existing `Runtime\Database` data is copied once to the local location and retained at the old path as a backup. The launcher chooses an available local port, passes `--no-defaults`, binds to `127.0.0.1`, disables remote-root creation, and never opens a firewall port.

Two random passwords are generated: a private administrator credential used only for lifecycle/provisioning and a dedicated `ace_singleplayer` application credential granted only over the three configured ACE schemas. Both are protected by Windows DPAPI. MariaDB output is redacted against both secrets. The launcher sends MariaDB's supported `SHUTDOWN` statement before forcing only its own child after a timeout. A version-1 loopback `root` configuration migrates into the private setup flow; non-root external configurations remain external.

## Client providers

`ServerDatProvisioner` copies the four user-supplied DAT files into `%LOCALAPPDATA%\OpenDereth\ServerData` before ACE.Server starts. File size and modification time are used to refresh only changed files, and every replacement is staged before it becomes active. ACE.Server keeps its DAT streams open for performance, so this private copy prevents it from locking the original client files. The proprietary DATs remain local and are never placed in the release archive or repository.

`IClientLaunchProvider` isolates client choices. `DirectClientLaunchProvider` uses `ProcessStartInfo.ArgumentList` for `-a`, `-v`, and `-h`. Vanilla mode has no Decal, Thwarg, Chorizite, or injection dependency.

`DecalClientLaunchProvider` detects `SOFTWARE\Decal\Agent\AgentPath` and `SOFTWARE\Thwargle Games\ThwargLauncher\Path` in 32/64-bit HKLM/HKCU views, then validates Decal's `Inject.dll` and ThwargLauncher's `injector.dll`. A separately built, self-contained x86 bridge loads the installed Thwarg native launcher and asks it to start `acclient.exe` with Decal's `DecalStartup` entry point. The account, protected password, and loopback server address are supplied automatically by OpenDereth. No Decal, ThwargLauncher, ThwargFilter, or third-party injector binary is bundled or copied.

The main window exposes this choice as a **Use Decal** checkbox beside **PLAY**. Checking it persists `ClientLaunchMode.Decal`; unchecking it persists `ClientLaunchMode.Vanilla`. The checkbox is disabled while the game or server is running and when either required installation is missing. Database and path setup contain no separate client-mode dropdown.

## Mods

ACE server mods remain owned by the existing `ModManager`, `ModContainer`, `IHarmonyMod`, and `Meta.json` flow. The launcher only inventories metadata and safely changes `Enabled` while preserving unknown JSON fields. Editing is disabled while the server runs.

Decal registry inventory is read-only and clearly typed as client-side. Chorizite and world-content providers are empty future boundaries. A DLL's presence never establishes compatibility; ACE.Server output remains the source for load errors.

## Known Phase-1 limitations

- A real client and all four proprietary DAT files are user-supplied.
- The client and live character-selection screen were not available for end-to-end testing here.
- Decal mode requires separate working Decal and ThwargLauncher installations. Client creation through the installed Thwarg injector is verified; individual plugins remain outside this project's control.
- Chorizite launch and package installation are reserved for future work.
