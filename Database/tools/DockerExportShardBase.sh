#!/usr/bin/env bash
password=$(<password.txt)
rm -f ../Base/ShardBase.sql
docker exec acedb mysqldump --user=root --password="$password" --databases ace_shard --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed "s/ AUTO_INCREMENT=[0-9]*\\b//g" > ../Base/ShardBase.sql
