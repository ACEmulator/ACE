# ACEmulator Core Server

Build status: [![Windows CI](https://ci.appveyor.com/api/projects/status/qyueypl7cb9xq5am/branch/master?svg=true)](https://ci.appveyor.com/project/ACEmulator/ace/branch/master)

**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
**The ACEmu project is for learning purposes and we only make use of the game client for interoperability with the server.**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * Currently intended for developers that wish to contribute to the ACEmulator project.

***
## Recommended Tools
* SQLYog [on Github](https://github.com/webyog/sqlyog-community/wiki/Downloads)
* Hex Editor (Hexplorer or 010 Editor are both good)
* ACLogView [on Github](https://github.com/ACEmulator/aclogview)
* StyleCop Visual Studio Extension [on visualstudio.com](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)

## Getting Started

* For a more detailed installation process, please see [this excellent write up](https://shinobyte.gitbooks.io/shinobyte-knowledge-repository/content/acemu/acemu-server-installation.html) by "Immortus"
* Install MySQL (MariaDB is preferred, but either will work)
  [MySQL minimum required version 5.7.17+](https://dev.mysql.com/downloads/windows/installer/5.7.html)
  [MariaDB minimum required version 10.2+](https://mariadb.org/download/)
* Create three databases named `ace_auth`, `ace_shard`, and `ace_world`.
* Load AuthenticationBase.sql, ShardBase.sql, and WorldBase.sql for their respective databases. 
* Load all incremental SQL updates found in the Database\Updates\Authentication sub directory in the order of oldest to newest.
* Load all incremental SQL updates found in the Database\Updates\Shard sub directory in the order of oldest to newest.
* Download from [ACE-World](https://github.com/ACEmulator/ACE-World) the [latest release](https://github.com/ACEmulator/ACE-World/releases/latest) of world data, extract and load into your ace_world database.
* Load all incremental SQL updates found in the Database\Updates\World sub directory in the order of oldest to newest.
* Copy `Config.json.example` to `Config.json` and modify database settings, such as your database password.
* Build and run ACE.
* Create your first account as an admin at the prompt - `accountcreate testaccount testpassword 5`
* Launch AC - `acclient.exe -a testaccount -h 127.0.0.1:9000 -glsticketdirect testpassword`

## Contributions

* Contributions in the form of issues and pull requests are welcomed and encouraged.
* The preferred way to contribute is to fork the repo and submit a pull request on GitHub.
* Code style information can be found on the [Wiki](https://github.com/ACEmulator/ACE/wiki/Code-Style).

Please note that this project is released with a [Contributor Code of Conduct](https://github.com/ACEmulator/ACE/blob/master/CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## Bug Reports

* Please use the [issue tracker](https://github.com/ACEmulator/ACE/issues) provided by GitHub to send us bug reports.
* You may also submit bug reports to the [ACEmu Forums](http://acemulator.org/forums).

## Contact

- [Discord Channel](https://discord.gg/mVtGhSv) (best option)
- [ACEmulator Forums](http://acemulator.org/forums)

## FAQ

#### 1. StyleCop.MSBuild.targets not found
* _Problem_
> When opening the solution, you get a "The imported project "{project path}\ACE\Source\packages\StyleCop.MSBuild.5.0.0-beta01\build\StyleCop.MSBuild.targets" was not found. Confirm that the path in the <Import> declaration is correct, and that the file exists on disk" error.
* _Solution_
> Right click "Solution 'ACE'" in the Solution Explorer and select "Restore Nuget Packages".  After it restores, right click "ACE (load failed)" and select "Reload Project."

#### 2. My PR failed because AppVeyor timed out - "Build execution time has reached the maximum allowed time for your plan (60 minutes)."
* _Problem_
>When you submit a PR, we have automation in place that automatically kicks off a build in AppVeyor.  These builds sometimes time out.  The most common cause is because a Debug.Assert statement was hit that popped up a UI dialog on AppVeyor.  However, because it's just running a command line tool, there's no way to click the popup.  Even worse, there's now way for you to even see what it says.
* _Solution_: 
> 1) Right click your solution in Visual Studio, select "Rebuild Solution" and make sure there are no compliation errors.
> 2) Installed with Visual Studio 2015 is "Developer Command Prompt for VS2015".  Open it up, and change to your "ACE\Source" directory.
> 3) Run the following command  and you'll be able to see the popup triggering the build failure.  
   `vstest.console /inIsolation "ACE.Tests\bin\x64\Debug\ACE.Tests.dll" /Platform:x64`



## Other Resources
* [Skunkworks Protocol documentation](http://skunkworks.sourceforge.net/protocol/Protocol.php)
* [Virindi Protocol XML documentation](http://www.virindi.net/junk/messages_annotated_final.xml)
* [Miach's PCAP Repository](http://aka-steve.com/AC/AC-Files/AC1%20PCAPS/All%20PCAPS/)
* [Mag-nus Logs Repository](http://aka-steve.com/AC/AC-Files/AC1%20PCAPS/All%20Mag-nus%20Logs/)
