@echo on
msbuild "C:\projects\ace\Source\ACE.sln" /verbosity:minimal /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:NoWarn="NETSDK1233"
AppVeyor\AppVeyorBeforePackage.bat
