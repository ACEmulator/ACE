#!/usr/bin/env bash
password=$(<password.txt)
mysql -h localhost -u root -p"$password" mysql < ../Base/WorldBase.sql
for sql in ../Updates/World/*.sql; do
  [ -e "$sql" ] || continue
  echo "Found file: $sql"
  mysql -h localhost -u root -p"$password" ace_world < "$sql"
done
rm -f ../Base/WorldBase.sql
rm -f ../Updates/World/*.sql
mysqldump --user=root --password="$password" --databases ace_world --add-drop-database --add-drop-table --create-options --quote-names --lock-tables --dump-date --flush-privileges --set-gtid-purged=AUTO --disable-keys --tz-utc --add-locks --extended-insert --opt --no-data | sed "s/ AUTO_INCREMENT=[0-9]*\\b//g" > ../Base/WorldBase.sql
