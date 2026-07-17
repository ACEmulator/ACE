# Installing OpenDereth

OpenDereth runs the ACE server, a private MariaDB database, and the original Asheron's Call client together on one Windows computer. The portable release includes ACE.Server, MariaDB, the complete ACE World database, and .NET. It does not include the proprietary game client or DAT files, Decal, or ThwargLauncher.

## What you need

- A 64-bit Windows 10 or Windows 11 computer.
- A working Asheron's Call client. A typical installation is `C:\Turbine\Asheron's Call`.
- These files together in the client directory:
  - `acclient.exe`
  - `client_cell_1.dat`
  - `client_portal.dat`
  - `client_local_English.dat`
  - `client_highres.dat`
- Keep the complete client installation in one writable folder outside OneDrive and `Program Files`, such as `C:\Games\AsheronsCall` or `C:\Turbine\Asheron's Call`.
- Optional for Decal mode: working Decal and ThwargLauncher installations. OpenDereth uses ThwargLauncher's installed `injector.dll` but does not copy or redistribute it.

## Download and extract

1. Open this project's **Releases** page and download the latest `OpenDereth-*.zip` file. Do not download GitHub's automatically generated **Source code** archives unless you intend to build the program yourself.
2. Create a writable folder outside OneDrive and `Program Files`, such as `C:\Games\OpenDereth`.
3. Extract the entire ZIP into that folder. `ACE.SinglePlayer.exe`, `Server`, and `Dependencies` must remain together.

The release is self-contained. If Windows says that .NET must be installed, the source archive or an incomplete build was downloaded instead of the release ZIP.

## First-time setup

1. Double-click `ACE.SinglePlayer.exe`.
2. If prompted, select the folder containing `acclient.exe` and all four DAT files. Standard `C:\Turbine\Asheron's Call` installations are detected automatically.
3. On the main launcher, check **Use Decal** beside **PLAY** when both Decal and ThwargLauncher are installed. Leave it unchecked for Vanilla.
4. Click **PLAY** once. The launcher generates protected credentials, initializes its bundled private MariaDB, imports the bundled world, starts ACE.Server, waits for logins to open, and launches the client. The initial import can take several minutes and is performed only once.

The first successful login creates the local ACE account automatically. Closing the game normally stops the launcher-owned server and database.

## Where your files are kept

- Settings: `%LOCALAPPDATA%\OpenDereth\settings.json`
- Private database and characters: `%LOCALAPPDATA%\OpenDereth\Database`
- Private server DAT copy: `%LOCALAPPDATA%\OpenDereth\ServerData`
- Generated runtime configuration and logs: `%LOCALAPPDATA%\OpenDereth\Runtime`

Passwords are generated automatically and protected for the current Windows user. The launcher does not change the MariaDB service's `root` account.

Back up the entire `%LOCALAPPDATA%\OpenDereth` folder before reinstalling Windows, resetting the private database, or making major world changes. Never publish that folder or include it in a GitHub issue. On the first OpenDereth startup, an existing `%LOCALAPPDATA%\ACESinglePlayer` folder from earlier previews is moved to `%LOCALAPPDATA%\OpenDereth` and its saved paths are repaired automatically.

## Common problems

### Nothing opens after clicking PLAY

Use **Open Logs** in the launcher. Confirm that the entire release was extracted, the `Dependencies` folder is present, and no other program is using the configured local port.

### MariaDB says access denied for root

The portable release does not use an installed MariaDB service or its root password. Re-extract the complete release and use **Automatic private database** in advanced Settings.

### The client cannot open the data files

Confirm that all four required DAT files are beside `acclient.exe`. The client folder must be writable and outside OneDrive or `Program Files`. Current releases automatically give ACE.Server a separate copy under `%LOCALAPPDATA%\OpenDereth\ServerData`; the launcher log should show different client and server DAT directories. If an older release shows the same directory for both, install the current release.

### Decal mode is unavailable

Install and configure both Decal and ThwargLauncher normally, then reopen OpenDereth. The **Use Decal** checkbox becomes available when both installations are detected. Decal mode deliberately uses their installed files and never bundles them.

### The world starts and then immediately shuts down

Use **Open Logs** and verify that `Dependencies\World\ACE-World-Database-v0.9.294.sql` exists. Re-extract the complete release if it is missing.

## Updating

1. Stop the game and launcher.
2. Back up `%LOCALAPPDATA%\OpenDereth`.
3. Extract the new release to a new writable folder.
4. Start the new `ACE.SinglePlayer.exe`. Existing local settings and the private database are retained under `%LOCALAPPDATA%`.

Do not replace the original client DAT files with files from an untrusted package. Future managed content-pack support should use isolated copies with checksums, backups, and rollback.

## Building from source

Developers need the .NET 10 SDK and should follow [OpenDereth build and test](SINGLE_PLAYER_BUILD_AND_TEST.md). Players should use the prebuilt release ZIP instead.
