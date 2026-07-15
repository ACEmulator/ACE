param(
    [Parameter(Mandatory = $true)]
    [string] $ProjectDirectory,
    [string] $OutputDirectory,
    [ValidateSet("Debug", "Release")]
    [string] $Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$repoRoot = [IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent))
$ProjectDirectory = (Resolve-Path -LiteralPath $ProjectDirectory).Path
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $repoRoot "artifacts\Mods"
}
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)

$projects = @(Get-ChildItem -LiteralPath $ProjectDirectory -File -Filter "*.csproj")
if ($projects.Count -ne 1) {
    throw "The mod directory must contain exactly one .csproj file."
}

$manifestPath = Join-Path $ProjectDirectory "ace-mod.json"
$metadataPath = Join-Path $ProjectDirectory "Meta.json"
if (-not (Test-Path -LiteralPath $manifestPath) -or -not (Test-Path -LiteralPath $metadataPath)) {
    throw "The mod directory must contain ace-mod.json and Meta.json."
}
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
if ($manifest.formatVersion -ne 1) {
    throw "Only ace-mod.json formatVersion 1 is supported."
}
foreach ($field in @("id", "name", "version", "folderName", "entryAssembly")) {
    if ([string]::IsNullOrWhiteSpace($manifest.$field)) {
        throw "ace-mod.json is missing $field."
    }
}
if ($manifest.folderName -match '[\\/:*?"<>|]' -or $manifest.entryAssembly -match '[\\/:*?"<>|]' -or
    -not $manifest.entryAssembly.EndsWith(".dll", [StringComparison]::OrdinalIgnoreCase) -or
    -not [string]::Equals([IO.Path]::GetFileNameWithoutExtension($manifest.entryAssembly), $manifest.folderName, [StringComparison]::OrdinalIgnoreCase)) {
    throw "The folderName and entryAssembly are invalid or do not match."
}

$localDotnet = Join-Path $repoRoot ".dotnet\dotnet.exe"
$dotnet = if (Test-Path -LiteralPath $localDotnet) { $localDotnet } else { (Get-Command dotnet -ErrorAction Stop).Source }
$tempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$stage = Join-Path $tempRoot "ACE-Mod-Package-$PID-$($manifest.folderName)"
$build = Join-Path $stage "Build"
$artifacts = Join-Path $stage "Artifacts"
$package = Join-Path $stage "Package"
$modDirectory = Join-Path $package "mod"

try {
    if (Test-Path -LiteralPath $stage) {
        Remove-Item -LiteralPath $stage -Recurse -Force
    }
    New-Item -ItemType Directory -Force -Path $build, $artifacts, $modDirectory, $OutputDirectory | Out-Null

    & $dotnet build $projects[0].FullName -c $Configuration --artifacts-path $artifacts -o $build
    if ($LASTEXITCODE -ne 0) {
        throw "The mod did not build successfully."
    }

    $entryAssembly = Join-Path $build $manifest.entryAssembly
    if (-not (Test-Path -LiteralPath $entryAssembly)) {
        throw "The build did not produce $($manifest.entryAssembly)."
    }

    Copy-Item -LiteralPath $manifestPath -Destination $package
    Copy-Item -LiteralPath $entryAssembly, $metadataPath -Destination $modDirectory
    foreach ($optionalName in @("Settings.json", "README.md")) {
        $optionalPath = Join-Path $ProjectDirectory $optionalName
        if (Test-Path -LiteralPath $optionalPath) {
            Copy-Item -LiteralPath $optionalPath -Destination $modDirectory
        }
    }
    Copy-Item -LiteralPath (Join-Path $repoRoot "LICENSE") -Destination (Join-Path $modDirectory "LICENSE.txt")

    $archive = Join-Path $OutputDirectory "$($manifest.id)-$($manifest.version).zip"
    if (Test-Path -LiteralPath $archive) {
        Remove-Item -LiteralPath $archive -Force
    }
    Compress-Archive -Path (Join-Path $package "*") -DestinationPath $archive -CompressionLevel Optimal
    (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash | Set-Content -LiteralPath ($archive + ".sha256") -Encoding ascii

    Write-Host "Mod package: $archive"
    Write-Host "Checksum: $archive.sha256"
}
finally {
    if (Test-Path -LiteralPath $stage) {
        Remove-Item -LiteralPath $stage -Recurse -Force
    }
}
