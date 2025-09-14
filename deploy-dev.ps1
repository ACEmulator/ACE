# ACE Development Environment Deployment Script
param(
    [switch]$SkipBackup = $false,
    [switch]$Force = $false
)

Write-Host "🔧 ACE Development Deployment Starting..." -ForegroundColor Green

# Ensure we're in the right directory
Set-Location "C:\ACE"

# Create backup directory if it doesn't exist
if (!(Test-Path "Backups")) {
    New-Item -ItemType Directory -Path "Backups" | Out-Null
}

# Check if containers are running and backup if requested
if (!$SkipBackup) {
    Write-Host "💾 Creating database backup..." -ForegroundColor Yellow

    $backupFile = "Backups\dev-backup-$(Get-Date -Format 'yyyy-MM-dd-HHmm').sql"

    try {
        # Only backup if database container is running
        $dbRunning = docker ps --filter "name=ace-db-dev" --filter "status=running" --quiet
        if ($dbRunning) {
            docker exec ace-db-dev mysqldump -u acedockeruser -p2020acEmulator2017 --all-databases > $backupFile
            Write-Host "✅ Backup created: $backupFile" -ForegroundColor Green
        } else {
            Write-Host "ℹ️ Database not running, skipping backup" -ForegroundColor Cyan
        }
    } catch {
        Write-Host "⚠️ Backup failed, but continuing deployment..." -ForegroundColor Yellow
    }
}

# Pull latest code
Write-Host "📥 Pulling latest development code..." -ForegroundColor Yellow
git checkout dev
git pull origin dev

# Stop existing containers
Write-Host "🛑 Stopping existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml down

# Build and start containers
Write-Host "🚀 Building and starting development environment..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml up --build -d

# Wait for containers to be healthy
Write-Host "⏳ Waiting for containers to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Check container status
Write-Host "📊 Container Status:" -ForegroundColor Cyan
docker ps --filter "name=ace-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# Check if ACE server is responding
Write-Host "🔍 Checking ACE server health..." -ForegroundColor Yellow
try {
    $serverHealthy = docker exec ace-server-dev netstat -an | Select-String ":9000"
    if ($serverHealthy) {
        Write-Host "✅ Development server is running!" -ForegroundColor Green
        Write-Host "🌐 Connect to: dev.thresholme.online:9002" -ForegroundColor Green
    } else {
        Write-Host "⚠️ Server may still be starting up..." -ForegroundColor Yellow
        Write-Host "📋 Check logs with: docker-compose -f docker-compose.dev.yml logs -f ace-server-dev" -ForegroundColor Cyan
    }
} catch {
    Write-Host "⚠️ Unable to check server status - may still be starting" -ForegroundColor Yellow
}

Write-Host "🎉 Development deployment complete!" -ForegroundColor Green
