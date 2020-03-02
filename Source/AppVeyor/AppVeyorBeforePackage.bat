@echo on
dotnet publish "C:\projects\ace\source\ACE.Server\ACE.Server.csproj" --output C:\projects\ace\publish --configuration Release --verbosity minimal
7z.exe a ACE.Server-v%APPVEYOR_BUILD_VERSION%.zip
