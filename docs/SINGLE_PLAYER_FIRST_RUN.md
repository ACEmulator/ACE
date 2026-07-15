# ACE Single Player: first run

ACE Single Player keeps ACE.Server as a private local process and launches the original client directly. It does not include or download Asheron's Call, DAT files, MariaDB, Decal, ThwargLauncher, or community mods.

## Before opening the launcher

1. Put the package in a folder your Windows user can write to, such as `C:\Games\ACE-SinglePlayer`. Do not install it under `Program Files` unless you deliberately choose a different writable Runtime directory. The package is self-contained and does not require a separate .NET installation.
2. Have an existing `acclient.exe` installation. Its directory must contain:
   - `client_cell_1.dat`
   - `client_portal.dat`
   - `client_local_English.dat`
   - `client_highres.dat` is optional for ACE.Server, although the client installation normally includes it.
3. Have MariaDB for Windows installed. Its Windows service may be running or stopped; the recommended private mode uses the installed programs to create a separate database and does not need the service's `root` password. The launcher does not bundle or download MariaDB.
4. Download the current official ACE-World release package and extract its populated `ACE-World-Database-*.sql` file. Do not select the repository's `Database\Base\WorldBase.sql`; that file defines empty tables and cannot run a world. The launcher never downloads or trusts a database package on its own.

## Setup wizard

Double-click `ACE.SinglePlayer.exe`.

1. Select `acclient.exe`. The wizard fills the DAT directory automatically when all three required DAT files are beside it.
2. Select the packaged `Server\ACE.Server.exe`.
3. Keep the package `Mods` directory and the default local Windows app-data `Runtime` directory unless you intentionally store them elsewhere. Runtime configuration contains short-lived local credentials and should not be placed in OneDrive.
4. Keep **Automatic private database (recommended)**. The wizard detects `mariadbd.exe`, selects a free loopback-only port, generates protected credentials, and stores MariaDB data under local Windows app data rather than OneDrive. No MariaDB username or password is requested.
5. Select the extracted, populated `ACE-World-Database-*.sql` file. Click **Prepare Private Database** if you want to prepare it immediately, or continue; **Save Setup** performs the same preparation automatically. The first world import can take several minutes. Existing complete databases are never overwritten. Private databases left with empty world tables by an interrupted or older setup are repaired from the selected package; external databases are never repaired automatically.
6. Keep the account name `singleplayer` or choose one permanent name. A strong password is generated once, protected with Windows DPAPI, and reused. Do not change or delete this account if you want the same characters.
7. Keep local port `9000` unless it conflicts with another application.
8. Save setup.
9. On the main launcher, check **Use Decal** beside **PLAY** when Decal, ThwargLauncher, `Inject.dll`, and `injector.dll` are detected. Leave it unchecked for Vanilla. **PLAY** starts the entire session directly from ACE Single Player.

The private database lives at `%LOCALAPPDATA%\ACESinglePlayer\Database`. The launcher uses the `mariadb-install-db.exe` shipped beside the detected MariaDB program, initializes into a staging directory, and moves it into place only after initialization succeeds. It creates a dedicated `ace_singleplayer` database user; generated administrator and application passwords are protected with Windows DPAPI and are not displayed or logged. The existing MariaDB service and its `root` account are not changed.

To use a separately administered server instead, choose **Existing MariaDB/MySQL (advanced)** and enter its host, port, username, password, and database names. Use **Test Connection** and **Initialize Missing Databases** as needed.

## Normal play

1. Double-click `ACE.SinglePlayer.exe`.
2. Click **PLAY** once.
3. The launcher starts its private database when selected, checks the persistent schemas, writes a private loopback-only config, starts ACE.Server, waits for an atomic ready signal and the UDP listener, and launches `acclient.exe`.
4. Use the normal character-selection screen. ACE automatically creates the persistent local account on its first successful login.

Repeated Play clicks do not create duplicate processes. By default, closing the game gracefully stops the launcher-owned server. The Stop button requests ACE's `stop-now` console command and forces termination only if that verified child does not exit within 20 seconds.

Characters live in `%LOCALAPPDATA%\ACESinglePlayer\Database\ace_shard` when private mode is selected, not in the launcher executable. Preserve the entire `%LOCALAPPDATA%\ACESinglePlayer` directory across upgrades and back it up before replacing or resetting anything. Settings live at `%LOCALAPPDATA%\ACESinglePlayer\settings.json`; secrets there are DPAPI-protected. The generated Runtime `Config.js` necessarily contains the database password for ACE.Server and is restricted to the current Windows user.

Use **Open Logs** for startup errors. The launcher never writes account or database passwords to its log. The real client and a live ACE world database are required for an end-to-end character-screen test.
