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
