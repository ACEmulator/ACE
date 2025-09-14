# ACE Production Rollback Script
param(
    [Parameter(Mandatory=$true)]
    [string]$BackupFile,
    [switch]$Force = $false
)

Write-Host "🔄 ACE Production Rollback Starting..." -ForegroundColor Red

# Safety check
if (!$Force) {
    Write-Host "⚠️ This will ROLLBACK PRODUCTION to backup: $BackupFile" -ForegroundColor Red
    $confirm = Read-Host "Are you absolutely sure? (yes/no)"
    if ($confirm -ne "yes") {
        Write-Host "❌ Rollback cancelled." -ForegroundColor Red
        exit 1
    }
}

# Ensure we're in the right directory
Set-Location "C:\ACE"

# Check if backup file exists
if (!(Test-Path $BackupFile)) {
    Write-Host "❌ Backup file not found: $BackupFile" -ForegroundColor Red
    Write-Host "📋 Available backups:" -ForegroundColor Cyan
    Get-ChildItem "Backups\*.sql" | Sort-Object LastWriteTime -Descending | Select-Object Name, LastWriteTime
    exit 1
}

Write-Host "💾 Using backup: $BackupFile" -ForegroundColor Yellow

# Stop production server (keep database running)
Write-Host "🛑 Stopping production server..." -ForegroundColor Yellow
docker stop ace-server-prod

# Restore database
Write-Host "📥 Restoring database from backup..." -ForegroundColor Yellow
try {
    # Check if database is running
    $dbRunning = docker ps --filter "name=ace-db-prod" --filter "status=running" --quiet
    if (!$dbRunning) {
        Write-Host "🚀 Starting database container..." -ForegroundColor Yellow
        docker start ace-db-prod
        Start-Sleep -Seconds 30
    }

    # Restore the backup
    Get-Content $BackupFile | docker exec -i ace-db-prod mysql -u acedockeruser -p2020acEmulator2017
    Write-Host "✅ Database restored successfully!" -ForegroundColor Green

} catch {
    Write-Host "❌ Database restore failed!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    exit 1
}

# Restart production server
Write-Host "🚀 Restarting production server..." -ForegroundColor Yellow
docker start ace-server-prod

# Wait for server to be ready
Write-Host "⏳ Waiting for server to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 60

# Check if server is responding
$attempts = 0
$maxAttempts = 12
$serverHealthy = $false

do {
    try {
        $serverCheck = docker exec ace-server-prod netstat -an | Select-String ":9000"
        if ($serverCheck) {
            $serverHealthy = $true
            break
        }
    } catch {
        # Server not ready yet
    }

    $attempts++
    if ($attempts -lt $maxAttempts) {
        Write-Host "⏳ Server still starting... ($attempts/$maxAttempts)" -ForegroundColor Yellow
        Start-Sleep -Seconds 10
    }
} while ($attempts -lt $maxAttempts)

if ($serverHealthy) {
    Write-Host "✅ PRODUCTION rollback successful!" -ForegroundColor Green
    Write-Host "🌐 Server is live at: play.thresholme.online:9000" -ForegroundColor Green
} else {
    Write-Host "❌ Rollback completed but server health check failed!" -ForegroundColor Red
    Write-Host "📋 Check logs: docker-compose -f docker-compose.prod.yml logs ace-server-prod" -ForegroundColor Red
}

Write-Host "🎉 Rollback complete!" -ForegroundColor Green
