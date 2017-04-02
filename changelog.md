# ACEmulator Change Log
### 2017-03-29
[fantoms]
* Changed the Player.Position to Player.PhysicalPosition to match the Developer meeting video and Issue #166.
* Created `CharacterPostion` and `CharacterPositionType` classes to store the Database types requested in Issue #166.
* Added `CharacterPostion` ORM logic.
* Built out helper extension functions to serve the `StartingLocation`, until we pull from the database. Also included an Invalid location as a default (0).
* Changed the `character_positions` table and Added `positionType` column.
* Set the primary key to include `positionType` column.
* Added `positionType` to required Critera for `character_positions`.
* Added `CharacterPositionInsert` and `CharacterPositionUpdate` to the Character Database.
* Created `CharacterPositionInsert` and `CharacterPositionUpdate` prepared SQL statements by utilizing the `ConstructStatement` function.
* Added the required `positionType` clause to the `CharacterPositionSelect` prepared statement.
* Added a position update statement for updating the player position.
* Reworked GetPosition to include the `CharacterPositionType`.
* Added function for getting a `CharacterPostion` from the database, requires a character Id and `CharacterPositionType`.
* Added logic to get location function to insert a location into the database, if not present.
* Reworked the UpdateCharacter and Update functions to save positions.
* Added functions for inserting new character positions on character creation.
* Added functions for saving/loading the character positions from the database on joining the World.
* Attempted to get log4net working properly and moved some code to logging.
* Added DBDEBUG project constant for debugging the database calls.
* Changed the `CharacterPositionSelect` prepared statement to a `ConstructStatement`.

[Mogwai]
* Added changelog.md to clearly identify changes from 1 commit to another.
* Updated readme.md.

### 2017-03-28
[Jyrus]
* Fixed remaning stylecop warnings

### 2017-03-27
[fantoms]
* SQL syntax for inserts in statement construction

[Og II]
* Fixed bug in F748: Set Position and Motion

### 2017-03-26
[Miach, fantoms]
* Check for null welcome message

[OptimShi]
* Fixed Model write order and some spelling errors

[fantoms]
* Renamed the new db statement constructor label

### 2017-03-25
[Mogwai]
* weenies weenies everywhere. object structure pass 1
