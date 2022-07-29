using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject 
    {
        public virtual void SerializeUpdateObject(BinaryWriter writer, bool adminvision = false, bool changenodraw = false)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer, false, adminvision, changenodraw);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer, bool adminvision = false, bool changenodraw = false)
        {
            SerializeCreateObject(writer, false, adminvision, changenodraw);
        }

        public virtual void SerializeGameDataOnly(BinaryWriter writer, bool adminvision = false)
        {
            SerializeCreateObject(writer, true, adminvision, false);
        }

        /// <summary>
        /// This is the function used for the GameMessage.ObjDescEvent
        /// </summary>
        /// <param name="writer">Passed from the GameMessageEvent</param>
        public virtual void SerializeUpdateModelData(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            SerializeModelData(writer);
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectVisualDesc));
        }

        private void SerializeCreateObject(BinaryWriter writer, bool gamedataonly, bool adminvision = false, bool changenodraw = false)
        {
            writer.WriteGuid(Guid);

            if (!gamedataonly)
            {
                SerializeModelData(writer);
                SerializePhysicsData(writer, adminvision, changenodraw);
            }

            var weenieFlags =  CalculateWeenieHeaderFlag();
            var weenieFlags2 = CalculateWeenieHeaderFlag2();

            UpdateObjectDescriptionFlags();

            var objDescriptionFlags = ObjectDescriptionFlags;

            if (adminvision)
                objDescriptionFlags &= ~ObjectDescriptionFlag.UiHidden;

            writer.Write((uint)weenieFlags);
            writer.WriteString16L(Name ?? string.Empty);
            writer.WritePackedDword(WeenieClassId);
            writer.WritePackedDwordOfKnownType(IconId, 0x6000000);
            writer.Write((uint)ItemType);
            writer.Write((uint)objDescriptionFlags);
            writer.Align();

            if ((objDescriptionFlags & ObjectDescriptionFlag.IncludesSecondHeader) != 0)
                writer.Write((uint)weenieFlags2);

            if ((weenieFlags & WeenieHeaderFlag.PluralName) != 0)
                writer.WriteString16L(PluralName);

            if ((weenieFlags & WeenieHeaderFlag.ItemsCapacity) != 0)
                writer.Write(ItemCapacity ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.ContainersCapacity) != 0)
                writer.Write(ContainerCapacity ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort?)AmmoType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(Value ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)ItemUseable ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write((uint?)TargetType ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint?)UiEffects ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((sbyte?)CombatUse ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Structure) != 0)
                writer.Write(Structure ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(MaxStructure ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write((ushort?)StackSize ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(MaxStackSize ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(WielderId ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint?)ValidLocations ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.CurrentlyWieldedLocation) != 0)
                writer.Write((uint?)CurrentWieldedLocation ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint?)ClothingPriority ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.RadarBlipColor) != 0)
                writer.Write((byte?)RadarColor ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.RadarBehavior) != 0)
                writer.Write((byte?)RadarBehavior ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.PScript) != 0)
                writer.Write(Script ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(Workmanship ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write((ushort)(EncumbranceVal ?? 0));

            if ((weenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort?)SpellDID ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
            {
                // if mansion, send house owner from master copy
                var houseOwner = HouseOwner;
                var house = this as House;
                if (house != null && house.HouseType == HouseType.Mansion)
                    houseOwner = house.LinkedHouses[0].HouseOwner;

                writer.Write(houseOwner ?? 0);
            }

            if ((weenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
                var house = this as House;

                // if house object is in dungeon,
                // send the permissions from the outdoor house
                if (house.HouseType != HouseType.Apartment && house.CurrentLandblock.IsDungeon)
                {
                    house = house.RootHouse;
                }
                else
                {
                    // if mansion or villa, send permissions from master copy
                    if (house.HouseType == HouseType.Villa || house.HouseType == HouseType.Mansion)
                        house = house.RootHouse;
                }

                writer.Write(new RestrictionDB(house));
            }

            if ((weenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write((uint?)HookItemType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(MonarchId ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(HookType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.WritePackedDwordOfKnownType((IconOverlayId ?? 0), 0x6000000);

            if ((weenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.WritePackedDwordOfKnownType((IconUnderlayId ?? 0), 0x6000000);

            if ((weenieFlags & WeenieHeaderFlag.MaterialType) != 0)
                writer.Write((uint)(MaterialType ?? 0u));

            if ((weenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(CooldownId ?? 0);

            if ((weenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write((double?)CooldownDuration ?? 0u);

            if ((weenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(PetOwner ?? 0);

            writer.Align();
        }

        private void SerializeModelData(BinaryWriter writer)
        {
            var objDesc = CalculateObjDesc();

            writer.Write((byte)0x11);
            writer.Write((byte)objDesc.SubPalettes.Count);
            writer.Write((byte)objDesc.TextureChanges.Count);
            writer.Write((byte)objDesc.AnimPartChanges.Count);

            if (objDesc.SubPalettes.Count > 0)
                writer.WritePackedDwordOfKnownType(objDesc.PaletteID, 0x4000000);

            foreach (var palette in objDesc.SubPalettes)
            {
                writer.WritePackedDwordOfKnownType(palette.SubPaletteId, 0x4000000);
                writer.Write((byte)palette.Offset);
                writer.Write((byte)palette.Length);
            }

            foreach (var texture in objDesc.TextureChanges)
            {
                writer.Write(texture.PartIndex);
                writer.WritePackedDwordOfKnownType(texture.OldTexture, 0x5000000);
                writer.WritePackedDwordOfKnownType(texture.NewTexture, 0x5000000);
            }

            foreach (var model in objDesc.AnimPartChanges)
            {
                writer.Write(model.Index);
                writer.WritePackedDwordOfKnownType(model.AnimationId, 0x1000000);
            }

            writer.Align();
        }

        /// <summary>
        /// Returns the current physics state for an object,
        /// falling back to defaults if no PhysicsObj is loaded (inventory items)
        /// </summary>
        private PhysicsState GetPhysicsStateOrDefault()
        {
            if (PhysicsObj != null)
                return PhysicsObj.State;

            // special case for players logging in - sets pink bubble state here
            if (this is Player)
                return PhysicsState.IgnoreCollisions | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide;

            var defaultObjState = GetProperty(PropertyInt.PhysicsState);

            if (defaultObjState != null)
                return (PhysicsState)defaultObjState;
            else
                return PhysicsGlobals.DefaultState;
        }

        /// <summary>
        /// Sent as part of the CreateObject message, PhysicsDesc in protocol docs
        /// </summary>
        private void SerializePhysicsData(BinaryWriter writer, bool adminvision = false, bool changenodraw = false)
        {
            var physicsDescriptionFlag = CalculatedPhysicsDescriptionFlag();

            if (adminvision && this is Player && CloakStatus == CloakStatus.On)
            {
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Translucency;
            }

            writer.Write((uint)physicsDescriptionFlag);

            var physicsState = GetPhysicsStateOrDefault();

            if (changenodraw)
            {
                physicsState &= ~PhysicsState.NoDraw;
                physicsState &= ~PhysicsState.Cloaked;
            }

            if (this is SpellProjectile && PropertyManager.GetBool("spell_projectile_ethereal").Item)
                physicsState |= PhysicsState.Ethereal;

            writer.Write((uint)physicsState);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                var movementData = new MovementData(this, CurrentMotionState).Serialize();

                writer.Write((uint)movementData.Length);

                if (movementData.Length > 0)
                {
                    writer.Write(movementData);
                    writer.Write(Convert.ToUInt32(CurrentMotionState.IsAutonomous));
                }
            }
            else if ((physicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write(((uint)(Placement ?? ACE.Entity.Enum.Placement.Default)));

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Location.Serialize(writer);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MotionTableId);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.STable) != 0)
                writer.Write(SoundTableId);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.PeTable) != 0)
                writer.Write(PhysicsTableId);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(SetupTableId);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write(WielderId ?? 0);
                writer.Write((uint)(ParentLocation ?? 0));
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(Children.Count);
                foreach (var child in Children)
                {
                    writer.Write(child.Guid);
                    writer.Write(child.LocationId);
                }
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale ?? 0f);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0f);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0f);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                if (adminvision && this is Player && CloakStatus == CloakStatus.On)
                    writer.Write(0.5f);
                else
                    writer.Write(Translucency ?? 0f);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                writer.Write(Velocity);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                writer.Write(Acceleration);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                writer.Write(Omega);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(DefaultScriptId ?? 0);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(DefaultScriptIntensity ?? 0f);

            // timestamps
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectPosition));        // 0
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectMovement));        // 1
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectState));           // 2
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVector));          // 3
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));        // 4
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectServerControl));   // 5
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));   // 6
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVisualDesc));      // 7
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));        // 8

            writer.Align();
        }

        public DateTime LastUpdatePosition;

        /// <summary>
        /// Broadcast position updates to players within range
        /// </summary>
        /// <param name="adminMove">only used if admin is teleporting a non-player object</param>
        public void SendUpdatePosition(bool adminMove = false)
        {
            //Console.WriteLine($"{Name}.SendUpdatePosition({Location.ToLOCString()})");

            EnqueueBroadcast(new GameMessageUpdatePosition(this, adminMove));

            LastUpdatePosition = DateTime.UtcNow;
        }

        public virtual void SendPartialUpdates(Session targetSession, List<GenericPropertyId> properties)
        {
            foreach (var property in properties)
            {
                switch (property.PropertyType)
                {
                    case PropertyType.PropertyInt:
                        int? value = GetProperty((PropertyInt)property.PropertyId);
                        if (value != null)
                            targetSession.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(targetSession.Player, (PropertyInt)property.PropertyId, value.Value));
                        break;
                    default:
                        log.Debug($"Unsupported property in SendPartialUpdates: id {property.PropertyId}, type {property.PropertyType}.");
                        break;
                }
            }
        }

        /// <summary>
        /// Calculates the PhysicsDesc flags from the current object state
        /// </summary>
        protected PhysicsDescriptionFlag CalculatedPhysicsDescriptionFlag()
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            if (CurrentMotionState != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            else if (Placement != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;

            if (Location != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Position;

            if (MotionTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.MTable;

            if (SoundTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.STable;

            if (PhysicsTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.PeTable;

            if (SetupTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.CSetup;

            if (Children.Count != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Children;

            if ((WielderId != null && ParentLocation != null))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Parent;

            // where did this epsilon value come from?
            // why is it different from the physics engine epsilon?
            if ((ObjScale != null) && (Math.Abs(ObjScale ?? 0) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;

            if (Friction != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Friction;

            if (Elasticity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Elasticity;

            if ((Translucency != null) && (Math.Abs(Translucency ?? 0) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Translucency;

            if (Velocity != Vector3.Zero)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Velocity;

            if (Acceleration != Vector3.Zero)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Acceleration;

            if (Omega != Vector3.Zero)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Omega;

            if (DefaultScriptId != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScript;

            if (DefaultScriptIntensity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScriptIntensity;

            return physicsDescriptionFlag;
        }

        private PhysicsState CalculatedPhysicsState()
        {
            // This is doing 2 things. It's pulling the default flags from the PropertyInt.PhysicsState, then in turn, setting the PropertyBool counterparts ONLY if they are null.
            // This seems a bit confusing...
            // If we really want to set default states on create or load, we need to separate this function into two parts.

            // Read in Object's Default PhysicsState
            var physicsState = GetPhysicsStateOrDefault();

            if (physicsState.HasFlag(PhysicsState.Static))
                if (!Static.HasValue)
                    Static = true;
            if (physicsState.HasFlag(PhysicsState.Ethereal))
                if (!Ethereal.HasValue)
                    Ethereal = true;
            if (physicsState.HasFlag(PhysicsState.ReportCollisions))
                if (!ReportCollisions.HasValue)
                    ReportCollisions = true;
            if (physicsState.HasFlag(PhysicsState.IgnoreCollisions))
                if (!IgnoreCollisions.HasValue)
                    IgnoreCollisions = true;
            if (physicsState.HasFlag(PhysicsState.NoDraw))
                if (!NoDraw.HasValue)
                    NoDraw = true;
            if (physicsState.HasFlag(PhysicsState.Missile))
                if (!Missile.HasValue)
                    Missile = true;
            if (physicsState.HasFlag(PhysicsState.Pushable))
                if (!Pushable.HasValue)
                    Pushable = true;
            if (physicsState.HasFlag(PhysicsState.AlignPath))
                if (!AlignPath.HasValue)
                    AlignPath = true;
            if (physicsState.HasFlag(PhysicsState.PathClipped))
                if (!PathClipped.HasValue)
                    PathClipped = true;
            if (physicsState.HasFlag(PhysicsState.Gravity))
                if (!GravityStatus.HasValue)
                    GravityStatus = true;
            if (physicsState.HasFlag(PhysicsState.LightingOn))
                if (!LightsStatus.HasValue)
                    LightsStatus = true;
            if (physicsState.HasFlag(PhysicsState.ParticleEmitter))
                if (!ParticleEmitter.HasValue)
                    ParticleEmitter = true;
            if (physicsState.HasFlag(PhysicsState.Hidden))
                if (!Hidden.HasValue)
                    Hidden = true;
            if (physicsState.HasFlag(PhysicsState.ScriptedCollision))
                if (!ScriptedCollision.HasValue)
                    ScriptedCollision = true;
            if (physicsState.HasFlag(PhysicsState.Inelastic))
                if (!Inelastic.HasValue)
                    Inelastic = true;
            if (physicsState.HasFlag(PhysicsState.Cloaked))
                if (!Cloaked.HasValue)
                    Cloaked = true;
            if (physicsState.HasFlag(PhysicsState.ReportCollisionsAsEnvironment))
                if (!ReportCollisionsAsEnvironment.HasValue)
                    ReportCollisionsAsEnvironment = true;
            if (physicsState.HasFlag(PhysicsState.EdgeSlide))
                if (!AllowEdgeSlide.HasValue)
                    AllowEdgeSlide = true;
            if (physicsState.HasFlag(PhysicsState.Sledding))
                if (!Sledding.HasValue)
                    Sledding = true;
            if (physicsState.HasFlag(PhysicsState.Frozen))
                if (!IsFrozen.HasValue)
                    IsFrozen = true;

            ////Static                      = 0x00000001,
            if (Static ?? false)
                physicsState |= PhysicsState.Static;
            else
                physicsState &= ~PhysicsState.Static;
            ////Unused1                     = 0x00000002,
            ////Ethereal                    = 0x00000004,
            if (Ethereal ?? false)
                physicsState |= PhysicsState.Ethereal;
            else
                physicsState &= ~PhysicsState.Ethereal;
            ////ReportCollision             = 0x00000008,
            if (ReportCollisions ?? false)
                physicsState |= PhysicsState.ReportCollisions;
            else
                physicsState &= ~PhysicsState.ReportCollisions;
            ////IgnoreCollision             = 0x00000010,
            if (IgnoreCollisions ?? false)
                physicsState |= PhysicsState.IgnoreCollisions;
            else
                physicsState &= ~PhysicsState.IgnoreCollisions;
            ////NoDraw                      = 0x00000020,
            if (NoDraw ?? false)
                physicsState |= PhysicsState.NoDraw;
            else
                physicsState &= ~PhysicsState.NoDraw;
            ////Missile                     = 0x00000040,
            if (Missile ?? false)
                physicsState |= PhysicsState.Missile;
            else
                physicsState &= ~PhysicsState.Missile;
            ////Pushable                    = 0x00000080,
            if (Pushable ?? false)
                physicsState |= PhysicsState.Pushable;
            else
                physicsState &= ~PhysicsState.Pushable;
            ////AlignPath                   = 0x00000100,
            if (AlignPath ?? false)
                physicsState |= PhysicsState.AlignPath;
            else
                physicsState &= ~PhysicsState.AlignPath;
            ////PathClipped                 = 0x00000200,
            if (PathClipped ?? false)
                physicsState |= PhysicsState.PathClipped;
            else
                physicsState &= ~PhysicsState.PathClipped;
            ////Gravity                     = 0x00000400,
            if (GravityStatus ?? false)
                physicsState |= PhysicsState.Gravity;
            else
                physicsState &= ~PhysicsState.Gravity;
            ////LightingOn                  = 0x00000800,
            if (LightsStatus ?? false)
                physicsState |= PhysicsState.LightingOn;
            else
                physicsState &= ~PhysicsState.LightingOn;
            ////ParticleEmitter             = 0x00001000,
            if (ParticleEmitter ?? false)
                physicsState |= PhysicsState.ParticleEmitter;
            else
                physicsState &= ~PhysicsState.ParticleEmitter;
            ////Unused2                     = 0x00002000,
            ////Hidden                      = 0x00004000,
            if (Hidden ?? false)
                physicsState |= PhysicsState.Hidden;
            else
                physicsState &= ~PhysicsState.Hidden;
            ////ScriptedCollision           = 0x00008000,
            if (ScriptedCollision ?? false)
                physicsState |= PhysicsState.ScriptedCollision;
            else
                physicsState &= ~PhysicsState.ScriptedCollision;
            ////HasPhysicsBSP               = 0x00010000,
            if (CSetup.HasPhysicsBSP)
                physicsState |= PhysicsState.HasPhysicsBSP;
            else
                physicsState &= ~PhysicsState.HasPhysicsBSP;
            ////Inelastic                   = 0x00020000,
            if (Inelastic ?? false)
                physicsState |= PhysicsState.Inelastic;
            else
                physicsState &= ~PhysicsState.Inelastic;
            ////HasDefaultAnim              = 0x00040000,
            if (PhysicsObj != null && PhysicsObj.HasDefaultAnimation && CSetup.DefaultAnimation > 0)
                physicsState |= PhysicsState.HasDefaultAnim;
            else
                physicsState &= ~PhysicsState.HasDefaultAnim;
            ////HasDefaultScript            = 0x00080000,
            if (PhysicsObj != null && PhysicsObj.HasDefaultScript && CSetup.DefaultScript > 0)
                physicsState |= PhysicsState.HasDefaultScript;
            else
                physicsState &= ~PhysicsState.HasDefaultScript;
            ////Cloaked                     = 0x00100000,
            if (Cloaked ?? false)
                physicsState |= PhysicsState.Cloaked;
            else
                physicsState &= ~PhysicsState.Cloaked;
            ////ReportCollisionAsEnviroment = 0x00200000,
            if (ReportCollisionsAsEnvironment ?? false)
                physicsState |= PhysicsState.ReportCollisionsAsEnvironment;
            else
                physicsState &= ~PhysicsState.ReportCollisionsAsEnvironment;
            ////EdgeSlide                   = 0x00400000,
            if (AllowEdgeSlide ?? false)
                physicsState |= PhysicsState.EdgeSlide;
            else
                physicsState &= ~PhysicsState.EdgeSlide;
            ////Sledding                    = 0x00800000,
            if (Sledding ?? false)
                physicsState |= PhysicsState.Sledding;
            else
                physicsState &= ~PhysicsState.Sledding;
            ////Frozen                      = 0x01000000,
            if (IsFrozen ?? false)
                physicsState |= PhysicsState.Frozen;
            else
                physicsState &= ~PhysicsState.Frozen;

            return physicsState;
        }

        protected WeenieHeaderFlag CalculateWeenieHeaderFlag()
        {
            var weenieHeaderFlag = WeenieHeaderFlag.None;

            if (PluralName != null)
                weenieHeaderFlag |= WeenieHeaderFlag.PluralName;

            if (ItemCapacity != null && !(this is SlumLord))
                weenieHeaderFlag |= WeenieHeaderFlag.ItemsCapacity;

            if (ContainerCapacity != null && !(this is SlumLord))
                weenieHeaderFlag |= WeenieHeaderFlag.ContainersCapacity;

            if (AmmoType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.AmmoType;

            if (Value != null && (Value > 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Value;

            if (ItemUseable != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Usable;

            if (UseRadius != null)
                weenieHeaderFlag |= WeenieHeaderFlag.UseRadius;

            if (TargetType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.TargetType;

            if (UiEffects != null)
                weenieHeaderFlag |= WeenieHeaderFlag.UiEffects;

            if (CombatUse != null)
                weenieHeaderFlag |= WeenieHeaderFlag.CombatUse;

            if (Structure != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Structure;

            if (MaxStructure != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaxStructure;

            if (StackSize != null)
                weenieHeaderFlag |= WeenieHeaderFlag.StackSize;

            if (MaxStackSize != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaxStackSize;

            if (ContainerId != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Container;

            if (WielderId != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Wielder;

            if (ValidLocations != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ValidLocations;

            if ((CurrentWieldedLocation != null) && (CurrentWieldedLocation != 0) && (WielderId != null) && (WielderId != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.CurrentlyWieldedLocation;

            if (ClothingPriority != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Priority;

            if (RadarColor != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBlipColor;

            if (RadarBehavior != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBehavior;

            var physicsScriptDID = GetProperty(PropertyDataId.PhysicsScript);
            if ((physicsScriptDID != null) && (physicsScriptDID != 0u))
                weenieHeaderFlag |= WeenieHeaderFlag.PScript;

            if ((Workmanship != null) && (uint?)Workmanship != 0u)
                weenieHeaderFlag |= WeenieHeaderFlag.Workmanship;

            if ((EncumbranceVal != null) && (EncumbranceVal != 0) && !(this is Creature) && !(this is SpellProjectile))
                weenieHeaderFlag |= WeenieHeaderFlag.Burden;

            if ((SpellDID != null) && (SpellDID != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Spell;

            var houseOwner = HouseOwner;
            var house = this as House;
            if (house != null)
            {
                weenieHeaderFlag |= WeenieHeaderFlag.HouseRestrictions;

                if (house.HouseType == HouseType.Mansion)
                    houseOwner = house.LinkedHouses[0].HouseOwner;
            }

            if (houseOwner != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HouseOwner;

            var hookItemTypeInt = GetProperty(PropertyInt.HookItemType);
            if (hookItemTypeInt != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookItemTypes;

            if (MonarchId != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Monarch;

            if (HookType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookType;

            if ((IconOverlayId != null) && (IconOverlayId != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.IconOverlay;

            if (MaterialType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaterialType;

            return weenieHeaderFlag;
        }

        private WeenieHeaderFlag2 CalculateWeenieHeaderFlag2()
        {
            var weenieHeaderFlag2 = WeenieHeaderFlag2.None;

            if ((IconUnderlayId != null) && (IconUnderlayId != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.IconUnderlay;

            if ((CooldownId != null) && (CooldownId != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.Cooldown;

            if ((CooldownDuration != null) && Math.Abs((float)CooldownDuration) >= 0.001)
                weenieHeaderFlag2 |= WeenieHeaderFlag2.CooldownDuration;

            if ((PetOwner != null) && (PetOwner != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.PetOwner;

            return weenieHeaderFlag2;
        }

        private void UpdateObjectDescriptionFlags()
        {
            if (WeenieType == WeenieType.Container || WeenieType == WeenieType.Corpse || WeenieType == WeenieType.Chest
                || WeenieType == WeenieType.Hook || WeenieType == WeenieType.Storage)
            {
                UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Openable, !IsLocked);
            }

            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Inscribable, Inscribable);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Stuck, Stuck);

            if (WeenieType == WeenieType.Admin || WeenieType == WeenieType.Sentinel)
                UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Player, CloakStatus < CloakStatus.Creature);

            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Attackable, Attackable);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.PlayerKiller, PlayerKillerStatus == PlayerKillerStatus.PK);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.HiddenAdmin, HiddenAdmin);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.UiHidden, UiHidden);

            if (WeenieType == WeenieType.Admin || WeenieType == WeenieType.Sentinel)
                UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Admin, CloakStatus < CloakStatus.Player);

            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.FreePkStatus, PlayerKillerStatus == PlayerKillerStatus.Free);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.ImmuneCellRestrictions, IgnoreHouseBarriers);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.RequiresPackSlot, RequiresPackSlot);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.Retained, Retained);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.PkLiteStatus, PlayerKillerStatus == PlayerKillerStatus.PKLite);

            var weenieFlags2 = CalculateWeenieHeaderFlag2();

            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.IncludesSecondHeader, weenieFlags2 > WeenieHeaderFlag2.None);

            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.WieldOnUse, WieldOnUse);
            UpdateObjectDescriptionFlag(ObjectDescriptionFlag.WieldLeft, WieldLeft);
        }

        private void UpdateObjectDescriptionFlag(ObjectDescriptionFlag flag, bool value)
        {
            if (value)
                ObjectDescriptionFlags |= flag;
            else
                ObjectDescriptionFlags &= ~flag;
        }

        public bool? IgnoreCloIcons
        {
            get => GetProperty(PropertyBool.IgnoreCloIcons);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnoreCloIcons); else SetProperty(PropertyBool.IgnoreCloIcons, value.Value); }
        }

        public bool? Dyable
        {
            get => GetProperty(PropertyBool.Dyable);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Dyable); else SetProperty(PropertyBool.Dyable, value.Value); }
        }

        public virtual ACE.Entity.ObjDesc CalculateObjDesc()
        {
            if (this is Hook hook && hook.HasItem)
                return hook.Item.CalculateObjDesc();

            ACE.Entity.ObjDesc objDesc = new ACE.Entity.ObjDesc();
            ClothingTable item;

            AddBaseModelData(objDesc);

            if (ClothingBase.HasValue)
                item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)ClothingBase);
            else
            {
                return objDesc;
            }

            if (item.ClothingBaseEffects.ContainsKey(SetupTableId))
            // Check if the ClothingBase is applicable for this Setup. (Gear Knights, this is usually you.)
            {
                // Add the model and texture(s)
                ClothingBaseEffect clothingBaseEffect = item.ClothingBaseEffects[SetupTableId];
                foreach (CloObjectEffect t in clothingBaseEffect.CloObjectEffects)
                {
                    byte partNum = (byte)t.Index;
                    objDesc.AnimPartChanges.Add(new PropertiesAnimPart { Index = (byte)t.Index, AnimationId = t.ModelId });
                    foreach (CloTextureEffect t1 in t.CloTextureEffects)
                        objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = (byte)t.Index, OldTexture = t1.OldTexture, NewTexture = t1.NewTexture });
                }

                //if (item.ClothingSubPalEffects.Count == 1 && (PaletteTemplate.HasValue | Shade.HasValue))
                //    Console.WriteLine($"Found an item with 1 ClothingSubPalEffects and a PaletteTemplate = {PaletteTemplate} and/or Shade = {Shade} ");

                // If there are no ClothingSubPalEffects, or this item has no Shade and no PaletteTemplate set, we will not apply any Palette changes
                if (item.ClothingSubPalEffects.Count > 0 && (Shade.HasValue || PaletteTemplate.HasValue))
                { 
                    CloSubPalEffect itemSubPal;
                    int palOption = 0;
                    if (PaletteTemplate.HasValue)
                        palOption = (int)PaletteTemplate;

                    // Load the correct ClothingSubPalEffects for the assigned PaletteTemplate, or the first in the Dictionary if none set or it is set to an invalid value
                    if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
                        itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
                    else
                        itemSubPal = item.ClothingSubPalEffects[item.ClothingSubPalEffects.Keys.ElementAt(0)];

                    if (itemSubPal.Icon > 0 && !(IgnoreCloIcons ?? false))
                        IconId = itemSubPal.Icon;

                    float shade = 0; // Default if no shade is set.
                    if (Shade.HasValue)
                        shade = (float)Shade;

                    for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                    {
                        var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
                        ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

                        for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            ushort palOffset = (ushort)(itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8);
                            ushort numColors = (ushort)(itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8);
                            objDesc.SubPalettes.Add(new PropertiesPalette { SubPaletteId = itemPal, Offset = palOffset, Length = numColors });
                        }
                    }
                }
            }

            return objDesc;
        }

        protected void AddBaseModelData(ACE.Entity.ObjDesc objDesc)
        {
            // Hair/head

            // if (HeadObjectDID.HasValue && !HairStyle.HasValue)
            // This Heritage check has been added for backwards compatibility. It works around the butthead Gear Knights appearance.
            if (HeadObjectDID.HasValue && !HairStyle.HasValue && Heritage.HasValue && Heritage != (int)HeritageGroup.Gearknight)
                objDesc.AnimPartChanges.Add(new PropertiesAnimPart { Index = 0x10, AnimationId = HeadObjectDID.Value });
            else if (HairStyle.HasValue && Heritage.HasValue && Gender.HasValue)
            {
                // This indicates we have a Gear Knight or Olthoi(that is, player types treat "hairstyle" as a "Body Style")

                // Load the CharGen data. It has all the anim & texture changes for the Body Style defined within it
                var cg = DatManager.PortalDat.CharGen;
                SexCG sex = cg.HeritageGroups[(uint)Heritage].Genders[(int)Gender];
                if (sex.HairStyleList.Count > (int)HairStyle) // just check for a valid entry...
                {
                    HairStyleCG hairstyle = sex.HairStyleList[(int)HairStyle];

                    // Add all the texture changes
                    foreach (var tm in hairstyle.ObjDesc.TextureChanges)
                        objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = tm.PartIndex, OldTexture = tm.OldTexture, NewTexture = tm.NewTexture });

                    // Add all the animation part changes
                    foreach (var part in hairstyle.ObjDesc.AnimPartChanges)
                        objDesc.AnimPartChanges.Add(new PropertiesAnimPart { Index = part.PartIndex, AnimationId = part.PartID });
                }
            }

            if (this is Player player)
                objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = 0x10, OldTexture = player.Character.DefaultHairTexture, NewTexture = player.Character.HairTexture });
            //AddTexture(0x10, DefaultHairTextureDID.Value, HairTextureDID.Value);
            if (HairPaletteDID.HasValue)
                objDesc.SubPalettes.Add(new PropertiesPalette { SubPaletteId = HairPaletteDID.Value, Offset = 0x18, Length = 0x8 });
            //AddPalette(HairPaletteDID.Value, 0x18, 0x8);

            // Skin
            // PaletteBaseId = PaletteBaseDID;
            if (PaletteBaseDID.HasValue)
                objDesc.PaletteID = PaletteBaseDID.Value;
            if (SkinPaletteDID.HasValue)
                objDesc.SubPalettes.Add(new PropertiesPalette { SubPaletteId = SkinPaletteDID.Value, Offset = 0x0, Length = 0x18 });
            //AddPalette(SkinPalette.Value, 0x0, 0x18);

            // Eyes
            if (DefaultEyesTextureDID.HasValue && EyesTextureDID.HasValue)
                objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = 0x10, OldTexture = DefaultEyesTextureDID.Value, NewTexture = EyesTextureDID.Value });
            //AddTexture(0x10, DefaultEyesTextureDID.Value, EyesTextureDID.Value);
            if (EyesPaletteDID.HasValue)
                objDesc.SubPalettes.Add(new PropertiesPalette { SubPaletteId = EyesPaletteDID.Value, Offset = 0x20, Length = 0x8 });
            //AddPalette(EyesPaletteDID.Value, 0x20, 0x8);

            // Nose & Mouth
            if (DefaultNoseTextureDID.HasValue && NoseTextureDID.HasValue)
                objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = 0x10, OldTexture = DefaultNoseTextureDID.Value, NewTexture = NoseTextureDID.Value });
            //AddTexture(0x10, NoseTextureDID.Value, NoseTextureDID.Value);
            if (DefaultMouthTextureDID.HasValue && MouthTextureDID.HasValue)
                objDesc.TextureChanges.Add(new PropertiesTextureMap { PartIndex = 0x10, OldTexture = DefaultMouthTextureDID.Value, NewTexture = MouthTextureDID.Value });
            //AddTexture(0x10, DefaultMouthTextureDID.Value, MouthTextureDID.Value);
        }

        /// <summary>
        /// Runs a function for all Players that currently know about this object
        /// </summary>
        public void EnqueueActionBroadcast(Action<Player> delegateAction, bool excludeSelf = false)
        {
            if (PhysicsObj == null) return;

            if (!excludeSelf && this is Player self)
                self.EnqueueAction(new ActionEventDelegate(() => delegateAction(self)));

            foreach (var player in PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer())
            {
                if (Visibility && !player.Adminvision)
                    continue;

                if (excludeSelf && this == player)
                    continue;

                player.EnqueueAction(new ActionEventDelegate(() => delegateAction(player)));
            }
        }

        /// <summary>
        /// Traverses the owner list for the input item,
        /// and returns the object in the current landblock
        /// </summary>
        public WorldObject GetParentLandblock(WorldObject item)
        {
            var iterator = item;
            while (iterator.CurrentLandblock == null)
            {
                if (iterator.OwnerId == null)
                    break;

                iterator = CurrentLandblock.GetObject(iterator.OwnerId.Value);
            }
            return iterator.CurrentLandblock == null ? null : iterator;
        }

        public float EnqueueMotionMagic(ActionChain actionChain, MotionCommand motionCommand, float speed = 1.0f)
        {
            var motion = new Motion(MotionStance.Magic, motionCommand, speed);
            motion.MotionState.TurnSpeed = 2.25f;  // ??

            var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, MotionStance.Magic, motionCommand, speed);

            actionChain.AddAction(this, () =>
            {
                if (this is Player player && player.MagicState.IsCasting)
                    EnqueueBroadcastMotion(motion);
            });
            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotion(ActionChain actionChain, MotionCommand motionCommand, float speed = 1.0f, bool useStance = true, MotionCommand? prevCommand = null, bool castGesture = false, bool half = false)
        {
            var stance = CurrentMotionState != null && useStance ? CurrentMotionState.Stance : MotionStance.NonCombat;

            if (castGesture)
                stance = MotionStance.Magic;

            var motion = new Motion(stance, motionCommand, speed);
            motion.MotionState.TurnSpeed = 2.25f;  // ??

            var animLength = 0.0f;
            if (prevCommand != null)
            {
                animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, prevCommand.Value, motionCommand, speed);
            }
            else
                animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);

            actionChain.AddAction(this, () =>
            {
                if (castGesture && this is Player player && !player.MagicState.IsCasting)
                    return;

                CurrentMotionState = motion;
                EnqueueBroadcastMotion(motion);
            });

            if (half)
                animLength *= 0.5f;

            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotion(ActionChain actionChain, MotionStance stance, MotionCommand motionCommand, float speed = 1.0f)
        {
            // specialized function to mitigate odd client behavior w/ swapping bows during repeat attacks
            // TODO: fix the CurrentMotionState mess
            var motion = new Motion(stance, motionCommand, speed);
            motion.MotionState.TurnSpeed = 2.25f;  // ??

            var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);

            actionChain.AddAction(this, () =>
            {
                // if no longer in missile combat, don't bother
                if (this is Player player && player.CombatMode != CombatMode.Missile) return;

                // retain original profile of function, but if something else has changed the stance (such as weapon swapping),
                // do not thrash CurrentMotionState.Stance
                if (CurrentMotionState.Stance == stance)
                    CurrentMotionState = motion;

                EnqueueBroadcastMotion(motion);
            });
            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotionPersist(ActionChain actionChain, MotionStance stance, MotionCommand motionCommand, float speed = 1.0f)
        {
            if (!PropertyManager.GetBool("persist_movement").Item)
            {
                return EnqueueMotion(actionChain, stance, motionCommand, speed);
            }

            // specialized function to mitigate odd client behavior w/ swapping bows during repeat attacks
            // TODO: fix the CurrentMotionState mess
            var motion = new Motion(stance, motionCommand, speed);
            motion.MotionState.TurnSpeed = 2.25f;  // ??

            var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);

            actionChain.AddAction(this, () =>
            {
                // if no longer in missile combat, don't bother
                if (this is Player player && player.CombatMode != CombatMode.Missile) return;

                // retain original profile of function, but if something else has changed the stance (such as weapon swapping),
                // do not thrash CurrentMotionState.Stance
                if (CurrentMotionState.Stance == stance)
                    CurrentMotionState = motion;

                motion.Persist(CurrentMotionState);

                EnqueueBroadcastMotion(motion);
            });
            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotionPersist(ActionChain actionChain, MotionCommand motionCommand, float speed = 1.0f, bool useStance = true, MotionCommand? prevCommand = null, bool castGesture = false, bool half = false)
        {
            if (!PropertyManager.GetBool("persist_movement").Item)
            {
                return EnqueueMotion(actionChain, motionCommand, speed, useStance, prevCommand, castGesture, half);
            }

            var stance = CurrentMotionState != null && useStance ? CurrentMotionState.Stance : MotionStance.NonCombat;

            if (castGesture)
                stance = MotionStance.Magic;

            var animLength = 0.0f;
            if (prevCommand != null)
            {
                animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, prevCommand.Value, motionCommand, speed);
            }
            else
                animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);

            actionChain.AddAction(this, () =>
            {
                if (castGesture && this is Player player && !player.MagicState.IsCasting)
                    return;

                var motion = new Motion(stance, motionCommand, speed);
                motion.Persist(CurrentMotionState);

                motion.MotionState.TurnSpeed = 2.25f;  // ??

                CurrentMotionState = motion;
                EnqueueBroadcastMotion(motion);
            });

            if (half)
                animLength *= 0.5f;

            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotionAction(ActionChain actionChain, List<MotionCommand> motionCommands, float speed = 1.0f, MotionStance? useStance = null, bool usePrevCommand = false)
        {
            var stance = useStance ?? CurrentMotionState.Stance;

            var motion = new Motion(stance, MotionCommand.Ready, speed);

            foreach (var motionCommand in motionCommands)
                motion.MotionState.AddCommand(this, motionCommand, speed);

            motion.MotionState.TurnSpeed = 2.25f;  // ??

            var animLength = 0.0f;
            if (usePrevCommand)
            {
                var prevCommand = CurrentMotionState.MotionState.ForwardCommand;

                foreach (var motionCommand in motionCommands)
                    animLength += Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, prevCommand, motionCommand, speed);
            }
            else
            {
                foreach (var motionCommand in motionCommands)
                    animLength += Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);
            }

            actionChain.AddAction(this, () =>
            {
                CurrentMotionState = motion;
                EnqueueBroadcastMotion(motion, null, false);

                ApplyPhysicsMotion(new Motion(stance, MotionCommand.Ready, speed));

                foreach (var motionCommand in motionCommands)
                    ApplyPhysicsMotion(new Motion(stance, motionCommand, speed));
            });

            actionChain.AddDelaySeconds(animLength);

            return animLength;
        }

        public float EnqueueMotion_Force(ActionChain actionChain, MotionStance stance, MotionCommand motionCommand, MotionCommand? prevCommand = null, float speed = 1.0f, float animMod = 1.0f)
        {
            var motion = new Motion(stance, motionCommand, speed);

            var animLength = 0.0f;

            if (prevCommand == null)
                animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, motionCommand, speed);
            else
            {
                var isStance = Enum.IsDefined(typeof(MotionStance), (uint)prevCommand);

                if (isStance)
                    animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, (MotionStance)prevCommand, motionCommand, (MotionCommand)stance, speed);
                else
                    animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, stance, prevCommand.Value, motionCommand, speed);
            }

            actionChain.AddAction(this, () =>
            {
                CurrentMotionState = motion;

                EnqueueBroadcastMotion(motion);
            });

            actionChain.AddDelaySeconds(animLength * animMod);
            return animLength;
        }

        public static bool EnqueueBroadcastMotion_Physics = true;

        public void EnqueueBroadcastMotion(Motion motion, float? maxRange = null, bool? applyPhysics = null)
        {
            if (applyPhysics == null)
            {
                if (this is Player player)
                    applyPhysics = player.FastTick;
                else
                    applyPhysics = false;
            }

            var msg = new GameMessageUpdateMotion(this, motion);

            if (maxRange == null)
                EnqueueBroadcast(msg);
            else
                EnqueueBroadcast(msg, maxRange.Value);

            if (EnqueueBroadcastMotion_Physics && applyPhysics.Value)
                ApplyPhysicsMotion(motion);
        }

        public void ApplyPhysicsMotion(Motion motion)
        {
            var minterp = PhysicsObj.get_minterp();
            var rawState = minterp.RawState;

            var allowJump = minterp.motion_allows_jump(minterp.InterpretedState.ForwardCommand) == WeenieError.None;

            rawState.CurrentStyle = (uint)motion.Stance;
            rawState.ForwardCommand = (uint)motion.MotionState.ForwardCommand;
            rawState.ForwardSpeed = motion.MotionState.ForwardSpeed;

            if (!PhysicsObj.IsMovingOrAnimating)
                //PhysicsObj.UpdateTime = Physics.Common.PhysicsTimer.CurrentTime - PhysicsGlobals.MinQuantum;
                PhysicsObj.UpdateTime = Physics.Common.PhysicsTimer.CurrentTime;

            minterp.apply_raw_movement(true, allowJump);
        }


        /// <summary>
        /// Returns TRUE if there are any players within range of this object
        /// </summary>
        public bool PlayersInRange(float range = 96.0f)
        {
            var isDungeon = CurrentLandblock != null && CurrentLandblock.PhysicsLandblock != null && CurrentLandblock.PhysicsLandblock.IsDungeon;

            var rangeSquared = range * range;

            foreach (var player in PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer())
            {
                if (isDungeon && Location.Landblock != player.Location.Landblock)
                    continue;

                if (Visibility && !player.Adminvision)
                    continue;

                //var dist = Vector3.Distance(Location.ToGlobal(), player.Location.ToGlobal());
                //var distSquared = Vector3.DistanceSquared(Location.ToGlobal(), player.Location.ToGlobal());
                var distSquared = Location.SquaredDistanceTo(player.Location);
                if (distSquared <= rangeSquared)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sends network messages to all Players who currently know about this object
        /// within a maximum range
        /// </summary>
        public void EnqueueBroadcast(GameMessage msg, float range, ChatMessageType? squelchType = null)
        {
            if (PhysicsObj == null || CurrentLandblock == null) return;

            Player self = null;
            if (this is Player)
            {
                self = this as Player;
                self.Session.Network.EnqueueSend(msg);
            }

            var isDungeon = CurrentLandblock.PhysicsLandblock != null && CurrentLandblock.PhysicsLandblock.IsDungeon;

            var rangeSquared = range * range;

            foreach (var player in PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer())
            {
                if (self != null && squelchType != null && player.SquelchManager.Squelches.Contains(self, squelchType.Value))
                    continue;

                if (isDungeon && Location.Landblock != player.Location.Landblock)
                    continue;

                if (Visibility && !player.Adminvision)
                    continue;

                //var dist = Vector3.Distance(Location.ToGlobal(), player.Location.ToGlobal());
                //var distSquared = Vector3.DistanceSquared(Location.ToGlobal(), player.Location.ToGlobal());
                var distSquared = Location.SquaredDistanceTo(player.Location);
                if (distSquared <= rangeSquared)
                    player.Session.Network.EnqueueSend(msg);
            }
        }

        /// <summary>
        /// Sends network messages to all Players who currently know about this object
        /// </summary>
        public List<Player> EnqueueBroadcast(params GameMessage[] msgs)
        {
            return EnqueueBroadcast(true, msgs);
        }

        public List<Player> EnqueueBroadcast(bool sendSelf = true, params GameMessage[] msgs)
        {
            if (PhysicsObj == null)
            {
                if (Container != null)
                    return Container.EnqueueBroadcast(sendSelf, msgs);

                return null;
            }

            if (sendSelf)
            {
                if (this is Player self)
                    self.Session.Network.EnqueueSend(msgs);
            }

            var nearbyPlayers = PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer();
            foreach (var player in nearbyPlayers)
            {
                if (Visibility && !player.Adminvision)
                    continue;

                player.Session.Network.EnqueueSend(msgs);
            }
            return nearbyPlayers;
        }

        public List<Player> EnqueueBroadcast(List<Player> excludePlayers, bool sendSelf = true, params GameMessage[] msgs)
        {
            if (PhysicsObj == null) return null;

            if (sendSelf)
            {
                if (this is Player self)
                    self.Session.Network.EnqueueSend(msgs);
            }

            var nearbyPlayers = PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer();
            foreach (var player in nearbyPlayers.Except(excludePlayers))
            {
                if (Visibility && !player.Adminvision)
                    continue;

                player.Session.Network.EnqueueSend(msgs);
            }
            return nearbyPlayers;
        }

        /// <summary>
        /// Called when a new PhysicsObj enters the world
        /// </summary>
        public void NotifyPlayers()
        {
            // send create object network message to visible players
            foreach (var player in PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer())
                player.AddTrackedObject(this);

            if (this is Creature creature && !(this is Player))
                creature.CheckTargets();
        }
    }
}
