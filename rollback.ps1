# ACE Safe World Rollback Script - Rolls Back World Data While Preserving Player Data
param(
    [Parameter(Mandatory=$true)]
    [string]$BackupDir,
    [string]$Environment = "prod",
    [switch]$Force = $false,
    [switch]$WorldOnly = $true
)

Write-Host "ACE Safe World Rollback Starting..." -ForegroundColor Red

# Safety check
if (!$Force) {
    Write-Host "WARNING: This will rollback $Environment world data to: $BackupDir" -ForegroundColor Red
    Write-Host "PLAYER DATA WILL BE PRESERVED (ace_shard/ace_auth)" -ForegroundColor Green
    $confirm = Read-Host "Continue? (yes/no)"
    if ($confirm -ne "yes") {
        Write-Host "Rollback cancelled." -ForegroundColor Red
        exit 1
    }
}

# Validate backup directory exists
if (!(Test-Path $BackupDir)) {
    Write-Host "ERROR: Backup directory not found: $BackupDir" -ForegroundColor Red
    Write-Host "Available backups:" -ForegroundColor Cyan
    Get-ChildItem "Backups" | Where-Object { $_.PSIsContainer } | Sort-Object LastWriteTime -Descending | Select-Object Name, LastWriteTime
    exit 1
}

# Set container names
if ($Environment -eq "prod") {
    $dbContainer = "ace-db-prod"
    $serverContainer = "ace-server-prod"
} else {
    $dbContainer = "ace-db-dev"
    $serverContainer = "ace-server-dev"
}

# Validate backup files exist
$worldBackup = "$BackupDir\ace_world.sql"
if (!(Test-Path $worldBackup)) {
    Write-Host "ERROR: World backup not found: $worldBackup" -ForegroundColor Red
    Write-Host "Available files in backup:" -ForegroundColor Cyan
    Get-ChildItem $BackupDir
    exit 1
}

Write-Host "Using world backup: $worldBackup" -ForegroundColor Yellow

# Create emergency backup before rollback
Write-Host "Creating emergency backup before rollback..." -ForegroundColor Yellow
.\backup-safe.ps1 -Environment $Environment -WorldDataOnly

# Stop ACE server (keep database running)
Write-Host "Stopping ACE server for rollback..." -ForegroundColor Yellow
docker stop $serverContainer

# Restore ONLY world data (preserve player data)
Write-Host "Restoring world data (preserving player data)..." -ForegroundColor Yellow
Write-Host "IMPORTANT: Player data (ace_shard/ace_auth) will NOT be affected" -ForegroundColor Green

try {
    # Only restore ace_world database
    Get-Content $worldBackup | docker exec -i $dbContainer mysql -u acedockeruser -p2020acEmulator2017 ace_world
    Write-Host "World data rollback completed!" -ForegroundColor Green
}
catch {
    Write-Host "ERROR: World rollback failed!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red

    # Restart server even if rollback failed
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
    Write-Host "SUCCESS: World rollback completed and server is running!" -ForegroundColor Green
    Write-Host "Player data was preserved during rollback" -ForegroundColor Green

    if ($Environment -eq "prod") {
        Write-Host "Production server: play.thresholme.online:9000" -ForegroundColor Cyan
    } else {
        Write-Host "Development server: dev.thresholme.online:9002" -ForegroundColor Cyan
    }
} else {
    Write-Host "WARNING: Rollback completed but server health check failed!" -ForegroundColor Yellow
    Write-Host "Check server logs for issues" -ForegroundColor Yellow
}

Write-Host "Safe world rollback complete!" -ForegroundColor Green
