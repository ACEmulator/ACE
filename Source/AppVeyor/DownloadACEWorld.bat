REM @echo off
rem IF EXIST Source\AppVeyor\db-pr-override.txt (
rem set /p dbversion=<Source\AppVeyor\db-pr-override.txt
rem FOR /f "delims=" %%i in ("%dbversion%") do ( 
rem set dbfullfilename=%%~nxi
rem )
REM Download latest ACE-World database, extract and import it 
rem appveyor DownloadFile %dbversion%
rem 7z x %dbfullfilename%
rem "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < %dbfullfilename:~0,-4%
rem ) ELSE (
rem set /p dbversion=<Source\AppVeyor\dbversion.txt
REM Download latest ACE-World database, extract and import it 
rem appveyor DownloadFile https://github.com/ACEmulator/ACE-World-16PY/releases/download/v%dbversion%/ACE-World-16PY-db-v%dbversion%.sql.zip
rem 7z x ACE-World-16PY-db-v%dbversion%.sql.zip
rem "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-16PY-db-v%dbversion%.sql
rem )
REM echo %downloadfile%
REM echo %zipfile%
REM echo %sqlfile%
rem FOR /f "delims=" %%i in ("%dbversion%") do ( 
rem set dbfullfilename=%%~nxi
rem )
rem echo %dbfullfilename%
REM @echo on

rem @echo off

echo %downloadfile%
echo %zipfile%
echo %sqlfile%

set /p dbversion=<Source\AppVeyor\dbversion.txt

IF EXIST Source\AppVeyor\db-pr-override.txt (
    set /p downloadfile=<Source\AppVeyor\db-pr-override.txt
) ELSE (
    set /p downloadfile=https://github.com/ACEmulator/ACE-World-16PY/releases/download/v%dbversion%/ACE-World-16PY-db-v%dbversion%.sql.zip
)

FOR /f "delims=" %%i in ("%downloadfile%") DO ( 
    set zipfile=%%~nxi
)

set sqlfile=%zipfile:~0,-4%

echo %downloadfile%
echo %zipfile%
echo %sqlfile%

appveyor %downloadfile%
7z x %zipfile%
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < %sqlfile%

rem @echo on
