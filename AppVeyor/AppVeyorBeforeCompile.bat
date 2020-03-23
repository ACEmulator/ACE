@echo off
echo APPVEYOR_REPO_BRANCH   is: %APPVEYOR_REPO_BRANCH%
echo APPVEYOR_REPO_COMMIT   is: %APPVEYOR_REPO_COMMIT%
echo APPVEYOR_BUILD_NUMBER  is: %APPVEYOR_BUILD_NUMBER%
echo APPVEYOR_BUILD_VERSION is: %APPVEYOR_BUILD_VERSION%

REM set VER=%APPVEYOR_BUILD_VERSION:~0,3%
set COMMIT_ID=%APPVEYOR_REPO_COMMIT:~0,7%
REM set NEWVER=%VER%.%APPVEYOR_REPO_BRANCH%-%COMMIT_ID%.%APPVEYOR_BUILD_NUMBER%
set NEWVER=%APPVEYOR_BUILD_VERSION%.%APPVEYOR_REPO_BRANCH%-%COMMIT_ID%.%APPVEYOR_BUILD_NUMBER%

REM echo VER       is: %VER%
echo COMMIT_ID is: %COMMIT_ID%
echo NEWVER    is: %NEWVER%

appveyor UpdateBuild -Version "%NEWVER%"

@echo on
nuget restore Source\ACE.sln
copy AppVeyor\Config.js Source\ACE.Server\Config.js

@echo off
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

IF EXIST Source\ACE.Server\VersionConstant.cs DEL Source\ACE.Server\VersionConstant.cs

echo. >> Source\ACE.Server\VersionConstant.cs
echo using System; >> Source\ACE.Server\VersionConstant.cs
echo using System.Collections.Generic; >> Source\ACE.Server\VersionConstant.cs
echo using System.Text; >> Source\ACE.Server\VersionConstant.cs
echo. >> Source\ACE.Server\VersionConstant.cs
echo namespace ACE.Server >> Source\ACE.Server\VersionConstant.cs
echo { >> Source\ACE.Server\VersionConstant.cs
echo     public static partial class VersionConstant >> Source\ACE.Server\VersionConstant.cs
echo     { >> Source\ACE.Server\VersionConstant.cs
echo         public static DateTime CompilationTimestampUtc >> Source\ACE.Server\VersionConstant.cs
echo         { >> Source\ACE.Server\VersionConstant.cs
echo             get >> Source\ACE.Server\VersionConstant.cs
echo             { >> Source\ACE.Server\VersionConstant.cs
echo                 return new DateTime(%YYYY%, %MM%, %DD%, %HH%, %Min%, %Sec%, DateTimeKind.Utc); >> Source\ACE.Server\VersionConstant.cs
echo             } >> Source\ACE.Server\VersionConstant.cs
echo         } >> Source\ACE.Server\VersionConstant.cs
echo     } >> Source\ACE.Server\VersionConstant.cs
echo } >> Source\ACE.Server\VersionConstant.cs
echo. >> Source\ACE.Server\VersionConstant.cs
@echo on
