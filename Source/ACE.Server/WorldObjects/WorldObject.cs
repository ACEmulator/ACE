using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
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
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Util;
using ACE.Server.WorldObjects.Managers;

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
        /// If this object was created from a weenie (and not a database biota), this is the source.
        /// You should never manipulate these values. You should only reference these values in extreme cases.
        /// </summary>
        public Weenie Weenie { get; }

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

        // todo: move these to a base projectile class
        public WorldObject ProjectileSource { get; set; }
        public WorldObject ProjectileTarget { get; set; }

        public WorldObject ProjectileLauncher { get; set; }

        public bool HitMsg;     // FIXME: find a better way to do this for projectiles

        public WorldObject Wielder;

        public WorldObject() { }

        /// <summary>
        /// A new biota will be created taking all of its values from weenie.
        /// </summary>
        protected WorldObject(Weenie weenie, ObjectGuid guid)
        {
            Weenie = weenie;
            Biota = ACE.Entity.Adapter.WeenieConverter.ConvertToBiota(weenie, guid.Full, false, true);
            Guid = guid;

            InitializePropertyDictionaries();
            SetEphemeralValues();
            InitializeGenerator();
            InitializeHeartbeats();

            CreationTimestamp = (int)Time.GetUnixTime();
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

        public bool BumpVelocity { get; set; }

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
                var setupTableId = SetupTableId;

                // TODO: REMOVE ME?
                // Temporary workaround fix to account for ace spawn placement issues with certain hooked objects.
                if (this is Hook)
                {
                    var hookWeenie = DatabaseManager.World.GetCachedWeenie(WeenieClassId);
                    setupTableId = hookWeenie.GetProperty(PropertyDataId.Setup) ?? SetupTableId;
                }
                // TODO: REMOVE ME?

                PhysicsObj = PhysicsObj.makeObject(setupTableId, Guid.Full, isDynamic);
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

            //if (creature != null) AllowEdgeSlide = true;

            if (BumpVelocity)
                PhysicsObj.Velocity = new Vector3(0, 0, 0.5f);
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

            if (!success || PhysicsObj.CurCell == null)
            {
                PhysicsObj.DestroyObject();
                PhysicsObj = null;
                //Console.WriteLine($"AddPhysicsObj: failure: {Name} @ {cell.ID.ToString("X8")} - {Location.Pos} - {Location.Rotation} - SetupID: {SetupTableId.ToString("X8")}, MTableID: {MotionTableId.ToString("X8")}");
                return false;
            }

            //Console.WriteLine($"AddPhysicsObj: success: {Name} ({Guid})");
            SyncLocation();

            SetPosition(PositionType.Home, new Position(Location));

            return true;
        }

        public void SyncLocation()
        {
            Location.LandblockId = new LandblockId(PhysicsObj.Position.ObjCellID);
            Location.Pos = PhysicsObj.Position.Frame.Origin;
            Location.Rotation = PhysicsObj.Position.Frame.Orientation;
        }

        private void InitializePropertyDictionaries()
        {
            if (Biota.PropertiesEnchantmentRegistry == null)
                Biota.PropertiesEnchantmentRegistry = new Collection<PropertiesEnchantmentRegistry>();
        }

        private void SetEphemeralValues()
        { 
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

        public bool HasGiveOrRefuseEmoteForItem(WorldObject item, out PropertiesEmote emote)
        {
            // NPC refuses this item, with a custom response
            var refuseItem = EmoteManager.GetEmoteSet(EmoteCategory.Refuse, null, null, item.WeenieClassId);
            if (refuseItem != null)
            {
                emote = refuseItem;
                return true;
            }            

            // NPC accepts this item
            var giveItem = EmoteManager.GetEmoteSet(EmoteCategory.Give, null, null, item.WeenieClassId);
            if (giveItem != null)
            {
                emote = giveItem;
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
            return PhysicsObj.ObjMaint.VisibleTargetsContainsKey(wo.PhysicsObj.ID);
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

            SightObj.State |= PhysicsState.Missile;

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

            SightObj.State |= PhysicsState.Missile;

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

        public bool IsMeleeVisible(WorldObject wo)
        {
            if (PhysicsObj == null || wo.PhysicsObj == null)
                return false;

            var startPos = new Physics.Common.Position(PhysicsObj.Position);
            var targetPos = new Physics.Common.Position(wo.PhysicsObj.Position);

            PhysicsObj.ProjectileTarget = wo.PhysicsObj;

            // perform line of sight test
            var transition = PhysicsObj.transition(startPos, targetPos, false);

            PhysicsObj.ProjectileTarget = null;

            if (transition == null) return false;

            // check if target object was reached
            var isVisible = transition.CollisionInfo.CollideObject.FirstOrDefault(c => c.ID == wo.PhysicsObj.ID) != null;
            return isVisible;
        }

        public bool IsProjectileVisible(WorldObject proj)
        {
            if (!(this is Creature) || (Ethereal ?? false))
                return true;

            if (PhysicsObj == null || proj.PhysicsObj == null)
                return false;

            var startPos = new Physics.Common.Position(proj.PhysicsObj.Position);
            var targetPos = new Physics.Common.Position(PhysicsObj.Position);

            // set to eye level
            targetPos.Frame.Origin.Z += PhysicsObj.GetHeight() - proj.PhysicsObj.GetHeight();

            var prevTarget = proj.PhysicsObj.ProjectileTarget;
            proj.PhysicsObj.ProjectileTarget = PhysicsObj;

            // perform line of sight test
            var transition = proj.PhysicsObj.transition(startPos, targetPos, false);

            proj.PhysicsObj.ProjectileTarget = prevTarget;

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

        /// <summary>
        /// Flag indicates if RequestedLocation should be broadcast to other players
        /// - For AutoPos packets, this is set to TRUE
        /// - For MoveToState packets, this is set to FALSE
        /// </summary>
        public bool RequestedLocationBroadcast { get; set; }

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
                        sb.AppendLine($"{prop.Name} = {obj.ItemUseable.ToString()}" + " (" + (uint)obj.ItemUseable + ")");
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
                healthPercentage = (float)creature.Health.Current / creature.Health.MaxValue;

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

            ProjectileCollisionHelper.OnCollideObject(this, target);
        }

        public virtual void OnCollideObjectEnd(WorldObject target)
        {
            // empty base
        }

        public virtual void OnCollideEnvironment()
        {
            // thrown weapons
            if (ProjectileTarget == null) return;

            ProjectileCollisionHelper.OnCollideEnvironment(this);
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
            //Console.WriteLine($"{Name}.OnGeneration()");

            EmoteManager.OnGeneration();
        }

        public virtual bool EnterWorld()
        {
            if (Location == null)
                return false;

            if (!LandblockManager.AddObject(this))
                return false;

            if (SuppressGenerateEffect != true)
                ApplyVisualEffects(PlayScript.Create);

            if (Generator != null)
                OnGeneration(Generator);

            //Console.WriteLine($"{Name}.EnterWorld()");

            return true;
        }

        // todo: This should really be an extension method for Position, or a static method within Position or even AdjustPos
        public static void AdjustDungeon(Position pos)
        {
            AdjustDungeonPos(pos);
            AdjustDungeonCells(pos);
        }

        // todo: This should really be an extension method for Position, or a static method within Position or even AdjustPos
        public static bool AdjustDungeonCells(Position pos)
        {
            if (pos == null) return false;

            var landblock = LScape.get_landblock(pos.Cell);
            if (landblock == null || !landblock.HasDungeon) return false;

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

        // todo: This should really be an extension method for Position, or a static method within Position, or even AdjustPos
        public static bool AdjustDungeonPos(Position pos)
        {
            if (pos == null) return false;

            var landblock = LScape.get_landblock(pos.Cell);
            if (landblock == null || !landblock.HasDungeon) return false;

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
        public BaseDamageMod GetDamageMod(Creature wielder, WorldObject weapon = null)
        {
            var baseDamage = GetBaseDamage();

            if (weapon == null)
                weapon = wielder.GetEquippedWeapon();

            var baseDamageMod = new BaseDamageMod(baseDamage, wielder, weapon);

            return baseDamageMod;
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
                //log.WarnFormat("Item 0x{0:X8}:{1} called destroy more than once.", Guid.Full, Name);
                return;
            }

            IsDestroyed = true;

            ReleasedTimestamp = Time.GetUnixTime();

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

            if (this is Pet pet && pet.P_PetOwner?.CurrentActivePet == this)
                pet.P_PetOwner.CurrentActivePet = null;

            if (raiseNotifyOfDestructionEvent)
                NotifyOfEvent(RegenerationType.Destruction);

            if (IsGenerator)
                OnGeneratorDestroy();

            CurrentLandblock?.RemoveWorldObject(Guid);

            RemoveBiotaFromDatabase();

            if (Guid.IsDynamic())
                GuidManager.RecycleDynamicGuid(Guid);
        }

        public void FadeOutAndDestroy(bool raiseNotifyOfDestructionEvent = true)
        {
            EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Destroy));

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
        /// Returns TRUE if this object has non-cyclic animations in progress
        /// </summary>
        public bool IsAnimating { get => PhysicsObj != null && PhysicsObj.IsAnimating; }

        /// <summary>
        /// Executes a motion/animation for this object
        /// adds to the physics animation system, and broadcasts to nearby players
        /// </summary>
        /// <returns>The amount it takes to execute the motion</returns>
        public float ExecuteMotion(Motion motion, bool sendClient = true, float? maxRange = null, bool persist = false)
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

            if (persist && PropertyManager.GetBool("persist_movement").Item)
                motion.Persist(CurrentMotionState);

            // hardcoded ready?
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, CurrentMotionState.MotionState.ForwardCommand, motionCommand);
            CurrentMotionState = motion;

            // broadcast to nearby players
            if (sendClient)
                EnqueueBroadcastMotion(motion, maxRange, false);

            return animLength;
        }

        public float ExecuteMotionPersist(Motion motion, bool sendClient = true, float? maxRange = null)
        {
            return ExecuteMotion(motion, sendClient, maxRange, true);
        }

        public void SetStance(MotionStance stance, bool broadcast = true)
        {
            var motion = new Motion(stance);

            if (PropertyManager.GetBool("persist_movement").Item)
                motion.Persist(CurrentMotionState);

            CurrentMotionState = motion;

            if (broadcast)
                EnqueueBroadcastMotion(CurrentMotionState);
        }

        /// <summary>
        /// Returns the relative direction of this creature in relation to target
        /// expressed as a quadrant: Front/Back, Left/Right
        /// </summary>
        public Quadrant GetRelativeDir(WorldObject target)
        {
            var sourcePos = new Vector3(Location.PositionX, Location.PositionY, 0);
            var targetPos = new Vector3(target.Location.PositionX, target.Location.PositionY, 0);
            var targetDir = new AFrame(target.Location.Pos, target.Location.Rotation).get_vector_heading();

            targetDir.Z = 0;
            targetDir = Vector3.Normalize(targetDir);

            var sourceToTarget = Vector3.Normalize(sourcePos - targetPos);

            var dir = Vector3.Dot(sourceToTarget, targetDir);
            var angle = Vector3.Cross(sourceToTarget, targetDir);

            var quadrant = angle.Z <= 0 ? Quadrant.Left : Quadrant.Right;

            quadrant |= dir >= 0 ? Quadrant.Front : Quadrant.Back;

            return quadrant;
        }

        /// <summary>
        /// Returns TRUE if this WorldObject is a generic linkspot
        /// Linkspots are used for things like Houses,
        /// where the portal destination should be populated at runtime.
        /// </summary>
        public bool IsLinkSpot => WeenieType == WeenieType.Generic && WeenieClassName.Equals("portaldestination");

        public static readonly float LocalBroadcastRange = 96.0f;
        public static readonly float LocalBroadcastRangeSq = LocalBroadcastRange * LocalBroadcastRange;

        public SetPosition ScatterPos { get; set; }

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

        public virtual void OnMoveComplete(WeenieError status)
        {
            // empty base
        }

        public bool IsTradeNote => ItemType == ItemType.PromissoryNote;

        public virtual bool IsAttunedOrContainsAttuned => Attuned >= AttunedStatus.Attuned;

        public virtual bool IsStickyAttunedOrContainsStickyAttuned => Attuned >= AttunedStatus.Sticky;

        public virtual bool IsUniqueOrContainsUnique => Unique != null;

        public virtual List<WorldObject> GetUniqueObjects()
        {
            if (Unique == null)
                return new List<WorldObject>();
            else
                return new List<WorldObject>() { this };
        }

        public bool HasArmorLevel()
        {
            return ArmorLevel > 0;
        }
    }
}
