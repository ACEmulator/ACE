#!/usr/bin/env bash
password=$(<password.txt)
mysql -h localhost -u root -p"$password" mysql < ../Base/ShardBase.sql
for sql in ../Updates/Shard/*.sql; do
  [ -e "$sql" ] || continue
  echo "Found file: $sql"
  mysql -h localhost -u root -p"$password" ace_shard < "$sql"
done
rm -f ../Base/ShardBase.sql
rm -f ../Updates/Shard/*.sql
mysqldump --user=root --password="$password" --databases ace_shard --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed "s/ AUTO_INCREMENT=[0-9]*\\b//g" > ../Base/ShardBase.sql
