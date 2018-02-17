using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public PhysicsDescriptionFlag PhysicsDescriptionFlag { get { return CalculatedPhysicsDescriptionFlag(); } }

        public PhysicsState PhysicsState { get { return CalculatedPhysicsState(); } }

        public MotionState CurrentMotionState { get; set; }

        public virtual Position Location
        {
            get => AceObject.Location;
            set
            {
                /*
                log.Debug($"{Name} moved to {Position}");

                Position = value;
                */
                if (AceObject.Location != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;
                AceObject.Location = value;
            }
        }

        /// <summary>
        /// mtable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint MotionTableId
        {
            get => GetProperty(PropertyDataId.MotionTable) ?? 0;
            set => SetProperty(PropertyDataId.MotionTable, value);
        }

        /// <summary>
        /// stable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint SoundTableId
        {
            get => GetProperty(PropertyDataId.SoundTable) ?? 0;
            set => SetProperty(PropertyDataId.SoundTable, value);
        }

        /// <summary>
        /// phstable_id in aclogviewer This is the physics table for the object.   Looked up from dat file.
        /// </summary>
        public uint PhysicsTableId
        {
            get => GetProperty(PropertyDataId.PhysicsEffectTable) ?? 0;
            set => SetProperty(PropertyDataId.PhysicsEffectTable, value);
        }

        /// <summary>
        /// setup_id in aclogviewer - used to get the correct model out of the dat file
        /// </summary>
        public uint SetupTableId
        {
            get => GetProperty(PropertyDataId.Setup) ?? 0;
            set => SetProperty(PropertyDataId.Setup, value);
        }

        public WeenieHeaderFlag WeenieFlags { get { return CalculatedWeenieHeaderFlag(); } }

        public ObjectDescriptionFlag DescriptionFlags { get { return CalculatedDescriptionFlag(); } }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            SerializeCreateObject(writer, false);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer, bool gamedataonly)
        {
            writer.WriteGuid(Guid);

            if (!gamedataonly)
            {
                SerializeModelData(writer);
                SerializePhysicsData(writer);
            }

            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.WritePackedDword(Biota.WeenieClassId);
            writer.WritePackedDwordOfKnownType(IconId ?? 0, 0x6000000);
            writer.Write((uint)ItemType);
            writer.Write((uint)DescriptionFlags);
            writer.Align();

            if ((DescriptionFlags & ObjectDescriptionFlag.IncludesSecondHeader) != 0)
                writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PluralName) != 0)
                writer.WriteString16L(NamePlural);

            if ((WeenieFlags & WeenieHeaderFlag.ItemsCapacity) != 0)
                writer.Write(ItemCapacity ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.ContainersCapacity) != 0)
                writer.Write(ContainerCapacity ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort?)AmmoType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(Value ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)Usable ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(TargetType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint?)UiEffects ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((sbyte?)CombatUse ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Structure) != 0)
                writer.Write(Structure ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(MaxStructure ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(StackSize ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(MaxStackSize ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(WielderId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint?)ValidLocations ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CurrentlyWieldedLocation) != 0)
                writer.Write((uint?)CurrentWieldedLocation ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint?)Priority ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBlipColor) != 0)
                writer.Write((byte?)RadarColor ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBehavior) != 0)
                writer.Write((byte?)RadarBehavior ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.PScript) != 0)
                writer.Write(Script ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(Workmanship ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(Burden ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort?)Spell ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
                writer.Write(HouseRestrictions ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(HookItemType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(HookType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.WritePackedDwordOfKnownType((IconOverlayId ?? 0), 0x6000000);

            if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.WritePackedDwordOfKnownType((IconUnderlayId ?? 0), 0x6000000);

            if ((WeenieFlags & WeenieHeaderFlag.MaterialType) != 0)
                writer.Write((uint)(MaterialType ?? 0u));

            if ((WeenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(CooldownId ?? 0);

            if ((WeenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write((double?)CooldownDuration ?? 0u);

            if ((WeenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(PetOwner ?? 0u);

            writer.Align();
        }

        public void SerializeModelData(BinaryWriter writer)
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
        public void SerializePhysicsData(BinaryWriter writer)
        {
            writer.Write((uint)PhysicsDescriptionFlag);

            writer.Write((uint)PhysicsState);

            // PhysicsDescriptionFlag.Movement takes priorty over PhysicsDescription.FlagAnimationFrame
            // If both are set, only Movement is written.
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
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
                        log.Debug($"Our flag is set but we have no data length. {this.Guid.Full:X}");
                        writer.Write(0u);
                    }
                }
                else
                {
                    log.Debug($"Our flag is set but our current motion state is null. {this.Guid.Full:X}");
                    writer.Write(0u);
                }
            }
            else if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write(((uint)(Placement ?? ACE.Entity.Enum.Placement.Default)));

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Location.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MotionTableId);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.STable) != 0)
                writer.Write(SoundTableId);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.PeTable) != 0)
                writer.Write(PhysicsTableId);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(SetupTableId);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write(WielderId ?? 0u);
                writer.Write(ParentLocation ?? 0);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(Children.Count);
                foreach (var child in Children)
                {
                    writer.Write(child.Guid);
                    writer.Write(child.LocationId);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                Velocity.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                Acceleration.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                Omega.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(DefaultScriptId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
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


















        public virtual void SendPartialUpdates(Session targetSession, List<AceObjectPropertyId> properties)
        {
            foreach (var property in properties)
            {
                switch (property.PropertyType)
                {
                    case AceObjectPropertyType.PropertyInt:
                        int? value = this.AceObject.GetProperty((PropertyInt)property.PropertyId);
                        if (value != null)
                            targetSession.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(targetSession.Player.Sequences, (PropertyInt)property.PropertyId, value.Value));
                        break;
                    default:
                        log.Debug($"Unsupported property in SendPartialUpdates: id {property.PropertyId}, type {property.PropertyType}.");
                        break;
                }
            }
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

        public void WriteUpdatePositionPayload(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            Location.Serialize(writer, PositionFlag, (int)(Placement ?? global::ACE.Entity.Enum.Placement.Default));
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

        public virtual void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            // Excluding some times that are sent later as weapon status Og II
            var propertiesInt = PropertiesInt.Where(x => x.PropertyId < 9000
                                                          && x.PropertyId != (uint)PropertyInt.Damage
                                                          && x.PropertyId != (uint)PropertyInt.DamageType
                                                          && x.PropertyId != (uint)PropertyInt.WeaponSkill
                                                          && x.PropertyId != (uint)PropertyInt.WeaponTime).ToList();

            if (propertiesInt.Count > 0)
            {
                flags |= IdentifyResponseFlags.IntStatsTable;
            }

            var propertiesInt64 = PropertiesInt64.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesInt64.Count > 0)
            {
                flags |= IdentifyResponseFlags.Int64StatsTable;
            }

            var propertiesBool = PropertiesBool.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesBool.Count > 0)
            {
                flags |= IdentifyResponseFlags.BoolStatsTable;
            }

            // the float values 13 - 19 + 165 (nether added way later) are armor resistance and is shown in a different list. Og II
            // 21-22, 26, 62-63 are all sent as part of the weapons profile and not duplicated.
            var propertiesDouble = PropertiesDouble.Where(x => x.PropertyId < 9000
                                                               && (x.PropertyId < (uint)PropertyDouble.ArmorModVsSlash
                                                               || x.PropertyId > (uint)PropertyDouble.ArmorModVsElectric)
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponLength
                                                               && x.PropertyId != (uint)PropertyDouble.DamageVariance
                                                               && x.PropertyId != (uint)PropertyDouble.MaximumVelocity
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponOffense
                                                               && x.PropertyId != (uint)PropertyDouble.DamageMod
                                                               && x.PropertyId != (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesDouble.Count > 0)
            {
                flags |= IdentifyResponseFlags.FloatStatsTable;
            }

            var propertiesDid = PropertiesDid.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesDid.Count > 0)
            {
                flags |= IdentifyResponseFlags.DidStatsTable;
            }

            var propertiesString = PropertiesString.Where(x => x.PropertyId < 9000).ToList();

            var propertiesSpellId = PropertiesSpellId.ToList();

            if (propertiesSpellId.Count > 0)
            {
                flags |= IdentifyResponseFlags.SpellBook;
            }

            // TODO: Move to Armor class
            var propertiesArmor = PropertiesDouble.Where(x => (x.PropertyId < 9000
                                                         && (x.PropertyId >= (uint)PropertyDouble.ArmorModVsSlash
                                                         && x.PropertyId <= (uint)PropertyDouble.ArmorModVsElectric))
                                                         || x.PropertyId == (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesArmor.Count > 0)
            {
                flags |= IdentifyResponseFlags.ArmorProfile;
            }

            // TODO: Move to Weapon class
            // Weapons Profile
            var propertiesWeaponsD = PropertiesDouble.Where(x => x.PropertyId < 9000
                                                            && (x.PropertyId == (uint)PropertyDouble.WeaponLength
                                                            || x.PropertyId == (uint)PropertyDouble.DamageVariance
                                                            || x.PropertyId == (uint)PropertyDouble.MaximumVelocity
                                                            || x.PropertyId == (uint)PropertyDouble.WeaponOffense
                                                            || x.PropertyId == (uint)PropertyDouble.DamageMod)).ToList();

            var propertiesWeaponsI = PropertiesInt.Where(x => x.PropertyId < 9000
                                                         && (x.PropertyId == (uint)PropertyInt.Damage
                                                         || x.PropertyId == (uint)PropertyInt.DamageType
                                                         || x.PropertyId == (uint)PropertyInt.WeaponSkill
                                                         || x.PropertyId == (uint)PropertyInt.WeaponTime)).ToList();

            if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            {
                flags |= IdentifyResponseFlags.WeaponProfile;
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            // Ok Down to business - let's identify all of this stuff.
            WriteIdentifyObjectHeader(writer, flags, success);
            WriteIdentifyObjectIntProperties(writer, flags, propertiesInt);
            WriteIdentifyObjectInt64Properties(writer, flags, propertiesInt64);
            WriteIdentifyObjectBoolProperties(writer, flags, propertiesBool);
            WriteIdentifyObjectDoubleProperties(writer, flags, propertiesDouble);
            WriteIdentifyObjectStringsProperties(writer, flags, propertiesString);
            WriteIdentifyObjectDidProperties(writer, flags, propertiesDid);
            WriteIdentifyObjectSpellIdProperties(writer, flags, propertiesSpellId);

            // TODO: Move to Armor class
            WriteIdentifyObjectArmorProfile(writer, flags, propertiesArmor);

            // TODO: Move to Weapon class
            WriteIdentifyObjectWeaponsProfile(writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }
    }
}
