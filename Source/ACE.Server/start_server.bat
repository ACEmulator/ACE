@cd %~dp0
@if not exist "ACE.Server.dll" (echo please run the copy of this file residing in the output folder: .\bin\x64\XXXXX\netcoreapp2.1\
pause)
dotnet ACE.Server.dll
