"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! mysql < database\Create\create_mysql.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_auth < database\base\AuthenticationBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_character < database\base\CharacterBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\base\WorldBase.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\005_vw_ace_creature_object.sql
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < database\updates\world\006_2017_04_18_portal_locations.sql
