@echo on
nuget restore source\ACE.sln
copy Source\AppVeyor\Config.js Source\ACE.Server\Config.js
