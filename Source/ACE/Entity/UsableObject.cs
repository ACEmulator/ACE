using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Entity
{
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
                AddPalette(pal.SubPaletteId, pal.Offset, pal.Length);

            foreach (var tex in baseAceObject.TextureOverrides)
                AddTexture(tex.Index, tex.OldId, tex.NewId);

            foreach (var ani in baseAceObject.AnimationOverrides)
                AddModel(ani.Index, ani.AnimationId);

            AnimationFrame = 0x065; // baseAceObject.AnimationFrameId;   This is 1 in the database and 0x65 in the live pcap.
            CSetup = baseAceObject.ModelTableId;
            //// TODO: Og II - fix once you understand what we are doing in the database.   Looks like a blob??
            //// CurrentMotionState = baseAceObject.CurrentMotionState;
            DefaultScript = baseAceObject.DefaultScript;
            DefaultScriptIntensity = baseAceObject.PhysicsScriptIntensity;
            Elasticity = baseAceObject.Elasticity;
            EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            Friction = baseAceObject.Friction;
            MTableResourceId = baseAceObject.MotionTableId;
            ObjScale = baseAceObject.DefaultScale;
            Petable = baseAceObject.PhysicsTableId;
            PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsDescriptionFlag;

            // Creating from weenie - the pcap may have had a container or a position
            // but if we are creating new that will be sent when we place ground or container not at create
            PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Parent;
            PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            if (AnimationFrame != 0)
            {
                PhysicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;
            }

            PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            Stable = baseAceObject.SoundTableId;
            Translucency = baseAceObject.Translucency;
            // TODO: Check to see if we should default a 0 to fix these possible null errors Og II
            AmmoType = (AmmoType?)baseAceObject.AmmoType;
            Burden = baseAceObject.Burden;
            CombatUse = (CombatUse?)baseAceObject.CombatUse;
            ContainerCapacity = baseAceObject.ContainersCapacity;
            Cooldown = baseAceObject.CooldownId;
            CooldownDuration = (decimal?)baseAceObject.CooldownDuration;
            HookItemTypes = baseAceObject.HookItemTypes;
            HookType = baseAceObject.HookType;
            IconOverlay = (ushort)baseAceObject.IconOverlayId;
            IconUnderlay = (ushort)baseAceObject.IconUnderlayId;
            ItemCapacity = baseAceObject.ItemsCapacity;
            Material = (Material?)baseAceObject.MaterialType;
            MaxStackSize = baseAceObject.MaxStackSize;
            MaxStructure = baseAceObject.MaxStructure;
            RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            RadarColor = (RadarColor?)baseAceObject.Radar;
            UseRadius = baseAceObject.UseRadius;
            Spell = (Spell)baseAceObject.SpellId;
            Script = baseAceObject.PlayScript;
            ValidLocations = (EquipMask?)baseAceObject.ValidLocations;
            StackSize = baseAceObject.StackSize;
            Structure = baseAceObject.Structure;
            Value = baseAceObject.Value;
            GameDataType = (ushort)baseAceObject.ItemType;
            TargetType = baseAceObject.TargetTypeId;
            Usable = (Usable?)baseAceObject.ItemUseable;
        }
        public UsableObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassid = weenieClassId;
        }

        public virtual void OnUse(Player player)
        {
            // todo: implement.  default is probably to pick it up off the ground
        }
    }
}
