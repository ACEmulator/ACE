using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using log4net;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
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
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

using MotionTable = ACE.DatLoader.FileTypes.MotionTable;
using Position = ACE.Entity.Position;

namespace ACE.Server.WorldObjects
{
    public partial class Player : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Character Character { get; }

        public Session Session { get; }

        public QuestManager QuestManager;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Player(Weenie weenie, ObjectGuid guid, uint accountId) : base(weenie, guid)
        {
            Character = new Character();
            Character.Id = guid.Full;
            Character.AccountId = accountId;
            Character.Name = GetProperty(PropertyString.Name);
            CharacterChangesDetected = true;

            // Make sure properties this WorldObject requires are not null.
            AvailableExperience = AvailableExperience ?? 0;
            TotalExperience = TotalExperience ?? 0;

            Attackable = true;

            SetProperty(PropertyString.DateOfBirth, $"{DateTime.UtcNow:dd MMMM yyyy}");

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Player(Biota biota, IEnumerable<Biota> inventory, IEnumerable<Biota> wieldedItems, Character character, Session session) : base(biota)
        {
            SortBiotasIntoInventory(inventory);
            AddBiotasToEquippedObjects(wieldedItems);

            Character = character;
            Session = session;

            SetEphemeralValues();

            // THIS IS A TEMPORARY PATCH TO COPY OVER EXISTING CHARACTER OPTIONS FROM THE BIOTA TO THE CHARACTER OBJECT.
            // This can be removed in time. 2018-09-01 Mag-nus
            if (Character.CharacterOptions1 == 0 && Character.CharacterOptions2 == 0)
            {
                Character.CharacterOptions1 = GetProperty((PropertyInt)9003) ?? 1355064650;
                Character.CharacterOptions2 = GetProperty((PropertyInt)9004) ?? 34560;
                CharacterChangesDetected = true;
            }
        }

        public override void InitPhysicsObj()
        {
            base.InitPhysicsObj();

            // set pink bubble state
            IgnoreCollisions = true; ReportCollisions = false; Hidden = true;

            PhysicsObj.SetPlayer();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Player;

            // This is the default send upon log in and the most common. Anything with a velocity will need to add that flag.
            PositionFlag |= UpdatePositionFlag.ZeroQx | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.Contact | UpdatePositionFlag.Placement;

            CurrentMotionState = new UniversalMotion(MotionStance.NonCombat);

            // radius for object updates
            ListeningRadius = 5f;

            if (Session != null && Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
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

            if (Session != null && (AdvocateQuest ?? false) && IsAdvocate) // Advocate permissions are per character regardless of override
            {
                if (Session.AccessLevel == AccessLevel.Player)
                    Session.SetAccessLevel(AccessLevel.Advocate); // Elevate to Advocate permissions
                if (AdvocateLevel > 4)
                    IsPsr = true; // Enable AdvocateTeleport via MapClick
            }

            UpdateCoinValue(false);

            if (Session != null && Session.IsOnline)
                AllegianceManager.LoadPlayer(this);

            QuestManager = new QuestManager(this);

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

            // FirstEnterWorldDone = false;

            // IsAlive = true;
        }

        /// <summary>
        /// Called every ~5 secs for equipped mana consuming items
        /// </summary>
        public void ManaConsumersTick()
        {
            if (EquippedObjectsLoaded)
            {
                var EquippedManaConsumers = EquippedObjects.Where(k =>
                    (k.Value.IsAffecting ?? false) &&
                    k.Value.ManaRate.HasValue &&
                    k.Value.ItemMaxMana.HasValue &&
                    k.Value.ItemCurMana.HasValue &&
                    k.Value.ItemCurMana.Value > 0).ToList();

                EquippedManaConsumers.ForEach(k =>
                {
                    var item = k.Value;
                    var rate = item.ManaRate.Value;
                    if (!item.ItemManaConsumptionTimestamp.HasValue) item.ItemManaConsumptionTimestamp = DateTime.Now;
                    DateTime mostRecentBurn = item.ItemManaConsumptionTimestamp.Value;

                    var timePerBurn = -1 / rate;

                    var secondsSinceLastBurn = (DateTime.Now - mostRecentBurn).TotalSeconds;

                    var delta = secondsSinceLastBurn / timePerBurn;

                    var deltaChopped = (int)Math.Floor(delta);
                    var deltaExtra = delta - deltaChopped;

                    if (deltaChopped > 0)
                    {
                        var timeToAdd = (int)Math.Floor(deltaChopped * timePerBurn);
                        item.ItemManaConsumptionTimestamp = mostRecentBurn + new TimeSpan(0, 0, timeToAdd);
                        var manaToBurn = Math.Min(item.ItemCurMana.Value, deltaChopped);
                        deltaChopped = Math.Clamp(deltaChopped, 0, 10);
                        item.ItemCurMana -= deltaChopped;

                        if (item.ItemCurMana < 1 || item.ItemCurMana == null)
                        {
                            item.IsAffecting = false;
                            var msg = new GameMessageSystemChat($"Your {item.Name} is out of Mana.", ChatMessageType.Magic);
                            var sound = new GameMessageSound(Guid, Sound.ItemManaDepleted);
                            Session.Network.EnqueueSend(msg, sound);
                            if (item.WielderId != null)
                            {
                                if (item.Biota.BiotaPropertiesSpellBook != null)
                                {
                                    // unsure if these messages / sounds were ever sent in retail,
                                    // or if it just purged the enchantments invisibly
                                    // doing a delay here to prevent 'SpellExpired' sounds from overlapping with 'ItemManaDepleted'
                                    var actionChain = new ActionChain();
                                    actionChain.AddDelaySeconds(2.0f);
                                    actionChain.AddAction(this, () =>
                                    {
                                        for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                                        {
                                            // TODO: layering
                                            RemoveItemSpell(item.Guid, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell);
                                        }
                                    });
                                    actionChain.EnqueueChain();
                                }
                            }
                        }
                        else
                        {
                            // get time until empty
                            var secondsUntilEmpty = ((item.ItemCurMana - deltaExtra) * timePerBurn);
                            if (secondsUntilEmpty <= 120 && (!item.ItemManaDepletionMessageTimestamp.HasValue || (DateTime.Now - item.ItemManaDepletionMessageTimestamp.Value).TotalSeconds > 120))
                            {
                                item.ItemManaDepletionMessageTimestamp = DateTime.Now;
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {item.Name} is low on Mana.", ChatMessageType.Magic));
                            }
                        }
                    }
                });
            }
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

        public MotionStance stance = MotionStance.NonCombat;

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

            // if local, examine it
            if (wo != null)
                wo.Examine(Session);
            else
            {
                // examine item on landblock
                wo = CurrentLandblock?.GetObject(examinationId);
                if (wo != null)
                    wo.Examine(Session);
                else
                {
                    // search creature equipped weapons on nearby landblocks
                    wo = CurrentLandblock?.GetWieldedObject(examinationId);
                    if (wo != null)
                        wo.Examine(Session);
                    else
                        log.Warn("${Name} tried to appraise object {examinationId:X8}, couldn't find it");
                }
            }

            RequestedAppraisalTarget = examinationId.Full;
            CurrentAppraisalTarget = examinationId.Full;
        }

        public override void OnCollideObject(WorldObject target)
        {
            if (target.ReportCollisions == false)
                return;

            if (target is Portal)
                (target as Portal).OnCollideObject(this);
            else if (target is Hotspot)
                (target as Hotspot).OnCollideObject(this);
        }

        public override void OnCollideObjectEnd(WorldObject target)
        {
            if (target is Hotspot)
                (target as Hotspot).OnCollideObjectEnd(this);
        }

        public void HandleActionQueryHealth(ObjectGuid queryId)
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
            var obj = CurrentLandblock?.GetObject(queryId);
            if (obj != null)
                obj.QueryHealth(Session);
        }

        public void HandleActionQueryItemMana(ObjectGuid queryId)
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
                CurrentLandblock?.GetObject(bookId).ReadBookPage(Session, pageNum);
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

            // OutdoorChatRange?
            EnqueueBroadcast(deathBroadcast);
        }

        /// <summary>
        /// Emits a sound at location sourceId and volume
        /// The client will perform sound attenuation / volume adjustment
        /// based on the listener distance from the origin of sourceId
        /// </summary>
        public void PlaySound(Sound sound, ObjectGuid sourceId, float volume = 1.0f)
        {
            Session.Network.EnqueueSend(new GameMessageSound(sourceId, sound, volume));
        }

 
        /// <summary>
        /// Returns false if the player has chosen to Appear Offline.  Otherwise it will return their actual online status.
        /// </summary>
        public bool GetVirtualOnlineStatus()
        {
            if (GetCharacterOption(CharacterOption.AppearOffline))
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
                logoutChain.AddAction(this, () => CurrentLandblock.RemoveWorldObject(Guid, false));
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
                var logout = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.LogOut));
                EnqueueBroadcastMotion(logout);

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

            EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(this, PropertyBool.IgnoreHouseBarriers, IgnoreHouseBarriers ?? false));

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Bypass Housing Barriers now set to: {IgnoreHouseBarriers}", ChatMessageType.Broadcast));
        }

        public void SendAutonomousPosition()
        {
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }



        public void HandleActionFinishBarber(ClientMessage message)
        {
            // Read the payload sent from the client...
            PaletteBaseId = message.Payload.ReadUInt32();
            HeadObjectDID = message.Payload.ReadUInt32();
            Character.HairTexture = message.Payload.ReadUInt32();
            Character.DefaultHairTexture = message.Payload.ReadUInt32();
            CharacterChangesDetected = true;
            EyesTextureDID = message.Payload.ReadUInt32();
            DefaultEyesTextureDID = message.Payload.ReadUInt32();
            NoseTextureDID = message.Payload.ReadUInt32();
            DefaultNoseTextureDID = message.Payload.ReadUInt32();
            MouthTextureDID = message.Payload.ReadUInt32();
            DefaultMouthTextureDID = message.Payload.ReadUInt32();
            SkinPaletteDID = message.Payload.ReadUInt32();
            HairPaletteDID = message.Payload.ReadUInt32();
            EyesPaletteDID = message.Payload.ReadUInt32();
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
            EnqueueBroadcast(new GameMessageObjDescEvent(this));
        }

        /// <summary>
        ///  Sends object description if the client requests it
        /// </summary>
        /// <param name="item"></param>
        public void HandleActionForceObjDescSend(ObjectGuid item)
        {
            WorldObject wo = GetInventoryItem(item);
            if (wo != null)
                EnqueueBroadcast(new GameMessageObjDescEvent(wo));
            else
                log.Debug($"HandleActionForceObjDescSend() - couldn't find inventory item {item}");
        }

        protected override void SendUpdatePosition(bool forcePos = false)
        {
            GameMessage msg = new GameMessageUpdatePosition(this, forcePos);
            Session.Network.EnqueueSend(msg);
            base.SendUpdatePosition();
        }



        /// <summary>
        /// This method is part of the contract tracking functions.   This is used to remove or abandon a contract.
        /// The method validates the id passed from the client against the portal.dat file, then sends the appropriate
        /// response to the client to remove the item from the quest panel. Og II
        /// </summary>
        /// <param name="contractId">This is the contract id passed to us from the client that we want to remove.</param>
        public void HandleActionAbandonContract(uint contractId)
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
            PlaySound(sound, Guid);
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

        public void HandleActionTalk(string message)
        {
            EnqueueBroadcast(new GameMessageCreatureMessage(message, Name, Guid.Full, ChatMessageType.Speech));
        }

        public void HandleActionEmote(string message)
        {
            EnqueueBroadcast(new GameMessageEmoteText(Guid.Full, Name, message));
        }

        public void HandleActionSoulEmote(string message)
        {
            EnqueueBroadcast(new GameMessageSoulEmote(Guid.Full, Name, message));
        }

        public void HandleActionJump(JumpPack jump)
        {
            var strength = GetCreatureAttribute(PropertyAttribute.Strength).Current;
            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, 0);     // TODO: augs
            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);

            // calculate stamina cost for this jump
            var staminaCost = MovementSystem.JumpStaminaCost(jump.Extent, burden, false);

            //Console.WriteLine($"Strength: {strength}, Capacity: {capacity}, Encumbrance: {EncumbranceVal ?? 0}, Burden: {burden}, StaminaCost: {staminaCost}");

            // TODO: ensure player has enough stamina to jump
            UpdateVitalDelta(Stamina, -staminaCost);
        }

        /// <summary>
        /// Called when the Player's stamina has recently changed to 0
        /// </summary>
        public void OnExhausted()
        {
            // adjust player speed if running
            if (CurrentMotionCommand == (uint)MotionCommand.RunForward)
            {
                var motion = new UniversalMotion(CurrentMotionState.Stance);
                // this should be autonomous, like retail, but if it's set to autonomous here, the desired effect doesn't happen
                // motion.IsAutonomous = true;
                motion.MovementData = new MovementData()
                {
                    CurrentStyle = (uint)CurrentMotionState.Stance,
                    ForwardCommand = (uint)MotionCommand.RunForward
                };
                CurrentMotionState = motion;
                if (CurrentLandblock != null)
                    EnqueueBroadcastMotion(motion);
            }
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You're Exhausted!"));
        }

        /// <summary>
        /// Method used to perform the animation, sound, and vital update on consumption of food or potions
        /// </summary>
        /// <param name="consumableName">Name of the consumable</param>
        /// <param name="sound">Either Sound.Eat1 or Sound.Drink1</param>
        /// <param name="buffType">ConsumableBuffType.Spell,ConsumableBuffType.Health,ConsumableBuffType.Stamina,ConsumableBuffType.Mana</param>
        /// <param name="boostAmount">Amount the Vital is boosted by; can be null, if buffType = ConsumableBuffType.Spell</param>
        /// <param name="spellDID">Id of the spell cast by the consumable; can be null, if buffType != ConsumableBuffType.Spell</param>
        public void ApplyConsumable(string consumableName, Sound sound, ConsumableBuffType buffType, uint? boostAmount, uint? spellDID)
        {
            MotionCommand motionCommand;

            if (sound == Sound.Eat1)
                motionCommand = MotionCommand.Eat;
            else
                motionCommand = MotionCommand.Drink;

            // start the eat/drink motion
            var motion = new UniversalMotion(MotionStance.NonCombat, new MotionItem(motionCommand));
            EnqueueBroadcastMotion(motion);

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var animTime = motionTable.GetAnimationLength(CurrentMotionState.Stance, motionCommand, MotionCommand.Ready);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);

            actionChain.AddAction(this, () =>
            {
                GameMessageSystemChat buffMessage;

                if (buffType == ConsumableBuffType.Spell)
                {
                    bool result = false;

                    uint spellId = spellDID ?? 0;

                    if (spellId != 0)
                        result = CreateSingleSpell(spellId);

                    if (result)
                    {
                        var spell = new Server.Entity.Spell(spellId);
                        buffMessage = new GameMessageSystemChat($"{consumableName} applies {spell.Name} on you.", ChatMessageType.Craft);
                    }
                    else
                        buffMessage = new GameMessageSystemChat($"Consuming {consumableName} attempted to apply a spell not yet fully implemented.", ChatMessageType.System);
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

                    var vitalChange = UpdateVitalDelta(creatureVital, (uint)boostAmount);

                    buffMessage = new GameMessageSystemChat($"You regain {vitalChange} {vitalName}.", ChatMessageType.Craft);
                }

                var soundEvent = new GameMessageSound(Guid, sound, 1.0f);
                Session.Network.EnqueueSend(soundEvent, buffMessage);

                // return to original stance
                var returnStance = new UniversalMotion(CurrentMotionState.Stance);
                returnStance.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;

                EnqueueBroadcastMotion(returnStance);
            });

           actionChain.EnqueueChain();
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
                CurrentLandblock?.ResendObjectsInRange(this);

            string state = Adminvision ? "enabled" : "disabled";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Admin Vision is {state}.", ChatMessageType.Broadcast));

            if (oldState != Adminvision && !Adminvision)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Note that you will need to log out and back in before the visible items become invisible again.", ChatMessageType.Broadcast));
            }
        }
    }
}
