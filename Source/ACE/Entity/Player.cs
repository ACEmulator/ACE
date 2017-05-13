﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Entity.Events;
using ACE.Network.Sequence;
using System.Collections.Concurrent;
using ACE.Network.GameAction;
using ACE.Network.Motion;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.DatLoader;
using ACE.Factories;
using ACE.StateMachines.Enum;

using log4net;

namespace ACE.Entity
{

    public sealed class Player : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Session Session { get; }

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
        public uint SelectedTarget { get; set; }

        /// <summary>
        /// Amount of times this character has left a portal this session
        /// </summary>
        public uint PortalIndex { get; set; } = 1u;

        /// <summary>
        /// tick-stamp for the server time of the last time the player changed state (combat state?)
        /// </summary>
        public double LastStateChangeTicks { get; set; }

        /// <summary>
        /// Last Streaming Object change tick
        /// </summary>
        public double LastStreamingObjectChange { get; set; }

        /// <summary>
        /// Level of the player
        /// </summary>
        public uint Level
        {
            get { return character.Level; }
        }

        private Character character;

        private readonly object clientObjectMutex = new object();

        private readonly Dictionary<ObjectGuid, double> clientObjectList = new Dictionary<ObjectGuid, double>();

        // queue of all the "actions" that come from the player that require processing
        // asynchronous to or outside of the network thread
        private readonly ConcurrentQueue<QueuedGameAction> actionQueue = new ConcurrentQueue<QueuedGameAction>();

        // examination queue is really a subset of the actionQueue, but this existed on
        // retail servers as it's own separate thing and was intentionally throttled.
        private readonly ConcurrentQueue<QueuedGameAction> examinationQueue = new ConcurrentQueue<QueuedGameAction>();

        private readonly object delayedActionsMutex = new object();

        // dictionary for delaying further actions for an objectID
        private readonly Dictionary<uint, double> delayedActions = new Dictionary<uint, double>();

        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions
        {
            get { return character.CharacterOptions; }
        }

        public ReadOnlyDictionary<PositionType, Position> Positions
        {
            get { return character.Positions; }
        }

        public ReadOnlyCollection<Friend> Friends
        {
            get { return character.Friends; }
        }

        public bool IsAdmin
        {
            get { return character.IsAdmin; }
            set { character.IsAdmin = value; }
        }

        public bool IsEnvoy
        {
            get { return character.IsEnvoy; }
            set { character.IsEnvoy = value; }
        }

        public bool IsArch
        {
            get { return character.IsArch; }
            set { character.IsArch = value; }
        }

        public bool IsPsr
        {
            get { return character.IsPsr; }
            set { character.IsPsr = value; }
        }

        public ReadOnlyDictionary<PropertyBool, bool> PropertiesBool
        {
            get { return character.PropertiesBool; }
        }

        public ReadOnlyDictionary<PropertyDouble, double> PropertiesDouble
        {
            get { return character.PropertiesDouble; }
        }

        public ReadOnlyDictionary<PropertyInt, uint> PropertiesInt
        {
            get { return character.PropertiesInt; }
        }

        public ReadOnlyDictionary<PropertyInt64, ulong> PropertiesInt64
        {
            get { return character.PropertiesInt64; }
        }

        public ReadOnlyDictionary<PropertyString, string> PropertiesString
        {
            get { return character.PropertiesString; }
        }

        public ReadOnlyDictionary<Skill, CharacterSkill> Skills
        {
            get { return new ReadOnlyDictionary<Skill, CharacterSkill>(character.Skills); }
        }

        public uint TotalLogins
        {
            get { return character.TotalLogins; }
            set { character.TotalLogins = value; }
        }

        /// <summary>
        /// This signature services MoveToObject and TurnToObject
        /// Update Position prior to start, start them moving or turning, set statemachine to moving.
        /// </summary>
        /// <param name="worldObjectPosition"></param>
        /// <param name="sequence"></param>
        /// <param name="movementType"></param>
        /// <returns>MovementStates</returns>
        public MovementStates OnAutonomousMove(Position worldObjectPosition, SequenceManager sequence, MovementTypes movementType, ObjectGuid targetGuid)
        {
            var newMotion = new UniversalMotion(MotionStance.Standing, worldObjectPosition, targetGuid);
            newMotion.DistanceFrom = 0.60f;
            newMotion.MovementTypes = MovementTypes.MoveToObject;
            Session.Network.EnqueueSend(new GameMessageUpdatePosition(this));
            Session.Network.EnqueueSend(new GameMessageUpdateMotion(Guid,
                                        Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                        sequence, newMotion));
            CreatureMovementStates = MovementStates.Moving;
            return MovementStates.Moving;
        }

        public void UpdateAutonomousMove()
        {
            if ((Math.Abs(PhysicsData.Position.SquaredDistanceTo(MoveToPosition)) <= ArrivedRadiusSquared))
            {
                // We have arrived
                CreatureMovementStates = MovementStates.Arrived;
                if (BlockedGameAction != null)
                {
                    AddToActionQueue(BlockedGameAction);
                    BlockedGameAction = null;
                }
                // Clean up
                ClearDestinationInformation();
                CreatureMovementStates = MovementStates.Idle;
            }
        }

        public void NotifyAndDropItem(ObjectGuid inventoryId)
        {
            var inventoryItem = GetInventoryItem(inventoryId);
            if (inventoryItem == null)
                return;
            RemoveFromInventory(inventoryId);
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbVal, GameData.Burden));

            var motion = new UniversalMotion(MotionStance.Standing);
            motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
            var clearContainer = new ObjectGuid(0);
            Session.Network.EnqueueSend(new GameMessageUpdateMotion(Guid,
                                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                            Sequences, motion),
                new GameMessageUpdateInstanceId(inventoryId, clearContainer));

            motion = new UniversalMotion(MotionStance.Standing);
            Session.Network.EnqueueSend(new GameMessageUpdateMotion(Guid,
                                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                            Sequences, motion),
                new GameMessagePutObjectIn3d(Session, this, inventoryId),
                new GameMessageSound(Guid, Sound.DropItem, (float)1.0),
                new GameMessageUpdateInstanceId(inventoryId, clearContainer));

            // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            LandblockManager.AddObject(inventoryItem);

            // This may not be needed when we fix landblock update object -
            // TODO: Og II - check this later to see if it is still required.
            Session.Network.EnqueueSend(new GameMessageUpdateObject(inventoryItem));
            // Session.Network.EnqueueSend(new GameMessageUpdatePosition(inventoryItem));
        }

        public void NotifyAndAddToInventory(WorldObject inventoryItem)
        {
            var motion = new UniversalMotion(MotionStance.Standing);
            motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
            Session.Network.EnqueueSend(new GameMessageUpdatePosition(this),
                new GameMessageUpdateMotion(Guid,
                                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                            Sequences, motion),
                new GameMessageSound(Guid, Sound.PickUpItem, (float)1.0));

            // Add to the inventory list.
            AddToInventory(inventoryItem);

            motion = new UniversalMotion(MotionStance.Standing);
            Session.Network.EnqueueSend(
                    new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbVal, GameData.Burden),
                    new GameMessagePutObjectInContainer(Session, this, inventoryItem.Guid),
                    new GameMessageUpdateMotion(Guid,
                                                Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                Sequences, motion),
                    new GameMessageUpdateInstanceId(inventoryItem.Guid, Guid),
                    new GameMessagePickupEvent(Session, inventoryItem));

            // TrackObject(inventoryItem);
            // This may not be needed when we fix landblock update object -
            // TODO: Og II - check this later to see if it is still required.
            Session.Network.EnqueueSend(new GameMessageUpdateObject(inventoryItem));
        }

        public Player(Session session) : base(ObjectType.Creature, session.CharacterRequested.Guid, "Player", 1, ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable, WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar, CharacterPositionExtensions.StartingPosition(session.CharacterRequested.Guid.Low))
        {
            Session = session;

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevel, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkill, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyString, new ByteSequence(false));

            // This is the default send upon log in and the most common.   Anything with a velocity will need to add that flag.
            PositionFlag |= UpdatePositionFlag.ZeroQx | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.Contact | UpdatePositionFlag.Placement;

            Name = session.CharacterRequested.Name;
            Icon = 0x1036;
            GameData.ItemCapacity = 102;
            GameData.ContainerCapacity = 7;
            GameData.RadarBehavior = RadarBehavior.ShowAlways;
            GameData.RadarColour = RadarColor.White;
            GameData.Usable = Usable.UsableObjectSelf;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
            PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Movement;

            // apply defaults.  "Load" should be overwriting these with values specific to the character
            // TODO: Load from database should be loading player data - including inventroy and positions
            PhysicsData.CurrentMotionState = new UniversalMotion(MotionStance.Standing);
            PhysicsData.MTableResourceId = 0x09000001u;
            PhysicsData.Stable = 0x20000001u;
            PhysicsData.Petable = 0x34000004u;
            PhysicsData.CSetup = 0x02000001u;

            // radius for object updates
            ListeningRadius = 5f;
        }

        /// <summary>
        ///  Gets a list of Tracked Objects.
        /// </summary>
        public List<ObjectGuid> GetTrackedObjectGuids()
        {
            lock (clientObjectMutex)
            {
                return clientObjectList.Select(x => x.Key).ToList();
            }
        }

        public async Task Load(Character preloadedCharacter = null)
        {
            character = preloadedCharacter ?? await DatabaseManager.Character.LoadCharacter(Guid.Low);

            Strength = character.StrengthAbility;
            Endurance = character.EnduranceAbility;
            Coordination = character.CoordinationAbility;
            Quickness = character.QuicknessAbility;
            Focus = character.FocusAbility;
            Self = character.SelfAbility;
            Health = character.Health;
            Stamina = character.Stamina;
            Mana = character.Mana;

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

            Location = character.Location;

            // TODO: Move this all into Character Creation and store directly in the database.
            if (DatManager.PortalDat.AllFiles.ContainsKey(0x0E000002))
            {
                CharGen cg = CharGen.ReadFromDat();

                int h = Convert.ToInt32(character.PropertiesInt[PropertyInt.HeritageGroup]);
                int s = Convert.ToInt32(character.PropertiesInt[PropertyInt.Gender]);
                SexCG sex = cg.HeritageGroups[h].SexList[s];
                // Set the character basics
                PhysicsData.MTableResourceId = sex.MotionTable;
                PhysicsData.Stable = sex.SoundTable;
                PhysicsData.Petable = sex.PhysicsTable;
                PhysicsData.CSetup = sex.SetupID;
                ModelData.PaletteGuid = sex.BasePalette;

                // Check the character scale
                if (sex.Scale != 100u)
                {
                    // Set the PhysicsData flag to let it know we're changing the scale
                    PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;
                    PhysicsData.ObjScale = sex.Scale / 100f; // Scale is stored as a percentage
                }

                // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
                HairStyleCG hairstyle = sex.HairStyleList[Convert.ToInt32(character.Appearance.HairStyle)];
                bool isBald = hairstyle.Bald;

                // Certain races (Undead, Tumeroks, Others?) have multiple body styles available. This is controlled via the "hair style".
                if (hairstyle.AlternateSetup > 0)
                    PhysicsData.CSetup = hairstyle.AlternateSetup;

                // Apply the hair models & texture changes
                for (int i = 0; i < hairstyle.ObjDesc.AnimPartChanges.Count; i++)
                    ModelData.AddModel(hairstyle.ObjDesc.AnimPartChanges[i].PartIndex, hairstyle.ObjDesc.AnimPartChanges[i].PartID);
                for (int i = 0; i < hairstyle.ObjDesc.TextureChanges.Count; i++)
                    ModelData.AddTexture(hairstyle.ObjDesc.TextureChanges[i].PartIndex, hairstyle.ObjDesc.TextureChanges[i].OldTexture, hairstyle.ObjDesc.TextureChanges[i].NewTexture);

                // Eyes only have Texture Changes - eye color is set seperately
                ObjDesc eyes;
                if (hairstyle.Bald)
                    eyes = sex.EyeStripList[Convert.ToInt32(character.Appearance.Eyes)].ObjDescBald;
                else
                    eyes = sex.EyeStripList[Convert.ToInt32(character.Appearance.Eyes)].ObjDesc;
                for (int i = 0; i < eyes.TextureChanges.Count; i++)
                    ModelData.AddTexture(eyes.TextureChanges[i].PartIndex, eyes.TextureChanges[i].OldTexture, eyes.TextureChanges[i].NewTexture);

                // Nose only has Texture Changes
                ObjDesc nose = sex.NoseStripList[Convert.ToInt32(character.Appearance.Nose)].ObjDesc;
                for (int i = 0; i < nose.TextureChanges.Count; i++)
                    ModelData.AddTexture(nose.TextureChanges[i].PartIndex, nose.TextureChanges[i].OldTexture, nose.TextureChanges[i].NewTexture);

                // Mouth, suprise, only Texture Changes
                ObjDesc mouth = sex.MouthStripList[Convert.ToInt32(character.Appearance.Mouth)].ObjDesc;
                for (int i = 0; i < mouth.TextureChanges.Count; i++)
                    ModelData.AddTexture(mouth.TextureChanges[i].PartIndex, mouth.TextureChanges[i].OldTexture, mouth.TextureChanges[i].NewTexture);

                // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
                PaletteSet skinPalSet = PaletteSet.ReadFromDat(sex.SkinPalSet);
                ushort skinPal = (ushort)skinPalSet.GetPaletteID(character.Appearance.SkinHue);
                // Apply the skin palette...
                ModelData.AddPalette(skinPal, 0x0, 0x18);

                // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
                PaletteSet hairPalSet = PaletteSet.ReadFromDat(sex.HairColorList[Convert.ToInt32(character.Appearance.HairColor)]);
                ushort hairPal = (ushort)hairPalSet.GetPaletteID(character.Appearance.HairHue);
                ModelData.AddPalette(hairPal, 0x18, 0x8);

                // Eye color palette
                ModelData.AddPalette(sex.EyeColorList[Convert.ToInt32(character.Appearance.EyeColor)], 0x20, 0x8);

                // Get the character's startup gear.
                // TODO: Load the proper inventory/equipment options once that system is created.
                if (character.Appearance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
                {
                    uint headgearTableID = sex.HeadgearList[Convert.ToInt32(character.Appearance.HeadgearStyle)].ClothingTable;
                    ClothingTable headCT = ClothingTable.ReadFromDat(headgearTableID);
                    if (headCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect headCBE = headCT.ClothingBaseEffects[sex.SetupID];
                        for (int i = 0; i < headCBE.CloObjectEffects.Count; i++)
                        {
                            byte partNum = (byte)headCBE.CloObjectEffects[i].Index;
                            ModelData.AddModel((byte)headCBE.CloObjectEffects[i].Index, (ushort)headCBE.CloObjectEffects[i].ModelId);

                            for (int j = 0; j < headCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                                ModelData.AddTexture((byte)headCBE.CloObjectEffects[i].Index, (ushort)headCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)headCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                        }

                        // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                        CloSubPalEffect headSubPal = headCT.ClothingSubPalEffects[character.Appearance.HeadgearColor];
                        for (int i = 0; i < headSubPal.CloSubPalettes.Count; i++)
                        {
                            PaletteSet headgearPalSet = PaletteSet.ReadFromDat(headSubPal.CloSubPalettes[i].PaletteSet);
                            ushort headgearPal = (ushort)headgearPalSet.GetPaletteID(character.Appearance.HeadgearHue);

                            for (int j = 0; j < headSubPal.CloSubPalettes[i].Ranges.Count; j++)
                            {
                                uint palOffset = headSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                                uint numColors = headSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                                ModelData.AddPalette(headgearPal, (ushort)palOffset, (ushort)numColors);
                            }
                        }
                    }
                }

                // Get the character's initial pants
                uint pantsTableID = sex.PantsList[Convert.ToInt32(character.Appearance.PantsStyle)].ClothingTable;
                ClothingTable pantsCT = ClothingTable.ReadFromDat(pantsTableID);
                if (pantsCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
                {
                    ClothingBaseEffect pantsCBE = pantsCT.ClothingBaseEffects[sex.SetupID];
                    for (int i = 0; i < pantsCBE.CloObjectEffects.Count; i++)
                    {
                        byte partNum = (byte)pantsCBE.CloObjectEffects[i].Index;
                        ModelData.AddModel((byte)pantsCBE.CloObjectEffects[i].Index, (ushort)pantsCBE.CloObjectEffects[i].ModelId);

                        for (int j = 0; j < pantsCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                            ModelData.AddTexture((byte)pantsCBE.CloObjectEffects[i].Index, (ushort)pantsCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)pantsCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                    }

                    // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                    CloSubPalEffect pantsSubPal = pantsCT.ClothingSubPalEffects[character.Appearance.PantsColor];
                    for (int i = 0; i < pantsSubPal.CloSubPalettes.Count; i++)
                    {
                        PaletteSet pantsPalSet = PaletteSet.ReadFromDat(pantsSubPal.CloSubPalettes[i].PaletteSet);
                        ushort pantsPal = (ushort)pantsPalSet.GetPaletteID(character.Appearance.PantsHue);

                        for (int j = 0; j < pantsSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = pantsSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = pantsSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            ModelData.AddPalette(pantsPal, (ushort)palOffset, (ushort)numColors);
                        }
                    }
                } // end pants

                // Get the character's initial shirt
                uint shirtTableID = sex.ShirtList[Convert.ToInt32(character.Appearance.ShirtStyle)].ClothingTable;
                ClothingTable shirtCT = ClothingTable.ReadFromDat(shirtTableID);
                if (shirtCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
                {
                    ClothingBaseEffect shirtCBE = shirtCT.ClothingBaseEffects[sex.SetupID];
                    for (int i = 0; i < shirtCBE.CloObjectEffects.Count; i++)
                    {
                        byte partNum = (byte)shirtCBE.CloObjectEffects[i].Index;
                        ModelData.AddModel((byte)shirtCBE.CloObjectEffects[i].Index, (ushort)shirtCBE.CloObjectEffects[i].ModelId);

                        for (int j = 0; j < shirtCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                            ModelData.AddTexture((byte)shirtCBE.CloObjectEffects[i].Index, (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                    }

                    // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!

                    if (shirtCT.ClothingSubPalEffects.ContainsKey(character.Appearance.ShirtColor))
                    {
                        CloSubPalEffect shirtSubPal = shirtCT.ClothingSubPalEffects[character.Appearance.ShirtColor];
                        for (int i = 0; i < shirtSubPal.CloSubPalettes.Count; i++)
                        {
                            PaletteSet shirtPalSet = PaletteSet.ReadFromDat(shirtSubPal.CloSubPalettes[i].PaletteSet);
                            ushort shirtPal = (ushort)shirtPalSet.GetPaletteID(character.Appearance.ShirtHue);

                            if (shirtPal > 0) // shirtPal will be 0 if the palette set is empty/not found
                            {
                                for (int j = 0; j < shirtSubPal.CloSubPalettes[i].Ranges.Count; j++)
                                {
                                    uint palOffset = shirtSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                                    uint numColors = shirtSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                                    ModelData.AddPalette(shirtPal, (ushort)palOffset, (ushort)numColors);
                                }
                            }
                        }
                    }
                }

                // Get the character's initial footwear
                uint footwearTableID = sex.FootwearList[Convert.ToInt32(character.Appearance.FootwearStyle)].ClothingTable;
                ClothingTable footwearCT = ClothingTable.ReadFromDat(footwearTableID);
                if (footwearCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
                {
                    ClothingBaseEffect footwearCBE = footwearCT.ClothingBaseEffects[sex.SetupID];
                    for (int i = 0; i < footwearCBE.CloObjectEffects.Count; i++)
                    {
                        byte partNum = (byte)footwearCBE.CloObjectEffects[i].Index;
                        ModelData.AddModel((byte)footwearCBE.CloObjectEffects[i].Index, (ushort)footwearCBE.CloObjectEffects[i].ModelId);

                        for (int j = 0; j < footwearCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                            ModelData.AddTexture((byte)footwearCBE.CloObjectEffects[i].Index, (ushort)footwearCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture, (ushort)footwearCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture);
                    }

                    // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                    CloSubPalEffect footwearSubPal = footwearCT.ClothingSubPalEffects[character.Appearance.FootwearColor];
                    for (int i = 0; i < footwearSubPal.CloSubPalettes.Count; i++)
                    {
                        PaletteSet footwearPalSet = PaletteSet.ReadFromDat(footwearSubPal.CloSubPalettes[i].PaletteSet);
                        ushort footwearPal = (ushort)footwearPalSet.GetPaletteID(character.Appearance.FootwearHue);

                        for (int j = 0; j < footwearSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = footwearSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = footwearSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            ModelData.AddPalette(footwearPal, (ushort)palOffset, (ushort)numColors);
                        }
                    }
                } // end footwear
            }

            IsOnline = true;

            TotalLogins = character.TotalLogins = character.TotalLogins + 1;
            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence((ushort)TotalLogins));

            // SendSelf will trigger the entrance into portal space
            SendSelf();
            SendFriendStatusUpdates();

            // Init the client with the chat channel ID's, and then notify the player that they've choined the associated channels.
            var setTurbineChatChannels = new GameEventSetTurbineChatChannels(Session, 0, 1, 2, 3, 4, 6, 7, 0, 0, 0); // TODO these are hardcoded right now
            var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "General");
            var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Trade");
            var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "LFG");
            var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Roleplay");
            Session.Network.EnqueueSend(setTurbineChatChannels, general, trade, lfg, roleplay);
        }

        public void AddToActionQueue(QueuedGameAction action)
        {
            actionQueue.Enqueue(action);
        }

        public void AddToExaminationQueue(QueuedGameAction action)
        {
            examinationQueue.Enqueue(action);
        }

        public QueuedGameAction ActionQueuePop()
        {
            QueuedGameAction action = null;
            if (actionQueue.TryDequeue(out action))
            {
                lock (delayedActionsMutex)
                {
                    if (action.RespectDelay && delayedActions.ContainsKey(action.ObjectId))
                    {
                        double endTime = delayedActions[action.ObjectId];
                        if (action.StartTime > endTime)
                        {
                            // the new action starts after the old one is complete, so remove the old one
                            delayedActions.Remove(action.ObjectId);
                        }
                        else
                        {
                            // the new action should start before the old one is complete, so enqueue the new one again
                            action.StartTime = WorldManager.PortalYearTicks;
                            actionQueue.Enqueue(action);
                            return null;
                        }
                    }

                    // This is an action e.g. animation that takes time to complete
                    // Remember this object to delay further actions queued for the same object
                    if (action.EndTime > WorldManager.PortalYearTicks)
                    {
                        delayedActions.Add(action.ObjectId, action.EndTime);
                    }
                }
                return action;
            }
            else
                return null;
        }

        public QueuedGameAction ExaminationQueuePop()
        {
            QueuedGameAction action = null;
            if (examinationQueue.TryDequeue(out action))
                return action;
            else
                return null;
        }

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(ulong amount)
        {
            // until we are max level we must make sure that we send
            XpTable xpTable = XpTable.ReadFromDat();
            var chart = xpTable.LevelingXpChart;
            CharacterLevel maxLevel = chart.Levels.Last();
            if (character.Level != maxLevel.Level)
            {
                ulong amountLeftToEnd = maxLevel.TotalXp - character.TotalExperience;
                if (amount > amountLeftToEnd)
                {
                    amount = amountLeftToEnd;
                }
                character.GrantXp(amount);
                CheckForLevelup();
                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, character.TotalExperience);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var message = new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate, message);
            }
        }

        /// <summary>
        /// Public method for adding a new skill by spending skill credits.
        /// </summary>
        /// <remarks>
        ///  The client will throw up more then one train skill dialog and the user has the chance to spend twice.
        /// </remarks>
        /// <param name="skill"></param>
        /// <param name="creditsSpent"></param>
        public void TrainSkill(Skill skill, uint creditsSpent)
        {
            if (character.AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = character.TrainSkill(skill, creditsSpent);
                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, character.AvailableSkillCredits);
                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, Skill.None, SkillStatus.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText = "";

                // if the skill has already been trained or we do not have enough credits, then trainNewSkill be set false
                if (trainNewSkill)
                {
                    // replace the trainSkillUpdate message with the correct skill assignment:
                    trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, SkillStatus.Trained, 0, 0, 0);
                    trainSkillMessageText = $"{SkillExtensions.ToSentence(skill)} trained. You now have {character.AvailableSkillCredits} credits available.";
                }
                else
                {
                    trainSkillMessageText = $"Failed to train {SkillExtensions.ToSentence(skill)}! You now have {character.AvailableSkillCredits} credits available.";
                }

                // create the final game message and send to the client
                var message = new GameMessageSystemChat(trainSkillMessageText, ChatMessageType.Advancement);
                Session.Network.EnqueueSend(trainSkillUpdate, currentCredits, message);
            }
        }

        /// <summary>
        /// Determines if the player has advanced a level
        /// </summary>
        /// <remarks>
        /// Known issues:
        ///         1. XP updates from outside of the grantxp command have not been done yet.
        /// </remarks>
        private void CheckForLevelup()
        {
            // Question: Where do *we* call CheckForLevelup()? :
            //      From within the player.cs file, the options might be:
            //           GrantXp()
            //      From outside of the player.cs file, we may call CheckForLevelup() durring? :
            //           XP Updates?
            var startingLevel = character.Level;
            XpTable xpTable = XpTable.ReadFromDat();
            var chart = xpTable.LevelingXpChart;
            CharacterLevel maxLevel = chart.Levels.Last();
            bool creditEarned = false;
            if (character.Level == maxLevel.Level) return;

            // increases until the correct level is found
            while (chart.Levels[Convert.ToInt32(character.Level)].TotalXp <= character.TotalExperience)
            {
                character.Level++;
                CharacterLevel newLevel = chart.Levels.FirstOrDefault(item => item.Level == character.Level);
                // increase the skill credits if the chart allows this level to grant a credit
                if (newLevel.GrantsSkillPoint)
                {
                    character.AvailableSkillCredits++;
                    character.TotalSkillCredits++;
                    creditEarned = true;
                }
                // break if we reach max
                if (character.Level == maxLevel.Level)
                {
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    break;
                }
            }

            if (character.Level > startingLevel)
            {
                string level = $"{character.Level}";
                string skillCredits = $"{character.AvailableSkillCredits}";
                string xpAvailable = $"{character.AvailableExperience:#,###0}";
                var levelUp = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Level, character.Level);
                string levelUpMessageText = (character.Level == maxLevel.Level) ? $"You have reached the maximum level of {level}!" : $"You are now level {level}!";
                var levelUpMessage = new GameMessageSystemChat(levelUpMessageText, ChatMessageType.Advancement);
                string xpUpdateText = (character.AvailableSkillCredits > 0) ? $"You have {xpAvailable} experience points and {skillCredits} skill credits available to raise skills and attributes." : $"You have {xpAvailable} experience points available to raise skills and attributes.";
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.AvailableSkillCredits, character.AvailableSkillCredits);
                if (character.Level != maxLevel.Level && !creditEarned)
                {
                    string nextCreditAtText = $"You will earn another skill credit at {chart.Levels.Where(item => item.Level > character.Level).OrderBy(item => item.Level).First(item => item.GrantsSkillPoint).Level}";
                    var nextCreditMessage = new GameMessageSystemChat(nextCreditAtText, ChatMessageType.Advancement);
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits, nextCreditMessage);
                }
                else
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits);
                // play level up effect
                ActionApplyVisualEffect(Network.Enum.PlayScript.LevelUp, Guid);
            }
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            uint baseValue = character.Abilities[ability].Base;
            uint result = SpendAbilityXp(character.Abilities[ability], amount);
            bool isSecondary = (ability == Enum.Ability.Health || ability == Enum.Ability.Stamina || ability == Enum.Ability.Mana);
            uint ranks = character.Abilities[ability].Ranks;
            uint newValue = character.Abilities[ability].UnbuffedValue;
            string messageText = "";
            if (result > 0u)
            {
                GameMessage abilityUpdate;
                if (!isSecondary)
                {
                    abilityUpdate = new GameMessagePrivateUpdateAbility(Session, ability, ranks, baseValue, result);
                }
                else
                {
                    abilityUpdate = new GameMessagePrivateUpdateVital(Session, ability, ranks, baseValue, result, character.Abilities[ability].Current);
                }

                // checks if max rank is achieved and plays fireworks w/ special text
                if (IsAbilityMaxRank(ranks, isSecondary))
                {
                    // fireworks
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {ability} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {ability} is now {newValue}!";
                }
                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var soundEvent = new GameMessageSound(Guid, Network.Enum.Sound.RaiseTrait, 1f);
                var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);

                // This seems to be needed to keep health up to date properly.
                // Needed when increasing health and endurance.
                if (ability == Enum.Ability.Endurance)
                {
                    var healthUpdate = new GameMessagePrivateUpdateVital(Session, Enum.Ability.Health, Health.Ranks, Health.Base, Health.ExperienceSpent, Health.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, healthUpdate);
                }
                else if (ability == Enum.Ability.Self)
                {
                    var manaUpdate = new GameMessagePrivateUpdateVital(Session, Enum.Ability.Mana, Mana.Ranks, Mana.Base, Mana.ExperienceSpent, Mana.Current);
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message, manaUpdate);
                }
                else
                {
                    Session.Network.EnqueueSend(abilityUpdate, xpUpdate, soundEvent, message);
                }
            }
            else
            {
                ChatPacket.SendServerMessage(Session, $"Your attempt to raise {ability} has failed.", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendAbilityXp(CreatureAbility ability, uint amount)
        {
            uint result = 0;
            bool addToCurrentValue = false;
            ExperienceExpenditureChart chart;
            XpTable xpTable = XpTable.ReadFromDat();
            switch (ability.Ability)
            {
                case Enum.Ability.Health:
                case Enum.Ability.Stamina:
                case Enum.Ability.Mana:
                    chart = xpTable.VitalXpChart;
                    addToCurrentValue = true;
                    break;
                default:
                    chart = xpTable.AbilityXpChart;
                    break;
            }

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (ability.Ranks >= (chart.Ranks.Count - 1))
                return result;

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = 0u;
            int rank10Offset = 0;

            if (ability.Ranks + 10 >= (chart.Ranks.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(ability.Ranks + 10) - (chart.Ranks.Count - 1));
                rank10 = chart.Ranks[Convert.ToInt32(ability.Ranks) + rank10Offset].TotalXp - chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            }
            else
            {
                rank10 = chart.Ranks[Convert.ToInt32(ability.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(ability.Ranks)].TotalXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                {
                    rankUps = Convert.ToUInt32(rank10Offset);
                }
                else
                {
                    rankUps = 10u;
                }
            }

            if (rankUps > 0)
            {
                ability.Current += addToCurrentValue ? rankUps : 0u;
                ability.Ranks += rankUps;
                ability.ExperienceSpent += amount;
                character.SpendXp(amount);
                result = ability.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Check a rank against the ability charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if ability is max rank; false if ability is below max rank</returns>
        private bool IsAbilityMaxRank(uint rank, bool isAbilityVitals)
        {
            ExperienceExpenditureChart xpChart = new ExperienceExpenditureChart();
            XpTable xpTable = XpTable.ReadFromDat();

            if (isAbilityVitals)
                xpChart = xpTable.VitalXpChart;
            else
                xpChart = xpTable.AbilityXpChart;

            if (rank == (xpChart.Ranks.Count - 1))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check a rank against the skill charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if skill is max rank; false if skill is below max rank</returns>
        private bool IsSkillMaxRank(uint rank, SkillStatus status)
        {
            ExperienceExpenditureChart xpChart = new ExperienceExpenditureChart();
            XpTable xpTable = XpTable.ReadFromDat();

            if (status == SkillStatus.Trained)
                xpChart = xpTable.TrainedSkillXpChart;
            else if (status == SkillStatus.Specialized)
                xpChart = xpTable.SpecializedSkillXpChart;

            if (rank == (xpChart.Ranks.Count - 1))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Spend xp Skill ranks
        /// </summary>
        public void SpendXp(Skill skill, uint amount)
        {
            uint baseValue = 0;
            uint result = SpendSkillXp(character.Skills[skill], amount);

            uint ranks = character.Skills[skill].Ranks;
            uint newValue = character.Skills[skill].UnbuffedValue;
            var status = character.Skills[skill].Status;
            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
            var skillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, status, ranks, baseValue, result);
            var soundEvent = new GameMessageSound(Guid, Network.Enum.Sound.RaiseTrait, 1f);
            string messageText = "";

            if (result > 0u)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(ranks, status))
                {
                    // fireworks on rank up is 0x8D
                    ActionApplyVisualEffect(Network.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill} is now {newValue}!";
                }
            }
            else
            {
                messageText = $"Your attempt to raise {skill} has failed!";
            }
            var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
            Session.Network.EnqueueSend(xpUpdate, skillUpdate, soundEvent, message);
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
        public override void OnKill(Session killerSession)
        {
            base.OnKill(killerSession);

            ObjectGuid killerId = killerSession.Player.Guid;

            IsAlive = false;
            Health.Current = 0; // Set the health to zero
            character.NumDeaths++; // Increase the NumDeaths counter
            character.DeathLevel++; // Increase the DeathLevel

            // TODO: Find correct vitae formula/value
            character.VitaeCpPool = 0; // Set vitae

            // TODO: Generate a death message based on the damage type to pass in to each death message:
            string currentDeathMessage = $"died to {killerSession.Player.Name}.";

            // Send Vicitim Notification, or "Your Death" event to the client:
            // create and send the client death event, GameEventYourDeath
            var msgYourDeath = new GameEventYourDeath(Session, $"You have {currentDeathMessage}");
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Health, Health.Current);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.NumDeaths, character.NumDeaths);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.DeathLevel, character.DeathLevel);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.VitaeCpPool, character.VitaeCpPool);
            var msgPurgeEnchantments = new GameEventPurgeAllEnchantments(Session);
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);

            // Send first death message group
            Session.Network.EnqueueSend(msgHealthUpdate, msgYourDeath, msgNumDeaths, msgDeathLevel, msgVitaeCpPool, msgPurgeEnchantments);

            // Broadcast the 019E: Player Killed GameMessage
            ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);

            // create corpse at location
            var corpse = CorpseObjectFactory.CreateCorpse(this, Location);
            corpse.Location.PositionY -= corpse.PhysicsData.ObjScale;
            corpse.Location.PositionZ -= corpse.PhysicsData.ObjScale / 2;

            // Corpses stay on the ground for 5 * player level but minimum 1 hour
            // corpse.DespawnTime = Math.Max((int)session.Player.PropertiesInt[Enum.Properties.PropertyInt.Level] * 5, 360) + WorldManager.PortalYearTicks; // as in live
            corpse.DespawnTime = 20 + WorldManager.PortalYearTicks; // only for testing

            // Save character's last death position - for the time being, we will use any position
            SetCharacterPosition(PositionType.LastOutsideDeath, Location);

            // teleport to sanctuary or best location
            Position newPositon = new Position();

            if (Positions.ContainsKey(PositionType.Sanctuary))
                newPositon = Positions[PositionType.Sanctuary];
            else if (Positions.ContainsKey(PositionType.LastPortal))
                newPositon = Positions[PositionType.LastPortal];
            else
                newPositon = Positions[PositionType.Location];

            // add a Corpse at the current location via the ActionQueue to honor the motion and teleport delays
            // QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
            // AddToActionQueue(addCorpse);
            // If the player is outside of the landblock we just died in, then reboadcast the death for
            // the players at the lifestone.
            if (Positions[PositionType.LastOutsideDeath].Cell != newPositon.Cell)
            {
                ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);
            }

            // Queue the teleport to lifestone
            ActionQueuedTeleport(newPositon, Guid, GameActionType.TeleToLifestone);

            // Regenerate/ressurect?
            Health.Current = 5;
            msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Health, Health.Current);
            Session.Network.EnqueueSend(msgHealthUpdate);

            // Stand back up
            EnqueueMovementEvent(new UniversalMotion(MotionStance.Standing), Guid);
        }

        /// <summary>
        /// Used to ensure that teleport happens in a sequential order, on the queue
        /// </summary>
        /// <param name="teleportPosition"></param>
        /// <param name="objectId"></param>
        /// <param name="teleportType">Must be a teleportation range action type</param>
        public void ActionQueuedTeleport(Position teleportPosition, ObjectGuid objectId, GameActionType teleportType)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, teleportPosition, teleportType);
            AddToActionQueue(action);
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
            // TODO: remove TalkDirect hack and implement a proper mechanism for this.  perhaps a server action queue
            QueuedGameAction action = new QueuedGameAction(deathMessage, victimId.Full, killerId.Full, GameActionType.TalkDirect);
            AddToActionQueue(action);
        }

        /// <summary>
        /// Receieves a message from the action queue about other player deaths and Sends the message to the player,
        /// while maintaining the proper sequences. This will never be sent to the player, unless the player commits suicide.
        /// </summary>
        /// <param name="deathMessage">This message can be any text string</param>
        public void BroadcastPlayerDeath(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
        {
            var deathBroadcast = new GameMessagePlayerKilled(deathMessage, victimId, killerId);
            Session.Network.EnqueueSend(deathBroadcast);
        }

        public void ActionApplySoundEffect(Sound sound, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, (uint)sound, GameActionType.ApplySoundEffect);
            AddToActionQueue(action);
        }

        public void ActionApplyVisualEffect(PlayScript effect, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, (uint)effect, GameActionType.ApplyVisualEffect);
            AddToActionQueue(action);
        }

        public void EnqueueMovementEvent(UniversalMotion motion, ObjectGuid objectId)
        {
            QueuedGameAction action = new QueuedGameAction(objectId.Full, motion, GameActionType.MovementEvent);
            AddToActionQueue(action);
        }

        public void SendMovementEvent(UniversalMotion motion, WorldObject sender)
        {
            Session.Network.EnqueueSend(new GameMessageUpdateMotion(sender.Guid,
                                                                    sender.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                                    sender.Sequences, motion));
        }

        // Play a sound
        public void PlaySound(Sound sound, ObjectGuid targetId)
        {
            Session.Network.EnqueueSend(new GameMessageSound(targetId, sound, 1f));
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId)
        {
            var effectEvent = new GameMessageScript(targetId, effectId);
            Session.Network.EnqueueSend(effectEvent);
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Not checking and accounting for XP gained from skill usage.
        /// </remarks>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendSkillXp(CharacterSkill skill, uint amount)
        {
            uint result = 0u;
            ExperienceExpenditureChart chart;
            XpTable xpTable = XpTable.ReadFromDat();

            if (skill.Status == SkillStatus.Trained)
                chart = xpTable.TrainedSkillXpChart;
            else if (skill.Status == SkillStatus.Specialized)
                chart = xpTable.SpecializedSkillXpChart;
            else
                return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (chart.Ranks.Count - 1))
                return result;

            uint rankUps = 0u;
            uint currentXp = chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            uint rank1 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 1].XpFromPreviousRank;
            uint rank10 = 0u;
            int rank10Offset = 0;

            if (skill.Ranks + 10 >= (chart.Ranks.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(skill.Ranks + 10) - (chart.Ranks.Count - 1));
                rank10 = chart.Ranks[Convert.ToInt32(skill.Ranks) + rank10Offset].TotalXp - chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            }
            else
            {
                rank10 = chart.Ranks[Convert.ToInt32(skill.Ranks) + 10].TotalXp - chart.Ranks[Convert.ToInt32(skill.Ranks)].TotalXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                {
                    rankUps = Convert.ToUInt32(rank10Offset);
                }
                else
                {
                    rankUps = 10u;
                }
            }

            if (rankUps > 0)
            {
                skill.Ranks += rankUps;
                skill.ExperienceSpent += amount;
                character.SpendXp(amount);
                result = skill.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Will send out GameEventFriendsListUpdate packets to everyone online that has this player as a friend.
        /// </summary>
        private void SendFriendStatusUpdates()
        {
            List<Session> inverseFriends = WorldManager.FindInverseFriends(Guid);

            if (inverseFriends.Count > 0)
            {
                Friend playerFriend = new Friend();
                playerFriend.Id = Guid;
                playerFriend.Name = Name;
                foreach (var friendSession in inverseFriends)
                {
                    friendSession.Network.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
                }
            }
        }

        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public async Task<AddFriendResult> AddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
                return AddFriendResult.FriendWithSelf;

            // Check if friend exists
            if (character.Friends.SingleOrDefault(f => string.Equals(f.Name, friendName, StringComparison.CurrentCultureIgnoreCase)) != null)
                return AddFriendResult.AlreadyInList;

            // TODO: check if player is online first to avoid database hit??
            // Get character record from DB
            Character friendCharacter = await DatabaseManager.Character.GetCharacterByName(friendName);

            if (friendCharacter == null)
                return AddFriendResult.CharacterDoesNotExist;

            Friend newFriend = new Friend();
            newFriend.Name = friendCharacter.Name;
            newFriend.Id = new ObjectGuid(friendCharacter.Id, GuidType.Player);

            // Save to DB
            await DatabaseManager.Character.AddFriend(Guid.Low, newFriend.Id.Low);

            // Add to character object
            character.AddFriend(newFriend);

            // Send packet
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));

            return AddFriendResult.Success;
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendId">The ObjectGuid of the friend that is being removed</param>
        public async Task<RemoveFriendResult> RemoveFriend(ObjectGuid friendId)
        {
            Friend friendToRemove = character.Friends.SingleOrDefault(f => f.Id.Low == friendId.Low);

            // Not in friend list
            if (friendToRemove == null)
                return RemoveFriendResult.NotInFriendsList;

            // Remove from DB
            await DatabaseManager.Character.DeleteFriend(Guid.Low, friendId.Low);

            // Remove from character object
            character.RemoveFriend(friendId.Low);

            // Send packet
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));

            return RemoveFriendResult.Success;
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public async void RemoveAllFriends()
        {
            // Remove all from DB
            await DatabaseManager.Character.RemoveAllFriends(Guid.Low);

            // Remove from character object
            character.RemoveAllFriends();
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
        /// Set a single character option to the provided value. This does not save to the database.
        /// </summary>
        public void SetCharacterOption(CharacterOption option, bool value)
        {
            character.SetCharacterOption(option, value);
        }

        /// <summary>
        /// Saves options to the database.  Options include things like spell tabs, settings (F11), chat windows, etc.
        /// </summary>
        public void SaveOptions()
        {
            if (character != null)
                DatabaseManager.Character.SaveCharacterOptions(character);

            // TODO: Save other options as we implement them.
        }

        /// <summary>
        /// Set the currenly position of the character, to later save in the database.
        /// </summary>
        public void SetPhysicalCharacterPosition()
        {
            // Saves the current player position after converting from a Position Object, to a CharacterPosition object
            SetCharacterPosition(PositionType.Location, Session.Player.Location);
        }

        /// <summary>
        /// Saves a CharacterPosition to the character position dictionary
        /// </summary>
        public void SetCharacterPosition(Position newPosition)
        {
            // Some positions come from outside of the Player and Character classes
            if (newPosition.CharacterId == 0) newPosition.CharacterId = Guid.Low;
            // reset the landblock id
            if (newPosition.LandblockId.Landblock == 0 && newPosition.Cell > 0)
            {
                newPosition.LandblockId = new LandblockId(newPosition.Cell);
            }
            character.SetCharacterPosition(newPosition);
            DatabaseManager.Character.SaveCharacterPosition(character, newPosition);
        }

        /// <summary>
        /// Sets a position type and then Saves a CharacterPosition to the character position dictionary
        /// </summary>
        public void SetCharacterPosition(PositionType type, Position newPosition)
        {
            newPosition.PositionType = type;
            SetCharacterPosition(newPosition);
        }

        /// <summary>
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.
        /// </summary>
        public void SaveCharacter()
        {
            if (character != null)
            {
                // Save the current position to persistent storage, only durring the server update interval
                SetPhysicalCharacterPosition();
                DatabaseManager.Character.UpdateCharacter(character);
#if DEBUG
                if (Session.Player != null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved.", ChatMessageType.Broadcast));
                }
#endif
            }
        }

        public void UpdateAge()
        {
            if (character != null)
                character.Age++;
        }

        public void SendAgeInt()
        {
            try
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.Age, character.Age));
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
            if (character.CharacterOptions[CharacterOption.AppearOffline] == true)
                return false;

            return IsOnline;
        }

        private void SendSelf()
        {
            Session.Network.EnqueueSend(new GameMessageCreateObject(this), new GameMessagePlayerCreate(Guid));
            // TODO: gear and equip

            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);
        }

        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsData.PhysicsState = state;

            if (packet)
            {
                Session.Network.EnqueueSend(new GameMessageSetState(this, state));
                // TODO: this should be broadcast
            }
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(this));

            lock (clientObjectMutex)
            {
                clientObjectList.Clear();
                Session.Player.Location = newPosition;
                SetPhysicalCharacterPosition();
            }

            DelayedUpdatePosition(newPosition);
        }

        public void UpdatePosition(Position newPosition)
        {
            Location = newPosition;
            // character.SetCharacterPosition(newPosition);
            SendUpdatePosition();
        }

        private void DelayedUpdatePosition(Position newPosition)
        {
            var t = new Thread(() =>
            {
                Thread.Sleep(10);
                Location = newPosition;
                // character.SetCharacterPosition(newPosition);
                SendUpdatePosition();
            });
            t.Start();
        }

        public void SetTitle(uint title)
        {
            var updateTitle = new GameEventUpdateTitle(Session, title);
            var message = new GameMessageSystemChat($"Your title is now {title}!", ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(updateTitle, message);
        }

        public void ReceiveChat(WorldObject sender, ChatMessageArgs e)
        {
            // TODO: Implement
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known creatures
        /// </summary>
        public List<ObjectGuid> GetKnownCreatures()
        {
            lock (clientObjectMutex)
            {
                return (List<ObjectGuid>)clientObjectList.Select(x => x.Key).Where(o => o.IsCreature()).ToList();
            }
        }

        /// <summary>
        /// forces either an update or a create object to be sent to the client
        /// </summary>
        public void TrackObject(WorldObject worldObject)
        {
            bool sendUpdate = true;

            if (worldObject.Guid == Guid)
                return;

            lock (clientObjectMutex)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                if (!sendUpdate)
                {
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                    worldObject.PlayScript(Session);
                }
                else
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
            }

            log.Debug($"Telling {Name} about {worldObject.Name} - {worldObject.Guid.Full.ToString("X")}");

            if (sendUpdate)
            {
                // Session.Network.EnqueueSend(new GameMessageUpdateObject(worldObject));
                // TODO: Better handling of sending updates to client. The above line is causing much more problems then it is solving until we get proper movement.
                // Add this or something else back in when we handle movement better, until then, just send the create object once and move on.
            }
            else
                Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));
        }

        /// <summary>
        /// Do the player log out work.<para />
        /// If you want to force a player to logout, use Session.LogOffPlayer().
        /// </summary>
        public void Logout(bool clientSessionTerminatedAbruptly = false)
        {
            if (!IsOnline)
                return;

            InWorld = false;
            IsOnline = false;

            SendFriendStatusUpdates();

            // remove the player from landblock management
            LandblockManager.RemoveObject(this);

            if (!clientSessionTerminatedAbruptly)
            {
                var logout = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LogOut));
                Session.Network.EnqueueSend(new GameMessageUpdateMotion(Guid,
                                                                        Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                                        Sequences, logout));

                SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);

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
            lock (clientObjectMutex)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);
                if (sendUpdate)
                {
                    clientObjectList.Remove(worldObject.Guid);
                }
            }

            if (sendUpdate && remove)
            {
                Session.Network.EnqueueSend(new GameMessageRemoveObject(worldObject));
            }
        }

        public void SendUpdatePosition()
        {
            LastMovementBroadcastTicks = WorldManager.PortalYearTicks;
            Session.Network.EnqueueSend(new GameMessageUpdatePosition(this));
        }

        public void SendAutonomousPosition()
        {
            // Session.Network.EnqueueSend(new GameMessageAutonomousPosition(this));
        }

        public bool WaitingForDelayedTeleport { get; set; } = false;
        public Position DelayedTeleportDestination { get; private set; } = null;
        public DateTime DelayedTeleportTime { get; private set; } = DateTime.MinValue;

        public void SetDelayedTeleport(TimeSpan delay, Position destination)
        {
            DelayedTeleportDestination = destination;
            DelayedTeleportTime = DateTime.UtcNow + delay;
            WaitingForDelayedTeleport = true;
        }

        public void ClearDelayedTeleport()
        {
            DelayedTeleportDestination = null;
            DelayedTeleportTime = DateTime.MinValue;
            WaitingForDelayedTeleport = false;
        }

        override public void GameLoopUpdate()
        {
            uint oldHealth = Health.Current;
            uint oldStamina = Stamina.Current;
            uint oldMana = Mana.Current;

            base.GameLoopUpdate();

            // If the game loop changed a vital -- send an update message to the client
            if (Health.Current != oldHealth)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(Session, Enum.Ability.Health, Health));
            }

            if (Stamina.Current != oldStamina)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(Session, Enum.Ability.Stamina, Stamina));
            }

            if (Mana.Current != oldMana)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(Session, Enum.Ability.Mana, Mana));
            }
        }
    }
}