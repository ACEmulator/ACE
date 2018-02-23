using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject 
    {
         public virtual void SerializeUpdateObject(BinaryWriter writer)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            SerializeCreateObject(writer, false);
        }

        public virtual void SerializeGameDataOnly(BinaryWriter writer)
        {
            SerializeCreateObject(writer, true);
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
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }

        private void SerializeCreateObject(BinaryWriter writer, bool gamedataonly)
        {
            writer.WriteGuid(Guid);

            if (!gamedataonly)
            {
                SerializeModelData(writer);
                SerializePhysicsData(writer);
            }

            var weenieFlags = CalculatedWeenieHeaderFlag();
            var weenieFlags2 = CalculatedWeenieHeaderFlag2();
            var descriptionFlags = CalculatedDescriptionFlag();

            writer.Write((uint)weenieFlags);
            writer.WriteString16L(Name);
            writer.WritePackedDword(WeenieClassId);
            writer.WritePackedDwordOfKnownType(IconId, 0x6000000);
            writer.Write((uint)ItemType);
            writer.Write((uint)descriptionFlags);
            writer.Align();

            if ((descriptionFlags & ObjectDescriptionFlag.IncludesSecondHeader) != 0)
                writer.Write((uint)weenieFlags2);

            if ((weenieFlags & WeenieHeaderFlag.PluralName) != 0)
                writer.WriteString16L(NamePlural);

            if ((weenieFlags & WeenieHeaderFlag.ItemsCapacity) != 0)
                writer.Write(ItemCapacity ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.ContainersCapacity) != 0)
                writer.Write(ContainerCapacity ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort?)AmmoType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(Value ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)Usable ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(TargetType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint?)UiEffects ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((sbyte?)CombatUse ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Structure) != 0)
                writer.Write(Structure ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(MaxStructure ?? (ushort)0);

            if ((weenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(StackSize ?? (ushort)0);

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
                writer.Write((uint?)Priority ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.RadarBlipColor) != 0)
                writer.Write((byte?)RadarColor ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.RadarBehavior) != 0)
                writer.Write((byte?)RadarBehavior ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.PScript) != 0)
                writer.Write(Script ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(Workmanship ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(Burden ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort?)Spell ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
                writer.Write(HouseRestrictions ?? 0u);

            if ((weenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(HookItemType ?? 0);

            if ((weenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch ?? 0);

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
            writer.Write((byte)0x11);
            writer.Write((byte)modelPalettes.Count);
            writer.Write((byte)modelTextures.Count);
            writer.Write((byte)models.Count);

            if ((modelPalettes.Count > 0) && (PaletteBaseId != null))
                writer.WritePackedDwordOfKnownType((uint)PaletteBaseId, 0x4000000);
            foreach (var palette in modelPalettes)
            {
                writer.WritePackedDwordOfKnownType(palette.PaletteId, 0x4000000);
                writer.Write((byte)palette.Offset);
                writer.Write((byte)palette.Length);
            }

            foreach (var texture in modelTextures)
            {
                writer.Write((byte)texture.Index);
                writer.WritePackedDwordOfKnownType(texture.OldTexture, 0x5000000);
                writer.WritePackedDwordOfKnownType(texture.NewTexture, 0x5000000);
            }

            foreach (var model in models)
            {
                writer.Write((byte)model.Index);
                writer.WritePackedDwordOfKnownType(model.ModelID, 0x1000000);
            }

            writer.Align();
        }

        // todo: return bytes of data for network write ? ?
        private void SerializePhysicsData(BinaryWriter writer)
        {
            var physicsDescriptionFlag = CalculatedPhysicsDescriptionFlag();
            var physicsState = CalculatedPhysicsState();

            writer.Write((uint)physicsDescriptionFlag);

            writer.Write((uint)physicsState);

            // PhysicsDescriptionFlag.Movement takes priorty over PhysicsDescription.FlagAnimationFrame
            // If both are set, only Movement is written.
            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                if (CurrentMotionState != null)
                {
                    var movementData = CurrentMotionState.GetPayload(Guid, Sequences);
                    if (movementData.Length > 0)
                    {
                        writer.Write((uint)movementData.Length); // May not need this cast from int to uint, but the protocol says uint Og II
                        writer.Write(movementData);
                        uint autonomous = CurrentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                        writer.Write(autonomous);
                    }
                    else
                    {
                        // Adding these debug lines - don't think we can hit these, but want to make sure. Og II
                        log.Debug($"Our flag is set but we have no data length. {Guid.Full:X}");
                        writer.Write(0u);
                    }
                }
                else
                {
                    log.Debug($"Our flag is set but our current motion state is null. {Guid.Full:X}");
                    writer.Write(0u);
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
                writer.Write(ObjScale ?? 0u);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0u);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0u);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency ?? 0u);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                Velocity.Serialize(writer);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                Acceleration.Serialize(writer);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                Omega.Serialize(writer);
            }

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(DefaultScriptId ?? 0u);

            if ((physicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(DefaultScriptIntensity ?? 0u);

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


        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        public void WriteUpdatePositionPayload(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            Location.Serialize(writer, PositionFlag, (int)(Placement ?? ACE.Entity.Enum.Placement.Default));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
        }

        /// <summary>
        /// Alerts clients of change in position
        /// </summary>
        protected virtual void SendUpdatePosition()
        {
            LastMovementBroadcastTicks = WorldManager.PortalYearTicks;
            GameMessage msg = new GameMessageUpdatePosition(this);

            if (CurrentLandblock != null)
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
        }


        public virtual void SendPartialUpdates(Session targetSession, List<AceObjectPropertyId> properties)
        {
            foreach (var property in properties)
            {
                switch (property.PropertyType)
                {
                    case AceObjectPropertyType.PropertyInt:
                        int? value = GetProperty((PropertyInt)property.PropertyId);
                        if (value != null)
                            targetSession.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(targetSession.Player.Sequences, (PropertyInt)property.PropertyId, value.Value));
                        break;
                    default:
                        log.Debug($"Unsupported property in SendPartialUpdates: id {property.PropertyId}, type {property.PropertyType}.");
                        break;
                }
            }
        }

        public virtual void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            // Excluding some times that are sent later as weapon status Og II
            //var propertiesInt = PropertiesInt.Where(x => x.PropertyId < 9000
            //                                              && x.PropertyId != (uint)PropertyInt.Damage
            //                                              && x.PropertyId != (uint)PropertyInt.DamageType
            //                                              && x.PropertyId != (uint)PropertyInt.WeaponSkill
            //                                              && x.PropertyId != (uint)PropertyInt.WeaponTime).ToList();
            var propertiesInt = GetAllPropertyInt().Where(x => ClientProperties.PropertiesInt.Contains((ushort)x.Key)).ToList();

            if (propertiesInt.Count > 0)
            {
                flags |= IdentifyResponseFlags.IntStatsTable;
            }

            //var propertiesInt64 = PropertiesInt64.Where(x => x.PropertyId < 9000).ToList();
            var propertiesInt64 = GetAllPropertyInt64().Where(x => ClientProperties.PropertiesInt64.Contains((ushort)x.Key)).ToList();

            if (propertiesInt64.Count > 0)
            {
                flags |= IdentifyResponseFlags.Int64StatsTable;
            }

            //var propertiesBool = PropertiesBool.Where(x => x.PropertyId < 9000).ToList();
            var propertiesBool = GetAllPropertyBools().Where(x => ClientProperties.PropertiesBool.Contains((ushort)x.Key)).ToList();

            if (propertiesBool.Count > 0)
            {
                flags |= IdentifyResponseFlags.BoolStatsTable;
            }

            // the float values 13 - 19 + 165 (nether added way later) are armor resistance and is shown in a different list. Og II
            // 21-22, 26, 62-63 are all sent as part of the weapons profile and not duplicated.
            //var propertiesDouble = PropertiesDouble.Where(x => x.PropertyId < 9000
            //                                                   && (x.PropertyId < (uint)PropertyFloat.ArmorModVsSlash
            //                                                   || x.PropertyId > (uint)PropertyFloat.ArmorModVsElectric)
            //                                                   && x.PropertyId != (uint)PropertyFloat.WeaponLength
            //                                                   && x.PropertyId != (uint)PropertyFloat.DamageVariance
            //                                                   && x.PropertyId != (uint)PropertyFloat.MaximumVelocity
            //                                                   && x.PropertyId != (uint)PropertyFloat.WeaponOffense
            //                                                   && x.PropertyId != (uint)PropertyFloat.DamageMod
            //                                                   && x.PropertyId != (uint)PropertyFloat.ArmorModVsNether).ToList();

            var propertiesDouble = GetAllPropertyFloat().Where(x => ClientProperties.PropertiesDouble.Contains((ushort)x.Key)).ToList();

            if (propertiesDouble.Count > 0)
            {
                flags |= IdentifyResponseFlags.FloatStatsTable;
            }

            //var propertiesDid = PropertiesDid.Where(x => x.PropertyId < 9000).ToList();
            var propertiesDid = GetAllPropertyDataId().Where(x => ClientProperties.PropertiesDataId.Contains((ushort)x.Key)).ToList();

            if (propertiesDid.Count > 0)
            {
                flags |= IdentifyResponseFlags.DidStatsTable;
            }

            //var propertiesString = PropertiesString.Where(x => x.PropertyId < 9000).ToList();

            var propertiesString = GetAllPropertyString().Where(x => ClientProperties.PropertiesString.Contains((ushort)x.Key)).ToList();

            //var propertiesSpellId = PropertiesSpellId.ToList();

            //if (propertiesSpellId.Count > 0)
            //{
            //    flags |= IdentifyResponseFlags.SpellBook;
            //}

            // TODO: Move to Armor class
            //var propertiesArmor = PropertiesDouble.Where(x => (x.PropertyId < 9000
            //                                             && (x.PropertyId >= (uint)PropertyFloat.ArmorModVsSlash
            //                                             && x.PropertyId <= (uint)PropertyFloat.ArmorModVsElectric))
            //                                             || x.PropertyId == (uint)PropertyFloat.ArmorModVsNether).ToList();

            //if (propertiesArmor.Count > 0)
            //{
            //    flags |= IdentifyResponseFlags.ArmorProfile;
            //}

            // TODO: Move to Weapon class
            // Weapons Profile
            //var propertiesWeaponsD = PropertiesDouble.Where(x => x.PropertyId < 9000
            //                                                && (x.PropertyId == (uint)PropertyFloat.WeaponLength
            //                                                || x.PropertyId == (uint)PropertyFloat.DamageVariance
            //                                                || x.PropertyId == (uint)PropertyFloat.MaximumVelocity
            //                                                || x.PropertyId == (uint)PropertyFloat.WeaponOffense
            //                                                || x.PropertyId == (uint)PropertyFloat.DamageMod)).ToList();

            //var propertiesWeaponsI = PropertiesInt.Where(x => x.PropertyId < 9000
            //                                             && (x.PropertyId == (uint)PropertyInt.Damage
            //                                             || x.PropertyId == (uint)PropertyInt.DamageType
            //                                             || x.PropertyId == (uint)PropertyInt.WeaponSkill
            //                                             || x.PropertyId == (uint)PropertyInt.WeaponTime)).ToList();

            //if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            //{
            //    flags |= IdentifyResponseFlags.WeaponProfile;
            //}

            //if (propertiesString.Count > 0)
            //{
            //    flags |= IdentifyResponseFlags.StringStatsTable;
            //}

            // Ok Down to business - let's identify all of this stuff.
            flags = IdentifyResponseFlags.None;
            WriteIdentifyObjectHeader(writer, flags, success);
            //WriteIdentifyObjectIntProperties(writer, flags, propertiesInt);
            //WriteIdentifyObjectInt64Properties(writer, flags, propertiesInt64);
            //WriteIdentifyObjectBoolProperties(writer, flags, propertiesBool);
            //WriteIdentifyObjectDoubleProperties(writer, flags, propertiesDouble);
            //WriteIdentifyObjectStringsProperties(writer, flags, propertiesString);
            //WriteIdentifyObjectDidProperties(writer, flags, propertiesDid);
            //WriteIdentifyObjectSpellIdProperties(writer, flags, propertiesSpellId);

            // TODO: Move to Armor class
            //WriteIdentifyObjectArmorProfile(writer, flags, propertiesArmor);

            // TODO: Move to Weapon class
            //WriteIdentifyObjectWeaponsProfile(writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }


        protected PhysicsDescriptionFlag CalculatedPhysicsDescriptionFlag()
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            var movementData = CurrentMotionState?.GetPayload(Guid, Sequences);

            if (movementData != null && movementData.Length > 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            if (Placement != null)
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

            if ((ObjScale != null) && (Math.Abs(ObjScale ?? 0) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;

            if (Friction != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Friction;

            if (Elasticity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Elasticity;

            if ((Translucency != null) && (Math.Abs(Translucency ?? 0) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Translucency;

            if (Velocity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Velocity;

            if (Acceleration != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Acceleration;

            if (Omega != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Omega;

            if (CSetup.DefaultScript != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScript;

            if (DefaultScriptIntensity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScriptIntensity;

            return physicsDescriptionFlag;
        }

        private PhysicsState CalculatedPhysicsState()
        {
            // todo: This is doing 2 things. It's pulling the flag values from the PropertyInt.PhysicsState, then in turn, setting the bool value counterparts.
            // That seem a bit redundant
            // If we really want to set default states on create or load, we need to separate this function into two parts.
            // Right now, every time this is called, defaults are being set.

            // Read in Object's Default PhysicsState
            var physicsState = (PhysicsState)(GetProperty(PropertyInt.PhysicsState) ?? 0);

            if (physicsState.HasFlag(PhysicsState.Static))
                if (!Static.HasValue)
                    Static = true;
            if (physicsState.HasFlag(PhysicsState.Ethereal))
                if (!Ethereal.HasValue)
                    Ethereal = true;
            if (physicsState.HasFlag(PhysicsState.ReportCollision))
                if (!ReportCollisions.HasValue)
                    ReportCollisions = true;
            if (physicsState.HasFlag(PhysicsState.IgnoreCollision))
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
            if (physicsState.HasFlag(PhysicsState.ReportCollisionAsEnviroment))
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
                physicsState |= PhysicsState.ReportCollision;
            else
                physicsState &= ~PhysicsState.ReportCollision;
            ////IgnoreCollision             = 0x00000010,
            if (IgnoreCollisions ?? false)
                physicsState |= PhysicsState.IgnoreCollision;
            else
                physicsState &= ~PhysicsState.IgnoreCollision;
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
            ////HasPhysicsBsp               = 0x00010000,
            if (CSetup.HasPhysicsBSP)
                physicsState |= PhysicsState.HasPhysicsBsp;
            else
                physicsState &= ~PhysicsState.HasPhysicsBsp;
            ////Inelastic                   = 0x00020000,
            if (Inelastic ?? false)
                physicsState |= PhysicsState.Inelastic;
            else
                physicsState &= ~PhysicsState.Inelastic;
            ////HasDefaultAnim              = 0x00040000,
            if (CSetup.DefaultAnimation > 0)
                physicsState |= PhysicsState.HasDefaultAnim;
            else
                physicsState &= ~PhysicsState.HasDefaultAnim;
            ////HasDefaultScript            = 0x00080000,
            if (CSetup.DefaultScript > 0)
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
                physicsState |= PhysicsState.ReportCollisionAsEnviroment;
            else
                physicsState &= ~PhysicsState.ReportCollisionAsEnviroment;
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

        protected WeenieHeaderFlag CalculatedWeenieHeaderFlag()
        {
            var weenieHeaderFlag = WeenieHeaderFlag.None;

            if (NamePlural != null)
                weenieHeaderFlag |= WeenieHeaderFlag.PluralName;

            if (ItemCapacity != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ItemsCapacity;

            if (ContainerCapacity != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ContainersCapacity;

            if (AmmoType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.AmmoType;

            if (Value != null && (Value > 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Value;

            if (Usable != null)
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

            if (Priority != null)
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

            if (Burden != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Burden;

            if ((Spell != null) && (Spell != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Spell;

            if (HouseOwner != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HouseOwner;

            //TODO: HousingRestriction ACL property
            //if (HouseRestrictions != null)
            //    weenieHeaderFlag |= WeenieHeaderFlag.HouseRestrictions;

            var hookItemTypeInt = GetProperty(PropertyInt.HookItemType);
            if (hookItemTypeInt != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookItemTypes;

            if (Monarch != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Monarch;

            if (HookType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookType;

            if ((IconOverlayId != null) && (IconOverlayId != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.IconOverlay;

            if (MaterialType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaterialType;

            return weenieHeaderFlag;
        }

        private WeenieHeaderFlag2 CalculatedWeenieHeaderFlag2()
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

        private ObjectDescriptionFlag CalculatedDescriptionFlag()
        {
            var flag = BaseDescriptionFlags;
            var weenieFlags2 = CalculatedWeenieHeaderFlag2();


            //if (flag.HasFlag(ObjectDescriptionFlag.Openable))
            //    if (!Open.HasValue)
            //        Open = true;
            if (flag.HasFlag(ObjectDescriptionFlag.Inscribable))
                if (!Inscribable.HasValue)
                    Inscribable = true;
            if (flag.HasFlag(ObjectDescriptionFlag.Stuck))
                if (!Stuck.HasValue)
                    Stuck = true;
            if (flag.HasFlag(ObjectDescriptionFlag.Attackable))
                if (!Attackable.HasValue)
                    Attackable = true;
            if (flag.HasFlag(ObjectDescriptionFlag.HiddenAdmin))
                if (!HiddenAdmin.HasValue)
                    HiddenAdmin = true;
            if (flag.HasFlag(ObjectDescriptionFlag.UiHidden))
                if (!UiHidden.HasValue)
                    UiHidden = true;
            if (flag.HasFlag(ObjectDescriptionFlag.ImmuneCellRestrictions))
                if (!IgnoreHouseBarriers.HasValue)
                    IgnoreHouseBarriers = true;
            if (flag.HasFlag(ObjectDescriptionFlag.RequiresPackSlot))
                if (!RequiresBackpackSlot.HasValue)
                    RequiresBackpackSlot = true;
            if (flag.HasFlag(ObjectDescriptionFlag.Retained))
                if (!Retained.HasValue)
                    Retained = true;
            if (flag.HasFlag(ObjectDescriptionFlag.WieldOnUse))
                if (!WieldOnUse.HasValue)
                    WieldOnUse = true;
            if (flag.HasFlag(ObjectDescriptionFlag.WieldLeft))
                if (!AutowieldLeft.HasValue)
                    AutowieldLeft = true;

            // TODO: More uncommentting and wiring up for other flags
            ////None                   = 0x00000000,
            ////Openable               = 0x00000001,
            if (WeenieType == WeenieType.Container || WeenieType == WeenieType.Corpse || WeenieType == WeenieType.Chest)
            {
                if (!((IsLocked ?? false) && (IsOpen ?? false)))
                    flag |= ObjectDescriptionFlag.Openable;
                else
                    flag &= ~ObjectDescriptionFlag.Openable;
            }
            ////Inscribable            = 0x00000002,
            if (Inscribable ?? false)
                flag |= ObjectDescriptionFlag.Inscribable;
            else
                flag &= ~ObjectDescriptionFlag.Inscribable;
            ////Stuck                  = 0x00000004,
            if (Stuck ?? false)
                flag |= ObjectDescriptionFlag.Stuck;
            else
                flag &= ~ObjectDescriptionFlag.Stuck;
            ////Player                 = 0x00000008,
            // if (AceObject.Player ?? false)
            //    Player = true;
            ////Attackable             = 0x00000010,
            if (Attackable ?? false)
                flag |= ObjectDescriptionFlag.Attackable;
            else
                flag &= ~ObjectDescriptionFlag.Attackable;
            ////PlayerKiller           = 0x00000020,
            // if (AceObject.PlayerKiller ?? false)
            //    PlayerKiller = true;
            ////HiddenAdmin            = 0x00000040,
            if (HiddenAdmin ?? false)
                flag |= ObjectDescriptionFlag.HiddenAdmin;
            else
                flag &= ~ObjectDescriptionFlag.HiddenAdmin;
            ////UiHidden               = 0x00000080,
            if (UiHidden ?? false)
                flag |= ObjectDescriptionFlag.UiHidden;
            else
                flag &= ~ObjectDescriptionFlag.UiHidden;
            ////Book                   = 0x00000100,
            // if (AceObject.Book ?? false)
            //    Book = true;
            ////Vendor                 = 0x00000200,
            // if (AceObject.Vendor ?? false)
            //    Vendor = true;
            ////PkSwitch               = 0x00000400,
            // if (AceObject.PkSwitch ?? false)
            //    PkSwitch = true;
            ////NpkSwitch              = 0x00000800,
            // if (AceObject.NpkSwitch ?? false)
            //    NpkSwitch = true;
            ////Door                   = 0x00001000,
            // if (AceObject.Door ?? false)
            //    Door = true;
            ////Corpse                 = 0x00002000,
            // if (AceObject.Corpse ?? false)
            //    Corpse = true;
            ////LifeStone              = 0x00004000,
            // if (AceObject.LifeStone ?? false)
            //    LifeStone = true;
            ////Food                   = 0x00008000,
            // if (AceObject.Food ?? false)
            //    Food = true;
            ////Healer                 = 0x00010000,
            // if (AceObject.Healer ?? false)
            //    Healer = true;
            ////Lockpick               = 0x00020000,
            // if (AceObject.Lockpick ?? false)
            //    Lockpick = true;
            ////Portal                 = 0x00040000,
            // if (AceObject.Portal ?? false)
            //    Portal = true;
            ////Admin                  = 0x00100000,
            // if (AceObject.Admin ?? false)
            //    Admin = true;
            ////FreePkStatus           = 0x00200000,
            // if (AceObject.FreePkStatus ?? false)
            //    FreePkStatus = true;
            ////ImmuneCellRestrictions = 0x00400000,
            if (IgnoreHouseBarriers ?? false)
                flag |= ObjectDescriptionFlag.ImmuneCellRestrictions;
            else
                flag &= ~ObjectDescriptionFlag.ImmuneCellRestrictions;
            ////RequiresPackSlot       = 0x00800000,
            if (RequiresBackpackSlot ?? false)
                flag |= ObjectDescriptionFlag.RequiresPackSlot;
            else
                flag &= ~ObjectDescriptionFlag.RequiresPackSlot;
            ////Retained               = 0x01000000,
            if (Retained ?? false)
                flag |= ObjectDescriptionFlag.Retained;
            else
                flag &= ~ObjectDescriptionFlag.Retained;
            ////PkLiteStatus           = 0x02000000,
            // if (AceObject.PkLiteStatus ?? false)
            //    PkLiteStatus = true;
            ////IncludesSecondHeader   = 0x04000000,
            if (weenieFlags2 > WeenieHeaderFlag2.None)
                flag |= ObjectDescriptionFlag.IncludesSecondHeader;
            else
                flag &= ~ObjectDescriptionFlag.IncludesSecondHeader;
            ////BindStone              = 0x08000000,
            // if (AceObject.BindStone ?? false)
            //    BindStone = true;
            ////VolatileRare           = 0x10000000,
            // if (AceObject.VolatileRare ?? false)
            //    VolatileRare = true;
            ////WieldOnUse             = 0x20000000,
            if (WieldOnUse ?? false)
                flag |= ObjectDescriptionFlag.WieldOnUse;
            else
                flag &= ~ObjectDescriptionFlag.WieldOnUse;
            ////WieldLeft              = 0x40000000,
            if (AutowieldLeft ?? false)
                flag |= ObjectDescriptionFlag.WieldLeft;
            else
                flag &= ~ObjectDescriptionFlag.WieldLeft;

            return flag;
        }




        /// <summary>
        /// Records where the client thinks we are, for use by physics engine later
        /// </summary>
        /// <param name="newPosition"></param>
        protected void PrepUpdatePosition(Position newPosition)
        {
            RequestedLocation = newPosition;
        }

        public void ClearRequestedPositions()
        {
            ForcedLocation = null;
            RequestedLocation = null;
        }

        /// <summary>
        /// Used by physics engine to actually update the entities position
        /// Automatically notifies clients of updated position
        /// </summary>
        /// <param name="newPosition"></param>
        public void PhysicsUpdatePosition(Position newPosition)
        {
            Location = newPosition;
            SendUpdatePosition();

            ForcedLocation = null;
            RequestedLocation = null;
        }

        /// <summary>
        /// Manages action/broadcast infrastructure
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IActor parent)
        {
            CurrentParent = parent;
            actionQueue.RemoveParent();
            actionQueue.SetParent(parent);
        }

        /// <summary>
        /// Prepare new action to run on this object
        /// </summary>
        public LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            return actionQueue.EnqueueAction(action);
        }

        /// <summary>
        /// Satisfies action interface
        /// </summary>
        /// <param name="node"></param>
        public void DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }

        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        public void RunActions()
        {
            actionQueue.RunActions();
        }



        protected static void WriteIdentifyObjectHeader(BinaryWriter writer, IdentifyResponseFlags flags, bool success)
        {
            writer.Write((uint)flags); // Flags
            writer.Write(Convert.ToUInt32(success)); // Success bool
        }

        protected static void WriteIdentifyObjectIntProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt> propertiesInt)
        {
            const ushort tableSize = 16;
            var notNull = propertiesInt.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectProperties(BinaryWriter writer, IdentifyResponseFlags flags, Dictionary<PropertyInt, int> properties)
        {
            const ushort tableSize = 16;

            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (properties.Count == 0))
                return;

            writer.Write((ushort)properties.Count);
            writer.Write(tableSize);

            foreach (var property in properties)
            {
                writer.Write((uint)property.Key);
                writer.Write(property.Value);
            }
        }

        protected static void WriteIdentifyObjectInt64Properties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt64> propertiesInt64)
        {
            const ushort tableSize = 8;
            var notNull = propertiesInt64.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.Int64StatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt64 x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectBoolProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesBool> propertiesBool)
        {
            const ushort tableSize = 8;
            var notNull = propertiesBool.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.BoolStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesBool x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(Convert.ToUInt32(x.PropertyValue.Value));
            }
        }

        protected static void WriteIdentifyObjectDoubleProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesDouble)
        {
            const ushort tableSize = 8;
            var notNull = propertiesDouble.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.FloatStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDouble x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectStringsProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesString> propertiesStrings)
        {
            const ushort tableSize = 8;
            var notNull = propertiesStrings.Where(p => !string.IsNullOrWhiteSpace(p.PropertyValue)).ToList();
            if ((flags & IdentifyResponseFlags.StringStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesString x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.WriteString16L(x.PropertyValue);
            }
        }

        protected static void WriteIdentifyObjectDidProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDataId> propertiesDid)
        {
            const ushort tableSize = 16;
            var notNull = propertiesDid.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.DidStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDataId x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectSpellIdProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesSpell> propertiesSpellId)
        {
            if ((flags & IdentifyResponseFlags.SpellBook) == 0 || (propertiesSpellId.Count == 0)) return;
            writer.Write((uint)propertiesSpellId.Count);
            /* todo fix for new EF model
            foreach (AceObjectPropertiesSpell x in propertiesSpellId)
            {
                writer.Write(x.SpellId);
            }*/
        }

        // TODO: Move to Armor class
        protected static void WriteIdentifyObjectArmorProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesArmor)
        {
            var notNull = propertiesArmor.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.ArmorProfile) == 0 || (notNull.Count == 0)) return;

            foreach (AceObjectPropertiesDouble x in notNull)
                writer.Write((float)x.PropertyValue.Value);
        }

        // TODO: Move to Weapon class
        protected static void WriteIdentifyObjectWeaponsProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesWeaponsD, List<AceObjectPropertiesInt> propertiesWeaponsI)
        {
            if ((flags & IdentifyResponseFlags.WeaponProfile) == 0) return;
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.DamageType)?.PropertyValue ?? 0);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.WeaponTime)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.WeaponSkill)?.PropertyValue ?? 0);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.Damage)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyFloat.DamageVariance)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyFloat.DamageMod)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyFloat.WeaponLength)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyFloat.MaximumVelocity)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyFloat.WeaponOffense)?.PropertyValue ?? 0.00);
            // This one looks to be 0 - I did not find one with this calculated.   It is called Max Velocity Calculated
            writer.Write(0u);
        }
    }
}
