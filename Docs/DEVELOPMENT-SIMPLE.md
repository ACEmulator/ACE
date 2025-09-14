# 🔧 DUMMY-PROOF Development Workflow

## ⚠️ GOLDEN RULE ⚠️
**SERVERS NEVER COMMIT CODE - ONLY DEVELOPMENT MACHINES DO**

- 💻 **Development Machine**: Write code, commit, push to GitHub
- 🖥️ **Development Server**: Pull code, deploy, test
- 🏭 **Production Server**: Pull code, deploy for players

**SERVERS ARE DEPLOYMENT TARGETS ONLY!**

---

## 🚀 Development Workflow (Copy & Paste Ready)

### Step 1: Work on Your Development Machine

```bash
# Switch to development branch
git checkout dev
git pull origin dev

# Make your changes to the code
# ... edit files ...

# Commit and push changes
git add .
git commit -m "Description of what you changed"
git push origin dev
```

### Step 2: Test on Development Server

**On the server (100.105.32.14):**

```powershell
cd C:\ACE

# Pull latest dev changes
git checkout dev
git pull origin dev

# Deploy to test
docker-compose -f docker-compose.dev.yml up --build -d

# Check if it worked
docker ps
```

**Test at:** `dev.thresholme.online:9002`

### Step 3: Promote to Production (When Ready)

**On your development machine:**

```bash
# Merge dev into master
git checkout master
git merge dev
git push origin master
```

**On the server (100.105.32.14):**

```powershell
cd C:\ACE

# Pull production changes
git checkout master
git pull origin master

# Deploy to production
docker-compose -f docker-compose.prod.yml up --build -d
```

**Live at:** `play.thresholme.online:9000`

---

## 🎯 Smart Docker Setup Explained

### How Configs Work Now

**No more manual file copying!** Docker automatically handles configs:

- **Production build** → Uses `Config.js.prod` automatically
- **Development build** → Uses `Config.js.dev` automatically

### What Happens During Build

1. **You run:** `docker-compose -f docker-compose.prod.yml up --build -d`
2. **Docker uses:** `Dockerfile.prod`
3. **Docker copies:** `Config.js.prod` → `Config.js` inside container
4. **Result:** Production container has production config baked in

**Same process for development but with dev files.**

### Why This Is Better

❌ **Old way:** Manual config copying (error-prone)
✅ **New way:** Docker handles it automatically (foolproof)

---

## 📁 Project Structure

```
C:\ACE\                    # Single directory on server
├── Source\               # Your code (switches between branches)
├── Config.js.prod        # Production settings
├── Config.js.dev         # Development settings
├── docker-compose.prod.yml
├── docker-compose.dev.yml
├── Dockerfile.prod       # Builds prod container
├── Dockerfile.dev        # Builds dev container
├── Content\              # Shared game content
├── Dats\                # Shared DAT files
├── Logs-prod\           # Production logs
├── Logs-dev\            # Development logs
├── db-data-prod\        # Production database
└── db-data-dev\         # Development database
```

---

## 🔄 Common Development Tasks

### Quick Development Update
```powershell
# On server - test latest dev changes
cd C:\ACE
git checkout dev && git pull origin dev
docker-compose -f docker-compose.dev.yml up --build -d
```

### Emergency Production Rollback
```powershell
# On development machine - revert to previous commit
git checkout master
git reset --hard HEAD~1
git push origin master --force

# On server - deploy the rollback
cd C:\ACE
git checkout master && git pull origin master
docker-compose -f docker-compose.prod.yml up --build -d
```

### Check What's Running
```powershell
# See all containers
docker ps

# Check specific logs
docker-compose -f docker-compose.prod.yml logs -f ace-server-prod
docker-compose -f docker-compose.dev.yml logs -f ace-server-dev
```

### Restart After Code Changes
```powershell
# Development
docker-compose -f docker-compose.dev.yml restart ace-server-dev

# Production
docker-compose -f docker-compose.prod.yml restart ace-server-prod
```

---

## 🐛 Development Environment Features

### Development Server Has:
- **World Name:** "Thresholme-Dev"
- **Ports:** 9002-9003 (external)
- **Admin Access:** Level 5 by default
- **Debug Logging:** Detailed errors shown
- **Fast Startup:** No database precaching
- **Unlimited Sessions:** No IP limits for testing

### Production Server Has:
- **World Name:** "Thresholme"
- **Ports:** 9000-9001 (external)
- **Admin Access:** Level 0 by default
- **Security:** Production-grade settings
- **Performance:** Database precaching enabled
- **Session Limits:** 5 per IP, 256 total

---

## 🔍 Debugging Tips

### View Real-Time Logs
```powershell
# Development logs
docker-compose -f docker-compose.dev.yml logs -f ace-server-dev

# Production logs
docker-compose -f docker-compose.prod.yml logs -f ace-server-prod
```

### Connect to Database
```powershell
# Development database
docker exec -it ace-db-dev mysql -u acedockeruser -p
# Password: 2020acEmulator2017

# Production database
docker exec -it ace-db-prod mysql -u acedockeruser -p
# Password: 2020acEmulator2017
```

### Check Container Health
```powershell
# See container status
docker ps

# Check resource usage
docker stats

# Inspect specific container
docker inspect ace-server-prod
```

---

## ⚡ Quick Commands Reference

```powershell
# Update and deploy dev
cd C:\ACE && git checkout dev && git pull origin dev && docker-compose -f docker-compose.dev.yml up --build -d

# Update and deploy prod
cd C:\ACE && git checkout master && git pull origin master && docker-compose -f docker-compose.prod.yml up --build -d

# Stop everything
docker-compose -f docker-compose.prod.yml down && docker-compose -f docker-compose.dev.yml down

# Start everything
docker-compose -f docker-compose.prod.yml up -d && docker-compose -f docker-compose.dev.yml up -d

# View all logs
docker-compose -f docker-compose.prod.yml logs & docker-compose -f docker-compose.dev.yml logs
```

---

## 🚨 What NOT To Do

❌ **Don't commit code on the server**
❌ **Don't edit config files manually**
❌ **Don't skip testing in dev before prod**
❌ **Don't deploy directly to production**
❌ **Don't mix up the docker-compose files**

✅ **Always develop on dev machine**
✅ **Always test in dev environment first**
✅ **Always let Docker handle configs**
✅ **Always pull before deploying**
✅ **Always check logs if something breaks**

---

**🎯 Follow this workflow and you'll never have deployment issues!**
