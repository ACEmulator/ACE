# ACE Production Environment Deployment Script
param(
    [switch]$SkipBackup = $false,
    [switch]$Force = $false,
    [switch]$Emergency = $false
)

Write-Host "ACE Production Deployment Starting..." -ForegroundColor Red

# Safety check - require confirmation for production
if (!$Force -and !$Emergency) {
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
        Write-Host "Backing up production database..." -ForegroundColor Yellow
        docker exec ace-db-prod mysqldump -u acedockeruser -p2020acEmulator2017 --all-databases > $backupFile
        Write-Host "PRODUCTION backup created: $backupFile" -ForegroundColor Green

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

# Pull latest production code
Write-Host "Pulling latest production code..." -ForegroundColor Yellow
git checkout master
git pull origin master

# Check for any uncommitted changes
$gitStatus = git status --porcelain
if ($gitStatus -and !$Emergency) {
    Write-Host "Uncommitted changes detected:" -ForegroundColor Red
    Write-Host $gitStatus -ForegroundColor Red
    Write-Host "Clean your git workspace before production deployment" -ForegroundColor Red
    exit 1
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
