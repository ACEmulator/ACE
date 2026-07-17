# OpenDereth: first run

OpenDereth keeps ACE.Server and a bundled MariaDB instance as private local processes and launches the original client directly. The release includes the server, database runtime, complete world, .NET, and curated server mods. It does not include Asheron's Call, proprietary DAT files, Decal, or ThwargLauncher.

## Before opening the launcher

1. Put the package in a folder your Windows user can write to, such as `C:\Games\OpenDereth`. Do not install it under `Program Files` unless you deliberately choose a different writable Runtime directory. The package is self-contained and does not require a separate .NET installation.
2. Have an existing `acclient.exe` installation. Its directory must contain:
   - `client_cell_1.dat`
   - `client_portal.dat`
   - `client_local_English.dat`
   - `client_highres.dat`
   Keep the complete client installation together in one writable folder outside OneDrive and `Program Files`.
3. Extract the complete release. Do not move or delete its `Server` or `Dependencies` folders.

## First start

Double-click `ACE.SinglePlayer.exe`.

1. The launcher checks common AC locations automatically. If it cannot find a complete client, select the folder containing `acclient.exe` and all four DAT files.
2. The main launcher opens immediately. Check **Use Decal** when a separate working Decal and ThwargLauncher installation is detected; otherwise leave it unchecked.
3. Click **PLAY**. The first run creates the private database and imports the bundled world, which can take several minutes. Existing complete databases are never overwritten. Later launches skip the import.

Before the server starts for the first time, the launcher also makes a private copy of the four DAT files under `%LOCALAPPDATA%\OpenDereth\ServerData`. This needs roughly the same amount of free disk space as the four original DATs and prevents ACE.Server from locking the files used by the game client. Later launches reuse the copy unless the originals change.

The private database lives at `%LOCALAPPDATA%\OpenDereth\Database`. The launcher uses the bundled `Dependencies\MariaDB` programs, initializes into a staging directory, and moves it into place only after initialization succeeds. It creates a dedicated `ace_singleplayer` database user; generated administrator and application passwords are protected with Windows DPAPI and are not displayed or logged. Any separately installed MariaDB service and its `root` account are not changed.

To use a separately administered server instead, choose **Existing MariaDB/MySQL (advanced)** and enter its host, port, username, password, and database names. Use **Test Connection** and **Initialize Missing Databases** as needed.

## Normal play

1. Double-click `ACE.SinglePlayer.exe`.
2. Click **PLAY** once.
3. The launcher starts its private database when selected, checks the persistent schemas, writes a private loopback-only config, starts ACE.Server, waits for an atomic ready signal and the UDP listener, and launches `acclient.exe`.
4. Use the normal character-selection screen. ACE automatically creates the persistent local account on its first successful login.

Repeated Play clicks do not create duplicate processes. By default, closing the game gracefully stops the launcher-owned server. The Stop button requests ACE's `stop-now` console command and forces termination only if that verified child does not exit within 20 seconds.

Characters live in `%LOCALAPPDATA%\OpenDereth\Database\ace_shard` when private mode is selected, not in the launcher executable. Preserve the entire `%LOCALAPPDATA%\OpenDereth` directory across upgrades and back it up before replacing or resetting anything. Settings live at `%LOCALAPPDATA%\OpenDereth\settings.json`; secrets there are DPAPI-protected. The generated Runtime `Config.js` necessarily contains the database password for ACE.Server and is restricted to the current Windows user. Existing data from an earlier preview is moved automatically from `%LOCALAPPDATA%\ACESinglePlayer` before the launcher reads it.

Use **Open Logs** for startup errors. The launcher never writes account or database passwords to its log. The real client and a live ACE world database are required for an end-to-end character-screen test.
