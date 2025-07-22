#!/usr/bin/env bash
password=$(<password.txt)
rm -f ../Base/AuthenticationBase.sql
docker exec ace-db mysqldump --user=root --password="$password" --databases ace_auth --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed "s/ AUTO_INCREMENT=[0-9]*\\b//g" > ../Base/AuthenticationBase.sql
echo >> ../Base/AuthenticationBase.sql
cat AuthenticationBase/*.sql >> ../Base/AuthenticationBase.sql
