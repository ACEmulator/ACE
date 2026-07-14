param(
    [ValidateSet("Debug", "Release")]
    [string] $Configuration = "Release",
    [string] $OutputDirectory
)

$ErrorActionPreference = "Stop"
$repoRoot = [IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent))
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $repoRoot "artifacts\ACE-SinglePlayer"
}
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
$artifactsRoot = [IO.Path]::GetFullPath((Join-Path $repoRoot "artifacts"))
$artifactsPrefix = $artifactsRoot.TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
if ($OutputDirectory -ne $artifactsRoot -and -not $OutputDirectory.StartsWith($artifactsPrefix, [StringComparison]::OrdinalIgnoreCase)) {
    throw "OutputDirectory must remain under $artifactsRoot"
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
$stage = [IO.Path]::GetFullPath((Join-Path $tempRoot "ACE-SinglePlayer-Publish-$PID"))
$tempPrefix = $tempRoot.TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
if (-not $stage.StartsWith($tempPrefix, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Temporary staging directory must remain under $tempRoot"
}
$launcherPublish = Join-Path $stage "Launcher"
$serverPublish = Join-Path $stage "Server"
$decalHostPublish = Join-Path $stage "DecalHost"

function Invoke-DotNet([string[]] $Arguments) {
    & $dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') failed with exit code $LASTEXITCODE"
    }
}

foreach ($path in @($stage, $OutputDirectory)) {
    if (Test-Path -LiteralPath $path) {
        Remove-Item -LiteralPath $path -Recurse -Force
    }
}
New-Item -ItemType Directory -Path $launcherPublish, $serverPublish, $decalHostPublish, $OutputDirectory | Out-Null

Write-Host "Publishing ACE.Server as self-contained Windows x64..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.Server\ACE.Server.csproj"), "-c", $Configuration, "-r", "win-x64", "--self-contained", "true", "-o", $serverPublish)

Write-Host "Publishing ACE.SinglePlayer as self-contained Windows x64..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.SinglePlayer\ACE.SinglePlayer.csproj"), "-c", $Configuration, "-r", "win-x64", "--self-contained", "true", "-o", $launcherPublish)

Write-Host "Publishing the optional Decal helper as a self-contained Windows x86 executable..."
Invoke-DotNet @("publish", (Join-Path $repoRoot "Source\ACE.SinglePlayer.DecalHost\ACE.SinglePlayer.DecalHost.csproj"), "-c", $Configuration, "-r", "win-x86", "--self-contained", "true", "-p:PublishSingleFile=true", "-o", $decalHostPublish)

Copy-Item -Path (Join-Path $launcherPublish "*") -Destination $OutputDirectory -Recurse -Force
New-Item -ItemType Directory -Path (Join-Path $OutputDirectory "Server"), (Join-Path $OutputDirectory "Tools") | Out-Null
Copy-Item -Path (Join-Path $serverPublish "*") -Destination (Join-Path $OutputDirectory "Server") -Recurse -Force
Copy-Item -Path (Join-Path $decalHostPublish "ACE.SinglePlayer.DecalHost.exe") -Destination (Join-Path $OutputDirectory "Tools") -Force

foreach ($directory in @("Runtime", "Mods", "Logs", "Client", "Docs")) {
    New-Item -ItemType Directory -Path (Join-Path $OutputDirectory $directory) -Force | Out-Null
}
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_FIRST_RUN.md") (Join-Path $OutputDirectory "FIRST_RUN.md")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_ARCHITECTURE.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\AQUIFIR_MOD_COMPATIBILITY.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_ROADMAP.md") (Join-Path $OutputDirectory "Docs")
Copy-Item (Join-Path $repoRoot "docs\SINGLE_PLAYER_BUILD_AND_TEST.md") (Join-Path $OutputDirectory "Docs")

$forbidden = Get-ChildItem -LiteralPath $OutputDirectory -Recurse -File | Where-Object {
    $_.Name -in @("Config.js", "settings.json", "acclient.exe", "mariadbd.exe", "ace-server.ready.json", "ace-server.process.json") -or
    $_.Name -like "client_*.dat" -or $_.Extension -eq ".log"
}
if ($forbidden) {
    throw "Packaging safety check found forbidden runtime/proprietary files: $($forbidden.FullName -join ', ')"
}

Remove-Item -LiteralPath $stage -Recurse -Force
Write-Host "ACE Single Player package created at: $OutputDirectory"
