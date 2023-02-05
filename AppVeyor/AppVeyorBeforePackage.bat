@echo on
dotnet publish "C:\projects\ace\Source\ACE.Server\ACE.Server.csproj" --output C:\projects\ace\publish --configuration Release --verbosity minimal

7z.exe a AppVeyor\ACE.Server-v%APPVEYOR_BUILD_VERSION%.zip "C:\projects\ace\publish\*"

REM docker build -t acemulator/ace:latest -t acemulator/ace:%APPVEYOR_BUILD_VERSION% .
