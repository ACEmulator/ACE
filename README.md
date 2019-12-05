# ACEmulator Core Server

[![Discord](https://img.shields.io/discord/261242462972936192.svg?label=play+now!&style=for-the-badge&logo=discord)](https://discord.gg/C2WzhP9)

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
## Recommended Tools
* SQLYog [on Github](https://github.com/webyog/sqlyog-community/wiki/Downloads)
* Hex Editor (Hexplorer or 010 Editor are both good)
* ACLogView [on Github](https://github.com/ACEmulator/aclogview)
* StyleCop Visual Studio Extension [on visualstudio.com](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)

## Getting Started
The following three sections (Database, Code, and Starting the Server) contain all the required steps to setup your own ACE server and connect to it. Most setup errors can be traced back to not following one or more of these steps. Be sure to follow them carefully.

### Database
1. Install MySQL or MariaDB
   * [MySQL minimum required version - 5.7.17+](https://dev.mysql.com/downloads/windows/installer/)
   * [MariaDB minimum required version - 10.2+](https://mariadb.org/download/)
   * Optionally install SQLYog editor [on Github](https://github.com/webyog/sqlyog-community/wiki/Downloads) for the following steps
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
1. Install Visual Studio 2017
   * [Visual Studio minimum required version - VS Community 2017 15.7.0](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community&rel=15)
   * [.NET Core 2.2 x64 SDK (Visual Studio 2017) Required](https://www.microsoft.com/net/download/visual-studio-sdks)
   * If using Visual Studio Community Edition, make sure the following two workloads are installed: .NET Core cross-platform development and .NET Desktop Development
2. Copy `ACE.Server\Config.js.example` to `ACE.Server\Config.js` and modify settings, such as passwords, database connections, file paths, and other server settings.
3. Open ACE.sln with Visual Studio and build the solution. Your modified `Config.js` file will be copied to the output folder during the build process.
4. Download and install [Microsoft .NET Core Runtime - 2.2](https://www.microsoft.com/net/download) if you don't already have it.

### Starting the Server
1. Start the server by running the batch file located in the netcoreapp2.2 output directory: `start_server.bat`
   * ex. ACE\Source\ACE.Server\bin\x64\Debug\netcoreapp2.2\start_server.bat
2. Create your first account as an admin at the ACE prompt - `accountcreate testaccount testpassword 5`
3. Launch ACClient directly with this command: `acclient.exe -a testaccount -v testpassword -h 127.0.0.1:9000`



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

## FAQ

#### 1. StyleCop.MSBuild.targets not found
* _Problem_
> When opening the solution, you get a "The imported project "{project path}\ACE\Source\packages\StyleCop.MSBuild.5.0.0\build\StyleCop.MSBuild.targets" was not found. Confirm that the path in the <Import> declaration is correct, and that the file exists on disk" error.
* _Solution_
> Right click "Solution 'ACE'" in the Solution Explorer and select "Restore NuGet Packages".  After it restores, right click "ACE (load failed)" and select "Reload Project."

#### 2. My PR failed because AppVeyor timed out - "Build execution time has reached the maximum allowed time for your plan (60 minutes)."
* _Problem_
>When you submit a PR, we have automation in place that automatically kicks off a build in AppVeyor.  These builds sometimes time out.  The most common cause is because a Debug.Assert statement was hit that popped up a UI dialog on AppVeyor.  However, because it's just running a command line tool, there's no way to click the pop-up.  Even worse, there's no way for you to even see what it says.
* _Solution_
> 1) Right click your solution in Visual Studio, select "Rebuild Solution" and make sure there are no compilation errors.
> 2) Installed with Visual Studio 2015 is "Developer Command Prompt for VS2015".  Open it up, and change to your "ACE\Source" directory.
> 3) Run the following command and you'll be able to see the pop-up triggering the build failure.  
   `vstest.console /inIsolation "ACE.Tests\bin\x64\Debug\ACE.Tests.dll" /Platform:x64`

#### 3. Startup projects are not set / working
* _Problem_
> When you first load the solution and try to "run" the server, you may get a pop-up that says "A project with Output Type of Class Library cannot be started directly."
* _Solution_
> 1) Right click the Solution in Visual Studio ("Solution 'ACE' (8 projects)"), and select "Set StartUp Projects".
> 2) Click on the circle next to Single startup project and in the drop-down select: ACE.Server.

## Other Resources
* [ACEmulator Protocol documentation](https://acemulator.github.io/protocol/) (Recommended)
* [Skunkworks Protocol documentation](http://skunkworks.sourceforge.net/protocol/Protocol.php) (outdated)
* [Virindi Protocol XML documentation](http://www.virindi.net/junk/messages_annotated_final.xml)
