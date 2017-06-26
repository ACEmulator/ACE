# ACEmulator Change Log

### 2017-06-26
[Jyrus]
* Add protection to the SpawnPortal method, so any old ushort cannot be used for the weenieclassID that it is expecting

### 2017-06-25
[OptimShi]
* Re-enabled the "equiptest" debug command that was inadvertently disabled.

### 2017-06-24
[Lidefeath]
* Remove old creature spawning and generator stuff
* Implement creature spawning from generator objects, nested or hierarchical generators are still to do

[Ripley]
* Rebased SQL scripts.

### 2017-06-23
[Jyrus]
* Remove unneeded debug command CreateLifestone
* Modified PortalObjectFactory class to create specialized portals that are temporary spawns, such as the Humming Crystal Portal
* Created enums for the weenieclassID's for the special portals
* Added the missing ObjectType to the CollidableObject class

[ddevec]
* Fix to teleport freezing characters in place (Wasn't clearing requested location)
* Fix to occasional crash when moving long distances (Wasn't properly handling landblock transferring)
* Fix to glitch in movement when exiting teleport (was setting position when I shouldn't have been)

### 2017-06-22
[fantoms]
* Added `@set-shutdown-interval` command, to change the delay on the fly.
* Added `@shutdown`, `@stop-now`, and `@cancel-shutdown` commands.
* Added server shutdown command for admins - needed in consoles.
* Added shutdown text, logoff, admin shutdown message.
* Added `ShutdownInterval` to the `ConfigManager` Server Section, with a default of 60 seconds.
* Added `ShutdownInterval` to the example config
* Added some logging and moved the final exit too `ServerManager`.
* Added a message broadcast through Session, to announce server broadcast messages from `WorldManager`.
* Changed the case of the Config.json in the project build events, to match a Linux use case requirement for opening the Config file.

### 2017-06-21
[ddevec]
* Fix logoff crash in core/landblock restrucutre due to nulliing a location then
      moving an object
* Also reintroduced learnspell       
* Major overhaul to core/landblock structure.  Details can be found: (https://github.com/ACEmulator/ACE/pull/398#partial-pull-merging)
* Added actor/action interface
* Restrucutred parallelism in WorldManager
* Added broadcast system
* Added plugins for what will hopefully one day be a physics system
* Minor fixups to player tracking
* Forced character creation to wait for DbManager.SaveObject() to finish saving the character to avoid a race condition on character creation causing crashing
* Changed DbManager functionality to more cleanly allow shutdown.
* KNOWN BUG: On-death player often doesn't emerge from portal space

### 2017-06-20
[StackOverflow]
* Moved Saving operations outside of primary game loop to prevent db operations from slowing down primary game loop.
* Please call DbManager.SaveObject() now instead of DatabaseManager.Shard.SaveObject()

### 2017-06-19
* Trello card task - Extend ORM code to support multiple getLists or implement another method to query by alternate keys - re-implement cirand
* Many thanks to ddevec - he did the work on the ORM code revision.   I just tested and implemented for my use.
* Fixed undiscovered alignment bug at hooktype we were sending as dword and it is a word.   That was shifting everything below it.
* Added priority to aceObject and set the aceObject priority field as a backing object for worldObject priority.
* Created function to calculate container burden.
* added view to world database that was needed for cirand and will be used in loot generation.
* updated position table to have a faster function for location per discord discussion.
* Updated trello board to refect current project status.

### 2017-06-18
[Ripley]
* Made changes to AceObject, PhysicsData, DebugObject and UsableObject to make wielded items in database work better.
* Note that PhysicsData.Children does not set properly yet but is seemingly not needed for the effect to work at least for static npcs/items.

### 2017-06-17
[fantoms]
* Added the debug command `listplayers`, that will list all players currently connected too the server.
* Created a `DefaultValue` attribute for `MaximumAllowedSessions` of 128.
* Added `MaximumAllowedSessions` to the `ConfigManager` and the `Config.json.example` file.
* Began using the variable in the `WorldManager.cs` server initalization step, allowing usersr to configure the max allowed sessions.

[ddevec]
* Reintroduced CreatureVital patch from pre-overhaul-master
* Adds setvital helper function
* Adds Tick function to handle vital regeneration (will hopefully be removed with core restructure)
* Separates CreatureVital from CreatureAbility.

[ddevec]
* Cleaned up player creation.
* Fixed naming issue on player creation.
* Fixed saving/loading of several AceObject properties.

[Ripley]
* Made changes to WorldBase and ShardBase scripts to correct issues with landblocks and POIs. 
* Changed CachedWordObject to CachedWorldObject.
* Changed CachedWorldObject.Landblock from ushort to int.
* Changed CharacterBase and ace_character references to ShardBase and ace_shard in README.
* Altered AppVeyor SQL install batch file to execute proper scripts.

[OptimShi]
* Added functionality for the GameMessageObjDescEvent message (fired when a model changes, like when equipping new items). Also included a debug command "@equiptest" to expose the new functions which will cosmetically equip your character with a single piece of armor/clothing (only cosmetic, no actual "equipping" is being done at this time)

### 2017-06-16
[OptimShi]
* Added SpellTable/SpellComponent parsing from portal.dat. Also added "@learnspell" debug command and corresponding UpdateSpell Event. (Added back in after OO merge)
* Finished parsing the client_cell.dat file with the CLandblockInfo type (xxyyFFFE files), along with supporting classes. This makes the client_cell.dat reading complete. (Added back in after OO merge)
* Fixed a bug in the DatLoader.FileType.PaletteSet where loading a cached palette set could crash.

[Og II]
* Trello card task - delete character crashes server.   Fixed this issue.
* Added event code for database to do the actual house keeping to flip the flag once the hour restore period has expired.
* Minor code cleanup.
* Removed update directory for old character database.
* Fixed pickup and drop item.   Location was protected and not able to be set for loot (WorldObjects)   I temp set this allow set.   Once we refactor physicsData out this can go away.


### 2017-06-15
[ddevec]
* Refactor of AceObject
* Player/Creature/WorldObject persistent attirbutes now forward to AceObject
* Fixed Attribute2nd initailization error in the process.

### 2017-06-14
[ddevec]
* Fixed Position saving bug.
* Added intial Level, TotalExperience, and AvailiableExperience to AceCharacter
*    Fixes level ??? bug.
* Fixing Attributes2nd (health/stam/mana) intentionally delayed until merge with master -- master includes needed restructuring

[Og II]
* Trello card - General code clean up - look at all the TODO's -  add the most critical ones here
* Removed unused and replace weenie code that was deprecated with Object-Overhaul
* Fixed hair color bug with code from OptimShi - this should conclude our issues with character appearance.
* Removed unused files related to weenie.
* I spent about an hour trying to debug the position not saving.   None of the character save is working.   It does not error,
* but nothing is changed in the DB.    That is still to-do.
* Updated Trello board.
* I cleaned up the shard base script but I have left it out due to the contentious nature of the discussion in the apply named discord ;)
* Added first part of equip item.

### 2017-06-13
[Og II]
* Added getting and setting ContainerID and Wielder into AceObject and surfaced in worldObject
* Put in a hack to show how equiped weapons and shields work.   We will need to establish a place to store this data.
* I put in big TODO to indentify the hack - it is benign and will not impact anything else other than to demo the placement
* of the spear in Rand the Game Hunters hand in Holtburg.
* Modified SetPhysicsDescriptionFlags so it can be container and weilder aware.
* Worked with OptimShi and we put in a fix for Lord BucketHead.

### 2017-06-12
[Og II]
* fixed issue with null exception
* Added OptimShi code to load the correct player apperiance 
* Added sending the DataID and InstanceID fields on the player description event.
* Fixed assorted bugs.

### 2017-06-11
[Og II]
* fixed issues with the weenieHeaderFlags setting method, fixed PhysicsDescriptionFlag setting issue as well.
* cleaned up and rebased data scripts for WorldBase and ShardBase
* Found issue in my data that was ETL to our new schema - had two flag fields Reversed - that is fixed but would be no issue
* once we have Ripley's new data export.   I have shared out a link to the cleaned up data for use while we finish the new export.
* https://www.dropbox.com/s/pohcruvalt9s38h/WorldandShardData.zip?dl=0
* I put in a nasty hack - hard coded weenieClassId on character save - it was being set to 0 and I could not find it.   I marked with with a todo.
* player.cs line 490

[ddevec]
* Somewhat cleaned up CreatureAbility and CreatureSkill in-game and backend separation
* Cleaned up Position loading for players and objects
* Now logs into game.
* Landlbocks now load objects from the object view to load by landblock
* Fixed several ORM data load issues
* Fixed bugs with character updating
* Fixed player position saving -- position updated on relogging
* Player description packet now being sent.
* Known issues:
*   Race in ShardDatabase gathering next character id (Commented)

### 2017-06-10
[Og II]
* Continued work loading character from ace_shard.   Character reaches world now. 
* Fixed a number of null exception errors
* Outstanding issues:
* has an orm data load error 
* abilities and skills object issue not resolved
* palette, textures not being read or sent

### 2017-06-08
[ddevec]
* Implemented ShardDatabase.SaveObject
* Added DatabaseTransaction functions for InsertList, DeleteList to handle object properties
* Minor fixes to ORM attributes
* Reworked DbGetList Attribute to DbList Attribute -- now specifies "keys" designating a list in the DB.

### 2017-06-07
[Og II]
* Continued work loading character from ace_shard.   Rebased 
* Added cascade delete to all child tables in ace_shard.
* Completed and tested up to skillz loading.   Made a few changes on the db side for key consistancy.
* TODO items:
*	Finish up loading - spells, friends, allegiance info, spell comps, spell bars etc. etc....

### 2017-06-06
[OptimShi]
* Read character creation values from client_portal.dat and assign to appropriate Character Properties.

[Og II]
* Intital work done for loading character from ace_shard.   Rebased and removed update sql files.
* Tested up to position loading.   Made changes to position, but have not loaded the dictionary yet.
* Fixed a few bugs - attribute loading had a few.
* TODO items:
*	Finish up loading positions then see what is left. 
*	Work on saving as OptimShi - has character creation ready to go.   

### 2017-05-21
[Og II]
* Intital work done for new schema.   Rebased and removed update sql files.
* Tested and the auto setting of Physics and Weenie Header Flags looks really solid.   Side note, the values we have
* in the database for weenieHeaderFlags and PhysicsDescriptionFlag should probably be dropped we don't read them or use them now.
* Doors, portals NPC's and signs are all back in the world now.
* I would suggest we remearge with the main branch sooner rather than later - we can create another branch to introduce these
* changes into the character side.
* TODO items:
*	Creature spawns need to be refactored - I left them commented out.   We need to ditch seperate tables that we have now, I think all 
*	creature can use what we have now, plus maybe two additional world tables.   Everything else fits into the current schema.
*	Test Ripley data and update ACE_World.   
*	Refactor portals - I just left the old way in and faked it out with a view.  
*	Once this looks good and stable and we fix any found bugs - we need to clone this over to the character database and start refactoring
*	That will get us to persisted inventory.   

### 2017-05-21
[Og II]
* Continued work on Overhaul 
* re-enabled more constructed statements
* Most of the work to use existance of data to set flags for both phyics and weenieHeaderFlags - some cleanup remains - either logic issues or bad data
* modifed cirand to take an optional second parameter to spawn X items at a time.   This speeds up testing.   I set a default to 10 if you ommit the second parameter.
* base_ace_object and ace_object both seem to be loading.
* updated or created several views.
* have not tested with Ripley's new data export - I did not want to add a second variable to testing.   
* Next steps - use Ripley's data, create needed views, re-enable door, portal, NPC --- all the world things.

### 2017-05-17
[Og II]
* Continued work on Overhaul 
* Created mapping cross reference file to faciltate docuatation and cleaning up names - work in progress https://goo.gl/eaaNQb
* Created new database schema for world_object - modified mysqlinstall.bat to refect new baseline
* created script to ETL old schema to new schema.   I have not included it as we will be refactoring the initial data load to use the new
* schema according to Ripley.   I have the script tested and posted on my dropbox if anyone wants it.   I also have a complete data dump that
* zips down to a manageable size.
* BaseAceObject is now using the new schema
* I re-hooked up telepoi to use the new schema.   Still to do, convert to constructed statement
* Next steps - refactor AceObject and make sure world objects load again.
* Once that is complete - start same process using clone of this schema for character as discussed.

### 2017-05-12
[Og II]
* Started object Overhaul - added explicit values to properties enums.   Minor cleanup.  

### 2017-05-06
[Og II]
* I fixed both admin functions, ci and create.   I added some logic to leave, NPC's, portals and creatures with the same code as prior to my fix.
* Added Admin command cirand <typeId> to allow you to fully test and exercise the test ci command.

### 2017-05-05
[OptimShi]
* Added caching to all portal/cell.dat items.
* Added EnvCell decoding from the cell.dat (dungeon cells/indoor locations)
* Changed the cell-export console command to properly use datReader. If you had used this command previous, your exported files are incorrect.

### 2017-05-03
[Jyrus]
* Changed the structure of the table that contains the Portal Destinations to include other portal properties
* Renamed table from portal_destination to ace_portal_object and added a foreign key constraint to the weenieClassId field
* !!! Please note that the ACE-World tables must be filled before importing the ace_portal_object's table
* Implemented a portal recall debug command
* Added a Null check to Database.cs
* Fixed bug in ACE.Entity\Ace_portal_object.cs

[Ripley]
* Cleaned up project and solution files.
* Changed create and ci to use DebugObject.

[StackOverflow]
* Added Landblock Diag tool, type diag from console to launch the gui.

### 2017-05-02
[fantoms]
* Fixed character's total `Mana` when spending experience points on the `Self` ability.
* Added useful position debug commands, `@teletype`, `@setposition`, and `@listpositions`. `@teletype` will teleport the player, `@setposition` will test the database save functionality, and `@listpositions` will print out a list of all database positions too chat.
* Updated `SaveCharacterPosition` too prevent PortalType.Undef types from being used as a valid position type.
* Updated `PositionType` for Positions, too prevent position types outside of the definition from entering the database.

[Og II]
* Refactored ctw to admin commands ci and create both take parameter of a weenieId.   You can spawn anything in the database.   @ci 21376 - will spawn Martine's Robe   This does not include the turn to object code - I pulled that out due to a bug.   I will submit via new PR.

### 2017-05-01
[OptimShi]
* Modified DatLoader.SetupModel to be easier to initiate (got missed when other items had the same changes applied). Also added working example to the Lifestone.cs OnUse function to take model radius into account.
* Hooked up the "ExaminationQueuePop" in Player.cs to fire.
* Added IdentifyResponseFlags, for when we start to send actual data.
* Added a basic debug string to any DebugObject. It simply returns the objects GUID and WeenieClassID. (WorldObjects will return a "failed" examine response)

### 2017-04-30
[OptimShi]
* Add client_cell.dat reading for landblocks in CellLandblock.cs (named so as to avoid confusion with existing Landblock class)
* Adjusted "@tele" command to pull correct PositionZ from client_cell.dat

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
