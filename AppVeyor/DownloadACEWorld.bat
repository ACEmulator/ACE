@echo off
REM Download latest ACE-World database, extract and import it

REM echo %downloadfile%
REM echo %zipfile%
REM echo %sqlfile%

set /p dbversion=<AppVeyor\dbversion.txt

IF EXIST AppVeyor\db-pr-override.txt (
    set /p downloadfile=<AppVeyor\db-pr-override.txt
) ELSE (
    set downloadfile=https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/download/v%dbversion%/ACE-World-Database-v%dbversion%.sql.zip
)

FOR /f "delims=" %%i in ("%downloadfile%") DO (
    set zipfile=%%~nxi
)

set sqlfile=%zipfile:~0,-4%

REM echo %downloadfile%
REM echo %zipfile%
REM echo %sqlfile%

appveyor DownloadFile %downloadfile%
7z x %zipfile%
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < %sqlfile%

del %zipfile%
del %sqlfile%

@echo on
