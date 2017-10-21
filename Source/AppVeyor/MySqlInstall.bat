REM create databases
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < database\Create\create_mysql.sql

REM execute Base Scripts
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\base\ShardBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\base\WorldBase.sql

REM execute Update Scripts for Authentication Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\updates\auth\changeme.sql

REM execute Update Scripts for Shard Database
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\2017-08-20-00-vw-ace-wielded-object.sql

REM Download latest ACE-World database, extract and import it
appveyor DownloadFile https://github.com/ACEmulator/ACE-World/releases/download/v0.2.5/ACE-World-db-v0.2.5.sql.zip
7z x ACE-World-db-v0.2.5.sql.zip
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-db-v0.2.5.sql

REM execute Update Scripts for World Database
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2017-06-30-00-generator-chains-testdata.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\2017-10-18-00-create-ace-object-tracker.sql

