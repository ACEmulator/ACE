using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database.Models.Auth;
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
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

using MotionTable = ACE.DatLoader.FileTypes.MotionTable;
using ACE.Database;

namespace ACE.Server.WorldObjects
{
    public partial class Player : Creature, IPlayer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Account Account { get; }

        public Character Character { get; }

        public Session Session { get; }

        public QuestManager QuestManager;

        public bool LastContact = true;
        public bool IsJumping = false;

        public SquelchDB Squelches;

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

            Account = DatabaseManager.Authentication.GetAccountById(Character.AccountId);

            SetEphemeralValues();

            // Make sure properties this WorldObject requires are not null.
            AvailableExperience = AvailableExperience ?? 0;
            TotalExperience = TotalExperience ?? 0;

            Attackable = true;

            SetProperty(PropertyString.DateOfBirth, $"{DateTime.UtcNow:dd MMMM yyyy}");
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Player(Biota biota, IEnumerable<Biota> inventory, IEnumerable<Biota> wieldedItems, Character character, Session session) : base(biota)
        {
            Character = character;
            Session = session;

            Account = DatabaseManager.Authentication.GetAccountById(Character.AccountId);

            SetEphemeralValues();

            SortBiotasIntoInventory(inventory);
            AddBiotasToEquippedObjects(wieldedItems);

            UpdateCoinValue(false);
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
            // This should be handled automatically...
            //PositionFlags |= PositionFlags.OrientationHasNoX | PositionFlags.OrientationHasNoY | PositionFlags.IsGrounded | PositionFlags.HasPlacementID;

            SetStance(MotionStance.NonCombat, false);

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

            if (Session != null && AdvocateQuest && IsAdvocate) // Advocate permissions are per character regardless of override
            {
                if (Session.AccessLevel == AccessLevel.Player)
                    Session.SetAccessLevel(AccessLevel.Advocate); // Elevate to Advocate permissions
                if (AdvocateLevel > 4)
                    IsPsr = true; // Enable AdvocateTeleport via MapClick
            }

            QuestManager = new QuestManager(this);

            LastUseTracker = new Dictionary<int, DateTime>();

            LootPermission = new Dictionary<ObjectGuid, DateTime>();

            Squelches = new SquelchDB();

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


        public MotionStance stance = MotionStance.NonCombat;

        public void ExamineObject(uint objectGuid)
        {
            // TODO: Throttle this request?. The live servers did this, likely for a very good reason, so we should, too.

            if (objectGuid == 0)
            {
                // Deselect the formerly selected Target
                // selectedTarget = ObjectGuid.Invalid;
                RequestedAppraisalTarget = null;
                CurrentAppraisalTarget = null;
                return;
            }

            var wo = FindObject(objectGuid, SearchLocations.Everywhere, out _, out _, out _);
            if (wo == null)
            {
                log.Warn($"{Name}.ExamineObject({objectGuid:X8}): couldn't find object");
                SendUseDoneEvent();
                return;
            }

            RequestedAppraisalTarget = objectGuid;
            CurrentAppraisalTarget = objectGuid;

            Examine(wo);
        }

        public void Examine(WorldObject obj)
        {
            var success = true;
            var creature = obj as Creature;
            Player player = null;

            if (creature != null)
            {
                player = obj as Player;
                var skill = player != null ? Skill.AssessPerson : Skill.AssessCreature;

                var currentSkill = (int)GetCreatureSkill(skill).Current;
                int difficulty = (int)creature.GetCreatureSkill(Skill.Deception).Current;

                var chance = SkillCheck.GetSkillChance(currentSkill, difficulty);

                if (difficulty == 0 || player != null && (!player.GetCharacterOption(CharacterOption.AttemptToDeceiveOtherPlayers) || player == this))
                    chance = 1.0f;

                success = chance >= ThreadSafeRandom.Next(0.0f, 1.0f);
            }
            Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, obj, success));

            if (!success && player != null)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} tried and failed to assess you!", ChatMessageType.Appraisal));

            // pooky logic - handle monsters attacking on appraisal
            if (creature != null && creature.MonsterState == State.Idle)
            {
                var tolerance = (Tolerance)(creature.GetProperty(PropertyInt.Tolerance) ?? 0);
                if (tolerance.HasFlag(Tolerance.Appraise))
                {
                    creature.AttackTarget = this;
                    creature.WakeUp();
                }
            }
        }

        public override void OnCollideEnvironment()
        {
            //HandleFallingDamage();
        }

        public override void OnCollideObject(WorldObject target)
        {
            if (target.ReportCollisions == false)
                return;

            if (target is Portal portal)
                portal.OnCollideObject(this);
            else if (target is PressurePlate pressurePlate)
                pressurePlate.OnCollideObject(this);
            else if (target is Hotspot hotspot)
                hotspot.OnCollideObject(this);
        }

        public override void OnCollideObjectEnd(WorldObject target)
        {
            if (target is Hotspot hotspot)
                hotspot.OnCollideObjectEnd(this);
        }

        public void HandleActionQueryHealth(uint objectGuid)
        {
            if (objectGuid == 0)
            {
                // Deselect the formerly selected Target
                selectedTarget = ObjectGuid.Invalid;
                HealthQueryTarget = null;
                return;
            }

            // Remember the selected Target
            selectedTarget = new ObjectGuid(objectGuid);
            HealthQueryTarget = objectGuid;
            var obj = CurrentLandblock?.GetObject(objectGuid);
            if (obj != null)
                obj.QueryHealth(Session);
        }

        public void HandleActionQueryItemMana(uint itemGuid)
        {
            if (itemGuid == 0)
            {
                ManaQueryTarget = null;
                return;
            }

            // the object could be in the world or on the player, first check player
            var item = GetInventoryItem(itemGuid) ?? GetEquippedItem(itemGuid);

            if (item != null)
                item.QueryItemMana(Session);

            ManaQueryTarget = itemGuid;
        }

        public void ReadBookPage(uint bookGuid, uint pageNum)
        {
            // TODO: Do we want to throttle this request, like appraisals?
            // The object can be in two spots... on the player or on the landblock
            // First check the player
            WorldObject wo = GetInventoryItem(bookGuid);
            // book is in the player's inventory...
            if (wo != null)
            {
                wo.ReadBookPage(Session, pageNum);
            }
            else
            {
                CurrentLandblock?.GetObject(bookGuid).ReadBookPage(Session, pageNum);
            }
        }

        public void HandleActionBookAddPage(uint bookGuid)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var page = book.AddPage(Guid.Full, Name, Session.Account, false, "");

            if (page != null)
                Session.Network.EnqueueSend(new GameEventBookAddPageResponse(Session, bookGuid, page.PageId, true));
        }

        public void HandleActionBookModifyPage(uint bookGuid, uint pageId, string pageText)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            book.ModifyPage(pageId, pageText);
        }

        public void HandleActionBookDeletePage(uint bookGuid, uint pageId)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var success = book.DeletePage(pageId);

            Session.Network.EnqueueSend(new GameEventBookDeletePageResponse(Session, bookGuid, pageId, success));
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
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        public void LogOut(bool clientSessionTerminatedAbruptly = false)
        {
            if (Fellowship != null)
                FellowshipQuit(false);

            if (!clientSessionTerminatedAbruptly)
            {
                // Thie retail server sends a ChatRoomTracker 0x0295 first, then the status message, 0x028B. It does them one at a time for each individual channel.
                // The ChatRoomTracker message doesn't seem to change at all.
                // For the purpose of ACE, we simplify this process.
                var general = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "General");
                var trade = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "Trade");
                var lfg = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "LFG");
                var roleplay = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, "Roleplay");
                Session.Network.EnqueueSend(general, trade, lfg, roleplay);
            }

            if (CurrentActiveCombatPet != null)
                CurrentActiveCombatPet.Destroy();

            if (CurrentLandblock != null)
            {
                var logout = new Motion(MotionStance.NonCombat, MotionCommand.LogOut);
                EnqueueBroadcastMotion(logout);

                EnqueueBroadcastPhysicsState();

                var logoutChain = new ActionChain();

                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)MotionTableId);
                float logoutAnimationLength = motionTable.GetAnimationLength(MotionCommand.LogOut);
                logoutChain.AddDelaySeconds(logoutAnimationLength);

                // remove the player from landblock management -- after the animation has run
                logoutChain.AddAction(this, () =>
                {
                    CurrentLandblock?.RemoveWorldObject(Guid, false);
                    SetPropertiesAtLogOut();
                    SavePlayerToDatabase();
                    PlayerManager.SwitchPlayerFromOnlineToOffline(this);
                });

                // close any open chests
                if (LastOpenedContainerId != ObjectGuid.Invalid)
                {
                    var chest = CurrentLandblock.GetObject(LastOpenedContainerId) as Chest;

                    if (chest != null)
                        chest.Close(this);
                }

                logoutChain.EnqueueChain();
            }
            else
            {
                SetPropertiesAtLogOut();
                SavePlayerToDatabase();
                PlayerManager.SwitchPlayerFromOnlineToOffline(this);
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
            BarberActive = false;
        }

        /// <summary>
        ///  Sends object description if the client requests it
        /// </summary>
        public void HandleActionForceObjDescSend(uint itemGuid)
        {
            var wo = FindObject(itemGuid, SearchLocations.Everywhere);
            if (wo == null)
            {
                log.Debug($"HandleActionForceObjDescSend() - couldn't find object {itemGuid:X8}");
                return;
            }
            Session.Network.EnqueueSend(new GameMessageObjDescEvent(wo));
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
            EnqueueBroadcast(new GameMessageCreatureMessage(message, Name, Guid.Full, ChatMessageType.Speech), LocalBroadcastRange, true);
        }

        public void HandleActionEmote(string message)
        {
            EnqueueBroadcast(new GameMessageEmoteText(Guid.Full, Name, message), LocalBroadcastRange);
        }

        public void HandleActionSoulEmote(string message)
        {
            EnqueueBroadcast(new GameMessageSoulEmote(Guid.Full, Name, message), LocalBroadcastRange);
        }

        public void HandleActionJump(JumpPack jump)
        {
            StartJump = new ACE.Entity.Position(Location);

            var strength = Strength.Current;
            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, AugmentationIncreasedCarryingCapacity);
            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);

            // calculate stamina cost for this jump
            var extent = Math.Clamp(jump.Extent, 0.0f, 1.0f);
            var staminaCost = MovementSystem.JumpStaminaCost(extent, burden, false);

            //Console.WriteLine($"Strength: {strength}, Capacity: {capacity}, Encumbrance: {EncumbranceVal ?? 0}, Burden: {burden}, StaminaCost: {staminaCost}");

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

            IsJumping = true;

            UpdateVitalDelta(Stamina, -staminaCost);

            IsJumping = false;

            //Console.WriteLine($"Jump velocity: {jump.Velocity}");

            // set jump velocity
            // TODO: have server verify / scale magnitude
            PhysicsObj.set_velocity(jump.Velocity, true);

            // this shouldn't be needed, but without sending this update motion / simulated movement event beforehand,
            // running forward and then performing a charged jump does an uncharged shallow arc jump instead
            // this hack fixes that...
            var movementData = new MovementData(this);
            movementData.IsAutonomous = true;
            movementData.MovementType = MovementType.Invalid;
            movementData.Invalid = new MovementInvalid(movementData);
            EnqueueBroadcast(new GameMessageUpdateMotion(this, movementData));

            // broadcast jump
            EnqueueBroadcast(new GameMessageVectorUpdate(this));
        }

        /// <summary>
        /// Called when the Player's stamina has recently changed to 0
        /// </summary>
        public void OnExhausted()
        {
            // adjust player speed if running
            if (CurrentMotionCommand == MotionCommand.RunForward && !IsJumping)
            {
                // verify - forced commands from server should be non-autonomous, but could have been sent as autonomous in retail?
                // if set to autonomous here, the desired effect doesn't happen
                // motion.IsAutonomous = true;
                var motion = new Motion(this, MotionCommand.RunForward);

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
            var motion = new Motion(MotionStance.NonCombat, motionCommand);
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
                        buffMessage = new GameMessageSystemChat($"{consumableName} casts {spell.Name} on you.", ChatMessageType.Magic);
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
                    if (vitalName == "Health")
                    {
                        DamageHistory.OnHeal((uint)vitalChange);
                        if (Fellowship != null)
                            Fellowship.OnVitalUpdate(this);
                    }

                    buffMessage = new GameMessageSystemChat($"You regain {vitalChange} {vitalName}.", ChatMessageType.Craft);
                }

                var soundEvent = new GameMessageSound(Guid, sound, 1.0f);
                EnqueueBroadcast(soundEvent);

                Session.Network.EnqueueSend(buffMessage);

                // return to original stance
                var returnStance = new Motion(CurrentMotionState.Stance);
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

            // send CO network messages for admin objects
            if (Adminvision && oldState != Adminvision)
            {
                var adminObjs = PhysicsObj.ObjMaint.ObjectTable.Values.Where(o => o.WeenieObj.WorldObject.Visibility);
                PhysicsObj.enqueue_objs(adminObjs);

                // sending DO network messages for /adminvision off here doesn't work in client unfortunately?
            }

            string state = Adminvision ? "enabled" : "disabled";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Admin Vision is {state}.", ChatMessageType.Broadcast));

            if (oldState != Adminvision && !Adminvision)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Note that you will need to log out and back in before the visible items become invisible again.", ChatMessageType.Broadcast));
            }
        }

        public void SendMessage(string msg)
        {
            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        public void HandleActionEnterPkLite()
        {
            // ensure permanent npk
            if (PlayerKillerStatus != PlayerKillerStatus.NPK || MinimumTimeSincePk != null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyNonPKsMayEnterPKLite));
                return;
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is looking for a fight!", ChatMessageType.Broadcast));

            // perform pk lite entry motion / effect
            var motion = new Motion(MotionStance.NonCombat, MotionCommand.EnterPKLite);
            EnqueueBroadcastMotion(motion);

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var animLength = motionTable.GetAnimationLength(MotionStance.NonCombat, MotionCommand.EnterPKLite);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () =>
            {
                UpdateProperty(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.PKLite);

                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNowPKLite));
            });

            actionChain.EnqueueChain();
        }

        public void HandleActionModifyCharacterSquelch(bool squelch, uint playerGuid, string playerName, ChatMessageType messageType)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyCharacterSquelch({squelch}, {playerGuid:X8}, {playerName}, {messageType})");

            IPlayer player;

            if (playerGuid != 0)
            {
                player = PlayerManager.FindByGuid(new ObjectGuid(playerGuid));

                if (player == null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Couldn't find player to squelch.", ChatMessageType.Broadcast));
                    return;
                }
            }
            else
            {
                player = PlayerManager.FindByName(playerName);

                if (player == null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found.", ChatMessageType.Broadcast));
                    return;
                }
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You can't squelch yourself!", ChatMessageType.Broadcast));
                return;
            }

            if (squelch)
            {
                if (Squelches.Characters.ContainsKey(player.Guid))
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already squelched.", ChatMessageType.Broadcast));
                    return;
                }

                Squelches.Characters.Add(player.Guid, new SquelchInfo(messageType, player.Name, false));

                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been squelched.", ChatMessageType.Broadcast));
            }
            else
            {
                if (!Squelches.Characters.Remove(player.Guid))
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not squelched.", ChatMessageType.Broadcast));
                    return;
                }

                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been unsquelched.", ChatMessageType.Broadcast));
            }

            Session.Network.EnqueueSend(new GameEventSetSquelchDB(Session, Squelches));
        }

        public void HandleActionModifyAccountSquelch(bool squelch, string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyAccountSquelch({squelch}, {playerName})");

            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found.", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You can't squelch yourself!", ChatMessageType.Broadcast));
                return;
            }

            if (squelch)
            {
                if (Squelches.Accounts.ContainsKey(player.Session.Account))
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account is already squelched.", ChatMessageType.Broadcast));
                    return;
                }

                Squelches.Accounts.Add(player.Session.Account, player.Guid.Full);

                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account has been squelched.", ChatMessageType.Broadcast));
            }
            else
            {
                if (!Squelches.Accounts.Remove(player.Session.Account))
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account is not squelched.", ChatMessageType.Broadcast));
                    return;
                }

                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account has been unsquelched.", ChatMessageType.Broadcast));
            }

            Session.Network.EnqueueSend(new GameEventSetSquelchDB(Session, Squelches));
        }

        public void HandleActionModifyGlobalSquelch(bool squelch, ChatMessageType messageType)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyGlobalSquelch({squelch}, {messageType})");
        }
    }
}
