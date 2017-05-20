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
            Name = baseAceObject.Name;
            if (Name == null)
                Name = "NULL";

            DescriptionFlags = (ObjectDescriptionFlag)baseAceObject.AceObjectDescriptionFlags;
            WeenieClassid = baseAceObject.AceObjectId;
            WeenieFlags = (WeenieHeaderFlag)baseAceObject.WeenieHeaderFlags;

            PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            PhysicsData.Petable = baseAceObject.PhysicsTableId;

            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;
            PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;

            if (baseAceObject.CurrentMotionState == "0")
                PhysicsData.CurrentMotionState = null;
            else
                PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            PhysicsData.ObjScale = baseAceObject.DefaultScale ?? 0.00f;
            PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            PhysicsData.Translucency = baseAceObject.Translucency ?? 0.00f;

            // game data min required flags;
            Icon = baseAceObject.IconId;

            if (GameData.NamePlural == null)
                GameData.NamePlural = "NULLs";

            GameData.Type = (ushort)baseAceObject.AceObjectId;

            GameData.Usable = (Usable?)baseAceObject.ItemUseable;
            GameData.RadarColor = (RadarColor?)baseAceObject.BlipColor;
            GameData.RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            GameData.UseRadius = baseAceObject.UseRadius;

            GameData.HookType = baseAceObject.HookType;
            GameData.HookItemTypes = baseAceObject.HookItemTypes;
            GameData.Burden = (ushort?)baseAceObject.Burden;
            GameData.Value = baseAceObject.Value;
            GameData.ItemCapacity = baseAceObject.ItemsCapacity;

            // Put is in for Ripley - these are the fields I want to write that he was concerned with.
            if ((Type & (ObjectType.Creature | ObjectType.LifeStone | ObjectType.Portal)) == 0)
            {
                // because this comes from PCAP data - on create we are not animating.
                PhysicsData.AnimationFrame = 0x65;

                // I think this is wrong - we need the weenieClassId from weenie_class   Leaving it for now
                // TODO: use view to return the correct value.
                WeenieClassid = baseAceObject.AceObjectId;

                // Container will always be 0 or a value and we should write it.
                // Not sure if the align packs us out with 0's may be redundant Og II
                WeenieFlags |= WeenieHeaderFlag.Container;

                // Creating from a pcap of the weenie - this will be set by the loot generation factory. Og II
                PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Parent;
                GameData.ValidLocations = (EquipMask)baseAceObject.ValidLocations;
            }
            if (PhysicsData.AnimationFrame != 0)
            {
                PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;
            }
            PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            PhysicsData.Elastcity = baseAceObject.Elasticity;
            PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            PhysicsData.Friction = baseAceObject.Friction;

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

            GameData.Type = aceO.WeenieClassId;
        }

        public override void OnCollide(Player player)
        {
            // TODO: Implement
        }
    }
}
