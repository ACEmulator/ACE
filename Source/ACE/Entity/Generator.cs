﻿using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Diagnostics;

namespace ACE.Entity
{
    public class Generator : CollidableObject
    {
        public Generator(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Generator(ObjectGuid guid, AceObject baseAceObject)
            : base((ObjectType)baseAceObject.ItemType, guid)
        {
            Name = baseAceObject.Name ?? "NULL";
            // DescriptionFlags = (ObjectDescriptionFlag)baseAceObject.AceObjectDescriptionFlags;
            DescriptionFlags = ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable | ObjectDescriptionFlag.HiddenAdmin;
            WeenieClassid = baseAceObject.WeenieClassId;
            Location = baseAceObject.Location;
            // PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            // PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            // PhysicsData.Petable = baseAceObject.PhysicsTableId;
            // PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            PhysicsData.PhysicsState = PhysicsState.IgnoreCollision | PhysicsState.Hidden | PhysicsState.Ethereal;
            // PhysicsData.ObjScale = baseAceObject.DefaultScale ?? 0u;
            // PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            // PhysicsData.Translucency = baseAceObject.Translucency ?? 0u;
            // PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            // PhysicsData.DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            // PhysicsData.Elasticity = baseAceObject.Elasticity;
            // PhysicsData.Parent = baseAceObject.Parent;
            // PhysicsData.ParentLocation = baseAceObject.ParentLocation;
            // PhysicsData.Friction = baseAceObject.Friction;
            // if (baseAceObject.CurrentMotionState == "0" || baseAceObject.CurrentMotionState == null)
            //    PhysicsData.CurrentMotionState = null;
            // else
            //    PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // game data min required flags;
            Icon = baseAceObject.IconId;
            // Icon = 4328;
            // AmmoType = (AmmoType?)baseAceObject.AmmoType;
            // Burden = baseAceObject.Burden;
            // CombatUse = (CombatUse?)baseAceObject.CombatUse;
            // ContainerCapacity = baseAceObject.ContainersCapacity;
            // Cooldown = baseAceObject.CooldownId;
            // CooldownDuration = baseAceObject.CooldownDuration;
            // HookItemTypes = baseAceObject.HookItemTypes;
            // HookType = baseAceObject.HookType;
            // IconOverlay = baseAceObject.IconOverlayId;
            // IconUnderlay = baseAceObject.IconUnderlayId;
            // ItemCapacity = baseAceObject.ItemsCapacity;
            // Material = (Material?)baseAceObject.MaterialType;
            // MaxStackSize = baseAceObject.MaxStackSize;
            // Wielder = baseAceObject.WielderId;
            // ContainerId = baseAceObject.ContainerId;
            // CurrentWieldedLocation = (EquipMask?)baseAceObject.CurrentWieldedLocation;

            // TODO: this needs to be pulled in from pcap data. Missing - Name Plural never set need to address

            // Priority = (CoverageMask?)baseAceObject.Priority;
            RadarBehavior = Network.Enum.RadarBehavior.NeverShow;
            // RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            // RadarColor = (RadarColor?)baseAceObject.BlipColor;
            RadarColor = Network.Enum.RadarColor.Admin;
            // Script = baseAceObject.PhysicsScript;
            // Spell = (Spell?)baseAceObject.SpellId;
            // StackSize = baseAceObject.StackSize;
            // Structure = baseAceObject.Structure;
            // TargetType = baseAceObject.TargetTypeId;
            // GameDataType = baseAceObject.ItemType;
            // UiEffects = (UiEffects?)baseAceObject.UiEffects;
            // Usable = (Usable?)baseAceObject.ItemUseable;
            Usable = Network.Enum.Usable.UsableNo;
            // UseRadius = baseAceObject.UseRadius;
            // ValidLocations = (EquipMask?)baseAceObject.ValidLocations;
            // Value = baseAceObject.Value;
            // Workmanship = baseAceObject.Workmanship;

            PhysicsData.SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            baseAceObject.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, ao.AnimationId));
            baseAceObject.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            baseAceObject.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = baseAceObject.PaletteId;
        }

        public Generator(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            // FIXME(ddevec): These should be inhereted from aceO, not copied
            Location = aceO.Location;
            Debug.Assert(aceO.Location != null, "Trying to create DebugObject with null location");
            WeenieClassid = aceO.WeenieClassId;
            GameDataType = aceO.ItemType;
        }

        public override void OnCollide(ObjectGuid playerId)
        {
            // TODO: Implement
        }
    }
}
