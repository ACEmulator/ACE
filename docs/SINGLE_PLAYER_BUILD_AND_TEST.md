# OpenDereth build and test

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

The clean package is written to `%LOCALAPPDATA%\OpenDereth-Build\OpenDereth`, with a shareable `OpenDereth.zip` archive beside it. Keeping release output outside the checkout prevents cloud-sync software from locking it. `-OutputDirectory` may select another safe directory whose final name is `OpenDereth`; that directory is replaced on every build. By default, the packaging script downloads hash-pinned official MariaDB `12.3.2` and ACE World `v0.9.294` archives into `%LOCALAPPDATA%\OpenDereth-BuildCache`, verifies both SHA-256 hashes, and bundles their extracted runtime files. Use `-SkipBundledDependencies` only for a small developer package that is not suitable as the public ready-to-play release.

Publishing stages final output, compiler output, and intermediate files under the current user's temporary directory. This avoids a .NET 10 publish-transform bug when the repository path contains an apostrophe and prevents OneDrive from locking build files when a developer's source checkout is cloud-synced. It removes debug symbols that can contain build-machine paths, then rejects generated Config.js, settings, logs, private database data, proprietary client/DAT files, unpinned MariaDB servers, or personal user-profile paths. `BUNDLE-MANIFEST.json`, `THIRD_PARTY_NOTICES.md`, and the `Licenses` directory record the redistributed components.

The normal ACE solution remains separate and can be verified with:

```powershell
dotnet build .\Source\ACE.sln -c Release
```

Upstream `ACE.DatLoader.Tests` require proprietary DAT files at their hardcoded test location. `ACE.Database.Tests` and `ACE.Server.Tests.StartupTests` require a configured live ACE database. Those environment-dependent suites are not prerequisites for the launcher logic tests and must not be reported as passing unless their external data and database are actually available.
