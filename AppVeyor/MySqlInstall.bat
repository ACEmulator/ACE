REM create databases
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < Database\Create\create_mysql.sql

REM execute Base Scripts
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < Database\Base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < Database\Base\ShardBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < Database\Base\WorldBase.sql

REM execute Update Scripts for Authentication Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\updates\authentication\changeme.sql
For /R "Database\Updates\Authentication\" %%G IN (*.sql) do (
REM echo Found file: %%G
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < %%G
)

REM execute Update Scripts for Shard Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < database\updates\shard\changeme.sql
For /R "Database\Updates\Shard\" %%G IN (*.sql) do (
echo Found file: %%G
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_shard < %%G
)

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql

REM Download latest ACE-World database, extract and import it
call AppVeyor\DownloadACEWorld.bat

REM execute Update Scripts for World Database
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\changeme.sql
REM For /R "database\updates\world\" %%G IN (*.sql) do (
REM echo Found file: %%G
REM "C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < %%G
REM )
