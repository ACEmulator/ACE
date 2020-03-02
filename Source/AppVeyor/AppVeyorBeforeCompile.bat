@echo on
nuget restore source\ACE.sln
copy Source\AppVeyor\Config.js Source\ACE.Server\Config.js

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
