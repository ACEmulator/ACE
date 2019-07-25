using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Util;

using Landblock = ACE.Server.Entity.Landblock;
using Position = ACE.Entity.Position;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Base Object for Game World
    /// </summary>
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
        public ObjectGuid Guid { get; }

        public PhysicsObj PhysicsObj { get; protected set; }

        public ObjectDescriptionFlag ObjectDescriptionFlags { get; protected set; }

        public SequenceManager Sequences { get; } = new SequenceManager();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        /// <summary>
        /// Should only be adjusted by Landblock -- default is null
        /// </summary>
        public Landblock CurrentLandblock { get; internal set; }

        public DateTime? ItemManaDepletionMessageTimestamp { get; set; } = null;
        public DateTime? ItemManaConsumptionTimestamp { get; set; } = null;

        public bool IsBusy { get; set; }
        public bool IsShield { get => CombatUse != null && CombatUse == ACE.Entity.Enum.CombatUse.Shield; }
        // ValidLocations is bugged for some older two-handed weapons, still contains MeleeWeapon instead of TwoHanded?
        //public bool IsTwoHanded { get => CurrentWieldedLocation != null && CurrentWieldedLocation == EquipMask.TwoHanded; }
        public bool IsTwoHanded => WeaponSkill == Skill.TwoHandedCombat;
        public bool IsBow { get => DefaultCombatStyle != null && (DefaultCombatStyle == CombatStyle.Bow || DefaultCombatStyle == CombatStyle.Crossbow); }
        public bool IsAtlatl { get => DefaultCombatStyle != null && DefaultCombatStyle == CombatStyle.Atlatl; }
        public bool IsAmmoLauncher { get => IsBow || IsAtlatl; }
        public bool IsThrownWeapon { get => DefaultCombatStyle != null && DefaultCombatStyle == CombatStyle.ThrownWeapon; }
        public bool IsRanged { get => IsAmmoLauncher || IsThrownWeapon; }

        public EmoteManager EmoteManager;
        public EnchantmentManagerWithCaching EnchantmentManager;

        public WorldObject ProjectileSource;
        public WorldObject ProjectileTarget;

        public WorldObject Wielder;

        public WorldObject() { }

        /// <summary>
        /// A new biota will be created taking all of its values from weenie.
        /// </summary>
        protected WorldObject(Weenie weenie, ObjectGuid guid)
        {
            Biota = weenie.CreateCopyAsBiota(guid.Full);
            Guid = guid;

            InitializePropertyDictionaries();
            SetEphemeralValues();
            InitializeGenerator();
            InitializeHeartbeats();

            CreationTimestamp = (int)Time.GetUnixTime();

            // TODO: fix weenie data
            if (Lifespan != null)
                RemainingLifespan = Lifespan;
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        protected WorldObject(Biota biota)
        {
            Biota = biota;
            Guid = new ObjectGuid(Biota.Id);

            biotaOriginatedFromDatabase = true;

            InitializePropertyDictionaries();
            SetEphemeralValues();
            InitializeGenerator();
            InitializeHeartbeats();
        }

        /// <summary>
        /// Initializes a new default physics object
        /// </summary>
        public virtual void InitPhysicsObj()
        {
            //Console.WriteLine($"InitPhysicsObj({Name} - {Guid})");

            var defaultState = CalculatedPhysicsState();

            if (!(this is Creature))
            {
                var isDynamic = Static == null || !Static.Value;
                PhysicsObj = PhysicsObj.makeObject(SetupTableId, Guid.Full, isDynamic);
            }
            else
            {
                PhysicsObj = new PhysicsObj();
                PhysicsObj.makeAnimObject(SetupTableId, true);
            }

            PhysicsObj.set_object_guid(Guid);

            PhysicsObj.set_weenie_obj(new WeenieObject(this));

            PhysicsObj.SetMotionTableID(MotionTableId);

            PhysicsObj.SetScaleStatic(ObjScale ?? 1.0f);

            PhysicsObj.State = defaultState;

            // gaerlan rolling balls of death
            if (Name.Equals("Rolling Death"))
            {
                PhysicsObj.SetScaleStatic(1.0f);
                PhysicsObj.State |= PhysicsState.Ethereal;
            }

            //if (creature != null) AllowEdgeSlide = true;
        }

        public bool AddPhysicsObj()
        {
            if (PhysicsObj.CurCell != null)
                return false;

            AdjustDungeon(Location);

            // exclude linkspots from spawning
            if (WeenieClassId == 10762) return true;

            var cell = LScape.get_landcell(Location.Cell);
            if (cell == null)
            {
                PhysicsObj.DestroyObject();
                PhysicsObj = null;
                return false;
            }

            PhysicsObj.Position.ObjCellID = cell.ID;

            var location = new Physics.Common.Position();
            location.ObjCellID = cell.ID;
            location.Frame.Origin = Location.Pos;
            location.Frame.Orientation = Location.Rotation;

            var success = PhysicsObj.enter_world(location);

            if (!success)
            {
                PhysicsObj.DestroyObject();
                PhysicsObj = null;
                //Console.WriteLine($"AddPhysicsObj: failure: {Name} @ {cell.ID.ToString("X8")} - {Location.Pos} - {Location.Rotation} - SetupID: {SetupTableId.ToString("X8")}, MTableID: {MotionTableId.ToString("X8")}");
                return false;
            }

            //Console.WriteLine($"AddPhysicsObj: success: {Name} ({Guid})");
            Location.LandblockId = new LandblockId(PhysicsObj.Position.ObjCellID);
            Location.Pos = PhysicsObj.Position.Frame.Origin;
            Location.Rotation = PhysicsObj.Position.Frame.Orientation;

            SetPosition(PositionType.Home, new Position(Location));

            return true;
        }

        private void InitializePropertyDictionaries()
        {
            foreach (var x in Biota.BiotaPropertiesBool)
                biotaPropertyBools[(PropertyBool)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesDID)
                biotaPropertyDataIds[(PropertyDataId)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesFloat)
                biotaPropertyFloats[(PropertyFloat)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesIID)
                biotaPropertyInstanceIds[(PropertyInstanceId)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesInt)
                biotaPropertyInts[(PropertyInt)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesInt64)
                biotaPropertyInt64s[(PropertyInt64)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesString)
                biotaPropertyStrings[(PropertyString)x.Type] = x;

            foreach (var x in EphemeralProperties.PropertiesBool.ToList())
                ephemeralPropertyBools.TryAdd((PropertyBool)x, null);
            foreach (var x in EphemeralProperties.PropertiesDataId.ToList())
                ephemeralPropertyDataIds.TryAdd((PropertyDataId)x, null);
            foreach (var x in EphemeralProperties.PropertiesDouble.ToList())
                ephemeralPropertyFloats.TryAdd((PropertyFloat)x, null);
            foreach (var x in EphemeralProperties.PropertiesInstanceId.ToList())
                ephemeralPropertyInstanceIds.TryAdd((PropertyInstanceId)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt.ToList())
                ephemeralPropertyInts.TryAdd((PropertyInt)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt64.ToList())
                ephemeralPropertyInt64s.TryAdd((PropertyInt64)x, null);
            foreach (var x in EphemeralProperties.PropertiesString.ToList())
                ephemeralPropertyStrings.TryAdd((PropertyString)x, null);

            foreach (var x in Biota.BiotaPropertiesSpellBook)
                BiotaPropertySpells[x.Spell] = x;
        }

        private void SetEphemeralValues()
        { 
            foreach (var x in Biota.BiotaPropertiesBool.Where(i => EphemeralProperties.PropertiesBool.Contains(i.Type)).ToList())
                ephemeralPropertyBools[(PropertyBool)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesDID.Where(i => EphemeralProperties.PropertiesDataId.Contains(i.Type)).ToList())
                ephemeralPropertyDataIds[(PropertyDataId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesFloat.Where(i => EphemeralProperties.PropertiesDouble.Contains(i.Type)).ToList())
                ephemeralPropertyFloats[(PropertyFloat)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesIID.Where(i => EphemeralProperties.PropertiesInstanceId.Contains(i.Type)).ToList())
                ephemeralPropertyInstanceIds[(PropertyInstanceId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt.Where(i => EphemeralProperties.PropertiesInt.Contains(i.Type)).ToList())
                ephemeralPropertyInts[(PropertyInt)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt64.Where(i => EphemeralProperties.PropertiesInt64.Contains(i.Type)).ToList())
                ephemeralPropertyInt64s[(PropertyInt64)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesString.Where(i => EphemeralProperties.PropertiesString.Contains(i.Type)).ToList())
                ephemeralPropertyStrings[(PropertyString)x.Type] = x.Value;

            foreach (var x in EphemeralProperties.PositionTypes.ToList())
                ephemeralPositions.TryAdd((PositionType)x, null);

            foreach (var x in Biota.BiotaPropertiesPosition.Where(i => EphemeralProperties.PositionTypes.Contains(i.PositionType)).ToList())
                ephemeralPositions[(PositionType)x.PositionType] = new Position(x.ObjCellId, x.OriginX, x.OriginY, x.OriginZ, x.AnglesX, x.AnglesY, x.AnglesZ, x.AnglesW);

            ObjectDescriptionFlags = ObjectDescriptionFlag.Attackable;

            EmoteManager = new EmoteManager(this);
            EnchantmentManager = new EnchantmentManagerWithCaching(this);

            if (Placement == null)
                Placement = ACE.Entity.Enum.Placement.Resting;

            if (MotionTableId != 0)
                CurrentMotionState = new Motion(MotionStance.Invalid);
        }

        /// <summary>
        /// This will be true when teleporting
        /// </summary>
        public bool Teleporting { get; set; } = false;

        public bool HandleNPCReceiveItem(WorldObject item, WorldObject giver, out BiotaPropertiesEmote emote)
        {
            // NPC refuses this item, with a custom response
            var refuseItem = EmoteManager.GetEmoteSet(EmoteCategory.Refuse, null, null, item.WeenieClassId);
            if (refuseItem != null)
            {
                emote = refuseItem;
                EmoteManager.ExecuteEmoteSet(refuseItem, giver);
                return true;
            }            

            // NPC accepts this item
            var giveItem = EmoteManager.GetEmoteSet(EmoteCategory.Give, null, null, item.WeenieClassId);
            if (giveItem != null)
            {
                emote = giveItem;
                EmoteManager.ExecuteEmoteSet(giveItem, giver);
                return true;
            }

            emote = null;
            return false;
        }

        /// <summary>
        /// Returns TRUE if this object has wo in VisibleTargets list
        /// </summary>
        public bool IsVisibleTarget(WorldObject wo)
        {
            if (PhysicsObj == null || wo.PhysicsObj == null)
                return false;

            // note: VisibleTargets is only maintained for monsters and combat pets
            return PhysicsObj.ObjMaint.VisibleTargets.ContainsKey(wo.PhysicsObj.ID);
        }

        //public static PhysicsObj SightObj = PhysicsObj.makeObject(0x02000124, 0, false, true);     // arrow

        /// <summary>
        /// Returns TRUE if this object has direct line-of-sight visibility to input object
        /// </summary>
        public bool IsDirectVisible(WorldObject wo)
        {
            if (PhysicsObj == null || wo.PhysicsObj == null)
                return false;

            var SightObj = PhysicsObj.makeObject(0x02000124, 0, false, true);

            var startPos = new Physics.Common.Position(PhysicsObj.Position);
            var targetPos = new Physics.Common.Position(wo.PhysicsObj.Position);

            if (PhysicsObj.GetBlockDist(startPos, targetPos) > 1)
                return false;

            // set to eye level
            startPos.Frame.Origin.Z += PhysicsObj.GetHeight() - SightObj.GetHeight();
            targetPos.Frame.Origin.Z += wo.PhysicsObj.GetHeight() - SightObj.GetHeight();

            var dir = Vector3.Normalize(targetPos.Frame.Origin - startPos.Frame.Origin);
            var radsum = PhysicsObj.GetPhysicsRadius() + SightObj.GetPhysicsRadius();
            startPos.Frame.Origin += dir * radsum;

            SightObj.CurCell = PhysicsObj.CurCell;
            SightObj.ProjectileTarget = wo.PhysicsObj;

            // perform line of sight test
            var transition = SightObj.transition(startPos, targetPos, false);

            SightObj.DestroyObject();

            if (transition == null) return false;

            // check if target object was reached
            var isVisible = transition.CollisionInfo.CollideObject.FirstOrDefault(c => c.ID == wo.PhysicsObj.ID) != null;
            return isVisible;
        }

        public bool IsDirectVisible(Position pos)
        {
            if (PhysicsObj == null)
                return false;

            var SightObj = PhysicsObj.makeObject(0x02000124, 0, false, true);

            var startPos = new Physics.Common.Position(PhysicsObj.Position);
            var targetPos = new Physics.Common.Position(pos);

            if (PhysicsObj.GetBlockDist(startPos, targetPos) > 1)
                return false;

            // set to eye level
            startPos.Frame.Origin.Z += PhysicsObj.GetHeight() - SightObj.GetHeight();
            targetPos.Frame.Origin.Z += SightObj.GetHeight();

            var dir = Vector3.Normalize(targetPos.Frame.Origin - startPos.Frame.Origin);
            var radsum = PhysicsObj.GetPhysicsRadius() + SightObj.GetPhysicsRadius();
            startPos.Frame.Origin += dir * radsum;

            SightObj.CurCell = PhysicsObj.CurCell;
            SightObj.ProjectileTarget = PhysicsObj;

            // perform line of sight test
            var transition = SightObj.transition(targetPos, startPos, false);

            SightObj.DestroyObject();

            if (transition == null) return false;

            // check if target object was reached
            var isVisible = transition.CollisionInfo.CollideObject.FirstOrDefault(c => c.ID == PhysicsObj.ID) != null;
            return isVisible;
        }

























        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************


        public MoveToState LastMoveToState { get; set; }

        public Position RequestedLocation { get; set; }

        public Position PreviousLocation { get; set; }


        ////// Logical Game Data
        public ContainerType ContainerType
        {
            get
            {
                if (WeenieType == WeenieType.Container)
                    return ContainerType.Container;
                else if (RequiresPackSlot)
                    return ContainerType.Foci;
                else
                    return ContainerType.NonContainer;
            }
        }

        public string DebugOutputString(WorldObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine("ACE Debug Output:");
            sb.AppendLine("ACE Class File: " + GetType().Name + ".cs");
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
                        sb.AppendLine($"{prop.Name} = {ObjectDescriptionFlags.ToString()}" + " (" + (uint)ObjectDescriptionFlags + ")");
                        break;
                    case "weenieflags":
                        var weenieFlags = CalculateWeenieHeaderFlag();
                        sb.AppendLine($"{prop.Name} = {weenieFlags.ToString()}" + " (" + (uint)weenieFlags + ")");
                        break;
                    case "weenieflags2":
                        var weenieFlags2 = CalculateWeenieHeaderFlag2();
                        sb.AppendLine($"{prop.Name} = {weenieFlags2.ToString()}" + " (" + (uint)weenieFlags2 + ")");
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
                        var physicsState = PhysicsObj.State;
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
                        sb.AppendLine($"{prop.Name} = {obj.ClothingPriority}" + " (" + (uint)obj.ClothingPriority + ")");
                        break;
                    case "radarcolor":
                        sb.AppendLine($"{prop.Name} = {obj.RadarColor}" + " (" + (uint)obj.RadarColor + ")");
                        break;
                    case "location":
                        sb.AppendLine($"{prop.Name} = {obj.Location.ToLOCString()}");
                        break;
                    case "destination":
                        sb.AppendLine($"{prop.Name} = {obj.Destination.ToLOCString()}");
                        break;
                    case "instantiation":
                        sb.AppendLine($"{prop.Name} = {obj.Instantiation.ToLOCString()}");
                        break;
                    case "sanctuary":
                        sb.AppendLine($"{prop.Name} = {obj.Sanctuary.ToLOCString()}");
                        break;
                    case "home":
                        sb.AppendLine($"{prop.Name} = {obj.Home.ToLOCString()}");
                        break;
                    case "portalsummonloc":
                        sb.AppendLine($"{prop.Name} = {obj.PortalSummonLoc.ToLOCString()}");
                        break;
                    case "houseboot":
                        sb.AppendLine($"{prop.Name} = {obj.HouseBoot.ToLOCString()}");
                        break;
                    case "lastoutsidedeath":
                        sb.AppendLine($"{prop.Name} = {obj.LastOutsideDeath.ToLOCString()}");
                        break;
                    case "linkedlifestone":
                        sb.AppendLine($"{prop.Name} = {obj.LinkedLifestone.ToLOCString()}");
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
                sb.AppendLine($"PropertyFloat.{Enum.GetName(typeof(PropertyFloat), item.Key)} ({(int)item.Key}) = {item.Value}");
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


        public void EnqueueBroadcastPhysicsState()
        {
            if (PhysicsObj != null)
            {
                if (!Visibility)
                    EnqueueBroadcast(new GameMessageSetState(this, PhysicsObj.State));
                else
                {
                    if (this is Player player && player.CloakStatus == CloakStatus.On)
                    {
                        var ps = PhysicsObj.State;
                        ps &= ~PhysicsState.Cloaked;
                        ps &= ~PhysicsState.NoDraw;
                        player.Session.Network.EnqueueSend(new GameMessageSetState(this, PhysicsObj.State));
                        EnqueueBroadcast(false, new GameMessageSetState(this, ps));
                    }
                    else
                        EnqueueBroadcast(new GameMessageSetState(this, PhysicsObj.State));
                }
            }
        }

        public void EnqueueBroadcastUpdateObject()
        {
            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }

        public virtual void OnCollideObject(WorldObject target)
        {
            // thrown weapons
            if (ProjectileTarget == null) return;

            var proj = new Projectile(this);
            proj.OnCollideObject(target);
        }

        public virtual void OnCollideObjectEnd(WorldObject target)
        {
            // empty base
        }

        public virtual void OnCollideEnvironment()
        {
            // thrown weapons
            if (ProjectileTarget == null) return;

            var proj = new Projectile(this);
            proj.OnCollideEnvironment();
        }

        public void ApplyVisualEffects(PlayScript effect, float speed = 1)
        {
            if (CurrentLandblock != null)
                PlayParticleEffect(effect, Guid, speed);
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId, float speed = 1)
        {
            EnqueueBroadcast(new GameMessageScript(targetId, effectId, speed));
        }

        public void ApplySoundEffects(Sound sound, float volume = 1)
        {
            if (CurrentLandblock != null)
                PlaySoundEffect(sound, Guid, volume);
        }

        public void PlaySoundEffect(Sound soundId, ObjectGuid targetId, float volume = 1)
        {
            EnqueueBroadcast(new GameMessageSound(targetId, soundId, volume));
        }

        public virtual void OnGeneration(WorldObject generator)
        {
            EmoteManager.OnGeneration();
        }

        public virtual void EnterWorld()
        {
            if (Location != null)
            {
                LandblockManager.AddObject(this);

                if (SuppressGenerateEffect != true)
                    ApplyVisualEffects(ACE.Entity.Enum.PlayScript.Create);

                if (Generator != null)
                    OnGeneration(Generator);
            }
        }

        public void AdjustDungeon(Position pos)
        {
            AdjustDungeonPos(pos);
            AdjustDungeonCells(pos);
        }

        public bool AdjustDungeonCells(Position pos)
        {
            if (pos == null) return false;

            var landblock = LScape.get_landblock(pos.Cell);
            if (landblock == null || !landblock.IsDungeon) return false;

            var dungeonID = pos.Cell >> 16;

            var adjustCell = AdjustCell.Get(dungeonID);
            var cellID = adjustCell.GetCell(pos.Pos);

            if (cellID != null && pos.Cell != cellID.Value)
            {
                pos.LandblockId = new LandblockId(cellID.Value);
                return true;
            }
            return false;
        }

        public bool AdjustDungeonPos(Position pos)
        {
            if (pos == null) return false;

            var landblock = LScape.get_landblock(pos.Cell);
            if (landblock == null || !landblock.IsDungeon) return false;

            var dungeonID = pos.Cell >> 16;

            var adjusted = AdjustPos.Adjust(dungeonID, pos);
            return adjusted;
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
        public virtual BaseDamage GetBaseDamage()
        {
            var maxDamage = GetProperty(PropertyInt.Damage) ?? 0;
            var variance = GetProperty(PropertyFloat.DamageVariance) ?? 0;

            return new BaseDamage(maxDamage, (float)variance);
        }

        /// <summary>
        /// Returns the modified damage for a weapon,
        /// with the wielder enchantments taken into account
        /// </summary>
        public BaseDamageMod GetDamageMod(Creature wielder)
        {
            var baseDamage = GetBaseDamage();
            var weapon = wielder.GetEquippedWeapon();

            var baseDamageMod = new BaseDamageMod(baseDamage, wielder, weapon);

            return baseDamageMod;
        }

        /// <summary>
        /// Returns the damage type for the currently equipped weapon / ammo
        /// </summary>
        /// <param name="multiple">If true, returns all of the damage types for the weapon</param>
        public virtual DamageType GetDamageType(bool multiple = false)
        {
            var creature = this as Creature;
            if (creature == null)
            {
                Console.WriteLine("WorldObject.GetDamageType(): null creature");
                return DamageType.Undef;
            }

            var weapon = creature.GetEquippedWeapon();
            var ammo = creature.GetEquippedAmmo();

            if (weapon == null)
                return DamageType.Bludgeon;

            DamageType damageTypes;
            var attackType = creature.GetCombatType();
            if (attackType == CombatType.Melee || ammo == null || !weapon.IsAmmoLauncher)
                damageTypes = (DamageType)(weapon.GetProperty(PropertyInt.DamageType) ?? 0);
            else
                damageTypes = (DamageType)(ammo.GetProperty(PropertyInt.DamageType) ?? 0);

            // returning multiple damage types
            if (multiple) return damageTypes;

            // get single damage type
            var motion = creature.CurrentMotionState.MotionState.ForwardCommand.ToString();
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                if ((damageTypes & damageType) != 0)
                {
                    // handle multiple damage types
                    if (damageType == DamageType.Slash && motion.Contains("Thrust"))
                        continue;

                    return damageType;
                }
            }
            return damageTypes;
        }

        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// If this is a container or a creature, all of the inventory and/or equipped objects will also be destroyed.<para />
        /// An object should only be destroyed once.
        /// </summary>
        public virtual void Destroy(bool raiseNotifyOfDestructionEvent = true)
        {
            if (IsDestroyed)
            {
                log.WarnFormat("Item 0x{0:X8}:{1} called destroy more than once.", Guid.Full, Name);
                return;
            }

            IsDestroyed = true;

            if (this is Container container)
            {
                foreach (var item in container.Inventory.Values)
                    item.Destroy();
            }

            if (this is Creature creature)
            {
                foreach (var item in creature.EquippedObjects.Values)
                    item.Destroy();
            }

            if (this is CombatPet combatPet)
            {
                if (combatPet.P_PetOwner != null && combatPet.P_PetOwner.CurrentActiveCombatPet == this)
                    combatPet.P_PetOwner.CurrentActiveCombatPet = null;
            }

            if (raiseNotifyOfDestructionEvent)
                NotifyOfEvent(RegenerationType.Destruction);

            CurrentLandblock?.RemoveWorldObject(Guid);
            RemoveBiotaFromDatabase();

            if (Guid.IsDynamic())
                GuidManager.RecycleDynamicGuid(Guid);
        }

        public void FadeOutAndDestroy(bool raiseNotifyOfDestructionEvent = true)
        {
            EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Destroy));

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(1.0f);
            actionChain.AddAction(this, () => Destroy(raiseNotifyOfDestructionEvent));
            actionChain.EnqueueChain();
        }

        public string GetPluralName()
        {
            var pluralName = PluralName;

            if (pluralName == null)
                pluralName = Name.Pluralize();

            return pluralName;
        }

        /// <summary>
        /// Returns TRUE if this object has a non-zero velocity
        /// </summary>
        public bool IsMoving { get => PhysicsObj != null && (PhysicsObj.Velocity.X != 0 || PhysicsObj.Velocity.Y != 0 || PhysicsObj.Velocity.Z != 0); }

        /// <summary>
        /// Returns TRUE if this object has non-cyclic animations in progress
        /// </summary>
        public bool IsAnimating { get => PhysicsObj != null && PhysicsObj.IsAnimating; }

        /// <summary>
        /// Executes a motion/animation for this object
        /// adds to the physics animation system, and broadcasts to nearby players
        /// </summary>
        /// <returns>The amount it takes to execute the motion</returns>
        public float ExecuteMotion(Motion motion, bool sendClient = true, float? maxRange = null)
        {
            var motionCommand = motion.MotionState.ForwardCommand;

            if (motionCommand == MotionCommand.Ready)
                motionCommand = (MotionCommand)motion.Stance;

            // run motion command on server through physics animation system
            if (PhysicsObj != null && motionCommand != MotionCommand.Ready)
            {
                var motionInterp = PhysicsObj.get_minterp();

                var rawState = new Physics.Animation.RawMotionState();
                rawState.ForwardCommand = 0;    // always 0? must be this for monster sleep animations (skeletons, golems)
                                                // else the monster will immediately wake back up..
                rawState.CurrentHoldKey = HoldKey.Run;
                rawState.CurrentStyle = (uint)motionCommand;

                if (!PhysicsObj.IsMovingOrAnimating)
                    //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - PhysicsGlobals.MinQuantum;
                    PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

                motionInterp.RawState = rawState;
                motionInterp.apply_raw_movement(true, true);
            }

            // hardcoded ready?
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, CurrentMotionState.MotionState.ForwardCommand, motionCommand);
            CurrentMotionState = motion;

            // broadcast to nearby players
            if (sendClient)
                EnqueueBroadcastMotion(motion, maxRange);

            return animLength;
        }

        public void SetStance(MotionStance stance, bool broadcast = true)
        {
            CurrentMotionState = new Motion(stance);

            if (broadcast)
                EnqueueBroadcastMotion(CurrentMotionState);
        }

        /// <summary>
        /// Returns TRUE if this WorldObject is a generic linkspot
        /// Linkspots are used for things like Houses,
        /// where the portal destination should be populated at runtime.
        /// </summary>
        public bool IsLinkSpot => WeenieType == WeenieType.Generic && WeenieClassName.Equals("portaldestination");

        public static readonly float LocalBroadcastRange = 96.0f;

        public SetPosition ScatterPos;

        public DestinationType DestinationType;

        public Skill ConvertToMoASkill(Skill skill)
        {
            if (this is Player player)
            {
                if (SkillExtensions.RetiredMelee.Contains(skill))
                    return player.GetHighestMeleeSkill();
                if (SkillExtensions.RetiredMissile.Contains(skill))
                    return Skill.MissileWeapons;
            }

            return skill;
        }

        public void GetCurrentMotionState(out MotionStance currentStance, out MotionCommand currentMotion)
        {
            currentStance = MotionStance.Invalid;
            currentMotion = MotionCommand.Ready;

            if (CurrentMotionState != null)
            {
                currentStance = CurrentMotionState.Stance;

                if (CurrentMotionState.MotionState != null)
                    currentMotion = CurrentMotionState.MotionState.ForwardCommand;
            }
        }

        public virtual void HandleMotionDone(uint motionID, bool success)
        {
            // empty base
        }

        public virtual void OnMoveComplete(WeenieError status, int cycles)
        {
            // empty base
        }

        public virtual void OnSticky()
        {
            // empty base
        }

        public virtual void OnUnsticky()
        {
            // empty base
        }

        public virtual bool IsAttunedOrContainsAttuned => (Attuned ?? 0) >= 1;
    }
}
