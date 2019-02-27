Note that most of these are the same instructions for installation @ https://github.com/ACEmulator/ACE, with some Linux specifics

1. Install .NET Core SDK
https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-2.2.104

2. Install MySQL or MariaDB
  - MySQL minimum required version - 5.7.17+
  - MariaDB minimum required version - 10.2+

3. Clone the project with git:
  - git clone https://github.com/ACEmulator/ACE.git
  
4. In ACE/Source/ACE.Server, copy Config.js.example to Config.js, and modify settings such as DAT folder, passwords, and other server settings.

5. In ACE/Source, run 'dotnet build'

6. Load all base SQL table structures found in the Database/Base folder: AuthenticationBase.sql, WorldBase.sql, and ShardBase.sql

7. Load all incremental SQL updates found in the Database/Updates/Authentication folder in the order of oldest to newest. Skip this step if there are no updates in this directory.

8. Load all incremental SQL updates found in the Database/Updates/Shard sub directory in the order of oldest to newest. Skip this step if there are no updates in this directory.

9. Download the latest release of the World DB from https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest and install the ace_world db

10. In ACE/Source/ACE.Server/bin/x64/Debug/netcoreapp2.1, run the server with 'dotnet ACE.Server.dll'

11. Create your first account as an admin at the ACE prompt: "accountcreate testaccount testpassword 5"

12. Launch acclient directly with this command: "acclient.exe -a testaccount -v testpassword -h your-server-address:9000"
