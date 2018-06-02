using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using log4net;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Util;

using Landblock = ACE.Server.Entity.Landblock;
using Position = ACE.Entity.Position;

namespace ACE.Server.WorldObjects
{
    public abstract partial class WorldObject : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is object property overrides that should have come from the shard db (or init to defaults of object is new to this instance).
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Biota { get; }

        /// <summary>
        /// This is just a wrapper around Biota.Id
        /// </summary>
        public ObjectGuid Guid => new ObjectGuid(Biota.Id);

        public PhysicsObj PhysicsObj { get; protected set; }

        public bool InitPhysics { get; protected set; }

        public ObjectDescriptionFlag BaseDescriptionFlags { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; }

        public SequenceManager Sequences { get; } = new SequenceManager();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        private bool busyState;
        private bool movingState;

        public bool IsBusy { get => busyState; set => busyState = value; }
        public bool IsMovingTo { get => movingState; set => movingState = value; }

        public EmoteManager EmoteManager;
        public EnchantmentManager EnchantmentManager;

        public List<AttackDamage> AttackList = new List<AttackDamage>();

        public WorldObject ProjectileSource;
        public WorldObject ProjectileTarget;

        /// <summary>
        /// A new biota will be created taking all of its values from weenie.
        /// </summary>
        protected WorldObject(Weenie weenie, ObjectGuid guid)
        {
            Biota = weenie.CreateCopyAsBiota(guid.Full);

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        protected WorldObject(Biota biota)
        {
            Biota = biota;

            ExistsInDatabase = true;
            LastRequestedDatabaseSave = DateTime.UtcNow;

            SetEphemeralValues();
        }

        /// <summary>
        /// Initializes a new default physics object
        /// </summary>
        public virtual void InitPhysicsObj()
        {
            PhysicsObj = new PhysicsObj();
            PhysicsObj.set_object_guid(Guid);
            PhysicsObj.TransientState |= TransientStateFlags.Contact | TransientStateFlags.OnWalkable;

            // will eventually map directly to WorldObject
            PhysicsObj.set_weenie_obj(new WeenieObject(this));

            PhysicsObj.makeAnimObject(SetupTableId, true);
            PhysicsObj.SetMotionTableID(MotionTableId);

            PhysicsObj.SetScale(ObjScale ?? 1.0f);
        }

        private void SetEphemeralValues()
        { 
            Sequences.AddOrSetSequence(SequenceType.ObjectPosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectMovement, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectState, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVector, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectTeleport, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectServerControl, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectForcePosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVisualDesc, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence());

            Sequences.AddOrSetSequence(SequenceType.Motion, new UShortSequence(1, 0x7FFF)); // MSB is reserved, so set max value to exclude it.

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeStrength, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeEndurance, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeQuickness, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeCoordination, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeFocus, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttributeSelf, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevel, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelHealth, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelStamina, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelMana, new ByteSequence(false));

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkill, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillAxe, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillBow, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillCrossBow, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillDagger, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMace, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMeleeDefense, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMissileDefense, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSling, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSpear, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillStaff, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSword, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillThrownWeapon, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillUnarmedCombat, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillArcaneLore, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMagicDefense, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillManaConversion, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSpellcraft, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillItemAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillPersonalAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillDeception, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillHealing, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillJump, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillLockpick, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillRun, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillAwareness, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillArmsAndArmorRepair, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillCreatureAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillWeaponAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillArmorAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMagicItemAppraisal, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillCreatureEnchantment, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillItemEnchantment, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillLifeMagic, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillWarMagic, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillLeadership, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillLoyalty, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillFletching, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillAlchemy, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillCooking, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSalvaging, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillTwoHandedCombat, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillGearcraft, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillVoidMagic, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillHeavyWeapons, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillLightWeapons, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillFinesseWeapons, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillMissileWeapons, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillShield, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillDualWield, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillRecklessness, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSneakAttack, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillDirtyFighting, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillChallenge, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkillSummoning, new ByteSequence(false));

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyString, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDataID, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInstanceID, new ByteSequence(false));

            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyString, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyDataID, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInstanceId, new ByteSequence(false));

            Sequences.AddOrSetSequence(SequenceType.SetStackSize, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.Confirmation, new ByteSequence(false));

            foreach (var x in EphemeralProperties.PropertiesBool.ToList())
                EphemeralPropertyBools.TryAdd((PropertyBool)x, null);
            foreach (var x in EphemeralProperties.PropertiesDataId.ToList())
                EphemeralPropertyDataIds.TryAdd((PropertyDataId)x, null);
            foreach (var x in EphemeralProperties.PropertiesDouble.ToList())
                EphemeralPropertyFloats.TryAdd((PropertyFloat)x, null);
            foreach (var x in EphemeralProperties.PropertiesInstanceId.ToList())
                EphemeralPropertyInstanceIds.TryAdd((PropertyInstanceId)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt.ToList())
                EphemeralPropertyInts.TryAdd((PropertyInt)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt64.ToList())
                EphemeralPropertyInt64s.TryAdd((PropertyInt64)x, null);
            foreach (var x in EphemeralProperties.PropertiesString.ToList())
                EphemeralPropertyStrings.TryAdd((PropertyString)x, null);

            foreach (var x in Biota.BiotaPropertiesBool.Where(i => EphemeralProperties.PropertiesBool.Contains(i.Type)).ToList())
                EphemeralPropertyBools[(PropertyBool)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesDID.Where(i => EphemeralProperties.PropertiesDataId.Contains(i.Type)).ToList())
                EphemeralPropertyDataIds[(PropertyDataId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesFloat.Where(i => EphemeralProperties.PropertiesDouble.Contains(i.Type)).ToList())
                EphemeralPropertyFloats[(PropertyFloat)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesIID.Where(i => EphemeralProperties.PropertiesInstanceId.Contains(i.Type)).ToList())
                EphemeralPropertyInstanceIds[(PropertyInstanceId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt.Where(i => EphemeralProperties.PropertiesInt.Contains(i.Type)).ToList())
                EphemeralPropertyInts[(PropertyInt)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt64.Where(i => EphemeralProperties.PropertiesInt64.Contains(i.Type)).ToList())
                EphemeralPropertyInt64s[(PropertyInt64)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesString.Where(i => EphemeralProperties.PropertiesString.Contains(i.Type)).ToList())
                EphemeralPropertyStrings[(PropertyString)x.Type] = x.Value;

            GeneratorProfiles = Biota.BiotaPropertiesGenerator.ToList();

            BaseDescriptionFlags = ObjectDescriptionFlag.Attackable;

            InitPhysicsObj();

            EncumbranceVal = EncumbranceVal ?? (StackUnitEncumbrance ?? 0) * (StackSize ?? 1);

            EmoteManager = new EmoteManager(this);
            EnchantmentManager = new EnchantmentManager(this);

            if (Placement == null)
                Placement = ACE.Entity.Enum.Placement.Resting;

            //CurrentMotionState = new UniversalMotion(MotionStance.Invalid, new MotionItem(MotionCommand.Invalid));

            QueueNextHeartBeat();
        }

        /// <summary>
        /// This will be true when teleporting
        /// </summary>
        public bool Teleporting { get; set; } = false;

        public bool HandleReceive(WorldObject item, uint amount, WorldObject receiver, WorldObject giver)
        {
            var emoteChain = new ActionChain();
             
            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
            var result = Biota.BiotaPropertiesEmote.Where(emote => emote.WeenieClassId == item.WeenieClassId && rng >= emote.Probability);
            if (result.Count() < 1)
            {
                result = Biota.BiotaPropertiesEmote.Where(emote => emote.WeenieClassId == item.WeenieClassId);
            }
            if (result.Count() > 0)
            {
                var actions = Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.ElementAt(result.Count() - 1).EmoteSetId && action.EmoteCategory == result.ElementAt<BiotaPropertiesEmote>(0).Category);

                foreach (var action in actions)
                {
                    EmoteManager.ExecuteEmote(result.ElementAt(result.Count() - 1), action, emoteChain, receiver, giver);
                }
                emoteChain.EnqueueChain();
                return true;

            }
            else
            {
                return false;
            }
        }























        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************


        public static float MaxObjectTrackingRange { get; } = 20000f;

        public Position ForcedLocation { get; private set; }

        public Position RequestedLocation { get; private set; }

        public Position PreviousLocation { get; set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock => CurrentParent as Landblock;


        /// <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }

        public virtual void PlayScript(Session session) { }


        ////// Logical Game Data
        public ContainerType ContainerType
        {
            get
            {
                if (WeenieType == WeenieType.Container)
                    return ContainerType.Container;
                else if (RequiresBackpackSlot ?? false)
                    return ContainerType.Foci;
                else
                    return ContainerType.NonContainer;
            }
        }

        public void Examine(Session examiner)
        {
            // TODO : calculate if we were successful
            bool successfulId = true;
            GameEventIdentifyObjectResponse identifyResponse = new GameEventIdentifyObjectResponse(examiner, this, successfulId);
            examiner.Network.EnqueueSend(identifyResponse);

#if DEBUG
            examiner.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.System));
            examiner.Network.EnqueueSend(new GameMessageSystemChat($"{DebugOutputString(GetType(), this)}", ChatMessageType.System));
#endif
        }

        public void ReadBookPage(Session reader, uint pageNum)
        {
            //PageData pageData = new PageData();
            //AceObjectPropertiesBook bookPage = PropertiesBook[pageNum];

            //pageData.AuthorID = bookPage.AuthorId;
            //pageData.AuthorName = bookPage.AuthorName;
            //pageData.AuthorAccount = bookPage.AuthorAccount;
            //pageData.PageIdx = pageNum;
            //pageData.PageText = bookPage.PageText;
            //pageData.IgnoreAuthor = false;
            //// TODO - check for PropertyBool.IgnoreAuthor flag

            //var bookDataResponse = new GameEventBookPageDataResponse(reader, Guid.Full, pageData);
            //reader.Network.EnqueueSend(bookDataResponse);
        }

 
        private string DebugOutputString(Type type, WorldObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine("ACE Debug Output:");
            sb.AppendLine("ACE Class File: " + type.Name + ".cs");
            sb.AppendLine("Guid: " + obj.Guid.Full + " (0x" + obj.Guid.Full.ToString("X") + ")");

            sb.AppendLine("----- Private Fields -----");
            foreach (var prop in obj.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).OrderBy(field => field.Name))
            {
                if (prop.GetValue(obj) == null)
                    continue;

                sb.AppendLine($"{prop.Name.Replace("<", "").Replace(">k__BackingField", "")} = {prop.GetValue(obj)}");
            }

            sb.AppendLine("----- Public Properties -----");
            foreach (var prop in obj.GetType().GetProperties().OrderBy(property => property.Name))
            {
                if (prop.GetValue(obj, null) == null)
                    continue;

                switch (prop.Name.ToLower())
                {
                    case "guid":
                        sb.AppendLine($"{prop.Name} = {obj.Guid.Full} (GuidType.{obj.Guid.Type.ToString()})");
                        break;
                    case "descriptionflags":
                        var descriptionFlags = CalculatedDescriptionFlag();
                        sb.AppendLine($"{prop.Name} = {descriptionFlags.ToString()}" + " (" + (uint)descriptionFlags + ")");
                        break;
                    case "weenieflags":
                        var weenieFlags = CalculatedWeenieHeaderFlag();
                        sb.AppendLine($"{prop.Name} = {weenieFlags.ToString()}" + " (" + (uint)weenieFlags + ")");
                        break;
                    case "weenieflags2":
                        var weenieFlags2 = CalculatedWeenieHeaderFlag2();
                        sb.AppendLine($"{prop.Name} = {weenieFlags2.ToString()}" + " (" + (uint)weenieFlags2 + ")");
                        break;
                    case "positionflag":
                        sb.AppendLine($"{prop.Name} = {obj.PositionFlag.ToString()}" + " (" + (uint)obj.PositionFlag + ")");
                        break;
                    case "itemtype":
                        sb.AppendLine($"{prop.Name} = {obj.ItemType.ToString()}" + " (" + (uint)obj.ItemType + ")");
                        break;
                    case "creaturetype":
                        sb.AppendLine($"{prop.Name} = {obj.CreatureType.ToString()}" + " (" + (uint)obj.CreatureType + ")");
                        break;
                    case "containertype":
                        sb.AppendLine($"{prop.Name} = {obj.ContainerType.ToString()}" + " (" + (uint)obj.ContainerType + ")");
                        break;
                    case "usable":
                        sb.AppendLine($"{prop.Name} = {obj.Usable.ToString()}" + " (" + (uint)obj.Usable + ")");
                        break;
                    case "radarbehavior":
                        sb.AppendLine($"{prop.Name} = {obj.RadarBehavior.ToString()}" + " (" + (uint)obj.RadarBehavior + ")");
                        break;
                    case "physicsdescriptionflag":
                        var physicsDescriptionFlag = CalculatedPhysicsDescriptionFlag();
                        sb.AppendLine($"{prop.Name} = {physicsDescriptionFlag.ToString()}" + " (" + (uint)physicsDescriptionFlag + ")");
                        break;
                    case "physicsstate":
                        var physicsState = CalculatedPhysicsState();
                        sb.AppendLine($"{prop.Name} = {physicsState.ToString()}" + " (" + (uint)physicsState + ")");
                        break;
                    //case "propertiesspellid":
                    //    foreach (var item in obj.PropertiesSpellId)
                    //    {
                    //        sb.AppendLine($"PropertySpellId.{Enum.GetName(typeof(Spell), item.SpellId)} ({item.SpellId})");
                    //    }
                    //    break;
                    case "validlocations":
                        sb.AppendLine($"{prop.Name} = {obj.ValidLocations}" + " (" + (uint)obj.ValidLocations + ")");
                        break;
                    case "currentwieldedlocation":
                        sb.AppendLine($"{prop.Name} = {obj.CurrentWieldedLocation}" + " (" + (uint)obj.CurrentWieldedLocation + ")");
                        break;
                    case "priority":
                        sb.AppendLine($"{prop.Name} = {obj.Priority}" + " (" + (uint)obj.Priority + ")");
                        break;
                    case "radarcolor":
                        sb.AppendLine($"{prop.Name} = {obj.RadarColor}" + " (" + (uint)obj.RadarColor + ")");
                        break;
                    case "location":
                        sb.AppendLine($"{prop.Name} = {obj.Location.ToLOCString()}");
                        break;
                    case "channelsactive":
                        sb.AppendLine($"{prop.Name} = {(Channel)obj.GetProperty(PropertyInt.ChannelsActive)}" + " (" + (uint)obj.GetProperty(PropertyInt.ChannelsActive) + ")");
                        break;
                    case "channelsallowed":
                        sb.AppendLine($"{prop.Name} = {(Channel)obj.GetProperty(PropertyInt.ChannelsAllowed)}" + " (" + (uint)obj.GetProperty(PropertyInt.ChannelsAllowed) + ")");
                        break;
                    case "playerkillerstatus":
                        sb.AppendLine($"{prop.Name} = {obj.PlayerKillerStatus}" + " (" + (uint)obj.PlayerKillerStatus + ")");
                        break;
                    default:
                        sb.AppendLine($"{prop.Name} = {prop.GetValue(obj, null)}");
                        break;
                }
            }

            sb.AppendLine("----- Property Dictionaries -----");

            foreach (var item in obj.GetAllPropertyBools())
                sb.AppendLine($"PropertyBool.{Enum.GetName(typeof(PropertyBool), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyDataId())
                sb.AppendLine($"PropertyDataId.{Enum.GetName(typeof(PropertyDataId), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyFloat())
                sb.AppendLine($"PropertyDouble.{Enum.GetName(typeof(PropertyFloat), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyInstanceId())
                sb.AppendLine($"PropertyInstanceId.{Enum.GetName(typeof(PropertyInstanceId), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyInt())
                sb.AppendLine($"PropertyInt.{Enum.GetName(typeof(PropertyInt), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyInt64())
                sb.AppendLine($"PropertyInt64.{Enum.GetName(typeof(PropertyInt64), item.Key)} ({(int)item.Key}) = {item.Value}");
            foreach (var item in obj.GetAllPropertyString())
                sb.AppendLine($"PropertyString.{Enum.GetName(typeof(PropertyString), item.Key)} ({(int)item.Key}) = {item.Value}");

            sb.AppendLine("\n");

            return sb.ToString().Replace("\r", "");
        }

        public void QueryHealth(Session examiner)
        {
            float healthPercentage = 1f;

            if (this is Creature creature)
                healthPercentage = (float)creature.Health.Current / (float)creature.Health.MaxValue;

            var updateHealth = new GameEventUpdateHealth(examiner, Guid.Full, healthPercentage);
            examiner.Network.EnqueueSend(updateHealth);
        }

        public void QueryItemMana(Session examiner)
        {
            float manaPercentage = 1f;
            uint success = 0;

            if (ItemCurMana != null && ItemMaxMana != null)
            {
                manaPercentage = (float)ItemCurMana / (float)ItemMaxMana;
                success = 1;
            }

            if (success == 0) // according to retail PCAPs, if success = 0, mana = 0.
                manaPercentage = 0;

            var updateMana = new GameEventQueryItemManaResponse(examiner, Guid.Full, manaPercentage, success);
            examiner.Network.EnqueueSend(updateMana);
        }


        // This fully replaces the PhysicsState of the WO, use sparingly?
        //public void SetPhysicsState(PhysicsState state, bool packet = true)
        //{
        //    PhysicsState = state;

        //    if (packet)
        //    {
        //        EnqueueBroadcastPhysicsState();
        //    }
        //}

        public void EnqueueBroadcastPhysicsState()
        {
            if (CurrentLandblock != null)
            {
                var physicsState = CalculatedPhysicsState();
                GameMessage msg = new GameMessageSetState(this, physicsState);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
            }
        }

        public void EnqueueBroadcastUpdateObject()
        {
            if (CurrentLandblock != null)
            {
                GameMessage msg = new GameMessageUpdateObject(this);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
            }
        }

        /*public AceObject SnapShotOfAceObject(bool clearDirtyFlags = false)
        {
            AceObject snapshot = (AceObject)AceObject.Clone();
            if (clearDirtyFlags)
                AceObject.ClearDirtyFlags();
            return snapshot;
        }*/

        public virtual void OnCollideObject(WorldObject target)
        {
            // empty base
        }

        public virtual void OnCollideObjectEnd(WorldObject target)
        {
            // empty base
        }

        public virtual void OnCollideEnvironment()
        {
            // empty base
        }

        public void HandleActionMotion(UniversalMotion motion)
        {
            if (CurrentLandblock != null)
            {
                DoMotion(motion);
            }
        }

        public void DoMotion(UniversalMotion motion)
        {
            if (CurrentLandblock != null)
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        public void ApplyVisualEffects(PlayScript effect)
        {
            // new ActionChain(this, () => PlayParticleEffect(effect, Guid)).EnqueueChain();
            if (CurrentLandblock != null)
            {
                PlayParticleEffect(effect, Guid);
            }
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId)
        {
            if (CurrentLandblock != null)
            {
                var effectEvent = new GameMessageScript(targetId, effectId);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, effectEvent);
            }
        }

        //public List<AceObjectInventory> CreateList => AceObject.CreateList;
        //public List<AceObjectInventory> CreateList { get; set; } = new List<AceObjectInventory>();

        /*public List<AceObjectInventory> WieldList
        {
            get { return CreateList.Where(x => x.DestinationType == (uint)DestinationType.Wield).ToList(); }
        }

        public List<AceObjectInventory> ShopList
        {
            get { return CreateList.Where(x => x.DestinationType == (uint)DestinationType.Shop).ToList(); }
        }*/

        public void EnterWorld()
        {
            if (Location != null)
            {
                LandblockManager.AddObject(this);
                if (SuppressGenerateEffect != true)
                    ApplyVisualEffects(ACE.Entity.Enum.PlayScript.Create);
            }
        }

        public virtual void HeartBeat()
        {            
            // Do Stuff
            if (!(FirstEnterWorldDone ?? false))
            {
                FirstEnterWorldDone = true;
            }

            CheckGeneratorStatus();

            if (!(GeneratorEnteredWorld ?? false))
            {
                if (FirstEnterWorldDone ?? false)
                {
                    if (!(GeneratorDisabled ?? false))
                    {
                        CurrentlyPoweringUp = true;
                        SelectGeneratorProfiles();
                        UpdateGeneratorInts();
                        QueueGenerator();
                        CurrentlyPoweringUp = false;
                    }

                    GeneratorEnteredWorld = true;
                }
            }

            EmoteManager.HeartBeat();

            if (!(GeneratorDisabled ?? false))
            {
                if (GeneratorRegistry.Count < InitGeneratedObjects)
                {
                    SelectMoreGeneratorProfiles();
                    QueueGenerator();
                }

                if (GeneratorQueue.Count > 0)
                    ProcessGeneratorQueue();
            }

            QueueNextHeartBeat();

            SetProperty(PropertyFloat.HeartbeatTimestamp, Time.GetTimestamp());
        }

        public void QueueNextHeartBeat()
        {
            ActionChain nextHeartBeat = new ActionChain();
            nextHeartBeat.AddDelaySeconds(HeartbeatInterval ?? 5);
            nextHeartBeat.AddAction(this, () => HeartBeat());
            nextHeartBeat.EnqueueChain();
        }

        public bool AdjustDungeonCells(Position pos)
        {
            var dungeonID = pos.Cell >> 16;
            //if (!AdjustCell.AdjustDungeons.Contains(dungeonID))
            //    return;

            var adjustCell = AdjustCell.Get(dungeonID);
            var cellID = adjustCell.GetCell(pos.Pos);

            if (cellID != null && pos.Cell != cellID.Value)
            {
                pos.Cell = cellID.Value;
                return true;
            }
            return false;
        }

        public virtual void Activate(WorldObject activator)
        {
            // empty base, override in child objects
        }

        public virtual void Open(WorldObject opener)
        {
            // empty base, override in child objects
        }

        public virtual void Close(WorldObject closer)
        {
            // empty base, override in child objects
        }

        /// <summary>
        /// Applies some amount of damage to this world object from source
        /// </summary>
        /// <param name="source">The attacker / source of damage</param>
        /// <param name="_amount">The amount of damage rounded</param>
        public virtual void TakeDamage(WorldObject source, float _amount, bool crit = false)
        {
            // currently only handles creature types
            if (!(this is Creature)) return;
            var monster = this as Creature;

            var player = source is Player ? source as Player : null;

            var amount = (uint)Math.Round(_amount);
            var newMonsterHealth = (int)(monster.Health.Current - amount);

            AttackList.Add(new AttackDamage(source, amount, crit));

            // apply damage
            if (newMonsterHealth > 0)
                monster.Health.Current = (uint)newMonsterHealth;
            else
            {
                monster.Health.Current = 0;
                monster.OnDeath();
                monster.Die();

                if (player != null)
                {
                    var deathMessage = monster.GetDeathMessage(source, crit);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(deathMessage, monster.Name), ChatMessageType.Broadcast));
                    player.GrantXp((long)monster.XpOverride, false);
                }
            }
        }

        /// <summary>
        /// Returns a strike message based on damage type and severity
        /// </summary>
        public virtual string GetAttackMessage(Creature creature, DamageType damageType, uint amount)
        {
            var percent = (float)amount / creature.Health.Base;
            string verb = null, plural = null;
            Strings.GetAttackVerb(damageType, percent, ref verb, ref plural);
            var type = damageType.GetName().ToLower();
            return $"You {verb} {creature.Name} for {amount} points of {type} damage!";
        }

        /// <summary>
        /// Returns a randomized death message based on damage type
        /// </summary>
        public virtual string GetDeathMessage(WorldObject killer, bool criticalHit = false)
        {
            if (!criticalHit)
            {
                var damageType = killer.GetDamageType();
                Strings.DeathMessages.TryGetValue(damageType, out var messages);
                var idx = Physics.Common.Random.RollDice(0, messages.Count - 1);
                return messages[idx];
            }
            else
            {
                var messages = Strings.Critical;
                var idx = Physics.Common.Random.RollDice(0, messages.Count - 1);
                return messages[idx];
            }
        }

        public Dictionary<PropertyInt, int?> GetProperties(WorldObject wo)
        {
            var props = new Dictionary<PropertyInt, int?>();

            var fields = Enum.GetValues(typeof(PropertyInt)).Cast<PropertyInt>();
            foreach (var field in fields)
            {
                var prop = wo.GetProperty(field);
                props.Add(field, prop);
            }
            return props;
        }

        /// <summary>
        /// Returns the base damage for a weapon
        /// </summary>
        public virtual Range GetBaseDamage()
        {
            var maxDamage = GetProperty(PropertyInt.Damage) ?? 0;
            var variance = GetProperty(PropertyFloat.DamageVariance) ?? 0;
            var minDamage = maxDamage * variance;
            return new Range((float)minDamage, (float)maxDamage);
        }

        /// <summary>
        /// Returns the modified damage for a weapon,
        /// with the wielder enchantments taken into account
        /// </summary>
        public Range GetDamageMod(Creature wielder)
        {
            var baseDamage = GetBaseDamage();
            var damageMod = wielder.EnchantmentManager.GetDamageMod();
            var varianceMod = wielder.EnchantmentManager.GetVarianceMod();

            var baseVariance = baseDamage.Min / baseDamage.Max;

            var maxDamageMod = baseDamage.Max + damageMod;
            var minDamageMod = maxDamageMod * (baseVariance * varianceMod);

            return new Range(minDamageMod, maxDamageMod);
        }

        /// <summary>
        /// Returns the damage type for the currently equipped weapon / ammo
        /// </summary>
        /// <param name="multiple">If true, returns all of the damage types for the weapon</param>
        public virtual DamageType GetDamageType(bool multiple = false)
        {
            var creature = this as Creature;

            var weapon = creature.GetEquippedWeapon();
            if (weapon == null)
                return DamageType.Bludgeon;

            DamageType damageTypes;
            var attackType = creature.GetAttackType();
            if (attackType == AttackType.Melee)
                damageTypes = (DamageType)weapon.GetProperty(PropertyInt.DamageType);
            else
            {
                var ammo = creature.GetEquippedAmmo();
                damageTypes = (DamageType)ammo.GetProperty(PropertyInt.DamageType);
            }

            // returning multiple damage types
            if (multiple) return damageTypes;

            // get single damage type
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                if ((damageTypes & damageType) != 0)
                {
                    // handle multiple damage types
                    if (damageType == DamageType.Slash && creature.CurrentMotionState.Commands[0].Motion.ToString().Contains("Thrust"))
                        continue;

                    return damageType;
                }
            }
            return damageTypes;
        }

        public virtual void Destroy()
        {
            if (Location != null)
            {
                ActionChain destroyChain = new ActionChain();
                destroyChain.AddAction(this, () =>
                {
                    ApplyVisualEffects(ACE.Entity.Enum.PlayScript.Destroy);
                });
                destroyChain.AddDelaySeconds(3);
                destroyChain.AddAction(this, () =>
                {
                    NotifyOfEvent(RegenerationType.Destruction);
                    LandblockManager.RemoveObject(this);
                });
                destroyChain.EnqueueChain();
            }
        }
    }
}
