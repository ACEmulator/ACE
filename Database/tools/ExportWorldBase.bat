set /p password=<password.txt
del ..\Base\WorldBase.sql
"C:\Program Files\MySql\MySQL Server 8.0\bin\mysqldump.exe" --user=root --password=%password% --databases ace_world --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed\sed "s/ AUTO_INCREMENT=[0-9]*\b//g" > ..\Base\WorldBase.sql
