using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Physics;

using Position = ACE.Entity.Position;

namespace ACE.Server.WorldObjects
{
    public partial class Player : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Session Session { get; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Player(Weenie weenie, ObjectGuid guid, Session session) : base(weenie, guid)
        {
            Session = session;

            // Make sure properties this WorldObject requires are not null.
            AvailableExperience = AvailableExperience ?? 0;
            TotalExperience = TotalExperience ?? 0;

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Player(Biota biota, IEnumerable<Biota> inventory, IEnumerable<Biota> wieldedItems, Session session) : base(biota)
        {
            SortBiotasIntoInventory(inventory);
            AddBiotasToEquippedObjects(wieldedItems);

            Session = session;

            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Player;

            IgnoreCollisions = true; ReportCollisions = false; Hidden = true;

            // This is the default send upon log in and the most common. Anything with a velocity will need to add that flag.
            PositionFlag |= UpdatePositionFlag.ZeroQx | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.Contact | UpdatePositionFlag.Placement;

            CurrentMotionState = new UniversalMotion(MotionStance.Standing);

            // radius for object updates
            ListeningRadius = 5f;

            if (Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    IsArch = true;
                if (Session.AccessLevel == AccessLevel.Envoy)
                    IsEnvoy = true;
                // TODO: Need to setup and account properly for IsSentinel and IsAdvocate.
                // if (Session.AccessLevel == AccessLevel.Sentinel)
                //    character.IsSentinel = true;
                // if (Session.AccessLevel == AccessLevel.Advocate)
                //    character.IsAdvocate= true;
            }

            ContainerCapacity = 7;

            if ((AdvocateQuest ?? false) && IsAdvocate) // Advocate permissions are per character regardless of override
            {
                if (Session.AccessLevel == AccessLevel.Player)
                    Session.SetAccessLevel(AccessLevel.Advocate); // Elevate to Advocate permissions
                if (AdvocateLevel > 4)
                    IsPsr = true; // Enable AdvocateTeleport via MapClick
            }

            // Start vital ticking, if they need it
            if (Health.Current != Health.MaxValue)
                VitalTickInternal(Health);

            if (Stamina.Current != Stamina.MaxValue)
                VitalTickInternal(Stamina);

            if (Mana.Current != Mana.MaxValue)
                VitalTickInternal(Mana);

            IsOnline = true;

            return; // todo
            /* todo fix for new EF model
            TrackedContracts = new Dictionary<uint, ContractTracker>();
            // Load the persisted tracked contracts into the working dictionary on player object.
            foreach (var trackedContract in AceObject.TrackedContracts)
            {
                ContractTracker loadContract = new ContractTracker(trackedContract.Value.ContractId, Guid.Full)
                {
                    DeleteContract = trackedContract.Value.DeleteContract,
                    SetAsDisplayContract = trackedContract.Value.SetAsDisplayContract,
                    Stage = trackedContract.Value.Stage,
                    TimeWhenDone = trackedContract.Value.TimeWhenDone,
                    TimeWhenRepeats = trackedContract.Value.TimeWhenRepeats
                };

                TrackedContracts.Add(trackedContract.Key, loadContract);
            }*/

            LastUseTracker = new Dictionary<int, DateTime>();

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
                //    character.IsSentinel = true;
                // if (Session.AccessLevel == AccessLevel.Advocate)
                //    character.IsAdvocate= true;
            }*/

            FirstEnterWorldDone = false;

            IsAlive = true;
        }


        public override void HeartBeat()
        {
            // Do Stuff

            

            QueueNextHeartBeat();
        }




















        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        /// <summary>
        /// Enum used for the DoEatOrDrink() method
        /// </summary>
        public enum ConsumableBuffType : uint
        {
            Spell = 0,
            Health = 2,
            Stamina = 4,
            Mana = 6
        }

        /// <summary>
        /// This tracks the contract tracker objects
        /// </summary>
        public Dictionary<uint, ContractTracker> TrackedContracts { get; set; }


        public void CompleteConfirmation(ConfirmationType confirmationType, uint contextId)
        {
            Session.Network.EnqueueSend(new GameEventConfirmationDone(Session, confirmationType, contextId));
        }

        //[Obsolete]
        //private AceCharacter Character => AceObject as AceCharacter;






        //public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions => CharacterOptions;

        //public ReadOnlyCollection<Friend> Friends => Friends;
        public ReadOnlyCollection<Friend> Friends { get; set; }


        public bool FirstEnterWorldDone = false;



        private MotionStance stance = MotionStance.Standing;





        




        public override void DoOnKill(Session killerSession)
        {
            // First do on-kill
            OnKill(killerSession);
            // Then get onKill from our parent
            ActionChain killChain = base.OnKillInternal(killerSession);

            // Send the teleport out after we animate death
            killChain.AddAction(this, () =>
            {
                // teleport to sanctuary or best location
                Position newPosition = PositionSanctuary ?? PositionLastPortal ?? Location;

                // Enqueue a teleport action, followed by Stand-up
                // Queue the teleport to lifestone
                ActionChain teleportChain = GetTeleportChain(newPosition);

                teleportChain.AddAction(this, () =>
                {
                    // Regenerate/ressurect?
                    UpdateVitalInternal(Health, 5);

                    // Stand back up
                    DoMotion(new UniversalMotion(MotionStance.Standing));

                    // add a Corpse at the current location via the ActionQueue to honor the motion and teleport delays
                    // QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
                    // AddToActionQueue(addCorpse);
                    // If the player is outside of the landblock we just died in, then reboadcast the death for
                    // the players at the lifestone.
                    if (Positions.ContainsKey(PositionType.LastOutsideDeath) && Positions[PositionType.LastOutsideDeath].Cell != newPosition.Cell)
                    {
                        string currentDeathMessage = $"died to {killerSession.Player.Name}.";
                        ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerSession.Player.Guid);
                    }
                });
                teleportChain.EnqueueChain();
            });
            killChain.EnqueueChain();
        }

        /// <summary>
        /// Player Death/Kill, use this to kill a session's player
        /// </summary>
        /// <remarks>
        ///     TODO:
        ///         1. Find the best vitae formula and add vitae
        ///         2. Generate the correct death message, or have it passed in as a parameter.
        ///         3. Find the correct player death noise based on the player model and play on death.
        ///         4. Determine if we need to Send Queued Action for Lifestone Materialize, after Action Location.
        ///         5. Find the health after death formula and Set the correct health
        /// </remarks>
        private void OnKill(Session killerSession)
        {
            ObjectGuid killerId = killerSession.Player.Guid;

            IsAlive = false;
            Health.Current = 0; // Set the health to zero
            NumDeaths++; // Increase the NumDeaths counter
            DeathLevel++; // Increase the DeathLevel

            // TODO: Find correct vitae formula/value
            VitaeCpPool = 0; // Set vitae

            // TODO: Generate a death message based on the damage type to pass in to each death message:
            string currentDeathMessage = $"died to {killerSession.Player.Name}.";

            // Send Vicitim Notification, or "Your Death" event to the client:
            // create and send the client death event, GameEventYourDeath
            var msgYourDeath = new GameEventYourDeath(Session, $"You have {currentDeathMessage}");
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, Health.Current);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.NumDeaths, NumDeaths.Value);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.DeathLevel, DeathLevel.Value);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value);
            var msgPurgeEnchantments = new GameEventPurgeAllEnchantments(Session);
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);

            // Send first death message group
            Session.Network.EnqueueSend(msgHealthUpdate, msgYourDeath, msgNumDeaths, msgDeathLevel, msgVitaeCpPool, msgPurgeEnchantments);

            // Broadcast the 019E: Player Killed GameMessage
            ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);
        }

        public void ExamineObject(ObjectGuid examinationId)
        {
            // TODO: Throttle this request?. The live servers did this, likely for a very good reason, so we should, too.

            if (examinationId.Full == 0)
            {
                // Deselect the formerly selected Target
                // selectedTarget = ObjectGuid.Invalid;
                RequestedAppraisalTarget = null;
                CurrentAppraisalTarget = null;
                return;
            }

            // The object can be in two spots... on the player or on the landblock
            // First check the player
            // search packs
            WorldObject wo = GetInventoryItem(examinationId);

            // search wielded items
            if (wo == null)
                wo = GetWieldedItem(examinationId);

            // search interactive objects
            if (wo == null)
            {
                if (interactiveWorldObjects.ContainsKey(examinationId))
                    wo = interactiveWorldObjects[examinationId];
            }

            // if its local examine it
            if (wo != null)
            {
                wo.Examine(Session);
            }
            else
            {
                // examine item on land block
                CurrentLandblock.GetObject(examinationId).Examine(Session);
            }

            RequestedAppraisalTarget = examinationId.Full;
            CurrentAppraisalTarget = examinationId.Full;
        }

        public void QueryHealth(ObjectGuid queryId)
        {
            if (queryId.Full == 0)
            {
                // Deselect the formerly selected Target
                selectedTarget = ObjectGuid.Invalid;
                HealthQueryTarget = null;
                return;
            }

            // Remember the selected Target
            selectedTarget = queryId;
            HealthQueryTarget = queryId.Full;
            CurrentLandblock.GetObject(queryId).QueryHealth(Session);
        }

        public void QueryItemMana(ObjectGuid queryId)
        {
            if (queryId.Full == 0)
            {
                ManaQueryTarget = null;
                return;
            }

            // the object could be in the world or on the player, first check player
            WorldObject wo = GetInventoryItem(queryId);
            if (wo != null)
            {
                wo.QueryItemMana(Session);
            }
            else
            {
                // We could be wielded - let's check that next.
                if (EquippedObjects.TryGetValue(queryId, out wo))
                    wo.QueryItemMana(Session);
            }

            ManaQueryTarget = queryId.Full;
        }

        public void ReadBookPage(ObjectGuid bookId, uint pageNum)
        {
            // TODO: Do we want to throttle this request, like appraisals?
            // The object can be in two spots... on the player or on the landblock
            // First check the player
            WorldObject wo = GetInventoryItem(bookId);
            // book is in the player's inventory...
            if (wo != null)
            {
                wo.ReadBookPage(Session, pageNum);
            }
            else
            {
                CurrentLandblock.GetObject(bookId).ReadBookPage(Session, pageNum);
            }
        }

 
        /// <summary>
        /// Sends a death message broadcast all players on the landblock? that a killer has a victim
        /// </summary>
        /// <remarks>
        /// TODO:
        ///     1. Figure out who all receieves death messages, on what landblock and at what order -
        ///         Example: Does the players around the vicitm receive the message or do the players at the lifestone receieve the message, or both?
        /// </remarks>
        /// <param name="deathMessage"></param>
        /// <param name="victimId"></param>
        public void ActionBroadcastKill(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
        {
            var deathBroadcast = new GameMessagePlayerKilled(deathMessage, victimId, killerId);
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.OutdoorChatRange, deathBroadcast);
        }

        // Play a sound
        public void PlaySound(Sound sound, ObjectGuid targetId)
        {
            Session.Network.EnqueueSend(new GameMessageSound(targetId, sound, 1f));
        }

   


        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public void AddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
                ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);

            // Check if friend exists
            if (Friends.SingleOrDefault(f => string.Equals(f.Name, friendName, StringComparison.CurrentCultureIgnoreCase)) != null)
                ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);

            // TODO: check if player is online first to avoid database hit??
            // Get character record from DB
            throw new System.NotImplementedException();
            /* TODO fix for new EF model
            DatabaseManager.Shard.GetObjectInfoByName(friendName, ((ObjectInfo friendInfo) =>
            {
                if (friendInfo == null)
                {
                    ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                    return;
                }

                Friend newFriend = new Friend();
                newFriend.Name = friendInfo.Name;
                newFriend.Id = new ObjectGuid(friendInfo.Guid);

                // Save to DB, assume success
                DatabaseManager.Shard.AddFriend(Guid.Low, newFriend.Id.Low, (() =>
                {
                    // Add to character object
                    Character.AddFriend(newFriend);

                    // Send packet
                    Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));
                }));
            }));*/
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendId">The ObjectGuid of the friend that is being removed</param>
        public void RemoveFriend(ObjectGuid friendId)
        {
            Friend friendToRemove = Friends.SingleOrDefault(f => f.Id.Low == friendId.Low);

            // Not in friend list
            if (friendToRemove == null)
            {
                ChatPacket.SendServerMessage(Session, "That character is not in your friends list!", ChatMessageType.Broadcast);
                return;
            }

            // Remove from DB
            DatabaseManager.Shard.DeleteFriend(Guid.Low, friendId.Low, (() =>
            {
                // Remove from character object
                RemoveFriend(friendId);

                // Send packet
                Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));
            }));
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public void RemoveAllFriends()
        {
            // Remove all from DB
            DatabaseManager.Shard.RemoveAllFriends(Guid.Low, null);

            // Remove from character object
            RemoveAllFriends();
        }

        /// <summary>
        /// Set the AppearOffline option to the provided value.  It will also send out an update to all online clients that have this player as a friend. This option does not save to the database.
        /// </summary>
        public void AppearOffline(bool appearOffline)
        {
            SetCharacterOption(CharacterOption.AppearOffline, appearOffline);
            SendFriendStatusUpdates();
        }


        /// <summary>
        /// Set the currently position of the character, to later save in the database.
        /// </summary>
        public void SetPhysicalCharacterPosition()
        {
            // Saves the current player position after converting from a Position Object, to a CharacterPosition object
            SetCharacterPosition(PositionType.Location, Session.Player.Location);
        }

        /// <summary>
        /// Saves a CharacterPosition to the character position dictionary
        /// </summary>
        public void SetCharacterPosition(PositionType type, Position newPosition)
        {
            // reset the landblock id
            if (newPosition.LandblockId.Landblock == 0 && newPosition.Cell > 0)
            {
                newPosition.LandblockId = new LandblockId(newPosition.Cell);
            }

            Positions[type] = newPosition;
        }

        public void UpdateAge()
        {
            if (Age != null)
                Age++;
            else
                Age = 1;
        }

        public void SendAgeInt()
        {
            try
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Age, Age ?? 1));
            }
            catch (NullReferenceException)
            {
                // Do Nothing since player data hasn't loaded in
            }
        }

        /// <summary>
        /// Returns false if the player has chosen to Appear Offline.  Otherwise it will return their actual online status.
        /// </summary>
        public bool GetVirtualOnlineStatus()
        {
            //if (CharacterOptions[CharacterOption.AppearOffline])
                return false;

            return IsOnline;
        }




        public void HandleActionLogout(bool clientSessionTerminatedAbruptly = false)
        {
            GetLogoutChain().EnqueueChain();
        }

        public ActionChain GetLogoutChain(bool clientSessionTerminatedAbruptly = false)
        {
            ActionChain logoutChain = new ActionChain(this, () => LogoutInternal(clientSessionTerminatedAbruptly));

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
            float logoutAnimationLength = motionTable.GetAnimationLength(MotionCommand.LogOut);
            logoutChain.AddDelaySeconds(logoutAnimationLength);

            if (CurrentLandblock != null)
            {
                // remove the player from landblock management -- after the animation has run
                logoutChain.AddChain(CurrentLandblock.GetRemoveWorldObjectChain(Guid, false));
            }

            return logoutChain;
        }

        /// <summary>
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        private void LogoutInternal(bool clientSessionTerminatedAbruptly)
        {
            if (Fellowship != null)
            {
                FellowshipQuit(false);
            }

            if (!IsOnline)
                return;

            InWorld = false;
            IsOnline = false;

            SendFriendStatusUpdates();

            if (!clientSessionTerminatedAbruptly)
            {
                var logout = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LogOut));
                CurrentLandblock.EnqueueBroadcastMotion(this, logout);

                EnqueueBroadcastPhysicsState();

                // Thie retail server sends a ChatRoomTracker 0x0295 first, then the status message, 0x028B. It does them one at a time for each individual channel.
                // The ChatRoomTracker message doesn't seem to change at all.
                // For the purpose of ACE, we simplify this process.
                var general = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "General");
                var trade = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "Trade");
                var lfg = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "LFG");
                var roleplay = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "Roleplay");
                Session.Network.EnqueueSend(general, trade, lfg, roleplay);
            }
        }

        public void HandleMRT()
        {
            ActionChain mrtChain = new ActionChain();

            // Handle MRT Toggle internal must decide what to do next...
            mrtChain.AddAction(this, new ActionEventDelegate(() => HandleMRTToggleInternal()));

            mrtChain.EnqueueChain();
        }

        private void HandleMRTToggleInternal()
        {
            // This requires the Admin flag set on ObjectDescriptionFlags
            // I would expect this flag to be set in Admin.cs which would be a subclass of Player
            // FIXME: maybe move to Admin class?
            // TODO: reevaluate class location

            if (!IgnoreHouseBarriers ?? false)
                SetProperty(PropertyBool.IgnoreHouseBarriers, true);
            else
                SetProperty(PropertyBool.IgnoreHouseBarriers, false);

            // The EnqueueBroadcastUpdateObject below sends the player back into teleport. I assume at this point, this was never done to players
            // EnqueueBroadcastUpdateObject();

            // The private message below worked as expected, but it only broadcast to the player. This would be a problem with for others in range seeing something try to
            // pass through a barrier but not being allowed.
            // var updateBool = new GameMessagePrivateUpdatePropertyBool(Session, PropertyBool.IgnoreHouseBarriers, ImmuneCellRestrictions);
            // Session.Network.EnqueueSend(updateBool);

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this, PropertyBool.IgnoreHouseBarriers, IgnoreHouseBarriers ?? false));

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Bypass Housing Barriers now set to: {IgnoreHouseBarriers}", ChatMessageType.Broadcast));
        }

        public void SendAutonomousPosition()
        {
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }

 

        public void HandleActionFinishBarber(ClientMessage message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoFinishBarber(message));
            chain.EnqueueChain();
        }

        public void DoFinishBarber(ClientMessage message)
        {
            // Read the payload sent from the client...
            PaletteBaseId = message.Payload.ReadUInt32();
            HeadObject = message.Payload.ReadUInt32();
            HairTexture = message.Payload.ReadUInt32();
            DefaultHairTexture = message.Payload.ReadUInt32();
            EyesTexture = message.Payload.ReadUInt32();
            DefaultEyesTexture = message.Payload.ReadUInt32();
            NoseTexture = message.Payload.ReadUInt32();
            DefaultNoseTexture = message.Payload.ReadUInt32();
            MouthTexture = message.Payload.ReadUInt32();
            DefaultMouthTexture = message.Payload.ReadUInt32();
            SkinPalette = message.Payload.ReadUInt32();
            HairPalette = message.Payload.ReadUInt32();
            EyesPalette = message.Payload.ReadUInt32();
            SetupTableId = message.Payload.ReadUInt32();

            uint option_bound = message.Payload.ReadUInt32(); // Supress Levitation - Empyrean Only
            uint option_unk = message.Payload.ReadUInt32(); // Unknown - Possibly set aside for future use?

            // Check if Character is Empyrean, and if we need to set/change/send new motion table
            if (Heritage == 9)
            {
                // These are the motion tables for Empyrean float and not-float (one for each gender). They are hard-coded into the client.
                const uint EmpyreanMaleFloatMotionDID = 0x0900020Bu;
                const uint EmpyreanFemaleFloatMotionDID = 0x0900020Au;
                const uint EmpyreanMaleMotionDID = 0x0900020Eu;
                const uint EmpyreanFemaleMotionDID = 0x0900020Du;

                // Check for the Levitation option for Empyrean. Shadow crown and Undead flames are handled by client.
                if (Gender == 1) // Male
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanMaleMotionDID)
                    {
                        MotionTableId = EmpyreanMaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanMaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanMaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                }
                else // Female
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanFemaleMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanFemaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                }
            }


            // Broadcast updated character appearance
            CurrentLandblock.EnqueueBroadcast(
                Location,
                Landblock.MaxObjectRange,
                new GameMessageObjDescEvent(this));
        }

        /// <summary>
        ///  Sends object description if the client requests it
        /// </summary>
        /// <param name="item"></param>
        public void HandleActionForceObjDescSend(ObjectGuid item)
        {
            ActionChain objDescChain = new ActionChain();
            objDescChain.AddAction(this, () =>
            {
                WorldObject wo = GetInventoryItem(item);
                if (wo != null)
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageObjDescEvent(wo));
                else
                    log.Debug($"Error - requested object description for an item I do not know about - {item.Full:X}");
            });
            objDescChain.EnqueueChain();
        }

        protected override void SendUpdatePosition()
        {
            
            GameMessage msg = new GameMessageUpdatePosition(this);
            Session.Network.EnqueueSend(msg);
            base.SendUpdatePosition();
        }

 

        /// <summary>
        /// This method is part of the contract tracking functions.   This is used to remove or abandon a contract.
        /// The method validates the id passed from the client against the portal.dat file, then sends the appropriate
        /// response to the client to remove the item from the quest panel. Og II
        /// </summary>
        /// <param name="contractId">This is the contract id passed to us from the client that we want to remove.</param>
        public void AbandonContract(uint contractId)
        {
            ContractTracker contractTracker = new ContractTracker(contractId, Guid.Full)
            {
                Stage = 0,
                TimeWhenDone = 0,
                TimeWhenRepeats = 0,
                DeleteContract = 1,
                SetAsDisplayContract = 0
            };

            GameEventSendClientContractTracker contractMsg = new GameEventSendClientContractTracker(Session, contractTracker);
            /* todo fix for new EF model
            AceContractTracker contract = new AceContractTracker();
            if (TrackedContracts.ContainsKey(contractId))
                contract = TrackedContracts[contractId].SnapShotOfAceContractTracker();

            TrackedContracts.Remove(contractId);
            LastUseTracker.Remove((int)contractId);
            AceObject.TrackedContracts.Remove(contractId);

            DatabaseManager.Shard.DeleteContract(contract, deleteSuccess =>
            {
                if (deleteSuccess)
                    log.Info($"ContractId {contractId:X} successfully deleted");
                else
                    log.Error($"Unable to delete contractId {contractId:X} ");
            });

            Session.Network.EnqueueSend(contractMsg);*/
        }



        public void HandleActionApplySoundEffect(Sound sound)
        {
            new ActionChain(this, () => PlaySound(sound, Guid)).EnqueueChain();
        }

        public void HandleActionSmiteAllNearby()
        {
            // Create smite action chain... then send it
            new ActionChain(this, () =>
            {
                if (CurrentLandblock == null)
                {
                    return;
                }

                foreach (ObjectGuid toSmite in GetKnownCreatures())
                {
                    Creature smitee = CurrentLandblock.GetObject(toSmite) as Creature;
                    if (smitee != null)
                    {
                        smitee.DoOnKill(Session);
                    }
                }
            }).EnqueueChain();
        }

        public void HandleActionSmiteSelected()
        {
            new ActionChain(this, () =>
            {
                if (selectedTarget != ObjectGuid.Invalid)
                {
                    var target = selectedTarget;
                    throw new NotImplementedException(); // We can't use the GUID to see if this is a creature, we need another way
                    /*if (target.IsCreature() || target.IsPlayer())
                    {
                        HandleActionKill(target);
                    }*/
                }
                else
                {
                    ChatPacket.SendServerMessage(Session, "No target selected, use @smite all to kill all creatures in radar range.", ChatMessageType.Broadcast);
                }
            }).EnqueueChain();
        }

        //public void TestWieldItem(Session session, uint modelId, int palOption, float shade = 0)
        //{
        //    // ClothingTable item = ClothingTable.ReadFromDat(0x1000002C); // Olthoi Helm
        //    // ClothingTable item = ClothingTable.ReadFromDat(0x10000867); // Cloak
        //    // ClothingTable item = ClothingTable.ReadFromDat(0x10000008); // Gloves
        //    // ClothingTable item = ClothingTable.ReadFromDat(0x100000AD); // Heaume
        //    var item = DatManager.PortalDat.ReadFromDat<ClothingTable>(modelId);

        //    int palCount = 0;

        //    List<uint> coverage = new List<uint>(); // we'll store our fake coverage items here
        //    ClearObjDesc();
        //    AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette

        //    if (item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
        //    {
        //        // Add the model and texture(s)
        //        ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
        //        for (int i = 0; i < clothingBaseEffec.CloObjectEffects.Count; i++)
        //        {
        //            byte partNum = (byte)clothingBaseEffec.CloObjectEffects[i].Index;
        //            AddModel((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].ModelId);
        //            coverage.Add(partNum);
        //            for (int j = 0; j < clothingBaseEffec.CloObjectEffects[i].CloTextureEffects.Count; j++)
        //                AddTexture((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
        //        }

        //        // Apply an appropriate palette. We'll just pick a random one if not specificed--it's a surprise every time!
        //        // For actual equipment, these should just be stored in the ace_object palette_change table and loaded from there
        //        if (item.ClothingSubPalEffects.Count > 0)
        //        {
        //            int size = item.ClothingSubPalEffects.Count;
        //            palCount = size;

        //            CloSubPalEffect itemSubPal;
        //            // Generate a random index if one isn't provided
        //            if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
        //            {
        //                itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
        //            }
        //            else
        //            {
        //                List<CloSubPalEffect> values = item.ClothingSubPalEffects.Values.ToList();
        //                Random rand = new Random();
        //                palOption = rand.Next(size);
        //                itemSubPal = values[palOption];
        //            }

        //            for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
        //            {
        //                var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
        //                ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

        //                for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
        //                {
        //                    uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
        //                    uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
        //                    AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
        //                }
        //            }
        //        }

        //        // Add the "naked" body parts. These are the ones not already covered.
        //        var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>((uint)SetupTableId);
        //        for (byte i = 0; i < baseSetup.Parts.Count; i++)
        //        {
        //            if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head.
        //                AddModel(i, baseSetup.Parts[i]);
        //        }

        //        var objDescEvent = new GameMessageObjDescEvent(this);
        //        session.Network.EnqueueSend(objDescEvent);
        //        ChatPacket.SendServerMessage(session, "Equipping model " + modelId.ToString("X") +
        //                                              ", Applying palette index " + palOption + " of " + palCount +
        //                                              " with a shade value of " + shade + ".", ChatMessageType.Broadcast);
        //    }
        //    else
        //    {
        //        // Alert about the failure
        //        ChatPacket.SendServerMessage(session, "Could not match that item to your character model.", ChatMessageType.Broadcast);
        //    }
        //}

        public void HandleActionTestCorpseDrop()
        {
            new ActionChain(this, () =>
            {
                if (selectedTarget != ObjectGuid.Invalid)
                {
                    // FIXME(ddevec): This is wrong
                    var target = selectedTarget;
                    throw new NotImplementedException(); // We can't use the GUID to see if this is a creature, we need another way
                    /*if (target.IsCreature())
                    {
                        HandleActionKill(target);
                    }*/
                }
                else
                {
                    ChatPacket.SendServerMessage(Session, "No creature selected.", ChatMessageType.Broadcast);
                }
            }).EnqueueChain();
        }

        public void HandleActionKill(ObjectGuid toSmite)
        {
            // Create smite action chain... then send it
            new ActionChain(this, () =>
            {
                Creature c = CurrentLandblock.GetObject(toSmite) as Creature;
                if (c != null)
                {
                    c.DoOnKill(Session);
                }
            }).EnqueueChain();
        }


        public void HandleActionTalk(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoTalk(message));
            chain.EnqueueChain();
        }

        public void DoTalk(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChat(this, message);
        }

        public void HandleActionEmote(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoEmote(message));
            chain.EnqueueChain();
        }

        public void DoEmote(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChatEmote(this, message);
        }

        public void HandleActionSoulEmote(string message)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoSoulEmote(message));
            chain.EnqueueChain();
        }

        public void DoSoulEmote(string message)
        {
            CurrentLandblock.EnqueueBroadcastLocalChatSoulEmote(this, message);
        }

        /// <summary>
        /// Method used to perform the animation, sound, and vital update on consumption of food or potions
        /// </summary>
        /// <param name="consumableName">Name of the consumable</param>
        /// <param name="sound">Either Sound.Eat1 or Sound.Drink1</param>
        /// <param name="buffType">ConsumableBuffType.Spell,ConsumableBuffType.Health,ConsumableBuffType.Stamina,ConsumableBuffType.Mana</param>
        /// <param name="boostAmount">Amount the Vital is boosted by; can be null, if buffType = ConsumableBuffType.Spell</param>
        /// <param name="spellDID">Id of the spell cast by the consumable; can be null, if buffType != ConsumableBuffType.Spell</param>
        public void ApplyComsumable(string consumableName, Sound sound, ConsumableBuffType buffType, uint? boostAmount, uint? spellDID)
        {
            GameMessageSystemChat buffMessage;
            MotionCommand motionCommand;

            if (sound == Sound.Eat1)
                motionCommand = MotionCommand.Eat;
            else
                motionCommand = MotionCommand.Drink;

            var soundEvent = new GameMessageSound(Guid, sound, 1.0f);
            var motion = new UniversalMotion(MotionStance.Standing, new MotionItem(motionCommand));

            DoMotion(motion);

            if (buffType == ConsumableBuffType.Spell)
            {
                // Null check for safety
                if (spellDID == null)
                    spellDID = 0;

                // TODO: Handle spell cast
                buffMessage = new GameMessageSystemChat($"Consuming {consumableName} not yet fully implemented.", ChatMessageType.System);
            }
            else
            {
                CreatureVital creatureVital;
                string vitalName;

                // Null check for safety
                if (boostAmount == null)
                    boostAmount = 0;

                switch (buffType)
                {
                    case ConsumableBuffType.Health:
                        creatureVital = Health;
                        vitalName = "Health";
                        break;
                    case ConsumableBuffType.Mana:
                        creatureVital = Mana;
                        vitalName = "Mana";
                        break;
                    default:
                        creatureVital = Stamina;
                        vitalName = "Stamina";
                        break;
                }

                uint updatedVitalAmount = creatureVital.Current + (uint)boostAmount;

                if (updatedVitalAmount > creatureVital.MaxValue)
                    updatedVitalAmount = creatureVital.MaxValue;

                boostAmount = updatedVitalAmount - creatureVital.Current;

                UpdateVital(creatureVital, updatedVitalAmount);

                buffMessage = new GameMessageSystemChat($"You regain {boostAmount} {vitalName}.", ChatMessageType.Craft);
            }

            Session.Network.EnqueueSend(soundEvent, buffMessage);

            // Wait for animation
            var motionChain = new ActionChain();
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
            var motionAnimationLength = motionTable.GetAnimationLength(MotionCommand.Eat);
            motionChain.AddDelaySeconds(motionAnimationLength);

            // Return to standing position after the animation delay
            motionChain.AddAction(this, () => DoMotion(new UniversalMotion(MotionStance.Standing)));
            motionChain.EnqueueChain();
        }

        public bool Adminvision;

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

            if (Adminvision)
                CurrentLandblock.ResendObjectsInRange(this);

            string state = Adminvision ? "enabled" : "disabled";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Admin Vision is {state}.", ChatMessageType.Broadcast));

            if (oldState != Adminvision && !Adminvision)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Note that you will need to log out and back in before the visible items become invisible again.", ChatMessageType.Broadcast));
            }
        }
    }
}
