REM Download latest ACE-World database, extract and import it 
appveyor DownloadFile https://github.com/ACEmulator/ACE-World-16PY/releases/download/v0.0.5/ACE-World-16PY-db-v0.0.5.sql.zip
7z x ACE-World-16PY-db-v0.0.5.sql.zip
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-16PY-db-v0.0.5.sql
