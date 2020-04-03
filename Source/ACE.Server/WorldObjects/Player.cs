using System;
using System.Collections.Generic;
using System.Numerics;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects.Managers;

using Character = ACE.Database.Models.Shard.Character;
using MotionTable = ACE.DatLoader.FileTypes.MotionTable;

namespace ACE.Server.WorldObjects
{
    public partial class Player : Creature, IPlayer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Account Account { get; }

        public Character Character { get; }

        public Session Session { get; }

        public ContractManager ContractManager;

        public bool LastContact = true;
        public bool IsJumping = false;

        public DateTime LastJumpTime;

        public ACE.Entity.Position LastGroundPos;
        public ACE.Entity.Position SnapPos;

        public ConfirmationManager ConfirmationManager;

        public SquelchManager SquelchManager;

        public static readonly float MaxRadarRange_Indoors = 25.0f;
        public static readonly float MaxRadarRange_Outdoors = 75.0f;

        public DateTime PrevObjSend;

        public float CurrentRadarRange => Location.Indoors ? MaxRadarRange_Indoors : MaxRadarRange_Outdoors;

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
        public Player(Biota biota, IEnumerable<ACE.Database.Models.Shard.Biota> inventory, IEnumerable<ACE.Database.Models.Shard.Biota> wieldedItems, Character character, Session session) : base(biota)
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
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Player;

            // This is the default send upon log in and the most common. Anything with a velocity will need to add that flag.
            // This should be handled automatically...
            //PositionFlags |= PositionFlags.OrientationHasNoX | PositionFlags.OrientationHasNoY | PositionFlags.IsGrounded | PositionFlags.HasPlacementID;

            FirstEnterWorldDone = false;

            SetStance(MotionStance.NonCombat, false);

            // radius for object updates
            ListeningRadius = 5f;

            if (Session != null && Common.ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (Session.AccessLevel == AccessLevel.Admin)
                    IsAdmin = true;
                if (Session.AccessLevel == AccessLevel.Developer)
                    IsArch = true;
                if (Session.AccessLevel == AccessLevel.Envoy || Session.AccessLevel == AccessLevel.Sentinel)
                    IsSentinel = true;
                if (Session.AccessLevel == AccessLevel.Advocate)
                    IsAdvocate = true;
            }

            ContainerCapacity = (byte)(7 + AugmentationExtraPackSlot);

            if (Session != null && AdvocateQuest && IsAdvocate) // Advocate permissions are per character regardless of override
            {
                if (Session.AccessLevel == AccessLevel.Player)
                    Session.SetAccessLevel(AccessLevel.Advocate); // Elevate to Advocate permissions
                if (AdvocateLevel > 4)
                    IsPsr = true; // Enable AdvocateTeleport via MapClick
            }

            CombatTable = DatManager.PortalDat.ReadFromDat<CombatManeuverTable>(CombatTableDID.Value);

            _questManager = new QuestManager(this);

            ContractManager = new ContractManager(this);

            ConfirmationManager = new ConfirmationManager(this);

            LootPermission = new Dictionary<ObjectGuid, DateTime>();

            SquelchManager = new SquelchManager(this);

            MagicState = new MagicState(this);

            RecordCast = new RecordCast(this);

            AttackQueue = new AttackQueue(this);

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
                //    character.IsSentinel = true;
                // if (Session.AccessLevel == AccessLevel.Advocate)
                //    character.IsAdvocate= true;
            }*/

            // FirstEnterWorldDone = false;

            // IsAlive = true;
        }

        public bool IsDeleted => Character.IsDeleted;
        public bool IsPendingDeletion => Character.DeleteTime > 0 && !IsDeleted;


        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        public MotionStance stance = MotionStance.NonCombat;

        /// <summary>
        /// Called when player presses the 'e' key to appraise an object
        /// </summary>
        public void HandleActionIdentifyObject(uint objectGuid)
        {
            //Console.WriteLine($"{Name}.HandleActionIdentifyObject({objectGuid:X8})");

            if (objectGuid == 0)
            {
                // Deselect the formerly selected Target
                //selectedTarget = ObjectGuid.Invalid;
                RequestedAppraisalTarget = null;
                CurrentAppraisalTarget = null;
                return;
            }

            var wo = FindObject(objectGuid, SearchLocations.Everywhere, out _, out _, out _);

            if (wo == null)
            {
                log.Debug($"{Name}.HandleActionIdentifyObject({objectGuid:X8}): couldn't find object");
                Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, objectGuid));
                return;
            }

            var currentTime = Time.GetUnixTime();

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

            RequestedAppraisalTarget = objectGuid;
            AppraisalRequestedTimestamp = currentTime;

            Examine(wo);
        }

        public void Examine(WorldObject obj)
        {
            //Console.WriteLine($"{Name}.Examine({obj.Name})");

            var success = true;
            var creature = obj as Creature;
            Player player = null;

            if (creature != null)
            {
                player = obj as Player;
                var skill = player != null ? Skill.AssessPerson : Skill.AssessCreature;

                var currentSkill = (int)GetCreatureSkill(skill).Current;
                int difficulty = (int)creature.GetCreatureSkill(Skill.Deception).Current;

                if (PropertyManager.GetBool("assess_creature_mod").Item && skill == Skill.AssessCreature
                        && Skills[Skill.AssessCreature].AdvancementClass < SkillAdvancementClass.Trained)
                    currentSkill = (int)((Focus.Current + Self.Current) / 2);

                var chance = SkillCheck.GetSkillChance(currentSkill, difficulty);

                if (difficulty == 0 || player == this || player != null && !player.GetCharacterOption(CharacterOption.AttemptToDeceiveOtherPlayers))
                    chance = 1.0f;

                if ((this is Admin || this is Sentinel) && CloakStatus == CloakStatus.On)
                    chance = 1.0f;

                success = chance >= ThreadSafeRandom.Next(0.0f, 1.0f);
            }

            if (creature is Pet || creature is CombatPet)
                success = true;

            if (success)
                CurrentAppraisalTarget = obj.Guid.Full;

            Session.Network.EnqueueSend(new GameEventIdentifyObjectResponse(Session, obj, success));

            OnAppraisal(obj, success);
        }

        public void OnAppraisal(WorldObject obj, bool success)
        {
            if (!success && obj is Player player && !player.SquelchManager.Squelches.Contains(this, ChatMessageType.Appraisal))
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} tried and failed to assess you!", ChatMessageType.Appraisal));

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
                UpdateSelectedTarget(null);
                return;
            }

            var obj = CurrentLandblock?.GetObject(objectGuid) as Creature;

            if (obj == null)
            {
                // Deselect the formerly selected Target
                UpdateSelectedTarget(null);
                return;
            }

            UpdateSelectedTarget(obj);

            obj.QueryHealth(Session);
        }

        private void UpdateSelectedTarget(Creature target)
        {
            if (selectedTarget != null)
            {
                var prevSelected = selectedTarget.TryGetWorldObject() as Creature;

                if (prevSelected != null)
                    prevSelected.OnTargetDeselected(this);
            }

            if (target != null)
            {
                selectedTarget = new WorldObjectInfo(target);
                HealthQueryTarget = target.Guid.Full;

                target.OnTargetSelected(this);
            }
            else
            {
                selectedTarget = null;
                HealthQueryTarget = null;
            }
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
        /// Returns TRUE if a Player Killer has clicked logout after being involved in a PK battle
        /// within the past 2 mins.
        /// The server delays the logout for 20s, and the client remains in frozen state during this delay
        /// </summary>
        public bool PKLogout;

        /// <summary>
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        public bool LogOut(bool clientSessionTerminatedAbruptly = false, bool forceImmediate = false)
        {
            if (PKLogoutActive && !forceImmediate)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                Session.Network.EnqueueSend(new GameMessageSystemChat("Logging out in 20s...", ChatMessageType.Magic));

                if (!PKLogout)
                {
                    PKLogout = true;

                    LogoffTimestamp = Time.GetFutureUnixTime(PropertyManager.GetLong("pk_timer").Item);
                    PlayerManager.AddPlayerToLogoffQueue(this);
                }
                return false;
            }

            LogOut_Inner(clientSessionTerminatedAbruptly);

            return true;
        }

        public void LogOut_Inner(bool clientSessionTerminatedAbruptly = false)
        {
            if (Fellowship != null)
                FellowshipQuit(false);

            if (IsTrading && TradePartner != null)
            {
                var tradePartner = PlayerManager.GetOnlinePlayer(TradePartner);

                if (tradePartner != null)
                    tradePartner.HandleActionCloseTradeNegotiations();
            }

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

            if (CurrentActivePet != null)
                CurrentActivePet.Destroy();

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
                    if (CurrentLandblock == null)
                    {
                        log.Debug($"0x{Guid}:{Name}.LogOut_Inner.logoutChain: CurrentLandblock is null, unable to remove from a landblock...");
                        if (Location != null)
                            log.Debug($"0x{Guid}:{Name}.LogOut_Inner.logoutChain: Location is not null, Location = {Location.ToLOCString()}");
                    }

                    CurrentLandblock?.RemoveWorldObject(Guid, false);
                    SetPropertiesAtLogOut();
                    SavePlayerToDatabase();
                    PlayerManager.SwitchPlayerFromOnlineToOffline(this);
                });

                // close any open landblock containers (chests / corpses)
                if (LastOpenedContainerId != ObjectGuid.Invalid)
                {
                    var container = CurrentLandblock.GetObject(LastOpenedContainerId) as Container;

                    if (container != null)
                        container.Close(this);
                }

                logoutChain.EnqueueChain();
            }
            else
            {
                log.Debug($"0x{Guid}:{Name}.LogOut_Inner: CurrentLandblock is null");
                if (Location != null)
                {
                    log.Debug($"0x{Guid}:{Name}.LogOut_Inner: Location is not null, Location = {Location.ToLOCString()}");
                    var validLoadedLandblock = LandblockManager.GetLandblock(Location.LandblockId, false);
                    if (validLoadedLandblock.GetObject(Guid.Full) != null)
                    {
                        log.Debug($"0x{Guid}:{Name}.LogOut_Inner: Player is still on landblock, removing...");
                        validLoadedLandblock.RemoveWorldObject(Guid, false);
                    }
                    else
                        log.Debug($"0x{Guid}:{Name}.LogOut_Inner: Player is not found on the landblock Location references.");
                }
                else
                    log.Debug($"0x{Guid}:{Name}.LogOut_Inner: Location is null");
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

            // The EnqueueBroadcastUpdateObject below sends the player back into teleport. I assume at this point, this was never done to players
            // EnqueueBroadcastUpdateObject();

            // The private message below worked as expected, but it only broadcast to the player. This would be a problem with for others in range seeing something try to
            // pass through a barrier but not being allowed.
            // var updateBool = new GameMessagePrivateUpdatePropertyBool(Session, PropertyBool.IgnoreHouseBarriers, ImmuneCellRestrictions);
            // Session.Network.EnqueueSend(updateBool);

            UpdateProperty(this, PropertyBool.IgnoreHouseBarriers, !IgnoreHouseBarriers, true);

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
            if (!IsGagged)
                EnqueueBroadcast(new GameMessageCreatureMessage(message, Name, Guid.Full, ChatMessageType.Speech), LocalBroadcastRange, ChatMessageType.Speech);
            else
                SendGagError();
        }

        public void SendGagError()
        {
            var msg = "You are unable to talk locally, globally, or send tells because you have been gagged.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendGagNotice()
        {
            var msg = "Your chat privileges have been suspended.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        public void SendUngagNotice()
        {
            var msg = "Your chat privileges have been restored.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        public void HandleActionEmote(string message)
        {
            if (!IsGagged)
                EnqueueBroadcast(new GameMessageEmoteText(Guid.Full, Name, message), LocalBroadcastRange);
            else
                SendGagError();
        }

        public void HandleActionSoulEmote(string message)
        {
            if (!IsGagged)
                EnqueueBroadcast(new GameMessageSoulEmote(Guid.Full, Name, message), LocalBroadcastRange);
            else
                SendGagError();
        }

        public void HandleActionJump(JumpPack jump)
        {
            StartJump = new ACE.Entity.Position(Location);
            //Console.WriteLine($"JumpPack: Velocity: {jump.Velocity}, Extent: {jump.Extent}");

            var strength = Strength.Current;
            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, AugmentationIncreasedCarryingCapacity);
            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);

            // calculate stamina cost for this jump
            var extent = Math.Clamp(jump.Extent, 0.0f, 1.0f);
            var staminaCost = MovementSystem.JumpStaminaCost(extent, burden, PKTimerActive);

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
            LastJumpTime = DateTime.UtcNow;

            UpdateVitalDelta(Stamina, -staminaCost);

            IsJumping = false;

            //Console.WriteLine($"Jump velocity: {jump.Velocity}");

            // TODO: have server verify / scale magnitude
            if (FastTick)
            {
                if (!PhysicsObj.IsMovingOrAnimating)
                    //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - Physics.PhysicsGlobals.MinQuantum;
                    PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

                // perform jump in physics engine
                PhysicsObj.TransientState &= ~(Physics.TransientStateFlags.Contact | Physics.TransientStateFlags.WaterContact);
                PhysicsObj.calc_acceleration();
                PhysicsObj.set_on_walkable(false);
                PhysicsObj.set_local_velocity(jump.Velocity, false);

                if (CombatMode == CombatMode.Magic && MagicState.IsCasting)
                    FailCast();
            }
            else
            {
                PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

                // set jump velocity
                //var glob_velocity = Vector3.Transform(jump.Velocity, Location.Rotation);
                //PhysicsObj.set_velocity(glob_velocity, true);

                // perform jump in physics engine
                PhysicsObj.TransientState &= ~(Physics.TransientStateFlags.Contact | Physics.TransientStateFlags.WaterContact);
                PhysicsObj.calc_acceleration();
                PhysicsObj.set_on_walkable(false);
                PhysicsObj.set_local_velocity(jump.Velocity, false);
            }

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

            if (MagicState.IsCasting && RecordCast.Enabled)
                RecordCast.OnJump(jump);
        }

        /// <summary>
        /// Called when the Player's stamina has recently changed to 0
        /// </summary>
        public void OnExhausted()
        {
            // adjust player speed if they are currently pressing movement keys
            HandleRunRateUpdate();

            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You're Exhausted!"));
        }

        /// <summary>
        /// Detects changes in the player's RunRate --
        /// If there are changes, re-broadcasts player movement packet
        /// </summary>
        public bool HandleRunRateUpdate()
        {
            //Console.WriteLine($"{Name}.HandleRunRateUpdates()");

            if (CurrentMovementData.MovementType != MovementType.Invalid || CurrentMovementData.Invalid == null)
                return false;

            var prevState = CurrentMovementData.Invalid.State;

            var movementData = new MovementData(this, CurrentMoveToState);
            var currentState = movementData.Invalid.State;

            var changed = currentState.ForwardSpeed  != prevState.ForwardSpeed ||
                          currentState.TurnSpeed     != prevState.TurnSpeed ||
                          currentState.SidestepSpeed != prevState.SidestepSpeed;

            if (!changed)
                return false;

            //Console.WriteLine($"Old: {prevState.ForwardSpeed}, New: {currentState.ForwardSpeed}");

            if (!CurrentMovementData.Invalid.State.HasMovement() || IsJumping)
                return false;

            //Console.WriteLine($"{Name}.OnRunRateChanged()");

            CurrentMovementData = new MovementData(this, CurrentMoveToState);

            // verify - forced commands from server should be non-autonomous, but could have been sent as autonomous in retail?
            // if set to autonomous here, the desired effect doesn't happen
            CurrentMovementData.IsAutonomous = false;

            var movementEvent = new GameMessageUpdateMotion(this, CurrentMovementData);
            EnqueueBroadcast(movementEvent);    // broadcast to all players, including self

            return true;
        }

        /// <summary>
        /// Returns a modifier for a player's Run, Jump, Melee Defense, and Missile Defense skills if they are overburdened
        /// </summary>
        public override float GetBurdenMod()
        {
            var strength = Strength.Current;

            var capacity = EncumbranceSystem.EncumbranceCapacity((int)strength, AugmentationIncreasedCarryingCapacity);

            var burden = EncumbranceSystem.GetBurden(capacity, EncumbranceVal ?? 0);

            var burdenMod = EncumbranceSystem.GetBurdenMod(burden);

            //Console.WriteLine($"Burden mod: {burdenMod}");

            return burdenMod;
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

        public void SendMessage(string msg, ChatMessageType type = ChatMessageType.Broadcast, WorldObject source = null)
        {
            if (SquelchManager.IsLegalChannel(type) && SquelchManager.Squelches.Contains(source, type))
                return;

            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, type));
        }

        public void HandleActionEnterPkLite()
        {
            // ensure permanent npk
            if (PlayerKillerStatus != PlayerKillerStatus.NPK || MinimumTimeSincePk != null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyNonPKsMayEnterPKLite));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is looking for a fight!", ChatMessageType.Broadcast), LocalBroadcastRange);

            // perform pk lite entry motion / effect

            IsBusy = true;

            var prevStance = CurrentMotionState.Stance;

            var actionChain = new ActionChain();

            var animTime = 0.0f;

            animTime += EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.EnterPKLite);

            actionChain.AddAction(this, () =>
            {
                UpdateProperty(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.PKLite, true);

                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNowPKLite));
            });

            // return to previous stance, if applicable
            if (prevStance != MotionStance.NonCombat)
                animTime += EnqueueMotion_Force(actionChain, prevStance, MotionCommand.Ready, MotionCommand.NonCombat);

            actionChain.AddAction(this, () => IsBusy = false);

            actionChain.EnqueueChain();
        }
    }
}
