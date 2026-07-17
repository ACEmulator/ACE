param(
    [ValidateSet("Debug", "Release")]
    [string] $Configuration = "Release",
    [string] $OutputDirectory,
    [switch] $SkipBundledDependencies
)

$ErrorActionPreference = "Stop"
$repoRoot = [IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent))
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $localBuildRoot = if ([string]::IsNullOrWhiteSpace($env:LOCALAPPDATA)) {
        Join-Path ([IO.Path]::GetTempPath()) "OpenDereth-Build"
    } else {
        Join-Path $env:LOCALAPPDATA "OpenDereth-Build"
    }
    $OutputDirectory = Join-Path $localBuildRoot "OpenDereth"
}
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
$outputPrefix = $OutputDirectory.TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
if ((Split-Path $OutputDirectory -Leaf) -ine "OpenDereth" -or
    [string]::Equals($OutputDirectory, [IO.Path]::GetPathRoot($OutputDirectory), [StringComparison]::OrdinalIgnoreCase)) {
    throw "OutputDirectory must name a non-root directory called OpenDereth. The directory is replaced on every build."
}

$localDotnet = Join-Path $repoRoot ".dotnet\dotnet.exe"
$dotnet = if (Test-Path -LiteralPath $localDotnet) {
    $localDotnet
} else {
    (Get-Command dotnet -ErrorAction Stop).Source
}

# Microsoft.NET.Publish.targets uses a single-quoted item transform for PublishDir.
# Staging outside a repository path containing an apostrophe avoids MSB3094 on .NET 10.
$tempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$stage = [IO.Path]::GetFullPath((Join-Path $tempRoot "OpenDereth-Publish-$PID"))
$tempPrefix = $tempRoot.TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
if (-not $stage.StartsWith($tempPrefix, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Temporary staging directory must remain under $tempRoot"
}
$launcherPublish = Join-Path $stage "Launcher"
$serverPublish = Join-Path $stage "Server"
$decalHostPublish = Join-Path $stage "DecalHost"
$customClothingExtract = Join-Path $stage "CustomClothingBaseExtract"
$customClothingPackage = Join-Path $stage "CustomClothingBasePackage"
$mariaDbExtract = Join-Path $stage "MariaDB"
$worldExtract = Join-Path $stage "World"
$buildArtifacts = Join-Path $stage "BuildArtifacts"

$mariaDbVersion = "12.3.2"
$mariaDbArchiveName = "mariadb-$mariaDbVersion-winx64.zip"
$mariaDbUri = "https://archive.mariadb.org/mariadb-$mariaDbVersion/winx64-packages/$mariaDbArchiveName"
$mariaDbSha256 = "67347c129eb9c5923d002ea34fbfa27c60eb95d36dd73b85af2651cdeceecac5"
$worldVersion = "0.9.294"
$worldArchiveName = "ACE-World-Database-v$worldVersion.sql.zip"
$worldUri = "https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/download/v$worldVersion/$worldArchiveName"
$worldSha256 = "aa8275a2fd8edd8c2b95092d2407ece4616ba7b8d7eab1405719bbbfa80c8f89"
$customClothingVersion = "1.11"
$customClothingArchiveName = "CustomClothingBase-v$customClothingVersion.zip"
$customClothingUri = "https://github.com/OptimShi/CustomClothingBase/releases/download/v$customClothingVersion/CustomClothingBase.zip"
$customClothingSha256 = "505dcb951bdba9ec7788b2f947f3b8d6a7638e06c43000bd38beb129689873a6"
$cacheRoot = if ([string]::IsNullOrWhiteSpace($env:LOCALAPPDATA)) {
    Join-Path $tempRoot "OpenDereth-BuildCache"
} else {
    Join-Path $env:LOCALAPPDATA "OpenDereth-BuildCache"
}

function Invoke-DotNet([string[]] $Arguments) {
    & $dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') failed with exit code $LASTEXITCODE"
    }
}

function Get-VerifiedDownload([string] $Uri, [string] $Destination, [string] $ExpectedSha256) {
    if (Test-Path -LiteralPath $Destination) {
        $actual = (Get-FileHash -LiteralPath $Destination -Algorithm SHA256).Hash.ToLowerInvariant()
        if ($actual -eq $ExpectedSha256.ToLowerInvariant()) {
            Write-Host "Using verified dependency cache: $(Split-Path $Destination -Leaf)"
            return
        }
        Remove-Item -LiteralPath $Destination -Force
    }

    Write-Host "Downloading $(Split-Path $Destination -Leaf)..."
    Invoke-WebRequest -Uri $Uri -OutFile $Destination -UseBasicParsing
    $downloaded = (Get-FileHash -LiteralPath $Destination -Algorithm SHA256).Hash.ToLowerInvariant()
    if ($downloaded -ne $ExpectedSha256.ToLowerInvariant()) {
        Remove-Item -LiteralPath $Destination -Force
        throw "SHA-256 mismatch for $Uri. Expected $ExpectedSha256 but received $downloaded."
    }
}

foreach ($path in @($stage, $OutputDirectory)) {
    if (Test-Path -LiteralPath $path) {
        Remove-Item -LiteralPath $path -Recurse -Force
    }
}
New-Item -ItemType Directory -Path $launcherPublish, $serverPublish, $decalHostPublish, $buildArtifacts, $OutputDirectory | Out-Null

Write-Host "Publishing ACE.Server as self-contained Windows x64..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.Server\ACE.Server.csproj"), "-c", $Configuration, "-r", "win-x64", "--self-contained", "true", "--artifacts-path", $buildArtifacts, "-o", $serverPublish)

Write-Host "Publishing OpenDereth as self-contained Windows x64..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.SinglePlayer\ACE.SinglePlayer.csproj"), "-c", $Configuration, "-r", "win-x64", "--self-contained", "true", "--artifacts-path", $buildArtifacts, "-o", $launcherPublish)

Write-Host "Publishing the optional Decal helper as a self-contained Windows x86 executable..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.SinglePlayer.DecalHost\ACE.SinglePlayer.DecalHost.csproj"), "-c", $Configuration, "-r", "win-x86", "--self-contained", "true", "--artifacts-path", $buildArtifacts, "-p:PublishSingleFile=true", "-o", $decalHostPublish)

Copy-Item -Path (Join-Path $launcherPublish "*") -Destination $OutputDirectory -Recurse -Force
New-Item -ItemType Directory -Path (Join-Path $OutputDirectory "Server"), (Join-Path $OutputDirectory "Tools"), (Join-Path $OutputDirectory "Packages") | Out-Null
Copy-Item -Path (Join-Path $serverPublish "*") -Destination (Join-Path $OutputDirectory "Server") -Recurse -Force
Copy-Item -Path (Join-Path $decalHostPublish "OpenDereth.DecalHost.exe") -Destination (Join-Path $OutputDirectory "Tools") -Force

Write-Host "Building the curated mod packages..."
foreach ($modProject in @(
    "ACE.SinglePlayer.Mods.CriticalOverride",
    "ACE.SinglePlayer.Mods.ACEUniqueWeeniesProc",
    "ACE.SinglePlayer.Mods.HelloCommand",
    "ACE.SinglePlayer.Mods.SocietyTailoring"
)) {
    & (Join-Path $repoRoot "scripts\package-mod.ps1") `
        -ProjectDirectory (Join-Path $repoRoot "Source\$modProject") `
        -OutputDirectory (Join-Path $OutputDirectory "Packages") `
        -Configuration $Configuration
    if ($LASTEXITCODE -ne 0) {
        throw "Packaging $modProject failed with exit code $LASTEXITCODE"
    }
}

Write-Host "Packaging OptimShi's checksum-pinned official CustomClothingBase v$customClothingVersion release..."
New-Item -ItemType Directory -Force -Path $cacheRoot, $customClothingExtract, $customClothingPackage | Out-Null
$customClothingArchive = Join-Path $cacheRoot $customClothingArchiveName
Get-VerifiedDownload $customClothingUri $customClothingArchive $customClothingSha256
Expand-Archive -LiteralPath $customClothingArchive -DestinationPath $customClothingExtract -Force
$customClothingUpstreamFiles = @(
    "ACE.Shared.dll",
    "CustomClothingBase.dll",
    "JsonNet.ContractResolvers.dll",
    "Newtonsoft.Json.dll",
    "Settings.json"
)
foreach ($fileName in $customClothingUpstreamFiles) {
    if (-not (Test-Path -LiteralPath (Join-Path $customClothingExtract $fileName))) {
        throw "The official CustomClothingBase archive is missing $fileName."
    }
}
$customClothingModDirectory = Join-Path $customClothingPackage "mod"
$customClothingJsonDirectory = Join-Path $customClothingModDirectory "json"
New-Item -ItemType Directory -Force -Path $customClothingModDirectory, $customClothingJsonDirectory | Out-Null
Copy-Item -LiteralPath (Join-Path $repoRoot "packaging\CustomClothingBase\ace-mod.json") -Destination $customClothingPackage
foreach ($fileName in $customClothingUpstreamFiles) {
    Copy-Item -LiteralPath (Join-Path $customClothingExtract $fileName) -Destination $customClothingModDirectory
}
Copy-Item -LiteralPath (Join-Path $repoRoot "packaging\CustomClothingBase\Meta.json") -Destination $customClothingModDirectory
Copy-Item -LiteralPath (Join-Path $repoRoot "packaging\CustomClothingBase\README.md") -Destination $customClothingModDirectory
Copy-Item -LiteralPath (Join-Path $repoRoot "packaging\CustomClothingBase\json\README.txt") -Destination $customClothingJsonDirectory
$customClothingOutputArchive = Join-Path $OutputDirectory "Packages\optimshi.custom-clothing-base-1.11-upstream.zip"
Compress-Archive -Path (Join-Path $customClothingPackage "*") -DestinationPath $customClothingOutputArchive -CompressionLevel Optimal
$customClothingPackageSha256 = (Get-FileHash -LiteralPath $customClothingOutputArchive -Algorithm SHA256).Hash.ToLowerInvariant()
$customClothingPackageSha256 | Set-Content -LiteralPath ($customClothingOutputArchive + ".sha256") -Encoding ascii

if (-not $SkipBundledDependencies) {
    New-Item -ItemType Directory -Path $cacheRoot, $mariaDbExtract, $worldExtract -Force | Out-Null
    $mariaDbArchive = Join-Path $cacheRoot $mariaDbArchiveName
    $worldArchive = Join-Path $cacheRoot $worldArchiveName
    Get-VerifiedDownload $mariaDbUri $mariaDbArchive $mariaDbSha256
    Get-VerifiedDownload $worldUri $worldArchive $worldSha256

    Write-Host "Extracting the pinned portable MariaDB runtime..."
    Expand-Archive -LiteralPath $mariaDbArchive -DestinationPath $mariaDbExtract -Force
    $mariaDbRoots = @(Get-ChildItem -LiteralPath $mariaDbExtract -Directory | Where-Object {
        Test-Path -LiteralPath (Join-Path $_.FullName "bin\mariadbd.exe")
    })
    if ($mariaDbRoots.Count -ne 1) {
        throw "The MariaDB archive did not contain exactly one complete Windows runtime root."
    }
    $mariaDbOutput = Join-Path $OutputDirectory "Dependencies\MariaDB"
    New-Item -ItemType Directory -Path (Split-Path $mariaDbOutput -Parent) -Force | Out-Null
    Move-Item -LiteralPath $mariaDbRoots[0].FullName -Destination $mariaDbOutput

    Write-Host "Extracting the pinned complete ACE World database..."
    Expand-Archive -LiteralPath $worldArchive -DestinationPath $worldExtract -Force
    $worldSqlFiles = @(Get-ChildItem -LiteralPath $worldExtract -Recurse -File -Filter "ACE-World-Database-*.sql")
    if ($worldSqlFiles.Count -ne 1) {
        throw "The ACE World archive did not contain exactly one complete ACE-World-Database SQL file."
    }
    $worldOutput = Join-Path $OutputDirectory "Dependencies\World"
    New-Item -ItemType Directory -Path $worldOutput -Force | Out-Null
    Move-Item -LiteralPath $worldSqlFiles[0].FullName -Destination (Join-Path $worldOutput "ACE-World-Database-v$worldVersion.sql") -Force

    $licensesOutput = Join-Path $OutputDirectory "Licenses"
    New-Item -ItemType Directory -Path $licensesOutput -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $repoRoot "LICENSE") -Destination (Join-Path $licensesOutput "ACE-and-ACE-World-AGPL-3.0.txt")
    $mariaDbLicense = Get-ChildItem -LiteralPath $mariaDbOutput -File | Where-Object { $_.Name -in @("COPYING", "COPYING.txt") } | Select-Object -First 1
    if ($null -eq $mariaDbLicense) {
        throw "The MariaDB distribution license was not found in the official archive."
    }
    Copy-Item -LiteralPath $mariaDbLicense.FullName -Destination (Join-Path $licensesOutput "MariaDB-GPL-2.0.txt")
    Copy-Item -LiteralPath (Join-Path $repoRoot "docs\THIRD_PARTY_NOTICES.md") -Destination (Join-Path $OutputDirectory "THIRD_PARTY_NOTICES.md")

    $bundleManifest = [ordered]@{
        formatVersion = 1
        aceUpstream = [ordered]@{
            repository = "https://github.com/ACEmulator/ACE"
            commit = "650c5b75ae909957feaf58db320e46be16502653"
            build = "1.77.4782"
            license = "AGPL-3.0"
        }
        mariaDb = [ordered]@{
            version = $mariaDbVersion
            source = $mariaDbUri
            sha256 = $mariaDbSha256
            license = "GPL-2.0"
        }
        aceWorld = [ordered]@{
            version = $worldVersion
            source = $worldUri
            sha256 = $worldSha256
            license = "AGPL-3.0"
        }
        customClothingBase = [ordered]@{
            repository = "https://github.com/OptimShi/CustomClothingBase"
            version = $customClothingVersion
            source = $customClothingUri
            upstreamSha256 = $customClothingSha256
            packagedSha256 = $customClothingPackageSha256
            license = "No LICENSE file in upstream repository; redistributed with author permission reported by the project maintainer"
        }
    }
    $bundleManifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $OutputDirectory "BUNDLE-MANIFEST.json") -Encoding utf8
}

foreach ($directory in @("Runtime", "Mods", "Logs", "Client", "Docs")) {
    New-Item -ItemType Directory -Path (Join-Path $OutputDirectory $directory) -Force | Out-Null
}
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_FIRST_RUN.md") (Join-Path $OutputDirectory "FIRST_RUN.md")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_INSTALL.md") (Join-Path $OutputDirectory "INSTALL.md")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_ARCHITECTURE.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\AQUIFIR_MOD_COMPATIBILITY.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\MOD_LIBRARY.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\MOD_AUTHOR_GUIDE.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\CUSTOM_WEENIES.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_ROADMAP.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_BUILD_AND_TEST.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "README.md") $OutputDirectory
Copy-Item (Join-Path $repoRoot "LICENSE") $OutputDirectory

# Portable public packages do not need debug symbols. Portable PDBs can contain
# source document paths from the computer that produced the release.
Get-ChildItem -LiteralPath $OutputDirectory -Recurse -Filter "*.pdb" -File | Remove-Item -Force

$forbidden = Get-ChildItem -LiteralPath $OutputDirectory -Recurse -File | Where-Object {
    $relativePath = if ($_.FullName.StartsWith($outputPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        $_.FullName.Substring($outputPrefix.Length)
    } else {
        $_.FullName
    }
    $isPinnedMariaDb = $relativePath -ieq "Dependencies\MariaDB\bin\mariadbd.exe"
    $_.Name -in @("Config.js", "settings.json", "acclient.exe", "ace-server.ready.json", "ace-server.process.json") -or
    ($_.Name -ieq "mariadbd.exe" -and -not $isPinnedMariaDb) -or
    $_.Name -like "client_*.dat" -or $_.Extension -in @(".log", ".pdb") -or
    $relativePath -match '^(Runtime|Logs|Client)[\\/]'
}
if ($forbidden) {
    throw "Packaging safety check found forbidden runtime/proprietary files: $($forbidden.FullName -join ', ')"
}

$textExtensions = @(".bat", ".config", ".example", ".js", ".json", ".md", ".ps1", ".sh", ".sql", ".txt", ".xml", ".yml", ".yaml")
$personalPathMatches = Get-ChildItem -LiteralPath $OutputDirectory -Recurse -File | Where-Object {
    $relativePath = if ($_.FullName.StartsWith($outputPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        $_.FullName.Substring($outputPrefix.Length)
    } else {
        $_.FullName
    }
    $_.Extension -in $textExtensions -and $relativePath -notmatch '^Dependencies[\\/]'
} | Select-String -Pattern '(?i)(C:[\\/]Users[\\/][^\\/]+|/Users/[^/]+)' -List
if ($personalPathMatches) {
    throw "Packaging safety check found a personal user-profile path: $($personalPathMatches.Path -join ', ')"
}

$archivePath = "$OutputDirectory.zip"
if (Test-Path -LiteralPath $archivePath) {
    Remove-Item -LiteralPath $archivePath -Force
}
Compress-Archive -Path (Join-Path $OutputDirectory "*") -DestinationPath $archivePath -CompressionLevel Optimal

Remove-Item -LiteralPath $stage -Recurse -Force
Write-Host "OpenDereth package created at: $OutputDirectory"
Write-Host "Shareable ZIP created at: $archivePath"
