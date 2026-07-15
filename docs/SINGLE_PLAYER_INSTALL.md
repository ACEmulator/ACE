# Installing ACE Single Player

ACE Single Player runs the ACE server, a private MariaDB database, and the original Asheron's Call client together on one Windows computer. It does not include or download the game client, DAT files, MariaDB, Decal, ThwargLauncher, or the ACE World database.

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
- [MariaDB Community Server for Windows](https://mariadb.org/download/). The launcher uses its database programs but creates a separate private database. The password chosen for MariaDB's normal Windows service is not used by the recommended setup.
- The populated `ACE-World-Database-*.sql` file from the [official ACE World release](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest).
- Optional for Decal mode: working Decal and ThwargLauncher installations. ACE Single Player uses ThwargLauncher's installed `injector.dll` but does not copy or redistribute it.

Do not select `Database\Base\WorldBase.sql` from the ACE source repository. That file only creates empty tables and does not contain the playable world.

## Download and extract

1. Open this project's **Releases** page and download the latest `ACE-SinglePlayer-*.zip` file. Do not download GitHub's automatically generated **Source code** archives unless you intend to build the program yourself.
2. Create a writable folder outside OneDrive and `Program Files`, such as `C:\Games\ACE-SinglePlayer`.
3. Extract the entire ZIP into that folder. `ACE.SinglePlayer.exe` and the `Server` folder must remain together.

The release is self-contained. If Windows says that .NET must be installed, the source archive or an incomplete build was downloaded instead of the release ZIP.

## First-time setup

1. Double-click `ACE.SinglePlayer.exe`.
2. Select `acclient.exe`. When the DAT files are beside it, the launcher fills in the DAT location automatically.
3. Select `Server\ACE.Server.exe` if it was not detected automatically.
4. Keep the suggested Mods and Runtime locations.
5. Choose **Automatic private database (recommended)**. The launcher should detect MariaDB automatically. If it does not, select the installed `mariadbd.exe`, normally found under `C:\Program Files\MariaDB*\bin`.
6. Select the extracted, populated `ACE-World-Database-*.sql` file.
7. Keep the account name `singleplayer`, or choose one name that you will continue using for this world.
8. Save setup. Preparing and importing the world for the first time can take several minutes.
9. On the main launcher, check **Use Decal** beside **PLAY** when both Decal and ThwargLauncher are installed. Leave it unchecked for Vanilla. No second launcher window or repeated login is required.
10. Click **PLAY** once. The launcher starts the private database and server, waits until the world is ready, and then opens the game client.

The first successful login creates the local ACE account automatically. Closing the game normally stops the launcher-owned server and database.

## Where your files are kept

- Settings: `%LOCALAPPDATA%\ACESinglePlayer\settings.json`
- Private database and characters: `%LOCALAPPDATA%\ACESinglePlayer\Database`
- Generated runtime configuration and logs: `%LOCALAPPDATA%\ACESinglePlayer\Runtime`

Passwords are generated automatically and protected for the current Windows user. The launcher does not change the MariaDB service's `root` account.

Back up the entire `%LOCALAPPDATA%\ACESinglePlayer` folder before reinstalling Windows, resetting the private database, or making major world changes. Never publish that folder or include it in a GitHub issue.

## Common problems

### Nothing opens after clicking PLAY

Use **Open Logs** in the launcher. Confirm that MariaDB is installed, the populated world SQL file was selected, and no other program is using the configured local port.

### MariaDB says access denied for root

Return to setup and choose **Automatic private database**. It does not use the existing MariaDB service's root password.

### The client cannot open the data files

Confirm that all four required DAT files are beside `acclient.exe`, not merely in the separate folder selected for ACE.Server. The client folder must be writable and outside OneDrive or `Program Files`. The launcher now checks this before starting MariaDB and logs the exact executable, client working directory, and server DAT directory.

### Decal mode is unavailable

Install and configure both Decal and ThwargLauncher normally, then reopen ACE Single Player. The **Use Decal** checkbox becomes available when both installations are detected. Decal mode deliberately uses their installed files and never bundles them.

### The world starts and then immediately shuts down

Make sure the selected SQL file is the populated `ACE-World-Database-*.sql` release, not the empty `WorldBase.sql` schema.

## Updating

1. Stop the game and launcher.
2. Back up `%LOCALAPPDATA%\ACESinglePlayer`.
3. Extract the new release to a new writable folder.
4. Start the new `ACE.SinglePlayer.exe`. Existing local settings and the private database are retained under `%LOCALAPPDATA%`.

Do not replace the original client DAT files with files from an untrusted package. Future managed content-pack support should use isolated copies with checksums, backups, and rollback.

## Building from source

Developers need the .NET 10 SDK and should follow [ACE Single Player build and test](SINGLE_PLAYER_BUILD_AND_TEST.md). Players should use the prebuilt release ZIP instead.
