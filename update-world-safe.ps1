# ACE Safe World Update Script - Updates World Content While Preserving Player Data
param(
    [Parameter(Mandatory=$true)]
    [string]$UpdateFile,
    [string]$Environment = "prod",
    [switch]$SkipBackup = $false,
    [switch]$Force = $false
)

Write-Host "ACE Safe World Update Starting..." -ForegroundColor Green

# Safety check for production
if ($Environment -eq "prod" -and !$Force) {
    Write-Host "WARNING: This will update PRODUCTION world data!" -ForegroundColor Red
    $confirm = Read-Host "Are you sure? (yes/no)"
    if ($confirm -ne "yes") {
        Write-Host "Update cancelled." -ForegroundColor Red
        exit 1
    }
}

# Set container names
if ($Environment -eq "prod") {
    $dbContainer = "ace-db-prod"
    $serverContainer = "ace-server-prod"
} else {
    $dbContainer = "ace-db-dev"
    $serverContainer = "ace-server-dev"
}

# Validate update file exists
if (!(Test-Path $UpdateFile)) {
    Write-Host "ERROR: Update file not found: $UpdateFile" -ForegroundColor Red
    exit 1
}

Write-Host "Update file: $UpdateFile" -ForegroundColor Cyan
Write-Host "Environment: $Environment" -ForegroundColor Cyan

# Create backup before update (unless skipped)
if (!$SkipBackup) {
    Write-Host "Creating safety backup before update..." -ForegroundColor Yellow
    .\backup-safe.ps1 -Environment $Environment -FullBackup
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Backup failed! Aborting update." -ForegroundColor Red
        exit 1
    }
}

# Stop ACE server (keep database running)
Write-Host "Stopping ACE server for safe update..." -ForegroundColor Yellow
docker stop $serverContainer

# Apply world update (ONLY to ace_world database)
Write-Host "Applying world update to ace_world database..." -ForegroundColor Yellow
Write-Host "IMPORTANT: This will NOT affect player data (ace_shard/ace_auth)" -ForegroundColor Green

try {
    Get-Content $UpdateFile | docker exec -i $dbContainer mysql -u acedockeruser -p2020acEmulator2017 ace_world
    Write-Host "World update applied successfully!" -ForegroundColor Green
}
catch {
    Write-Host "ERROR: World update failed!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red

    # Restart server even if update failed
    Write-Host "Restarting server..." -ForegroundColor Yellow
    docker start $serverContainer
    exit 1
}

# Restart ACE server
Write-Host "Restarting ACE server..." -ForegroundColor Yellow
docker start $serverContainer

# Wait for server to be ready
Write-Host "Waiting for server to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Verify server is responding
$attempts = 0
$maxAttempts = 12
$serverHealthy = $false

do {
    try {
        $serverCheck = docker exec $serverContainer netstat -an 2>$null | Select-String ":9000"
        if ($serverCheck) {
            $serverHealthy = $true
            break
        }
    }
    catch {
        # Server not ready yet
    }

    $attempts++
    if ($attempts -lt $maxAttempts) {
        Write-Host "Server still starting... ($attempts/$maxAttempts)" -ForegroundColor Yellow
        Start-Sleep -Seconds 10
    }
} while ($attempts -lt $maxAttempts)

if ($serverHealthy) {
    Write-Host "SUCCESS: World update completed and server is running!" -ForegroundColor Green
    Write-Host "Player data was preserved (ace_shard/ace_auth untouched)" -ForegroundColor Green

    if ($Environment -eq "prod") {
        Write-Host "Production server: play.thresholme.online:9000" -ForegroundColor Cyan
    } else {
        Write-Host "Development server: dev.thresholme.online:9002" -ForegroundColor Cyan
    }
} else {
    Write-Host "WARNING: World update completed but server health check failed!" -ForegroundColor Yellow
    Write-Host "Check server logs for issues" -ForegroundColor Yellow
}

Write-Host "Safe world update complete!" -ForegroundColor Green
