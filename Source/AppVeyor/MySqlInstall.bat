REM create databases
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < database\Create\create_mysql.sql

REM execute Base Scripts
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\base\ShardBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\base\WorldBase.sql

REM execute Update Scripts for Authentication and Shard Databases
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\updates\authentication\changeme.sql
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\changeme.sql

REM curl -O http://www.openss7.org/repos/tarballs/strx25-0.9.2.1.tar.bz2

REM $latestTag=$(git describe --abbrev=0 --tags)
REM set latestTag=$(git ls-remote --tags https://github.com/ACEmulator/ACE-World.git | awk '{print $2}' | grep -v '{}' | awk -F"/" '{print $3}' | sort -n -t. -k1,1 -k2,2 -k3,3 | tail -n 1.chomp)

REM echo latestTag
REM curl -L https://github.com/reactiveui/ReactiveUI/releases/download/$latestTag/ReactiveUI-$latestTag.zip

REM curl -o c:\projects\ace\ACE-World.zip https://github.com/ACEmulator/ACE-World/releases/download/v0.1.4/ACE-World-db-v0.1.4.sql.zip

appveyor DownloadFile https://github.com/ACEmulator/ACE-World/releases/download/v0.1.4/ACE-World-db-v0.1.4.sql.zip

REM 7z x c:\projects\ace\ACE-World.zip "-oc:\projects\ace" 
7z x ACE-World-db-v0.1.4.sql.zip

REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < c:\projects\ace\ACE-World-db-v0.1.4.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-db-v0.1.4.sql

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\04-06-24-2017-generator-testdata.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\06-06-30-2017-generator-chains-testdata.sql
