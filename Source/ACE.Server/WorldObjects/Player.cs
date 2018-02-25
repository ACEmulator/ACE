using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects.Entity;

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

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Player(Biota biota, Session session) : base(biota)
        {
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

            //AddCharacterBaseModelData();

            UpdateAppearance(this);

            return; // todo

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
            }

            LastUseTracker = new Dictionary<int, DateTime>();

            // =======================================
            // This code was taken from the old Load()
            // =======================================
            AceCharacter character;

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
            }

            FirstEnterWorldDone = false;

            IsAlive = true;
            IsOnline = true;            

            // Start vital ticking, if they need it
            if (Health.Current != Health.MaxValue)
                VitalTickInternal(Health);

            if (Stamina.Current != Stamina.MaxValue)
                VitalTickInternal(Stamina);

            if (Mana.Current != Mana.MaxValue)
                VitalTickInternal(Mana);

            ContainerCapacity = 7;

            ////Burden = UpdateBurden();
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

        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position MarketplaceDrop = new Position(23855548, 49.206f, -31.935f, 0.005f, 0f, 0f, -0.7071068f, 0.7071068f); // PCAP verified drop
        private static readonly float PickUpDistance = .75f;

        /// <summary>
        /// This will be false when in portal space
        /// </summary>
        public bool InWorld { get; set; }

        /// <summary>
        /// Different than InWorld which is false when in portal space
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// ObjectId of the currently selected Target (only players and creatures)
        /// </summary>
        private ObjectGuid selectedTarget = ObjectGuid.Invalid;

        /// <summary>
        /// Temp tracked Objects of vendors / trade / containers.. needed for id / maybe more.
        /// </summary>
        private readonly Dictionary<ObjectGuid, WorldObject> interactiveWorldObjects = new Dictionary<ObjectGuid, WorldObject>();

        /// <summary>
        /// This tracks the contract tracker objects
        /// </summary>
        public Dictionary<uint, ContractTracker> TrackedContracts { get; set; }

        /// <summary>
        /// This dictionary is used to keep track of the last use of any item that implemented shared cooldown.
        /// It is session specific.   I think (could be wrong) cooldowns reset if you logged out and back in.
        /// This is a different mechanic than quest repeat timers and rare item use timers.
        /// example - contacts have a shared cooldown key value 100 so each time a player uses an item that has
        /// a shared cooldown we just add to the dictionary 100, datetime.now()   The check becomes trivial at that
        /// point if on a subsequent use, now() minus the last use value from the dictionary
        /// is greater than or equal to the cooldown, we can do the use - if not you must wait message.   Og II
        /// </summary>
        public Dictionary<int, DateTime> LastUseTracker { get; set; }



        /// <summary>
        /// This code handle objects between players and other world objects
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="objectID"></param>
        /// <param name="amount"></param>
        public void HandleGiveObjectRequest(uint targetID, uint objectID, uint amount)
        {
            ////ObjectGuid target = new ObjectGuid(targetID);
            ////ObjectGuid item = new ObjectGuid(objectID);
            ////WorldObject targetObject = CurrentLandblock.GetObject(target) as WorldObject;
            ////WorldObject itemObject = GetInventoryItem(item);
            ////////WorldObject itemObject = CurrentLandblock.GetObject(item) as WorldObject;
            ////Session.Network.EnqueueSend(new GameMessagePutObjectInContainer(Session, (ObjectGuid)targetObject.Guid, itemObject, 0));
            ////SendUseDoneEvent();
        }



        public void CompleteConfirmation(ConfirmationType confirmationType, uint contextId)
        {
            Session.Network.EnqueueSend(new GameMessageConfirmationDone(this, confirmationType, contextId));
        }

        [Obsolete]
        private AceCharacter Character => AceObject as AceCharacter;


        /// <summary>
        /// FIXME(ddevec): This is the only object that need be locked in the player under the new model.
        ///   It must be locked because of how we handle object updates -- We can clean this up in the future
        /// </summary>
        private readonly Dictionary<ObjectGuid, double> clientObjectList = new Dictionary<ObjectGuid, double>();



        //public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions => CharacterOptions;

        //public ReadOnlyCollection<Friend> Friends => Friends;
        public ReadOnlyCollection<Friend> Friends { get; set; }

        /// <summary>
        ///  Gets a list of Tracked Objects.
        /// </summary>
        public List<ObjectGuid> GetTrackedObjectGuids()
        {
            lock (clientObjectList)
            {
                return clientObjectList.Select(x => x.Key).ToList();
            }
        }

        public bool FirstEnterWorldDone;


        public AceObject GetAceObject()
        {
            return Character;
        }

        private MotionStance stance = MotionStance.Standing;



        private void AddCharacterBaseModelData()
        {
            // Hair/head
            if (HeadObjectDID.HasValue)
                AddModel(0x10, HeadObjectDID.Value);
            if (DefaultHairTextureDID.HasValue && HairTextureDID.HasValue)
                AddTexture(0x10, DefaultHairTextureDID.Value, HairTextureDID.Value);
            if (HairPaletteDID.HasValue)
                AddPalette(HairPaletteDID.Value, 0x18, 0x8);

            // Skin
            // PaletteBaseId = PaletteBaseDID;
            if (SkinPalette.HasValue)
                AddPalette(SkinPalette.Value, 0x0, 0x18);

            // Eyes
            if (DefaultEyesTextureDID.HasValue && EyesTextureDID.HasValue)
                AddTexture(0x10, DefaultEyesTextureDID.Value, EyesTextureDID.Value);
            if (EyesPaletteDID.HasValue)
                AddPalette(EyesPaletteDID.Value, 0x20, 0x8);

            // Nose & Mouth
            if (NoseTextureDID.HasValue && NoseTextureDID.HasValue)
                AddTexture(0x10, NoseTextureDID.Value, NoseTextureDID.Value);
            if (DefaultMouthTextureDID.HasValue && MouthTextureDID.HasValue)
                AddTexture(0x10, DefaultMouthTextureDID.Value, MouthTextureDID.Value);
        }

        




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
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Health, Health.Current);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.NumDeaths, NumDeaths.Value);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.DeathLevel, DeathLevel.Value);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.VitaeCpPool, VitaeCpPool.Value);
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
        }

        public void QueryHealth(ObjectGuid queryId)
        {
            if (queryId.Full == 0)
            {
                // Deselect the formerly selected Target
                selectedTarget = ObjectGuid.Invalid;
                return;
            }

            // Remember the selected Target
            selectedTarget = queryId;
            CurrentLandblock.GetObject(queryId).QueryHealth(Session);
        }

        public void QueryItemMana(ObjectGuid queryId)
        {
            if (queryId.Full == 0)
            {
                // Do nothing if the queryID is 0
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
                if (WieldedObjects.TryGetValue(queryId, out wo))
                {
                    wo.QueryItemMana(Session);
                }
            }
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
        /// Call this to add any new World Objects to inventory
        /// </summary>
        public void HandleAddNewWorldObjectsToInventory(List<WorldObject> wolist)
        {
            foreach (WorldObject wo in wolist)
            {
                HandleAddNewWorldObjectToInventory(wo);
            }
        }

        /// <summary>
        /// Add New WorldObject to Inventory
        /// </summary>
        /// <param name="wo"></param>
        public void HandleAddNewWorldObjectToInventory(WorldObject wo)
        {
            // Get Next Avalibale Pack Location.
            // uint packid = GetCreatureInventoryFreePack();

            // default player until I get above code to work!
            uint packid = Guid.Full;

            if (packid != 0)
            {
                wo.ContainerId = (int)packid;
                AddToInventory(wo);
                Session.Network.EnqueueSend(new GameMessageCreateObject(wo));
                if (wo is Container container)
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, container));
            }
        }

        /// <summary>
        /// Adds a new object to the 's inventory of the specified weenie class.  intended use case: giving items to players
        /// while they are plaplayerying.  this calls all the necessary helper functions to have the item be tracked and sent to the client.
        /// </summary>
        /// <returns>the object created</returns>
        public WorldObject AddNewItemToInventory(uint weenieClassId)
        {
            var wo = Factories.WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            wo.ContainerId = (int)Guid.Full;
            wo.PlacementPosition = 0;
            AddToInventory(wo);
            TrackObject(wo);
            return wo;
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

        // Just preps the character to save
        public void HandleActionSaveCharacter()
        {
            GetSaveChain().EnqueueChain();
        }

        // Gets the ActionChain to save a character
        public ActionChain GetSaveChain()
        {
            return new ActionChain(this, SavePlayer);
        }

        ///// <summary>
        ///// This method is used to clear the wielded items list ( the list of ace objects used to save wielded items ) and loads it with a snapshot
        ///// of the aceObjects from the current list of wielded world objects. Og II
        ///// </summary>
        //public void SnapshotWieldedItems(bool clearDirtyFlags = false)
        //{
        //    WieldedItems.Clear();
        //    foreach (var wo in WieldedObjects)
        //    {
        //        WieldedItems.Add(wo.Value.Guid, wo.Value.SnapShotOfAceObject(clearDirtyFlags));
        //    }
        //}

        ///// <summary>
        ///// This method is used to clear the inventory lists of all containers. ( the list of ace objects used to save inventory items items ) and loads each with a snapshot
        ///// of the aceObjects from the current list of inventory world objects by container. Og II
        ///// </summary>
        //public void SnapshotInventoryItems(bool clearDirtyFlags = false)
        //{
        //    Inventory.Clear();
        //    foreach (var wo in InventoryObjects)
        //    {
        //        Inventory.Add(wo.Value.Guid, wo.Value.SnapShotOfAceObject(clearDirtyFlags));
        //        if (wo.Value.WeenieType == WeenieType.Container)
        //        {
        //            wo.Value.Inventory.Clear();
        //            throw new System.NotImplementedException();/*
        //            foreach (var item in wo.Value.InventoryObjects)
        //            {
        //                wo.Value.Inventory.Add(item.Value.Guid, item.Value.SnapShotOfAceObject(clearDirtyFlags));
        //            }*/
        //        }
        //    }
        //}

        //public void SnapShotTrackedContracts()
        //{
        //    foreach (var tc in TrackedContracts)
        //    {
        //        AceObject.SetTrackedContract(tc.Key, tc.Value.SnapShotOfAceContractTracker());
        //    }
        //}

        /// <summary>
        /// Internal save character functionality
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.
        /// </summary>
        private void SavePlayer()
        {
            // Save the current position to persistent storage, only during the server update interval
            SetPhysicalCharacterPosition();

            DatabaseManager.Shard.SaveBiota(Biota, null);

            #if DEBUG
            if (Session.Player != null)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved.", ChatMessageType.Broadcast));
            #endif
        }

        public void UpdateAge()
        {
            //if (Character != null)
                Age++;
        }

        public void SendAgeInt()
        {
            try
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.Age, Age.Value));
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





        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            var message = new GameMessageSystemChat($"Your title is now {title}!", ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(updateTitle, message);
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known creatures
        /// </summary>
        private List<ObjectGuid> GetKnownCreatures()
        {
            lock (clientObjectList)
            {
                throw new NotImplementedException(); // We can't use the GUID to see if this is a creature, we need another way
                //return clientObjectList.Select(x => x.Key).Where(o => o.IsCreature()).ToList();
            }
        }

        /// <summary>
        /// Tracks Interacive world object you are have interacted with recently.  this should be
        /// called from the context of an action chain being executed by the landblock loop.
        /// </summary>
        public void TrackInteractiveObjects(List<WorldObject> worldObjects)
        {
            // todo: figure out a way to expire objects.. objects clearly not in range of interaction /etc
            foreach (WorldObject wo in worldObjects)
            {
                if (interactiveWorldObjects.ContainsKey(wo.Guid))
                    interactiveWorldObjects[wo.Guid] = wo;
                else
                    interactiveWorldObjects.Add(wo.Guid, wo);
            }
        }

        /// <summary>
        /// forces either an update or a create object to be sent to the client
        /// </summary>
        public void TrackObject(WorldObject worldObject)
        {
            bool sendUpdate = true;

            if (worldObject.Guid == this.Guid)
                return;

            // If Visibility is true, do not send object to client, object is meant for server side only, unless Adminvision is true.
            if ((worldObject.Visibility ?? false) && !Adminvision)
                return;

            lock (clientObjectList)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                if (!sendUpdate)
                {
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                    worldObject.PlayScript(this.Session);
                }
                else
                {
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
                }
            }

            log.Debug($"Telling {Name} about {worldObject.Name} - {worldObject.Guid.Full:X}");

            if (sendUpdate)
            {
                // Session.Network.EnqueueSend(new GameMessageUpdateObject(worldObject));
                // TODO: Better handling of sending updates to client. The above line is causing much more problems then it is solving until we get proper movement.
                // Add this or something else back in when we handle movement better, until then, just send the create object once and move on.
            }
            else
            {
                Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));
            }
        }

        public void HandleActionLogout(bool clientSessionTerminatedAbruptly = false)
        {
            GetLogoutChain().EnqueueChain();
        }

        public ActionChain GetLogoutChain(bool clientSessionTerminatedAbruptly = false)
        {
            ActionChain logoutChain = new ActionChain(this, () => LogoutInternal(clientSessionTerminatedAbruptly));

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
            float logoutAnimationLength = MotionTable.GetAnimationLength(motionTable, MotionCommand.LogOut);
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
                var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "General");
                var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Trade");
                var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "LFG");
                var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveLeftThe_Channel, "Roleplay");
                Session.Network.EnqueueSend(general, trade, lfg, roleplay);
            }
        }

        public void StopTrackingObject(WorldObject worldObject, bool remove)
        {
            bool sendUpdate = true;
            lock (clientObjectList)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);
                if (sendUpdate)
                {
                    clientObjectList.Remove(worldObject.Guid);
                }
            }

            // Don't remove it if it went into our inventory...
            if (sendUpdate && remove)
            {
                Session.Network.EnqueueSend(new GameMessageRemoveObject(worldObject));
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

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageUpdatePropertyBool(this, PropertyBool.IgnoreHouseBarriers, IgnoreHouseBarriers ?? false));

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
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanMaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanMaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                }
                else // Female
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanFemaleMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanFemaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(Session, PropertyDataId.MotionTable, (uint)MotionTableId));
                    }
                }
            }

            UpdateAppearance(this);

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
            base.SendUpdatePosition();
            GameMessage msg = new GameMessageUpdatePosition(this);
            Session.Network.EnqueueSend(msg);
        }

        /// <summary>
        /// This method removes an item from Inventory and adds it to wielded items.
        /// It also clears all properties used when an object is contained and sets the needed properties to be wielded Og II
        /// </summary>
        /// <param name="item">The world object we are wielding</param>
        /// <param name="wielder">Who is wielding the item</param>
        /// <param name="currentWieldedLocation">What wield location are we going into</param>
        private void AddToWieldedObjects(ref WorldObject item, WorldObject wielder, EquipMask currentWieldedLocation)
        {
            // Unset container fields
            item.PlacementPosition = null;
            item.ContainerId = null;
            // Set fields needed to be wielded.
            item.WielderId = (int)wielder.Guid.Full;
            item.CurrentWieldedLocation = currentWieldedLocation;

            if (!wielder.WieldedObjects.ContainsKey(item.Guid))
            {
                wielder.WieldedObjects.Add(item.Guid, item);

                Burden += item.Burden;
            }
        }

        /// <summary>
        /// This method is used to remove an item from the Wielded Objects dictionary.
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II
        /// </summary>
        /// <param name="itemGuid">Guid of the item to be unwielded.</param>
        public void RemoveFromWieldedObjects(ObjectGuid itemGuid)
        {
            if (WieldedObjects.ContainsKey(itemGuid))
            {
                Burden -= WieldedObjects[itemGuid].Burden;
                WieldedObjects.Remove(itemGuid);
            }
        }





        /// <summary>
        /// This method is called in response to a put item in container message.  It is used when the item going
        /// into a container was wielded.   It sets the appropriate properties, sends out response messages
        /// and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        private void HandleUnwieldItem(Container container, WorldObject item, int placement)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            item.ContainerId = (int)container.Guid.Full;
            SetInventoryForContainer(item, placement);

            RemoveFromWieldedObjects(item.Guid);
            // We will always be updating the player appearance
            UpdateAppearance(this);

            if ((oldLocation & EquipMask.Selectable) != 0)
            {
                // We are coming from a hand shield slot.
                Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
            }

            // Set the container stuff
            item.ContainerId = (int)container.Guid.Full;
            item.PlacementPosition = placement;

            ActionChain inContainerChain = new ActionChain();
            inContainerChain.AddAction(this, () =>
            {
                if (container.Guid != Guid)
                {
                    container.AddToInventory(item, placement);
                    Burden += item.Burden;
                }
                else
                    AddToInventory(item, placement);
            });
            inContainerChain.EnqueueChain();

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, 0),
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, container.Guid),
                                            new GameMessagePickupEvent(item),
                                            new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0),
                                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                                            new GameMessageObjDescEvent(this));

            if ((oldLocation != EquipMask.MissileWeapon && oldLocation != EquipMask.Held && oldLocation != EquipMask.MeleeWeapon) || ((CombatMode & CombatMode.CombatCombat) == 0))
                return;
            HandleSwitchToPeaceMode(CombatMode);
            HandleSwitchToMeleeCombatMode(CombatMode);
        }

        /// <summary>
        /// Method is called in response to put item in container message.   This use case is we are just
        /// reorganizing our items.   It is either a in pack slot to slot move, or we could be going from one
        /// pack (container) to another. This method is called from an action chain.  Og II
        /// </summary>
        /// <param name="item">the item we are moving</param>
        /// <param name="container">what container are we going in</param>
        /// <param name="placement">what is my slot position within that container</param>
        private void HandleMove(ref WorldObject item, Container container, int placement)
        {
            RemoveWorldObjectFromInventory(item.Guid);

            item.ContainerId = (int)container.Guid.Full;
            item.PlacementPosition = placement;

            container.AddToInventory(item, placement);

            if (item.ContainerId != Guid.Full)
            {
                Burden += item.Burden ?? 0;
                if (item.WeenieType == WeenieType.Coin)
                    UpdateCurrencyClientCalculations(WeenieType.Coin);
            }
            Session.Network.EnqueueSend(
                new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container,
                    container.Guid));

            Session.SaveSession();
        }

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting
        /// item. Og II
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are spliting</param>
        /// <param name="containerId">The guid of the container</param>
        /// <param name="place">Place is the slot in the container we are spliting into.   Range 0-MaxCapacity</param>
        /// <param name="amount">The amount of the stack we are spliting from that we are moving to a new stack.</param>
        /// <returns></returns>

        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, int place, ushort amount)
        {
            // TODO: add the complementary method to combine items Og II
            ActionChain splitItemsChain = new ActionChain();
            splitItemsChain.AddAction(this, () =>
            {
                Container container;
                if (containerId == Guid.Full)
                {
                    container = this;
                }
                else
                {
                    container = (Container)GetInventoryItem(new ObjectGuid(containerId));
                }

                if (container == null)
                {
                    log.InfoFormat("Asked to split stack {0} in container {1} - the container was not found",
                        stackId,
                        containerId);
                    return;
                }
                WorldObject stack = container.GetInventoryItem(new ObjectGuid(stackId));
                if (stack == null)
                {
                    log.InfoFormat("Asked to split stack {0} in container {1} - the stack item was not found",
                        stackId,
                        containerId);
                    return;
                }
                if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
                {
                    log.InfoFormat(
                        "Asked to split stack {0} in container {1} - with amount of {2} but there is not enough to split",
                        stackId, containerId, amount);
                    return;
                }

                // Ok we are in business
                throw new System.NotImplementedException();/*
                WorldObject newStack = WorldObjectFactory.CreateWorldObject(stack.NewAceObjectFromCopy()); // Fix suggested by Mogwai and Og II
                container.AddToInventory(newStack);

                ushort oldStackSize = (ushort)stack.StackSize;
                stack.StackSize = (ushort)(oldStackSize - amount);

                newStack.StackSize = amount;

                GameMessagePutObjectInContainer msgPutObjectInContainer =
                    new GameMessagePutObjectInContainer(Session, container.Guid, newStack, place);
                GameMessageSetStackSize msgAdjustOldStackSize = new GameMessageSetStackSize(stack.Sequences,
                    stack.Guid, (int)stack.StackSize, (int)stack.Value);
                GameMessageCreateObject msgNewStack = new GameMessageCreateObject(newStack);

                CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange,
                    msgPutObjectInContainer, msgAdjustOldStackSize, msgNewStack);

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCurrencyClientCalculations(WeenieType.Coin);*/
            });
            splitItemsChain.EnqueueChain();
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

            Session.Network.EnqueueSend(contractMsg);
        }

        /// <summary>
        /// This method is used to remove X number of items from a stack, including
        /// If amount to remove is greater or equal to the current stacksize, item will be removed.
        /// </summary>
        /// <param name="stackId">Guid.Full of the stacked item</param>
        /// <param name="containerId">Guid.Full of the container that contains the item</param>
        /// <param name="amount">Amount taken out of the stack</param>
        public void HandleActionRemoveItemFromInventory(uint stackId, uint containerId, ushort amount)
        {
            // FIXME: This method has been morphed into doing a few things we either need to rename it or
            // something.   This may or may not remove item from inventory.
            ActionChain removeItemsChain = new ActionChain();
            removeItemsChain.AddAction(this, () =>
            {
                Container container;
                if (containerId == Guid.Full)
                    container = this;
                else
                    container = (Container)GetInventoryItem(new ObjectGuid(containerId));

                if (container == null)
                {
                    log.InfoFormat("Asked to remove an item {0} in container {1} - the container was not found",
                        stackId,
                        containerId);
                    return;
                }

                WorldObject item = container.GetInventoryItem(new ObjectGuid(stackId));
                if (item == null)
                {
                    log.InfoFormat("Asked to remove an item {0} in container {1} - the item was not found",
                        stackId,
                        containerId);
                    return;
                }

                if (amount >= item.StackSize)
                    amount = (ushort)item.StackSize;

                ushort oldStackSize = (ushort)item.StackSize;
                ushort newStackSize = (ushort)(oldStackSize - amount);

                if (newStackSize < 1)
                    newStackSize = 0;

                item.StackSize = newStackSize;

                Session.Network.EnqueueSend(new GameMessageSetStackSize(item.Sequences, item.Guid, (int)item.StackSize, (int)item.Value));

                if (newStackSize < 1)
                {
                    // Remove item from inventory
                    DestroyInventoryItem(item);
                    // Clean up the shard database.
                    DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);
                }
                else
                    Burden = (ushort)(Burden - (item.StackUnitBurden * amount));
            });
            removeItemsChain.EnqueueChain();
        }

        /// <summary>
        /// This method is used to pick items off the world - out of 3D space and into our inventory or to a wielded slot.
        /// It checks the use case needed, sends the appropriate response messages.   In addition, it will move to objects
        /// that are out of range in the attemp to pick them up.   It will call update apperiance if needed and you have
        /// wielded an item from the ground. Og II
        /// </summary>
        /// <param name="container"></param>
        /// <param name="itemGuid"></param>
        /// <param name="placement"></param>
        /// <param name="iidPropertyId"></param>
        private void HandlePickupItem(Container container, ObjectGuid itemGuid, int placement, PropertyInstanceId iidPropertyId)
        {
            // Logical operations:
            // !! FIXME: How to handle repeat on condition?
            // while (!objectInRange)
            //   try Move to object
            // !! FIXME: How to handle conditional
            // Try acquire from landblock
            // if acquire successful:
            //   add to container
            ActionChain pickUpItemChain = new ActionChain();

            // Move to the object
            pickUpItemChain.AddChain(CreateMoveToChain(itemGuid, PickUpDistance));

            // Pick up the object
            // Start pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(Guid,
                        Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                        Sequences, motion));
            });
            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
            var pickupAnimationLength = MotionTable.GetAnimationLength(motionTable, MotionCommand.Pickup);
            pickUpItemChain.AddDelaySeconds(pickupAnimationLength);

            // Ask landblock to transfer item
            // pickUpItemChain.AddAction(CurrentLandblock, () => CurrentLandblock.TransferItem(itemGuid, containerGuid));
            if (container.Guid.IsPlayer())
                CurrentLandblock.QueueItemTransfer(pickUpItemChain, itemGuid, container.Guid);
            else
                CurrentLandblock.ScheduleItemTransferInContainer(pickUpItemChain, itemGuid, (Container)GetInventoryItem(container.Guid));

            // Finish pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                // If success, the item is in our inventory:
                WorldObject item = GetInventoryItem(itemGuid);

                if (item.ContainerId != Guid.Full)
                {
                    Burden += item.Burden ?? 0;

                    if (item.WeenieType == WeenieType.Coin)
                    {
                        UpdateCurrencyClientCalculations(WeenieType.Coin);
                    }
                }

                if (item is Container itemAsContainer)
                {
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

                    foreach (var packItem in itemAsContainer.Inventory)
                    {
                        Session.Network.EnqueueSend(new GameMessageCreateObject(packItem.Value));
                        UpdateCurrencyClientCalculations(WeenieType.Coin);
                    }
                }

                // Update all our stuff if we succeeded
                if (item != null)
                {
                    SetInventoryForContainer(item, placement);
                    // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...
                    var motion = new UniversalMotion(MotionStance.Standing);
                    if (iidPropertyId == PropertyInstanceId.Container)
                    {
                        Session.Network.EnqueueSend(
                            ////new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, UpdateBurden()),
                            new GameMessageSound(Guid, Sound.PickUpItem, 1.0f),
                            new GameMessageUpdateInstanceId(itemGuid, container.Guid, iidPropertyId),
                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement));
                    }
                    else
                    {
                        AddToWieldedObjects(ref item, container, (EquipMask)placement);
                        UpdateAppearance(container);
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                    new GameMessageObjDescEvent(this),
                                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid, PropertyInstanceId.Wielder),
                                                    new GameEventWieldItem(Session, itemGuid.Full, placement));
                    }

                    CurrentLandblock.EnqueueBroadcast(
                        Location,
                        Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(
                            Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences,
                            motion),
                        new GameMessagePickupEvent(item));

                    if (iidPropertyId == PropertyInstanceId.Wielder)
                        CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageObjDescEvent(this));

                    // TODO: Og II - check this later to see if it is still required.
                    Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                }
                // If we didn't succeed, just stand up and be ashamed of ourself
                else
                {
                    var motion = new UniversalMotion(MotionStance.Standing);

                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences, motion));
                    // CurrentLandblock.EnqueueBroadcast(self shame);
                }
            });
            // Set chain to run
            pickUpItemChain.EnqueueChain();
        }

        /// <summary>
        /// This method was developed by OptimShi.   It looks at currently wielded items and does the appropriate
        /// model replacements.   It then sends all uncovered  body parts. Og II
        /// </summary>
        /// <param name="container"></param>
        public void UpdateAppearance(Container container)
        {
            ClearObjDesc();
            AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette

            var coverage = new List<uint>();

            foreach (var w in WieldedObjects)
            {
                // We can wield things that are not part of our model, only use those items that can cover our model.
                if ((w.Value.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0)
                {
                    ClothingTable item;
                    if (w.Value.ClothingBase != null)
                        item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)w.Value.ClothingBase);
                    else
                    {
                        ChatPacket.SendServerMessage(
                            Session,
                            "We have not implemented the visual appearance for that item yet. ",
                            ChatMessageType.AdminTell);
                        return;
                    }

                    if (item.ClothingBaseEffects.ContainsKey(SetupTableId))
                    // Check if the player model has data. Gear Knights, this is usually you.
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[SetupTableId];
                        foreach (CloObjectEffect t in clothingBaseEffec.CloObjectEffects)
                        {
                            byte partNum = (byte)t.Index;
                            AddModel((byte)t.Index, (ushort)t.ModelId);
                            coverage.Add(partNum);
                            foreach (CloTextureEffect t1 in t.CloTextureEffects)
                                AddTexture((byte)t.Index, (ushort)t1.OldTexture, (ushort)t1.NewTexture);
                        }

                        foreach (ModelPalette p in w.Value.GetPalettes)
                            AddPalette(p.PaletteId, p.Offset, p.Length);
                    }
                }
            }
            // Add the "naked" body parts. These are the ones not already covered.
            if (SetupTableId > 0)
            {
                var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>(SetupTableId);
                for (byte i = 0; i < baseSetup.Parts.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head, that was already covered by AddCharacterBaseModelData()
                        AddModel(i, baseSetup.Parts[i]);
                }
            }
        }

        /// <summary>
        /// This method sets properties needed for items that will be child items.
        /// Items here are only items equipped in the hands.  This deals with the orientation
        /// and positioning for visual appearance of the child items held by the parent. Og II
        /// </summary>
        /// <param name="container">Who is the parent of this child item</param>
        /// <param name="item">The child item - we link them together</param>
        /// <param name="placement">Where is this on the parent - where is it equipped</param>
        /// <param name="placementId">out parameter - this deals with the orientation of the child item as it relates to parent model</param>
        /// <param name="childLocation">out parameter - this is another part of the orientation data for correct visual display</param>
        public void SetChild(Container container, WorldObject item, int placement, out int placementId, out int childLocation)
        {
            placementId = 0;
            childLocation = 0;
            // TODO:   I think there is a state missing - it is one of the edge cases.   I need to revist this.   Og II
            switch ((EquipMask)placement)
            {
                case EquipMask.MissileWeapon:
                    {
                        ////if (item.DefaultCombatStyle == MotionStance.BowAttack ||
                        ////    item.DefaultCombatStyle == MotionStance.CrossBowAttack ||
                        ////    item.DefaultCombatStyle == MotionStance.AtlatlCombat)
                        if (item.DefaultCombatStyle == CombatStyle.Atlatl ||
                            item.DefaultCombatStyle == CombatStyle.Bow ||
                            item.DefaultCombatStyle == CombatStyle.Crossbow)
                        {
                            childLocation = 2;
                            placementId = 3;
                        }
                        else
                        {
                            childLocation = 1;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Shield:
                    {
                        if (item.ItemType == ItemType.Armor)
                        {
                            childLocation = 3;
                            placementId = 6;
                        }
                        else
                        {
                            childLocation = 8;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Held:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
                default:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
            }
            if (item.CurrentWieldedLocation != null)
                container.Children.Add(new HeldItem(item.Guid.Full, childLocation, (EquipMask)item.CurrentWieldedLocation));
            item.ParentLocation = (ParentLocation)childLocation;
            item.Location = Location;
            item.Placement = (Placement)placementId;
        }

        public void HandleActionWieldItem(Container container, uint itemId, int placement)
        {
            ActionChain wieldChain = new ActionChain();
            wieldChain.AddAction(this, () =>
            {
                ObjectGuid itemGuid = new ObjectGuid(itemId);
                WorldObject item = GetInventoryItem(itemGuid);
                WorldObject packItem;

                if (item != null)
                {
                    ObjectGuid containerGuid = ObjectGuid.Invalid;
                    var containers = Inventory.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
                    foreach (var subpack in containers)
                    {
                        throw new System.NotImplementedException();/*
                        if (subpack.Value.InventoryObjects.TryGetValue(itemGuid, out packItem))
                        {
                            containerGuid = subpack.Value.Guid;
                            break;
                        }*/
                    }

                    Container pack;
                    if (item != null && containerGuid != ObjectGuid.Invalid)
                    {
                        pack = (Container)GetInventoryItem(containerGuid);

                        RemoveWorldObjectFromInventory(itemGuid);
                    }
                    else
                    {
                        if (item != null)
                            RemoveWorldObjectFromInventory(itemGuid);
                    }

                    AddToWieldedObjects(ref item, container, (EquipMask)placement);

                    if ((EquipMask)placement == EquipMask.MissileAmmo)
                        Session.Network.EnqueueSend(new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                    new GameMessageSound(Guid, Sound.WieldObject, (float)1.0));
                    else
                    {
                        if (((EquipMask)placement & EquipMask.Selectable) != 0)
                        {
                            SetChild(container, item, placement, out var placementId, out var childLocation);

                            UpdateAppearance(container);

                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                            new GameMessageParentEvent(Session.Player, item, childLocation, placementId),
                                                            new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                            new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement));

                            if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                                return;
                            switch ((EquipMask)placement)
                            {
                                case EquipMask.MissileWeapon:
                                    SetCombatMode(CombatMode.Missile);
                                    break;
                                case EquipMask.Held:
                                    SetCombatMode(CombatMode.Magic);
                                    break;
                                default:
                                    SetCombatMode(CombatMode.Melee);
                                    break;
                            }
                        }
                        else
                        {
                            UpdateAppearance(container);

                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                        new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                        new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                        new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                        new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                        new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement),
                                                        new GameMessageObjDescEvent(container));
                        }
                    }
                }
                else
                {
                    HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Wielder);
                }
            });
            wieldChain.EnqueueChain();
        }

        public void PutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, int placement = 0)
        {
            Container container;

            if (containerGuid.IsPlayer())
                container = this;
            else
            {
                // Ok I am going into player pack - not the main pack.

                // TODO pick up here - I have a generic object for a container, need to find out why.
                container = (Container)GetInventoryItem(containerGuid);
            }

            // is this something I already have? If not, it has to be a pickup - do the pickup and out.
            if (!HasItem(itemGuid))
            {
                // This is a pickup into our main pack.
                HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Container);
                return;
            }

            // Ok, I know my container and I know I must have the item so let's get it.
            WorldObject item = GetInventoryItem(itemGuid);

            // check wilded.
            if (item == null)
                item = GetWieldedItem(itemGuid);

            // Was I equiped?   If so, lets take care of that and unequip
            if (item.WielderId != null)
            {
                HandleUnwieldItem(container, item, placement);
                return;
            }

            // if were are still here, this needs to do a pack pack or main pack move.
            HandleMove(ref item, container, placement);
        }

        /// <summary>
        /// Context: only call when in the player action loop
        /// </summary>
        public void DestroyInventoryItem(WorldObject wo)
        {
            RemoveWorldObjectFromInventory(wo.Guid);
            Session.Network.EnqueueSend(new GameMessageRemoveObject(wo));
            ////Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, (uint)Burden));
        }

        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            ActionChain dropChain = new ActionChain();

            // Goody Goody -- lets build  drop chain
            // First start drop animation
            dropChain.AddAction(this, () =>
            {
                // check packs of item.
                WorldObject item = GetInventoryItem(itemGuid);
                if (item == null)
                {
                    // check to see if this item is wielded
                    item = GetWieldedItem(itemGuid);
                    if (item != null)
                    {
                        RemoveFromWieldedObjects(itemGuid);
                        UpdateAppearance(this);
                        Session.Network.EnqueueSend(
                            new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                            new GameMessageObjDescEvent(this),
                            new GameMessageUpdateInstanceId(Guid, new ObjectGuid(0), PropertyInstanceId.Wielder));
                    }
                }
                else
                {
                    RemoveWorldObjectFromInventory(itemGuid);
                    if (item.WeenieType == WeenieType.Coin || item.WeenieType == WeenieType.Container)
                        UpdateCurrencyClientCalculations(WeenieType.Coin);
                }

                SetInventoryForWorld(item);

                UniversalMotion motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                Session.Network.EnqueueSend(new GameMessageUpdateInstanceId(itemGuid, new ObjectGuid(0), PropertyInstanceId.Container));

                // Set drop motion
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);

                // Now wait for Drop Motion to finish -- use ActionChain
                ActionChain chain = new ActionChain();

                // Wait for drop animation
                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
                var pickupAnimationLength = MotionTable.GetAnimationLength(motionTable, MotionCommand.Pickup);
                chain.AddDelaySeconds(pickupAnimationLength);

                // Play drop sound
                // Put item on landblock
                chain.AddAction(this, () =>
                {
                    motion = new UniversalMotion(MotionStance.Standing);
                    CurrentLandblock.EnqueueBroadcastMotion(this, motion);
                    Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.DropItem, (float)1.0),
                    new GameMessagePutObjectIn3d(Session, this, itemGuid),
                    new GameMessageUpdateInstanceId(itemGuid, new ObjectGuid(0), PropertyInstanceId.Container));

                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                    Debug.Assert(item != null, "item != null");
                    item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                    item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                    CurrentLandblock.AddWorldObject(item);

                    // Ok we have handed off to the landblock, let's clean up the shard database.
                    DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);

                    Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                });

                chain.EnqueueChain();
                // Removed SaveSession - this was causing items that were dropped to not be removed
                // from inventory.   If this causes a problem with vendor, we need to fix vendor.  Og II
            });

            dropChain.EnqueueChain();
        }

        public void HandleActionUseOnTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            ActionChain chain = new ActionChain(this, () =>
            {
                WorldObject invSource = GetInventoryItem(sourceObjectId);
                WorldObject invTarget = GetInventoryItem(targetObjectId);

                if (invTarget != null)
                {
                    // inventory on inventory, we can do this now
                    RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
                }
                else if (invSource.WeenieType == WeenieType.Key)
                {
                    WorldObject theTarget = CurrentLandblock.GetObject(targetObjectId);
                    Key key = invSource as Key;
                    key.HandleActionUseOnTarget(this, theTarget);
                }
                else if (targetObjectId == Guid)
                {
                    // using something on ourselves
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
                }
                else
                {
                    WorldObject theTarget = CurrentLandblock.GetObject(targetObjectId);
                    RecipeManager.UseObjectOnTarget(this, invSource, theTarget);
                }
            });
            chain.EnqueueChain();
        }

        public void HandleActionUse(ObjectGuid usedItemId)
        {
            new ActionChain(this, () =>
            {
                WorldObject iwo = GetInventoryItem(usedItemId);
                if (iwo != null)
                {
                    iwo.OnUse(Session);
                }
                else
                {
                    if (CurrentLandblock != null)
                    {
                        // Just forward our action to the appropriate user...
                        WorldObject wo = CurrentLandblock.GetObject(usedItemId);
                        if (wo != null)
                        {
                            wo.ActOnUse(Guid);
                        }
                    }
                }
            }).EnqueueChain();
        }

        /// <summary>
        /// This method handles inscription.   If you remove the inscription, it will remove the data from the object and
        /// remove it from the shard database - all inscriptions are stored in ace_object_properties_string Og II
        /// </summary>
        /// <param name="itemGuid">This is the object that we are trying to inscribe</param>
        /// <param name="inscriptionText">This is our inscription</param>
        public void HandleActionSetInscription(ObjectGuid itemGuid, string inscriptionText)
        {
            new ActionChain(this, () =>
            {
                WorldObject iwo = GetInventoryItem(itemGuid);
                if (iwo == null)
                {
                    return;
                }

                //if (iwo.Inscribable && iwo.ScribeName != "prewritten")
                //{
                //    if (iwo.ScribeName != null && iwo.ScribeName != this.Name)
                //    {
                //        ChatPacket.SendServerMessage(Session,
                //            "Only the original scribe may alter this without the use of an uninscription stone.",
                //            ChatMessageType.Broadcast);
                //    }
                //    else
                //    {
                //        if (inscriptionText != "")
                //        {
                //            iwo.Inscription = inscriptionText;
                //            iwo.ScribeName = this.Name;
                //            iwo.ScribeAccount = Session.Account;
                //            Session.Network.EnqueueSend(new GameEventInscriptionResponse(Session, iwo.Guid.Full,
                //                iwo.Inscription, iwo.ScribeName, iwo.ScribeAccount));
                //        }
                //        else
                //        {
                //            iwo.Inscription = null;
                //            iwo.ScribeName = null;
                //            iwo.ScribeAccount = null;
                //        }
                //    }
                //}
                //else
                //{
                //    // Send some cool you cannot inscribe that item message.   Not sure how that was handled live,
                //    // I could not find a pcap of a failed inscription. Og II
                //    ChatPacket.SendServerMessage(Session, "Target item cannot be inscribed.", ChatMessageType.System);
                //}
            }).EnqueueChain();
        }

        public void HandleActionApplySoundEffect(Sound sound)
        {
            new ActionChain(this, () => PlaySound(sound, Guid)).EnqueueChain();
        }

        public ActionChain CreateMoveToChain(ObjectGuid target, float distance)
        {
            ActionChain moveToChain = new ActionChain();
            // While !at(thing) moveToThing
            ActionChain moveToBody = new ActionChain();
            moveToChain.AddAction(this, () =>
            {
                Position dest = CurrentLandblock.GetPosition(target);
                if (dest == null)
                {
                    log.Error("FIXME: Need the ability to cancel actions on error");
                    return;
                }

                if (CurrentLandblock.GetWeenieType(target) == WeenieType.Portal)
                {
                    OnAutonomousMove(CurrentLandblock.GetPosition(target),
                                            Sequences, MovementTypes.MoveToPosition, target);
                }
                else
                {
                    OnAutonomousMove(CurrentLandblock.GetPosition(target),
                                            Sequences, MovementTypes.MoveToObject, target);
                }
            });

            // poll for arrival every .1 seconds
            moveToBody.AddDelaySeconds(.1);

            moveToChain.AddLoop(this, () =>
            {
                float outdistance;
                // Break loop if CurrentLandblock == null (we portaled or logged out), or if we arrive at the item
                if (CurrentLandblock == null)
                {
                    return false;
                }

                bool ret = !CurrentLandblock.WithinUseRadius(Guid, target, out outdistance, out var valid);
                if (!valid)
                {
                    // If one of the items isn't on a landblock
                    ret = false;
                }
                return ret;
            }, moveToBody);

            return moveToChain;
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

        public void TestWieldItem(Session session, uint modelId, int palOption, float shade = 0)
        {
            // ClothingTable item = ClothingTable.ReadFromDat(0x1000002C); // Olthoi Helm
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000867); // Cloak
            // ClothingTable item = ClothingTable.ReadFromDat(0x10000008); // Gloves
            // ClothingTable item = ClothingTable.ReadFromDat(0x100000AD); // Heaume
            var item = DatManager.PortalDat.ReadFromDat<ClothingTable>(modelId);

            int palCount = 0;

            List<uint> coverage = new List<uint>(); // we'll store our fake coverage items here
            ClearObjDesc();
            AddCharacterBaseModelData(); // Add back in the facial features, hair and skin palette

            if (item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
            {
                // Add the model and texture(s)
                ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
                for (int i = 0; i < clothingBaseEffec.CloObjectEffects.Count; i++)
                {
                    byte partNum = (byte)clothingBaseEffec.CloObjectEffects[i].Index;
                    AddModel((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].ModelId);
                    coverage.Add(partNum);
                    for (int j = 0; j < clothingBaseEffec.CloObjectEffects[i].CloTextureEffects.Count; j++)
                        AddTexture((byte)clothingBaseEffec.CloObjectEffects[i].Index, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)clothingBaseEffec.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                }

                // Apply an appropriate palette. We'll just pick a random one if not specificed--it's a surprise every time!
                // For actual equipment, these should just be stored in the ace_object palette_change table and loaded from there
                if (item.ClothingSubPalEffects.Count > 0)
                {
                    int size = item.ClothingSubPalEffects.Count;
                    palCount = size;

                    CloSubPalEffect itemSubPal;
                    // Generate a random index if one isn't provided
                    if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
                    {
                        itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
                    }
                    else
                    {
                        List<CloSubPalEffect> values = item.ClothingSubPalEffects.Values.ToList();
                        Random rand = new Random();
                        palOption = rand.Next(size);
                        itemSubPal = values[palOption];
                    }

                    for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                    {
                        var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
                        ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

                        for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
                        }
                    }
                }

                // Add the "naked" body parts. These are the ones not already covered.
                var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>((uint)SetupTableId);
                for (byte i = 0; i < baseSetup.Parts.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head.
                        AddModel(i, baseSetup.Parts[i]);
                }

                var objDescEvent = new GameMessageObjDescEvent(this);
                session.Network.EnqueueSend(objDescEvent);
                ChatPacket.SendServerMessage(session, "Equipping model " + modelId.ToString("X") +
                                                      ", Applying palette index " + palOption + " of " + palCount +
                                                      " with a shade value of " + shade + ".", ChatMessageType.Broadcast);
            }
            else
            {
                // Alert about the failure
                ChatPacket.SendServerMessage(session, "Could not match that item to your character model.", ChatMessageType.Broadcast);
            }
        }

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

        public void DoMoveTo(WorldObject wo)
        {
            ActionChain moveToObjectChain = new ActionChain();

            moveToObjectChain.AddChain(CreateMoveToChain(wo.Guid, 0.2f));
            moveToObjectChain.AddDelaySeconds(0.50);

            moveToObjectChain.AddAction(wo, () => wo.ActOnUse(Guid));

            moveToObjectChain.EnqueueChain();
        }



        public void SendUseDoneEvent()
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session));
        }

        private int coinValue;
        public override int? CoinValue
        {
            get => coinValue;
            set
            {
                if (value != coinValue)
                {
                    base.CoinValue = value;
                    coinValue = (int)value;
                    if (FirstEnterWorldDone)
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.CoinValue, coinValue));
                }
            }
        }

        private ushort burden;
        public override ushort? Burden
        {
            get => burden;
            set
            {
                if (value != burden)
                {
                    base.Burden = value;
                    burden = (ushort)value;
                    if (FirstEnterWorldDone)
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.EncumbranceVal, burden));
                }
            }
        }

        private int value = 0;
        public override int? Value
        {
            get => value;
            set { base.Value = 0; }
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
            var motionAnimationLength = MotionTable.GetAnimationLength(motionTable, MotionCommand.Eat);
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
