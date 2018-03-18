using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

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

        public ObjectDescriptionFlag BaseDescriptionFlags { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; }

        public SequenceManager Sequences { get; } = new SequenceManager();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
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

            EncumbranceVal = EncumbranceVal ?? (StackUnitEncumbrance ?? 0) * (StackSize ?? 1);

            if (Placement == null)
                Placement = ACE.Entity.Enum.Placement.Resting;


            SelectGeneratorProfiles();
            UpdateGeneratorInts();
            QueueGenerator();

            QueueNextHeartBeat();
        }

        /// <summary>
        /// This will be true when teleporting
        /// </summary>
        public bool Teleporting { get; set; } = false;

























        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************


        public static float MaxObjectTrackingRange { get; } = 20000f;

        public IActor CurrentParent { get; private set; }

        public Position ForcedLocation { get; private set; }

        public Position RequestedLocation { get; private set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock => CurrentParent as Landblock;


        /// <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        private readonly NestedActionQueue actionQueue = new NestedActionQueue();



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
                if (ItemCapacity != null && ItemCapacity != 0)
                    return ContainerType.Container;
                if (Name.Contains("Foci"))
                    return ContainerType.Foci;
                return ContainerType.NonContainer;
            }
        }

         /// <summary>
        /// This is used to determine how close you need to be to use an item.
        /// NOTE: cheat factor added for items with null use radius.   Og II
        /// </summary>
        public float UseRadiusSquared => ((UseRadius ?? 2) + CSetup.Radius) * ((UseRadius ?? 2) + CSetup.Radius);

        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            if (Location.SquaredDistanceTo(wo.Location) >= wo.UseRadiusSquared)
                return false;
            return true;
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


        public virtual void ActOnUse(ObjectGuid playerId)
        {
            // Do Nothing by default
            if (CurrentLandblock != null)
            {
                Player player = CurrentLandblock.GetObject(playerId) as Player;
                if (player == null)
                {
                    return;
                }

#if DEBUG
                var errorMessage = new GameMessageSystemChat($"Default HandleActionOnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
                player.Session.Network.EnqueueSend(errorMessage);
#endif

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }

        public virtual void HandleActionOnCollide(ObjectGuid playerId)
        {
            // todo: implement.  default is probably to do nothing.
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

        public List<BiotaPropertiesGenerator> GeneratorProfiles = new List<BiotaPropertiesGenerator>();

        public void EnterWorld()
        {
            if (Location != null)
            {
                LandblockManager.AddObject(this);
                if (SuppressGenerateEffect != true)
                    ApplyVisualEffects(ACE.Entity.Enum.PlayScript.Create);
            }
        }

        public Dictionary<uint, GeneratorRegistryNode> GeneratorRegistry = new Dictionary<uint, GeneratorRegistryNode>();

        public List<GeneratorQueueNode> GeneratorQueue = new List<GeneratorQueueNode>();

        public virtual void HeartBeat()
        {
            // Do Stuff

            if (GeneratorQueue.Count > 0)
                ProcessGeneratorQueue();

            QueueNextHeartBeat();
        }

        public void QueueNextHeartBeat()
        {
            ActionChain nextHeartBeat = new ActionChain();
            nextHeartBeat.AddDelaySeconds(HeartbeatInterval ?? 5);
            nextHeartBeat.AddAction(this, () => HeartBeat());
            nextHeartBeat.EnqueueChain();
        }

        public List<int> GeneratorProfilesActive = new List<int>();

        public void SelectGeneratorProfiles()
        {
            GeneratorProfilesActive.Clear();

            Random random = new Random((int)DateTime.UtcNow.Ticks);

            if (GeneratorProfiles.Count > 0)
            {
                foreach (var profile in GeneratorProfiles)
                {
                    int slot = GeneratorProfiles.IndexOf(profile);

                    if (random.NextDouble() < profile.Probability)
                    {
                        GeneratorProfilesActive.Add(slot);
                    }
                }

            }
        }

        public void UpdateGeneratorInts()
        {
            bool initZero = (InitGeneratedObjects == 0);
            bool maxZero = (MaxGeneratedObjects == 0);

            foreach (int slot in GeneratorProfilesActive)
            {
                if (initZero)
                {
                    InitGeneratedObjects += (int?)GeneratorProfiles[slot].InitCreate;
                }

                if (maxZero)
                {
                    MaxGeneratedObjects += (int?)GeneratorProfiles[slot].MaxCreate;
                }
            }
        }

        public void QueueGenerator()
        {
            foreach(int slot in GeneratorProfilesActive)
            {
                bool slotInUse = false;
                foreach (var obj in GeneratorRegistry)
                {
                    if (obj.Value.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                foreach (var obj in GeneratorQueue)
                {
                    if (obj.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                if (slotInUse)
                    continue;

                var queue = new GeneratorQueueNode();

                queue.Slot = (uint)slot;
                double when = Common.Time.GetFutureTimestamp((RegenerationInterval ?? 0) + (GeneratorProfiles[slot].Delay ?? 0));

                if (GeneratorRegistry.Count < InitGeneratedObjects && !(GeneratorEnteredWorld ?? false))
                    when = Common.Time.GetTimestamp();

                queue.When = when;

                // System.Diagnostics.Debug.WriteLine($"Adding {queue.Slot} @ {queue.When} to GeneratorQueue for {Guid.Full}");
                GeneratorQueue.Add(queue);

                if (GeneratorQueue.Count >= InitGeneratedObjects)
                    GeneratorEnteredWorld = true;
            }
        }

        public void ProcessGeneratorQueue()
        {
            var index = 0;
            while (index < GeneratorQueue.Count)
            {
                double ts = Common.Time.GetTimestamp();
                if (ts >= GeneratorQueue[index].When)
                {
                    if (GeneratorRegistry.Count >= MaxGeneratedObjects)
                    {
                        // System.Diagnostics.Debug.WriteLine($"GeneratorRegistry for {Guid.Full} is at MaxGeneratedObjects {MaxGeneratedObjects}");
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                        GeneratorQueue.RemoveAt(index);
                        index++;
                        continue;
                    }
                    var profile = GeneratorProfiles[(int)GeneratorQueue[index].Slot];

                    var rNode = new GeneratorRegistryNode();

                    rNode.WeenieClassId = profile.WeenieClassId;
                    rNode.Timestamp = Common.Time.GetTimestamp();
                    rNode.Slot = GeneratorQueue[index].Slot;

                    var wo = WorldObjectFactory.CreateNewWorldObject(profile.WeenieClassId);

                    if (wo != null)
                    {
                        switch (profile.WhereCreate)
                        {
                            case 4:
                                wo.Location = new Position(profile.ObjCellId ?? 0,
                                    profile.OriginX ?? 0, profile.OriginY ?? 0, profile.OriginZ ?? 0,
                                    profile.AnglesX ?? 0, profile.AnglesY ?? 0, profile.AnglesZ ?? 0, profile.AnglesW ?? 0);
                                break;
                            default:
                                wo.Location = Location;
                                break;
                        }

                        wo.GeneratorId = Guid.Full;

                        // System.Diagnostics.Debug.WriteLine($"Adding {wo.Guid.Full} | {rNode.Slot} in GeneratorRegistry for {Guid.Full}");
                        GeneratorRegistry.Add(wo.Guid.Full, rNode);
                        // System.Diagnostics.Debug.WriteLine($"Spawning {GeneratorQueue[index].Slot} in GeneratorQueue for {Guid.Full}");
                        wo.EnterWorld();
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                        GeneratorQueue.RemoveAt(index);
                    }
                    else
                    {
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full} because wcid {rNode.WeenieClassId} is not in the database");
                        GeneratorQueue.RemoveAt(index);
                    }
                }
                else
                    index++;
            }
        }

        public void NotifyGeneratorOfPickup(uint guid)
        {
            if (GeneratorRegistry.ContainsKey(guid))
            {
                int slot = (int)GeneratorRegistry[guid].Slot;

                if (GeneratorProfiles[slot].WhenCreate == (uint)RegenerationType.PickUp)
                {
                    GeneratorRegistry.Remove(guid);
                    QueueGenerator();
                }
            }
        }
    }
}
