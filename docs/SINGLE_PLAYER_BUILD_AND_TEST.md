# ACE Single Player build and test

Building requires the .NET 10 SDK on Windows. The packaging script publishes the launcher and server self-contained for Windows x64 and the optional Decal helper self-contained for Windows x86. Users of the finished package do not need a separate .NET installation.

Run these commands from the repository root:

```powershell
dotnet build .\Source\ACE.SinglePlayer.slnx -c Release
dotnet test .\Source\ACE.SinglePlayer.Tests\ACE.SinglePlayer.Tests.csproj -c Release
dotnet test .\Source\ACE.Server.Tests\ACE.Server.Tests.csproj -c Release --filter "FullyQualifiedName~SinglePlayerStartupTests"
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\publish-singleplayer.ps1 -Configuration Release
```

The normal launcher test run does not require MariaDB. To additionally exercise real private initialization, isolated-user provisioning, schema bootstrap, validation, and shutdown against an installed MariaDB distribution:

```powershell
$env:ACE_TEST_MARIADBD = 'C:\Program Files\MariaDB 11.2\bin\mariadbd.exe'
dotnet test .\Source\ACE.SinglePlayer.Tests\ACE.SinglePlayer.Tests.csproj -c Release --filter 'FullyQualifiedName~PrivateDatabaseTests'
```

The integration test uses a unique temporary data directory and does not modify the Windows MariaDB service. Set `ACE_TEST_WORLD_SQL` to an extracted official `ACE-World-Database-*.sql` file to exercise the full populated-world import and empty-world repair path; otherwise the test uses a minimal populated fixture.

The clean package is written to `artifacts\ACE-SinglePlayer`, with a shareable `artifacts\ACE-SinglePlayer.zip` archive beside it. The packaging script stages publish output under the current user's temporary directory to avoid a .NET 10 publish-transform bug when the repository path contains an apostrophe. It removes debug symbols that can contain build-machine paths, then checks that the package contains no generated Config.js, settings, logs, database data, proprietary client/DAT files, MariaDB executable, or personal user-profile paths.

The normal ACE solution remains separate and can be verified with:

```powershell
dotnet build .\Source\ACE.sln -c Release
```

Upstream `ACE.DatLoader.Tests` require proprietary DAT files at their hardcoded test location. `ACE.Database.Tests` and `ACE.Server.Tests.StartupTests` require a configured live ACE database. Those environment-dependent suites are not prerequisites for the launcher logic tests and must not be reported as passing unless their external data and database are actually available.
