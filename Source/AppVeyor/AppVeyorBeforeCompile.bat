@echo on
copy Source\AppVeyor\Config.json Source\ACE\Config.json
nuget restore source\ACE.sln
source\AppVeyor\MySqlInstall.bat