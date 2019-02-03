@cd %~dp0
@if not exist "ACE.Server.dll" (if exist ".\bin\x64\Debug\netcoreapp2.1\ACE.Server.dll" (@cd ".\bin\x64\Debug\netcoreapp2.1\"))
dotnet ACE.Server.dll
