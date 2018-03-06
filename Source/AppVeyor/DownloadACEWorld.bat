REM Download latest ACE-World database, extract and import it 
appveyor DownloadFile https://ci.appveyor.com/api/buildjobs/tcv394f0q590n1ik/artifacts/ACE-World-16PY-db-v0.0.5-ixhngyfm.sql.zip
7z x ACE-World-16PY-db-v0.0.5-ixhngyfm.sql.zip
"C:\Program Files\MySql\MySQL Server 5.7\bin\mysql.exe" -h localhost -u root -pPassword12! ace_world < ACE-World-16PY-db-v0.0.5-ixhngyfm.sql
