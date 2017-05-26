using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Entity
{
    public abstract class CollidableObject : WorldObject
    {
        internal CollidableObject(ObjectType type, ObjectGuid guid)
            : base(type, guid)
        {
        }

        internal CollidableObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassid = weenieClassId;
        }

        internal CollidableObject(AceObject aceO)
            : base((ObjectType)aceO.ItemType, new ObjectGuid(aceO.AceObjectId))
        {
            Name = aceO.Name;
            DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;
            PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            // game data min required flags;
            Icon = aceO.IconId;

            // TODO: Check to see if we should default a 0 to fix these possible null errors Og II
            AmmoType = (AmmoType?)aceO.AmmoType;
            Burden = aceO.Burden;
            CombatUse = (CombatUse?)aceO.CombatUse;
            ContainerCapacity = aceO.ContainersCapacity;
            Cooldown = aceO.CooldownId;
            CooldownDuration = (decimal?)aceO.CooldownDuration;
            HookItemTypes = aceO.HookItemTypes;
            HookType = aceO.HookType;
            IconOverlay = aceO.IconOverlayId;
            IconUnderlay = aceO.IconUnderlayId;
            ItemCapacity = aceO.ItemsCapacity;
            Material = (Material?)aceO.MaterialType;
            MaxStackSize = aceO.MaxStackSize;
            MaxStructure = aceO.MaxStructure;
            RadarBehavior = (RadarBehavior?)aceO.Radar;
            RadarColor = (RadarColor?)aceO.BlipColor;
            UseRadius = aceO.UseRadius;
            Spell = (Spell)aceO.SpellId;
            Script = aceO.PlayScript;
            ValidLocations = (EquipMask?)aceO.ValidLocations;
            StackSize = aceO.StackSize;
            Structure = aceO.Structure;
            Value = aceO.Value;
            GameDataType = aceO.AceObjectId;
            TargetType = aceO.TargetTypeId;
            Usable = (Usable?)aceO.ItemUseable;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public abstract void OnCollide(Player player);
    }
}
