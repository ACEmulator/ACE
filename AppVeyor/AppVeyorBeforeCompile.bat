@echo off
echo APPVEYOR_REPO_BRANCH              is: %APPVEYOR_REPO_BRANCH%
echo APPVEYOR_REPO_COMMIT              is: %APPVEYOR_REPO_COMMIT%
echo APPVEYOR_PULL_REQUEST_HEAD_COMMIT is: %APPVEYOR_PULL_REQUEST_HEAD_COMMIT%
echo APPVEYOR_BUILD_NUMBER             is: %APPVEYOR_BUILD_NUMBER%
echo APPVEYOR_BUILD_VERSION            is: %APPVEYOR_BUILD_VERSION%

REM for /f "tokens=1,2,3 delims=." %%a in ("%APPVEYOR_BUILD_VERSION%") do set TRUE_VERSION=%%a.%%b & set TRUE_BUILD=%%c

echo BUILD_DATETIME                    is: %BUILD_DATETIME%
echo TRUE_VERSION                      is: %TRUE_VERSION%
echo TRUE_BUILD                        is: %TRUE_BUILD%
echo COMMIT_ID                         is: %COMMIT_ID%
echo REVISED_VERSION                   is: %REVISED_VERSION%

REM appveyor UpdateBuild -Version "%REVISED_VERSION%"

REM set APPVEYOR_BUILD_VERSION=%REVISED_VERSION%
REM echo APPVEYOR_BUILD_VERSION            is: %APPVEYOR_BUILD_VERSION%

@echo on
nuget restore Source\ACE.sln
IF NOT EXIST Source\ACE.Server\Config.js copy AppVeyor\Config.js Source\ACE.Server\Config.js

@echo off
echo.
echo Updating ServerBuildInfo_Dynamic.cs with build details...
echo.
IF EXIST Source\ACE.Server\ServerBuildInfo_Dynamic.cs DEL Source\ACE.Server\ServerBuildInfo_Dynamic.cs

echo. >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo namespace ACE.Server >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo { >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo     public static partial class ServerBuildInfo >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo     { >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static string Branch = "%APPVEYOR_REPO_BRANCH%"; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static string Commit = "%APPVEYOR_REPO_COMMIT%"; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo. >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static string Version = "%TRUE_VERSION%"; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static string Build   = "%TRUE_BUILD%"; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo. >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildYear   = %BUILD_DATETIME:~0,4%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildMonth  = %BUILD_DATETIME:~4,2%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildDay    = %BUILD_DATETIME:~6,2%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildHour   = %BUILD_DATETIME:~8,2%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildMinute = %BUILD_DATETIME:~10,2%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo         public static int BuildSecond = %BUILD_DATETIME:~12,2%; >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo     } >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo } >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs
echo. >> Source\ACE.Server\ServerBuildInfo_Dynamic.cs

@echo on
