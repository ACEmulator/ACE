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
            WeenieClassid = baseAceObject.AceObjectId;
            WeenieFlags = (WeenieHeaderFlag)baseAceObject.WeenieHeaderFlags;

            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;
            PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            PhysicsData.Petable = baseAceObject.PhysicsTableId;
            PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            PhysicsData.ObjScale = baseAceObject.DefaultScale ?? 0.00f;
            PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            PhysicsData.Translucency = baseAceObject.Translucency ?? 0.00f;
            PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            PhysicsData.Elasticity = baseAceObject.Elasticity;
            PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            PhysicsData.Friction = baseAceObject.Friction;
            if (PhysicsData.AnimationFrame != 0)
                PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;
            if (baseAceObject.CurrentMotionState == "0")
                PhysicsData.CurrentMotionState = null;
            else
                PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // game data min required flags;
            Icon = baseAceObject.IconId;

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
            GameData.NamePlural = GameData.NamePlural ?? "NULLs";
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
            GameData.Value = baseAceObject.Value;
            GameData.Workmanship = baseAceObject.Workmanship;

            // Put is in for Ripley - these are the fields I want to write that he was concerned with.
            if ((Type & (ObjectType.Creature | ObjectType.LifeStone | ObjectType.Portal)) == 0)
            {
                // because this comes from PCAP data - on create we are not animating.
                PhysicsData.AnimationFrame = 0x65;

                // Container will always be 0 or a value and we should write it.
                // Not sure if the align packs us out with 0's may be redundant Og II
                WeenieFlags &= ~WeenieHeaderFlag.Container;

                // Creating from a pcap of the weenie - this will be set by the loot generation factory. Og II
                PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Parent;
                if (baseAceObject.ValidLocations != null)
                    GameData.ValidLocations = (EquipMask)baseAceObject.ValidLocations;
            }
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
