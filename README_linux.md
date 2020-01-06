# ACEmulator Core Server for Linux

Note that most of these are the same instructions for installation on Windows @ https://github.com/ACEmulator/ACE, with some Linux specifics

Issue status: [![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/ACEmulator/ACE.svg)](http://isitmaintained.com/project/ACEmulator/ACE "Average time to resolve an issue")
[![Percentage of issues still open](http://isitmaintained.com/badge/open/ACEmulator/ACE.svg)](http://isitmaintained.com/project/ACEmulator/ACE "Percentage of issues still open")

Build status: [![Windows CI](https://ci.appveyor.com/api/projects/status/rqebda31cgu8u59w/branch/master?svg=true)](https://ci.appveyor.com/project/LtRipley36706/ace)

**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * Currently intended for developers that wish to contribute to the ACEmulator project.

***
## Disclaimer
**This project is for educational and non-commercial purposes only, use of the game client is for interoperability with the emulated server.**
- Asheron's Call was a registered trademark of Turbine, Inc. and WB Games Inc which has since expired.
- ACEmulator is not associated or affiliated in any way with Turbine, Inc. or WB Games Inc.
***

## Getting Started
The following three sections (Database, Code, and Starting the Server) contain all the required steps to setup your own ACE server and connect to it. Most setup errors can be traced back to not following one or more of these steps. Be sure to follow them carefully.

### Database
1. Install MySQL or MariaDB
   * [MySQL minimum required version - 5.7.17+](https://dev.mysql.com/downloads/windows/installer/5.7.html)
   * [MariaDB minimum required version - 10.2+](https://mariadb.org/download/)
2. Create three databases named `ace_auth`, `ace_shard`, and `ace_world`
3. Load AuthenticationBase.sql and ShardBase.sql for their respective databases. These can be found in the Database\Base directory.
4. Load all incremental SQL updates found in the Database\Updates\Authentication sub directory in the order of oldest to newest. Skip this step if there are no updates in this directory.
5. Load all incremental SQL updates found in the Database\Updates\Shard sub directory in the order of oldest to newest. Skip this step if there are no updates in this directory.
6. Download from [ACE-World-16PY-Patches](https://github.com/ACEmulator/ACE-World-16PY-Patches) the [latest release](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest) of world data, extract and load into your ace_world database.
   * [ACE World Database (ACE-World-16PY-Patches) minimum required version - 0.9.150+](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest)
7. SKIP THIS STEP IF USING DOWNLOADED WORLD DATA FROM PREVIOUS STEP.
   * If using a custom database, you may need to update the schema for the emulator to operate correctly. If you're using the official release data, this step is not recommended.
   * Load WorldBase.sql from Database\Base into your `ace_world` database
   * Load all incremental SQL updates found in the Database\Updates\World sub directory in the order of oldest to newest. Skip this step if there are no updates in this directory.

### Code
1. Install .NET Core SDK
   * https://dotnet.microsoft.com/download/linux-package-manager/rhel/sdk-current
2. Clone the project with git:
   * git clone https://github.com/ACEmulator/ACE.git
3. Copy `ACE.Server\Config.js.example` to `ACE.Server\Config.js` and modify settings, such as DAT folder, passwords and other server settings.
4. In `ACE/Source`, run `dotnet build`


### Starting the Server
1. In ACE/Source/ACE.Server/bin/x64/Debug/netcoreapp3.1, run the server with `dotnet ACE.Server.dll`
2. Create your first account as an admin at the ACE prompt: `accountcreate testaccount testpassword 5`
3. Launch acclient directly with this command: `acclient.exe -a testaccount -v testpassword -h your-server-address:9000`




## Contributions

* Contributions in the form of issues and pull requests are welcomed and encouraged.
* The preferred way to contribute is to fork the repo and submit a pull request on GitHub.
* Code style information can be found on the [Wiki](https://github.com/ACEmulator/ACE/wiki/Code-Style).

Please note that this project is released with a [Contributor Code of Conduct](https://github.com/ACEmulator/ACE/blob/master/CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## Bug Reports

* Please use the [issue tracker](https://github.com/ACEmulator/ACE/issues) provided by GitHub to send us bug reports.
* You may also discuss issues and bug reports on our discord listed below.

## Contact

- [Discord Channel](https://discord.gg/C2WzhP9)

## Other Resources
* [ACEmulator Protocol documentation](https://acemulator.github.io/protocol/) (Recommended)
* [Skunkworks Protocol documentation](http://skunkworks.sourceforge.net/protocol/Protocol.php) (outdated)
* [Virindi Protocol XML documentation](http://www.virindi.net/junk/messages_annotated_final.xml)
