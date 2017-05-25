using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class DebugObject : CollidableObject
    {
        public DebugObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public DebugObject(ObjectGuid guid,  BaseAceObject baseAceObject)
            : base((ObjectType)baseAceObject.ItemType, guid)
        {
            Name = baseAceObject.Name ?? "NULL";
            DescriptionFlags = (ObjectDescriptionFlag)baseAceObject.AceObjectDescriptionFlags;
            WeenieClassid = baseAceObject.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)baseAceObject.WeenieHeaderFlags;

            // this should probably be determined based on the presence of data.
            PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;
            MTableResourceId = baseAceObject.MotionTableId;
            Stable = baseAceObject.SoundTableId;
            CSetup = baseAceObject.ModelTableId;
            Petable = baseAceObject.PhysicsTableId;
            PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            ObjScale = baseAceObject.DefaultScale ?? 0u;
            AnimationFrame = baseAceObject.AnimationFrameId;
            Translucency = baseAceObject.Translucency ?? 0u;
            DefaultScript = baseAceObject.DefaultScript;
            DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            Elasticity = baseAceObject.Elasticity;
            EquipperPhysicsDescriptionFlag = (EquipMask?)baseAceObject.CurrentWieldedLocation;
            Friction = baseAceObject.Friction;
            if (baseAceObject.CurrentMotionState == "0")
                CurrentMotionState = null;
            else
                CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // game data min required flags;
            Icon = baseAceObject.IconId;
            SetPhysicsDescriptionFlag();

            AmmoType = (AmmoType?)baseAceObject.AmmoType;
            Burden = baseAceObject.Burden;
            CombatUse = (CombatUse?)baseAceObject.CombatUse;
            ContainerCapacity = baseAceObject.ContainersCapacity;
            Cooldown = baseAceObject.CooldownId;
            CooldownDuration = (decimal?)baseAceObject.CooldownDuration;
            HookItemTypes = baseAceObject.HookItemTypes;
            HookType = baseAceObject.HookType;
            IconOverlay = (ushort?)baseAceObject.IconOverlayId;
            IconUnderlay = (ushort?)baseAceObject.IconUnderlayId;
            ItemCapacity = baseAceObject.ItemsCapacity;
            Material = (Material?)baseAceObject.MaterialType;
            MaxStackSize = baseAceObject.MaxStackSize;
            NamePlural = NamePlural;
            // TODO: this needs to be pulled in from pcap data. Missing
            // Priority = baseAceObject.Priority;
            RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            RadarColor = (RadarColor?)baseAceObject.BlipColor;
            Script = baseAceObject.PlayScript;
            Spell = (Spell?)baseAceObject.SpellId;
            StackSize = baseAceObject.StackSize;
            Structure = baseAceObject.Structure;
            TargetType = baseAceObject.TargetTypeId;
            GameDataType = (ushort)baseAceObject.ItemType;
            UiEffects = (UiEffects?)baseAceObject.UiEffects;
            Usable = (Usable?)baseAceObject.ItemUseable;
            UseRadius = baseAceObject.UseRadius;
            ValidLocations = (EquipMask?)baseAceObject.ValidLocations;
            Value = baseAceObject.Value;
            Workmanship = baseAceObject.Workmanship;

            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            baseAceObject.AnimationOverrides.ForEach(ao => AddModel(ao.Index, ao.AnimationId));
            baseAceObject.TextureOverrides.ForEach(to => AddTexture(to.Index, to.OldId, to.NewId));
            baseAceObject.PaletteOverrides.ForEach(po => AddPalette(po.SubPaletteId, po.Offset, po.Length));
            PaletteGuid = baseAceObject.PaletteId;
        }

        public DebugObject(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            GameDataType = (ushort)aceO.ItemType;
        }

        public override void OnCollide(Player player)
        {
            // TODO: Implement
        }
    }
}
