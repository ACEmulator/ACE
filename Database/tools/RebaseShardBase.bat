set /p password=<password.txt

"C:\Program Files\MySql\MySQL Server 8.0\bin\mysql.exe" -h localhost -u root -p%password% mysql < ..\Base\ShardBase.sql

REM execute Update Scripts for Shard Database
For /R "..\Updates\Shard\" %%G IN (*.sql) do (
echo Found file: %%G
"C:\Program Files\MySql\MySQL Server 8.0\bin\mysql.exe" -h localhost -u root -p%password% ace_shard < %%G
)

del ..\Base\ShardBase.sql
del ..\Updates\Shard\*.sql

"C:\Program Files\MySql\MySQL Server 8.0\bin\mysqldump.exe" --user=root --password=%password% --databases ace_shard --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed\sed "s/ AUTO_INCREMENT=[0-9]*\b//g" > ..\Base\ShardBase.sql
