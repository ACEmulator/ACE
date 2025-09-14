# ACE Development Guide

## Development Workflow

### ⚠️ CRITICAL SECURITY RULE ⚠️
**THE DEVELOPMENT SERVER (100.105.32.14) MUST NEVER COMMIT CODE CHANGES**

- **Development machines**: Where code is written, committed, and pushed
- **Development server**: Only pulls and deploys code - NEVER commits
- **Production server**: Only pulls and deploys code - NEVER commits

All code changes happen on development machines and get pushed to GitHub. Servers only pull and deploy.

### Repository Setup
- **Fork**: https://github.com/jamesmcmenamin/ACE (forked from ACEmulator/ACE)
- **Upstream**: https://github.com/ACEmulator/ACE (original project)

### Git Configuration
```bash
# Check current remotes
git remote -v

# Should show:
# origin    https://github.com/jamesmcmenamin/ACE.git (fetch)
# origin    https://github.com/jamesmcmenamin/ACE.git (push)
# upstream  https://github.com/ACEmulator/ACE.git (fetch)
# upstream  https://github.com/ACEmulator/ACE.git (push)
```

### Branch Strategy
- **master**: Production-ready code, deployed to `C:\Prod\ACE`
- **dev**: Active development, deployed to `C:\Dev\ACE`

### Development Process
1. **Work on dev branch**:
   ```bash
   git checkout dev
   git pull origin dev
   # Make changes
   git add .
   git commit -m "Description of changes"
   git push origin dev
   ```

2. **Deploy to development server for testing**:
   ```powershell
   # ON THE SERVER (100.105.32.14) - PULL ONLY, NEVER COMMIT
   cd C:\Dev\ACE
   git pull origin dev
   docker-compose -f docker-compose.dev.yml up --build -d
   ```

3. **When stable, promote to production**:
   ```bash
   git checkout master
   git merge dev
   git push origin master
   ```

4. **Deploy to production server**:
   ```powershell
   # ON THE SERVER (100.105.32.14) - PULL ONLY, NEVER COMMIT
   cd C:\Prod\ACE
   git pull origin master
   docker-compose -f docker-compose.prod.yml up --build -d
   ```

### Syncing with Upstream
Periodically sync with the main ACEmulator project:
```bash
git fetch upstream
git checkout master
git merge upstream/master
git push origin master

git checkout dev
git merge master
git push origin dev
```

## Development Environment

### Configuration
- **World Name**: "Thresholme-Dev"
- **Ports**: 9002-9003 (externally), 9000-9001 (internally in container)
- **Database**: Separate dev database (ace-db-dev)
- **Admin Access**: Default level 5 for testing
- **Debug Features**: Detailed error logging enabled

### Useful Development Settings
- Shorter cache times for faster testing
- No world database precaching (faster startup)
- Minimal landblock preloading
- Lower password hash work factor
- Fast shutdown (10 seconds vs 60)

### Testing Players Connect To
- **URL**: `dev.thresholme.online:9002`
- **Access**: Unlimited sessions per IP for testing

## Code Structure

### Key Projects
- **ACE.Server**: Main server application
- **ACE.Database**: Database models and operations
- **ACE.Entity**: Game entities and enums
- **ACE.Common**: Shared utilities and configuration
- **ACE.DatLoader**: Asheron's Call DAT file reader

### Configuration System
- **Config.js**: Main configuration file
- **Environment Variables**: Docker environment overrides
- **PropertyManager**: Runtime property management

### Database Structure
- **ace_auth**: Authentication and accounts
- **ace_shard**: Character data and world state
- **ace_world**: Static world data (NPCs, items, etc.)

## Debugging and Testing

### Container Logs
```powershell
# View real-time logs
docker-compose -f docker-compose.dev.yml logs -f ace-server-dev

# View database logs
docker-compose -f docker-compose.dev.yml logs -f ace-db-dev
```

### Database Access
```powershell
# Connect to dev database
docker exec -it ace-db-dev mysql -u acedockeruser -p
# Password: 2020acEmulator2017
```

### Performance Monitoring
Development config enables server performance monitoring:
- Use `/serverperformance` command in-game
- Monitor thread allocation and performance bottlenecks

### Common Development Tasks

#### Adding New Features
1. Create feature branch from dev
2. Implement changes
3. Test in development environment
4. Merge back to dev
5. Test integration
6. Promote to master when stable

#### Database Changes
1. Test in development environment first
2. Document any schema changes
3. Ensure migration scripts work properly
4. Test with production-like data volume

#### Configuration Changes
1. Update both Config.js.dev and Config.js.prod
2. Document differences in DEPLOYMENT.md
3. Test both environments after changes

## Performance Considerations

### Development Environment
- Conservative threading for easier debugging
- No database precaching for faster startup
- Minimal memory usage for development machine

### When Promoting to Production
- Ensure threading settings are optimized
- Enable performance features (precaching, etc.)
- Test under load before going live

## Security Notes

### Development Environment
- Higher default admin access for testing
- Detailed error logging (may expose sensitive info)
- Unlimited sessions per IP
- Lower password security for faster testing

### Before Production
- Review all security settings
- Disable debug logging
- Implement proper session limits
- Use production-grade password hashing

## Troubleshooting

### Common Development Issues
1. **Port conflicts**: Check if dev ports 9002-9003 are in use
2. **Database connection**: Ensure ace-db-dev container is healthy
3. **Config errors**: Validate JSON syntax in Config.js
4. **DAT file issues**: Ensure all 4 DAT files are accessible

### Reset Development Environment
```powershell
cd C:\Dev\ACE
docker-compose -f docker-compose.dev.yml down -v
docker-compose -f docker-compose.dev.yml up --build -d
```
**Warning**: This deletes all development database data!

### Useful Development Commands
```powershell
# Quick restart after code changes
docker-compose -f docker-compose.dev.yml restart ace-server-dev

# Rebuild only the server (not database)
docker-compose -f docker-compose.dev.yml up --build -d ace-server-dev

# View container resource usage
docker stats
```
