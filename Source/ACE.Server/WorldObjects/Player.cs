/**
 * Player.cs (Partial Class)
 * 
 * Player world object implementation for Asheron's Call Emulator (ACE)
 * 
 * This partial class implements the core Player functionality, extending the Creature
 * base class and implementing the IPlayer interface. It handles:
 * - Player authentication and session management
 * - Character data persistence and loading
 * - Movement and physics interactions
 * - Combat and appraisal systems
 * - Chat and communication handling
 * - Player killer (PK) status management
 * - Logout and session termination logic
 * - Administrative features and debugging tools
 * 
 * Author: ACE Development Team
 * Repository: https://github.com/ACEmulator/ACE
 * License: MIT License
 * 
 * Last Modified: 2025-09-19
 * Version: Master Branch
 * 
 * Dependencies:
 * - ACE.Common (shared utilities and extensions)
 * - ACE.Database (authentication and shard database models)
 * - ACE.DatLoader (portal.dat file parsing)
 * - ACE.Entity (core entity models and enums)
 * - ACE.Server (server managers and network handling)
 * - log4net (logging framework)
 * - System.Numerics (vector mathematics)
 */

using System;
using System.Collections.Generic;
using System.Numerics;                          // Vector3 for 3D position and velocity calculations
using log4net;                                  // Logging framework for debug, info, warn, and error messages
using ACE.Common;                               // Shared utility classes and constants
using ACE.Common.Extensions;                    // Extension methods for common operations
using ACE.Database;                             // Database manager for authentication and shard data
using ACE.Database.Models.Auth;                 // Authentication database models (Account)
using ACE.DatLoader;                            // Portal.dat file loading utilities
using ACE.DatLoader.FileTypes;                  // Dat file types (MotionTable, CombatManeuverTable, etc.)
using ACE.Entity;                               // Core entity classes and enums
using ACE.Entity.Enum;                          // Game enums (CombatMode, MotionStance, etc.)
using ACE.Entity.Enum.Properties;               // Property enums (PropertyString, PropertyBool, etc.)
using ACE.Entity.Models;                        // Entity data models
using ACE.Server.Entity;                        // Server-side entity implementations
using ACE.Server.Entity.Actions;                // Action chain and delay utilities
using ACE.Server.Managers;                      // World, Player, and Physics managers
using ACE.Server.Network;                       // Network session and packet handling
using ACE.Server.Network.GameEvent.Events;      // Game event packet types
using ACE.Server.Network.GameMessages.Messages; // Game message packet types
using ACE.Server.Network.Sequence;              // Network sequence management
using ACE.Server.Network.Structure;             // Network structure definitions
using ACE.Server.Physics;                       // Physics engine integration
using ACE.Server.Physics.Animation;             // Animation system
using ACE.Server.Physics.Common;                // Physics common utilities
using ACE.Server.WorldObjects.Managers;         // World object manager classes
using Character = ACE.Database.Models.Shard.Character; // Database character model alias
using MotionTable = ACE.DatLoader.FileTypes.MotionTable; // Motion table dat file alias

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Partial implementation of the Player class, handling core gameplay mechanics
    /// </summary>
    public partial class Player : Creature, IPlayer
    {
        /// <summary>
        /// Logger instance for this class, used for debugging and monitoring player actions
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The authenticated account this player belongs to
        /// </summary>
        public Account Account { get; }

        /// <summary>
        /// Database character model for this player, containing persistent data
        /// </summary>
        public Character Character { get; }

        /// <summary>
        /// Network session for this player, handles client communication
        /// </summary>
        public Session Session { get; }

        /// <summary>
        /// Manages player contracts and quest tracking
        /// </summary>
        public ContractManager ContractManager;

        /// <summary>
        /// Tracks if the player has recent network contact (used for timeout detection)
        /// </summary>
        public bool LastContact = true;

        /// <summary>
        /// Determines if the player is currently in a jumping state
        /// </summary>
        /// <remarks>
        /// In FastTick mode: Returns true if not on walkable surface
        /// In legacy mode: Also requires non-zero velocity to handle edge cases with ramps and monsters
        /// </remarks>
        public bool IsJumping
        {
            get
            {
                if (FastTick)
                    /// <summary>
                    /// FastTick mode: Simple check for walkable surface status
                    /// </summary>
                    return !PhysicsObj.TransientState.HasFlag(TransientStateFlags.OnWalkable);
                else
                {
                    /// <summary>
                    /// Legacy mode: Additional velocity check to fix NPK collision bugs near ramps
                    /// This prevents false jumping states during brief AutoPos frame glitches
                    /// </summary>
                    // for npks only, fixes a bug where OnWalkable can briefly lose state for 1 AutoPos frame
                    // a good repro for this is collision w/ monsters near the top of ramps
                    return !PhysicsObj.TransientState.HasFlag(TransientStateFlags.OnWalkable) && Velocity != Vector3.Zero;
                }
            }
        }

        /// <summary>
        /// Timestamp of the last jump action performed by this player
        /// </summary>
        public DateTime LastJumpTime;

        /// <summary>
        /// Records the last ground position before jumping (used for fall damage calculations)
        /// </summary>
        public ACE.Entity.Position LastGroundPos;

        /// <summary>
        /// Position snapshot used for position validation and anti-cheat measures
        /// </summary>
        public ACE.Entity.Position SnapPos;

        /// <summary>
        /// Manages player confirmation dialogs and prompts
        /// </summary>
        public ConfirmationManager ConfirmationManager;

        /// <summary>
        /// Handles player squelch (mute) functionality for chat filtering
        /// </summary>
        public SquelchManager SquelchManager;

        /// <summary>
        /// Maximum radar range for indoor environments (25 units)
        /// </summary>
        public const float MaxRadarRange_Indoors = 25.0f;

        /// <summary>
        /// Maximum radar range for outdoor environments (75 units)
        /// </summary>
        public const float MaxRadarRange_Outdoors = 75.0f;

        /// <summary>
        /// Timestamp of the last object send operation (used for network throttling)
        /// </summary>
        public DateTime PrevObjSend;

        /// <summary>
        /// Gets the current radar range based on indoor/outdoor status
        /// </summary>
        public float CurrentRadarRange => Location.Indoors ? MaxRadarRange_Indoors : MaxRadarRange_Outdoors;

        /// <summary>
        /// Creates a new player instance from weenie template data
        /// </summary>
        /// <param name="weenie">Base weenie template for character creation</param>
        /// <param name="guid">Unique object GUID for this player</param>
        /// <param name="accountId">Database account ID for authentication</param>
        /// <remarks>
        /// Used during character creation - initializes fresh player with default values
        /// </remarks>
        public Player(Weenie weenie, ObjectGuid guid, uint accountId) : base(weenie, guid)
        {
            /// <summary>
            /// Initialize database character model with basic identity information
            /// </summary>
            Character = new Character();
            Character.Id = guid.Full;
            Character.AccountId = accountId;
            Character.Name = GetProperty(PropertyString.Name);
            CharacterChangesDetected = true;

            /// <summary>
            /// Load account data from authentication database
            /// </summary>
            Account = DatabaseManager.Authentication.GetAccountById(Character.AccountId);

            /// <summary>
            /// Set ephemeral (temporary/runtime) values that don't persist to database
            /// </summary>
            SetEphemeralValues();

            /// <summary>
            /// Ensure required properties have valid default values
            /// </summary>
            // Make sure properties this WorldObject requires are not null.
            AvailableExperience = AvailableExperience ?? 0;
            TotalExperience = TotalExperience ?? 0;
            Attackable = true;

            /// <summary>
            /// Set character creation timestamp
            /// </summary>
            SetProperty(PropertyString.DateOfBirth, $"{DateTime.UtcNow:dd MMMM yyyy}");

            /// <summary>
            /// Handle special inventory setup for Olthoi player race
            /// </summary>
            if (IsOlthoiPlayer)
            {
                GenerateContainList();
            }
            else
                Biota.PropertiesCreateList?.Clear();
        }

        /// <summary>
        /// Restores a player from persistent database storage
        /// </summary>
        /// <param name="biota">Base biota data from shard database</param>
        /// <param name="inventory">Collection of inventory biota records</param>
        /// <param name="wieldedItems">Collection of equipped item biota records</param>
        /// <param name="character">Character database model with persistent data</param>
        /// <param name="session">Active network session for this player</param>
        /// <remarks>
        /// Used during player login - reconstructs full player state from saved data
        /// </remarks>
        public Player(Biota biota, IEnumerable<ACE.Database.Models.Shard.Biota> inventory, IEnumerable<ACE.Database.Models.Shard.Biota> wieldedItems, Character character, Session session) : base(biota)
        {
            /// <summary>
            /// Link database character model and network session
            /// </summary>
            Character = character;
            Session = session;

            /// <summary>
            /// Load account authentication data
            /// </summary>
            Account = DatabaseManager.Authentication.GetAccountById(Character.AccountId);

            /// <summary>
            /// Initialize runtime/ephemeral values
            /// </summary>
            SetEphemeralValues();

            /// <summary>
            /// Sort and load inventory items into appropriate containers
            /// </summary>
            SortBiotasIntoInventory(inventory);

            /// <summary>
            /// Add equipped/wielded items to character
            /// </summary>
            AddBiotasToEquippedObjects(wieldedItems);

            /// <summary>
            /// Recalculate total coin value from inventory
            /// </summary>
            UpdateCoinValue(false);
        }

        /// <summary>
        /// Initializes the physics object for this player with default collision properties
        /// </summary>
        /// <remarks>
        /// Sets initial "pink bubble" state (invisible, non-collidable) during login
        /// </remarks>
        public override void InitPhysicsObj()
        {
            base.InitPhysicsObj();

            /// <summary>
            /// Configure initial collision properties for login state
            /// </summary>
            // set pink bubble state
            IgnoreCollisions = true; ReportCollisions = false; Hidden = true;
        }

        /// <summary>
        /// Sets ephemeral (non-persistent) runtime values for the player
        /// </summary>
        /// <remarks>
        /// Called during both character creation and login to initialize temporary state
        /// </remarks>
        private void SetEphemeralValues()
        {
            /// <summary>
            /// Mark as player object for network description flags
            /// </summary>
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Player;

            /// <summary>
            /// Initialize first enter world state and default combat stance
            /// </summary>
            // This is the default send upon log in and the most common. Anything with a velocity will need to add that flag.
            // This should be handled automatically...
            //PositionFlags |= PositionFlags.OrientationHasNoX | PositionFlags.OrientationHasNoY | PositionFlags.IsGrounded | PositionFlags.HasPlacementID;
            FirstEnterWorldDone = false;
            SetStance(MotionStance.NonCombat, false);

            /// <summary>
            /// Set initial listening radius for object updates (5 units)
            /// </summary>
            // radius for object updates
            ListeningRadius = 5f;

            /// <summary>
            /// Apply account-level permission overrides based on access level
            /// </summary>
            if (Session != null && ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    IsArch = true;
                if (Session.AccessLevel == AccessLevel.Sentinel)
                    IsSentinel = true;
                if (Session.AccessLevel == AccessLevel.Envoy)
                {
                    IsEnvoy = true;
                    IsSentinel = true; //IsEnvoy is not recognized by the client and therefore the client should treat the user as a Sentinel.
                }
                if (Session.AccessLevel == AccessLevel.Advocate)
                    IsAdvocate = true;
            }

            /// <summary>
            /// Detect special player races (Olthoi, Gearknight with core plating)
            /// </summary>
            IsOlthoiPlayer = HeritageGroup == HeritageGroup.Olthoi || HeritageGroup == HeritageGroup.OlthoiAcid;
            IsGearKnightPlayer = PropertyManager.GetBool("gearknight_core_plating").Item && HeritageGroup == HeritageGroup.Gearknight;

            /// <summary>
            /// Calculate container capacity including augmentation bonuses
            /// </summary>
            ContainerCapacity = (byte)(7 + AugmentationExtraPackSlot);

            /// <summary>
            /// Handle advocate quest permissions (character-specific regardless of account override)
            /// </summary>
            if (Session != null && AdvocateQuest && IsAdvocate) // Advocate permissions are per character regardless of override
            {
                if (Session.AccessLevel == AccessLevel.Player)
                    Session.SetAccessLevel(AccessLevel.Advocate); // Elevate to Advocate permissions
                if (AdvocateLevel > 4)
                    IsPsr = true; // Enable AdvocateTeleport via MapClick
            }

            /// <summary>
            /// Load combat maneuver table from portal.dat
            /// </summary>
            CombatTable = DatManager.PortalDat.ReadFromDat<CombatManeuverTable>(CombatTableDID.Value);

            /// <summary>
            /// Initialize manager instances for various gameplay systems
            /// </summary>
            _questManager = new QuestManager(this);
            ContractManager = new ContractManager(this);
            ConfirmationManager = new ConfirmationManager(this);
            LootPermission = new Dictionary<ObjectGuid, DateTime>();
            SquelchManager = new SquelchManager(this);
            MagicState = new MagicState(this);
            FoodState = new FoodState(this);
            RecordCast = new RecordCast(this);
            AttackQueue = new AttackQueue(this);

            /// <summary>
            /// Initialize default PK kill counters if not set
            /// </summary>
            if (!PlayerKillsPk.HasValue)
                PlayerKillsPk = 0;
            if (!PlayerKillsPkl.HasValue)
                PlayerKillsPkl = 0;

            /// <summary>
            /// Early return - legacy code preserved below for reference
            /// </summary>
            return; // todo

            // =======================================
            // This code was taken from the old Load()
            // =======================================
            /*AceCharacter character;
            if (Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    character.IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    character.IsArch = true;
                if (Session.AccessLevel == AccessLevel.Envoy)
                    character.IsEnvoy = true;
                // TODO: Need to setup and account properly for IsSentinel and IsAdvocate.
                // if (Session.AccessLevel == AccessLevel.Sentinel)
                // character.IsSentinel = true;
                // if (Session.AccessLevel == AccessLevel.Advocate)
                // character.IsAdvocate= true;
            }*/
            // FirstEnterWorldDone = false;
            // IsAlive = true;
        }

        /// <summary>
        /// Gets whether this character has been marked for permanent deletion
        /// </summary>
        public bool IsDeleted => Character.IsDeleted;

        /// <summary>
        /// Gets whether this character is pending deletion (timer active but not finalized)
        /// </summary>
        public bool IsPendingDeletion => Character.DeleteTime > 0 && !IsDeleted;

        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        /// <summary>
        /// Current combat stance state (legacy field - marked for removal)
        /// </summary>
        public MotionStance stance = MotionStance.NonCombat;

        /// <summary>
        /// Handles player appraisal action (pressing 'e' key to examine object)
        /// </summary>
        /// <param name="objectGuid">GUID of object to appraise</param>
        /// <remarks>
        /// Implements appraisal skill checks and rate limiting to prevent spam
        /// </remarks>
        public void HandleActionIdentifyObject(uint objectGuid)
        {
            //Console.WriteLine($"{Name}.HandleActionIdentifyObject({objectGuid:X8})");
            if (objectGuid == 0)
            {
                /// <summary>
                /// Clear appraisal target selection when no object specified
                /// </summary>
                // Deselect the formerly selected Target
                //selectedTarget = ObjectGuid.Invalid;
                RequestedAppraisalTarget = null;
                CurrentAppraisalTarget = null;
                return;
            }

            /// <summary>
            /// Find target object in world, inventory, or equipped items
            /// </summary>
            var wo = FindObject(objectGuid, SearchLocations.Everywhere, out _, out _, out _);
            if (wo == null)
            {
                //log.DebugFormat("{0}.HandleActionIdentifyObject({1:X8}): couldn't find object", Name, objectGuid);
                Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, objectGuid));
                return;
            }

            /// <summary>
            /// Get current timestamp for rate limiting calculations
            /// </summary>
            var currentTime = Time.GetUnixTime();

            /// <summary>
            /// Handle continued appraisal of same target (no RNG needed after first success)
            /// </summary>
            // compare with previously requested appraisal target
            if (objectGuid == RequestedAppraisalTarget)
            {
                if (objectGuid == CurrentAppraisalTarget)
                {
                    // continued success, rng roll no longer needed
                    Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, wo, true));
                    OnAppraisal(wo, true);
                    return;
                }
                if (currentTime < AppraisalRequestedTimestamp + 5.0f)
                {
                    // rate limit for unsuccessful appraisal spam
                    Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, wo, false));
                    OnAppraisal(wo, false);
                    return;
                }
            }

            /// <summary>
            /// Set new appraisal target and timestamp for rate limiting
            /// </summary>
            RequestedAppraisalTarget = objectGuid;
            AppraisalRequestedTimestamp = currentTime;

            /// <summary>
            /// Perform appraisal skill check
            /// </summary>
            Examine(wo);
        }

        /// <summary>
        /// Performs appraisal skill check against target object
        /// </summary>
        /// <param name="obj">World object to examine</param>
        /// <remarks>
        /// Handles creature vs player appraisal with appropriate skill checks
        /// </remarks>
        public void Examine(WorldObject obj)
        {
            //Console.WriteLine($"{Name}.Examine({obj.Name})");
            var success = true;
            var creature = obj as Creature;
            Player player = null;

            /// <summary>
            /// Determine appraisal type and skill requirements
            /// </summary>
            if (creature != null)
            {
                player = obj as Player;
                var skill = player != null ? Skill.AssessPerson : Skill.AssessCreature;
                var currentSkill = (int)GetCreatureSkill(skill).Current;
                int difficulty = (int)creature.GetCreatureSkill(Skill.Deception).Current;

                /// <summary>
                /// Special case: Untrained assess creature uses focus/self average
                /// </summary>
                if (PropertyManager.GetBool("assess_creature_mod").Item && skill == Skill.AssessCreature
                        && Skills[Skill.AssessCreature].AdvancementClass < SkillAdvancementClass.Trained)
                    currentSkill = (int)((Focus.Current + Self.Current) / 2);

                /// <summary>
                /// Calculate success chance based on skill vs difficulty
                /// </summary>
                var chance = SkillCheck.GetSkillChance(currentSkill, difficulty);

                /// <summary>
                /// Special cases for guaranteed success
                /// </summary>
                if (difficulty == 0 || player == this || player != null && !player.GetCharacterOption(CharacterOption.AttemptToDeceiveOtherPlayers))
                    chance = 1.0f;
                if ((this is Admin || this is Sentinel) && CloakStatus == CloakStatus.On)
                    chance = 1.0f;

                /// <summary>
                /// Perform random skill check
                /// </summary>
                success = chance > ThreadSafeRandom.Next(0.0f, 1.0f);
            }

            /// <summary>
            /// Item appraisal resistance check (999+ = immune)
            /// </summary>
            if (obj.ResistItemAppraisal >= 999)
                success = false;

            /// <summary>
            /// Pets always succeed appraisal
            /// </summary>
            if (creature is Pet || creature is CombatPet)
                success = true;

            /// <summary>
            /// Track successful appraisal target
            /// </summary>
            if (success)
                CurrentAppraisalTarget = obj.Guid.Full;

            /// <summary>
            /// Send appraisal result to client
            /// </summary>
            Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, obj, success));
            OnAppraisal(obj, success);
        }

        /// <summary>
        /// Handles appraisal completion events and side effects
        /// </summary>
        /// <param name="obj">Appraised object</param>
        /// <param name="success">Whether appraisal succeeded</param>
        /// <remarks>
        /// Handles failure notifications and monster aggression triggers
        /// </remarks>
        public void OnAppraisal(WorldObject obj, bool success)
        {
            /// <summary>
            /// Notify target player of failed appraisal attempt (unless squelched)
            /// </summary>
            if (!success && obj is Player player && !player.SquelchManager.Squelches.Contains(this, ChatMessageType.Appraisal))
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} tried and failed to assess you!", ChatMessageType.Appraisal));

            /// <summary>
            /// Handle monster aggression from appraisal (Pooky logic)
            /// </summary>
            // pooky logic - handle monsters attacking on appraisal
            if (obj is Creature creature && creature.MonsterState == State.Idle)
            {
                if (creature.Tolerance.HasFlag(Tolerance.Appraise))
                {
                    creature.AttackTarget = this;
                    creature.WakeUp();
                }
            }
        }

        /// <summary>
        /// Called when player collides with environment geometry
        /// </summary>
        /// <remarks>
        /// Currently disabled - fall damage handling removed
        /// </remarks>
        public override void OnCollideEnvironment()
        {
            //HandleFallingDamage();
        }

        /// <summary>
        /// Handles collision with other world objects
        /// </summary>
        /// <param name="target">Collided world object</param>
        /// <remarks>
        /// Routes collision handling to appropriate object-specific handlers
        /// </remarks>
        public override void OnCollideObject(WorldObject target)
        {
            //Console.WriteLine($"{Name}.OnCollideObject({target.Name})");
            if (target.ReportCollisions == false)
                return;

            /// <summary>
            /// Route collision to specialized handlers based on object type
            /// </summary>
            if (target is Portal portal)
                portal.OnCollideObject(this);
            else if (target is PressurePlate pressurePlate)
                pressurePlate.OnCollideObject(this);
            else if (target is Hotspot hotspot)
                hotspot.OnCollideObject(this);
            else if (target is SpellProjectile spellProjectile)
                spellProjectile.OnCollideObject(this);
            else if (target.ProjectileTarget != null)
                ProjectileCollisionHelper.OnCollideObject(target, this);
        }

        /// <summary>
        /// Handles end of collision with world objects
        /// </summary>
        /// <param name="target">Object collision has ended with</param>
        public override void OnCollideObjectEnd(WorldObject target)
        {
            if (target is Hotspot hotspot)
                hotspot.OnCollideObjectEnd(this);
        }

        /// <summary>
        /// Handles player health query action (right-click on creature)
        /// </summary>
        /// <param name="objectGuid">GUID of creature to query</param>
        public void HandleActionQueryHealth(uint objectGuid)
        {
            if (objectGuid == 0)
            {
                /// <summary>
                /// Clear target selection
                /// </summary>
                // Deselect the formerly selected Target
                UpdateSelectedTarget(null);
                return;
            }

            /// <summary>
            /// Find target creature in current landblock
            /// </summary>
            var obj = CurrentLandblock?.GetObject(objectGuid) as Creature;
            if (obj == null)
            {
                /// <summary>
                /// Clear selection if target not found
                /// </summary>
                // Deselect the formerly selected Target
                UpdateSelectedTarget(null);
                return;
            }

            /// <summary>
            /// Update selection and request health data
            /// </summary>
            UpdateSelectedTarget(obj);
            obj.QueryHealth(Session);
        }

        /// <summary>
        /// Updates the currently selected target creature
        /// </summary>
        /// <param name="target">New target creature</param>
        /// <remarks>
        /// Handles target selection/deselection events and notifications
        /// </remarks>
        private void UpdateSelectedTarget(Creature target)
        {
            if (selectedTarget != null)
            {
                /// <summary>
                /// Notify previous target of deselection
                /// </summary>
                var prevSelected = selectedTarget.TryGetWorldObject() as Creature;
                if (prevSelected != null)
                    prevSelected.OnTargetDeselected(this);
            }

            if (target != null)
            {
                /// <summary>
                /// Set new target and notify of selection
                /// </summary>
                selectedTarget = new WorldObjectInfo(target);
                HealthQueryTarget = target.Guid.Full;
                target.OnTargetSelected(this);
            }
            else
            {
                /// <summary>
                /// Clear target selection
                /// </summary>
                selectedTarget = null;
                HealthQueryTarget = null;
            }
        }

        /// <summary>
        /// Handles item mana query action (right-click on mana-using item)
        /// </summary>
        /// <param name="itemGuid">GUID of item to query</param>
        public void HandleActionQueryItemMana(uint itemGuid)
        {
            if (itemGuid == 0)
            {
                /// <summary>
                /// Clear mana query target
                /// </summary>
                ManaQueryTarget = null;
                return;
            }

            /// <summary>
            /// Search for item in inventory or equipped slots
            /// </summary>
            // the object could be in the world or on the player, first check player
            var item = GetInventoryItem(itemGuid) ?? GetEquippedItem(itemGuid);
            if (item != null)
                item.QueryItemMana(Session);
            ManaQueryTarget = itemGuid;
        }

        /// <summary>
        /// Broadcasts death message to nearby players when a kill occurs
        /// </summary>
        /// <param name="deathMessage">Formatted death message text</param>
        /// <param name="victimId">GUID of the killed player</param>
        /// <param name="killerId">GUID of the killing player</param>
        /// <remarks>
        /// TODO: Determine exact broadcast range and recipient logic (landblock vs lifestone)
        /// </remarks>
        public void ActionBroadcastKill(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
        {
            var deathBroadcast = new GameMessagePlayerKilled(deathMessage, victimId, killerId);
            // OutdoorChatRange?
            EnqueueBroadcast(deathBroadcast);
        }

        /// <summary>
        /// Plays a sound effect at the player's location with specified volume
        /// </summary>
        /// <param name="sound">Sound effect ID to play</param>
        /// <param name="sourceId">Source object GUID for sound origin</param>
        /// <param name="volume">Volume multiplier (0.0-1.0)</param>
        /// <remarks>
        /// Client handles sound attenuation based on listener distance from source
        /// </remarks>
        public void PlaySound(Sound sound, ObjectGuid sourceId, float volume = 1.0f)
        {
            Session.Network.EnqueueSend(new GameMessageSound(sourceId, sound, volume));
        }

        /// <summary>
        /// Flag indicating PK logout delay is active (prevents immediate logout after combat)
        /// </summary>
        /// <remarks>
        /// Returns TRUE if a Player Killer has clicked logout after being involved in a PK battle
        /// within the past 2 mins. The server delays the logout for 20s, and the client remains
        /// in frozen state during this delay
        /// </remarks>
        public bool PKLogout;

        /// <summary>
        /// Flag indicating logout process is currently active
        /// </summary>
        public bool IsLoggingOut;

        /// <summary>
        /// Initiates player logout process with optional force override
        /// </summary>
        /// <param name="clientSessionTerminatedAbruptly">True if client disconnected unexpectedly</param>
        /// <param name="forceImmediate">Bypasses PK timer restrictions</param>
        /// <returns>True if logout completed immediately</returns>
        /// <remarks>
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </remarks>
        public bool LogOut(bool clientSessionTerminatedAbruptly = false, bool forceImmediate = false)
        {
            if (PKLogoutActive && !forceImmediate)
            {
                //Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                Session.Network.EnqueueSend(new GameMessageSystemChat("Beginning delayed player killer logoff...", ChatMessageType.Broadcast));
                if (!PKLogout)
                {
                    PKLogout = true;
                    IsFrozen = true;
                    EnqueueBroadcastPhysicsState();
                    LogoffTimestamp = Time.GetFutureUnixTime(PropertyManager.GetLong("pk_timer").Item);
                    PlayerManager.AddPlayerToLogoffQueue(this);
                }
                return false;
            }
            LogOut_Inner(clientSessionTerminatedAbruptly);
            return true;
        }

        /// <summary>
        /// Core logout processing (called by LogOut or after PK timer expires)
        /// </summary>
        /// <param name="clientSessionTerminatedAbruptly">True if abrupt client termination</param>
        public void LogOut_Inner(bool clientSessionTerminatedAbruptly = false)
        {
            IsBusy = true;
            IsLoggingOut = true;
            PlayerManager.AddPlayerToFinalLogoffQueue(this);

            /// <summary>
            /// Handle fellowship cleanup on logout
            /// </summary>
            if (Fellowship != null)
                FellowshipQuit(false);

            /// <summary>
            /// Cancel active trades on logout
            /// </summary>
            if (IsTrading && TradePartner != ObjectGuid.Invalid)
            {
                var tradePartner = PlayerManager.GetOnlinePlayer(TradePartner);
                if (tradePartner != null)
                    tradePartner.HandleActionCloseTradeNegotiations();
            }

            /// <summary>
            /// Clean up chat channel subscriptions (skip if abrupt disconnect)
            /// </summary>
            if (!clientSessionTerminatedAbruptly)
            {
                if (PropertyManager.GetBool("use_turbine_chat").Item)
                {
                    if (IsOlthoiPlayer)
                    {
                        LeaveTurbineChatChannel("Olthoi");
                    }
                    else
                    {
                        if (GetCharacterOption(CharacterOption.ListenToGeneralChat))
                            LeaveTurbineChatChannel("General");
                        if (GetCharacterOption(CharacterOption.ListenToTradeChat))
                            LeaveTurbineChatChannel("Trade");
                        if (GetCharacterOption(CharacterOption.ListenToLFGChat))
                            LeaveTurbineChatChannel("LFG");
                        if (GetCharacterOption(CharacterOption.ListenToRoleplayChat))
                            LeaveTurbineChatChannel("Roleplay");
                        if (GetCharacterOption(CharacterOption.ListenToAllegianceChat) && Allegiance != null)
                            LeaveTurbineChatChannel("Allegiance");
                        if (GetCharacterOption(CharacterOption.ListenToSocietyChat) && Society != FactionBits.None)
                            LeaveTurbineChatChannel("Society");
                    }
                }
            }

            /// <summary>
            /// Destroy active pets on logout
            /// </summary>
            if (CurrentActivePet != null)
                CurrentActivePet.Destroy();

            /// <summary>
            /// Block logout during death animation sequence
            /// </summary>
            // If we're in the dying animation process, we cannot logout until that animation completes..
            if (IsInDeathProcess)
                return;

            LogOut_Final();
        }

        /// <summary>
        /// Finalizes logout with optional animation skipping
        /// </summary>
        /// <param name="skipAnimations">Bypass logout animation sequence</param>
        private void LogOut_Final(bool skipAnimations = false)
        {
            if (CurrentLandblock != null)
            {
                if (skipAnimations)
                {
                    FinalizeLogout();
                }
                else
                {
                    /// <summary>
                    /// Clear frozen state before animation
                    /// </summary>
                    if (IsFrozen ?? false)
                        IsFrozen = false;
                    EnqueueBroadcastPhysicsState();

                    /// <summary>
                    /// Create logout animation sequence
                    /// </summary>
                    var motionCommand = MotionCommand.LogOut;
                    var motion = new Motion(this, motionCommand);
                    var stanceNonCombat = MotionStance.NonCombat;
                    var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stanceNonCombat, motionCommand);
                    var logoutChain = new ActionChain();

                    /// <summary>
                    /// Send logout motion command to client
                    /// </summary>
                    logoutChain.AddAction(this, () => SendMotionAsCommands(motionCommand, stanceNonCombat));
                    logoutChain.AddDelaySeconds(animLength);

                    /// <summary>
                    /// Final cleanup after animation completes
                    /// </summary>
                    // remove the player from landblock management -- after the animation has run
                    logoutChain.AddAction(WorldManager.ActionQueue, () =>
                    {
                        // If we're in the dying animation process, we cannot RemoveWorldObject and logout until that animation completes..
                        if (IsInDeathProcess)
                            return;
                        FinalizeLogout();
                    });

                    /// <summary>
                    /// Close any open containers before leaving landblock
                    /// </summary>
                    // close any open landblock containers (chests / corpses)
                    if (LastOpenedContainerId != ObjectGuid.Invalid)
                    {
                        var container = CurrentLandblock.GetObject(LastOpenedContainerId) as Container;
                        if (container != null)
                            container.Close(this);
                    }
                    logoutChain.EnqueueChain();
                }
            }
            else
            {
                FinalizeLogout();
            }
        }

        /// <summary>
        /// Timestamp when final logout processing completed
        /// </summary>
        public double LogOffFinalizedTime;

        /// <summary>
        /// Flag for admin-initiated forced logout requests
        /// </summary>
        public bool ForcedLogOffRequested;

        /// <summary>
        /// Executes forced logout for admin commands or server shutdown
        /// </summary>
        /// <remarks>
        /// THIS FUNCTION FOR SYSTEM USE ONLY; If you want to force a player to logout, use Session.LogOffPlayer().
        /// </remarks>
        public void ForceLogoff()
        {
            if (!ForcedLogOffRequested) return;
            log.WarnFormat("[LOGOUT] Executing ForcedLogoff for Account {0} with character {1} (0x{2}) at {3}.", Account.AccountName, Name, Guid, DateTime.Now.ToCommonString());
            FinalizeLogout();
            ForcedLogOffRequested = false;
        }

        /// <summary>
        /// Performs final cleanup and database synchronization for logout
        /// </summary>
        private void FinalizeLogout()
        {
            PlayerManager.RemovePlayerFromFinalLogoffQueue(this);
            CurrentLandblock?.RemoveWorldObject(Guid, false);

            /// <summary>
            /// Update properties for logged out state
            /// </summary>
            SetPropertiesAtLogOut();

            /// <summary>
            /// Persist final state to database
            /// </summary>
            SavePlayerToDatabase();

            /// <summary>
            /// Update player manager status
            /// </summary>
            PlayerManager.SwitchPlayerFromOnlineToOffline(this);

            log.DebugFormat("[LOGOUT] Account {0} exited the world with character {1} (0x{2}) at {3}.", Account.AccountName, Name, Guid, DateTime.Now.ToCommonString());
        }

        /// <summary>
        /// Handles MRT (Magic Resistance Toggle?) admin command
        /// </summary>
        /// <param name="choice">Toggle option (-1=do nothing, 0=off, 1=on, 2=toggle)</param>
        /// <remarks>
        /// Requires Admin flag on ObjectDescriptionFlags - intended for Admin subclass
        /// </remarks>
        public void HandleMRT()
        {
            // This requires the Admin flag set on ObjectDescriptionFlags
            // I would expect this flag to be set in Admin.cs which would be a subclass of Player
            // FIXME: maybe move to Admin class?
            // TODO: reevaluate class location
            // The EnqueueBroadcastUpdateObject below sends the player back into teleport. I assume at this point, this was never done to players
            // EnqueueBroadcastUpdateObject();
            // The private message below worked as expected, but it only broadcast to the player. This would be a problem with for others in range seeing something try to
            // pass through a barrier but not being allowed.
            // var updateBool = new GameMessagePrivateUpdatePropertyBool(Session, PropertyBool.IgnoreHouseBarriers, ImmuneCellRestrictions);
            // Session.Network.EnqueueSend(updateBool);
            UpdateProperty(this, PropertyBool.IgnoreHouseBarriers, !IgnoreHouseBarriers, true);
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Bypass Housing Barriers now set to: {IgnoreHouseBarriers}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Sends autonomous position update to client (currently disabled)
        /// </summary>
        public void SendAutonomousPosition()
        {
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }

        /// <summary>
        /// Forces object description send to client when requested
        /// </summary>
        /// <param name="itemGuid">GUID of object to describe</param>
        public void HandleActionForceObjDescSend(uint itemGuid)
        {
            var wo = FindObject(itemGuid, SearchLocations.Everywhere);
            if (wo == null)
            {
                //log.DebugFormat("HandleActionForceObjDescSend() - couldn't find object {0:X8}", itemGuid);
                return;
            }
            Session.Network.EnqueueSend(new GameMessageObjDescEvent(wo));
        }

        /// <summary>
        /// Applies sound effect to player character
        /// </summary>
        /// <param name="sound">Sound effect to play</param>
        public void HandleActionApplySoundEffect(Sound sound)
        {
            PlaySound(sound, Guid);
        }

        //public void TestWieldItem(Session session, uint modelId, int palOption, float shade = 0)
        //{
        // // ClothingTable item = ClothingTable.ReadFromDat(0x1000002C); // Olthoi Helm
        // // ClothingTable item = ClothingTable.ReadFromDat(0x10000867); // Cloak
        // // ClothingTable item = ClothingTable.ReadFromDat(0x10000008); // Gloves
        // // ClothingTable item = ClothingTable.ReadFromDat(0x100000AD); // Heaume
        // var item = DatManager.PortalDat.ReadFromDat<ClothingTable>(modelId);
        // int palCount = 0;
        // List<uint> coverage = new List<uint>(); // we'll store our fake coverage items here
        // ClearObjDesc();
        // AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette
        // if (item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
        // {
        // // Add the model and texture(s)
        // ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
        // for (int i = 0; i < clothingBaseEffec.CloObjectEffects.Count; i++)
        // {
        // byte partNum = (byte)clothingBaseEffec.CloObjectEffects[i].Index;
        // AddModel((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].ModelId);
        // coverage.Add(partNum);
        // for (int j = 0; j < clothingBaseEffec.CloObjectEffects[i].CloTextureEffects.Count; j++)
        // AddTexture((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
        // }
        // // Apply an appropriate palette. We'll just pick a random one if not specificed--it's a surprise every time!
        // // For actual equipment, these should just be stored in the ace_object palette_change table and loaded from there
        // if (item.ClothingSubPalEffects.Count > 0)
        // {
        // int size = item.ClothingSubPalEffects.Count;
        // palCount = size;
        // CloSubPalEffect itemSubPal;
        // // Generate a random index if one isn't provided
        // if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
        // {
        // itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
        // }
        // else
        // {
        // List<CloSubPalEffect> values = item.ClothingSubPalEffects.Values.ToList();
        // Random rand = new Random();
        // palOption = rand.Next(size);
        // itemSubPal = values[palOption];
        // }
        // for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
        // {
        // var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
        // ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);
        // for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
        // {
        // uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
        // uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
        // AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
        // }
        // }
        // }
        // // Add the "naked" body parts. These are the ones not already covered.
        // var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>((uint)SetupTableId);
        // for (byte i = 0; i < baseSetup.Parts.Count; i++)
        // {
        // if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head.
        // AddModel(i, baseSetup.Parts[i]);
        // }
        // var objDescEvent = new GameMessageObjDescEvent(this);
        // session.Network.EnqueueSend(objDescEvent);
        // ChatPacket.SendServerMessage(session, "Equipping model " + modelId.ToString("X") +
        // ", Applying palette index " + palOption + " of " + palCount +
        // " with a shade value of " + shade + ".", ChatMessageType.Broadcast);
        // }
        // else
        // {
        // // Alert about the failure
        // ChatPacket.SendServerMessage(session, "Could not match that item to your character model.", ChatMessageType.Broadcast);
        // }
        //}

        /// <summary>
        /// Handles local speech chat messages from player
        /// </summary>
        /// <param name="message">Text message to broadcast</param>
        public void HandleActionTalk(string message)
        {
            if (!IsGagged)
            {
                /// <summary>
                /// Broadcast speech message to local players within range
                /// </summary>
                EnqueueBroadcast(new GameMessageHearSpeech(message, GetNameWithSuffix(), Guid.Full, ChatMessageType.Speech), LocalBroadcastRange, ChatMessageType.Speech);
                OnTalk(message);
            }
            else
                SendGagError();
        }

        /// <summary>
        /// Sends error message when gagged player attempts to speak
        /// </summary>
        public void SendGagError()
        {
            var msg = "You are unable to talk locally, globally, or send tells because you have been gagged.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        /// <summary>
        /// Notifies player that gag has been applied
        /// </summary>
        public void SendGagNotice()
        {
            var msg = "Your chat privileges have been suspended.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        /// <summary>
        /// Notifies player that gag has been removed
        /// </summary>
        public void SendUngagNotice()
        {
            var msg = "Your chat privileges have been restored.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        /// <summary>
        /// Handles emote text chat messages
        /// </summary>
        /// <param name="message">Emote text to broadcast</param>
        public void HandleActionEmote(string message)
        {
            if (!IsGagged)
            {
                /// <summary>
                /// Broadcast emote to local players
                /// </summary>
                EnqueueBroadcast(new GameMessageEmoteText(Guid.Full, GetNameWithSuffix(), message), LocalBroadcastRange);
                OnTalk(message);
            }
            else
                SendGagError();
        }

        /// <summary>
        /// Handles soul emote (global) chat messages
        /// </summary>
        /// <param name="message">Soul emote text to broadcast</param>
        /// <remarks>
        /// Olthoi players can opt out of soul emotes via NoOlthoiTalk flag
        /// </remarks>
        public void HandleActionSoulEmote(string message)
        {
            if (!IsGagged)
            {
                if (!IsOlthoiPlayer || (IsOlthoiPlayer && NoOlthoiTalk))
                    EnqueueBroadcast(new GameMessageSoulEmote(Guid.Full, Name, message), LocalBroadcastRange);
                OnTalk(message);
            }
            else
                SendGagError();
        }

        /// <summary>
        /// Processes chat message for emote manager reactions from nearby creatures
        /// </summary>
        /// <param name="message">Chat message content</param>
        /// <remarks>
        /// Only processes if physics and landblock are valid
        /// </remarks>
        public void OnTalk(string message)
        {
            if (PhysicsObj == null || CurrentLandblock == null) return;

            /// <summary>
            /// Determine broadcast range based on dungeon/outdoor status
            /// </summary>
            var isDungeon = CurrentLandblock.PhysicsLandblock != null && CurrentLandblock.PhysicsLandblock.IsDungeon;
            var rangeSquared = LocalBroadcastRangeSq;

            /// <summary>
            /// Process message for all nearby creatures within range
            /// </summary>
            foreach (var creature in PhysicsObj.ObjMaint.GetKnownObjectsValuesAsCreature())
            {
                if (isDungeon && Location.Landblock != creature.Location.Landblock)
                    continue;
                var distSquared = Location.SquaredDistanceTo(creature.Location);
                if (distSquared <= rangeSquared)
                    creature.EmoteManager.OnHearChat(this, message);
            }
        }

        /// <summary>
        /// Handles player jump action with physics and stamina calculations
        /// </summary>
        /// <param name="jump">Jump parameters including velocity and extent</param>
        public void HandleActionJump(JumpPack jump)
        {
            /// <summary>
            /// Record jump start position for fall damage calculations
            /// </summary>
            StartJump = new ACE.Entity.Position(Location);
            //Console.WriteLine($"JumpPack: Velocity: {jump.Velocity}, Extent: {jump.Extent}");

            /// <summary>
            /// Calculate stamina cost based on strength, encumbrance, and jump extent
            /// </summary>
            var strength = Strength.Current;
            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, AugmentationIncreasedCarryingCapacity);
            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);
            // calculate stamina cost for this jump
            var extent = Math.Clamp(jump.Extent, 0.0f, 1.0f);
            var staminaCost = MovementSystem.JumpStaminaCost(extent, burden, PKTimerActive);
            //Console.WriteLine($"Strength: {strength}, Capacity: {capacity}, Encumbrance: {EncumbranceVal ?? 0}, Burden: {burden}, StaminaCost: {staminaCost}");

            /// <summary>
            /// Deduct stamina cost (commented code handles insufficient stamina adjustment)
            /// </summary>
            // ensure player has enough stamina to jump
            /*if (staminaCost > Stamina.Current)
            {
                // get adjusted power
                extent = MovementSystem.GetJumpPower(Stamina.Current, burden, false);
                staminaCost = (int)Stamina.Current;
                // adjust jump velocity
                var velocityZ = MovementSystem.GetJumpHeight(burden, GetCreatureSkill(Skill.Jump).Current, extent, 1.0f);
                jump.Velocity.Z = velocityZ;
            }*/
            LastJumpTime = DateTime.UtcNow;
            UpdateVitalDelta(Stamina, -staminaCost);
            //Console.WriteLine($"Jump velocity: {jump.Velocity}");

            /// <summary>
            /// TODO: Server-side jump validation and magnitude scaling
            /// </summary>
            // TODO: have server verify / scale magnitude
            if (FastTick)
            {
                if (!PhysicsObj.IsMovingOrAnimating)
                    //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - Physics.PhysicsGlobals.MinQuantum;
                    PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;
                // perform jump in physics engine
                PhysicsObj.TransientState &= ~(TransientStateFlags.Contact | TransientStateFlags.WaterContact);
                PhysicsObj.calc_acceleration();
                PhysicsObj.set_on_walkable(false);
                PhysicsObj.set_local_velocity(jump.Velocity, false);
                PhysicsObj.RemoveLinkAnimations(); // matches MotionInterp.LeaveGround more closely
                PhysicsObj.MovementManager.MotionInterpreter.PendingMotions.Clear(); //hack
                PhysicsObj.IsAnimating = false;
                if (CombatMode == CombatMode.Magic && MagicState.IsCasting)
                {
                    // clear possible CastMotion out of InterpretedMotionState.ForwardCommand
                    PhysicsObj.MovementManager.MotionInterpreter.StopCompletely();
                    FailCast();
                }
            }
            else
            {
                PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;
                // set jump velocity
                //var glob_velocity = Vector3.Transform(jump.Velocity, Location.Rotation);
                //PhysicsObj.set_velocity(glob_velocity, true);
                // perform jump in physics engine
                PhysicsObj.TransientState &= ~(TransientStateFlags.Contact | TransientStateFlags.WaterContact);
                PhysicsObj.calc_acceleration();
                PhysicsObj.set_on_walkable(false);
                PhysicsObj.set_local_velocity(jump.Velocity, false);
                PhysicsObj.RemoveLinkAnimations(); // matches MotionInterp.LeaveGround more closely
                PhysicsObj.MovementManager.MotionInterpreter.PendingMotions.Clear(); //hack
                PhysicsObj.IsAnimating = false;
            }

            /// <summary>
            /// Send movement correction to prevent charged jump arc issues
            /// </summary>
            // this shouldn't be needed, but without sending this update motion / simulated movement event beforehand,
            // running forward and then performing a charged jump does an uncharged shallow arc jump instead
            // this hack fixes that...
            var movementData = new MovementData(this);
            movementData.IsAutonomous = true;
            movementData.MovementType = MovementType.Invalid;
            movementData.Invalid = new MovementInvalid(movementData);
            EnqueueBroadcast(new GameMessageUpdateMotion(this, movementData));

            /// <summary>
            /// Broadcast jump physics update to nearby players
            /// </summary>
            // broadcast jump
            EnqueueBroadcast(new GameMessageVectorUpdate(this));
            if (MagicState.IsCasting && RecordCast.Enabled)
                RecordCast.OnJump(jump);
        }

        /// <summary>
        /// Called when player's stamina reaches 0 (exhaustion state)
        /// </summary>
        /// <remarks>
        /// Updates movement speed and notifies player of exhaustion
        /// </remarks>
        public void OnExhausted()
        {
            // adjust player speed if they are currently pressing movement keys
            HandleRunRateUpdate();
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You're Exhausted!"));
        }

        /// <summary>
        /// Detects and broadcasts changes in player run rate (movement speed)
        /// </summary>
        /// <returns>True if run rate changed and was broadcast</returns>
        /// <remarks>
        /// Monitors forward, turn, and sidestep speed changes due to stamina, burden, etc.
        /// </remarks>
        public bool HandleRunRateUpdate()
        {
            //Console.WriteLine($"{Name}.HandleRunRateUpdates()");
            if (CurrentMovementData.MovementType != MovementType.Invalid || CurrentMovementData.Invalid == null)
                return false;

            /// <summary>
            /// Compare current state against previous movement state
            /// </summary>
            var prevState = CurrentMovementData.Invalid.State;
            var movementData = new MovementData(this, CurrentMoveToState);
            var currentState = movementData.Invalid.State;
            var changed = currentState.ForwardSpeed != prevState.ForwardSpeed ||
                          currentState.TurnSpeed != prevState.TurnSpeed ||
                          currentState.SidestepSpeed != prevState.SidestepSpeed;

            if (!changed)
                return false;
            //Console.WriteLine($"Old: {prevState.ForwardSpeed}, New: {currentState.ForwardSpeed}");

            if (!CurrentMovementData.Invalid.State.HasMovement() || IsJumping)
                return false;
            //Console.WriteLine($"{Name}.OnRunRateChanged()");
            CurrentMovementData = new MovementData(this, CurrentMoveToState);

            /// <summary>
            /// Server-initiated movement updates should be non-autonomous
            /// </summary>
            // verify - forced commands from server should be non-autonomous, but could have been sent as autonomous in retail?
            // if set to autonomous here, the desired effect doesn't happen
            CurrentMovementData.IsAutonomous = false;
            var movementEvent = new GameMessageUpdateMotion(this, CurrentMovementData);
            EnqueueBroadcast(movementEvent); // broadcast to all players, including self
            return true;
        }

        /// <summary>
        /// Calculates burden modifier for movement skills when encumbered
        /// </summary>
        /// <returns>Movement penalty multiplier (0.0-1.0)</returns>
        /// <remarks>
        /// Affects Run, Jump, Melee Defense, and Missile Defense skills
        /// </remarks>
        public override float GetBurdenMod()
        {
            var strength = Strength.Current;
            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, AugmentationIncreasedCarryingCapacity);
            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);
            var burdenMod = EncumbranceSystem.GetBurdenMod(burden);
            //Console.WriteLine($"Burden mod: {burdenMod}");
            return burdenMod;
        }

        /// <summary>
        /// Admin vision mode toggle (shows hidden/admin objects)
        /// </summary>
        public bool Adminvision;

        /// <summary>
        /// Toggles admin vision mode with various control options
        /// </summary>
        /// <param name="choice">-1=do nothing, 0=off, 1=on, 2=toggle</param>
        public void HandleAdminvisionToggle(int choice)
        {
            bool oldState = Adminvision;
            switch (choice)
            {
                case -1:
                    // Do nothing
                    break;
                case 0:
                    Adminvision = false;
                    break;
                case 1:
                    Adminvision = true;
                    break;
                case 2:
                    if (Adminvision)
                        Adminvision = false;
                    else
                        Adminvision = true;
                    break;
            }

            /// <summary>
            /// Update visibility of admin objects when enabling adminvision
            /// </summary>
            // send CO network messages for admin objects
            if (Adminvision && oldState != Adminvision)
            {
                var adminObjs = PhysicsObj.ObjMaint.GetKnownObjectsValuesWhere(o => o.WeenieObj.WorldObject != null && o.WeenieObj.WorldObject.Visibility);
                PhysicsObj.enqueue_objs(adminObjs);
                var nodrawObjs = PhysicsObj.ObjMaint.GetKnownObjectsValuesWhere(o => o.WeenieObj.WorldObject != null && ((o.WeenieObj.WorldObject.NoDraw ?? false) || o.WeenieObj.WorldObject.UiHidden));
                foreach (var wo in nodrawObjs)
                    Session.Network.EnqueueSend(new GameMessageUpdateObject(wo.WeenieObj.WorldObject, Adminvision, Adminvision ? true : false));
                // sending DO network messages for /adminvision off here doesn't work in client unfortunately?
            }
            string state = Adminvision ? "enabled" : "disabled";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Admin Vision is {state}.", ChatMessageType.Broadcast));
            if (oldState != Adminvision && !Adminvision)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Note that you will need to log out and back in before the visible items become invisible again.", ChatMessageType.Broadcast));
            }
        }

        /// <summary>
        /// Sends system message to player (filtered by squelch settings)
        /// </summary>
        /// <param name="msg">Message text</param>
        /// <param name="type">Chat message type/category</param>
        /// <param name="source">Source object for squelch filtering</param>
        public void SendMessage(string msg, ChatMessageType type = ChatMessageType.Broadcast, WorldObject source = null)
        {
            if (SquelchManager.IsLegalChannel(type) && SquelchManager.Squelches.Contains(source, type))
                return;
            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, type));
        }

        /// <summary>
        /// Handles player entering PK Lite status
        /// </summary>
        public void HandleActionEnterPkLite()
        {
            /// <summary>
            /// Validate NPK status and timer restrictions
            /// </summary>
            // ensure permanent npk
            if (PlayerKillerStatus != PlayerKillerStatus.NPK || MinimumTimeSincePk != null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyNonPKsMayEnterPKLite));
                return;
            }
            if (IsBusy || Teleporting || suicideInProgress)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            /// <summary>
            /// Calculate animation timing for combat mode change
            /// </summary>
            var animTime = 0.0f;
            if (CombatMode != CombatMode.NonCombat)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat));
                animTime += SetCombatMode(CombatMode.NonCombat);
            }

            /// <summary>
            /// Create action chain for PK Lite entry sequence
            /// </summary>
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);
            actionChain.AddAction(this, () =>
            {
                IsBusy = true;
                EnqueueBroadcast(new GameMessageSystemChat($"{Name} is looking for a fight!", ChatMessageType.Broadcast), LocalBroadcastRange);
                // perform pk lite entry motion / effect
                SendMotionAsCommands(MotionCommand.EnterPKLite, MotionStance.NonCombat);
                var innerChain = new ActionChain();
                // wait for animation to complete
                animTime = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.EnterPKLite);
                innerChain.AddDelaySeconds(animTime);
                innerChain.AddAction(this, () =>
                {
                    IsBusy = false;
                    if (PropertyManager.GetBool("allow_pkl_bump").Item)
                    {
                        // check for collisions
                        PlayerKillerStatus = PlayerKillerStatus.PKLite;
                        var colliding = PhysicsObj.ethereal_check_for_collisions();
                        if (colliding)
                        {
                            // try initial placement
                            var result = PhysicsObj.SetPositionSimple(PhysicsObj.Position, true);
                            if (result == SetPositionError.OK)
                            {
                                // handle landblock update?
                                SyncLocation();
                                // force broadcast
                                Sequences.GetNextSequence(SequenceType.ObjectForcePosition);
                                SendUpdatePosition();
                            }
                        }
                    }
                    UpdateProperty(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.PKLite, true);
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNowPKLite));
                });
                innerChain.EnqueueChain();
            });
            actionChain.EnqueueChain();
        }
    }
}
