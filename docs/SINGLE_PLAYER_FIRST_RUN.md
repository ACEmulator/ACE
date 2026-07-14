# ACE Single Player: first run

ACE Single Player keeps ACE.Server as a private local process and launches the original client directly. It does not include or download Asheron's Call, DAT files, MariaDB, Decal, or community mods.

## Before opening the launcher

1. Put the package in a folder your Windows user can write to, such as `C:\Games\ACE-SinglePlayer`. Do not install it under `Program Files` unless you deliberately choose a different writable Runtime directory. The package is self-contained and does not require a separate .NET installation.
2. Have an existing `acclient.exe` installation. Its directory must contain:
   - `client_cell_1.dat`
   - `client_portal.dat`
   - `client_local_English.dat`
   - `client_highres.dat` is optional for ACE.Server, although the client installation normally includes it.
3. Have MariaDB/MySQL running locally. External mode is the dependable Phase-1 option. You need a database user that can create the three ACE databases during setup, or pre-create valid `ace_auth`, `ace_shard`, and `ace_world` databases yourself.
4. Have an appropriate ACE world-database SQL package available if `ace_world` is missing. The launcher never downloads a database from an unverified source.

## Setup wizard

Double-click `ACE.SinglePlayer.exe`.

1. Select `acclient.exe`. The wizard fills the DAT directory automatically when all three required DAT files are beside it.
2. Select the packaged `Server\ACE.Server.exe`.
3. Keep the package `Mods` and `Runtime` directories unless you intentionally store them elsewhere.
4. Choose **Already-running MariaDB/MySQL**, then enter host, port, username, password, and database names. The defaults are `127.0.0.1:3306`, `ace_auth`, `ace_shard`, and `ace_world`.
5. Click **Test Connection**. If databases are missing, select the world SQL package and click **Initialize Missing Databases**. Authentication and shard base schemas come from the packaged ACE server. Existing complete databases are never overwritten; partial databases produce an error and are not modified.
6. Keep the account name `singleplayer` or choose one permanent name. A strong password is generated once, protected with Windows DPAPI, and reused. Do not change or delete this account if you want the same characters.
7. Keep local port `9000` unless it conflicts with another application.
8. Choose **Vanilla** for the dependable direct-launch path. Choose **Decal** only when Decal and its `Inject.dll` are detected. Chorizite is displayed as future work and cannot be selected yet.
9. Save setup.

Experimental managed MariaDB mode starts only a user-supplied `mariadbd.exe` and an already initialized data directory at `Runtime\Database`. It does not download binaries or guess distribution-specific initialization flags. External mode is recommended for this release.

## Normal play

1. Double-click `ACE.SinglePlayer.exe`.
2. Click **PLAY** once.
3. The launcher checks the persistent databases, writes a private loopback-only config, starts ACE.Server, waits for an atomic ready signal and the UDP listener, and launches `acclient.exe`.
4. Use the normal character-selection screen. ACE automatically creates the persistent local account on its first successful login.

Repeated Play clicks do not create duplicate processes. By default, closing the game gracefully stops the launcher-owned server. The Stop button requests ACE's `stop-now` console command and forces termination only if that verified child does not exit within 20 seconds.

Characters live in `ace_shard`, not in the launcher. Back up the MariaDB data, and do not delete or replace the shard database when upgrading the launcher. Settings live at `%LOCALAPPDATA%\ACESinglePlayer\settings.json`; secrets there are DPAPI-protected. The generated Runtime `Config.js` necessarily contains the database password for ACE.Server and is restricted to the current Windows user.

Use **Open Logs** for startup errors. The launcher never writes account or database passwords to its log. The real client and a live ACE world database are required for an end-to-end character-screen test.
