#
# Utility script to create MySql databases required by ACE
#
# Place this .ps1 in the root DATABASE folder in the solution directory.
# Remember to update server/user/password variables in config.json
#
# Created By Brian Mitchell
# 2017-2-13
#

[void][system.reflection.Assembly]::LoadWithPartialName("MySql.Data")

# Ask for elevated permissions if required
If (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]"Administrator")) {
    Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
    Exit
}

#Parse config file to object
$config = (Get-Content ..\Source\ACE\config.json) -join "`n" | ConvertFrom-Json

$basescriptpath = ".\Base\"
$authupdatescriptpath = ".\Updates\Authentication\"
$worldupdatescriptpath = ".\Updates\World\"
$characterupdatescriptpath = ".\Updates\Character\"

# Create schemas in MySql
# Auth
$connStr = "server=" + $Config.MySql.Authentication.Host + 
		   ";Port=" + $Config.MySql.Authentication.Port + 
		   ";Persist Security Info=false;user id=" + $Config.MySql.Authentication.Username + 
		   ";pwd=" + $Config.MySql.Authentication.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

# Drop
$cmd.CommandText = "DROP DATABASE IF EXISTS " + $Config.MySql.Authentication.Database
$cmd.ExecuteNonQuery()

# Create
$cmd.CommandText = 'CREATE SCHEMA `' + $Config.MySql.Authentication.Database + '`'
$cmd.ExecuteNonQuery()

$conn.Close()

# World
$connStr = "server=" + $Config.MySql.World.Host + 
		   ";Port=" + $Config.MySql.World.Port + 
		   ";Persist Security Info=false;user id=" + $Config.MySql.World.Username + 
		   ";pwd=" + $Config.MySql.World.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

# Drop
$cmd.CommandText = "DROP DATABASE IF EXISTS " + $Config.MySql.World.Database
$cmd.ExecuteNonQuery()

# Create
$cmd.CommandText = 'CREATE SCHEMA `' + $Config.MySql.World.Database + '`'
$cmd.ExecuteNonQuery()

$conn.Close()

# Character
$conn = "server=" + $Config.MySql.Character.Host + 
		";Port=" + $Config.MySql.Character.Port + 
		";Persist Security Info=false;user id=" + $Config.MySql.Character.Username + 
		";pwd=" + $Config.MySql.Character.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

# Drop
$cmd.CommandText = "DROP DATABASE IF EXISTS " + $Config.MySql.Character.Database
$cmd.ExecuteNonQuery()

# Create
$cmd.CommandText = 'CREATE SCHEMA `' + $Config.MySql.Character.Database + '`'
$cmd.ExecuteNonQuery()

$conn.Close()

# Run data scripts
# Auth
$connStr = "server=" + $Config.MySql.Authentication.Host + 
		   ";Port=" + $Config.MySql.Authentication.Port + 
		   ";Database=" + $Config.MySql.Authentication.Database + 
		   ";Persist Security Info=false;user id=" + $Config.MySql.Authentication.Username + 
		   ";pwd=" + $Config.MySql.Authentication.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()		

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

$sql = (Get-Content -path ($basescriptpath + "AuthenticationBase.sql")) -join "`r`n"

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.CommandText = $sql
$cmd.Connection  = $conn
$cmd.ExecuteNonQuery()

$updates = Get-ChildItem $authupdatescriptpath -Filter *.sql | Sort-Object

for ($i=0; $i -lt $updates.Count; $i++) {
	$sql = (Get-Content -path ($authupdatescriptpath + $updates[$i])) -join "`r`n"
	$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
	$cmd.CommandText = $sql
	$cmd.Connection  = $conn
	$cmd.ExecuteNonQuery()
}

$conn.Close()

# World
$connStr = "server=" + $Config.MySql.World.Host + 
		   ";Port=" + $Config.MySql.World.Port + 
		   ";Database=" + $Config.MySql.World.Database + 
		   ";Persist Security Info=false;user id=" + $Config.MySql.World.Username + 
		   ";pwd=" + $Config.MySql.World.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

$sql = (Get-Content -path ($basescriptpath + "WorldBase.sql")) -join "`r`n"

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.CommandText = $sql
$cmd.Connection  = $conn
$cmd.ExecuteNonQuery()

$updates = Get-ChildItem $worldupdatescriptpath -Filter *.sql | Sort-Object

for ($i=0; $i -lt $updates.Count; $i++) {
	$sql = (Get-Content -path ($worldupdatescriptpath + $updates[$i])) -join "`r`n"
	$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
	$cmd.CommandText = $sql
	$cmd.Connection  = $conn
	$cmd.ExecuteNonQuery()
}

$conn.Close()

# Character
$connStr = "server=" + $Config.MySql.Character.Host + 
		   ";Port=" + $Config.MySql.Character.Port + 
		   ";Database=" + $Config.MySql.Character.Database + 
		   ";Persist Security Info=false;user id=" + $Config.MySql.Character.Username + 
		   ";pwd=" + $Config.MySql.Character.Password + ";"

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection  = $conn

$sql = (Get-Content -path ($basescriptpath + "CharacterBase.sql")) -join "`r`n"

$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.CommandText = $sql
$cmd.Connection  = $conn
$cmd.ExecuteNonQuery()

$updates = Get-ChildItem $characterupdatescriptpath -Filter *.sql | Sort-Object

for ($i=0; $i -lt $updates.Count; $i++) {
	$sql = (Get-Content -path ($characterupdatescriptpath + $updates[$i])) -join "`r`n"
	$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
	$cmd.CommandText = $sql
	$cmd.Connection  = $conn
	$cmd.ExecuteNonQuery()
}

$conn.Close()