# ACE Safe Backup Script - Separates Player Data from World Data
param(
    [string]$Environment = "prod",  # prod or dev
    [switch]$PlayerDataOnly = $false,
    [switch]$WorldDataOnly = $false,
    [switch]$FullBackup = $false
)

Write-Host "ACE Safe Backup Starting..." -ForegroundColor Green

# Set container names based on environment
if ($Environment -eq "prod") {
    $dbContainer = "ace-db-prod"
    $backupPrefix = "prod"
} else {
    $dbContainer = "ace-db-dev"
    $backupPrefix = "dev"
}

# Create backup directory
$timestamp = Get-Date -Format 'yyyy-MM-dd-HHmm'
$backupDir = "Backups\$timestamp-$backupPrefix"
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

Write-Host "Creating backups in: $backupDir" -ForegroundColor Cyan

# Check if database container is running
$dbRunning = docker ps --filter "name=$dbContainer" --filter "status=running" --quiet
if (!$dbRunning) {
    Write-Host "ERROR: Database container $dbContainer is not running!" -ForegroundColor Red
    exit 1
}

if ($PlayerDataOnly -or $FullBackup) {
    Write-Host "Backing up PLAYER DATA (ace_auth + ace_shard)..." -ForegroundColor Yellow

    # Backup user accounts
    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_auth > "$backupDir\ace_auth.sql"
    Write-Host "  ace_auth backup: $backupDir\ace_auth.sql" -ForegroundColor Green

    # Backup player characters and data
    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_shard > "$backupDir\ace_shard.sql"
    Write-Host "  ace_shard backup: $backupDir\ace_shard.sql" -ForegroundColor Green
}

if ($WorldDataOnly -or $FullBackup) {
    Write-Host "Backing up WORLD DATA (ace_world)..." -ForegroundColor Yellow

    # Backup world content (NPCs, items, quests, etc.)
    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_world > "$backupDir\ace_world.sql"
    Write-Host "  ace_world backup: $backupDir\ace_world.sql" -ForegroundColor Green
}

if (!$PlayerDataOnly -and !$WorldDataOnly -and !$FullBackup) {
    # Default: Full backup
    Write-Host "Creating FULL BACKUP (all databases)..." -ForegroundColor Yellow

    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_auth > "$backupDir\ace_auth.sql"
    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_shard > "$backupDir\ace_shard.sql"
    docker exec $dbContainer mysqldump -u acedockeruser -p2020acEmulator2017 --single-transaction ace_world > "$backupDir\ace_world.sql"

    Write-Host "  Full backup completed in: $backupDir" -ForegroundColor Green
}

# Create backup manifest
$manifest = @{
    timestamp = $timestamp
    environment = $Environment
    container = $dbContainer
    files = Get-ChildItem "$backupDir\*.sql" | Select-Object Name, Length
} | ConvertTo-Json -Depth 3

$manifest | Out-File "$backupDir\manifest.json"

Write-Host "`nBackup Summary:" -ForegroundColor Cyan
Get-ChildItem "$backupDir" | Select-Object Name, Length | Format-Table

Write-Host "Safe backup complete: $backupDir" -ForegroundColor Green

# Clean up old backups (keep last 20)
$oldBackups = Get-ChildItem "Backups" | Where-Object { $_.Name -like "*-$backupPrefix" } | Sort-Object LastWriteTime -Descending
if ($oldBackups.Count -gt 20) {
    Write-Host "Cleaning up old backups (keeping last 20)..." -ForegroundColor Yellow
    $oldBackups[20..($oldBackups.Count-1)] | Remove-Item -Recurse -Force
}
