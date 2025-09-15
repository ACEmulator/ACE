# ACE Development Environment Deployment Script
param(
    [switch]$SkipBackup = $false,
    [switch]$Force = $false
)

Write-Host "ACE Development Deployment Starting..." -ForegroundColor Green

# Ensure we're in the right directory
Set-Location "C:\ACE"

# Create backup directory if it doesn't exist
if (!(Test-Path "Backups")) {
    New-Item -ItemType Directory -Path "Backups" | Out-Null
}

# Check if containers are running and backup if requested
if (!$SkipBackup) {
    Write-Host "Creating database backup..." -ForegroundColor Yellow

    $backupFile = "Backups\dev-backup-$(Get-Date -Format 'yyyy-MM-dd-HHmm').sql"

    # Only backup if database container is running
    $dbRunning = docker ps --filter "name=ace-db-dev" --filter "status=running" --quiet
    if ($dbRunning) {
        docker exec ace-db-dev mysqldump -u acedockeruser -p2020acEmulator2017 --all-databases > $backupFile
        Write-Host "Backup created: $backupFile" -ForegroundColor Green
    } else {
        Write-Host "Database not running, skipping backup" -ForegroundColor Cyan
    }
}

# Check current branch and pull latest code
Write-Host "=== Git Operations ===" -ForegroundColor Cyan
Write-Host "Current branch:" -ForegroundColor Yellow
git branch --show-current

Write-Host "Switching to dev branch..." -ForegroundColor Yellow
git checkout dev

Write-Host "Current branch after switch:" -ForegroundColor Yellow
git branch --show-current

Write-Host "Fetching latest changes from origin..." -ForegroundColor Yellow
$fetchResult = git fetch origin 2>&1
Write-Host "Fetch result: $fetchResult" -ForegroundColor Cyan

Write-Host "Pulling latest changes from origin/dev..." -ForegroundColor Yellow
$pullResult = git pull origin dev 2>&1
Write-Host "Pull result: $pullResult" -ForegroundColor Green

Write-Host "Latest commit:" -ForegroundColor Yellow
git log --oneline -1

# Stop existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml down

# Build and start containers
Write-Host "Building and starting development environment..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml up --build -d

# Wait for containers to be healthy
Write-Host "Waiting for containers to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Check container status
Write-Host "Container Status:" -ForegroundColor Cyan
docker ps --filter "name=ace-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# Check if ACE server is responding
Write-Host "Checking ACE server health..." -ForegroundColor Yellow
$serverHealthy = docker exec ace-server-dev netstat -an 2>$null | Select-String ":9000"
if ($serverHealthy) {
    Write-Host "Development server is running!" -ForegroundColor Green
    Write-Host "Connect to: dev.thresholme.online:9002" -ForegroundColor Green
} else {
    Write-Host "Server may still be starting up..." -ForegroundColor Yellow
    Write-Host "Check logs with: docker-compose -f docker-compose.dev.yml logs -f ace-server-dev" -ForegroundColor Cyan
}

Write-Host "Development deployment complete!" -ForegroundColor Green
