# ACEmulator Core Server
**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * Currently intended for developers that wish to contribute to the ACEmulator project.

***
## Recommended Tools
* SQLYog [on Github](https://github.com/webyog/sqlyog-community/wiki/Downloads)
* Hex Editor (Hexplorer or 010 Editor are both good)
* ACLogView [on Github](https://github.com/tfarley/aclogview)
* StyleCop Visual Studio Extension [on visualstudio.com](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)

## Getting Started

* Install MySQL (MariaDB is preferred, but either will work).
* Create three databases named `ace_auth`, `ace_character`, and `ace_world`.
* Load AuthenticationBase.sql, CharacterBase.sql, and WorldBase.sql for their respective databases. 
* Load all incremental SQL updates in the Database\Updates sub directories. 
* Copy `Config.json.example` to `Config.json` and modify database settings, such as your database password.
* Build and run ACE.
* Create an account at the prompt - `accountcreate testaccount testpassword`
* Launch AC - `acclient.exe -a testaccount -h 127.0.0.1:9000 -glsticketdirect testpassword`

## Contributions

* The preferred way to contribute is to fork the repo and submit a pull request on GitHub.
* Code style information can be found on the [Wiki](https://github.com/ACEmulator/ACE/wiki/Code-Style).

## Bug Reports

* Please use the [issue tracker](https://github.com/ACEmulator/ACE/issues) provided by GitHub to send us bug reports.
* You may also submit bug reports to the [ACEmu Forums](http://acemulator.org/forums).

## Contact

- [Discord Channel](https://discord.gg/mVtGhSv) (best option)
- [ACEmulator Forums](http://acemulator.org/forums)

## Other Resources
* [Skunkworks Protocol documentation](http://skunkworks.sourceforge.net/protocol/Protocol.php)
* [Virindi Protocol XML documentation](http://www.virindi.net/junk/messages_annotated_final.xml)
* [Miach's PCAP Repository](http://aka-steve.com/AC/AC-Files/AC1%20PCAPS/All%20PCAPS/)
* [Mag-nus Logs Repository](http://aka-steve.com/AC/AC-Files/AC1%20PCAPS/All%20Mag-nus%20Logs/)
* [AC Dungeon Viewer](https://github.com/floaterxk/DungeonViewerAC)
