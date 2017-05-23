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
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;
            PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            PhysicsData.Petable = baseAceObject.PhysicsTableId;
            PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            PhysicsData.ObjScale = baseAceObject.DefaultScale ?? 0u;
            PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            PhysicsData.Translucency = baseAceObject.Translucency ?? 0u;
            PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            PhysicsData.Elasticity = baseAceObject.Elasticity;
            PhysicsData.EquipperPhysicsDescriptionFlag = (EquipMask?)baseAceObject.CurrentWieldedLocation;
            PhysicsData.Friction = baseAceObject.Friction;
            if (baseAceObject.CurrentMotionState == "0")
                PhysicsData.CurrentMotionState = null;
            else
                PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // game data min required flags;
            Icon = baseAceObject.IconId;
            PhysicsData.SetPhysicsDescriptionFlag();

            GameData.AmmoType = (AmmoType?)baseAceObject.AmmoType;
            GameData.Burden = baseAceObject.Burden;
            GameData.CombatUse = (CombatUse?)baseAceObject.CombatUse;
            GameData.ContainerCapacity = baseAceObject.ContainersCapacity;
            GameData.Cooldown = baseAceObject.CooldownId;
            GameData.CooldownDuration = (decimal?)baseAceObject.CooldownDuration;
            GameData.HookItemTypes = baseAceObject.HookItemTypes;
            GameData.HookType = baseAceObject.HookType;
            GameData.IconOverlay = (ushort?)baseAceObject.IconOverlayId;
            GameData.IconUnderlay = (ushort?)baseAceObject.IconUnderlayId;
            GameData.ItemCapacity = baseAceObject.ItemsCapacity;
            GameData.Material = (Material?)baseAceObject.MaterialType;
            GameData.MaxStackSize = baseAceObject.MaxStackSize;
            GameData.NamePlural = GameData.NamePlural;
            // TODO: this needs to be pulled in from pcap data. Missing
            // GameData.Priority = baseAceObject.Priority;
            GameData.RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            GameData.RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            GameData.RadarColor = (RadarColor?)baseAceObject.BlipColor;
            GameData.RadarColor = (RadarColor?)baseAceObject.BlipColor;
            GameData.Script = baseAceObject.PlayScript;
            GameData.Spell = (Spell?)baseAceObject.SpellId;
            GameData.StackSize = baseAceObject.StackSize;
            GameData.Structure = baseAceObject.Structure;
            GameData.TargetType = baseAceObject.TargetTypeId;
            GameData.Type = (ushort)baseAceObject.AceObjectId;
            GameData.Type = (ushort)baseAceObject.ItemType;
            GameData.UiEffects = (UiEffects?)baseAceObject.UiEffects;
            GameData.Usable = (Usable?)baseAceObject.ItemUseable;
            GameData.UseRadius = baseAceObject.UseRadius;
            GameData.ValidLocations = (EquipMask?)baseAceObject.ValidLocations;
            GameData.Value = baseAceObject.Value;
            GameData.Workmanship = baseAceObject.Workmanship;

            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            baseAceObject.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, ao.AnimationId));
            baseAceObject.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            baseAceObject.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = baseAceObject.PaletteId;
        }

        public DebugObject(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            GameData.Type = (ushort)aceO.ItemType;
        }

        public override void OnCollide(Player player)
        {
            // TODO: Implement
        }
    }
}
