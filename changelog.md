# ACEmulator Change Log

### 2017-04-29
[Ripley]
* Added @cloak command. Use this to bypass objects blocking your way. Partially implemented as was used in retail.
* Added Translucency to DebugObject. This fixes ghostly npcs.
* Changed GOF and Landblock to key off of ObjectDescriptionFlag first for at least some objects, made lots of changes to Door to allow for simple open/close usage.

### 2017-04-26
[OptimShi]
* Renamed ace_character.character.birth to lastUpdate to better reflect its use. (Birth is already stored in character_properties_int). See Character database update 01_2017_04_26_CharacterLastUpdate.sql
* Changed sort order of character list to use character.lastUpdate instead of alphabetical. Your last used character will now be selected upon a new client instance.

[fantoms]
* Built out the admin `@teleto` command. Use a player's name as the parameter.

[fantoms]
* Added admin `@boot` command logic for Player, Account, and Guid - too kick a player from the server and display a CoC Violation warning message.
* Added Booting capabilites in the `Sesssion` object.
* Added the `GameMessageBootAccount` game message.
* Added an enum for `AccountLookupType`.
* Added `CharacterNameExtension.cs` and included the string helper function `stringArrayToCharacterName`, for converting a string array containing spaces to a player name string.

[Ripley]
* Rebased SQL scripts.
* It is recommended if you have an already existing database you wipe it and start fresh from the new base scripts.
* Download the latest release of the ACE-World database to populate your world.
* Added some exception catching to UniversalMotion.

### 2017-04-25
[MiachofTD]
* Changed 010_2017_04_24_Gharu_town_Part_2_portal_destination.sql to 011_2017_04_24_Gharu_town_Part_2_portal_destination.sql.
* Changed 0010_2017_04_25_portal_destination.sql to 012_2017_04_25_portal_destination.sql and updated MySqlInstall.bat.
* Added 010_2017_04_24_portal_destination_Academy.sql to MySqlInstall.bat.

[Jyard1]
* Added 16 portals around the Shoushi area.

[Og II]
* Added MoveToObject GameMessageUpdateMotion (F74C 0x006)   This is part of a total of 5 variants on the F74C message.
* Can be tested with the new debug command MoveTo <40>  - the parameter is optional.   The default is 30 if you do not put a parameter.   You can test the walk run
* by setting any distance for the wand to spawn under 15.  If you use moveto 10 - your character will walk to the wand.    If you type moveto 90 - he will run a long way.

[MiachofTD]
* Added 30 portals in the destination_portal for Gharu towns.

### 2017-04-24
[OptimShi]
* Added XpTable class to the ACE.DatLoader.FileTypes. This loads the XP tables from the client_portal.dat
* Modified Player.cs to use the new XpTable class.
* Removed JSON based XP charts.
* Removed ACE.Database.ChartDatabase (classes that loaded the JSON XP charts)
* Removed DatReader parameter from DatLoader.FileTypes classes CharGen and GeneratorTable. It's always going to be the same file, so no need to specify it.
* Changed parameters of other DatLoader.FileTypes to be more straight-forward. They will auto-create the DatReader and the parameter is the fileId to read.

[Jyrus]
* Add a few more portals, add a level five restriction to Drudge Hideout, and correct drop point for Tou-Tou Outpost portal - File will not work using PHPMYADMIN
	Must use MySQL, SQLyog, or possibly HeidiSQL
* Move Portal logic to Portal class
* Change OnCollide for portals to route the portals correctly in the four Training Academy dungeon versions and implement level requirements for portals
* Move Lifestone logic from landblock class into Lifestone class OnUse

[Ripley]
* Added currentMotionState to base_ace_object in ace_world database.
* Changed location in base_ace_object to an INT(10) in ace_world database.
* Changed vw_ace_creature_object and vw_ace_object to include currentMotionState.
* Added CurrentMotionState to BaseAceObject and linked it to currentMotionState.
* Added a method to GeneralMotion to convert from a byte array to GeneralMotion object.
* Added DebugObject to assist with building and testing out real objects for world.
* Changed GenericObjectFactory to spawn DebugObjects as a default if running server in DEBUG.
* Fixed Code Style issues within ClothingTable.cs and ClothingBaseEffect.cs in ACE.DatLoader.
* Changed Position to set Z to 0 to fix @tele command.
* Removed PlayScript.Create from landblock respawn section. Players should not have that effect applied to them.

### 2017-04-23
[fantoms]
* Changed the `Character` Stats from the `Current` `ICreatureStats` interface member, too `UnbuffedValue` in attempt at fixing the Player Vitals.
* Added a Health Update game message after the abilities skill spend, too synchronize Player health/stam after spending experience points on `Endurance`.
* `@heal` should now work.

[MiachofTD]
* Added 64 ports in the destination_portal for Gharu towns. 

### 2017-04-22
[OptimShi]
* Changed Manager initalize order in ACE.cs to ensure that the DatLoader is fully loaded before clients can connect since it is required.
* Added GetPaletteId() function to PaletteSet. Helps clean up some of the redundancy in the player loading code.
* Added ClothingTable class to the DatLoader system (0x10 items in the portal.dat) and several related sub-classes.
* Added a PreparedStatement to CharacterDatabase to load the character_starup_gear
* Dressed character in the chosen startup gear. This is currently only cosmetic and unremovable.
* Fixed a bug where certain types of Tumeroks and Undead did not have the right body style (CSetup) applied.

### 2017-04-21
[Jyrus]
* Fixed QX, QY, QZ, and QW mappings to the correct fields in the database to allow facing to work correctly
* Add 88 more working portal destinations, including most of the Town Network

[fantoms]
* Removed `@reset-pos` after it was incorrectly re-added in another members commit.

[StackOverFlow]
* Added Landblock Streaming Objects.

[Ripley]
* Kill `Session.Network.EnqueueSend(new GameMessageUpdateObject(worldObject));` until we get proper movement implemented.

[fantoms]
* Removed `@reset-pos` after it was incorrectly re-added in another members commit.

### 2017-04-22
[Lidefeath]
* Add a DatLoader for the Generators in portal.dat file 0x0E00000D
* Add a method to DatReader to handle obfuscated strings by fliping the low and highbytes
* Add a debugcommand to read the whole file and access one of the generators as an example

### 2017-04-20
[Lidefeath]
* Removed the manual conversion for Model, Texture and PaletteData in Creature.cs
* Fixed BaseAceObject.cs to use byte for ItemCapacity as in the DB

[Mogwai-AC]
* Fixed PackedDWORD logic

[Ripley]
* Filled out Portal object and included ObjScale.
* Changed ModelData.Serialize to use WritePackedDwordOfKnownType for PaletteGuid, palette.PaletteId, texture.OldTexture, texture.NewTexture and model.ModelID.
* Changed IconOverlay and IconUnderlay to use WritePackedDwordOfKnownType.

### 2017-04-19
[Mogwai-AC]
* Added PackedDWORD

[Ripley]
* Changed WeenieClassid and Icon to use PackedDWORD in WorldObject.SerializeCreateObject
* Updated PhysicsData to create and send a new currentMotionState when encountering a null one when flag PhysicsDescriptionFlag.Movement is set.

### 2017-04-18
[Lidefeath]
* Remove the corpse of a creature from the landblock after a calculated time (or a fixed time for testing purposes)
* Respawn the creature in the same spot after the respawn timer has ended
* Spawn randomly generated creatures from DB using the generator tables - these need to be more fleshed out still
* Improved MonsterFactory to create Creatures without saving it as static spawn, to reuse the creation code for generators
* Edit the database table ace_creature_generator_locations to add or change spawn locations with random spawns

[Jyrus]
* Create a new table adding Portal destinations that reference the WeenieClassID of the portal object
* Add an ActionQueue switch case to implement Portal travel

[Mogwai]
* Overhaul of the Object structure.  Removed Mutable and Immutable, added a bunch of new ones: Lifestone, Portal, SummonedPortal, UsableObject, CollidableObject, Container, Creature, and Door and refactored a bunch of stuff to support them all.

### 2017-04-17
[fantoms]
* Removed `@reset-pos` command and also removed another default that set last portal upon character creation.
* Changed `GetPositions` to return a dictionary.
* Changed `Position LandblockId` to have a private value that will create a new objected when needed.
* Moved the position check logic into the CharacterDatabase.cs file, to simply things.

### 2017-04-16
[fantoms]
* Fixed 2 bugs in landblock where the landblock was becoming invalid on load.
* Added checks to prevent missing Positions in the database from crashing the server or preventing character load.
* Removed Lastportal from Position creation, as it will be created on demand when portals are introduced.

### 2017-04-15
[Ripley]
* Updated CommandManager and CommandHandlerAttribute to support description and usage for help commands.
* Added @acehelp and @acecommands (used @Thwargle PR #135 as a reference for listing commands) mimicking @help and @help commands
* Added description and usage information to commands currently implemented.
* Added sending chat messages to inform players upon logging in-game about the existence of ACE specific help.
* Updated GameActionTalk to return better information when a command is invalid or is missing parameters.

[fantoms]
* Added `Queued` Teleporting.
* Added landblock death message broadcasting.
* Added lame static player death messages.
* Now saves last death position in character positions.
* Added Psuedo Event Action for internal ACE `GameActionEvent = 0xF819`.
* Updated `GameEventYourDeath`.
* Added `PlayerKilled` GameMessage type and handling.
* Added `NumDeaths`, `DeathLevel`, and `VitaeCpPool` to character properties.
* Added `PurgeAllEnchantments` GameEvent.
* Updated `Player.Kill()` to update properties.

### 2017-04-14
[Lidefeath]
* Added /testcorpsedrop as debugcommand to kill a creature so it spawns its corpse
* Fix CreatureObject so it's parseable now by aclogview
* Add GameEventDeathNotice to broadcast the killing of a creature
* If a creature is killed - currently only with the smite command - it is removed and a corpse is spawned
* Enhance GameActionQueue to handle delayed actions, so animations have time to play
* Define two GameActionTypes for CreateObject and DeleteObject so they can be used with the delayed GameActionQueue
* Creature Guids now start with 0x90 instead of 0x80 to separate them from items 

### 2017-04-13
[Og II]
* Completed conversion to Action Queue
* Cleaned up a lot of sloppy code
* To test ctw me - drop the wand, pick it back up repeat.

### 2017-04-12
[OptimShi]
* Changed DatDatabase.AllFiles to a Dictionary<uint objectid, DatFile> (was simply a list List<DatFile> previously)
* Added a DatReader class for reading from the a Dat file. Only confirmed for client_portal.dat.
* Added some preliminary client_portal classes (CharGen, SetupModel, PaletteSet)
* Loads correct player data from Character Creation.
* Converted PortalDat.ExtractCategorizedContents to use DatReader for better export of raw files

### 2017-04-10
[Ripley]
* Added MovementEvent Action Queue
* Changed GetNextSequence back to GetCurrentSequence for SequenceType.ObjectInstance
* Animations work again!

[Lidefeath]
* Fix GameActionType.QueryHealth to work across landblocks

### 2017-04-09
[Ripley]
* Changed Position.Serialize to fix rotation issue #222
* Added DelayedTeleport and converted MarketplaceRecall and LifestoneRecall to use it.
* Using a lifestone recall command now uses mana as it did in retail.
* Added animation to using a lifestone.

[fantoms]
* Added visual effect and sound broadcasting in action queue processing.
* Added visual effect broadcasting to `Landblocks`.
* Added `GameActionType` for `ApplySoundEfect` and `ApplyVisualEffect`.

### 2017-04-08
[fantoms]
* Addded sound broadcasting to `Landblocks`.

[Zegeger]
* Added basic motion classes.
* Added current motion state to PhysicsData, which can now be sent in CreateObject messages.
* Updated GameEventUpdateMotion to use these new classes.
* Updated any areas that were affected.
* Basic Combat Stance changing.  Can go into UA.  For some reason, you can't get out right now.

### 2017-04-07
[Lidefeath]
* Added weenie_creature_data for a few drudges.
* Deleted the old view vw_ace_creature_static and replaced it with vw_ace_creature_object, so creature object data and weenie data are in one view.
* Updated WorldDatabase to first read the static creature spawn locations and then the creature data.
* Added a debugcommand 'createstaticcreature <weenieClassId>' to spawn a creature, add it to the landblock and save it in ace_creature_static_locations.
* Replace HandleQueryHealth in Landblock and LandblockManager with the new GameActionQueue mechanic.
* Hint: you need to import database/updates/world/005_vw_ace_creature_object.sql to get this all working.

### 2017-04-06
[fantoms]
* Reversed the logic for distance checking, in the Queued Action code that @Mogwai introduced today on Item Usage for `Landblocks` owned Lifestones. 
* Changed the Lifestone usage text to be `Light Blue`.
* Added `YourDeath` GameEvent.
* Added `Die` GameAction.
* Added `Player.Kill()` function, too initiate a player death.
* Added `kill` admin/debug command for killing a player on the server.
* Updated `SetCharacterPosition`, too set the `characterId` and `LandblockId`, when possible.
* Added a message to @ls/@lifestone informing you, why you can't travel when if a Sanctuary Location has not been saved in the database.

[Zegeger]
* Got 9 CreateObject sequences in correct order and named according to client code.
* Changed many sequences over to SequenceManager.

### 2017-04-05
[fantoms]
* Reworked Character `Positions`.
* `Player.Location`, `WorldObject/ImmutableWorldObject.Location`, and `Character.Location` are now the `Position` instances for the respected Objects.
* Fixed `DBDEBUG` constant output in `Database`.
* Simplified the `SetCharacterPosition` function.
* Refactored `GetCharacterPositions` and added `GetCharacterPosition` (single get).
* For function clarity, I updated the function name for `SaveCharacterPosition` in the Character Database Interface.
* Fixed default a position issue by taking out values for an `InvalidPosition`.
* Added a SIMPLE Lifestone `WeenieClassIds` enum, to check against when determining a lifetone object's capabilities. TODO: This should be changed when UseItem is built out further.
* Updated comments on PostionTypes from @ogmage78's lifestonetie branch.
* Added GameAction for `TeleToLifestone` and `UseItem`.
* Added GameEvent for `UseDone` and `TeleToLifestone` allowing @ls or @lifestone commands to work.
* Added lifestone binding capabilities. 
* Added data to the Character position that comes from the client in `GameActionMoveToState` and set the `Location` position type.

[Lidefeath]
* Fixed the creature data in the Database scripts, so the Drudge can finally spawn

### 2017-04-04
[Mogwai]
* Added AppVeyor integration to build and run unit tests in ACE.Tests
* Rebased all Database scripts
* Updated readme to have FAQ section

[Mag-nus]
* Fixed the stuff Mogwai missed and/or broke.

[Zegeger]
* Fixed inbound packet combining

### 2017-04-03
[Lidefeath]
* Add basic creature spawn tables to the database
* Load a static Drudge Sneaker Spawn near the Holtburg LS in Landblock.cs

[Jyrus]
* Add changes to ACE-World ace_object and weenie tables to widen the indexing restrictions to allow more fields to be used for unique indexing
* Add Portal weenie and object table data

[Mogwai]
* Velocity vector fixed
* Fixed protection level of weenie class in mutable world object.

### 2017-04-02
[StackOverflow]
* SpellFactory + World Object Fixes and a real Vector3  - Improvements- AceVector

### 2017-03-31
[fantoms]
* Added debug command to test positions.
* Changed SQL syntax to use BEFORE instead of AFTER for the `positionType` column, in the `character_positions` table.
* Removed the github friendly enums from the `PositionTypes`.
* Changed a log4net debug line.
* Changed the name class name `CharacterPositionType` to `PositionType`
* Reworked `Position.cs` with ORM.

### 2017-03-30
[StackOverflow]
* Start of a SpellFactory + World Object Fixes and a real Vector3 - AceVector

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
+ Updates after PR review: +
* Changed the `LoadCharacterPositions` to ORM OUT of the database.
* Changed the introduced @save command to @save-now to avoid conflict
* Changed CharacterPositionType to PositionTypes and merged in Ripley's enums
* Changed PhyiscalPosition to Location
* Removed plural in function name for position types; broke the character loading
* Added database view for all list positions
* Added GetList statement for all positions
* Migrated GetPosition to GetLocation, Seporated Logic, added ORM calls on selecting data
* Moved the Character.Location to Character.CharacterPositions[PositionType.Location] dictionary

[Mogwai]
* Added changelog.md to clearly identify changes from 1 commit to another.
* Updated readme.md.

### 2017-03-28
[Jyrus]
* Fixed remaning stylecop warnings

[Lidefeath]
* Added UpdateHealth GameAction and Event for Players

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


