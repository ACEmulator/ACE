Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -UseBasicParsing -OutFile "$env:temp\dotnet-install.ps1"
& $env:temp\dotnet-install.ps1 -Architecture x64 -Version '10.0.100' -InstallDir "$env:ProgramFiles\dotnet"
