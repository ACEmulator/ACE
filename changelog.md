# ACEmulator Change Log

### 2020-06-04
[OptimShi]
* Corrected colors for PFID_CUSTOM_LSCAPE_R8G8B8 type textures

### 2020-06-02
[Ripley]
* Fixed issue with createinst in creating child objects

### 2020-06-01
[Ripley]
* Fixed issue with character creation skill bonuses

### 2020-05-31
[gmriggs]
* Including previously owned houses in /hslist
* Fix minor off-by-1 numbers

### 2020-05-27
[gmriggs]
* Improved AuditItemSpells

### 2020-05-27
[gmriggs]
* Added CreateInst gap finder

### 2020-05-26
[CrimsonMage]
* Commit #3000 - Fixed typo "Cannon unban" to "Cannot unban"

### 2020-05-25
[gmriggs]
* Improved collision detection for projectiles on players
* Ensure object is on landblock for /removeinst 

[Ripley]
* Portal.CheckUseRequirements checks PlayerKillerStatus not PkLevel
* Removed  temporary patches
* Added cloaks to SelectWieldedClothing()

### 2020-05-24
[gmriggs]
* Updated AnimationPartChange DatLoader accessibility for ACViewer clothing

[Trevis, Ripley]
* Use quest_mindelta_rate server prop when displaying myquests output
* Added 'Quest list is empty' response to myquests
* Don't pass default to property manager

### 2020-05-22
[OptimShi]
* Allowed console commands to start with "/" or "@". Would previously respond with "Invalid Command" if the command started with a "/" or "@".
* Added dat file Iteration checking to server files.
* Handled DDD_InterrogationResponse to check clients connecting for up-to-date dat files and display a warning upon login.
* Use command "modifybool show_dat_warning true" to show warning to clients upon login.
* Fixed ACE.Server.Network binaryReader.Align() function.

[OptimShi, Harli Quin]
* Added Variance to Burden
* Updated Burden Formula for Tiers
* Added MutateBurden curve tables
* Added MutateFilter, armor burden mod

### 2020-05-17
[gmriggs]
* Only send fellowship vital updates to clients w/ fellowship window currently open

### 2020-05-10
[Ripley]
* Skip handing out IOUs if system isn't enabled

[Ahric]
* Added ability to exclude specific IPs from max session count
* Added config examples
* Adding extra options for AutoApplyWorldCustomizations

### 2020-05-08
[Ripley]
* Fixed read issue with GetContentFolder
* Added rare timestamping and counters
* Added Real-Time Rare chance modifier
* Added server configurables for real time rares
* Updated CanGenerateRare to false in the case of bad data
* Fixed issue with SellPrice usage for AlternateCurrency
* Added ping to clients at character select
* Do not send Structure in Appraisal reply for TinkeringMaterial CraftTools
* Updated AwardSkillXP to optionally notify player as appropriate
* Destroy projectiles on collision
* Update offline purge to clean up invalid allegiance data
* Prevent create/ci from using specialized weenie types
* Enable global chat toggling
* Fixed issue with Recipe conversions
* Wire up automatic database updates
  - Support automatic database patching
  - Support automatic updating of world database to latest release
  - Support automatic re-application of world database customization
* Update fix-allegiances command
* Update Fellowship messaging + better FellowPortalSending handling
  - Limit FellowPortalSending spells to in range of caster
  - Update handling to not teleport multiple times

### 2020-05-05
[gmriggs]
* Separated CreateItem from MutateItem in loot generator, wiring up full support for /lootgen=
* Added support for PetDevice to /lootgen
* Updated lootsim
* Updated trinket cleanup script

### 2020-04-25
[gmriggs]
* Added support for magic resist to /debugdamage
* Added /debugdamage for SpellProjectile
* Cleaned up lootgen factory

[Ripley]
* Update Corpse messaging

### 2020-04-24
[gmriggs]
* Use sphere radius when determining distance for mob aggro
* Added cylinder distance to /dist dev test command
* Added support for more sealed bags of salvage to RecipeManager_New

### 2020-04-23
[gmriggs]
* Updated casting start pos to match retail
* Set AppraisalLongDescDecoration on missile weapons 
* Fixed salvage fail msg
* Synced get_ending_frame up with acclient 

### 2020-04-20
[gmriggs]
* Added handling to prevent server crashes

### 2020-04-18
[Ripley]
* Updated ConsumeUnlocker output to match pcaps
* Updated GetHouse

[Harli Quin]
* Added Burden to tables

### 2020-04-18
[gmriggs]
* Fixed black thistle 
* Improved pk logout stop movement
* More CMT fixes for players
* Fixed copper / silver salvage

[Harli Quin]
* Missile loot refactor

[Ripley]
* Updated Corpse permission conditions
* Updated CheckUseRequirements for Portals
 
### 2020-04-16
[gmriggs]
* Prevent ninja looting chests
* Added server option for /pkl entry collision check, enabling door skipping glitch from retail
* Normalized config option names / default retail values
* Normalized server config option for allow_jump_loot
* Added retail message for Enduring Enchantment
* Updated Surge of Affliction + show_dot_messages for something more appropriate
* Further tweaks to DF, adding 'health' to message to suppress Mag-Tools parse error
* Added server option for players to 'long hold' doors open for other players, as per retail
* Improved logging for scatter threshold z 

[Mag-nus]
* EnableSensitiveDataLogging for shard and world
* Add missing .GetFullMessage()

[Ripley]
* Fix writability issue with opening "blank/empty" books.

[Ziang]
* Fixed gap in UpdateQuest emote

### 2020-04-14
[gmriggs]
* Temporarily revert npk IsJumping checks again. Fixing actual bug, retaining existing functionality

### 2020-04-13
[gmriggs]
* CurCell=null rare bug quick fix
* Updating resistance checks for hotspots and falling damage

### 2020-04-12
[gmriggs]
* Added support for mansion_min_rank 0
* Prevent door thrashing
* Fixed bug with marketplace recall
* Tied monster aggro distance to VisualAwarenessRange
* Added support for abdomen armor to Monster_Inventory 
* Improved monster sticky melee
* Added support for AiUseMagicDelay, synced up to retail

[Mag-nus]
* Fixed EnchantmentManager Heartbeats

[Ripley]
* Disallow [re]entry into world if shutdown is in progress
* Add support for account bans
* Fix spell bar position save issue

### 2020-04-11
[gmriggs]
* Fixed antius blackmoor statue
* Added support for PowerupTime for monster melee attacks
* Fixed monster stop-and-go bug after sticky
* Removed line of sight check for mob archers
* Synced thrown weapons re-appearance time for monsters up to retail
* Updated legacy function for player melee skill
* Synced movement params (stop completely, distance to object) up between client and server slightly better w/ defaults
* Revert "fixing shade=0, from optimshi (#2870)"
* Added support for caster weapons to Monster_Inventory
* Updated no stamina use chance
* Added DefenseStanceMod
* Bumped up Healing_MaxMove for pks a small amount, based on player feedback 
* IsJumping consistency across pks and npks
* Added jump looting per retail
* Added reguid support for json landblock conversion w/ relative guids 
* Ensured pk timer logout stops movement completely 
* Synced critical frequency for existing ice tachis

[Mag-nus]
* Add order to BiotaPropertiesPalette

[Ripley]
* Check player names against creature names in world db
* Disable creating players with names of creatures in world
* Update boot command

[deca]
* Fixed god/ungod failing on SkillAdvancementClass

[fartwhif]
* Fixed bug allowing a corrupt or malformed packet to crash the server when the fragment parser tries to read past the end of the data


### 2020-04-10
[gmriggs]
* Verified null restriction tables

### 2020-04-08
[gmriggs]
* Fixed sidestep clamp
* Improved house root guid
* Synced bouncy housing barriers with server physics

### 2020-04-06
[Ripley]
* Updated docker-compose.arm64 / .dockerignore

[gmriggs]
* Added support for lockpick mod, fixing lockpick sound broadcast
* Updated coded recipes for trinkets latest version
* Fixed bandits dual wielding bow + melee
* Further updates to melee/missile repeat cycle to sync more precisely to retail
* Added preliminary support for fast missiles
* Added support for 'lead missile targets' character option disabled
* Added power bar reset on charge failed

### 2020-04-04
[Mag-nus]
* Improved ShardDatabaseWithCaching Locking
* Hybrid weenie biota

[gmriggs]
* Updated LoS tests for recent changes to ProjectileTarget
* Added function to automatically fix any 'invisible' equipped items

### 2020-04-03
[gmriggs]
* Updated physics rotation on EmoteManager.Turn

[Mag-nus]
* Converted ShardDatabase/WorldObjects biota from the ACE.Database model to the new ACE.Entity model

### 2020-03-31
[gmriggs]
* Added null check for built-in weapon spells

### 2020-03-30
[Ripley]
* Disable account auto promotion when admin account is created via console

[gmriggs]
* Fixed shade=0, from optimshi
* Prevent actions while suicide in progress 
* Suppress log message for require_spell_comps server option

### 2020-03-29
[gmriggs]
* Fixed require_spell_comps for freshly loaded acclient 
* Fixed some rare cases of invisible player bug
* Ensure npks can take falling damage when portal destination is aerial
* Fixed possible gap for point-blank life projectile damage

### 2020-03-28
[gmriggs]
* Improved animation for AimLevel

### 2020-03-25
[gmriggs]
* Update resist aug (3rd times the charm)
* Removed buggy AL code 
* Improved ring projectiles spawns for spells with even # of projectiles
* Revert "Switch Quest stamp on pickup to on generator controlled items only 

[Ripley]
* Fixed issue with Quest stamp on pickup

### 2020-03-24
[gmriggs]
* Improved consistency for NPK falling damage
* Fixed corpses spawning frozen in mid-air
* Added support for more fellowship spell types
* Added support for fellowship spells from non-player sources
* Fixed display bug for enchantment messages 
* Fixed gap in admin vision 

[Ripley]
* Support ARM64 (RPI4 4gb - Ubuntu x64)
* Updated appveyor.xml
* Updated AppVeyorAfterDeploy.bat
* Updated dbversion.txt with latest ACE-World-Database release
* Updated docker configuration + Improved Build/Version tracking
* Stacking: Do nothing on use
* Switch Quest stamp on pickup to on generator controlled items only 

### 2020-03-23
[Ripley]
* AppVeyor build pipeline changes
* Updated appveyor.xml

[gmriggs]
* Added null check to GetScrollWeenie

### 2020-03-22
[gmriggs]
* Update stance if /pkl is entered from combat mode
* Spell rotation updates
* Ensure floating point values are retained for IgnoreMagicArmorScaled RL
* Further cleanup for spell_projectile_ethereal

### 2020-03-21
[gmriggs]
* Fixed small movement gap
* Synced up rotation on MoveToState
* Added non-default server option to broadcast spell projectiles as ethereal to clients
* Fixed global velocity for jump broadcast
* Ensured life projectiles take natural resistances into account
* Fixed resist aug (again)

### 2020-03-20
[gmriggs]
* Cleaned up debug code

[Ziang]
* Fixed lack of Shield AL assignment

### 2020-03-19
[gmriggs]
* Minor adjustments to crafting messages
* Ensure 19E - PlayerDeath is broadcast to victim for plugins
* Fixed pressure plate generators w/ RegenInterval 0
* Added find_cell_list debugging 

### 2020-03-18
[gmriggs]
* Added support for 'crafting chance of success dialog' player option to regular recipes
* Updated hotspot cycle variance to use standard formula
* Broadcasting icon underlay changes for all recipe mods
* Added server option for 'craft_exact_msg' 

### 2020-03-17
[gmriggs]
* Fixed broadcast move to home

### 2020-03-16
[gmriggs]
* Fixed monsters sliding around if they have already started a MoveTo before CreateObject is sent
* Filled some gaps for instant vital updates for targeted creature
* Added debugging for passive pet owner going null 
* Fixed spell projectiles sometimes jumping above target's head on collision
* Ensure spell projectiles remain inactive if collided on world entry

### 2020-03-15
[gmriggs]
* Prevent vendor service spam / cast animation desync 
* Ensure sync between ace and physics motion state for magic casting
* Added options for server admins to scale hollow damage in pvp battles
* Synced Player_Death network broadcasts up to retail
* Adjusted spellcast_max_angle logic
* Fixed burning coals
* Improved accuracy of 'your movement has disrupted spellcasting / healing!' distance check
* Fixed missing e / typo
* Added support for gems with UseUserAnimation and UseSound
* Refactored food
* Added player.IsDead check to Gem use

### 2020-03-14
[fartwhif]
* Limit the number of sequence IDs per S2C NAK packet

### 2020-03-12
[gmriggs]
* Reduced unnecessary spam in debug logs
* Fixed summon into wall issue

### 2020-03-11
[gmriggs]
* Prevent player-initiated dispels while pk timers active
* Fixed players sometimes invisible after teleport

### 2020-03-10
[gmriggs]
* Added support for taboo table during character creation 

[gmriggs, Ripley]
* Added support for passive pets

### 2020-03-09
[gmriggs]
* Updated house query
* Improve responsiveness of first turn after cast gesture 
* Slightly improved target tracking for spell projectiles 
* Improvements from fixcast
* Adjusted healing movement fail range
* Server broadcast motions back to self client
* Adjusted spell projectile target height
* Cleaned up Server.Entity.Spell
* Fixed no scrolls dropping when world precaching is enabled 

[Harli Quin]
* Added trinkets
* Refactored Jewelry

[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release

### 2020-03-08
[gmriggs]
* Updated epic slashing ward 
* Fixed chest reset interval
* Updated verify-armor-levels
* Fixed pk logout bug

[Ripley]
* LandblockInstanceWriter: InstanceNames -> WeenieNames

### 2020-03-07
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release
* More fixes for GenerateNewFace

[gmriggs]
* Revert cast motion turn stop completely
* Fixed casting self spells while /attackable off
* Wielder null fix
* Re-implemented spellcast_recoil_queue server option
* Fixed cast tick gap
* Added /verify-armor-levels admin command
* Fixed aug resist
* Fixed minor sync issue in TurnToHeading
* Reworked TurnTo during magic casting to allow for player manual control
* Improved target tracking for pk casting
* Verify TimeSync
* Fixed some spellcast angle issues

[Harli Quin]
* LootFactory Melee refactor
* LootFactory Simulator Jewelry 

### 2020-03-06
[Ripley]
* Be less restrictive with GenerateNewFace

[Mag-nus]
* Revert .net standard 2.1 to 2.0 in Adapter and Database.

[gmriggs]
* Fixed CS icon underlay color on initial application

### 2020-03-05
[gmriggs]
* Added upper cap to max spell range
* Added additional case for runrate_add_hooks
* Defender spell fix

### 2020-03-04
[gmriggs]
* Prevent monster initial snap forward when they start moving
* Ensured crit ratings work on combat pets
* Added /debugdamage support for all creatures
* Fixed defense and attack bonuses for dual wielding

[Ripley]
* Fixed Alt Currency minimum cost

### 2020-03-03
[Ziang]
* Fixed underclothes getting AL

[Ripley]
* Exposed content folder in containers

### 2020-03-02
[Mag-nus]
* Added ACE.Database.WorldDatabaseWithEntityCache class
* Switched ACE.Server over to the new ACE.Entity.Models.Weenie model

[gmriggs]
* Updated healing
* Spell projectile dormant fix redux
* Death item stack

[Harli Quin]
* Pet device level loot tiers

[Ripley]
* Dockerized ACE
* Updated .gitignore
* Adjustments to AppVeyor
* Updated Dockerfile
* Updated version output to client
* Updated appveyor.yml
* Updated AppVeyorAfterDeploy.bat


### 2020-03-01
[gmriggs]
* Ensure melee target is alive before MoveTo

### 2020-02-29
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release
* Updated project to include database scripts for publishing
* Added support for KillQuest processing
* Hooked up WoundedTaunt emoting
* Updated LocalSignal emitting
* Updated Monster_Awareness.cs

[Ziang]
* Loot gen changes

[gmriggs]
* Added minor QoL improvements for content dev commands

### 2020-02-28
[gmriggs]
* Fixed spell projectile in dormant landblock bug 

### 2020-02-27
[gmriggs]
* Improved json landblock export
* Synced skill raise message up to retail 
* Added support for landblock / recipe / quest to export-sql and export-json

### 2020-02-26
[gmriggs]
* Added sticky distance check for melee repeat attacks

### 2020-02-23
[gmriggs]
* Synced up emotes for scream and newenemy
* Added support for cloak proc spells
* Added equipment ratings for DR / DRR / CD / CDR 
* Updated generators
* Cleaned up door patterns 
* Fixed acclient skipping over ethereal animation hooks for known doors that are opened > 96 distance
* Added /reload-landblock command for content devs

[Ripley]
* Updated project files
* Update OnActivate for deep-linked object activation propagation 
* Fixed sound on Switch weenie class

### 2020-02-22
[gmriggs]
* Fixed AuditItemSpells for spell sets
* Added built-in damage bonus for some quest bows


### 2020-02-20
* Added/removeitemspell dev command
* Updated homesick

### 2020-02-19
[gmriggs]
* Fixed landblock instances content folder 
* Fixed monsters failing to navigate home 
* Added landblock instances to /clearcache content developer command

### 2020-02-18
[gmriggs]
* Fixed launching projectiles at point-blank range through doors 
* Fixed untargeted spell projectiles hitting scrum drudge
* Suppress TakeItems output when player has 0 items
* Homesick adjustments

[CrimsonMage]
* Fix to Spine Glaive to stop Double dropping Mazule

### 2020-02-16
[gmriggs]
* Ensure gems used directly by player are not resisted0
* Fixed negative item magic spell resistance check

### 2020-02-15
[gmriggs]
* Moved dequip item spell code from Player.TryDequipObjectWithNetworking to Creature.TryDequipObjectWithBroadcasting
* Added support for IgnoreMagicResist / Armor on creatures directly

### 2020-02-14
[gmriggs]
* Cleaned up spell resistance code

[Mag-nus]
* Added new adapter classes and models

### 2020-02-13
[Mag-nus]
* Improved startup time and memory consumption
* EmoteManager: Where before OrderBy

### 2020-02-12
[gmriggs]
* Added quest support to import-json and import-sql
* Added landblock instance support to import-sql and import-json
* Added support for recipes to DeveloperContentCommands
* Fixed a bug where fellowship leader drops under abnormal circumstances
* Added z-bump to scatter gen
* FinishCast patch

[Ripley]
* Wired up adaptor support for wieldedTreasure.json, region.json and encounters.json
* Switch delayed LogOut_Inner due to PKLogoutActive to PlayerManager.Tick

### 2020-02-08
[Ripley]
* Added support for minimums in AwardLevelProportional*XP

### 2020-02-07
[gmriggs]
* Further improvements to combat stance swapping

### 2020-02-06
[Harli Quin]
* Loot simulator update

[gmriggs]
* Fixed combat stance and death gaps during pre-windup turn 
* Matched melee weapon tailoring up to latest GDLE logic

### 2020-02-04
[gmriggs]
* Added /dungeonname dev command
* Fixed some combat state thrashing for food and healing kits
* Added support for fastbuffing
* Ensure pktypes are calling the correct MoveTo function 
* Ensure same PKType for healing other targets, and beneficial spells 
* Added additional fellowship xp sharing logic from Mirror, Mirror
* Fixed a gap in pk casting
* Fixed casting spells on self-wielded target
* Added dot resistance on appraisal panel
* Added /fellow-dist dev command 

### 2020-02-02
[Mag-nus]
* Fixed AllegianceManager/HouseManager mulit-thread corruption from database callback

### 2020-02-01
[gmriggs]
* Added missing swirlies to recall spells
* Added caster effects to portal tie and lifestone tie

### 2020-01-31
[Ripley]
* Add support for inferred %tqm and %tqc in EmoteManager
* Updated CreateDestroyItems to gracefully fail if it can't find Success/Fail item in DB

### 2020-01-30
[gmriggs]
* Fixed jump skill in god mode
* Fixed cliff slide, contact plane, and landblock transition bugs 

### 2020-01-29
[Ripley]
* Fix Ethereal issue with DefaultOpen doors

### 2020-01-28
[gmriggs]
* Alternate multi-projectile spell fix

[Ripley]
* Allow Doors to be controlled via Open/Close Me emotes

### 2020-01-27
[gmriggs]
* Fixed missed line for multi-projectile spell targets
* Improved efficiency for large allegiances

[Ripley]
* Updated Lifestone usage to drain half of player's current stamina
* Use DefaultOpen / DefaultLocked properties from DB for Doors
* Enforced CheckUseRequirements on portal ties and summons

### 2020-01-26
[gmriggs]
* Improved swapping weapons while bow is reloading

### 2020-01-25
[Harli Quin]
* Added missing T4 Rares

### 2020-01-24
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release

[Harli Quin]
* Added % loot counters that were missing. Changed Counters to Floats

### 2020-01-23
[gmriggs]
* Updated GetLastAppraisalObject()
* Added AimLow/AimHigh animations for missile launchers

### 2020-01-22
[Harli Quin]
* Increased Melee D Bonus for no wield wands from 5% to 10%

[gmriggs]
* Enabled post-windup turn for pve
* Fixed missile bug + matron bug 
* Added preliminary support for PropertyInt.Unique
* Normalized some attuned / bonded logic

[Mag-nus]
* Updated default dats path on windows
* Update Readmes - consolidated per duplicate information on wiki
* Updated ShardDatabaseWithoutCaching.cs 

### 2020-01-21
[gmriggs]
* Added support for IgnoreShield
* Added InqCollisionProfile null check
* Added support for armor cleaving
* Fixed attacking while airborne during pk battles 
* Fixed *wave* spam
* Broadcast windup motions as actions, as per retail
* Added handling for multiple attack sequences sent by acclient
* Updated default values for range and increment/decrement in EmoteManager
* Fixed mega shields (scaling issue)
* Fixed missing feedback when launching projectiles point-blank into environment
* Improved the TakeItems message
* Normalized defaults for InqQuestSolves to match other emotes
* Added support to clean up house guest lists for deleted players
* Added busy checks to melee / missile

[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release

### 2020-01-19
[fartwhif]
* Removed network test helpers

### 2020-01-17
[gmriggs]
* Enabled players to queue next pickup while previous pickup is in retraction state, as per retail
* Minor appraisal refactor
* Improved /createinst command for content dev
* Added /getinfo command to get basic weenie info 

[Ripley]
* Added more TestNoQuality support in EmoteManager

### 2020-01-16
[gmriggs]
* Filling gaps for attack done
* Matched TakeItems stackSize default value up to other emotes
* Fixed /import-sql filename munging
* Updated MotionTable static tables to ConcurrentDictionary

### 2020-01-15
[gmriggs]
* Fixed target velocity for non-tracking spells
* Added multiplier to LumAugItemManaUsage to match client values
* Improved peturbation formula
* Added support for SpellFlags.NotIndoor / SpellFlags.NotOutdoor
* Updated multi-projectile spells to be closer to retail / match client descriptions.

[Ripley]
* Enforce quest timers for Portal.QuestRestriction
* Added time limit to Portal Space
* Fixed issue with VerifyUse
* Added alert for incomplete dats

### 2020-01-14
[gmriggs]
* Removed cooldown spel message from log during dispel
* Updated SpendLuminacne to use HeroXP64
* Added Support for SpellFlags.IgnoresManaConversion

### 2020-01-13
[Mag-nus]
* Switched BiotaPropertiesInt to Composite Key

[Ripley]
* Added/Adjusted some emote handling
* Reordered Contract Flag staging

### 2020-01-07
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-Database release

### 2020-01-06
[gmriggs]
* Added NumProjectiles verification

[Mag-nus, Ripley]
* Updated to .NET Core 3.1

[Mag-nus]
* Added database logging
* Added more verbose retransmit logging

### 2020-01-05
[Mag-nus]
* Renamed the user supplied path for Config.js from 'filename' to 'path'

### 2020-01-04
[gmriggs]
* Updated ring projectile spawn position to match retail
* Fixed some combat pet maneuvers
* Fixed some player movement issues during pk spellcasting
* Updated casting negative item spells on items wielded by target
* Improved 'Rolling Ball of Death' handling

[Mag-nus]
* Added support for application startup from outside the directory

### 2020-01-02
[gmriggs]
* Added HasDungeon criteria to landblock (check if landblock is a dungeon with no traversable overworld)
* Re-added missing .EnqueueAction() to EmoteType.CastSpell

[Ripley]
* Changed Project order in SLN file

### 2019-12-31
[Mag-nus]
* Added ShardDatabaseWithoutCaching

[gmriggs]
* Improved monster combat maneuvers table
* Added support for dual-wielding mobs
* Added support for EmoteManager.CastSpell untargeted spell projectiles
* Additional rotation checks for EmoteType.Move*
* Updated spell projectiles to use more spell / weenie data
* Refactored spell projectiles
* Improved usage of body part quadrants for physical attacks
* Use ItemSpellCraft for target resistance check when casting spells built into wands
* Improved handling of json weenie metadata

### 2019-12-30
[gmriggs]
* Merged fastcast branch into master
  * Advanced player movement and spellcasting techniques are now available for PK/PKLite players

* Improved /targetloc

[Mag-nus]
* Network improvements

[Harli Quin]
* Refactored magic defense and missile defense bonuses in loot generator
* Removed non-retail vital regen cantrips
* Added LootFactory simulator

### 2019-12-28
[gmriggs]
* Fixed boost variance for life transfer spells

### 2019-12-26
[gmriggs]
* Fixed mobs casting flame wave

### 2019-12-22
[gmriggs]
* Fixed monster special attack fx

[Yonneh]
* Minor math optimization for CRC verification

[Harli Quin]
* Fixed spell IDs for CreateRandomScroll

### 2019-12-21
[Morosity]
* Updated Pets and PetDevices for Summoning

[fartwhif]
* Removing unused pattern from CryptoSystem for slightly better performance

[gmriggs]
* Fixed an issue with Sphere.FindPlacementPos
* Fixed generator respawn retries
* Slightly improved corpse positioning for creeping mobs

### 2019-12-19
[gmriggs]
* Only apply NetherDotDamageRating to direct damage

[Ripley]
* Switched Lifespan to use RealTime instead of GameTime

### 2019-12-18
[gmriggs]
* Do not add cross-landblock references for calc_cross_cells_static

[dgarson]
* Fixed caster damage modifier to use appropriate PvP modifier

### 2019-12-17
[gmriggs]
* Continued porting of advanced player movement / fastcast to master branch

### 2019-12-16
[fartwhif]
* Fixed a bug that occurs when client sends only a cleartext CRC NAK

[Ripley]
* Updated AugmentationDevice to use WeenieErrorWithString

### 2019-12-15
[gmriggs]
* Backporting support functions from fastcast branch

[Ripley]
* Clutch of the Miser augmentation does not affect items for PK / PKLite deaths

### 2019-12-14
[Ripley]
* Added support for 'no drop on death' landblocks
* Expand scope for generator wipe conditions
* Additional verifications for 'item in trade window'
* Added support for 'FirstEnterWorldDone' on players
* Updated House.OnProperty() HasDungeon boot condition
* Updated /generatordump information

[Yonneh]
* Fixed CharacterOption enum to match PlayerOption ordering in acclient

[gmriggs]
* Fixed a bug where Critical Strike bonus wasn't applying to life projectiles
* Fixed EmoteType.AwardLevelProportionalXP when Max64==0
* Ensure level 8 item self spells take precedence over level 8 item other spells

### 2019-12-12
[gmriggs]
* Re-verify client checks for GameAction 0x35 - UseWithTarget
* Fixed armor tailoring intermediate kit TargetType
* Tailoring - do not copy UIEffects to destination item

### 2019-12-11
[gmriggs]
* Added RecipeManager verifications for source and target usability flags
* Added lifestone_broadcast_death server option, clamping to local broadcast range
* Fixed scatter generators for Neftet plateaus

[Ripley]
* Added PetClass commenting to SQLWriter

[Mag-nus]
* PlayerFactory improvements

### 2019-12-10
[gmriggs]
* Added missing function for physics @ plateau

[gmriggs + fartwhif]
* Improved handling for overages for house payments

[Ripley]
* Additional pcap properties

### 2019-12-09
[Mag-nus]
* PlayerManager GetOnlineCount() and GetOfflineCount()

[gmriggs]
* Updated Enlightenment vitality formula for max health
* Removed some incorrect log / chat messages for SpellComponentsRequired
* /addenc command for content creators to add encounters
* Matching DoT ticks up with retail heartbeats

### 2019-12-08
[gmriggs]
* Additional verifications for RecipeManager

[Mag-nus]
* Switch ThreadSafeRandom from lock to ThreadLocal<Random>

[Harli Quin]
* Fixed an issue with loot generator creating melee weapons with overcapped damage

### 2019-12-07
[gmriggs]
* Fixed client hourglass for applying encapsulated essence while in busy state
* Improved speed for /import-json all content dev command
* Additional CurrentLandblock checks to Slumlord
* Pre-validating EnvCell transitions

### 2019-12-06
[Ripley]
* Fixing issues with nulls in EmoteManager.Replace

### 2019-12-05

[gmriggs]
* Setting default CultureInfo to en_US
* Fixing shield tailoring
* Move* emotes use default WalkRunThreshold
* Added support for creeping mobs
* Refactored raise attribute/vita/skill
* Added verify* console commands for admins
* ManaConversionMod tweaks
* Fixed some minor issues with AwardSkillXP emotes
* Added /resist-info developer command
* Additional checks for /verify-skill

[Ripley]
* Removed some dungeons from AdjustPos that have been fixed in database
* Added support for RelativeDestination on portals
* Added support for emote branching with events

[Ripley, Ziang, gmriggs]
* Added support for Creature.QuestManager

### 2019-12-03
* Moved UpdatePlayerPhysics to Player_Tick.cs

### 2019-12-01
[gmriggs]
* Fixed a possible system-dependent state crash in EnvCell constructor

[Harli Quin]
* Updated loot generator for missile and casting weapons to better match retail
* Improved test loot generator commands

[Mag-nus]
* Added server option for adjusting quest timers

### 2019-11-28
[Harli Quin]
* Removed a bugged rare crystal

[gmriggs]
* Added support for 'confirm use of rare gems' player option
* PlayScript cleanup

### 2019-11-27
[Mag-nus]
* CharacterExporter fixes
* Added more EoR spells to Player_AllowedSpellIDs

### 2019-11-26
[gmriggs]
* Fixed ThreadSafeTeleportOnDeath -> SetLifestoneProtection
* Additional IsDead checks for magic casting
* Additional verifications for DamageHistory.CheckInternal()
* Added fix-biota-emote-delay server console command

[Mag-nus]
* Set all shard non-zero defaults to zero

### 2019-11-25
[gmriggs]
* Additional checks for IsDead
* Ensuring HealingOverTime does not tick up health for dead creatures
* Fixed stance swap state thrashing
* Adding server option to include viewer name in 'container already opened' message
* Enabling retail behavior for trade bots
* Refactoring house permissions

### 2019-11-24
[Mag-nus]
* Added PurgeOrphanedBiotas
* Moved ThreadSafeTeleport to WorldManager
* Execute no-delay emotes immediately

### 2019-11-22
[Harli Quin]
* Updated test loot generator

[gmriggs]
* Added post-windup range check to spellcasting
* Improved accuracy for XP divvy
* Fixed 2 tailoring bugs
* Handle special case for PKType deaths + Enduring Enchantment + DoTs
* Fixed death items for players < level 35
* Allowing Obsidian Chittick spines to do DamageType.Undef as per PY16 data

[Ripley]
* Update setups for creature dressup

### 2019-11-21
[Ripley]
* Updated ACE.Adapter to support spawn maps

[Mag-nus]
* ThreadSafeRandom cleanup

[gmriggs]
* Fixed monster resleep -> wakeup cycle for combat mode

### 2019-11-20
[gmriggs]
* Added support for WeaponMissileDefense and WeaponMagicDefense
* WorldObject_Weapon / imbue refactoring
* Fixing /buff &lt;target&gt; for life spells
* Fixed a double message bug for archers
* Reversed exemption checks, added missing WeenieObject.Is&lt;Type&gt; methods
* Added debug logging for failed player transitions
* Removed PKTimer for Bael'Zharon battles
* Removed Player.Rotate during looting that wasn't in retail

[Harli Quin]
* Added 7 missing rare wcids, removed 5 bugged ones

### 2019-11-18
[gmriggs]
* Fixed a DamageHistory static bug
* Minimized DamageHistory.WeakReferences
* Send updates to Monarch ID over network
* Added some missing calls to allegiance house boot

### 2019-11-17
[Mag-nus]
* Improved multithreaded landblock group ticking (non-physics)
* Fixed a NullReferenceException with CurrentMovementData.Invalid
* Fixed a NullReferenceException in EnchantmentManagerWithCaching

### 2019-11-15
[gmriggs]
* Added preliminary version of pickup busy state handling
* Improved RunRate handling for players
* Broadcast correct RunRate for overburdened players
* Improved AI for missile combat mobs
* Fixed a possible lockup in physical combat when repeat attacks is disabled
* Backported Player_Magic refactoring from fastcasting branch
* Updated house storage chests to remain open while being viewed (no auto-close timer)
* Improved NaN handling for projectiles

[Harli Quin]
* Adjusted drop rates in loot generator for better alignment to retail
* Added developer commands to test the loot generator

### 2019-11-14
[Ripley]
* Support standard and non-standard usage of %tqt and custom %CDtime

### 2019-11-11
[gmriggs]
* Added /damagehistory developer command
* Ensuring ResistanceMod never goes below 0

[gmriggs + Yeonan]
* Applying transferCap to both srcVital and destVital for life transfer spells

[Ripley]
* Added protection and logging for house basements w/ missing location data

### 2019-11-10
[gmriggs]
* Improved PK/PKLite top damager detection

[Ripley]
* Added server option for create_list DestinationType.Wield items dropping to corpse
* Updated EmoteType.DeleteSelf

### 2019-11-09
[gmriggs]
* Added T6 rares for two-handed weapons
* Added defense imbues to effective defense calculations

[Ripley]
* Updated GameEventInscriptionResponse

[dirtyelf]
* Added valid skills hashset, check valid skills upon entering god mode

### 2019-11-08
[gmriggs]
* Updated DamageMod enchantments from multiplicative to additive (ie., Eye of the Hunter)

[Mag-nus]
* EmoteManager AddDelaySeconds() only when > 0

### 2019-11-07
[gmriggs]
* Added luminance award message for quest turn-ins

### 2019-11-06
[Ripley]
* Added DestinationType.Wield to corpse object selection

### 2019-11-03
[Ripley]
* Added different pickup animations based on source/target height

### 2019-11-02
[gmriggs]
* Added some missing shield cantrips
* Fixed a bug with material probability
* Improved handling for monster spell.IsResistable
* Added support for Overpower and OverpowerResist

### 2019-10-31
[Ripley]
* Cleanup ActOnUse for Ammo and move to debug only alert.

[gmriggs]
* Enabled magic absorption for missile/magic weapons

### 2019-10-30
[Ripley]
* Fix buy issue with AltCurrency Vendors.
* Add support to limit the number of accepted connections per IP Address.

[gmriggs]
* Improved handling for attack sequences

[Ziang]
* Ensure that Impen spell is present on magical Covenant Armor

### 2019-10-29
[gmriggs]
* Fixed some gaps for DST

### 2019-10-27
[gmriggs]
* Backported hotspot accuracy from PR 1991
* Added support for PropertyBool.AffectsAis
* Added support for PropertyBool.AiImmobile
* Added support for PropertyBool.OpensAnyLock
* Added support for PropertyBool.PortalIgnoresPkAttackTimer
* Broadcasting item give sound to match retail

[slushnas]
* Allow items that proc to cast untargeted spells

### 2019-10-26
[gmriggs]
* Additional checks for CombatMode
* Updated EmoteType.AwardLuminance to use HeroXP64

### 2019-10-25
[gmriggs]
* Fixed monster idle emote update sync
* Refactored GetProjectileSpellType

### 2019-10-24
[dirtyelf]
* Improved /ungod resiliency

### 2019-10-23
[Ripley]
* Adjust the way SpendCurrency spends currency.

[gmriggs]
* Added item_dispel server config option to fix end of retail bug

### 2019-10-222
[gmriggs]
* Improved monster position sync and corpse sync
* Improved /teledungeon name search
* Fixed some gaps with PK logout timer

### 2019-10-20
[Ripley]
* Re-order operations in GiveObjectToNPC.
* Adjust ApproachVendor to track last spent alt-currency better.

[gmriggs]
* Updated monster spellbook probabilities
* Added player /config command

### 2019-10-19
[gmriggs]
* Fixing some gaps with nested emote chains and EmoteManager.IsBusy
* Adding support for monster casting untargeted spells (ie. ring spells)

### 2019-10-18
[gmriggs]
* Allow neutral casters and missile launchers to be tailored onto weapons with DamageType

### 2019-10-16
[Ripley]
* Fix bug with pickup/merge into a full container.

[gmriggs]
* Added support for monsters with PropertyBool.NonProjectileMagicImmune
* Fixed a bug where atlatl/thrown weaps were using power instead of accuracy bar

### 2019-10-14
[gmriggs]
* Added luminance sharing to fellowships

### 2019-10-02
[Ripley]
* Fix issue with EmoteType.Give handling stackable/nonstackable items.

### 2019-09-28
[Ripley, Mag-nus]
* Support Purging of deleted Characters from database

### 2019-09-27
[Ripley]
* Support Dark Idol recipe DataId mods

[gmriggs]
* Added luminance award for kills

### 2019-09-26
[Ripley]
* Adjustments to Guid Recycling
* Some Give/Trade Wielded Adjustments

[gmriggs]
* Fixed a bug with player missile defense

### 2019-09-24
[Ripley]
* Support SpellSets for Items that don't level

### 2019-09-22
[Ripley]
* Validate int data for GenerateNewFace
* Remove coded North Glenden Prison position adjustments
* Adjust Split StackSize Checks

### 2019-09-21
[Ripley]
* Add logging to HousePortal nulls

### 2019-09-19
[Ripley]
* Update Killed by misadventure conditions

### 2019-09-18
[Ripley]
* Fix issue with opening empty hooks
* Sync HouseHooksVisible to root house object

### 2019-09-15
[Ripley]
* Generate Random Faces for CreatureType.Empyrean NPCs

### 2019-09-14
[Ripley]
* Fix Hook Visibility Control in Basements
* Fix issue on Hook Appraisal
* Workaround fix to spawn hooks
* Misc Housing Cleanup
* Rebase SQL scripts, archived previous batch for legacy servers.
* Turn on Guid Recycling
* Update Logging messages
* Notify Generator of spawn failure for regen retry

[gmriggs]
* Added server option for Aetheria healing over time message color

### 2019-09-12
[OptimShi]
* Fix issue with AlternateSetup for certain non-human models

[Ripley]
* Fix issue with Umbraen Males and Female crown options

### 2019-09-01
[Ripley]
* Update logging with some missing tags.

### 2019-08-31
[Ripley]
* Fix issue with Nekodes being created as LightWeapons, should have been Katars.

### 2019-08-30
[Mag-nus]
* Cleanup log.Info level messages
* Added failed physics transitions to the warning log
* Added log4net.async and support for console colors

### 2019-08-25
[gmriggs]
* Started to clean up / organize /acecommands a bit
  - Added optional &lt;access level&gt; and &lt;search term&gt; parameters, ie. /acecommands developer, /acecommands house

* Fixes / refactoring to loot generator clothing

* Fixed Aura of Infected Spirit Caress spell category / stacking ()
  - Database/Optional/Shard/2019-07-12-00-Match_World_Spells_In_Biota_Enchantment_Registry.sql can be run by server operators after upgrading to this release to fix an existing spells in enchantment registries

[Theran]
* Visual updates for Withered mobs
* Removed bad weenie for Inscription of Heal Self

### 2019-08-24
[Ripley]
* Fix crash with Pickup and Wield.
* Fix issue with selling 0 value items to NPCs.

[gmriggs]
* Adjusted WEENIE_MAX for custom content creators

### 2019-08-23
[gmriggs]
* Filling a gap with item spell redirects
* Fixed an issue with thrown goblets

[Theran]
* Fixed the loot generator GetWield() method for magic casters
* Update Vile Scourge, Fist of Massacre, Vein-Thirst Kukri, and Yaja's Reach for MoA
* Fix missing PropertyInt 159 on Squalid Leggings
* Add Sedgemail Leather Armor weenies, using PCAP and ac.yotesfan.com data

### 2019-08-22
[Theran]
* Removed some weenies from the loot generator clothing tables to match retail
* Update Steel Chest with Clouded Soul scroll
* Update Guards for Clutch of Kings quest
* Add elemental hatchets for summoned Undead minions
* Three portals from LSD and their landblock instance spawn points
* Add PortalSummonLoc to Bur Lizk
* Update AddCharacterTitle statements to add missing titles and annotations
* Add Haebrean armor pieces that are set to base AL of Platemail, being of similar material composition
* Update some quest weenies related to Hamud's Demise quest
* Fix broken Kill Task quests

[Ripley]
* Update IOU handling code.
* Fix issue CoinValue client desync.
* Added some boxed items

[gmriggs]
* Fixed a possible null exception with RestrictionDB
* Removing officer titles from allegiance info packet (not used in retail, not parsed by Decal)
* Setting IsBusy state while dying

### 2019-08-21
[Ripley]
* Update Aetheria and how ProcSpell is displayed for SpellBook.
* Apply shard update script `2019-08-21-00-Fix_Aetheria_SpellBook.sql` to remove unneeded spellbook entries.
* Update [Fellow]PortalSending code to magic retail messaging.
* Updated Hooks to allow them to be accessible by roommates (characters on same account as house owner)

[gmriggs]
* Updated salvaging output formula to match retail
* Fixed a possible crash with Aetheria

### 2019-08-20
[Ripley]
* Add global configuration for SpellComponentsRequired.
* Add UseCreateItem + UseCreateQuantity support to Gems.
* Update TakeItems msg to match retail messaging and support "Take All".

[gmriggs]
* Updated AttributeMod for finesse weapons
* Added /createinst for content creators to spawn landblock_instances in-game
* Added EmoteType.RemoveVitaePenalty
* Fixed some gaps for swapping weapons after launching a projectile

[Mag-nus]
* Fixed landblock monitors

### 2019-08-19
[Ripley]
* Fix issue with creatures dropping their wieldables to corpses.
* Fix some bugs with Pickup and Wield.

[gmriggs]
* Added content export commands (/export-json, /export-sql) for content developers
* Fixed some issues with teleporting
* Ensure GetCleaveTarget() doesn't return targets in dying state
* Updated Steel and Covenant Armor

[Mag-nus]
* Added some config.js.example comments
* Relocated landblock tick code
* Added more detailed landblock tick performance measurements
* Added OS and vCPU to /serverstatus

### 2019-08-18
[Ripley]
* Add support for Day/Night GeneratorTimeType generators.
* Fixed issue with accounts that own monarch-only housing to now allow the other characters to swear allegiance.

[gmriggs]
* Added support for Pathwarden Salvage for newer items
* Added warning / lockup prevention for RegenInterval < 0

[Mag-nus]
* Switch TickOutbound back to parallel
* Added some debug messages to GuidManager

### 2019-08-17
[Theran]
* Fix a possible array out of bounds exception in LootGenerationFactory_Magic.cs that could happen with tier 8
* Add OverRobes to clothing drops
* Remove unneeded return for OlthoiKoujiaArmor AL assignment

[gmriggs]
* Reduced log spam for dynamic projectiles unable to enter world

### 2019-08-16
[Ripley]
* Fixed issue with invisible hooks on relinks (buy/abandon): bool/physicsstate mis-matches.
* Fixed issue with RDB on house buy: missing InstanceIID update.
* Fixed issue with logging into open houses: missing IsOpen check.
* Added Destroy() for items given to NPCs that are of AiAcceptEverything type.

### 2019-08-15
[Theran]
* Add over-robes
* Updates from LSD
* First pass for new mobs Snow Tusker, Bak'tshay Soldier, and Rynthid Rager; no spawn data for most of them, so they would need to be manually spawned
* First pass for Graveyard skeleton and wisp mobs and generators for day/night cycle shifting
* Guid corrections
* Modify how the Xik Minru event transpires, in the background; change should be transparent to the player
* Fix mana on PathWarden Trinket
* Add Risen Princess weenies
* Update Tome of Blood and Bone

### 2019-08-14
[dirtyelf]
* Added the ability to create stacks and multiple objects with /create

[gmriggs]
* Improved collision detection for archers
* Added some random null checks
* Fixed some teleport issues

### 2019-08-13
[gmriggs]
* Added DamageType.Undef protection / logging

### 2019-08-12
[dgarson]
* Added command to create named object, with specified count

[OptimShi]
* Fixed some reported issues with the visual display of different layered armor.

[Ripley]
* Updated PreCheckItem to be more specific about inventory/container slots.

[gmriggs]
* Fixed some minor housing issues
* Fixed stiletto animations

[Theran]
* Removed a few retired rares, updated some others that changed wcids
* Added more objects from MoA and updated content from Lifestoned

### 2019-08-11
[Ripley]
* Updated ManaStone objects use message.
* Fixed issue with UnlimitedUse ManaStones.
* Fixed issue with IsDecayable and TimeToRot.
* Restored Casting motion to Vendors that have VendorServices.
* Fix data desync issue with BiotaPropertiesSpellBook Probability.

[Theran]
* Backport Vissidal weenie mods from Lifestoned
* Updated Facility Hub spawn map
* Restored some missing NPCs

[gmriggs]
* Added IsDead check to falling damage
* Added support for spell projectile traps
* Fixed an issue with Tenacity/Blight
  - Updated spell messages to match retail
* Fixed casting gesture for built-in weapon spells

[dirtyelf]
* Updated /god command serializer for better type handling

### 2019-08-10
[OptimShi]
* Updated drop location for several recall spells to match end of retail

[Ripley]
* Wire up `bestow` and `remove` for advocate system.
* Updated several misc Advocate systems.
* Fixed issue with encounter overrides not applying to cached value.

[gmriggs]
* Added trophy drop rate config option
* Added SyncLocation() for enter_world()
  - This fixes a bug where scatter generators were sending the wrong position to the client

### 2019-08-09
[Ripley]
* Added Facility Hub Gem to early exit NPCs
* Updated Dark Tree Crystal Mine to MinLevel 100
* Updated Scarlet Red Letters and sentries
* Tweaked Snow Lily Trap

### 2019-08-07
[OptimShi]
* Tailoring cleanup

### 2019-08-06
[Mag-nus]
* Added log4net.config.example

[gmriggs]
* Added tinkering log
* Added RestrictionDB to house combat barriers

[Theran]
* Added versioning and ArmorTypes to Olthoi Armor
* Normalized Olthoi Armor AL values

[Ripley]
* Updated the PK altar MinimumTimeSincePk from 3 hours to 15 minutes to match end of retail

### 2019-08-05
[Ripley]
* Add immediate save to items being picked up from player corpses to avoid falling into void if server crashes before next player save.
* Updated more logging relating to corpses for history tracking.
* Fix issue with EventIsPKWorld.
* Add in NPK grace-period for newly created characters on PK worlds and optional enabling via `pk_server_safe_training_academy` configurable
* Improve handling for world type switches, NPK to PK, PK to NPK, etc.
* Improve handling for player pk status switching and timeouts.
* Added `pk_new_character_grace_period` and `pk_respite_timer` configuration options.
* Restored global PK death messages, player configurable via "Listen to PK deaths messages." character option.

[gmriggs]
* Added house barrier protections for PK/PKLite
* Updated start-server netcore version
* Added handlers for hand/foot armor without damage stats
* Updated PK deaths to only go temporarily NPK from PK battles

### 2019-08-04
[Ripley]
* Update AuthenticationHandler to boot oldest connection to account when new one connects with valid login/password.
 - Added new server configurable property `account_login_boots_in_use`, enabled by default retail rule.

* Restored Morathe to Candeth Keep

 [gmriggs]
 * Fixed GetDamageType() for UAC low power attacks
 * Added support for HomeRadius
 * Refined HandArmor/FootArmor selector
 * Fixed monster location desync after MoveHome()
 * Fixed untrained message for skill temple
 * Persisted allegiance bans and approved vassals to database

### 2019-08-03
[Ripley]
* Add support for AdminEnvirons
  - Wired up support commands `setlbenviron` and `setglobalenviron`
  - `setglobalenviron` is operates exactly as seen in retail worlds. in that it affects all players globally throughout the world, including bug with dungeons.
  - `setlbenviron` operates differently in that it localizes effect to landblock and immediate adjacent landblocks for area-based events.

[OptimShi]
* Updated some properties for Armor Middle Reduction Tool

[gmriggs]
* Fixed some issues with item spells redirecting to unenchantable targets
* Fixed various vital enchantment calc errors
* Removed AttackType.OffhandSlash from Mazules and Flanged Maces
* Added handlers for weapons w/ malformed offhand AttackType data

### 2019-08-02
[Ripley]
* Swap out some properties relating to Local Signals for more specific one found in property buckets.

[gmriggs]
* Improved player melee attack animations

### 2019-07-31
[gmriggs]
* Improved squelch system
* Added support for global /filters by message type
* Removed some redundant broadcasts
* Fixed a bug with some emotes such as *wave* playing multiple times
* Added PK Arenas / PKLite Arenas

[dgarson]
* Added configurable 'player_save_interval' seconds config property for server admins

### 2019-07-28
[Ripley]
* Wire up full support for contract system.
  - This requires a shard update script found in updates folder: `2019-07-22-00-Update_Contracts.sql`

[Theran]
* Separate out undead summon weapons
* Added more weenies, and corrections from LSD
* Added Skittering Mukkir
* Move patch Evolution weenies

[gmriggs]
* Filtering houses from /create and /ci

### 2019-07-27
[gmriggs]
* Added RestrictionDB null prevention
* Clamped local broadcast range for pklite messages
* Added epic / legendary cantrip logging

[Theran]
* Removed loot gen properties from quest item

### 2019-07-25
[OptimShi]
* Fix to allow *null* EncumberanceVal items to be carried.

[dgarson]
* Added pkl_server admin config option

[Ripley]
* Add forced save to db for items being moved between players to prevent loss if crash occurs before normal save interval.

[gmriggs]
* Fixed a bug when multiple items level up simultaneously
* Fixed a bug with transferring items from side packs -> main pack @ max burden
* Added prevention / debug code for spellbook probability 0
* Added support for RemainingLifespan

### 2019-07-24
[OptimShi]
* Added support for proper visual display of Layered Armor (Tailoring) and Reduced Armor (Tailoring) as well as future items such as Over-Robes.

[gmriggs]
* Fixed animation bug for unarmed combat - low power, high attack

[dgarson]
* Added IsBusy checks for recalls / PKLite commands

### 2019-07-23
[gmriggs]
* Added universal weapon masteries
* Fixed some bugs for PK/PKLite

[dirtyelf]
* Added /ungod command

[Theran]
* Fixed the deadly prismatic dart recipes

[Slushnas]
* Set NonTracking to True for later arc spells

### 2019-07-22
[Slushnas]
* Added Curse of Raven Fury spell

[dgarson]
* Ensure players are unattackable while in portal space
* Removed attribute cantrips from jewelry loot generator
* Include material type in give item message

[series8217]
* Fixed string buffer overrun / leaked memory in packet logs

[gmriggs]
* Updated war/void magic projectile skill damage bonus to match retail
* Syncing kill task shareable range with radar
* Capped passup xp display to uint.MaxValue to match client
* Added Critical Protection Augmentation message

### 2019-07-20
[gmriggs]
* Fixed a StackSize 0 bug in WieldedTreasure
* Fixed Thrungus special attack
* Added support for Ebon Rifts multiple damage types

[series8217]
* Improved database setup instructions in Readme.md

[Theran]
* Fixed physical damage on Wave and Aqueous Golems
* Add the four undead mobs for Vissidal from LSD
* Add new treasure table entry used by Ghastly Priestess and Shambling Adherent
* Update one of the Vissidal landscape generators to include the undead for spawning near the Temple of Xik Minru

[Slushnas]
* Updated number of projectiles for Curse of Raven Fury to match retail pcaps
* Adjusted drain and damage modifiers to match retail spell description

### 2019-07-19
[Ripley]
* Fixed Black Marrow Keyring Recipes

[gmriggs]
* Added optional debug info for RecipeMods
* Updated mana conversion to better match retail
* Suicide refactoring
* Fixed a weapon swapping client bug
* Improved calcs / appraisal info for Spirit Thirst

[Theran]
* Cantrip and weenie updates
* Temple of Xik Minru portal location update
* Update Jedetj Eckhart and add more Vissidal region spawn data
* Correct Dar Rell speech emote formating
* Fix portal spell and move chest

### 2019-07-18
[gmriggs]
* Fixed a bug with monster Ranks -> InitLevel
* Adding Surge of Regeneration tick messages
* Added Medicated Healing Kits to Rare T3 tables, and elixirs to Rare T2 tables
* Scaling SrcVital to DestVital for life transfer spells

### 2019-07-17
[Ripley]
* Further adjust vendor sell fix to include correct error msg when 0 items are sold as a result of a fail.
* Added EdgeSlide to Aetheria wisps

[gmriggs]
* Improved / refactored enchantment messages
* Fixed a bug with Aetheria DoT durations

[Theran]
* Increased spawns on Vissidal Island

### 2019-07-16
[Ripley]
* Fix issue with HousePortal permissions, IsOpen saving.

[gmriggs]
* Adjusting epic cantrip drop rates
* Added support for HealOverTime enchantments
* Fixed a bug where Aetheria could be wielded into unopened slots
* Update quest XP test notification messages
* Fixed a bug where bows and thrown weapons were showing (based on STRENGTH 100) during appriasal
* Added Spirit Thirst cantrips to LootGenerationFactory

### 2019-07-15
[Ripley]
* Various updates to Housing objects related to assessment
* Increase guestlist to 128 to match end of retail number.
* Stored Open status of house in different property to allow for future option to use HouseStatus to turn off rent.

[Mag-nus]
* Added WorldObject info to ActionQueue output

[gmriggs]
* Adjusted Aetheria drop rates closer to retail, fixed drop rate mods
* Fixed a bug with missing ManaRates
* Fixed a bug with level proportional xp

[Theran]
* Updated Creature Combat Skills for MoA

### 2019-07-13
[gmriggs]
* Fixed a bug with item auras

[Theran]
* Updated all weapon aura spells for MoA

### 2019-07-12
[Ripley]
* Added Aetheria quest
* Fix issue with some recipe changes and ObjectDescriptionFlags

[gmriggs]
* Added ObjMaint v3 - much improved object visibility system, improved/clearer architecture, fixes many bugs
* Added /import-json all, and /import-sql all commands for content creators

[Mag-nus]
* Added dungeon landblock counts, and unique connection count to /serverstatus

[Theran]
* Added Aetheria to LootGenerationFactory
* Refactored LootGenerationFactory magic code

### 2019-07-11
[Mag-nus]
* Updated Dat SubPalette and TextureMapChanges properties

### 2019-07-10
[Ripley]
* Clean up error on sell to Vendor fail message.

[gmriggs]
* Refactored ObjectDescriptionFlags
* Added multihouse decomissioning system
* Refactored HouseManager
* Added house_per_char server config option
* Added apartment deed location info

[Mag-nus]
* Upgraded project to .NET Core 2.2.1

### 2019-07-09
[gmriggs]
* Fixed some bugs for two-handed weapons
* Improved /delete for admins

[Theran]
* Updated Aetheria items
* Added Vissial Island spawns

### 2019-07-08
[Ripley]
* Add support for a version command response. Gives a basic idea of what version database is running currently and if running in debug/release mode.

[Mag-nus, gmriggs]
* Fixed a memory leak in the object visibility system

[gmriggs]
* Converted HandleAugsForwardCompatibility -> SetInnateAugs during character creation

[Theran]
* Added Tether and Core Plating recipes

[Mag-nus]
* Improved consistency for InventoryLoaded flag

### 2019-07-07
[Ripley]
* Only allow use of Hookers on Hooks.
* Fix some issues with using certain Hookers that were previously broken.
* Fix issues with Books on hooks, Chalk Boards now writable for all as expected, all other books readable.
* Fix some miscellaneous Aetheria issues.

### 2019-07-06
[Ripley, Mag-nus]
* Add `2019-06-10-00-Add_Fields_To_Account_Table.sql` to Auth Updates folder. Servers are required to update Auth database with this script.
* Rescaffolded to add in new fields to Account.
* Add in creation ip/time and last login ip/time to account creation/login.

[gmriggs]
* Fixed a bug where Gharu'ndim and Empyrean caster appraisal wasn't showing Heritage weapon mastery bonus

[Mag-nus]
* Updated documentation to latest version of MySQL

### 2019-07-05
[gmriggs]
* Refactored healing ratings
* Additional IsBusy checks

### 2019-07-04
[Ripley]
* Add DoNothing ActOnUse to CraftTool.
* Adjust EmoteManager
  - Wire up EmoteType.LocalSignal.
  - Wire up EmoteCategory.ReceiveLocalSignal.
  - Adjust EmoteType.Activate to fall back to linked generator if no activation target is specified.
* Add Landblock.EmitSignal to support localized object interaction.

[Mag-nus]
* Updated PlacementPosition types

[gmriggs]
* Improved handling for leveling up items casting spells

### 2019-07-03
[dirtyelf]
* Updated Siraluun weapons

### 2019-07-01
[Ripley]
* Fix issue with converting books from JSON to SQL.
* Adjusted some debug output.
* Add overridden properties for region encounter generators.
  - override_encounter_spawn_rates
    - encounter_regen_interval
    - encounter_delay
* Allow camp generators to despawn
  - When a full "camp" is wiped, if the camp has a parent generator, destroy it so the parent generator can roll the slot again.
* Added data error protection for treasure data in generator profiles.

[gmriggs]
* Added account-wide house recall / account house permissions
* Added JSON weenie import, and live editing features for content creators

### 2019-06-30
[Mag-nus]
* Renamed Envoy references to Sentinel

[Ripley]
* Updated Knight's Stash, and some other generator profiles

[gmriggs]
* Fixed a bug with handing equipped items to Town Criers

### 2019-06-29
[Ripley]
* Clean up issue with Weenie Cache corruption via admin commands/player creation.

[gmriggs]
* Fixed some bugs for PKLite

### 2019-06-28
[Theran]
* Fix issue with invisible Acid hotspot
* Add quest flags, update emote tables to allow reentry/reuse of Gatekeeper crystals. This is not retroactive to those who have already completed it. Turning in those required items will fix it for those characters going forward.
* Update Water and Flame Guardian npcs
  - Fix clothingbase/palette template for appearances

[gmriggs]
* Fixed various inventory stack bugs

### 2019-06-27
[OptimShi]
* Fixed issue with Recipe Manager "CopyFromSourceToResult" not using the player as the source.

[gmriggs]
* Fixed a bug with corpse/chest inventory closing when trying to loot from magic combat mode
* Log cleanup

### 2019-06-26
[Mag-nus]
* Additional network fixes
* Updated CommandParser for better handling of multiple arguments with spaces

[gmriggs]
* Fixed a bug where irresistible spells could be resisted
* Indexing biota_wcid db column, improved mansion/villa basement loading speed

### 2019-06-25
[Theran]
* Vissidal flagging quest
* Fix give chests spamming server logs about MaxGeneratedObjects being less than InitGeneratedObjects
* Update Tibri the Cavedweller and Peng-Ya
* Add missing MaterialType to Foolproof Salvage

[Mag-nus]
* Added multi-homed internet support

[gmriggs]
* Fixed item proc sources and messages
* Fixed edge case for equipping thrown weapon w/ dual weapon wielded
* Added defense spec bonuses

### 2019-06-24
[Ripley]
* Update GetCachedInstancesByLandblock for external exporting

### 2019-06-23
[Ripley]
* Fix issue of weenie cache corruption, setting the weenie's weenietype instead of biota's weenietype
* Ensure subbed combatpets are always melee-ing by not spawning any inventory/weapons.
* Generator system tweaks
  - Carry over Delay from profile template correctly.
  - Update RemoveQueue info in GeneratorDump.
  - Adjust CurrentCreate to be calculated to prevent out-of-sync issues.

[nwacks]
* Fixed a bug where stamina/mana kits could not be used at full health (was performing wrong vital checks)

[gmriggs]
* Additional refactoring for healing kits

[dirtyelf]
* Updated Nullify Item Magic scroll drop

### 2019-06-22
[Ripley]
* Expand `qst` command.
* Code fix for out of order emote sets.
* Fix issue with food consumables ObjectDescriptionFlag.
* Fix issue with certain doors.

[gmriggs]
* Updating outdoor house references for unloaded landblocks

### 2019-06-21
[Ripley]
* Generator system revamp
  - Add Landblock_Tick_GeneratorRegeneration
  - Split up Generator Regeneration and Heartbeats
  - Notify Geneator of Pickup of Landblock Stackable
  - Add RegenerationTimestamp and GeneratedTreasureItem properties
  - Add InitializeGenerator
  - Remove IsLinked
  - Change SelectProfilesInit to use GetMaxObjects
  - Change SelectProfilesMax to use GetMaxObjects
  - Update GetMaxObjects
    - -1 MaxCreate == Fill up all slots
  - Renamed HandleStatus to HandleStatusStaged
  - Restore previous HandleStatus
  - Update StartGenerator
    - GeneratorInitialDelay > 0: offset NextGeneratorRegenerationTime
    - else if InitCreate > 0: Regen
  - Add GetNextRegenerationTime
    - If generator isn't previously loaded, skip delay
  - Update DisableGenerator
    -  Add ProcessGeneratorDestructionDirective
    - Support GeneratorDestructionType and GeneratorEndDestructionType
  - Update AddGeneratorLinks
    - Increment InitCreate and MaxCreate per profile template spec (placeholder), per each link added
  - Update Generator_HeartBeat
    - Starts/Stops Generator, checks event status
  - Add Generator_Regeneration
    - Queues for Respawn and/or/both Spawns objects
  - Add ResetGenerator
  - Update GetSpawnTime to always return UtcNow
  - Update Spawn
    - If TreasureGenerator is used, set GeneratedTreasureItem
    - If profile PaletteId and/or/both Shade have value, use it
  - Wire up `@regen` command
  - Add `@generatordump` command
  - Change EmoteType.Generate to use Generator_Regeneration
  - Update OnDeath to call OnGeneratorDeath for Generators
  - Update OnActivate
    - Move default action to last action to allow other actions to start before
    - Change OnGenerate to use Generator_Regeneration
  - Update Chests
    - Remove ResetGenerator
    - If Locked disable timed regen of contents
    - Update Reset
    - Override ResetGenerator
    - Update ResetInterval resetting
  - Rename Generator to GeneratorProfile
  - Rename Generator.cs to GeneratorProfile.cs
  - Update WorldObject_Generators.cs
    - Skip PlaceHolder object profiles
    - Change ProcessGeneratorDestructionDirective to not affect dead/dying creatures
  - Update WorldObject_Generators.cs
    - Bad data protection
  - Code fix for linkitemgen2minutes
    - Bad Init/Max for this linkable
  - Add GeneratorUpdateTimestamp
  - GeneratorHeartbeat renamed to GeneratorUpdate
    - This matches property found in enum

* Stop saving Gateway portals to Shard DB
* Add IsGateway to Portal
* Add default sound to Pressure Plates
* Update Corpse Decay Logging
* Do not change default icon unless Shade or Palette was defined
* Add NpcInteractsSilently handling for EmoteType.Give
* Wire up `trophies` command

[gmriggs]
* Fixed highframe bug for level 6 spellcasts
* Fixed some vitae bugs
* Prevent allegiance passup xp from reducing vitae
* Refactored drop item

### 2019-06-20
[gmriggs]
* Added PK timers
* Improved item magic code

### 2019-06-19
[Ripley]
* Fire EmoteManager.OnDrop when an item is dropped to landblock. (Clutch of Kings (Rehir))
* Added Morgluuk Linvak event

[gmriggs]
* Fixed a bug with house payments for offline allegiances

### 2019-06-18
[OptimShi]
* Added code to use the Setup of a piece of clothing to visually equip it if no ClothingBase exists. This specifically applies to Ursuin Guise, WCID 32155, but may apply to others in the future.

[Ripley]
* Prevent using Hookers if house isn't owned or is closed and player isn't on guestlist.
* Fix gems that cast recall spells.

* Fixed doubled up spawns in Matron Hive East
* Minor changes to Destroyed Portals

[gmriggs]
* Fixing fellowship proportional xp sharing

[Mag-nus]
* ConnectionListener: Add NetworkReset error handling

[dirtyelf]
* Added Revitalize Other VI and Nullify Item Magic to loot system scroll drops

[Theran]
* Recipe updates / additions:
  - Retired old Barbed Crop and Sharpened Virindi Scalpel in favor of updated recipes using MoA skill checks
  - Added Foolproof salvage recipes
  - Added Ancient Armor dye recipe

### 2019-06-17
[gmriggs]
* Adding ExperienceHandlingType, ShareType for xp classification
* Updated fellowship quest bonus to match retail, with additional config options
* Refactored consumables

[Ripley]
* Update Vendor Buy/Sell to validate item amounts before creation
* Prevent using Hookers if they aren't hooked.
* Updated Olthoi Hunter

### 2019-06-16
[gmriggs]
* Updated fellowship kill tasks to match retail by default, with additional config options
* More busy state checks during teleport
* Updating allegiance chat channel when breaking from vassals

[Ripley]
* Some changes to Tomb of Adhorix

### 2019-06-15
[gmriggs]
* Improved support for Phantom weapons
* Added missile reload anim speed modifier
* Added burden modifier
* Fixed a bug where Oak Salvage couldn't be applied to melee weapon
* Added support for swearing allegiance to lower level characters
* ConfirmationManager refactoring

### 2019-06-14
[Slushnas]
* Updated chess pieces so that drudges are white team
* Added GameEventAllegianceLoginNotification packet
* Updated Allegiance panel for online/offline status

[Harli Quin]
* Added more info to /debugdamage

### 2019-06-13
[Theran]
* Fix prismatic ammunition
* Continue to add UCoN content and quests
* Fix Dire Mattekar Kill Task quest

[Mag-nus]
* More descriptive exceptions in ConnectionListener
* Added ActionChain performance measurement
* ObjectGuid.ToString() cleanup

### 2019-06-11
[Harli Quin]
* Added /additemspell command for devs/admins - add a spell to an existing item

[gmriggs]
* Added multiple wield requirement checks

[Mag-nus]
* Landblock decay code cleanup

### 2019-06-10
[Mag-nus]
* Updated Physics.WeenieObj to WeakReference<WorldObject>
* NetworkSession.cachedPackets pruning

[gmriggs]
* Added Sandstone Salvage for new items, updated existing Sandstone Salvage bags

### 2019-06-09
[Ripley]
* Prevent using contained Container (R keybind).
* Add Do nothing stub to Generic weenie class.
* Add `requirecomps` command.
* Add SpellComponentsRequired property.
* Add HasComponentsForSpell.
* Add HasComponentsForSpell checks to Player.CreatePlayerSpell for targeted and untargeted spells.
* Fix issue with certain createlist profiles.

[gmriggs]
* Additional cleanup for ignoremagic*
* Added Sandstone Salvage to RecipeManager

### 2019-06-08
[Ripley]
* Add LightSource Weenie class.

[gmriggs]
* Added lock resistance enchantments
* Added /verify-skill-credits
* More refactoring for hollow missile damage
* Fixed a bug with item enchantments not ticking in side containers

[dirtyelf]
* Added /god admin command

[Harli Quin]
* Added more info to /debugdamage

### 2019-06-07
[Ripley]
* Wire up EmoteType.CreateTreasure.
* Adjust ItemMagic/SpellType.PortalSummon to use spell.Link instead of spell.Name.

### 2019-06-06
[Theran]
* Add Laurana to her Cavern home in Glenden Wood; currently using texture update properties to craft the NPC's correct look, until SedgeMail Leather armor is added to the game; will require ACE PR 1968 to make accessible
* Add Lightning Longbow that is wielded by NPCs only
* Add Invitation to Master Fletchers "portal gem"
* Remove few spawns from Glenden Wood due to changes in town, such as doors in the air from a burned out structure
* Correct Sho Pathwarden Chest to hand out correct racial Pathwarden Robe
* Update Pathwardens to have the remaining WS5 granite and WS5 steel salvage at 33% reward on Pathwarden gear turn in
* Add the two Salvage bag items used for the Pathwarden gear turn in
* Add more Under Cover of Night patch weenies

[Ripley]
* Wire up EmoteType.TeleportTarget
* Adjust appraisal code for better NpcLooksLikeObject handling.

### 2019-06-04
[Theran]
* Start adding Under Cover of Night content, including some initial weenies and landblock spawns

[gmriggs]
* Added retained message for sandstone salvage
* Updated crafting chance of success dialog
* Fixed some issues with ivory and leather salvage
* Fixed hollow damage for chorizite missile weapons
* Fixed some issues with emote table RNG

### 2019-06-03
[OptimShi]
* Made a small adjustment to the GetPaletteID function that was, in certain circumstances, returning the wrong palette.

### 2019-06-02
[Mag-nus]
* Move network/packet work from WorldManager to NetworkManager
* Remove unused code from GameActionPacket
* /auditobjectmaint
* add /allstats, make /delete safer

### 2019-06-01
[gmriggs]
* Fixed tailoring for pauldrons
* Fixed some gaps in house abandon / eviction for unloaded landblocks

[Ripley]
* Disable players putting or merging items into corpses. You can only pull from not push to.
* Update logging for decay of corpses.
* Changed Landblock.Tick to not decay on first tick.

### 2019-05-31
[Ripley]
* Add more logging info for player corpses.
* Make sure corpses are saved if created indoors (dungeons).
* Add ResetSkill and adjust EmoteManager to use it for UntrainSkill.

[gmriggs]
* Fixed GetDynamicObjectsByLandblock uint/int problem.
* Adjust Decay for Corpses to ensure Inventory is loaded before decay is allowed to process.

[dirtyelf]
* Fixed spell IDs for Deception Self 7 / Salvaging Self 7 scrolls

### 2019-05-29
[dirtyelf]
* Added percentages for jewelry slot drops,
* Updated percentages to be easier to modify, cleaned up comments
* Moved jewelry items to loot tables for easier migration later

[Theran]
* Added Left-hand Tether and removal wcid and stub recipe for RecipeManager_New format
* Add Asheron's Island entry portal in Eastham and Asheron's Castle spawns
* Add Core Plating Integrator and stub recipe for RecipeManager_New format
* Update ClothingBase of many Isparian weapons
* Update TsysMutationData of 2 handed weapons
* Add wcids for Alchemical Throwing Phials and recipes to support those added so far ( code changes are needed to allow the final throwing phials to work correctly, as specified in ACE Issue 1922 :: ACEmulator/ACE#1922 )
* Various other wcid updates

[Ripley]
* Added Halls of Metos portal near Zaikhal

### 2019-05-27
[Ripley]
* Added database info for RenegadeGenerals.sql

### 2019-05-26
[Mag-nus]
* Improved Shard database indexing.
* Added further stack exploit mitigation and logging.

### 2019-05-26
[Ripley]
* Add QuestManager to Fellowship.
* Add IsLocked to Fellowship.
* Update QuestManager to support Fellowships for basic quest stamping.
* Update Fellowship to support locking via emotes. Allow members who were in fellowship at time of lock to rejoin if they get booted from game.
* Update EmoteManager emotes: InqFellowQuest, LockFellow, StampFellowQuest, UpdateFellowQuest
* Add `forcelogoff` command.
* Add `showsession` command.
* Add log messages to track corpse decay.

[gmriggs]
* Swapped underlay colors for pierce/slash rends to match retail
* Fixed AL on loot-generated clothing

### 2019-05-24
[Ripley]
* Moved MinimumTimeSincePk change upon PK death to occur before flag changes. This fixes issue with PKs recovering from death being able to attack other recovering PKs.
* Add type_value_idx to BiotaPropertiesIID table and Rescaffolded.

### 2019-05-23
[gmriggs, deca]
* Fixed a bug with negative skill #s and proficiency XP

### 2019-05-22
[Mag-nus]
* Added /landblockstats command
* Improved InboundMessageManager network exception handling
* Improved Shard query performance when loading landblocks

[dirtyelf]
* Fixed a bug with IsBusy check for consumables - vtank no longer conumes too much food / drink!

### 2019-05-21
[OptimShi]
* Added Dinnerware to Mundane lootgen.
* Removed Mana Scarabs from lootgen.

[Mag-nus]
* Monster_Tick profiling additions

### 2019-05-20
[Ripley]
* Fix Secure Trade to confirm both parties have enough pack space/burden to accept all items before trade completes.

[deca]
* Update /listplayers to accept optional accesslevel parameter.

[Mag-nus]
* Fixed Chest.RegenOnClose issue with generators

### 2019-05-19
[Ripley]
* Item Pickup fixes.
* Updated Green Garnet salvage
* More Stack Split/Merge fixes.

### 2019-05-18
[gmriggs]
* Fixed weapon tailoring
* Fixed house dungeon portal landblock detection

[KochiroOfHG]
* Improved Getting Started instructions in Readme

### 2019-05-17
[Ripley]
* Apply position corrections for teleports using magic.
* More Stack Split/Merge fixes.

### 2019-05-16
[Ripley]
* Adjust Stack Split/Merge Handling.

[gmriggs]
* Added vendor dupe prevention

### 2019-05-15
[gmriggs]
* Fixed a bug with player giving equipped items to NPCs
* Fixed a bug with dispels only selecting the top layer

[Mag-nus]
* Fixed a rare concurrency bug in PlayerManager

### 2019-05-14
[Theran]
* Adjustment to loot percentages
* Added optional assess creature mod w/ release formula
* Corrected properties on Overlord's Sword
* Updated numerous creature loot tiers, as per wiki

### 2019-05-13
[Mag-nus]
* Added PhysicsObj.Destroy() on AddPhysicsObj failure

### 2019-05-12
[Mag-nus]
* Added ObjectMain.ServerObjects.Count to /serverstatus

[Ripley]
* Add OnGeneration emote handling.

[gmriggs]
* Fixed some issues with chest regen
* Updated fellowship death message
* Added item material to tinkering broadcast message
* Fixed recipe skill check for pre-MoA skills

[Theran]
* Updated / consolidated spell assignment in loot generator
* Fixed house hook items with emotes

[dgarson]
* Added support for lower armor reduction kit applied to leggings

### 2019-05-11
[Ripley]
* Fix issue with `finger` command not showing correct account for character.

[gmriggs]
* Excluded augs from SkillAlterationDevice spec count

### 2019-05-10
[Ripley]
* Change pickup for items to also count as destruction for generators to regenerate. (Branith's Staff linked to a Linkable Monster Generator)
* Allow the few items incorrectly marked as "Treasure" and not "ContainTreasure" or "Contain" to appear on monster corpses.

[Theran]
* Improved / refactored Skill XP Rank increases

[Mag-nus]
* Improve mem leakage by Generators

[dirtyelf]
* Added more scrolls to loot generator

### 2019-05-09
[Mag-nus]
* Fixed concurrency issue in DelayAction

### 2019-05-08
[dirtyelf]
* Added some missing spells to drop as scrolls

### 2019-05-07
[gmriggs]
* Fixed an AllegianceUpdate packet parsing bug for TreeStats / Decal
* Updated Fellowship hashtables to match client order
* Moved Jack Of All Trades aug bonus from Skill.Base to Current

[dgarson]
* Standardized spells and cantrips for armor pieces

[Theran]
* Rein in excessive max mana on loot-generated items
* Cleaning up loot generator system

### 2019-05-06
[Ripley]
* Add support to ACE.Adapter to convert ACE weenies to LSD weenies.
* Updated appearance for Summoning Mastery statues

[Theran]
* Added new spells and cantrips to loot generator
* Added emote tables for Summoning Mastery statues in Arwic

### 2019-05-05
[gmriggs]
* Fix appraisal display for items with built-in defender spells

[Theran]
* Updated Heritage masteries to default masteries for retraining

### 2019-05-04
[Ripley]
* Update object appraisal code to properly show Wield/Activation Requires lines.
* Update RecipeManager for Ivory. Recipe in DB already handles mod application correctly.

[gmriggs]
* Improved trade system to work with additional Decal plugins

[Theran]
* Updates to summoned creatures
  - Update Geomancer Golems to match retail stats
  - Added option for CombatPets to cast spells (disabled by default to match retail)

* Reclassify Shields as WeenieType.Generic and update ValidLocations, for those wcids without correct values for shields

### 2019-05-03
[Ripley]
* Add fix up SQL script (2019-05-03-00-Fix_Biota_Jewelry_WeenieType.sql) for existing servers to run to correct jewelry WeenieType on existing items.
  - This script will only need to be run once and only fixes incorrect objects.

[gmriggs]
* Fixed a rare bug with damage spikes

### 2019-05-02
[Theran]
* Change lvl 8 spell comps from Generic to Stackable
* Change some Jewelry items from Clothing to Generic
* Fix Naughty Skeleton Kill Task NPC and mob emotes to match quest in DB
* Add the seven Geomancer summoning CombatPets

[Ripley]
* Changed House Warning Messages filter.
* Set HousePortal destinations based on data from database with fallback to SlumLord.
* Init House from World DB if not found in Shard DB.
* Updated House SQL writer.
* Update CombatPet to not drop loot, not spawn things.
* Remove Spells, Emotes from CombatPets.
* Add Pet and CombatPet to IsCreature in WorldDatabase load function.
* Always Allow ID for Pets.
* Added StartCooldown to EnchantmentManagerWithCaching.

[OptimShi]
* Fixed GetFreeInventorySlots() counting packs and foci.

[gmriggs]
* Excluded combat pets from cleaving damage
* Fixed extra pack slot aug
* Fixed ordering of enchantment masks on login
* Added missing skills to /buff
* Fixed a bug where summoned creatures would sometimes not spawn

### 2019-05-01
[Ripley, Theran]
* Change RecipeManager.ModifyX to fix dye (and other) mods.

[Ripley]
* Adjust rare generation code to apply expected icon underlay if wrong or missing.
* Adjust corpse world entry to issue rare alert text/sound after corpse is spawned and not before.
* Remove all properties from corpse assessment except those seen in pcaps.

[Mag-nus]
* Reduce Entity Framework biota tracking to only Players. Other objects will be reattached when saved.
* Release NetworkSession resources when a session drops.

[gmriggs]
* Updated stack values for death items

### 2019-04-30
[Theran]
* Update Tusker Guard loot tier profile to T4, up from T3, to match wiki
* Add ability to apply Ivory salvage to Attuned items

### 2019-04-29
[Ripley]
* Add support for preloading landblock groups (Apartment Landblocks)

[gmriggs]
* Fixed apartment landblock loading speed

[Theran]
* Align Scroll loot with wiki

### 2019-04-28
[Ripley]
* Fix issue with attuned items being places in packs, packs placed in other containers.
* Fix double use issue on some containers.

[gmriggs]
* Fixing recipe mods for keyrings
* Fixing some combat lockups after healing, particular with vtank + missile
* Fixing combining older bags of mahogany salvage
* Elemental damage bonus refactoring (thanks to Harli Quinn)
* Removing leader from disbanded fellowship
* More apartment fixes for Empyreans
* Fixing monster retired skill #s

[Theran]
* Removed some incorrectly added thrown weapons from loot gen
* Update Olthoi Swarm mobs for loot tier updates, per wiki
* Add Tomb Rubble weenie

### 2019-04-27
[gmriggs]
* Added possible fix for Mowen portal not spawning - IsBusy reset OnDeath
* Sorting appraisal properties to better match up with client hashtables
* Ensuring self spells are never resisted
* Fixing slippery item drop / rares popping when looted

[Theran]
* Added Covenant Armor, default Material Type, and thrown weapons to loot
* Replace leather sleeves with yoroi

### 2019-04-26
[Ripley]
* Add check for RecallsDisabled to command recalls.
* Add text to corpses that generated rares.
* Add in use error message for some objects.
* Update Storage permission error message.
* Update Healer messags and support Stamina/Mana kits.
* Update Enchantment Heartbeat to expire 0 duration spells.
* Swap out Dread Ursuin for Marae Ursuin in PetDevice

### 2019-04-25
[Ripley]
* Fix issues with crafting sending 0 burden to client.
* Add CraftTool WeenieType.
* Update Generic and Stackable WeenieType.
* Fix issue with clapping motion repeating.
* Fix giving stackables to NPCs that accept everything (Town Criers, Garbage Barrels)
* Fix giving or dropping packs with attuned items.

[Theran]
* Loot refactoring for clothing
* Set CraftTool weenies mistakenly marked as WeeniType.Generic to WeenieType.CraftTool
* Move 13 weenie files to new locations
* Add missing EncumbranceVal to Olthoi Amuli Armor
* Remove bad keyring recipes

### 2019-04-24
[gmriggs]
* Added GetBlockDist pre-check to IsDirectVisible()
* Added /ciloot dev command for RNG loot generation factory

[Ripley]
* Add support for AwardXP emote taking away from available xp (Donatello Linante -> Asheron's Lesser Benediction).
* Fixes for Give to Player issues: Send contents of a container when given, Return object to giver when it fails to be given.
* Fix for mis-matched mana on lootgen'd missile weapons.
* Fix crash on null CurrentMotionState (statues).
* Fix wonky mana on Missile weapons in LootGen

[fartwhif]
* Replaced character errors with confirmed list
* Fixed boot command throwing exception when used at console

[Theran]
* Fix quest names in San Ming's emote table
* Fix ClothingPriority on Olthoi Celdon armor
* Add Donatello Linante

### 2019-04-23
[Ripley]
* Fix issues with player corpse decay.
* Add support for IOU trade ins.
* Updates to Event generators for two staged on/off.
* Add caster effect to spells that have them.
* Creatures with loot and NoCorpse will drop their loot to the ground upon death.

[gmriggs]
* AdjustCells cleanup
* Fixing spirit essence busy state
* Updating tinkering message to match retail, adding /cisalvage dev command
* Fixed a bug with disappearing icon when players give partial stacks
* Added allegiance name to appraisals
* Initial fix for item heartbeats

[Theran]
* Adjust wield requirements and method used
* Add new Fetish of the Dark Idol recipes for new TOD missile weapons
* Add 11 salvage recipes
* Add missing Enhanced Isparian weapon recipes
* Add/update 42 spells
* Update/add four landblocks
* Update Olthoi Larvae body height entires

[Optimshi]
* Re-added random colors to clothing in loot gen
* Fixed gems not dropping in loot.
* Update Gem_MaterialType odds in loot gen

[fartwhif]
* Sequence validation and negative respose

### 2019-04-22
[Theran]
* Update loot generator for light dagger / multistrike
* Add modified tinkering recipes
* Update Atlan weenies
* Add Moina NPC
* Update two Facility Hub Wardens
* Add Olthoi Amuli armor weenies

[Mag-nus]
* Updated DamageHistory to use WeakReference

[gmriggs]
* Fixed Olthoi Rippers spawning too many ninjas
* Fixing /hr for some apartments

### 2019-04-21
[Ripley]
* Updated handling of scribe related functions.
* Fixed more issues with hotspots

[Theran]
* Add missing Missile DamageMod property for loot gen
* Mosswart Worshipper Kill Task fix
* Add missed Void Gem of Enlightenment

[gmriggs]
* Adding WeakReference option to GfxObjCache
* Added door opening ability for some newer Olthoi

### 2019-04-20
[Theran]
* Add shirts and pants to loot drop
* Change weapon damage to be based on wield difficulty, instead of tier number
* Lower essence drop rate to ~17%
* Converted many magic numbers to descriptive, enum based values

* Loot tier adjustments and updates from LSD
  - Adjustments for loot tier 7
  - Add new skills Gems of Enlightenment
  - Update new skills Wardens of Enlightenment to issue their gems
  - Update Enhanced Health Elixir
  - Add Enhanced Mana Elixir
  - Add Redspire Portal Gem that uses newer LinkedPortalOne
  - Update Sanamar Portal Gem to use newer LinkedPortalOne

[OptimShi]
* Added Treasure Material tables and Treasure Color tables to apply appropriate colors and materials to items in the LootGenerationFactory
**Note that this update requires _Database/Updates/World/2019-04-20-00-Treasure-Materials.sql_ to be applied, or ACE World Database v0.9.55 or higher**
* Added "lootgen" Developer Command to aid in testing the loot generator. Currently only applies materials and colors to items; no other properties factored in at this time.

[Ripley]
* Changed shortcut code to fix some collision issues.
* Updated shutdown sequence to support notifications to players at intervals and adjusted shutdown lockout to apply when server is less than 3 minutes from shutdown.
* Fix some issues with hotspots.
* Update myquests output to match plugin regex expectations.
* Fix rapid player corpse decay upon server restart.
* Fix issue with npcs activating other objects.

**Stage 2 Shortcut Bar Fix**
* Prior to update to latest master, re-run `fix-shortcut-bars` and confirm 0 bugged players. If bugged players exist, `git checkout 90c98c76a631382b761b1db49522c16dcf7602de`, follow Stage 1 guide to fix them then continue with update. `git checkout master`
* Apply patch `2019-04-17-00-Character_Shortcut_Changes.sql` in Shard updates to fix issue with shortcuts causing a save to fail for the character.

[gmriggs]
* Clamping broadcast range on some recalls
* Improved monster ReturnHome logic
* Added /myquests player command, and quest_info_enabled admin option
* Physics GC cleanup
* Synced WieldSkillType with WeaponType
* Fixed bug with players giving equipped items to other players

### 2019-04-19
[Ripley, gmriggs]
* Fix issue with non-player objects activating other non-player objects.
* Move some messages to WorldBroadcast filter.

**Stage 1 Shortcut Bar Fix**
  - Update to latest master
  - Start ACE and close world using `world close boot`
    - optionally, you can use `modifybool world_closed true` and restart world as well to ensure no players are connected and online for this process.
  - Run `fix-shortcut-bars` command
    - if there are bugged players, issue `fix-shortcut-bars execute` command
  - Once that is finished, world is ready for stage two update. You will probably want to run the above command again just prior to next update to confirm no bugged players exist.
    - if you set world to closed on startup, you can use `modifybool world_closed false` return world to default startup.

[gmriggs]
* Added more code protection for TOD salvage data

[Theran]
* Remove PropertyInt 159 from base melee weapon weenies
* Change Ruschk Iceberg Key from fixed spawn to generator based

### 2019-04-18
[Ripley]
* Fix bug with picking up items from your own corpse, hooks or storage that had quest stamps.
* Fix bug with naked corpses not appearing correctly.
* Added null check to Corpse.Open
* Updated Player.Examine success conditions

[gmriggs]
* Added busy check for food / consumables
* Fixed corpse null exception
* Fixed physics landblock memory leak
* Added CanAddToInventory / pre-check for Player.Give
* Fixing summoned portal spawning
* Fixing equipped item overlap

[Theran]
* Added missing WieldSkillType to loot generation factory melee weapons
* Update Soldier 31290 with Attackable false and Soldiers 31290, 70035, and 70036 with AiImmobile true
* Correct emote text for FH Warden
* Remove two Encounter table entries, as they cause landscape mob spawns within Fort Tethana and the rebuilt Yanshi

[Mag-nus]
* Landblock Dormancy after 1 min of no player activity
* Reduce rogue physics landblocks loaded by portal destination parsing
* Dispose ShardContext on RemoveBiota

### 2019-04-16
[Ripley]
* Fix bug with IDing items in Trade Window.
* Fix bug with trade causing receiver to turn to initiator when trade starts.
* Added rot after 5 minutes to player sold items on vendors.

[gmriggs]
* Fixed allegiance ranks
* Send RestrictionDB in order of client hash

### 2019-04-15
[gmriggs]
* Fixed some apartment bugs
* Fixed salvage workmanship bug
* Removed deprecated skill check formula
* Fixed player DoT damage history
* Improved fellowship vital update rate / sync
* Ensuring BF_OPENABLE flag for inventory containers
* Fixed AnimationHook and GameActionChatChannel line endings in source repo

[Theran]
* True up some recipes
* Fix body part heights of Fiun mobs
* Add missing Monster Fly Trap weenie
* Update Collectors with changes posted to LSD on 4/14
* Update Cave Penguin Egg, per changes posted to LSD on 4/14, and add missing pickup timer
* Hopefully, sort out Ulgrim's Recall

[Ripley]
* Update for quest changes
* Correct name of Starter Area for OlthoiPlay

[Mag-nus]
* Physics memory related additions
* Fix session timeout values
* Network stability improvements
* Fixing vendor overloads

### 2019-04-14
[gmriggs]
* Added support for new recipe formats
* Added support for broadcasting tinkering result message

[Theran]
* Update landblocks in Stonehold, Nanto, and Tou-Tou
* Add Jaffres Dini's spawn data to Sanamar
* Add Scrivener of the Void to Holtburg, Yaraq, Shoushi, and Sanamar locations
* Added five quest flags
* Updated pickup timers for Grael's Rage untranslated texts to align with info on wiki
* Updated three dungeon entry portals with quest restriction, as specified on portal text
* Updated three NPCs involved in Grael's Rage quest to stamp and erase quest stamps, as appropriate

### 2019-04-13
[Ripley]
* Fix minor issue with scrolls when IDing them.
* Fix major issue with recipes not scoping the requirements and mods to indexes which indicated where the checks or mods applied to.
* Update Crafting SQL Writer, Adapter for above fixes.
- **This change requires you to update to the latest world database release found in ACE-World-16PY-Patches, v0.9.48 or higher**

[gmriggs]
* More hotspot / corpse fixes
* Moved IsBusy check
* Fixing dispel filters
* Updating kill tasks in fellowships
* Further fixes to RestrictionDB
* Fixed tinkering values with pine and gold material
* Trajectory Debug.Assert -> return 0
* Added the ability for any player to loot monster corpses after 2 mins
* Added logging for player corpses

[Theran]
* Update Ulgrim the Unpleasant's location to AB
* Update Ulgrim's Recall scroll
* Update Tusker Wish statues for MoA skills
* Revert unintended SetSantuaryPosition change on three Jonathan NPCs
* Update Flinrala Ryndmad to once again hand out Facility Hub Portal gems
* Update the Temples of Forgetfulness and Enlightenment
* Update 10 F&F kill task NPCs from LSD update on 4/12
* Update Olthoi Slayer and Slasher Carapace and Ripper Spine from LSD update on 4/12
* Update Caliginous Aegis from LSD update on 4/12
* Add Jacob's Axe weenie for loot drop by Hard-Headed Skeleton
* Update Platinum Golem from LSD update on 4/13
* Add the three Trade Alliance sub quest stub quest registry DB entries
* Add updated Free Ride to Sanamar spell (3535)
* Fix SetSanctuaryPosition for Jonathan (29325) for Sanamar Academy instance

### 2019-04-12
[gmriggs]
* Fixing RestrictionDB table size
* Fixing hotspot damage
* Fixing blank allegiance officer title crash w/ decal
* Added fellowship sharing to kill tasks
* Cleaning up log messages for monster-wielded inventory

[Mag-nus]
* Process inbound GameAction packets in order received
* Add biota id:name to db exceptions

[fartwhif]
* Session termination and boot command enhancement

### 2019-04-11
[gmriggs]
* Adusting grant level propotional xp
* Adjusting two-handed stance swapping / slots
* Added AttributeTransferDevice
* Updated fellowship to WeakReference<Player>
* Major refactoring to fellowships to fix concurrency issues
* Removing slash animation from thrust weapons
* Added quest restrictions for summoned portals
* Ensuring impen/bane applies only to enchantable items

[Mag-nus]
* Fixed network session multithreading
* MemoryStream.ToArray() -> GetBuffer()

[dgatewood]
* Updated generated loot values

[Ripley]
* More fixes for player corpse appearance issue

[Theran]
* Fix exit portal from Karlun's Hall to remove level restriction
* Update Marauder Eater to always drop jaw, up from zero chance
* Update Base of the Timaru Plateau Portal to drop player near the NPC that sends the player to the top of the plateau
* Update Jonathan's emote table; token is attuned so cannot be lost. NPC doesn't need to reissue token to player
* Add Mountain Sewer entry portal
* Fix Void Scriveners attacking

### 2019-04-10
[Ripley]
* Update Crafting SQL Writer.
* Add in support to ACE.Adapter for converting GDLe recipes.
* * Apply two more patches in Shard updates to fix issues with certain player appearances causing a save to fail for the corpse.

[Mag-nus]
* Updated various log messages to Debug level
* Code cleanup

[gmriggs]
* Preventing players from deceiving themselves
* Adding support for non-house owner rent payments
* Added Lifestone Sending spell
* Fixed fellowship XP earning distance to be based on distance from XP earner, instead of leader

[Theran]
* Added some missing spawn maps
  - Add new Mountain Sewer, new Yanshi town spawns, Keminub, Iaret, and Sacmisi

* Fix reported issue with corpse at the end of the Lugian Ice Tunnels; ActivationResponse was incorrectly set to 1, instead of 2
* Fix Nullified Statues

### 2019-04-09
[Ripley]
* Fix issue with RealTime generators that have either no StartTime or EndTime set. (eg: Heart of the Innocent Event Gen)
* Apply patch in Shard updates to fix issue with certain object appearances causing a save to fail for the corpse.

[Mag-nus]
* Revert MemoryStream ToArray() -> GetBuffer()
* Fixed GetVariance

[gmriggs]
* Reduced fizzle mana usage

### 2019-04-08
[Mag-nus]
* Updated Generator.StopConditionsInit and StopConditionsMax log level
* Updated Generator.AddWorldObjectInternal failures to Debug
* ProcessPacket performance added to /serverpformance
* Retry failed shard db queries

[Ripley]
* Fix PK status for all objects on radar bug.
* Remove sorting on Generator list

[gmriggs]
* Fixing landblock adjacency sync / visible objs
* Fixing temple spec
* Adding rares to server logs

[Theran]
* Regen weenies for CreateList

### 2019-04-07
[gmriggs]
* More fellowship null checks
* More allegiance fixes
* Fixed a bug with with kill tasks
* Fixed a bug with packet crafting during character creation
* Updated harm spell text color
* Updated null spell -> spell.NotFound
* Ensure cooldown spells aren't evaluated for dispels
* Fixed a bug with scrolls and vtank
* Fixed a null crash with CombatPets

[CrimsonMage]
* Fixed the Mite Queen Staff to drop 1 instead of 250 and 1 Crumbled Note instead of 250.

[Mag-nus]
* Added SpellbookCache

[Ripley]
* Updated LSD converter to support enum shifting
* Fixed an issue with ClassName writer

### 2019-04-06
[Ripley]
* Fix OnDeath crash.
* Adjusted OnDeath to use LastDamager instead of foreach.
* Fix tells issue.
* Updated SummonPortal for Gateways

[gmriggs]
* Add prevention for from being dropped into / through walls
* Added support for prismatic arrows
* Fixed allegiance sync bug

[fartwhif]
* Moved order-insensitive items earlier in packet processing pipeline

[dgatewood]
* Added rares to loot system

### 2019-04-05
[Ripley]
* Add support for Barber NPCs.

### 2019-04-04
[CrimsonMage]
* Added Shoichi for Tusker Guard KT to Lin

[gmriggs]
* Added rollback mitigation for players receiving items from NPCs
* Updated spell component burn rate, and mana conversion for item spells
* Updated 2-handed weapon skill check
* Fixed some situations where multiple deaths could occur at the same time

[Ripley]
* Fix to prevent multiple death stacking. You can only die one time until you resurrect at lifestone.

[Mag-nus]
* Save Player Corpses if dropped.count > 0
* Reject new connections when server shuts down

### 2019-04-03
[Slushnas]
* Fix issue with long chat messages

[Ripley]
* Add disable_gateway_ties_to_be_summonable configurable option.
* Update CreateList output to not sort by weenie class id

[Mag-nus]
* /serverperformance command added. Optional parameters: start, stop, reset

[Theran]
* First pass for regen of Creature and Vendor weenies for fixing CreateList
  - Humans, Acid Elementals, Armoredillos, Aun Tumeroks, and Vendors completed

### 2019-04-02
[gmriggs]
* Fixed skill credit refunds for untraining skills in Temple of Forgetfulness

[Mag-nus]
* Use ServerGarbageCollection instead of Workstation GC

[Ripley]
* Change the way GiveObjecttoNPC and HandleNPCReceiveItem deal with emotes.
* Updated Mad Cow event

[Theran]
* Bump three loot profiles from 4 to, per wiki, as mobs using them should be dropping tier 5 loot
* Update Gold Golem and Banderling Mauler to tier 5 loot profiles, per wiki, as other mobs still using former loot profile ID should remain as tier 4
* Remove extra Ianto and Lady of Aerlinthe's Ornate Chest weenies

### 2019-04-01
[Ripley]
* Add some null checks to try to catch issue with SelectDispel.
* Add logging to Spell.Init
* Add warning to players about potential rollback issues when SaveBiota fails.
* Exclude Cooldowns from HandleMaxVitalUpdate

[gmriggs]
* Updated UpdateMaxVital for some spells
* Added more null checks to Fellowships, ConfirmationManager, and GetDeathMessage


### 2019-03-31
[Ripley]
* Update Logout to include server population and limits when character list is resent.
* Updated @acehelp and @acecommands to not be individual messages, so not to be broken up by other chat spew.
* Added @pop command -- Tells you how many players are online.
* Added @telereturn -- Teleport a character to their previous position saved when using @teletome.
* Added @watchmen command -- Displays accounts of a specific access level.
* Added @finger command -- Displays information about a character and/or account.
* Hide launcher pings and pongs.
* Exclude summoned pets from saving to Shard DB.
* Update PetDevice and Pet with class defaults.
* Change PetDevice to not consume on final use.

[gmriggs]
* Added support for Asherons's Benediction and Augmented Understanding
* Updated monster corpse looting permissions - after the top damager finishes looting, other players may loot
* Fixed a bug with vitae not expiring
* Fixed a salvaging bug with Green Garnet and Mahogany items

### 2019-03-30
[OptimShi]
* Fixed Gear Knights being literal buttheads. (Their abdomen was being swapped with their heads)

[Ripley]
* Update Shard DB Enchantment Registry composite key.
* Fixed CharGen issue for Dual Wield characters. 2x Melee Weapons are created if Dual Wield is trained or specialized.

[gmriggs]
* Additional null checks for allegiances and fellowships
* Fixed a bug where crafting components were being removed from the shortcut bar on usage
* The fellowship 'disband' button when clicked by non-leaders no longer acts as a 'leave' button, as per retail
* Added IsBusy checks to healing and recipe crafting for vtank
* Added option to enable/disable DoT messages
* Updated PKLite messages to match retail
* Fixed a bug where players could cast spells from inside portal space
* Improved allegiance data sync

### 2019-03-29
[Ripley]
* Change action that occurs for selling items to vendors so they appear in the buy window if they aren't destroyed on sell.
* Prevent selling objects (via drag-drop on vendor) that vendor doesn't accept in its MerchandiseItemTypes field. These same items would red-circle using traditional drop into panel method.
* Set minimum for hotspot cycles. Prevents hotspot of doom (HotspotCycleTime == 0)

[gmriggs]
* Improved jump with low stamina
* Fixed Holtburg sentries running in circles
* Fixing chests stuck in open state
* Updated War Magic spell projectile and resisted messages to match retail exactly (this was causing a delay in vtank between spellcasts)
* Added Monarch/Patron/Vassal prefixes to allegiance login messages, updated colors

[Ripley, gmriggs]
* Fixed Cow Tipping Quest

### 2019-03-28
[gmriggs]
* Fixed scroll usage in vtank
* Added /teledungeon command for Sentinels / Admins
* Added support for Encapsulated Spirit
* Added alternate currency for vendors
* Additional fixes for decal crashes when entering Portals, and around Holtburg
* Fixed a bug with items in chests sometimes appearing incorrectly in client

[Jyrus]
* Added IOUs for EmoteType.Give w/ missing wcids
* Emote skill check: ranks -> current
* Updated Active flag and chest logic
* Improved spell duration logic

### 2019-03-27
[Ripley]
* Support AdvocateItem changing/updating Radar Blip Color in similar fashion to retail servers.

[Jyrus]
* Added landblock spawns from Friend and Foe patch

### 2019-03-26
[Jyrus]
* Adding preliminary content for Friend and Foe monthly patch

### 2019-03-25
[OptimShi]
* Appraised items on housing hooks now show the details on the hooked item.

[gmriggs]
* Ensure monsters have a targeting tactic
* Added option to enable chess AI

### 2019-03-24
[gmriggs]
* Added support for Luminance augs
* Fixed an outdoor->indoor visibility bug

### 2019-03-22
[gmriggs]
* Adding support for Enlightenment
* Updating burden from ammo / spell component usage

[gmriggs, Cyberkiller]
* Fixed a bug with run backwards state / jump frozen bug

### 2019-03-21
[gmriggs]
* Fixed a bug with spell trap durations
* Improved handling for edge cases for refreshing spells w/ augs
* Improved chess piece movement

### 2019-03-20

[gmriggs]
* Added Aetheria!
* Additional ratings added to appraisal panel

[Jyrus]
* Added Tailoring NPCs
* Better orgnanization and restructuring for -Patches folders (now grouped by date)
* Updated spell info, added spells for item sets

### 2019-03-19

[Phenyl, gmriggs]
* Added Tailoring. Much thanks and credit to Phenyl for originally authoring this code!

### 2019-03-18

[gmriggs]
* Added spell procs / cast on strike
* Added support for item leveling
* Fixed a bug where enchantments from items were being cleared on death
* More visual updates for dye recipes

[OptimShi + gmriggs]
* Updated barber shop

### 2019-03-16

[Anahera, gmriggs]
* Added Chess. Much thanks and credit to Anahera for originally authoring this code!

[Mag-nus]
* AddWorldObjectInternal fix

### 2019-03-15
[Ripley]
* Exclude Burden in CreateObject messages for Creatures.
* Update Player Description Event to reflect WeenieType and HasHealth accurately and not hard-coded values.
* Changed TrackObject to only send Selectable child objects wielded by their parents (No more oversending of everyone's complete equipibles)

### 2019-03-14
[Ripley]
* Allow passthrough of permaload flag to landblocks loaded by adjacent load.

[gmriggs]
* Improved 'out of missile ammo' animation state / feedback
* More consistency for create lists and moving items to corpses
* Consolidated pyreals in death messages, added corpse_destroy_pyreals server option (defaults to true / end of retail)
* Additional checks for built-in weapon spells
* Added rare timers
* Added some missing types / info to login

[Mag-nus]
* Enqueue action message cleanup

### 2019-03-13
[Mag-nus]
* Fixed the legacy Open/Close virtual functions in WorldOjbect base

[gmriggs]
* Additional error messages for allegaince swearing
* Added local broadcast range to spell words
* Revising indoor fellowship distances
* Continued refactoring of HandleUseOnTarget

### 2019-03-12
[Jyrus]
* Player.HandleActionUseWithTarget refactoring

[gmriggs, Ripley]
* Fix Vendors to handle 0 value items properly in buy/sell.

[gmriggs]
* Added fellowship names to player appraisal panel
* Fixed an invisible player bug when players re-enter visibility to an unmoving player

[Mag-nus]
* Ensuring prior container is closed before opening a new one

### 2019-03-11
[gmriggs]
* Fixed some issues with CombatPet aggro

### 2019-03-10
[fartwhif]
* Network stability:
  - fixed bug causing disconnects due to NAK requests being ignored.
  - fixed bug causing session to enter an unspecified state after connect request packet sent to the client is corrupted in transit
  - fixed bug during handshake causing defunct session to linger
  - fixed bug whenever a bad handshake occurs causing crash
  - added asynchronous verification of encrypted CRCs
  - added checksum caching to ClientPacket
  - added handling of trusted packet with ClientSentNetErrorDisconnect flag
  - added parsing of optional "flow" header data
  - added more network logging
  - removed "generational ISAAC" debugging tools

[Mag-nus]
* Cleaned up UseWithTarget and GameMessageInventoryRemoveObject

[Ripley]
* Changed the way Name property is handled with regards to + (Admin/Sentinel characters).
* Set up basic path to support replicating accesslevel changes on to existing characters.
* Return @rename to functionality.
* Add @pk command.
* Revise @cloak command.

[Jyrus]
* Loot Generator refactoring / organizing

### 2019-03-09
[OptimShi]
* Added LanguageInfo to DatLoader (0x41 in client_local_English.dat)
* Added improved feedback when issuing invalid console commands or using incorrect syntax

[gmriggs]
* Added Natural Resistances for players

[Ripley]
* Added configurable BCrypt WorkFactor for password hashing

### 2019-03-08
[Ripley]
* Exclude GamePiece from saving to Shard DB.

[OptimShi]
* Corrected DatDatabaseType values

[gmriggs]
* Adjusted trained skill check for reading magic scrolls

### 2019-03-07
[Mag-nus]
* Added system for reusing dynamic guids. The database is now queried for guid fragmentation on startup, with support for recycling guids during the game

### 2019-03-06
[gmriggs]
* Added the ability for player to use all Augmentation Gems
* Added /teletome command for admins

### 2019-03-05
[gmriggs]
* Cleaned up welcome message / server mtod
* Added substates to initial player broadcasts
* Added RNG ratings to Summoning essences

[Jyrus]
* Disabled the portal messages to match retail

### 2019-03-03
[Jyrus]
* Added Void Magic scrolls and Summoning essences to loot generator

[gmriggs]
* Fixed a bug with dropping items from scatter generators

### 2019-03-02
[Ripley]
* Added migration coding to support migrating from previous SHA512 Hash/Salt method to BCrypt.
* Script for Updating Auth database added to default passwords to BCrypt.
* Added `passwd` and `set-accountpassword` commands for self-service password changing and admin-only override password changes.

[gmriggs]
* Added support for Magic Professors
* Updated spellcasting / healing movement check messages to match retail
* Updated salvaging result messages

### 2019-03-01
[zegegerslittlesis]
* Changed account authentication to use BCrypt.

### 2019-02-28
[gmriggs]
* Added Linux installation instructions
* Ensuring monsters can damage target

### 2019-02-26
[Mag-nus]
* Introduce ObjectGuid recycling into ACE

### 2019-02-25
[gmriggs]
* Updating item dispel

### 2019-02-23
[Jyrus]
* Update EmoteType.DirectBroadcast to support "<questname>@You must wait %CDtime to collect the <quest item> again."

[Mag-nus]
* Exceptions fixed in LootGenerationFactory

### 2019-02-22
[Jyrus]
* Enable basic functionality for InqOwnItems in EmoteManager, with code suggested by gmriggs
* Fix array indexing in LootHelper ( array indexes begin with 0 )

### 2019-02-20
[Mag-nus]
* Pack space is checked before selling items at a vendor
* Improved CoinValue property

[gmriggs]
* Added centralized damage calculation function

### 2019-02-18
[Jyrus]
* Start refactoring LootGenerationFactory.cs code

### 2019-02-17
[Mag-nus]
* Improved EncumbranceVal and Value properties

### 2019-02-16
[Mag-nus]
* Fixed an infinite loop when using a mana stone that didn't have enough mana to fill up all the items in need
* Items added to the world via the admin command /create no longer decay
* Monsters now Destroy() on death, removing them from the database if they persisted (possibly from /create)

[gmriggs]
* Adding enchantments for items wielded by monsters / NPCs

### 2019-02-13
[Mag-nus]
* Fixed a bug where splitting/merging stacks modified the StackSize in the wrong direction

[gmriggs]
* Updating ClothingPriority, create list selection
* Updating CoverageMask
* Updating allegiance rank, config properties

### 2019-02-12
[gmriggs]
* Adding house maintenance + config options

### 2019-02-11
[Mag-nus]
* Landblocks load their resources async again
* Improved the FindByObject() function to include objects WieldedByOther
* Added caching to a couple more WorldDB housing functions
* Fixed an exception for PropertyManager when requesting the value of a property that didn't exist in the database
* Cleanedup the WorldObject OnLoad, OnAddItem, OnRemoveItem code and flow

[gmriggs]
* Adjusting augs and slots armor
* Added server logic for the remaining player-configurable options
* Added magic-absorbing shields
* Added RNG create lists

### 2019-02-09
[Ripley]
* Changed GenerateTreasure to check for null DeathTreasure and de-duped generate code so that items weren't generated twice upon death.

### 2019-02-07
[gmriggs]
* Adding house deeds
* Adding house purchase requirements w/ config options

### 2019-02-06
[Jyrus]
* Changed many of the if, else if, else statements to switch blocks, as a suggested optimization
* Changed GetMaxDamage() for better scaling of damage levels for tier increases and weapon types
* Changed GetArmorLevel() to GetArmorLevelModifier() for better scaling of armor increases, based upon the default armor level from the armor item weenie, for loot tier increases
* Removed a number of properties that were being set in code, which was a holdover from before TOD weenies were being used
* Add functionality to support Mana Forge Chests in the LootGenerationFactory

[gmriggs]
* Adjusted imbue chance of success
* Fixing Shadow Armor quest

### 2019-02-05
[Ripley]
* Add support for spawning Treasure pile corpses when data indicates it should.
* Fix wield issue with TwoHanded weapons.

[gmriggs]
* Housing permissions sync improvements
* Fixed an issue with TOD monsters spawning with low health
* Adding no-log landblocks
* Adding synchronized ServerObject loading for non-adjacents
* Adding logic for locked doors openable from behind

### 2019-02-04
[Jyrus]
* Modify LootGenerationFactory to support the TOD updated weenies

[gmriggs]
* Adding squelches
* Improved handling for offline houses
* Adding heritage augs

### 2019-02-03
[Mag-nus]
* Write locks on WorldObject property dictionaries are only taken when the property is actually set.

### 2019-02-02
[gmriggs]
* Adding jsminify for config commenting

### 2019-02-01
[Mag-nus]
* Rejecting clients no longer consumes a session

[Ripley]
* Added LastModified to several tables in World Database
  - **This change requires you to update to the latest world database release found in ACE-World-16PY-Patches, v0.9.13 or higher**

### 2019-01-31
[gmriggs]
* Adding remaining Allegiance features:
  - TeleToMansion
  - Allegiance chat channels
  - Allegiance mansion / villa permissions
  - Bindstones
  - Allegiance names
  - Allegiance officers / titles
  - Allegiance message of the day

### 2019-01-30
[gmriggs]
* Fixed a bug where many players were crashing around Holtburg / Town Network

### 2019-01-29
[Jyrus]
* Updated EmoteManager to support Kill Task type quests
* Modified EmoteType.AwardLevelProportionalSkillXP and EmoteType.AwardLevelProportionalXP to use BiotaPropertiesEmoteAction.Max64, instead of BiotaPropertiesEmoteAction.Max

### 2019-01-28
[someodtech]
* Added all of the Epic/Legendary cantrip spells

### 2019-01-27
[gmriggs]
* Adding messages for unaffected PK targets
* Adding shop generators for vendor
* Improved enchantment layering, and refreshing for item spells
* Improved cooldown interface
* Adding heritage damage ratings

[Jyrus]
* Added TOD spells: Augmented Understanding, Facility Hub Recall

[Ripley]
* Updating DownloadACEWorld.bat with latest ACE-World-Database release (0.9.12)

### 2019-01-26
[gmriggs]
* Fixing house permissions w/ basements
* Added FailProgressCount support for monster logic
* Added cooldown enchantments
* Added /hslist (housetype) to show list of available houses.

[Mag-nus]
* Decouple IsDynamic from ShouldPersistToShard
* Updating base assemblies from x64 to Any CPU. ACE.Server still builds as x64
* Improved generator heartbeats
* Ensure WorldObjects are fully constructed before first heartbeat runs

[Ripley]
* Removed Burden field from portal appraisal
* Added Shade/Palette to Vendor ItemProfiles
* Improved corpse looting permissions data storage
* Added AiOptions, TargetingTactic, and Tolerance enums to commented SQL output

### 2019-01-25
[gmriggs]
* Refactoring Activate -> Use architecture

### 2019-01-24
[Mag-nus]
* Allow rot for generated items

[gmriggs]
* Ensuring monsters spawn on walkable slopes
* Fixed a bug where Mattekars couldn't be hit with high attacks

### 2019-01-23
[gmriggs]
* Improved scatter generators
* Fixed some issues with auras cast from multiple sources
* Added support for swapping dual weapons between hands

[Jyrus]
* Improved handling of unknown spell ids
* Refactored UseTimestamp / ResetTimestamp, fixed all compiler warnings

[Mag-nus]
* Fixed some possible null exceptions in network and action chain layers

### 2019-01-22
[dgatewood]
* Separated out magical, non magical, and mundane items from loot generation.
* Corrected some quest items that were appearing in random loot.
* Corrected Spells ID's in spell tables.
* Updated chests so that it would cast their spell if they have a spell DID.
* Fixed issue with out of bounds error in loot generation.

[gmriggs]
* Added falling impact damage
* Adding text message for mana pool usage
* Adding player movement checks for spellcasting
* Adding scribing abilities
* Reworking IsEnchantable for unenchantable items
* Adding monster targeting tactics

[Mag-nus]
* Improved network efficiency
* PropertyManager.ResyncVariables() on shutdown

### 2019-01-21
[gmriggs]
* Re-syncing vendor rotation
* Improved chest generators
* Improved built-in spells for casters

[Mag-nus]
* Refactored the core object ticking architecture for improved performance

[Jyrus]
* Updating EmoteManager for support for TOD data

### 2019-01-20
[Mag-nus]
* Adding LastUnlocker for chests

[Ripley]
* Adding dot_duration to spell table

[gmriggs]
* Adding fellowship spells

### 2019-01-19
[gmriggs]
* Adding vendor services, and vendors casting spells
* Adding summoned portal ties

### 2019-01-18
[gmriggs]
* Adding server persistence for spellbook filters
* Updating ObjDesc for items on housing hooks
* Adding physics simulation for corpses. This fixes corpses getting stuck in walls
* Improving portal recall
* Removed Stuck check from IsDecayable
* Added /fillcomps persistence to server

### 2019-01-17
[gmriggs]
* Added instant vital updates for fellowships
* Fixed Oswald's Dirk Quest
* Improved allegiance XP passup

[Mag-nus]
* Added code to ensure all exceptions are logged when using start_server.bat
* Added adapters to convert between different data formats (json)

[dgatewood]
* Separated out magical, non magical, and mundane items from loot generation.
* Corrected some quest items that were appearing in random loot.
* Corrected Spells ID's in spell tables.
* Updated chests so that it would cast their spell if they have a spell DID.
* Fixed issue with out of bounds error in loot generation.

### 2019-01-16
[gmriggs]
* Added loot sharing to fellowship system
* Fixed Asuger Temple for Elysa's Favor Quest
* Updated housing hooks for inventory refactoring

[Ripley]
* Remove duplicated data from the Spell table in the World Database
  - **This change requires you to update to the latest world database release found in ACE-World-16PY-Patches, v0.9.10 or higher**
* Renamed a column in Recipe Mods table in the World Database.
* Changed Emote.Display column to bool in the Shard and World Databases

### 2019-01-15
[gmriggs]
* Fixing armor coverage in slots inventory
* Fixing monsters alerting unattackable friends
* Improved monster navigation between indoors / outdoors
* Fixing /buff command for Hermetic Link
* Refactored the salvaging system
* Fixed spell projectiles not working in Black Dominion dungeon
* Improved EmoteManager timing

[Mag-nus]
* Switching to NonCombat when dequipping / swapping ammo
* Removing missiles from child lists

### 2019-01-14
[Jyrus]
* More Atlan and post-Training Academy content
* Added Pathwarden chests

[Ripley]
* Updating DownloadACEWorld.bat with latest ACE-World-Database release (0.9.9)

[gmriggs]
* Updating object decay for PropertyBool.Stuck
* Adding gem activation requirements, and cooldowns
* Fixing monster corpse spawn bug

### 2019-01-13
[Mag-nus]
* Adding DequipWithNetworking on final missile launch
* Adding RemoveFromInventory before TryEquip
* Fixed giving items back and forth to handle GameMessageDeleteObject instead of GameMessagePickupEvent
* More profiling-based adjustments

### 2019-01-12
[gmriggs]
* Adding enchantments for Hermetic Link / Void, Spirit Drinker / Loather
* Adding the remaining wield requirement checks
* Adding cantrip imbues
* Adding vendor handling for unsellable items
* Adding max missile range

### 2019-01-09
[Ripley]
* Updating DownloadACEWorld.bat with latest ACE-World-Database release (0.9.8)

[gmriggs]
* Adding proper handling of KillTaunts
* Fixing monsters with thrown weapons and shield

### 2019-01-08
[gmriggs]
* Added imbues to damage formulas: Critical Strike, Crippling Blow, Armor Rending, Resistance Rending

[Jyrus]
* Updated Academy Weapon recipe IDs to match the data from the Lifestoned team

### 2019-01-07
[Ripley]
* Updating DownloadACEWorld.bat with latest ACE-World-Database release (0.9.7)
* Changed the way recall/summon portal magics work
  - Instead of storing positions, store WCID of portal as a DID property
  - This allows us to do proper cloning of portal for use checks
  - Refactor SummonPortal to use proper clone of portalgateway
* Update EmoteManager to use TryCastSpell and differentiate between CastSpell (with windup motion) and CastSpellInstant (without windup motion)
* Update Portal to process Portal emotes post teleport (successful portal)

[Jyrus]
- Update to Samuel
- Add Blackmoor's Favor gem
- Completed the remainder of the Training Academy! This includes quests, special vendor, NPCs, and granted items.
- Updated base Atlan Weapons

### 2019-01-06
[Mag-nus]
* Fixed an issue when trying to pick up a stackable that you alrady have a parital stack of
* Fixed an issue with ConsumeFromInventory not consuming 1 item

### 2019-01-05
[Mag-nus]
* More profiling based improvements

[gmriggs]
* Adding summoning XP, refactoring XP granting for consistency / common methods
* Added built-in spells to casting items

### 2019-01-04
[gmriggs]
* Adding handler for EmoteSet.Refuse for NPCs
* Adding LocalBroadcastRange for chat messages
* Improved monster CombatManeuvers w/ multistrike weapons
* Porting AllegianceManager to use the new PlayerManager interface

[Ripley]
* Fixed double-linked trapdoor switches

[Mag-nus]
* Massive cleanup of ObjectGuid usage through the system

### 2019-01-03
[Mag-nus]
* Major Inventory Refactor
  - This fixes many of the inventory-related issues
  - It also simplifies the inventory code quite a bit!

[Ripley]
* Changed the Enchantment Registry in Shard Database to use a composite key instead of a single primary key for each record.
  - This eliminates, in theory, record caps for this table.
  - More tables in Shard will be converted to this format if this proves stable long term
  - **You must apply the 2018-12-30-00-Enchantment_Registry_Comp_Key.sql script to update your shard database**

[gmriggs]
* Improved enchantment layering
* Removing deception check from assessing NPCs

### 2019-01-02
[gmriggs]
* Updated Readme for additional clarity w/ installation instructions
* Added handlers for drop/destroy on death items
* Fixed prices for vendor trade notes, added Buy/Sell/Close vendor emotes
* Improved monsters returning to home position

[Mag-nus]
* Fixed a possible stall in GetCombatManeuver

### 2019-01-01
[gmriggs]
* Fixed a possible race condition with adjacent landblock loading
* Fixed DefaultCritMultiplier for magic casters
* Added case insensitive quest lookup to EmoteManager / QuestManager. This fixes the Hollow Weapons Quest.

[Ripley]
* Fix issue with switches
  - Changed code to use TryCastSpell so that it works with all schools of magic.
  - Added check for Emote and use Activation Emote if called for.
* Update @delete command to work correctly.
* Update EmoteManager to support InflictVitaePenalty

### 2018-12-31
[Mag-nus]
* Added ACE.Adapter project for supporting Lifestoned json data importing

### 2018-12-30
[gmriggs]
* Adding initial reload animation to atlatl combat
* Fixed vendor prices for stacks. Players no longer get ripped off (as much)
* Upgraded RecipeManager, added Tinkering and Imbues to crafting system
* The emulator now has 100% of the player skills available!

### 2018-12-29
[gmriggs]
* Adding house portals / dungeons
* Added support for mansions
* Updated food consumables for spellcasting
* Improved healing other target distances and animations
* Improved item spell handling for NPKs and monsters
* Fixed a bug where item buffs could be cast on monster-wielded items
* Added skill checks for assess person / creature
* Added support for monsters using multistrike weapons (properly)
* Fixed a bug where monsters could wield shield with bow equipped
* Fixed a bug where monsters could slash with non-slash weapons, and pierce with non-pierce weapons.
- Removed EdgeSlide property from monsters. Now they will jump down from ledges to chase the player

### 2018-12-28
[gmriggs]
* Added complete PKLite system
* Added config option for PK-only servers.
  - Server admins can change the PK status of the server via /modifybool pk_server true|false

* Added hollow weapons, and unenchantable items

* Added house guest and storage permissions, open/closed, hook visibility, and boot system
  - @house boot - Removes a player from your house.
  - @house boot -all - Removes everyone from your house.
  - @house guest add - Adds players to your house guest list.
  - @house guest remove - Removes players from your house guest list.
  - @house guest remove_all - Removes all guests from your house guest list.
  - @house guest list - Shows the current guest list.
  - @house storage add - Gives a player permission to use your house storage.
  - @house storage remove - Removes permission to use your house storage from a player.
  - @house storage remove_all - Removes all storage permissions from guests.
  - @house open - Creates an open house.
  - @house close - Closes your house.
  - @house hooks on|off - Makes the hooks in your house visible or invisible.

* The shard patch for this update is located in: Database/Updates/Shard/2018-12-29-00-House_Permission.sql

### 2018-12-27
[gmriggs]
* Improving monster AI for ranged spellcasting

### 2018-12-24
[gmriggs]
* Fixed the elusive monster stuck / falling bug

[dgatewood]
* Updated LootGenerationFactory with item updates

### 2018-12-23
[gmriggs]
* Fixed the elusive null position bug, once and for all.
* Fixed some NPCs not returning to default MotionStance.
* Improving monster AI to send alerts to nearby friends
* Monsters will now return to their starting positions if they wander too far away from home.
* Added 'Aura Other' spells to 16PY-Patches
* Fixed a bug with some enchantments not being registered properly
* Updated PlayerManager to work with both Player and +Player formats sent by client.
  - This fixes some issues across the codebase, such as /tell with admin characters

[gmriggs + Optimshi + Miach]
* Improving per-animation attack frame timing for players and monsters.

### 2018-12-22
[dgatewood]
* Updated some of the items in LootGenerationFactory

[gmriggs]
* Added lifestone protection system
* Fixing PK death message broadcasts
* Added instant updates to player health bar changes for selected target

[gmriggs + fartwhif]
* Adding sticky melee distance to charge attacks

### 2018-12-21
[gmriggs]
* Added full PVP damage formulas / calculations for physical combat

[Ripley]
* Fixed a bug where server wasn't reporting back to ThwargLauncher correctly
* Added Session.BootSession to expand DropSession beyond "Network Timeout" and allow for launcher pings.
* Added EmoteType.SetSanctuaryPosition (No more free rides back to Training Academy after leaving)

[fartwhif]
* Redesigned the project Random class -> ThreadSafeRandom
  - Replaced the thread discriminator with a lock

### 2018-12-20
[gmriggs]
* Improved corpse system, added /corpse, /permit, /consent
* Added function to convert landblock coords into map coordinates
* Added destination coordinates to portal appraisals
* Added LastOutsideDeath to login packet
* Added UpdatePosition opcodes / sequences
* Fixed a bug with InboundMessageManager displaying the wrong opcodes for GameActionType
* Improved corpse looting permissions system - now adds player /permits
* Added /corpse command
* Added LootPermission table for players to /permit other players to loot their corpses.
* Added handlers for commands:
  - /permit add &lt;namegt; - Allows another player to loot your corpse.
  - /permit remove &lt;namegt; - Removes permission to access your corpse from the named character.
  - /consent on - Turns on your ability to accept permissions from other players.
  - /consent off - Turns off your ability to accept permissions from other players. (default)
  - /consent who - Lists those who have given you permission to loot their corpses.
  - /consent remove &lt;name&gt; - Removes the permission a player granted to you.
  - /consent clear - Clears your entire consent list.

* Improved housing system
  - Added handler /house abandon
  - Added /house recall, /hr
  - Improved animations for placing items on hooks
  - Updated apartment maintenance
  - Added SlumLord 'on/off' activation, indicating house ownership
  - Added player names to house appraisals
  - Fixed a bug where hooks/storage were not immediately available for player

* Improved /tele &lt;mapcoords&gt; and minimap click teleporting
  - Teleporting to locations directly inside of buildings now works
  - Teleporting to impassable water landblocks is now detected beforehand

* SequenceManager has been redesigned for keying from PropertyType + Property. This should fix some network issues where the client was previously ignoring some updates from the server
* Added SetMaxVitals to Player on level up
* Additional bug fixes for some objects not spawning in indoor environments

[dgatewood]
* Updated LootGenerationFactory for standardized RNG methods

### 2018-12-19
[fartwhif]
* Improved resiliency for Command parser

### 2018-12-18
[gmriggs]
* Added melee charging attacks
* Added CreateList to monster inventories.
* Adjusted IsExhausted attack penalty to minimum power/accuracy bar, as per retail
* Re-adding physics initial update system.
* Omitting spellbook from creature appraisal. This should avoid a lot of the spam with certain decal plugins
* Fixed corpse looting in vtank

### 2018-12-17
[gmriggs]
* Fixed a bug where Fellowship XP wasn't being calculated correctly for some distances

### 2018-12-16
[gmriggs]
* Fixed various issues with some objects not spawning properly

[Ripley]
* Updated Readme for latest version of ACE-World-16PY-Patches

### 2018-12-15
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-16PY-Patches release for Summoning

[mcreedjr + gmriggs]
* Added complete Summoning player skill system!
* Added full set of Essences. These are currently linking to existing monsters in the game
* Added support for monsters fighting other monsters
* Added support for different aggro profiles for mobs

### 2018-12-14
[Ripley]
* Move Spell enum from server to Entity and rename SpellId
* Updated the AppVeyor downloader for new DB repo format 16PY-Patches
* Updated Readme instructions for new installs

[gmriggs]
* Broadcasting CreateObject / DeleteObject packets on item drop and pickup.
  - This differs from retail, but this fixes some bugs with object visibility when relogging, until the special cases with relogging and ObjMaint are figured out for this

### 2018-12-13
[gmriggs]
* Added spell component burning system
* Fixed a walk / run movement speed bug in multiplayer

[Mag-nus]
* Various improvements resulting from excessive load testing
  - Added thread safety to ObjectMaint / ServerObjects
  - Added a null check before logout removal
  - Cleanup more edge cases with network sessions
  - Cleanup SocketManager
  - Improved Vector3 handling
  - Updated InterpolationManager.PositionQueue from a Queue to LinkedList
  - Updated a couple of Enum.HasFlag's to & != 0 in performance critical sections
  - LinkedList.First != null instead of .Count > 0
  - Other various performance improvements for Debug builds

### 2018-12-11
[Mag-nus]
* Added a null check to LScape.get_landcell() in GetCell()
* Improved graceful handling for logout and session drops

### 2018-12-10
[Mag-nus]
* Fixed a bug where ServerPackets could be built with too many fragments, exceeding the MTU

[gmriggs]
* Fixing IsGrounded for players, and some object visibility issues

### 2018-12-09
[Mag-nus]
* Added /teleallto admin command - teleports all players on server to admin player

[fartwhif]
* Improved network architecture for WAN connections
  - client connections from behind specific rare firewall configurations should have a greater success rate
  - changed port strategy from 2 uni-directional ports, to a single bi-directional port (with the exception of the connect response packet)

* Improved network security by using CRNG init vectors for the CRC stream ciphers

### 2018-12-08
[Mag-nus]
* Improving player logout, save, and session drop flows
* Fixing heritage group skill cost overrides during character creation

### 2018-12-07
[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-16PY release

* Changes to support PCAP export data being used in the database
  - Added PCAPRecorded properties and enums
  - Adjusted CalculatedObjDesc to read and use Biota data if present and no EquippedObjects are found
  - Fixed an issue with /telepoi command

### 2018-12-04
[Mag-nus]
* Improved GetWorldObjectsForPhysics() iteration, and network listener exception handling

[Ripley]
* Updated AppVeyor for unit tests

[dgatewood]
* Fixed a bug where monsters were being incorrectly being reported as 'Killed by misadventure.'

### 2018-12-03
[Mag-nus]
* Performance improvements from profiling:
  - Removed the PhysicsObj != null check from Landblock.GetWorldObjectsForPhysics(). This reduces CPU demand by about 6%.
  - Removed the last of the reflection-based skill lookups. Now all skill lookups go through the portal.dat. This reduces CPU demand by about 2% or more.

[dgatewood]
* Added RNG treasure generation for loot chests

### 2018-12-02
[Mag-nus]
* Added PlayerManager architecture. This manages both online and offline players, and enables various subsystems to access data for offline players in a consistent manner.
* Integrated Friends and Allegiance system with new PlayerManager
* Updated DATLoader to use base DatDatabase for loading Highres and Language data

[dgatewood]
* Fixed a bug with vendors not sending network updates

### 2018-12-01
[gmriggs]
* Improved DAT loader performance and memory usage

### 2018-11-30
[Mag-nus]
* Fixed a memory leak where StaticAnimatingObjects was holding onto PhysicsObj references

### 2018-11-29
[Mag-nus]
* Improved design and performance for SequenceManager

### 2018-11-28
[OptimShi]
* Added RenderTexture, String and Font to DatLoader.FileTypes

### 2018-11-27
[dgatewood]
* Added scrolls to loot profiles
* Added caching system for scrolls

[gmriggs]
* Added stream support to audio export

### 2018-11-26
[fartwhif]
* Fixed a long-standing network bug with lost/corrupted packets dropping clients
  - Fixed a bug causing AcknowledgeSequence packets to increment the LastReceivedPacketSequence, causing the Sequence to break and never recover for that Session
  - Fixed a bug causing RequestTransmit packets to be ignored
  - Improved server resiliency against malformed packets
  - Changed the C2S RequestForRetransmit to per-request packet, instead of sequence
  - Added developer network debug commands
  - Added a generational feature to ISAAC development of network layer
  - Adding pre-processor definition NETDIAG + directives to ACE.Common and ACE.Server to help development by preserving troubleshooting tools, without interference with the optimal solution

### 2018-11-25
[gmriggs, parad0x]
* Updated client lib with features useful for third-party apps
  - Added outdoor landscape texture blending system (ImgTex, TexMerge, LandSurf)
  - Updated LandblockStruct to optionally generated texture UV coordinates for landscape textures. Defaults to off for server environments
  - Added animation-only updates to physics engine

### 2018-11-23
[quinw68]
* Added instructions for installing with VS Community Edition

[dgatewood]
* Added more RNG treasure generation for chests

### 2018-11-22
[gmriggs]
* Improved DatLoader for third-party apps
  - Fixed a bug where Unicode string lengths weren't being read as compressed
  - Added optional flag for caller to select whether or not to load the cell.dat.
  - Fixed a bug where Animation framecounts were being read as uint instead of int
  - Added more enum flags, as per client definitions

[gmriggs + Optimshi]
  - Added more DAT fletypes: EnumMapper, StringTable, DIDMapper, DualDIDMapper

### 2018-11-16
[Ripley]
* Fixed an issue with door and chest appraisal with respect to lockpick chances

[Mag-nus]
* keepOpen option added to DatDatabase FileStream - improves DatLoader performance
* Added Property dictionary system - reduces CPU needed for handling properties by about 80%
* Removed ActionQueue.Dequeue architecture, as it was unused. This change reduces the CPU ActionQueues require by half.

### 2018-11-15
[Ripley]
* Make StackSize an int instead of ushort
  - This fixes the housing prices to match retail

[Mag-nus]
* Changed format of ServerStatus, Total CPU Time to match server runtime
* Streamline attributes and vitals
* Cleanup LoadAllLandblocks status output, and improved comments

### 2018-11-14
[Mag-nus]
* Profiling-based performance improvements:
  - Landblock LoadMeshes summary improved
  - Added thread safety to LScape.get_landblock()
  - Improved WorldObject.ObjectGuid creation
* Added caching subsystem to EnchantmentManager
* With these recent changes, 50+ active players have been tested and running smoothly

### 2018-11-11
[gmriggs]
* Improved ShadowPart performance

### 2018-11-05
[fartwhif]
* Fixed a bug with using certain mana pools

### 2018-11-01
[Ripley]
* Update EmoteManager and WorldObject Generator with fixes for Anniversary event emotes.

### 2018-10-31
[Mag-nus]
* Retain TimeToRot magic values of 0 and -1. They may have come from the weenie
* Fixes for destroying stacks and consumables

### 2018-10-30
[gmriggs]
* Adding particle emitters and animation to physics system (useful for non-server based projects)
  - Much thanks to Pea and parad0x for their pioneering work to RE these systems

### 2018-10-28
[gmriggs]
* Refactored LandblockManager, improved VitalTick performance
* Added SetupModel caching

[Mag-nus]
* Improved performance of physics entity cache system
* DBObj boxing/unboxing performance improvements
* Improved performance of physics landblock loading / async - 30% faster landblock loading
* Fixed a bug with corpses decaying a Corpse.EmptyDecayTime after they were looted
* Fixed items given to NPCs reappearing back in player inventory

### 2018-10-26
[gmriggs]
* Lugian animation / stance improvements

### 2018-10-23
[Mag-nus]
* Added thread safety to DatLoader.ReadFromDat
* Refactored landblock item -> db persistence system
* Improved server shutdown for db consistency
* Fixed a bug where landblocks were loading equipped items as objects owned by the landblock
* Fixed a possible crash if invalid object added to landblock
* Improved object decayable system:
  - Decayable timer hierarchy now controlled by landblock
  - WorldObject_Decay added to manage decay of all decayable objects
  - WorldObject_Database now has functions to determine which static and dynamic objects should be saved to shard db
* Improved object database persistence system:
  - Performance improvements for player inventory actions
  - Players are now saved to the database on a per-player 5 minute interval. This collects all of their items and performs saves in parallel
  - Landblocks save their items to the database on a per-landblock 5 minute interval. This collects all of their items and performs saves in parallel
* WorldObject.Destroy now destroys contained and equipped items
* PositionType improvements

### 2018-10-21
[gmriggs]
* Fixed an issue with some monsters getting stuck if they wakeup during idle emotes
* Improved monster animation sync between server and client
* Added relative positions to EmoteType.Move

[fartwhif]
* Improved /portal_bypass admin command to ignore quest restrictions

### 2018-10-19
[gmriggs]
* Improved performance of GfxObjCache
* Massive refactoring to the movement and animation systems
  - UniversalMotion has been replaced with a new Motion class, which has a much simpler API
  - Fixed a multiplayer bug where jumping couldn't be seen by other players
  - Added Network structures for all movement and animation related packets in Network.Motion namespace
  - Fixed a bug where players performing standing longjumps would repeatedly appear to run ahead and snapback in multiplayer
  - Improved parsing of contact/isgrounded/sticky/standinglongjump/ flags in movement packets
  - Added network reader alignment
  - Fixed a bug where creatures were broadcasting UpdatePosition messages when performing emotes while standing still

### 2018-10-15
[gmriggs]
* Added dispels and pressure plates
* Adjusted use radius for some objects to match retail
* Added fishing!
* Refactored EmoteManager to better handle NPCs in busy state
* Added more emotes to support the quest system
* Bug fixes:
  - Fixed a bug where Olthoi Swarm Eviscerator would spawn a quarter staff in its inventory, and attempt to wield it
  - Fixed a bug where some linkable generators were not being classified correctly, and not spawning all of their objects
  - Fixed a bug where '/adminvision on' would not take effect immediately
  - Fixed a bug where larger monsters were not being detected as within melee range of player
  - Thanks to [Jyrus] for testing, finding and reporting these bugs

[Mag-nus]
* Added RateMonitor system
* Additional performance improvements from profiling:
  - NetworkSession GameMessageGroup based bundles switched from concurrent dictionary to an array
  - Landblock ctor async code grouped into single thread. This ensures landblock construction on server while loading assets doesn't consume idle threads, and saves that processor time for more important work in UpdateGameWorld
  - Renamed Session tick functions, and removed the redundant checks from LandblockManager.GetLandblock()

[Ripley]
* Updated DownloadACEWorld.bat with latest ACE-World-16PY release (v0.0.20)

### 2018-10-14
[fartwhif]
* added command verb "list" to developer command /telepoi that (re)caches and lists all POIs.

### 2018-10-13
[gmriggs]
* Adding methods to QuestManager
  - Added quest solve timers to item pickups
  - Added "You've solved this quest too recently!" timer text
  - Added quest restrictions for portals
* Fixed a bug where recipe items could be combined with themselves
* Added ConvertToMoASkill to recipe skill checks
* Added Item Tinkering to skill updates
* Complex quests such as Aerlinthe and Gaerlan's Citadel are now fully tested and completed

### 2018-10-11
[Mag-nus]
* VitalHeartbeat performance optimizations
* Fixed a possible null pointer exception in LaunchMissile

### 2018-10-10
[gmriggs]
* Improved death messages for PK battles
  - Messages added for all possible perspectives: the attacker, the defender, and nearby players
* Fixed @heal to use maximum vitals
* Added @deathxp command
* Added all death messages for players dying to monsters
* Fixed a bug where spell projectiles weren't triggering the possible critical death messages
* Improved the generic creature -> player death pipeline
* Fixed a bug where monsters would continually attack players while still materializing at the lifestone

[Ripley]
* Added @teleloc to SQL writers - now coordinates for every object in the game are visible in all the SQL files

[Mag-nus]
* Improved GameMessagePlayerTeleport packet structure to match retail exactly

### 2018-10-09
[gmriggs]
* Improved player movement when using items (corpses/chests/doors)
* Fixed a bug where player would switch to Peace mode to open containers
* Fixed a bug where player would return to an open container for closing
* Fixed a bug with arbitrary delays for using / rotating towards items (0.5s/1.0s)
* Improved animation timing when using consumables (food/drink)
* Fixed various bugs with player switching to Peace mode to perform various actions, instead of using the current stance
* Fixed a bug where NPCs were running Actions before completing rotation towards player
* Refactored MoveToChain a bit
* Added character titles

[Mag-nus]
* Improved performance with weenie precaching
* Improved performance of landblock loading - portal collision -> portal space -> landblock speed improvements

### 2018-10-08
[gmriggs]
* Added server support for all level 8 spells
* Added complete set of level 8 player spells
* Updated Spell enum to have consistent casing between spell levels
* Fixed a bug in the spell DAT reader for component loss (uint -> float)
* Fixed a 'collection was modified' error during player death while removing enchantments
* Added /dispel developer command
* Added /showstats debug command to show all player attribute and skill levels from server
* Improved /buff command with an optional spell level # parameter
  - Fixed a bug where /buff command was applying multiple versions of the same spells
* Fixed a bug where level 8 spells had their minimum power set to 350 instead of 400
* Fixed a bug where level 8 spell effects were too small
* Added TransferFlags enum for life magic transfer spells
* Fixed a bug where no death message was displayed for death blows from DamageType.Health (harm, drain, life projectiles)
* Fixed a bug where item enchantments were having their ticks applied twice
* Fixed a bug where mages would cast spells before rotating completely to target
* Added level 8 item auras to EnchantmentManager.GetModifier()
* Player magic refactoring:
  - Fixed a bug where players would windup / fizzle if they didn't have enough mana
  - Updated text messages for life magic transfer and boost spells to match retail more closely
  - Fixed a bug where life projectiles were getting the target stats instead of the source stats
  - Updated life magic transfer spells to use TransferFlags

### 2018-10-06
[Mag-nus]
* Added RateLimiter system for precise timing control on server components
* Improved performance for session handlers

### 2018-10-05
[gmriggs]
* Added 2-handed weapons, and cleaving damage system

### 2018-10-04
[gmriggs]
* Added the ItemManaDepleted sound effect
* Updated the low mana warning timer from 30s -> 2m
* Added commas to all numbers >= 1,000
* Added /givemana debug command
* Changed the wording of some message to match retail:
  - mana -> Mana for low/depleted messages
  - added a missing . to the end of item lists

### 2018-10-03
[gmriggs]
* Added multistrike weapons
* Improved dual weapons stat combinations from both weapons
* Imporved weapon swing animation timings for melee combat

[Riaf]
* Fixed a possible server crash if new players try to join an allegiance

### 2018-10-02
[Jyrus]
* Added landblock preloading / permaload system

[Ripley]
* Update Generator InitCreate and MaxCreate from uint -> int

[gmriggs]
* Massive refactoring to Generator systems:
  - Fixed various bugs with generators spawning too many objects, RNG selecting the wrong items, and missing various RNG selection formulas
  - Generators now use RegenInterval heartbeats
* Improved MotionRange broadcast performance w/ distance squared
* Improved performance for landblock objects by staggering the heartbeat offsets
* Added default values for Generator properties
* Moved HandlePhysics and ActiveLandblocks to UpdateGameWorld(), targeted to run @ 60fps
* Updated SessionHandler for improved performance
* Removed calls to legacy Landblock.UpdateStatus() methods
* Fixed a bug where resisted spells weren't alerting monsters
* Fixed a crash for landblocks with no LandblockInfo record

### 2018-10-01
[Jyrus]
* Fixed a bug with Life Magic boost spells pointing to DamageType
* Renamed DamageType2 to VitalDamageType

[Mag-nus]
* Added @propertydump command - displays all of the properties for the last appraised object
* Added PlayerFactory for load testing
* Added @spendallxp developer command

[gmriggs]
* Fixed a generator bug with RegenLocationType.Specific spawning some mobs in different landblocks

### 2018-09-30
[gmriggs]
* Improved monster timing / server animation sync
* Added direct visibility / line-of-sight test
* Added ExecuteMotion, which plays the animation on both the client and server
 - EmoteManager - EmoteType.Motion now goes through ExecuteMotion, so animations like monsters going to sleep (skeletons/golems) will now be simulated properly on the server. This fixes the bug where monsters would start moving while they were still transitioning from sleep->wakeup
* Added more robust logic for animation timing while switching combat stances, for both players and creatures
* Added randomized stack size variances to mob projectiles, based on WieldedTreasure tables
* Improved timing for all monster animation transitions (combat stance, missile launches, spellcasting)
* Added randomized variance to monster melee attack delay (0.5-2s)
* Fixed a bug where monsters selected combat maneuvers that didn't exist in their motion table
* Fixed a bug where monsters would sometimes walk instead of run, when re-chasing targets
* Fixed a bug where CreateObject network messages would be re-sent when monsters swapped inventory items

### 2018-09-29
[Ripley]
* Updated SQL scripts
 - Rebased and updated for MySQL 8.0 + backwards compatibility

* Updated DownloadACEWorld.bat with latest ACE-World-16PY release

[gmriggs]
* Added detection for post-PY16 skills, installation instructions on player login

### 2018-09-28
[gmriggs]
* Added complete Void Magic system w/ all spells
* Major refactoring to game code that uses SpellBase and Spell:
 - a new Spell class has been added for gameplay code, wrapping SpellBase and the database Spell class. All game code that deals with Spells is much easier to write, with significantly less to manage
* Added both server and client formulas for determining spell levels
* Added proper spellcasting wind-up animations based on spell formula scarabs
* Improved the spellcasting wind-up animations to use the exact animation timings
* Improved the damage over time system
* Added Damage Rating and Damage Resistance Rating to all damage formulas (physical and magic combat)
* Fixed a bug where projectiles wouldn't trigger environment collisions for some objects

Note: this patch requires the DB to be updated with Spells and Weenies from ACEmulator/ACE-World-16PY-Patches#4

Thanks to everyone on the ACE development team, and in the Discord development channel for helping with this patch!

[Mag-nus]
* Centralized ACE timing mechanisms

### 2018-09-24
[Mag-nus]
* Added CommandHandlerHelper class

### 2018-09-21
[gmriggs]
* Added Dirty Fighting skill to combat

### 2018-09-20
[gmriggs]
* Added Sneak Attack skill for player combat

### 2018-09-18
[gmriggs]
* Added Recklessness combat skill

### 2018-09-17
[gmriggs]
* Added proficiency points / character points, and skills increasing through usage
* Added TreasureWielded randomized armor/clothing for monsters

### 2018-09-16
[Mag-nus]
* Fix connection failures on initial account auto create session
* Removed warnings on CICMDCommand packets
* Session connecting log level improvements

### 2018-09-15
[mcreedjr]
* Implemented skill redistribution functionality
  - Added the ability for characters to use Gems of Forgetfulness and Gems of Enlightenment
  - Included check when specializing to make sure too many credits aren't already specialized
  - Included check to make sure a lower operation wouldn't violate the wield reqs of a currently wielded item
  - Included check to prevent heritage skills from being untrained

[Mag-nus]
* Usage of DateTime/PhysicsTimer cleaned up across the board

### 2018-09-13
[mcreedjr]
* Fixed a bug where player's trade status was not evaluated properly when entering combat

[gmriggs]
* Player housing updates:
 - Adding the ability to place and save items on floor/wall/ceiling hooks
 - Adding housing storage chests

### 2018-09-11
[mcreedjr]
* Resolve issue 866
 - Prevent players from picking up containers on the landscape that are being viewed by other players
 - Resolve bug where picking up an open pack caused it to remain locked and unusable

### 2018-09-10
[Mag-nus]
* Refactoring and cleanup for GetPosition / SetPosition properties

### 2018-09-09
[gmriggs]
* Added the ability for players to buy houses

[Jyrus]
* Added house portals

[Jyrus + gmriggs]
* Fixed the MoveToChain to stop within accurate UseRadius for all objects

[mcreedjr]
* Revised pass at secure trade
  - Added motion commands for turn to and approach
  - Added check for in progress trade session
  - Matched interactions more closely with retail PCAPs

### 2018-09-06
[gmriggs]
* Added network packet structure for player housing system
* Fixed a bug with indoor scatter generators not getting the new cell

[gmriggs + slushnas]
* Added weapon speed modifiers

### 2018-09-04
[gmriggs]
* Added accuracy modifier for missile weapons

[Jyrus]
* Added missile launcher Elemental damage modifier

[slushnas]
* Fixed serialization of the critical hit field

[Riaf]
* Additional chat channels added (admin, advocate, sentinel, fellow, patron, vassals, allegiance)

### 2018-09-03
[mcreedjr]
* Added secure trade system for players

[gmriggs]
* Improved monster animations / timers for ranged attacks (xbow)

[Mag-nus]
* First round of updates toward new threading model, and less ActionChain usage

### 2018-09-02
[Mag-nus]
* UpdateWorld refactoring
* Improved landblock multithreading

### 2018-09-01
[Mag-nus]
* Fixed a bug with player options not persisting on login
* Removed LandblockBroadcastQueue and LandblockMotionQueue (legacy / unused)
* Removed many unused ActionChains

### 2018-08-30
[gmriggs]
* Added thrown weapons for players and monsters
* Added atlatls for players and monsters
* Added monster wielded weapons
* Implemented monster inventory / weapon switching
* Monster projectiles are now dodgeable
* Fixed a bug with AmmoType not being enumerated as Flags

### 2018-08-28
[Mag-nus]
* Migrate Character tables over to new caching / threading model

### 2018-08-27
[Jyrus]
* Added support for weapon bonuses: Melee Defense, Mana Conversion, Biting Strike, Crushing Blow, Resistance Cleaving, Elemental Damage modifier for casters, and Slayer bonuses
  - Melee Defense bonus
  - Mana Conversion bonus
  - Biting Strike - Increases critical chance
  - Crushing Blow - Increases critical damage
  - Resistance Cleaving - Makes the target vulnerable to the weapon's element
  - Elemental Damage modifiers for magic weapons
  - Slayer - Increases damage to a specific creature type

[gmriggs]
* Added pre-MoA skill conversion to wielded item requirements
* Added enchantments that directly modify vitals / secondary attributes
* Added weapno offense bonuses
* Fixed a bug where players/monsters were greanted Melee Defense bonus for missile attacks
* Fixed the appraisal fields for ammunition
* Added DamageMod for missile launchers

### 2018-08-26
[gmriggs]
* Adding max broadcast range for some chat emotes (Act)
* Adding monster combat maneuvers / special attacks

[Slushnas]
* Added max spell range support for ring spells
* Updated magic casting range checks to match retail servers / client

### 2018-08-25
[Jyrus]
* Fixed a bug for Creatures calling EmoteManager

[Mag-nus]
* Threading improvement for WorldManager
* ActionChain AddDelaySeconds cleanup

### 2018-08-24
[gmriggs]
* Improved body part + attack height formulas
* Fixed some issues with stuck/ falling monsters

[Jyrus]
* Improved system for players giving items to other players

### 2018-08-22
[Mag-nus]
* Added ShardDbContext caching system
* EnchantmentRegistry improvements

### 2018-08-21
[gmriggs]
* Fixed a bug with monsters running away from players
* Added a system for finding the correct cell when spawning new items

[Slushnas]
* Added support for untargeted war spells
* Added war spell projectiles: rings, walls, and blast spells

### 2018-08-20
[Mag-nus]
* Shard Biota & Threading Model Rework

### 2018-08-19
[mcreedjr]
* Added alwaysshowwelcome Boolean config property

[Mag-nus]
* Added database performance tests

### 2018-08-18
[gmriggs]
* Added dual wield combat
* Added weapon swapping between hands

### 2018-08-17
* Fixing PhysicsDesc bitflags - monsters are now recognized by vtank
* Improved network resiliency, recovery from socket exceptions
* Fixed a bug with monster transitions between some landblocks

### 2018-08-16
[gmriggs]
* Complete rewrite of player visibility / broadcast system
* Fixed a bug where portaling could keep the player at original location
* Fixed a bug with player not appearing as pink bubbles on login/portal

### 2018-08-13
[gmriggs]
* Fixed a bug with generators not respawning items
* Fixed a bug with generators not spawning items in containers
* Fixed a bug with generators spawning the wrong items
* Refactored generators

### 2018-08-12
[OptimShi]
* Added handling of Bool.NpcLooksLikeObject to the AppraiseInfo event.

[fartwhif]
* Improved /buff Player command with auras, imp, and banes
* Added /fellowbuff command

[Jyrus]
* Fixed a bug with casting spells on items wielded by other players and creatures

### 2018-08-11
[mcreedjr]
* Added missing comma in updated ShardBase.sql to allow creation of updated Shard DB after Mag-nus' previous PR

[Ripley]
* Improved SQL writers w/ additional comments

[Jyrus]
* Fixed a bug with Admin and Sentinel players in the PK targeting system

### 2018-08-10
[Mag-nus]
* Significantly improved GetBiota performance from the shard by using flags to indicate populated collections.
* Also added Parallel.ForEach support for Shard GetPlayerBiotas.

[gmriggs]
* Adding dungeon position adjustments system. This fixes dungeons where cells have been relocated since PY16 (ie. Burial Temple, North Glenden Prison, and Nuhmudira's Dungeon)
* Adding physics entity caching system. This provides massive improvements with reduced memory usage, especially over the course of a lifetime for long-running servers.

### 2018-08-08
[gmriggs]
* Adding spell components to /comps debug command
* Adding proper object spawning / initial placement system

### 2018-08-07
[Mag-nus]
* Shard schema changed to remove foreign key link between Character and Biota.
* This also significantly changes the relationship of Session/Player/Character objects.

### 2018-08-06
[Mag-nus]
* Change from .NET Core 2.0 to .NET Core 2.1. This requires .NET Core 2.1 x64 SDK.
* Major schema refactoring + changes for World and Shard.
* New PY16 format required

[gmriggs]
* Refactored PhysicsState

### 2018-08-05
[Jyrus]
* Randomize spell death messages
* Refactored PK checks for magic system
* Added support for linking to portals

### 2018-08-02
[Slushnas]
* Added support for Volley spells

### 2018-08-01
[gmriggs]
* Added player stamina usage on melee / missile attack
* Refactored the death system
* Added Damage History system to track damage and healing sources - for corpse looting rights
* Fixed a bug where player death was removing enchantments on the client only
* Improved player exhaustion system. Both players and monsters now receive attack and defense penalties when stamina = 0
* Added attribute mod to monster damage. Some monsters now deal significantly more damage, so beware!
* Fixed a bug where a player autoattack could occur after death
* Fixed a bug where attributes could be debuffed below 10 on the server
* Fixed a bug with MaxVital calculation with vitae
* Updated shield effective angle to match retail

[Jyrus]
* Updated Life Magic code to properly utilize the DamageHistory for tracking damage through harm/drain/heals
* Fixed all compiler warnings
* Refactored spell resistance to common method for all creatures (players / monsters)
* Spell level now determined by Scarab instead of PowerLevel, as per retail servers and client

### 2018-07-31
[Mag-nus]
* Updated all NuGet packages to latest stable. Most notably EF Core 2.1.
* Fixed an issue in ShardDatabase caused by iterating over lazy loaded results.

### 2018-07-30
[Mag-nus]
* GroupChatType enum removed. It duplicated Channel enum.
* Renamed SkillStatus to SkillAdvancementClass

### 2018-07-29
[Jyrus]
* Added GameEventPopupString message to EmoteType.PopUp, when no confirmation is requested, and finish the Confirmation functionality
* Adjust the Welcome message to match retail (with a twist!)
* Added GameEventCommunicationTransientString to Chests, when chests are in the locked state

[gmriggs]
* Improved vital regeneration, stamina usage
* Updated EnchantmentManager queries to use the top layer for each SpellCategory
* Updated WeaponDefense and WeaponOffense enchantments to be additive floats
* Improved vital regeneration formulas / ticking
* Improved healing formulas
* Added player stance and enchantments to vital regeneration (regeneration/rejuvenation/mana renewal)
* Added basic stamina usage on evasion / jumping
* Fixed a bug in Player_Use not picking up or using items correctly

### 2018-07-28
[Mag-nus + Ripley]
* Added Ripley's .sql file generation code to ACE.Database. This will help us import data from any source into ACE EntityFramework objects, and then use these SQLWriters to create a unified output.
* Added and tested TreasureDeath and TreasureWielded SQL writers

[gmriggs]
* Added jump packet to network parsing, EmoteManager cleanup and refactoring
* Fixed a bug with PendingMotions that would cause monsters to run away from some players

[Jyrus]
* Added the ability to give Attuned items to appropriate NPCs

### 2018-07-27
[gmriggs]
* Added full monster movement physics
* Added MoveToManager, PositionManager, and StickyManager integration with ACE
* Enabled /forcepos as default setting
* Improved projectile physics. Projectiles should hit targets much more consistently now
* Fixed a bug in the animation system where objects were jumping to the last frame
* Fixed a bug with some doors not blocking monster movement

[Jyrus]
* Adjusted Emote and Attuned item handling according to PCAP data findings
* Prevent players from unloading on other players' corpses in PVP

### 2018-07-24
[Jyrus]
* Added new EmoteTypes to EmoteManager: PhysScript and Act
* Added a 2-second delay to Recall spells from the monthly patch 'Thorns of the Hopeslayer'

[gmriggs]
* Updated physics system to use proper enums for MotionCommand and WeenieError

### 2018-07-23
[Jyrus]
* Added PortalSending and PortalSummon functionality
* Allow for non-players casting gateways

### 2018-07-22
**ACE-World-16PY world db release v0.0.16+ required with this update**

[Ripley]
* Rebased Shard and World DBs.
* weenie_Class_Id made uniform across all tables, in both DBs, to be a unsigned int32.
* Rescaffolded databases.
  - You may need to drop both Shard and World DB and re-create.

[Mag-nus]
* Added some more client enums ACE.Entity.Enum
* Reworked Ripleys SQL writer code and added it to ACE.Database.SQLFormatters

### 2018-07-21
[Jyrus]
* Added more scenarios for different wield requirements

### 2018-07-19
[gmriggs]
* Fixed a bug with projectile velocity direction
* Fixed multiple issues with Scenery generation. Semi-random scenery objects should be matched up almost exactly to client now

[Jyrus]
* Added item wield requirement enforcement

### 2018-07-17
[Jyrus]
* Redirect impen/banes from player to equipped armor and clothing
* Added proper mana usage for impen/banes cast on player
* Added EnchantmentManager.HeartBeat() for WorldObjects in the landblock ticking
* Added Item Enchantment ticks
* Fixed redirection for some Aura spells

[gmriggs]
* Added shields to acceptable targets for impen/bane
* Added ArmorProfile to Shield appraisal
* Added Spells to Shield appraisal panel
* Update ShieldMod to use shield.EnchantmentManaager

### 2018-07-16
[gmriggs]
* Added TargetManager for server tracking of monster attack targets

[OptimShi]
* Fixed proper case of certain spell words and change how they are loaded for efficiency/ease of use.

### 2018-07-13
[OptimShi]
* Fixed remaining issue with Spell Formulas and non-ANSI characters. All spell formulas now appear to be accurate.

[Jyrus]
* Fixed a player bug with buying some items from vendors
* Updated Enchantment removal when items run out of mana from dispel to RemoveEnchantment

### 2018-07-12
[fartwhif]
* Added magic item persistence tracking

[gmriggs]
* EmoteManager refactoring for giving items to NPCs, receiving quest items in return

[Jyrus]
* Enchantments applied to armor affecting armor stats fixed to display correctly
* Rework AppraisalInfo to differentiate between intrinsic and applied spells
* Wielded weapon debuffs now taken into consideration

### 2018-07-11
[OptimShi]
* Updated recipe manager to properly destroy items in packs (as opposed to main backpack). Added in functionality to handle equipped items in recipes, but there are downstream issues related to inventory management (such as enchantments not being removed)

[dgatewood]
* Added QuestManager saving and loading to the database
* Further integration of the EventManager into EmoteManager
* Added a lot of new emotes to facilitate working quests

### 2018-07-10
[Jyrus]
* Updated Item Appraisal to list all the correct spells, and Auras redirected from the player

[gmriggs]
* Updated physics system to calculate accurate water depth
* Fixed a bug with players and monsters sliding along the edge of cliffs

### 2018-07-09
[OptimShi]
* Fixed a few console commands that either should not run in the console or had no use case if they did.

[Jyrus]
* Removed the unused start items on new player creation
* Fixed Enchantment registry duration and redirection of Aura spells
* Added spell layering to the Enchantment registry

[gmriggs]
* Updated the Object Maintenance system from a server-wide instance to per-player. This should fix the disappearing monsters in dungeons for multiplayer
* Added a call to handle_visible_cells() when an object enters the PVS for a player. This fixes a bug when players/creatures travel long distances to reach a player that has been standing in the same cell for awhile. It should also make the PVS system more consistent in general.
* Added a null check to all references to CurrentLandblock. This should fix a lot of null crashes from the ActionQueue

### 2018-07-07
[gmriggs]
* Fixed various bugs with monsters crossing between landblock and cell boundaries. Monsters should be able to chase the player through dungeons and indoor areas more consistently
* Added monster movement debug commands
* Fixed the missing portals in Hotel Swank
* Fixed a bug with some dungeons having black screens on portal entry

[OptimShi]
* Add SpellCategory enum

### 2018-07-06
[fartwhif]
* Added mana stone usage system

[OptimShi]
* Added "image-export" console function to extract textures from client_portal.dat and client_highres.dat. Run the command with no parameters for syntax.

### 2018-07-05
[Mag-nus]
* Fixed a bug with loading highres and language files from the .dat

### 2018-07-04
[Jyrus]
* Refactoring and improvements to the magic system code
* Added criticals and bonus damage to War magic, as per the 'Atonement' update

[OptimShi]
* Fixed an issue with the Replace DatLoader.Entity.AnimationHook
* Added SoundTable (0x20...) parsing from the client_portal.dat.
* Added support for client_highres.dat and client_local_English.dat to the DatManager, however these files are not required for server operation.
* Renamed DatLoader.Entity.Texture to DatLoader.FileType.RenderSurface and adjusted some structure to be consistent with the client.
* Adjusted DatLoader.FileType.SurfaceTexture to be consistent with the client.
* Added BadData (0x0E00001A) parsing from client_portal.dat
* Added ChatPoseTable (0x0E000007) parsing from client_portal.dat
* Added NameFilterTable (0x0E000020) parsing from client_portal.dat
* Added QualityFilter (0x0E01...) parsing from client_portal.dat
* Added SecondaryAttributeTable (0x0E000003) parsing from client_portal.dat
* Added SkillTable (0x0E000004) parsing from client_portal.dat
* Added highres-export and language-export console commands to dump the contents of client_highres.dat and client_local_English.dat respectively.

[gmriggs]
* Added instant updates to the monster health bars
* Updated the monster aggro / tolerance system

### 2018-07-02
[OptimShi]
* Added UI Window/Saving. Note this requires an SQL update in \Database\Updates\Shard\
* Finished parsing the DatLoader.SpellTable and added in Spell Sets.

### 2018-07-01
[gmriggs]
* Added /forcepos debug command to force the server monster positioning to the client
* Added dungeon landblock detection system
* Improved monster movement: turning, walking, and running speeds from the MotionTable / animation subsystem
* Fixed various bugs in the physics and animation subsystems

[OptimShi]
* Added CP1252 text encoding. Fixes some issues with Spell Formulas when spells used non ANSI characters as well as in sending strings to the client.

[Jyrus]
* Added better detection and handling for harmful spells
* Updated /addallspells to only add spells that are learnable by the player

### 2018-06-29
[fartwhif]
* Added sentinel command /buff [name]

[gmriggs]
* Rebuilt the Appraisal system from the ground up
* Added initial landblock unloading system.
* Added player corpses and death items. Players now lose items when they are killed, with formulas accurate to retail

### 2018-06-28
[gmriggs]
* Fixed a bug with specialized skill calcs

### 2018-06-23
[OptimShi]
* Fixed a bug that left Undead player characters naked, as well as Penumbrean

### 2018-06-22
[gmriggs]
* Added initial Allegiance system
* Improved Fellowship system
* Updated friends list functionality for Entity Framework

### 2018-06-18
[dgatewood]
* Fixed a bug giving wielded items to NPCs

[fartwhif]
* Added ability to /tele players to current location by name

### 2018-06-12
[fartwhif]
* Added network message fragment grouping by queue

### 2018-06-08
[gmriggs]
* Added monster physics-based movement
* Added shield combat

### 2018-06-05
[fartwhif]
* Added server-side request for retransmission. This should make WAN connections more reliable!

### 2018-06-04
[gmriggs]
* Added projectile physics collisions

### 2018-06-01
[fartwhif]
* Added Hotspots - revitalizing wells, damaging acid pools
* Added spell traps
* Updated Random for thread safety

### 2018-05-28
**ACE-World-16PY world db release v0.0.15+ required with this update**

**You will need to update world database with scripts in the respective update folders**
[Ripley]
* Added New Treasure Tables to World Database.
* Scaffolded the new tables.

### 2018-05-24
[Slushnas]
* Renamed GameMessageRemoveObject to GameMessageDeleteObject to avoid confusion with other messages.
* Renamed TryDestroyFromInventoryWithNetworking() to TryRemoveFromInventoryWithNetworking() as that function sends InventoryRemoveObject messages.
* Cleaned up some stack merging code and added support for merging missile ammo to currently equipped ammo.
* Added the ability to set the stacksize of spawned items created with the /ci command.
* Fix for observed players improperly playing last combat mode animation when moving.
* Fixed missile ammo appearing in players hands on login.
* Fixed arrows being fired after switching to peace mode.
* Added ammo usage to player missile attacks.
* Added damageSource parameter to damage functions to better support edge cases.
* Various other minor tweaks to bring sent game messages more in line with retail pcaps.

### 2018-05-24
**ACE-World-16PY world db release v0.0.14+ required with this update**

**You will need drop and recreate both Shard and World databases with this update**
[Ripley]
* Rebased Shard and World DB.
* Rescaffolded both databases.
* Fixed issues #782 and #783.

### 2018-05-23
[mcreedjr]
* Moved addallspells code from a debug handler to the Player object
* Added ability to filter requests to learn spells in bulk by both school and level
* Modified the LearnSpellWithNetworking method to not display Purple bubbles or chat text upon learning new spell

### 2018-05-22
[dgatewood]
* Implemented On-Use Emotes for NPCs. When interacting with a Quest NPC, they will now respond with actions.

### 2018-05-20
[mcreedjr]
* Added the `addallspells` debug command, which adds all in-game spells to the player's spellbook.

### 2018-05-19
[gmriggs]
* Combat: Added Ranged Combat for monsters.
  - Monsters can now attack players with bows, crossbows and thrown weapons.
  - Monsters can now switch between Ranged and Magical combat.

[dgatewood]
* Added the basis for Salvaging.
  - The Ust can now be used to salvage loot items via the Salvaging panel.
  - Implemented formulae to determine the workmanship, material type and the amount of salvage generated.

[fartwhif]
* Further clean-up and polish on Lockpicking and usage of keys on locked chests/doors.

### 2018-05-18
[gmriggs]
* Combat: Added Magical Combat for monsters.
  - Monsters now cast War/Creature/Life spells on players, using spells from their own "spellbook".
  - Added the associated emotes and behaviors for monster spellcasting to match retail.
  - Before exploring Dereth, make sure you've buffed!

### 2018-05-16
[dgatewood]
* Further implementation of Item Giving.
  - Many hand-in NPCs, including Collectors and Gamesmasters, now have basic functionality.
  - World Objects now handle the receiving of items, performing checks to see if an item has been given.

### 2018-05-14
[gmriggs]
* Combat: Added the basis for Melee Combat for monsters.

[fartwhif]
* Implemented Lockpicking. Players can now use lockpicks, with a success rate based on the Lockpick skill.

### 2018-05-12
[fartwhif]
* Added two new debug commands:
  - `crack` - Unlocks and opens locked doors and chests.
  - `nudge` - Corrects the player's position if they are stuck in black space.

### 2018-05-11
[Slushnas]
* Magic: Adjustments to Bolt/Streak/Arc projectile behavior to match retail more closely.

[Jyrus]
* Fixed routing for in-game messages given by magic and portal spells.

### 2018-05-09
[dgatewood]
* Re-implemented the basis for Item Giving.

### 2018-05-08
[Jyrus]
* Magic: Further fixes for null cases of targeted spells.

### 2018-05-07
[Jyrus]
* Magic: Clean-up of code for spells casted by items/creatures, plus miscellaneous fixes of null cases.

### 2018-05-04
[Jyrus]
* Magic: Items with spells can now cast them on players.
* Fixed a null exception bug where the player attacks without a weapon equipped.

### 2018-05-02
[gmriggs + dgatewood + Slushnas]
* Monster Movement added. Hostile creatures will now pursue players in range.

### 2018-04-29
**ACE-World-16PY world db release v0.0.13+ required with this update**

**You will need to update world database with scripts in the respective update folders**
[Ripley]
* Added New Crafting Tables to World Database.
* Removed old ace_recipe table.
* Scaffolded the new tables.
* Rewired RecipeManager to new data.
  - Basic X + Y = Z crafting done.
  - Dyeing and other object modifications via crafting are not handled properly yet.

[Jyrus]
* Fix for Attuned Item Property, which was missing a null check.

### 2018-04-28
[dgatewood]
* Loot: Updated Loot-generated Weapon Skill Types to match retail: Light, Heavy and Finesse.
* Fixed Skill Max Limit discontinuity between server and client.

[Jyrus]
* Added Attuned functionality to items, preventing items with that property from being dropped or traded.

### 2018-04-27
[dgatewood]
* Added Loot functionality:
  - Added Loot Generation Factory, generating Armor sets, Weapons, Jewelry, Gems, and Mundane Items across all tiers.
  - Added Loot Helper, which applies Value, Stats, Spells, Cantrips, Colors and Usage Restrictions to Loot Items.
  - Random Loot now generates in corpses, alongside generic items such as Food and Potions.

[Jyrus]
* Magic: Added Target Validation for Harmful Spells to prevent exploitation.
* Magic: Added Workaround for Projectile Spells Collisions in dungeons.
* Various additional Fixes for Spell Interactions and Collisions.

### 2018-04-26
[dgatewood]
* Fix for Player.cs, where examining a creature as it dies would crash the server.
* Removed Pyreal Value Property from Corpses.

[Jyrus]
* Magic: Magic Defense Skill functionality added.
* Magic: Player Killer Checks added. Harmful spells no longer affect friendly players.

### 2018-04-25
[Jyrus]
* Magic: Skill Checks Implemented. Spells now have a chance to fizzle if caster's skill is too low.

### 2018-04-24
[gmriggs]
* Combat: Missiles no longer cause delay when travelling between landblocks.
* Removed Vitae from Life Spells category.
* Adjusted Player Vitals on death.

[Jyrus]
* Magic: Arc War Spells added.

### 2018-04-23
[gmriggs]
* Combat: Bow/Crossbow Combat tweaks. Adjusted Missile Trajectory and Behaviors, Fixed Ghost Projectiles.

[Ripley]
* ACE-World: [ci-skip] Added Epoch Comments.

### 2018-04-22
[gmriggs]
* Combat: Added Bow/Crossbow Combat, Ammo Usage and Damage calculation.

[Ripley]
* ACE-World: Updates to Links and Generators, plus miscellaneous changes and cleanup.

### 2018-04-21
[Ripley]
* ACE-World: [ci-skip] Improved Comments in SQL Scripts.

### 2018-04-20
[gmriggs]
* Magic: Added support for Item Magic Buffs/Debuffs, including Weapon and Armor buffs/banes.

### 2018-04-19

[gmriggs]
* UI: Added Shortcut Bar functionality, allowing items to be bound to the hotkeys 1-9.
* Magic: Added support for Life Magic Buffs/Debuffs, including Armor, Imperil, Vulns and Prots.

### 2018-04-18
[gmriggs]
* Magic: Added support for Creature Magic Buffs/Debuffs.

### 2018-04-16
[gmriggs]
* Added Healing Kits, Healing Usage, Animation and Skill Checks.
* Fixed Landblock Adjacency Loading for improved performance.
* Fixed Vitae Display Bug

[gmriggs + Slushnas]
* Magic: Enchantment Manager now supports Adding/Updating/Removal of all Spells.
* Magic: Enchantment Manager now tracks Spell Durations.
* Player Health/Stamina/Mana now updates on Death.

### 2018-04-15

**ACE-World-16PY world db release v0.0.12+ required with this update**

**You will need to update world database with scripts in the respective update folders**

[Ripley]
* Added New Encounter Table to World Database.
* Scaffolded the new tables.

[Jyrus]
* Portals: Limited teleportation spamming from repeated collisions.
* Magic: Added Fast Casting, increased projectile velocity for Streak War Spells.

[gmriggs + Slushnas]
* Added Enchantment Registry.
* Added Vitae (Death Penalty).

### 2018-04-09

**ACE-World-16PY world db release v0.0.9+ required with this update**

**You will need to update both shard and world databases with scripts in the respective update folders**

[Ripley]
* Added Quest Table to World Database.
* Added Quest Registry Table to Shard Database.
* Added House Portal Table to World Database.
* Added Event Table to World Database.
* Added Encounter Table to World Database.
* Scaffolded the new tables.

### 2018-04-02

**ACE-World-16PY world db release v0.0.8+ required with this update**

**You will need to update world database with scripts in the respective update folders**

[Ripley]
* Added Spell Table to World Database.

### 2018-03-25

**ACE-World-16PY world db release v0.0.7+ required with this update**

**You will need to update both shard and world databases with scripts in the respective update folders**

[Ripley]
* Changes made to the Emote table storage in the database. Be sure to run updates to shard and world and THEN download and run the latest world database.

### 2018-03-15
[Ripley + gmriggs + OptimShi]
* Add Corpse class and basic framework for spawning a corpse at death of creature.
  * use /smite command when you have a creature selected.

### 2018-03-13
[Ripley]
* Add three new tables to Shard Database.
* Add CharacterTitle enum.
* Add proper title support and storage for players. Debug command of `addtitle` and `addalltitles` for testing.

### 2018-03-11

**ACE-World-16PY world db release v0.0.6+ required with this update**

**You will need to delete and recreate both shard and world databases with this update**

[Ripley]
* Changed IID storage in database from INT to UINT.

### 2018-03-10
[Ripley]
* Adding /morph command for admins.
  - The command was used to facilitate live ops by developers to play out story elements such as Martine wreaking havoc upon the lands.
  - For some samples, you can try `martinelo`, `baelzharon`, `asheronlo`, `ayanbaqurdrunkenscholar`, `rabbitwhite`, `monoguapaul`, or even `golemmegamagma`.
  - Protip: If you do morph into a golem, press the b key ;)

### 2018-03-07
[Ripley]
* Fixed Skills and Vital calculations when spending xp and potential sequence errors.
* Fixed issue with gitignore file. New files automatically are added to the project once again, YAY!

### 2018-03-06
**You will need to delete and recreate all three databases with this update**

**ACE-World-16PY world db release v0.0.5+ required with this update**

**ACE-World world db releases are currently incompatible with this update**

[Mag-nus + Ripley]
* Entity Framework Core migration to master.
* This is a complete backend change.
* Data is now stored in the database in a new and improved format.
* There will be some things broken that still been haven't migrated from the previous ORM or AceObject references to the new EF references which require further work but overall we're just about back to where we were previously.

### 2018-02-10
[Ripley]
* Renamed a couple of messages queues to match protocol documentation.

### 2018-02-09
[Morosity/Spazmodica]
* Added boolean to ClientPacket if packet fails to parse.
* Use Packet.IsValid to determine whether to readfragments
* Use Packet.IsValid to determine whether to send Packet to WorldManager
* Added new list for tracking endpoints calling server before creating sessions
* Added check if client is already in the loggedIn list and sending another Login packet to remove
* Added check if client is not in the loggedIn list ignore requests
* Added removal of client from loggedIn list
* Set loggedIn list max to match ConfigManager.Config.Server.Network.MaximumAllowedSessions

### 2018-02-08
[Mag-nus]
* Major code cosmetic cleanup across entire solution.
* ACE project renamed to ACE.Server

### 2018-02-07
[Mag-nus]
* ACE.DatLoader caching and ReadFromDat code refactored. No more static method/object messes. DatLoading is now done through DatManager and DatDatabases.

### 2018-02-06
[Mag-nus]
* ACE.DatLoader DatDirectory reading code cleaned up.

### 2018-02-05
[Mag-nus]
* ACE.DatLoader now reads the full header. FileType is no longer passed as an arg and pulled from the file headers.
* ACE.DatLoader full conversion to the IUnpackable interface completed.

### 2018-02-04
[Ripley]
* Disable OlthoiPlay character creation for now. They're not really implemented yet.
* Add Database lookup by WeenieClassDescription.
* Update Create and CI commands to support id or name search.
* Added catch for missing database objects in CharGen and IOU object creation for those missing.

[Slushnas]
* Added support for all starter spells during character creation.

[Mag-nus]
* ACE.DatLoader more IUnpackable refactoring

### 2018-02-03
[Mag-nus]
* ACE.DatLoader DatManager.Initialize() is no longer dependant on ConfigManager.
* ACE.DatLoader.Tests now include framework for unpack testing objects from the dats.

[dgatewood]
* Updated GameEventMagicUpdateEnchantment
* Hardcoded some test functionality in Gem.

### 2018-01-31
[Mag-nus]
* ACE.DatLoader more IUnpackable work
  * ACE.DatLoader: XpTable updated

  * ACE.DatLoader physics and environment using new unpack pattern

### 2018-01-29
[Ripley]
* Rebased SQL scripts.

### 2018-01-28
[Ripley]

**You will need to delete and recreate your ace_auth and ace_shard databases to proceed**

* Removed API and SecureAuth coding per new license agreement.
* Added some simple Authentication database tests.
* Added NetAuthType checking and password verification.
* GlsTicketDirect/GlsTicket checking is disabled until it can be reimplented in the future.
* **New ACClient launch command is in use now!**
  - ``` acclient.exe -a account_name_here -v password_here -h 127.0.0.1:9000 ```
  - The old command will not work.
* Auto Account creation now occurs for any successful connection for an account that does not currently exist. Default AccessLevel is based on config.json setting.
* Added some account password tests.
* Added AllowAutoAccountCreation to config.json. Set it to false to disable auto account creation.

### 2018-01-27
[Mag-nus]
* More dat refactoring, this time based on SetupModel and Hooks.

### 2018-01-26
[CrimsonMage]
* Added Summoning to StarterGear (Mud Golem Essence).

### 2018-01-24
[Ripley]

**Changes based on 16PY database, may result in problems when using ACE-World database**

* Adjusted SQL scripts slightly.
* Corrected issue with objects spawning at xxxx0000 in landblock.
* Added null catch for objects that have generator profiles for weenies not found in the database.
* Added Bindstone and Clothing classes.
* Added expected object flags to currently implemented WeenieType classes.
* Adjusted Bool recalls for WorldObjects.
* Added Shade (and TryToBond, which appears unimportant) to Inventory.
* Added Shade, Palette and StackSize to create command for testing.
* Added Palette and Shade to WorldObject and use them in conjunction with ClothingBaseDID to set ObjDesc.
* Added proper wielding for clothing on weenies and fixed clothing priority for them.
* Added face randomization for Human based NPCs.
* Removed GeneratatorLinks and AceObjectGeneratorLinks and cleaned up some AceObject load/clone code.

### 2018-01-23
[Ripley]
* Minor cleanup of PR #595 due to needing to more inventory handling.
* Added HearDirectSpeech from @dgatewood PR #585.

[Mag-nus]
* ACE.DatLoader refactoring and cleanup.

[dgatewood]
* Added GiveObjectRequest and HearDirectSpeech messages.

[Morosity/Spazmodica]
* Added ActionChain null wrapper added to not crash server if teleporting player disconnects.
* Work toward resolving #231.

### 2018-01-19
[Cydrith]
* Changed Licence from GPL V3 to AGPL V3. Additional licence restrictions introduced by AGPL apply from this point forward.

### 2018-01-18
[Morosity/Spazmodica]
* Fixed range xp calculation for members based upon distance to leader

### 2018-01-14
[Morosity/Spazmodica]
* Fellowships functionality updated to add Recruit, Dismiss, Disband, Make new leader, Quit, and Openness
* On leader quit (not disband), random player picked as new leader
* When limit hit of 9, error message will appear on trying to recruit.
* Join requests outstanding that could put fellowship above 9 will not get added if accepted
* Character options of "Ignore" and "Auto accept" fellowship requests saved and read
* Confirmations added for character with "ignore" and "auto accept" off
* ConfirmationRequest GameEvent added to send requests to client
* ConfirmationResponse GameAction added to send responses to server
* ConfirmationResponse GameAction has handling for other confirmation types
* Fellowship xp dist handled for both evensplit fellows as well as proportional
* Fellowship xp dist handled for up to 2.5 coords away with appropriate loss of amount of xp based upon distance
* EarnXP method added to Player with params of fixedAmount bool and sharable bool
* FixedAmount does not provide bonus xp when passed to fellowships
* Sharable provides ability to not share xp reward
* EarnXPFromFellowship method to bypass loop of trying to split xp through fellowship again
* Fellowship enums renamed to start with Fellowship for easier location
* GameActions, GameMessages, and GameEvents for fellowships names updated for easier location

### 2017-12-21
[Ripley]
* Added Placement Enum.
* Swapped storage of Placement data with PlacementPosition data.
* Removed AnimationFrame and linked it with Placement.

### 2017-12-19
[Ripley]
* Overhauled Generator system to bring it more in line with how it operated on retail servers.
  * You'll need to load some test data included with this update in the Updates/World folder for this code to work.
  * Locations of the test generators are below. The creature generators do not support regeneration, while the item generators will regenerate.
  * /teleloc 0x7f0301ad [12.319899559021 -28.482000350952 0.0049999998882413] 0.33894598484039 0 0 0.94080585241318
  * /telepoi Holtburg
* Added @adminvision command, use it to see the generators (and eventually other unsendable things).

### 2017-12-11
[ddevec]
* More updates to parallelism structure
* Network messages are now seralized
* Removed the vast majority of ActionChain code that was being used for parallelism/thread safety

### 2017-12-10
[Mag-nus]
* ACE has been converted from .net framework 4.6.1 to .net core 2.0.
  * This also involves changing the way databases are loaded to a manual method which is .net core compatible.
  * This breaks secure auth. That system will need to be reworked by someone more experienced to function properly.

### 2017-12-06
[HellsWrath]
* Fixed an issue with accountcreate command. It did not check if accountname already existed.
* This does not not fully resolve the issue of accounts being created on other threads.

### 2017-12-03
[Mag-nus]
* Many projects converted to .net standard 2.0 or .net core 2.0
* MySqlResult object removed from ACE.Database
* ACE.Diagnostics removed from ACE. ('diag' command is disabled until we fine a better solution)
* ACE.CmdLineLauncher changed over to ACE.ACClientLauncher. Build is also disabled by default in Configuration Manager.

### 2017-12-02
[Ripley]
* Updated README links relating to Protocol documentation.
* Fixed read issue with AnimationFrame in DatLoader. (Issue reported from Discord & confirmed by OptimShi)
* Rebased SQL scripts.

[Mag-nus]
* Changes to eventually support .net core 2.0
  * Changed target .net framework to 4.6.1
  * Changed build environment to Visual Studio 2017.

### 2017-11-26
[Ripley]
* Deleted VendorItems class.
* Replaced VendorItems class with more generic AceObjectInventory class.. This is entirely based on CreationProfile found in protocol doc so probably could stand to be renamed, same with database table.
* Made changes to Vendor to use AceObjectInventory and added a bit more functionality with respect to alternate currency.
* Added more to DestinationType enum.
* Added some properties to AO/WO.
* Added WieldList object spawning to the CreateWorldObjects function of the WorldObjectFactory. This is not the final, correct way to do this but does visually look right.
* WorldObjects are not really world aware so they have much work to do to be able to really wield and use objects, as well as report them properly to the landblock manager.

### 2017-11-20
[OptimShi]
* Changed EquipTest debug command to utilize the index of a clothing table item and added an optional shade parameter

### 2017-11-18
[Ripley]
* Added some foundational elements to support Chess minigame.
* Client handles it cleanly now.
* Find a chessboard in the world and try it out. Enjoy your table flipping.


### 2017-11-10
[Verbal]
* updated fellowship creation to read the xp sharing flag

### 2017-11-09
[fantoms]
* Added command and API endpoint for retreiving the database content from Github.
* You can now redeploy database content from local disk.
* Updated logging for redeployment.
* Added `ContentServer` example config setting.

### 2017-11-05
[fantoms]
* Added Database Redeployment feature to allow for updating or resetting a database from the latest github content.
* Added `redeploy-world` command from the console to issue world redeployment.
* Added `redeploy` console command to allow for resetting one or more databases, using the redeployment functionality. This command requires a database selectiona dnd the "force" string as a parameter, for added security.
* Implemented API functionality for World redeployment and database Redeployment.
* Added configuration section for Remote Content downloads from Github, `ContentServer`.
* Added database.cs functions to allow for database creation, removal, connectionString changes, and database script loading events.
* Added WorldDatabase function to check for user modified flag.
* Added a workaround that prevents a crash when a bad token is passed in to the API.

### 2017-11-03
[ddevec]
* Change core structure eliminating multi-threading between threads.
* Removed ChainOnObject() from landblock, make GetObject() public.
* Simplified Action code to reflect for new threading model.

[Ripley]
* Fix AppVeyor issues.

### 2017-11-02
[Ripley]
* Fix Landblock loading of weenie instance position data.

### 2017-11-01
[Ripley]
* Updated EmoteCategory, EmoteType, and SpellType enums.
* Add CombatStyle enum.
* Rebased SQL scripts.
* Changed all PropertyInt properties from uint to int.
** NOTE: ACE-World database version required to be 0.2.7 or higher from this point forward. **
* Todo/Fixme: Combat Stances don't work properly due to GetInventoryItem issue.
* Cleaned up StyleCop issues.
* Updated README with ACE-World requirement.

### 2017-10-30
[Mogwai]
* implemented updating of weenies in ace-world necessary for crowd-sourced content creation
* added database unit tests to AppVeyor build

### 2017-10-27
[OptimShi]
* Updated DatLoader.FileTypes.SetupModel to more closely represent what is in the client. Also added additional structures to this to fully read the file.

[Mogwai]
* Loads of new JSON property renaming
* New API definitions stubbed, some implemented

### 2017-10-25
[fantoms]
* Changed an exception in AuthApi, to prevent the server from crashing on a bad account.
* API Updates for roles in token, you should now be closer to using the correct `[AceAuthorize(AccessLevel)]` reflection with Api functions.
 - Api will take the first subscription's accesslevel, when checking roles; this should probably be set to take the highest level. AccessLevel of Player (0) is used when a subscription is missing.

### 2017-10-24
[OptimShi]
* Fixed bug with Equipment Mask enum that was causing Greaves/Lower Leg Armor not to appear on character

[Mogwai]
* API Host Updates for consistency and messaging.
* Defined and implemented Content API.
* Started the Weenie API definition.
* removed the old/unused web project.

[Ripley]
* @Slushnas found WebApp.Start error in ACE.AuthApi.Host: Changed ACE.Api.Startup to ACE.AuthApi.Startup to fix domain error.
* AuthServer Changes:
  - AuthServer needs a publicly accessible address to point connecting clients if you're trying to use the server outside of a localhost/internal lan only environment.
  - The current defaults basically point everything to "http://+:8001" which fails to resolve. So the following changes have been made..
  - Changed AuthServer.Url to AuthServer.ListenUrl
  - Added AuthServer.PublicUrl
  - Typically, you'll leave the AuthServer.ListenUrl to the default, but you'll want to change AuthServer.PublicUrl to either of the following:
    * The externally accessible address of the AuthServer so that clients from the internet can authenticate.
    or
    * localhost or the internal lan address so only those inside the local network/local machine can authenticate.

### 2017-10-23
[Jyrus]
* Fix a typeo in ACE.AuthApi.Host, as it was attempting to start the API host in place of the AuthAPI host

### 2017-10-22
[fantoms]
* Changed build output path for Api.Host and AuthApi.Host projects, in an attempt to share the server config for debug and build.
* Added simple service configuration function to Api.Common, to load the initial config for Api host apps.
* Updated README.md to include additional instructions for Api host support.
* Added a few null checks to prevent crashes in the AuthApi login function.
* Added a config error check on the cmd line launcher, for ease of use.
* Added config examples that are open to the world.

### 2017-10-21
[Jyrus]
* Add the initial set of CoinValue in Container.cs to the value of contained Coins
* Modified item move, drop, and pickup methods to account for pyreal changes using StackOverflow's UpdateCurrencyClientCalculations();

### 2017-10-20
[Mogwai]
* Authentication overhaul is done.  Please note there are significant changes to the Readme file.

### 2017-10-19
[Jyrus]
* Created WO.StackUnitValue and WO.StackUnitBurden to be initialized by Weenie.{Value,EncumbranceVal}, with builtin null to zero initialization
* Moved WorldObject value and encumbrance calculations down to the WO class based upon WO.StackUnitValue and WO.StackUnitBurden vs WO.StackSize

[Og II]
* Added processing for contract management add / abandon.
* Removed update script I added back on 10/14 to fix the burden of the weenie for the MMD note.   This is no lonager needed - use Dereth v0.2.4 or greater the issue has been
* resolved in the data and the script is no longer needed.
* Minor code cleanup and improved some method documentation.
* Added new table to store persisted tracked contracts (quests)
* Implemented two new game events / SendClientContractTrackerTable and SendClientContractTracker
* Implemented abandon quest - Game action - Social_AbandonContract
* Documeted methods and updated trello.
* To test @ci 44163 -create a contract.   Use the contract and you are now tracking that quest.   You can create any contract using ci or buy one from a vendor if they are In
* did not check vendors.   You can abandon the contract.   All management and the cooldown delay of 2 seconds is respected.

### 2017-10-18
[StackOverflow]
* Vendors - Fixed bug with sell of item from wielded locations
* Inventory - Major Refractor of Code. (many conditional bugs fixed)
* Fixed server crash bug on dropping wielded items (items now drop to ground np)
* Fixed bug with dropping clothing / armor to landblock

### 2017-10-17
[Jyrus]
* Reconfigured and renamed functions to account for Potions being included in class Food
* Wired up applying the chosen starting town to the Training Academy path used

[StackOverflow]
* Vendors are no longer free!
* Pyreals as currency has been added.
* @setcoin added for debugging of sync of coinValue gets away during dev.

### 2017-10-16
[Jyrus]
* Implemented Food class for consuming food items
* Stub created for handling Buffing food but implementation is not complete, as spellcasting has not been completed, yet
* Implemented a method for removing an item from inventory that also handles item stacks.
* Included a fix for stack splitting to create proper WorldObject, instead of defaulting everything to GenericObjects
[StackOverflow]
* Made all vendors free as long as you have some money in your pack
* Prepping for real pyreal transactions.
* Fixed Packs, you can now buy packs and use them without the need to relog

### 2017-10-14
[Og II]
* Added processing for merge items.
* Added script to update the burden, value and stack size of the weenie for MMD notes I used in testing.   20630 - value 250,000 burden 5 and stacksize 1

### 2017-10-13
[Og II]
* Added processing for item inscription. Implemented Set Inscription and Inscription Response messages.
* Inscriptions persist to the database and can be removed.
* Implemented first pass of same inscriber check. Sends error message and does not update the item. This may need to be revisited, I * * thought it blocked you from changing it - not sure if that was a weenie error sent to client - I did not see a fail inscribe in live * pcaps. It does check the inscribable flag and will prevent any pre-inscribed items (quest items with inscriptions from being altered. * Thanks @LtRipley36706 for pointing that out.

### 2017-10-12
[Ripley]
* Added Burden tracking to Players and Containers.
* Added CoinValue tracking via objects of WeenieType.Coin to Players and Containers.
* Added Value tracking to Containers.
* Commented out areas related to above changes that were not implemented correctly.
* TODO: Update Value/Burden and track changes as StackSize adjustments occur.
* TODO FIXME: Vendors code need to be adjusted to work with pyreal objects, not CoinValue direct.

### 2017-10-11
[StackOverflow]
* Added support for unique Vendor Items.
* Added a new Debug Cmd, @setcoin value for testing purposes.
* Major Vendor Code refractor, vendor now only load items for sale and not items they should be wielding.

### 2017-10-06
[Jyrus]
* Moved WeenieType keys into their own class and implemented unlocking doors via keys

[Immortalbob]
* Added equipmentset.cs to house the equipment set enums for future use.

### 2017-10-05
[Immortalbob]
* Identified unknown enums 381 382 383 384 387 388 389 and updated acecharacter.cs and propertyint.cs.

[Mogwai]
* Modified vendor stuff to use buy and sell rates from DoubleProperties

### 2017-10-04
[Jyrus]
* Moved handling of the Portals into the HandleOnCollide method and implemented MoveTo for OnUse for Portals.  Currently, OnUse also queues up portaling
	via a direct call to Portal.HandleOnCollide, which can be removed when collision is implemented.

[StackOverflow]
* Added Vendors (Go buy stuff!) - work in progress
* Added Debug command to give coins.
* @addcoin 100000
* known issue with pyreals in inventory.. drop them on the ground if you want the math to be correct for now.

### 2017-10-03
[OptimShi]
* Added method to get animation timings out of MotionTable and updated several instances where we were faking it previously.

[Og II]
* Added update script to modify defaultCombatStance ace_object_properties_int to match change with @OptimShi changes of 10/02
* Rolled back [ServerOnly] attribute on enums.   I left the mechanism in place and tagged all over 9000 as such.   Our method for determining
* this is not correct.   We can add these back when we fully understand which are truly not needed by the client.

### 2017-10-02
[OptimShi]
* Changed MotionCommand and MotionState enums to full uint instead of short and adjusted the movement serializing functions to work with these changes.
* Fixed bug when leaving CombatMode with no ammunition equipped (e.g. melee or magic mode)
* Added "acecommands" tip to console start up
* Corrected an issue with the palette of head gear chosen at character creation.

[Og II]
* Initial work on content interactions.
* Cleaned up the using statements on several files.
* Aligned the enums to = sign
* Fixed bug in universal motion using the TurnToObject
* Added enum attributes for all of our property enums per Mogwai using the data from OptimShi found here
* http://ac.yotesfan.com/ace_object/not_used_enums.php
* WIP need to filer out sending of aceObject properties using this information.   I am stuck on the lambda expression.
* see line 74 in GameEventPlayerDescription.   Any help appreciated.
* NOTE: the big gotcha with this is if we have any of the [ServerOnly] attributes set incorrectly, that data will not get sent to the client
* and if you don't remember that we are filtering by that attribute you will be going what the hell is wrong.   Just an FYI

[Immortal Bob]
* Identified and changed enum Unknown_386 to Overpower.

[Ripley]
* Added WeenieType.Coin handler.
* Wired up CoinValue and attached proper tracking to WeenieType.Container handler.
  - Proper stack splitting or pickup of items that allow stacking still not handled by server.
  - Correct mathing requires base WeenieType.Coin weenies in database be set correctly. Value/StackSize need to be set to single value initally.
  - ACE-World will need to be updated to fix base values.

### 2017-10-01
[Og II]
* Initial work on content interactions.
* Cleaned up the using statements on several files.
* Aligned the enums to = sign
* Fixed bug in universal motion using the TurnToObject

### 2017-09-30
[Ripley]
* Merged updates into WorldBase.
* Fixed saving books to the Shard database.
* Characters now equip items on world entry. (This will reveal bugs in clothing layer priority and model change issues)

### 2017-09-25
[Og II]
* Implemented persisting of wielded items.
* Changed inventory to manage a list of world objects instead of ace objects.   This was needed for sequence issues.
* fixed combat animation that had been broken for a while due to prior refactoring.
* fixed moving inventory from container to container with persistances which was broken due to prior refactoring.
* implemented the response message to send object description.
* cleaned up so code, aligned enums.
* TODO.   I have a dirty flag bug that causes issues when you do container to container moves and save.
* Not sure if you want to accept this with a known bug - it has changed a lot and is probably holding others up.

### 2017-09-20
[Ripley]
* Added a minimum UseRadius to Doors. This prevents the radius being so small as to require you to be inside the door to open/close it.

### 2017-09-18
[Mogwai]
* Starter clothing done.  other stuff WIP.
* Modified recipe table to items resulting from failure
* Updated recipe manager to create the appropriate items on failure

### 2017-09-15
[Mogwai]
* Modified recipe table to support heal kits
* Implemented heal kit usage (known bugs around sequences and GameMessagePublicUpdatePropertyInt as well as current health)

### 2017-09-14
[OptimShi]
* Added GfxObject (0x01...) parsing from the client_portal.dat.
* Added Surface (0x08...) parsing from the client_portal.dat.
* Added Environment (0x0D...) parsing from the client_portal.dat.

### 2017-09-13
[Mogwai]
* Started work on the Weenie Editor
* Added table for crafting recipes
* Added basic support for crafting
* Added confirmation messages for crafting

### 2017-09-09
[Og II]
* Added verbiage to the readme stating the educational purpose of this project.

### 2017-09-05
[OptimShi]
* Added Palette (0x04...) parsing from the client_portal.dat.
* Added PhysicsScript (0x33...) parsing from the client_portal.dat.
* Added PhysicsScriptTable (0x34...) parsing from the client_portal.dat.

### 2017-09-03
[Ripley]
* Changed GeneratorFactory to use WorldObjectFactory.CreateNewWorldObject insted of CreateWorldObject for now.

[OptimShi]
* Added Wave (0x0A...) parsing from the client_portal.dat and a "wave-export" console function to save as a playable .wav file.
* Added GfxObjDegradeInfo (0x11..) parsing from the client_portal.dat
* Added CombatManeuverTable (0x30...) parsing from the client_portal.dat
* Added ParticleEmitterInfo (0x32...) parsing from the client_portal.dat

### 2017-09-01
[OptimShi]
* Added Region (0x13...) parsing from the client_portal.dat
* Added Scene (0x12...) parsing from the client_portal.dat

### 2017-08-27
[Ripley]

**NOTE: The following changes require ACE-World version 0.2.0 or greater.**
* Changed Landblock initailization code to use ace_landblock instances.
  This change reduces database size and creation time significantly as well as faciltates easier worldbuilding operations going forward.
* Uncommented weenieHeaderFlags2 from AceObject.
* Rebased Shard and World SQL scripts and removed update scripts.

### 2017-08-24
[rtmruczek]
* Added support for creating and leaving a Fellowship. The current implementation is racy, but serves as an example for future work on Fellowships.

### 2017-08-16
[Zegeger]
* Rewrote packetizer code to fix incorrect multi-fragment messages (basically we shouldn't send optional headers when a single fragment fills the full packet)
* In general this made the packetizer much cleaner and more organized
* In the process I added support for premptive tail sending.  Essentially for multifragment packets, the last fragment will likely be much smaller then the full packet size, so it could fit in an earlier message among other fragments.  We see this can occur in production network traces (eg the last index arrives as the first fragment of the message). Previously we did not do this, but now we can.

### 2017-08-15
[OptimShi]
* Fixed book properties not cloning properly.
* Added Animation parsing from the client_portal.dat and all relevant hooks and properties.

### 2017-08-11
[OptimShi]
* Fixed bug with the components in spells that had extended characters (>7bit) in their name or description.

[Og II]
This is getting so large, I need to stop here - this is a good spot - adds significant functionality but with a few known bugs.
* Inventory will save with the container and load again in the correct positions.
* Character Description Event is now wired up for inventory.
* removed containers copy of aceObject Inventory - just make the list accessible.
* Updated Clone so it you pass a new guid, it resets all the associated child tables.
* implemented placement IntProperty 65 used to track slot in Container
* Created supporting methods to manage pack order
* Enhanced the debug log message for DBDEBUG
* Small temp fix to stutter when attempting to shift walk or shift jump. We still need to really understand autonomous
* Created new view vw_ace_inventory_object to expose ability to pull by container.
* general clean up comments and whitespace
* Inventory loading - working
TODO:
* Add in capacity checks
* bug still in picking up items off the ground and saving to database.
* bug with reloaded items saved in a secondary side pack.

### 2017-08-10
[Ripley]
* Minor changes to Book weenie.

[OptimShi]
* Added MotionTable parsing to DatLoader.
* Added GetSpellFormula() function to SpellTable. Will return component id's based on players account name for any given spell.
* Fixed some logic issues in the SpellComponentsTable

### 2017-08-08
[OptimShi]
* NOTE: The following change requires ACE-World database v0.1.9 or newer to make use of.
* Added ability to read books, notes, etc. (Currently read-only)
* Added Database Update scripts to Shard and World for new ace_object_properties_book

### 2017-08-07
[Ripley]
* Wired up World Broadcast commands.
* Cleaned up a few other commands that were not needed or found in other catagories.

### 2017-08-06
[Ripley]
* Added Scroll weenie object.

### 2017-08-02
[Ripley]
* NOTE: The following changes require ACE-World database v0.1.8 or newer...
* Added NpcLooksLikeObject check for Creature assessment profile fix provided by @OptimShi.
* Moved Default do nothing UseDone to WorldObject and removed it from GenericObject.
* Added Cow weenie object.
* Added Cow, Creature, and Container to the WorldObjectFactory.
* Say Hi to the cow in Holtburg.
* Load attributes stored in world database into objects.

### 2017-07-31
[OptimShi]
* Added check to ensure a player can't spend more attribute credits than they should at character creation.

### 2017-07-30
[Og II]
* Added spell bar management and persistence.   This continues the work for learning and persisting spells.
* Cleaned up some of the enum duplication in GameEventPlayerDescription.cs
* Added some debug code around universal motion.
* Added some debug asserts in PlayerDescription
* To test - use @learnspell to get some spells in your spell book if you do not have any.   They will persist with your character.
* You can also unlearn them and they will be removed.
* Now, drag your spells into any configuration in your spell bars you like.   You can add them, remove them or reorder them.
* They will be saved on our periodic save, or when you log out or you can force a save with the debug command @save-now.
* Added table to persist spell bar configuration.
* That's all for this PR.

### 2017-07-29
[Ripley]
* NOTE: The following changes require ACE-World database v0.1.6 or greater...
* No longer setting WeenieHeaderFlag.Value if Value = 0.
* DebugObject, CollidableObject and UsableObject are removed.
* Moved OnUse/OnCollide to WorldObject virtuals for overriding.
* Added WeenieType to AceObject/WorldObject for weenie classification.
* Added GenericObject which becomes the default object for weenies.
* Reworked GenericObjectFactory to use WorldObject.GetObjectFromWeenieType.
* Generic OnUse just sends UseDone
* Reorganized WorldObject class file, made changes to PhysicsDesc to align with client expectations.

### 2017-07-28
[Zegeger]
* Tweaked the ACK timer to help improve network reliability

[Og II]
* Spent 5 or 6 hours tracking down an align bug in create object - Universial Motion section.   Damn it Jim, I am a high level programmer not a down in the bits and bytes weenie.... :)

### 2017-07-25
[Ripley]
* Minor change to QueryItemMana based on PCAPs.

### 2017-07-24
[Lidefeath]
* Added GameAction and GameEvent for querying an items Mana.

[Og II]
* Refactored debug command learn spell.
* Made spells persist to the database.
* Added abilty to delete spell from spell book and send that persistantly to the database on character save.
* TODO: Put in the abilty to use scrolls to learn spells.

[Ripley]
* Added Bools used by ObjectDescriptionFlag and PhysicsState.
* Added WeenieType to PropertyInt Enum for future use.
* Changed GameMessageServerName to include connection count and max.
* Changed the way PhysicsDescriptionFlag, PhysicsState, WeenieHeaderFlag and WeenieHeaderFlag2 are set and sent to the client.
* Even more changes to main object flags.
* Wired up /mrt command.
* /mrt was used by admins to bypass the housing barriers. I used it to prove out ObjectDescriptionFlag state changes.
* Note that you have to have manually set the Admin flag on ObjectDescriptionFlag in the code for this command to work, See Player.cs constructor section.

### 2017-07-21
[Ripley]
* Moved Identify Serializes and Writers to WorldObject and Creature as appropriate.
* Changed the way Workmanship is set so to not always default to 0 if ItemWorkmanship was null.
* Moved GameActionTalk handle code to Player.
* Wired up broadcasting of local chat, emotes and soul emotes.
* Fixed StyleCop issue in AbilityFormulaAttribute.
* Fix for SetupModel bug provided by @OptimShi.

### 2017-07-20
[Lidefeath]
* Fix raising primary and secondary attributes.
* Aligned the enums PropertyAttribute.Quickness and PropertyAttribute.Coordination with the client.
* Due to this alignment old chars will have wrong coord and quick, so make new characters.
* I didn't touch the trigger of VitalTicks yet, so vital ticking is only enabled during Player.Load().

### 2017-07-17
[Ripley]
* More field changes.
* More new character defaults set.
* Fixed Guid assignment for new players and generated objects.
* Move Guid range assignments to ObjectGuid, GuidManager reads them in from there now.
* Changes to WorldObject, Door and Generator classes.
* Changed Debug Output to chat window only to allow for proper ID display.
* Reworked Door.cs.
* Locked Doors are now now locked and act accordingly.
* Multiple changes to AceObject and AceCharacter.

### 2017-07-14
[Og II]
* Initial work on combat mode.   System now will move into and out of the correct combat mode depending on what items you have equipped.
* TODO: There are some edge cases I still need to implement and additional testing.   I would like to get this in so that I do not run into huge merge issues later on.
* Testing - you can log in - run @weapons after log in.   This will get you various weapons based on weapon type.   You can go into and out of combat mode
* Melee, missile - both bow and xbow, thrown, sword, UA, two handed as well as dual wield.    I also implemented if you remove your weapon while in combat mode,
* you will drop into unarmed.   If you equip an item and you are not in peace mode, you will assume the correct combat mode.   There may be some tweaking needed
* for changing combat mode in combat mode - I think it did some slower animation - but the basics are in and I can tweak the sequence once I talk to players or If
* I can find a pcap with that behavior.

### 2017-07-12
[Ripley]
* Moved most of the enums under ACE.Network to ACE.Entity.
* Changed some properties of new characters to set proper title defaults and DOB.
* Moved writers out of Player and into GameEventIdentifyObjectResponse.

### 2017-07-11
[Ripley]
* Removed events from SQL scripts. No longer need them in current implementation.
* Renamed ObjectType to ItemType.
* Added Sequences for Health/Stamina/Mana for proper updating later.
* Set slew of initial values for new characters.
* Added PlayerKillerStatus, CreatureType, and WeenieType enums.

### 2017-07-10
[Og II]
* Fixed bug with animation - isAutonomous flag was incorrectly set and fix for limping broke animation. (Me)   This should allow both to work correctly.

### 2017-07-09
[Ripley]
* Changed the way deleting a character works. Instead of having the SQL server poll, the act of connecting to the server triggers the check.
* If character's delete timer is up, the next time you log in, the character is deleted before sending you the list.

[ddevec]
* Changed SerializedShardDatabase to use a BlockingCollection to stop busy polling of DB requests.
* Updated GuidManager to properly initialize current allocation Guids.
* Added support in databases for the queries requried to initialize Guids.

### 2017-07-08
[Ripley]
* Rebased SQL scripts.
* Updated README to reflect new database update procedures.
* Changed GuidManager ranges to reflect ACE-World usage.
* More tweaks to debug output upon object assessment.
* Fixed some Enum errors and misspellings.

[Og II]
* Fixed issue with MotionStateFlag - setting now by the existance of data like we do with all other flags.   This prevents data / flag mismatch.

### 2017-07-07
[OptimShi]
* Added "@barbershop" debug command and corresponding functionality to load the interface, and save and apply settings.
* Changed some CharGen types that were ushort to uints. (They were short for legacy reasons no longer needed)

[Og II]
* Initial work on splitting a stackable item.   This is working and in an action chain.   Right now, it is only working for items
* in your position.   I need to extend it to work with items in chests and off the ground.
* There is some new type of sequence that we need to research and implement.   I put a big todo in the code.
* I made some posts about getting guid manager going.   We are at a point where we need it.   I modified some of the ranges to make better use
* of our guid space.
* TODO - find out about sequence and then do stack which is the complement function from split.

### 2017-07-06
[Ripley]
* Required ACE-World database version: v0.1.4
* Rolled in PR #430 from @Lidefeath as starting base.
* Added generator links table to WorldBase.
* Made changes to GuidManger and ObjectGuid in an effort to avoid collisions with guids already present in ACE-World. Does not persist or track, resets each server start.
* Above changes made IsCreature function, and smite works for at least the Sparring Golems found in the Academy, does not seem to work for Drudges in Holtburg.
* Speaking of Drudges, there is a drudge invasion south and west of Holtburg.
* Several changes to 06-06-30-2017-generator-chains-testdata.sql found in Database/Updates/World to set up for generator instances and show examples of them.
* Changed the way generators are classified to make use of some AceObject properties.
* Slightly adjusted debug output on ID panel.

[OptimShi]
* Character list will now automatically select your last played character.
* Modified @tele command to allow comma in coordinate pair (e.g. "@tele 42.1N, 33.6E")

### 2017-07-05
[OptimShi]
* Fixed an issue with skills not being set properly at character creation.
* Applied the +10 bonus to specialized skills

### 2017-07-04
[Og II]
* Cleaned up some concurrency issues.

### 2017-07-03
[Mogwai]
* Massive Database optimizations and implementations for saving objects and all their properties.

[OptimShi]
* Removed some old default character properties that were no longer needed and causing incorrect models and animations to load for players.

### 2017-07-02
[Og II]
* Started the removal of DebugObject - NPC's now spawn as creatures and containers now spawn as containers.  Need to finish this work.
* Appraisal Event is 90% coded.   All stats (int, bool, string, int64, DataId and Double) as sent and coded.   Armor, creature, spell and weapon profiles are
* complete.   TODO: Hook profiles and enchantment bitfields as well as base armor status for coverage
* Basic infrastructure for the correct motion stance is in place.   Normally I would not have included this WIP code, but it has no negative impact and I started in the wrong
* branch.   It would be a pain to pull it out.   If possible, I would like to add this as is - with the note that I will complete this next.
* Fixed issue with the autonomous position flag.   The turned ankle problem is gone.

### 2017-06-30
[Ripley]
* Changed WorldBase to allow for auto incrementing of aceObjectId and weenieClassId
* Changed WEENIE_MAX to be 199999 to account for ACE (re)created weenies starting at 100000
* Several changes to GuidManager and redirected CommonObjectFactory to use it instead of its own list
* Fixed some constructor issues related to Guids. Not sure if it is the right way to go about it but does seem to work in testing.
* Altered GameEventIdentifyObjectResponse to output debug stuff for more objects temporaryly and fixed its output of Guids in decimal form.

[Og II]
* Fixed issue with the wrong data size being used in the weenie data serilzation.
* Structure, MaxStructure, StackSize and MaxStackSize were being sent as uint and they are ushort.  This caused intermittant alignment issues.

[Mogwai]
* Fixed all compiler warnings

### 2017-06-29
[Mogwai and ddevec]
* Moved all shard database calls into an asynchronous interface implementation.  Callbacks are now necessary to get results back from db I/O.
* Calls to the shard database are now serialized in another thread and should eliminate the deadlocks we were seeing in MySql.
* Cleaned up session delay loading of Player/Character data to allow for async database operations.

[ddevec]
* Added basic support for motion broadcastin

### 2017-06-28
[Jyrus]
* Implement Auto close timer for Doors

[Og II]
* Update readme with db minimum required versions.
* Completed work on flattening PhysicsData and ModelData
* Did work to align names of properties on aceObject and WorldObject using unabbreviated names used by the client.
* Put in comments for aclogviewer names
* Cleaned up initailization code for various decendants of worldObject.
* Cleaned up redundant using statements.
* Tested all of my pack work, equipping items, item drop and pick up, portal summoning, world portals, ci and cirand as well as The
* item generator work.   Could not test corpse.
* TODO could not test corpse creation as kill was broken before I started.

[Lidefeath]
* Added linked generators i.e. a generator can spawn other generators which then spawn objects - theoretically this can go even deeper
* Added testdata for the linked generators near Holtburg:
	- spawn a Drudge Skulker with his generator
	- spawn a Drudge Camp (3 drudges) with its generator
	- randomly spawn 10 drudge camp generators in a landblock

[fantoms]
* Added `TimeoutTick` variable to network sessions, to store the tick value for next timeout.
* Added functionality to `Session` that will increase the network Session timeout limit when a `successful` packet has been receieved.
* Added a configuration variable called `DefaultSessionTimeout`, to control the session timeout value from outside of the code.
* Added a session State enum called `NetworkTimeout`, for dead sessions.
* Added functionality to `WorldManager.UpdateWorld`, that will disconnect active network sessions after they have reached a network timeout limit.
* If a session connects and does not proceed to authenticate, the server will give a shorter network timeout that is set as a default in the Authentication Handler.
* If a session advances through the authentication steps, it will be given the `DefaultSessionTimeout`.

[Jyrus]
* Reactivated portal requirements by directing the lookups to the weenie entries in the IntProperty table, instead of the AceObject entries.
* Fix StyleCop warning in CachingWorldDatabase.cs

[StackOverflow]
* Added the following Landblock loading helper console commands.
* loadLB (landblockid)  -- example "loadLB 0" - loads landblock 0.
* loadALB - loads all 65k landblocks.
* abortALB - aborts loading operation for all 65k Landblocks.

[Og II]
* Added the ability to equip items and have it also update your visual appearance.
* We have some data work to do - finish add clothing table did values
* Tested - pack to pack, slot to slot, pack to ground, ground to pack, ground to wielded, wielded to ground
* move packs, move items.
* Fixed save character options - this will need to be refactored - our character object needs some work.
* UI changes still blows up - this fix just addressed the options1 and options2
* TODO: clothing priority does not look to be right - somethings overlap incorrectly.

[Zegeger]
* Updated Sequence objects to allow for direct access to value (as opposed to just the byte array)
* Added MaxValue option to Sequence objects.
* Enabled sending network acks to client.
* Updated Motion sequence to set max value based on client source, MSB of the sequence is reserved.

### 2017-06-26
[Ripley]
* Expanded upon @Lidefeath's generator work
* Added complete generator test of tutorial dungeon part 1 setup for review

[fantoms]
* Changed from `uint` too `ulong` in the `bigint` properties, when a field has already been set.

[Jyrus]
* Add protection to the SpawnPortal method, so any old ushort cannot be used for the weenieclassID that it is expecting
* Use CommonObjectFactory.DynamicObjectId to assign a unique AceObjectId to each spawn
* Set the portal object as the IActor and the portal.CurrentLandblock.RemoveWorldObject(portal.Guid, false) as the action to perform
* Reduce the decay timer to 15 seconds

[Mogwai]
* added weenie caching layer
* fixed portals
* fixed null ref exception in the object factory, though I highly doubt i fixed the real issue

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
[Og II]
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
* Note that Children does not set properly yet but is seemingly not needed for the effect to work at least for static npcs/items.

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
* Changed Serialize to use WritePackedDwordOfKnownType for PaletteGuid, palette.PaletteId, texture.OldTexture, texture.NewTexture and model.ModelID.
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
