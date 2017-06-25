using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class Monster : Creature
    {
        public Monster(AceObject aceO)
            : base((ObjectType)aceO.ItemType,
                  // TODO: replace this with GuidManager.NewMonsterGuid once the GuidRanges are defined and GuidManager integrates with ObjectGuid class
                  new ObjectGuid(GuidManager.NewItemGuid()),
                  aceO.Name,
                  (ushort)aceO.WeenieClassId,
                  (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags,
                  (WeenieHeaderFlag)aceO.WeenieHeaderFlags,
                  aceO.Location)
        {
            // TODO: Check why Drudges don't appear on radar yet and don't have a healthbar when you select them
            if (aceO.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceO.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceO.WeenieClassId - 0x8000);

            Name = aceO.Name ?? "NULL";
            DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            WeenieClassid = aceO.WeenieClassId;
            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;
            PhysicsData.Petable = aceO.PhysicsTableId;
            PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;
            PhysicsData.ObjScale = aceO.DefaultScale ?? 0u;
            PhysicsData.AnimationFrame = aceO.AnimationFrameId;
            PhysicsData.Translucency = aceO.Translucency ?? 0u;
            PhysicsData.DefaultScript = aceO.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float?)aceO.PhysicsScriptIntensity;
            PhysicsData.Elasticity = aceO.Elasticity;
            PhysicsData.Parent = aceO.Parent;
            PhysicsData.ParentLocation = aceO.ParentLocation;
            PhysicsData.Friction = aceO.Friction;
            if (aceO.CurrentMotionState == "0" || aceO.CurrentMotionState == null)
                PhysicsData.CurrentMotionState = null;
            else
                PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(aceO.CurrentMotionState));

            // game data min required flags;
            Icon = aceO.IconId;
            AmmoType = (AmmoType?)aceO.AmmoType;
            Burden = aceO.Burden;
            CombatUse = (CombatUse?)aceO.CombatUse;
            ContainerCapacity = aceO.ContainersCapacity;
            Cooldown = aceO.CooldownId;
            CooldownDuration = aceO.CooldownDuration;
            HookItemTypes = aceO.HookItemTypes;
            HookType = aceO.HookType;
            IconOverlay = aceO.IconOverlayId;
            IconUnderlay = aceO.IconUnderlayId;
            ItemCapacity = aceO.ItemsCapacity;
            Material = (Material?)aceO.MaterialType;
            MaxStackSize = aceO.MaxStackSize;
            Wielder = aceO.WielderId;
            ContainerId = aceO.ContainerId;
            CurrentWieldedLocation = (EquipMask?)aceO.CurrentWieldedLocation;

            // TODO: this needs to be pulled in from pcap data. Missing - Name Plural never set need to address

            Priority = (CoverageMask?)aceO.Priority;
            RadarBehavior = (RadarBehavior?)aceO.Radar;
            RadarColor = (RadarColor?)aceO.BlipColor;
            Script = aceO.PhysicsScript;
            Spell = (Spell?)aceO.SpellId;
            StackSize = aceO.StackSize;
            Structure = aceO.Structure;
            TargetType = aceO.TargetTypeId;
            GameDataType = aceO.ItemType;
            UiEffects = (UiEffects?)aceO.UiEffects;
            Usable = (Usable?)aceO.ItemUseable;
            UseRadius = aceO.UseRadius;
            ValidLocations = (EquipMask?)aceO.ValidLocations;
            Value = aceO.Value;
            Workmanship = aceO.Workmanship;

            PhysicsData.SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = aceO.PaletteId;

            SetObjectData(aceO);
            IsAlive = true;
            SetupVitals();
        }
    }
}
