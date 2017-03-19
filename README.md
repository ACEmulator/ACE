# ACEmulator Core Server
**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * Currently intended for developers that wish to contribute to the ACEmulator project.

***

## Getting Started

* Install MySQL.(all database steps can be skipped by using the [ace-db](https://hub.docker.com/r/maxc0c0s/ace-db-image/) docker image.)
* Create three databases named `ace_auth`, `ace_character`, and `ace_world`.
* Load AuthenticationBase.sql and CharacterBase.sql for their respective databases. 
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

- [ACEmulator Forums](http://acemulator.org/forums)
- [Discord Channel](https://discord.gg/mVtGhSv)
