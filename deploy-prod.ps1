# ACE Production Environment Deployment Script
param(
    [switch]$SkipBackup = $false,
    [switch]$Force = $false,
    [switch]$Emergency = $false
)

Write-Host "ACE Production Deployment Starting..." -ForegroundColor Red

# Safety check - require confirmation for production (only if not forced)
if (!$Force -and !$Emergency) {
    Write-Host "Use -Force to skip this confirmation" -ForegroundColor Cyan
    $confirm = Read-Host "This will deploy to PRODUCTION. Are you sure? (yes/no)"
    if ($confirm -ne "yes") {
        Write-Host "Production deployment cancelled." -ForegroundColor Red
        exit 1
    }
}

# Ensure we're in the right directory
Set-Location "C:\ACE"

# Create backup directory if it doesn't exist
if (!(Test-Path "Backups")) {
    New-Item -ItemType Directory -Path "Backups" | Out-Null
}

# ALWAYS backup production unless explicitly skipped
if (!$SkipBackup) {
    Write-Host "Creating PRODUCTION database backup..." -ForegroundColor Yellow

    $backupFile = "Backups\prod-backup-$(Get-Date -Format 'yyyy-MM-dd-HHmm').sql"

    # Check if production database is running
    $dbRunning = docker ps --filter "name=ace-db-prod" --filter "status=running" --quiet
    if ($dbRunning) {
        Write-Host "Creating SAFE production backup (preserves player data)..." -ForegroundColor Yellow
        .\backup-safe.ps1 -Environment prod -FullBackup
        Write-Host "SAFE production backup completed" -ForegroundColor Green

        # Keep only last 10 backups
        $backups = Get-ChildItem "Backups\prod-backup-*.sql" | Sort-Object LastWriteTime -Descending
        if ($backups.Count -gt 10) {
            $backups[10..($backups.Count-1)] | Remove-Item
            Write-Host "Cleaned up old backups (keeping last 10)" -ForegroundColor Cyan
        }
    } else {
        Write-Host "Production database not running!" -ForegroundColor Red
        if (!$Emergency) {
            Write-Host "Aborting deployment - database should be running for backup" -ForegroundColor Red
            exit 1
        }
    }
}

# Check current branch and pull latest production code
Write-Host "=== Git Operations ===" -ForegroundColor Cyan
Write-Host "Current branch:" -ForegroundColor Yellow
git branch --show-current

Write-Host "Switching to master branch..." -ForegroundColor Yellow
git checkout master

Write-Host "Current branch after switch:" -ForegroundColor Yellow
git branch --show-current

Write-Host "Fetching latest changes from origin..." -ForegroundColor Yellow
$fetchResult = git fetch origin 2>&1
Write-Host "Fetch result: $fetchResult" -ForegroundColor Cyan

Write-Host "Pulling latest changes from origin/master..." -ForegroundColor Yellow
$pullResult = git pull origin master 2>&1
Write-Host "Pull result: $pullResult" -ForegroundColor Green

Write-Host "Latest commit:" -ForegroundColor Yellow
git log --oneline -1

# Check for any uncommitted changes to SOURCE CODE (ignore data/log files)
$gitStatus = git status --porcelain | Where-Object {
    $_ -notlike "*Database-Install/*" -and
    $_ -notlike "*Logs-*" -and
    $_ -notlike "*db-data-*" -and
    $_ -notlike "*Backups/*" -and
    $_ -notlike "*ace_*.sql" -and
    $_ -notlike "*backup*.sql"
}
if ($gitStatus -and !$Emergency) {
    Write-Host "SOURCE CODE changes detected (data/log files ignored):" -ForegroundColor Red
    Write-Host $gitStatus -ForegroundColor Red
    Write-Host "Clean your git workspace before production deployment" -ForegroundColor Red
    exit 1
} else {
    Write-Host "Git workspace clean (ignoring data/log files)" -ForegroundColor Green
}

# Stop existing production containers gracefully
Write-Host "Stopping production containers gracefully..." -ForegroundColor Yellow
docker-compose -f docker-compose.prod.yml down

# Build and start production containers
Write-Host "Building and starting PRODUCTION environment..." -ForegroundColor Red
docker-compose -f docker-compose.prod.yml up --build -d

# Wait for containers to be healthy
Write-Host "Waiting for production containers to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 60

# Check container status
Write-Host "Production Container Status:" -ForegroundColor Cyan
docker ps --filter "name=ace-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# Check if production server is responding
Write-Host "Checking production server health..." -ForegroundColor Yellow
$attempts = 0
$maxAttempts = 12
$serverHealthy = $false

do {
    $serverCheck = docker exec ace-server-prod netstat -an 2>$null | Select-String ":9000"
    if ($serverCheck) {
        $serverHealthy = $true
        break
    }

    $attempts++
    if ($attempts -lt $maxAttempts) {
        Write-Host "Server still starting... ($attempts/$maxAttempts)" -ForegroundColor Yellow
        Start-Sleep -Seconds 10
    }
} while ($attempts -lt $maxAttempts)

if ($serverHealthy) {
    Write-Host "PRODUCTION server is running!" -ForegroundColor Green
    Write-Host "Live at: play.thresholme.online:9000" -ForegroundColor Green
    Write-Host "Monitor with: docker-compose -f docker-compose.prod.yml logs -f" -ForegroundColor Cyan
} else {
    Write-Host "PRODUCTION server health check failed!" -ForegroundColor Red
    Write-Host "Check logs immediately: docker-compose -f docker-compose.prod.yml logs ace-server-prod" -ForegroundColor Red
    Write-Host "Consider rollback if needed" -ForegroundColor Yellow
}

Write-Host "Production deployment complete!" -ForegroundColor Green
