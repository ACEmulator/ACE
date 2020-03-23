@echo on
dotnet publish "C:\projects\ace\Source\ACE.Server\ACE.Server.csproj" --output C:\projects\ace\publish --configuration Release --verbosity minimal

7z.exe a AppVeyor\ACE.Server-v%APPVEYOR_BUILD_VERSION%.zip "C:\projects\ace\publish\*"

REM docker build -t acemulator/ace:latest -t acemulator/ace:%APPVEYOR_BUILD_VERSION% .

@echo off
echo APPVEYOR_REPO_BRANCH   is: %APPVEYOR_REPO_BRANCH%
echo APPVEYOR_REPO_COMMIT   is: %APPVEYOR_REPO_COMMIT%
echo APPVEYOR_BUILD_NUMBER  is: %APPVEYOR_BUILD_NUMBER%
echo APPVEYOR_BUILD_VERSION is: %APPVEYOR_BUILD_VERSION%
