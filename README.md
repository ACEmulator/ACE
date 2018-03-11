# ACEmulator Core Server

Build status: [![Windows CI](https://ci.appveyor.com/api/projects/status/rqebda31cgu8u59w/branch/master?svg=true)](https://ci.appveyor.com/project/LtRipley36706/ace)

**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * Currently intended for developers that wish to contribute to the ACEmulator project.

***
## Disclaimer
**This project is for educational and non-commerical purposes only, use of the game client is for interoperability with the emulated server.**
- Asheron's Call was a registered trademark of Turbine, Inc. and WB Games Inc which has since expired.
- ACEmulator is not associated or affiliated in any way with Turbine, Inc. or WB Games Inc.
***
## Recommended Tools
* SQLYog [on Github](https://github.com/webyog/sqlyog-community/wiki/Downloads)
* Hex Editor (Hexplorer or 010 Editor are both good)
* ACLogView [on Github](https://github.com/ACEmulator/aclogview)
* StyleCop Visual Studio Extension [on visualstudio.com](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)

## Getting Started

**For a more detailed installation process, please see [this excellent write up](https://shinobyte.gitbooks.io/shinobyte-knowledge-repository/content/acemu/acemu-server-installation.html) by "Immortus"**
* Install Visual Studio 2017
  - [Visual Studio minimum required version - VS Community 2017 15.6.0](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community&rel=15)
* Install MySQL
  - [MySQL minimum required version - 5.7.17+](https://dev.mysql.com/downloads/windows/installer/5.7.html)
  - [MariaDB minimum required version - 10.2+](https://mariadb.org/download/)
* Create two databases named `ace_auth`, `ace_shard`.
* Load AuthenticationBase.sql and ShardBase.sql for their respective databases. 
* Load all incremental SQL updates found in the Database\Updates\Authentication sub directory in the order of oldest to newest.
* Load all incremental SQL updates found in the Database\Updates\Shard sub directory in the order of oldest to newest.
* Create a final database named `ace_world`.
* Load WorldBase.sql to initialize the ace_world database. 
* Download from [ACE-World-16PY](https://github.com/ACEmulator/ACE-World-16PY) the [latest release](https://github.com/ACEmulator/ACE-World-16PY/releases/latest) of world data, extract and load into your ace_world database.
  - [ACE-World-16PY minimum required version - 0.0.6+](https://github.com/ACEmulator/ACE-World-16PY/releases/latest)
* Load all incremental SQL updates found in the Database\Updates\World sub directory in the order of oldest to newest.
* Copy `ACE\Config.json.example` to `Config.json` and modify settings, such as passwords and other server settings.
* Build and run ACE.
* Create your first account as an admin at the ACE prompt - `accountcreate testaccount testpassword 5`
* Launch ACClient directly with this command: `acclient.exe -a testaccount -v testpassword -h 127.0.0.1:9000`

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
> Right click "Solution 'ACE'" in the Solution Explorer and select "Restore Nuget Packages".  After it restores, right click "ACE (load failed)" and select "Reload Project."

#### 2. My PR failed because AppVeyor timed out - "Build execution time has reached the maximum allowed time for your plan (60 minutes)."
* _Problem_
>When you submit a PR, we have automation in place that automatically kicks off a build in AppVeyor.  These builds sometimes time out.  The most common cause is because a Debug.Assert statement was hit that popped up a UI dialog on AppVeyor.  However, because it's just running a command line tool, there's no way to click the popup.  Even worse, there's now way for you to even see what it says.
* _Solution_
> 1) Right click your solution in Visual Studio, select "Rebuild Solution" and make sure there are no compliation errors.
> 2) Installed with Visual Studio 2015 is "Developer Command Prompt for VS2015".  Open it up, and change to your "ACE\Source" directory.
> 3) Run the following command  and you'll be able to see the popup triggering the build failure.  
   `vstest.console /inIsolation "ACE.Tests\bin\x64\Debug\ACE.Tests.dll" /Platform:x64`

#### 3. Startup projects are not set / working
* _Problem_
> When you first load the solution and try to "run" the server, you may get a popup that says "A project with Output Type of Class Library cannot be started directly."
* _Solution_
> 1) Right click the Solution in Visual Studio ("Solution 'ACE' (8 projects)"), and select "Set StartUp Projects".
> 2) Click on the circle next to Single startup project and in the dropdown select: ACE.Server.

## Other Resources
* [ACEmulator Protocol documentation](https://acemulator.github.io/protocol/) (Recommended)
* [Skunkworks Protocol documentation](http://skunkworks.sourceforge.net/protocol/Protocol.php) (outdated)
* [Virindi Protocol XML documentation](http://www.virindi.net/junk/messages_annotated_final.xml)
