REM create databases
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < database\Create\create_mysql.sql

REM execute Base Scripts
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\base\ShardBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\base\WorldBase.sql

REM execute Update Scripts for Authentication Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\updates\auth\changeme.sql

REM execute Update Scripts for Shard Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\changeme.sql

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-06-00-generator-profiles-table.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-06-01-landblock-links.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-07-00-inventory-shade.sql

REM Download latest ACE-World database, extract and import it
appveyor DownloadFile https://github.com/ACEmulator/ACE-World-16PY/releases/download/v0.0.2/ACE-World-16PY-db-v0.0.2.sql.zip
7z x ACE-World-16PY-db-v0.0.2.sql.zip
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-16PY-db-v0.0.2.sql

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-06-00-generator-profiles-table.sql
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-06-01-landblock-links.sql
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\2018-01-07-00-inventory-shade.sql
