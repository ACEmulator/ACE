using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Entity
{
    using System;

    using global::ACE.Network.Motion;

    public class UsableObject : WorldObject
    {
        public UsableObject(ObjectType type, ObjectGuid guid)
            : base(type, guid)
        {
        }

        public UsableObject(ObjectGuid guid, ObjectDescriptionFlag descriptionFlag, BaseAceObject baseAceObject)
           : base((ObjectType)baseAceObject.ItemType, guid)
        {
            Type = (ObjectType)baseAceObject.ItemType;
            WeenieClassid = baseAceObject.AceObjectId;
            Icon = baseAceObject.IconId - 0x06000000; // Database has the unpacked values - may be able to remove later.
            Name = baseAceObject.Name;
            DescriptionFlags = descriptionFlag;

            WeenieFlags = (WeenieHeaderFlag)baseAceObject.WeenieHeaderFlags;
            // Even if we spawn on the ground, we have the potential to have a container.   Container will always be 0 or a value and we should write it.
            WeenieFlags |= WeenieHeaderFlag.Container;

            foreach (var pal in baseAceObject.PaletteOverrides)
                ModelData.AddPalette(pal.SubPaletteId, pal.Offset, pal.Length);

            foreach (var tex in baseAceObject.TextureOverrides)
                ModelData.AddTexture(tex.Index, tex.OldId, tex.NewId);

            foreach (var ani in baseAceObject.AnimationOverrides)
                ModelData.AddModel(ani.Index, ani.AnimationId);

            PhysicsData.AnimationFrame = 0x065; // baseAceObject.AnimationFrameId;   This is 1 in the database and 0x65 in the live pcap.
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            //// TODO: Og II - fix once you understand what we are doing in the database.   Looks like a blob??
            //// PhysicsData.CurrentMotionState = baseAceObject.CurrentMotionState;
            PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float)baseAceObject.PhysicsScriptIntensity;
            PhysicsData.Elastcity = baseAceObject.Elasticity;
            PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            PhysicsData.Friction = baseAceObject.Friction;
            PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            PhysicsData.ObjScale = (float)baseAceObject.DefaultScale;
            PhysicsData.Petable = baseAceObject.PhysicsTableId;
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;

            // Creating from weenie - the pcap may have had a container or a position
            // but if we are creating new that will be sent when we place ground or container not at create
            PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Parent;
            PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            if (PhysicsData.AnimationFrame != 0)
            {
                PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;
            }

            PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.Translucency = (float)baseAceObject.Translucency;
            // TODO: Check to see if we should default a 0 to fix these possible null errors Og II
            GameData.AmmoType = (AmmoType?)baseAceObject.AmmoType;
            GameData.Burden = (ushort)baseAceObject.Burden;
            GameData.CombatUse = (CombatUse)baseAceObject.CombatUse;
            GameData.ContainerCapacity = (byte)baseAceObject.ContainersCapacity;
            GameData.Cooldown = baseAceObject.CooldownId;
            GameData.CooldownDuration = (decimal)baseAceObject.CooldownDuration;
            GameData.HookItemTypes = baseAceObject.HookItemTypes;
            GameData.HookType = (ushort)baseAceObject.HookType;
            GameData.IconOverlay = (ushort)baseAceObject.IconOverlayId;
            GameData.IconUnderlay = (ushort)baseAceObject.IconUnderlayId;
            GameData.ItemCapacity = baseAceObject.ItemsCapacity;
            GameData.Material = (Material)baseAceObject.MaterialType;
            GameData.MaxStackSize = baseAceObject.MaxStackSize;
            GameData.MaxStructure = baseAceObject.MaxStructure;
            // GameData.Name = baseAceObject.Name;  I think this is redundant and should be removed from Game Data or from world object
            GameData.RadarBehavior = (RadarBehavior)baseAceObject.Radar;
            GameData.RadarColour = (RadarColor)baseAceObject.Radar;
            GameData.UseRadius = baseAceObject.UseRadius;
            GameData.Spell = (Spell)baseAceObject.SpellId;
            GameData.Script = baseAceObject.PlayScript;
            GameData.ValidLocations = (EquipMask)baseAceObject.ValidLocations;
            GameData.StackSize = baseAceObject.StackSize;
            GameData.Structure = baseAceObject.Structure;
            GameData.Value = baseAceObject.Value;
            GameData.Type = (ushort)baseAceObject.AceObjectId;
            GameData.TargetType = baseAceObject.TargetTypeId;
            GameData.Usable = (Usable)baseAceObject.ItemUseable;
        }
        public UsableObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Location = position;
            this.WeenieClassid = weenieClassId;
        }

        public virtual void OnUse(Player player)
        {
            // todo: implement.  default is probably to pick it up off the ground
        }
    }
}
