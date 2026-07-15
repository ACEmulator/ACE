# ACE Single Player Launcher

ACE Single Player Launcher turns the open-source [ACEmulator ACE server](https://github.com/ACEmulator/ACE) into a private, local game session on Windows. One **PLAY** button starts an isolated MariaDB database, starts ACE.Server on your own computer, waits until the world is ready, and launches the original Asheron's Call client.

The launcher is designed for players who want a persistent personal world without manually maintaining server configuration files or entering a MariaDB root password. Characters and world state are kept between sessions, and nothing is exposed to the internet by default.

## Highlights

- Private, loopback-only ACE server and database
- Automatic database credentials protected for the current Windows user
- Persistent characters and world state
- Direct Vanilla client launch, with one-click Decal support when Decal and ThwargLauncher are already installed
- Self-contained Windows release; players do not need to install .NET
- Original game client, DAT files, MariaDB, and ACE World data remain user-supplied

## Install

The short version is:

1. Install MariaDB for Windows.
2. Have a working Asheron's Call client and DAT files. The usual client path is `C:\Turbine\Asheron's Call\acclient.exe`.
3. Download and extract the populated `ACE-World-Database-*.sql` file from the [official ACE World releases](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest).
4. Download the latest ACE Single Player release ZIP and extract it to a normal writable folder such as `C:\Games\ACE-SinglePlayer`.
5. Run `ACE.SinglePlayer.exe`, keep **Automatic private database**, select the client and world SQL file, save setup, and click **PLAY**.

See the [complete installation guide](docs/SINGLE_PLAYER_INSTALL.md) for prerequisites, expected file names, first-run instructions, troubleshooting, backups, and upgrade guidance.

> [!IMPORTANT]
> This repository and its release packages do not contain Asheron's Call, proprietary DAT files, MariaDB, Decal, ThwargLauncher, or the ACE World database. Do not upload those files when sharing builds or reporting problems.

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
