@cd %~dp0
@if not exist "ACE.WebApiServer.dll" (if exist ".\bin\x64\Debug\netcoreapp2.1\ACE.WebApiServer.dll" (@cd ".\bin\x64\Debug\netcoreapp2.1\"))
dotnet ACE.WebApiServer.dll
