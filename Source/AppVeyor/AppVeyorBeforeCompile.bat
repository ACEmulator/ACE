@echo on
nuget restore source\ACE.sln
copy Source\AppVeyor\Config.json Source\ACE.Server\Config.json