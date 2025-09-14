# 🚀 DUMMY-PROOF ACE Server Deployment Guide

## 📋 What You Need Before Starting

### Prerequisites Checklist
- [ ] Windows Server 2022 Datacenter (100.105.32.14)
- [ ] Tailscale network access to server
- [ ] Cloudflare DNS configured (thresholme.online)
- [ ] Firewall ports 9000-9003 UDP open
- [ ] Asheron's Call DAT files (4 files total)

### Required Software on Server
- [ ] Git for Windows
- [ ] Docker Desktop for Windows

---

## 🔧 Step 1: Install Git on Server

**Copy and paste these commands in PowerShell as Administrator:**

```powershell
# Download Git installer
Invoke-WebRequest -Uri "https://github.com/git-for-windows/git/releases/download/v2.42.0.windows.2/Git-2.42.0.2-64-bit.exe" -OutFile "C:\temp\Git-installer.exe"

# Install Git silently
Start-Process -FilePath "C:\temp\Git-installer.exe" -ArgumentList "/VERYSILENT", "/NORESTART" -Wait

# Verify installation
git --version
```

**Expected Result:** Should show Git version number

---

## 🐳 Step 2: Install Docker Desktop

1. **Download:** https://docs.docker.com/desktop/install/windows-install/
2. **Install:** Run installer as Administrator
3. **Restart:** Reboot server when prompted
4. **Verify:** Open PowerShell and run: `docker --version`

**Expected Result:** Should show Docker version number

---

## 📁 Step 3: Set Up Project Directory

**Copy and paste these commands:**

```powershell
# Create main directory
mkdir C:\ACE
cd C:\ACE

# Clone your repository
git clone https://github.com/jamesmcmenamin/ACE.git .

# Create required directories
mkdir Content, Dats, Logs-prod, Logs-dev, Mods
```

**Expected Result:** Directory structure created with Git repo cloned

---

## 🎮 Step 4: Add DAT Files

**You need these 4 files in C:\ACE\Dats\:**
- `client_cell_1.dat`
- `client_portal.dat`
- `client_highres.dat`
- `client_local_English.dat`

**Copy them from your Asheron's Call client installation**

---

## 🚀 Step 5: Deploy Development Environment (Test First!)

**Copy and paste these commands:**

```powershell
# Make sure you're in the right directory
cd C:\ACE

# Switch to development branch
git checkout dev
git pull origin dev

# Build and start development environment
docker-compose -f docker-compose.dev.yml up --build -d
```

**Expected Result:**
- Containers build successfully
- Development server starts on ports 9002-9003
- Database initializes automatically

**Test Connection:** `dev.thresholme.online:9002`

---

## 🏭 Step 6: Deploy Production Environment (After Dev Works!)

**Copy and paste these commands:**

```powershell
# Switch to production branch
git checkout master
git pull origin master

# Build and start production environment
docker-compose -f docker-compose.prod.yml up --build -d
```

**Expected Result:**
- Production containers build successfully
- Production server starts on ports 9000-9001
- Separate production database created

**Test Connection:** `play.thresholme.online:9000`

---

## 📊 Step 7: Verify Everything Works

**Check container status:**
```powershell
docker ps
```

**Expected Result:** Should show 4 containers running:
- `ace-server-prod`
- `ace-db-prod`
- `ace-server-dev`
- `ace-db-dev`

**Check logs if needed:**
```powershell
# Production logs
docker-compose -f docker-compose.prod.yml logs -f ace-server-prod

# Development logs
docker-compose -f docker-compose.dev.yml logs -f ace-server-dev
```

---

## 🔄 Daily Operations

### Update Development Server
```powershell
cd C:\ACE
git checkout dev
git pull origin dev
docker-compose -f docker-compose.dev.yml up --build -d
```

### Update Production Server
```powershell
cd C:\ACE
git checkout master
git pull origin master
docker-compose -f docker-compose.prod.yml up --build -d
```

### Stop Everything (Emergency)
```powershell
cd C:\ACE
docker-compose -f docker-compose.prod.yml down
docker-compose -f docker-compose.dev.yml down
```

### Start Everything Back Up
```powershell
cd C:\ACE
docker-compose -f docker-compose.prod.yml up -d
docker-compose -f docker-compose.dev.yml up -d
```

---

## 🆘 Troubleshooting

### Problem: "docker: command not found"
**Solution:** Install Docker Desktop and restart PowerShell

### Problem: "git: command not found"
**Solution:** Install Git for Windows and restart PowerShell

### Problem: Port already in use
**Solution:**
```powershell
# Find what's using the port
netstat -ano | findstr :9000

# Kill the process (replace XXXX with PID)
taskkill /PID XXXX /F
```

### Problem: Container won't start
**Solution:**
```powershell
# Check logs for errors
docker-compose -f docker-compose.prod.yml logs ace-server-prod

# Rebuild from scratch
docker-compose -f docker-compose.prod.yml down
docker-compose -f docker-compose.prod.yml up --build -d
```

### Problem: Database connection failed
**Solution:** Wait 2-3 minutes for database to fully initialize on first run

### Problem: Can't connect from game client
**Check:**
- [ ] Firewall ports 9000-9003 UDP are open
- [ ] DNS records point to correct IP
- [ ] Containers are running (`docker ps`)

---

## 🔒 Security Notes

- **Server auto-login configured** - No Ctrl+Alt+Delete needed
- **Separate databases** - Dev and prod never conflict
- **Tailscale network** - Secure remote access
- **Production settings** - Optimized for your 20-core server

---

## 📈 Performance Notes

**Your server specs (20 cores, 200GB RAM) are MASSIVE for ACE:**
- Production config uses 50% threading allocation
- World database precaching enabled
- Can easily handle 200+ concurrent players
- Development uses conservative settings for debugging

---

## ⚡ Quick Reference Commands

```powershell
# Status check
docker ps

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Restart service
docker-compose -f docker-compose.prod.yml restart ace-server-prod

# Full rebuild
docker-compose -f docker-compose.prod.yml down
docker-compose -f docker-compose.prod.yml up --build -d

# Update code and redeploy
git pull origin master
docker-compose -f docker-compose.prod.yml up --build -d
```

---

**🎯 That's it! Your ACE server should now be running perfectly on both development and production environments.**

**Need help?** Check the logs first, then verify all prerequisites are met.
