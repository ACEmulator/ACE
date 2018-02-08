@echo on
copy Source\AppVeyor\Config.json Source\ACE.Server\Config.json
nuget restore source\ACE.sln
source\AppVeyor\MySqlInstall.bat
