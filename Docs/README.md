# Thresholme ACE Server Documentation

Welcome to the Thresholme Asheron's Call Emulator server documentation. This directory contains comprehensive guides for setting up, developing, and maintaining the ACE server infrastructure.

## Quick Start

### For Administrators
1. Read [DEPLOYMENT.md](DEPLOYMENT.md) for complete setup instructions
2. Ensure all prerequisites are met (Docker, DAT files, etc.)
3. Follow the deployment process for both dev and production environments

### For Developers
1. Read [DEVELOPMENT.md](DEVELOPMENT.md) for development workflow
2. Set up your local development environment
3. Follow the git branching strategy for contributions

## Document Overview

### 🚀 Quick Start Guides (DUMMY-PROOF)
- **[DEPLOYMENT-SIMPLE.md](DEPLOYMENT-SIMPLE.md)** - Copy & paste server setup guide
- **[DEVELOPMENT-SIMPLE.md](DEVELOPMENT-SIMPLE.md)** - Copy & paste development workflow

### 📚 Detailed Technical Guides
- **[DEPLOYMENT.md](DEPLOYMENT.md)** - Complete infrastructure and deployment reference
- **[DEVELOPMENT.md](DEVELOPMENT.md)** - Detailed development workflow and architecture

**👀 Start with the SIMPLE guides - they have everything you need with copy & paste commands!**

## Infrastructure Summary

### Environments
- **Production**: `play.thresholme.online:9000` (C:\ACE - master branch)
- **Development**: `dev.thresholme.online:9002` (C:\ACE - dev branch)

### Server Specifications
- **Platform**: Windows Server Datacenter
- **Resources**: 20 cores, 200GB RAM, 1TB RAID SSD
- **Network**: Tailscale (100.105.32.14)
- **Domain**: thresholme.online (Cloudflare DNS)

### Key Technologies
- **Application**: ACE (Asheron's Call Emulator)
- **Database**: MySQL 8.0 (Docker containers)
- **Containerization**: Docker Desktop for Windows
- **Version Control**: Git (GitHub)

## Security and Access

### Network Security
- Tailscale VPN for secure server access
- Cloudflare DNS (DNS-only mode for game traffic)
- Windows Firewall configured for game ports

### Access Control
- Production: Limited sessions per IP
- Development: Unlimited access for testing
- Auto-login configured (no Ctrl+Alt+Delete required)

## Support and Troubleshooting

### Common Issues
- Port conflicts between environments
- Database connection problems
- DAT file access issues
- Docker container health problems

### Monitoring
- Container logs via Docker commands
- Server performance monitoring enabled
- Separate log directories for each environment

## Maintenance Schedule

### Regular Tasks
- Monitor server performance and resource usage
- Check container health and logs
- Update from upstream ACEmulator project
- Backup database data

### As Needed
- Apply security updates
- Scale resources based on player population
- Implement new features and content

## Getting Help

### Resources
- [ACEmulator GitHub](https://github.com/ACEmulator/ACE)
- [ACEmulator Discord](https://discord.gg/C2WzhP9)
- Project documentation and wiki

### Internal Procedures
1. Check logs first (container and application)
2. Verify configuration differences between environments
3. Test in development before applying to production
4. Document any changes or issues discovered

---

**Last Updated**: September 14, 2025
**Maintained By**: James McMenamin
**Server Version**: ACE Latest (Docker-based deployment)
