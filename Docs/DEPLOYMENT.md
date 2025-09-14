# ACE Server Deployment Guide

## Infrastructure Overview

- **Development Machine**: Local development and testing
- **Game Server**: Windows Server Datacenter (100.105.32.14)
  - 20 cores, 200GB RAM, 1TB RAID'd SSD
  - Connected via Tailscale network
- **Domain**: thresholme.online (managed via Cloudflare)

## Network Configuration

### Ports
- **Production**: 9000-9001 (UDP)
- **Development**: 9002-9003 (UDP)

### DNS Records (Cloudflare)
- `play.thresholme.online` → 100.105.32.14 (Production)
- `dev.thresholme.online` → 100.105.32.14 (Development)

**Important**: Set DNS records to "DNS only" (gray cloud) - do NOT proxy game traffic through Cloudflare.

### Windows Firewall Rules
```powershell
# Production ports (already open)
New-NetFirewallRule -DisplayName "ACE Production Server UDP 9000" -Direction Inbound -Protocol UDP -LocalPort 9000 -Action Allow
New-NetFirewallRule -DisplayName "ACE Production Server UDP 9001" -Direction Inbound -Protocol UDP -LocalPort 9001 -Action Allow

# Development ports
New-NetFirewallRule -DisplayName "ACE Dev Server UDP 9002" -Direction Inbound -Protocol UDP -LocalPort 9002 -Action Allow
New-NetFirewallRule -DisplayName "ACE Dev Server UDP 9003" -Direction Inbound -Protocol UDP -LocalPort 9003 -Action Allow
```

## Git Repository Structure

- **Repository**: https://github.com/jamesmcmenamin/ACE
- **Main Branch**: `master` - Production-ready, stable code
- **Dev Branch**: `dev` - Active development and testing

### Branch Workflow
1. Develop on `dev` branch
2. Test thoroughly in development environment
3. Merge `dev` → `master` when stable
4. Deploy `master` to production

## Server Directory Structure

```
C:\Prod\ACE\
├── Source\              # Production codebase (master branch)
├── Config\              # Production config (Config.js)
├── Content\             # Custom content files
├── Dats\               # Asheron's Call DAT files
├── Logs\               # Production server logs
├── Mods\               # Server modifications
├── db-data\            # MySQL production database files
├── docker-compose.prod.yml
└── Dockerfile

C:\Dev\ACE\
├── Source\              # Development codebase (dev branch)
├── Config\              # Development config (Config.js)
├── Content\             # Custom content files
├── Dats\               # Asheron's Call DAT files (can symlink to prod)
├── Logs\               # Development server logs
├── Mods\               # Server modifications
├── db-data\            # MySQL dev database files
├── docker-compose.dev.yml
└── Dockerfile
```

## Configuration Differences

### Production (Config.js.prod)
- WorldName: "Thresholme"
- Database hosts: ace-db-prod
- Session limits: 256 max, 5 per IP
- Performance optimized: World DB precaching enabled
- Security: Higher password work factor (12)
- All major towns preloaded

### Development (Config.js.dev)
- WorldName: "Thresholme-Dev"
- Database hosts: ace-db-dev
- Session limits: 32 max, unlimited per IP
- Debug optimized: No precaching, detailed error logging
- Security: Lower work factor (8), admin access level 5
- Minimal landblock preloading

## Deployment Process

### Initial Server Setup
1. **Install Docker Desktop** on Windows Server
2. **Create directory structure**:
   ```powershell
   mkdir C:\Prod\ACE, C:\Dev\ACE
   ```

3. **Clone repositories**:
   ```powershell
   cd C:\Prod
   git clone https://github.com/jamesmcmenamin/ACE.git ACE
   cd ACE
   git checkout master

   cd C:\Dev
   git clone https://github.com/jamesmcmenamin/ACE.git ACE
   cd ACE
   git checkout dev
   ```

4. **Copy configuration files**:
   ```powershell
   # Production
   copy Config.js.prod C:\Prod\ACE\Config\Config.js

   # Development
   copy Config.js.dev C:\Dev\ACE\Config\Config.js
   ```

5. **Copy Docker compose files**:
   ```powershell
   copy docker-compose.prod.yml C:\Prod\ACE\
   copy docker-compose.dev.yml C:\Dev\ACE\
   ```

6. **Install Asheron's Call DAT files** in both `Dats\` directories

### Building and Starting Services

#### Development Environment (Test First)
```powershell
cd C:\Dev\ACE
docker-compose -f docker-compose.dev.yml up --build -d
```

#### Production Environment (After Dev Testing)
```powershell
cd C:\Prod\ACE
docker-compose -f docker-compose.prod.yml up --build -d
```

### Updating Deployments

#### Development Updates
```powershell
# ON THE SERVER (100.105.32.14) - PULL ONLY, NEVER COMMIT
cd C:\Dev\ACE
git pull origin dev
docker-compose -f docker-compose.dev.yml up --build -d
```

#### Production Updates
```powershell
# ON THE SERVER (100.105.32.14) - PULL ONLY, NEVER COMMIT
cd C:\Prod\ACE
git pull origin master
docker-compose -f docker-compose.prod.yml up --build -d
```

## Monitoring and Maintenance

### Checking Container Status
```powershell
docker ps
docker logs ace-server-prod
docker logs ace-server-dev
```

### Database Access
- **Production MySQL**: localhost:3306
- **Development MySQL**: localhost:3307

### Log Locations
- **Production**: `C:\Prod\ACE\Logs\`
- **Development**: `C:\Dev\ACE\Logs\`

## Troubleshooting

### Common Issues
1. **Port conflicts**: Ensure no other services are using ports 9000-9003
2. **Database connection failures**: Check MySQL container health
3. **DAT file errors**: Verify all 4 DAT files are present and readable
4. **Memory issues**: Monitor RAM usage with 200GB available

### Useful Commands
```powershell
# Restart services
docker-compose -f docker-compose.prod.yml restart

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Stop all services
docker-compose -f docker-compose.prod.yml down

# Rebuild from scratch
docker-compose -f docker-compose.prod.yml down
docker-compose -f docker-compose.prod.yml up --build -d
```

## Security Notes

- Auto-login configured for server (no Ctrl+Alt+Delete required)
- Tailscale provides secure network access
- Production config uses higher security settings
- Database passwords should be rotated periodically

## Performance Optimization

With 20 cores and 200GB RAM:
- Production uses 50% threading allocation
- World database precaching enabled for production
- Conservative threading for development debugging
