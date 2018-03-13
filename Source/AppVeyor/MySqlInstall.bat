REM create databases
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < database\Create\create_mysql.sql

REM execute Base Scripts
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\base\ShardBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\base\WorldBase.sql

REM execute Update Scripts for Authentication Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\updates\authentication\changeme.sql
REM FOR /D /r %%G in ("*.sql") DO "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < %%~nxG

REM execute Update Scripts for Shard Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\changeme.sql
FOR /D /r %%G in ("database\updates\shard\*.sql") DO "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < %%~nxG

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql

REM Download latest ACE-World database, extract and import it
Source\AppVeyor\DownloadACEWorld.bat

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql
