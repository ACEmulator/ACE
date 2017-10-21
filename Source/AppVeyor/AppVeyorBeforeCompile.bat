@echo on
copy Source\AppVeyor\Config.json Source\ACE\Config.json
copy Source\AppVeyor\launcher_config.json Source\ACE.CmdLineLauncher\launcher_config.json
nuget restore source\ACE.sln
source\AppVeyor\MySqlInstall.bat