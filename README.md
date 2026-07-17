# ACE Single Player Launcher

ACE Single Player Launcher turns the open-source [ACEmulator ACE server](https://github.com/ACEmulator/ACE) into a private, local game session on Windows. One **PLAY** button starts an isolated MariaDB database, starts ACE.Server on your own computer, waits until the world is ready, and launches the original Asheron's Call client.

The launcher is designed for players who want a persistent personal world without manually maintaining server configuration files or entering a MariaDB root password. Characters and world state are kept between sessions, and nothing is exposed to the internet by default.

## Highlights

- Private, loopback-only ACE server and database
- Automatic database credentials protected for the current Windows user
- Persistent characters and world state
- Direct Vanilla client launch, with one-click Decal support when Decal and ThwargLauncher are already installed
- Curated server-mod library plus guarded AceForge Custom Weenie SQL imports
- Self-contained Windows release; players do not need to install .NET
- Pinned portable MariaDB and complete ACE World database included in the release
- Only the original game client and its four proprietary DAT files remain user-supplied

## Install

The short version is:

1. Have a complete Asheron's Call client in one writable folder with `acclient.exe` and all four client DAT files. The usual path is `C:\Turbine\Asheron's Call`.
2. Download the latest ACE Single Player release ZIP and extract the entire archive to a normal writable folder such as `C:\Games\ACE-SinglePlayer`.
3. Run `ACE.SinglePlayer.exe`. If the client is not found automatically, select its folder once, then click **PLAY**. The first private-world import can take several minutes; later launches are much faster.

On the first Play, the launcher also creates a private server copy of the four DAT files under `%LOCALAPPDATA%\ACESinglePlayer\ServerData`. This prevents ACE.Server from locking the original files needed by the client and requires roughly the same amount of free disk space as the four DATs. These local copies are never included in release ZIPs or GitHub.

The portable release currently pins ACEmulator server build `1.77.4782` from upstream commit `650c5b75`, ACE World `v0.9.294`, and MariaDB `12.3.2 LTS`. Exact source URLs, hashes, and licenses are recorded in `BUNDLE-MANIFEST.json` and [the third-party notices](docs/THIRD_PARTY_NOTICES.md).

## Mods

Open **Server Mods** in the launcher to browse the curated mod library. Installed and ready-to-install entries are listed before unavailable ports. Every entry includes a plain-language description, compatibility result, requirements, source-code links, and a saved-game safety warning. `CriticalOverride` is curated for one-click installation. `HelloCommand`, `SocietyTailoring`, and **Expanded Cast on Strike** are installable **Preview** ports. The last one enables the non-Aetheria equipped-item procs documented by [ACEUniqueWeenies](https://github.com/titaniumweiner/ACEUniqueWeenies); its custom item SQL remains a separate Custom Weenies import. OptimShi's `CustomClothingBase` v1.11 is also an installable Preview using the author's checksum-pinned, unmodified official DLLs after a successful current-ACE loader test. Preview mods are clearly marked as not thoroughly tested in game. Other samples remain listed but unavailable until they are ported and tested. **Import a Mod ZIP...** can atomically install separately rebuilt, checksummed packages in the documented ACE Single Player ZIP format.

Turning a mod off stops its code after a server restart, but it does not undo experience, items, balances, character properties, or world content already saved by that mod. The launcher blocks removal of mods whose saved data may still depend on them and moves safely removable files to a recovery folder rather than deleting them. See [Mod library and saved-game safety](docs/MOD_LIBRARY.md) for the full policy, or [How to make and import a mod](docs/MOD_AUTHOR_GUIDE.md) to build a compatible package.

## Custom Weenies and AceForge

Open **Custom Weenies** to import per-weenie `.sql` files created by [AceForge](https://github.com/shemtar-90/AceForge/releases/tag/v0.3.36). Choose an AceForge output folder or individual SQL files; the launcher previews each WCID, validates a strict list of ACE weenie-property operations, skips unsupported content, checks whether WCIDs already exist, and creates a complete `ace_world` backup before applying the import in one database transaction. Automatic imports require the launcher's Private Database mode.

Custom weenies become part of the saved world and do not have a one-click uninstaller. Quest, recipe, event, treasure, landscape, and client-DAT changes are not imported by this screen. See [Importing AceForge custom weenies](docs/CUSTOM_WEENIES.md) for the exact workflow and safety limits.

See the [complete installation guide](docs/SINGLE_PLAYER_INSTALL.md) for prerequisites, expected file names, first-run instructions, troubleshooting, backups, and upgrade guidance.

> [!IMPORTANT]
> The release does not contain Asheron's Call, `acclient.exe`, proprietary DAT files, Decal, or ThwargLauncher. It does include the redistributable open-source ACE server, ACE World database, and MariaDB runtime with their licenses and source links. Never upload your client or DAT files when sharing builds or reporting problems.

## Project status

This is a Windows-focused single-player launcher built as a maintained fork of ACE. Vanilla launch and the private-database path are the primary supported workflow. Decal integration is optional, and Chorizite integration is reserved for future work. See the [architecture](docs/SINGLE_PLAYER_ARCHITECTURE.md), [build instructions](docs/SINGLE_PLAYER_BUILD_AND_TEST.md), and [roadmap](docs/SINGLE_PLAYER_ROADMAP.md) for technical details.

## Privacy and local files

User-specific settings and protected credentials are stored under `%LOCALAPPDATA%\ACESinglePlayer`. Private database files, settings, logs, client files, and DAT files are ignored by Git and rejected by the public packaging script. Public release packages also exclude debug symbols that could reveal the build computer's source path.

## Upstream project

ACE Single Player is based on ACEmulator ACE and remains under ACE's AGPL-3.0 license. The original ACE project information follows.

---

# ACEmulator Core Server

[![Discord](https://img.shields.io/discord/261242462972936192.svg?label=play+now!&style=for-the-badge&logo=discord)](https://discord.gg/C2WzhP9)

Build status: [![GitHub last commit (master)](https://img.shields.io/github/last-commit/acemulator/ace/master)](https://github.com/ACEmulator/ACE/commits/master) [![Windows CI](https://ci.appveyor.com/api/projects/status/rqebda31cgu8u59w/branch/master?svg=true)](https://ci.appveyor.com/project/LtRipley36706/ace/branch/master) [![docker build](https://github.com/ACEmulator/ACE/actions/workflows/docker-image.yml/badge.svg)](https://hub.docker.com/r/acemulator/ace)

[![Download Latest Server Release](https://img.shields.io/github/v/release/ACEmulator/ACE?label=latest%20server%20release) ![GitHub Release Date](https://img.shields.io/github/release-date/acemulator/ace)](https://github.com/ACEmulator/ACE/releases/latest)
[![Download Latest World Database Release](https://img.shields.io/github/v/release/ACEmulator/ACE-World-16PY-Patches?label=latest%20world%20database%20release) ![GitHub Release Date](https://img.shields.io/github/release-date/acemulator/ACE-World-16PY-Patches)](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest)

[![GitHub All Releases](https://img.shields.io/github/downloads/acemulator/ace/total?label=server%20downloads)](https://github.com/ACEmulator/ACE/releases) [![GitHub All Releases](https://img.shields.io/github/downloads/acemulator/ACE-World-16PY-Patches/total?label=database%20downloads)](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases) [![Docker Pulls](https://img.shields.io/docker/pulls/acemulator/ace)](https://hub.docker.com/r/acemulator/ace)

**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * [![License](https://img.shields.io/github/license/acemulator/ace)](https://github.com/ACEmulator/ACE/blob/master/LICENSE)

***
## Disclaimer
**This project is for educational and non-commercial purposes only, use of the game client is for interoperability with the emulated server.**
- Asheron's Call was a registered trademark of Turbine, Inc. and WB Games Inc which has since expired.
- ACEmulator is not associated or affiliated in any way with Turbine, Inc. or WB Games Inc.
***
## Getting Started
Extended documentation can be found on the project [Wiki](https://github.com/ACEmulator/ACE/wiki).
* [Developing ACE](https://github.com/ACEmulator/ACE/wiki/ACE-Development)
* [Hosting ACE](https://github.com/ACEmulator/ACE/wiki/ACE-Hosting)
* [Content Creation](https://github.com/ACEmulator/ACE/wiki/Content-Creation)

## Contributions
* Contributions in the form of issues and pull requests are welcomed and encouraged.
* The preferred way to contribute is to fork the repo and submit a pull request on GitHub.
* Code style information can be found on the [Wiki](https://github.com/ACEmulator/ACE/wiki/Code-Style).

Please note that this project is released with a [Contributor Code of Conduct](https://github.com/ACEmulator/ACE/blob/master/CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## Bug Reports
* Please use the [issue tracker](https://github.com/ACEmulator/ACE/issues) provided by GitHub to send us bug reports.
* You may also discuss issues and bug reports on our discord listed below.

## Contact
* [Discord Channel](https://discord.gg/C2WzhP9)
