@cd %~dp0
@if not exist "ACE.Server.dll" (
    echo please run the copy of this file residing in the build output directory, e.g. .\bin\x64\<Configuration>\net8.0\
    pause
    exit /b
)
dotnet ACE.Server.dll
