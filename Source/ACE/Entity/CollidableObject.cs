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
            GameData.AmmoType = (AmmoType?)aceO.AmmoType;
            GameData.Burden = aceO.Burden;
            GameData.CombatUse = (CombatUse?)aceO.CombatUse;
            GameData.ContainerCapacity = aceO.ContainersCapacity;
            GameData.Cooldown = aceO.CooldownId;
            GameData.CooldownDuration = (decimal?)aceO.CooldownDuration;
            GameData.HookItemTypes = aceO.HookItemTypes;
            GameData.HookType = aceO.HookType;
            GameData.IconOverlay = (ushort?)aceO.IconOverlayId;
            GameData.IconUnderlay = (ushort?)aceO.IconUnderlayId;
            GameData.ItemCapacity = aceO.ItemsCapacity;
            GameData.Material = (Material?)aceO.MaterialType;
            GameData.MaxStackSize = aceO.MaxStackSize;
            GameData.MaxStructure = aceO.MaxStructure;
            GameData.RadarBehavior = (RadarBehavior?)aceO.Radar;
            GameData.RadarColor = (RadarColor?)aceO.Radar;
            GameData.UseRadius = aceO.UseRadius;
            GameData.Spell = (Spell)aceO.SpellId;
            GameData.Script = aceO.PlayScript;
            GameData.ValidLocations = (EquipMask?)aceO.ValidLocations;
            GameData.StackSize = aceO.StackSize;
            GameData.Structure = aceO.Structure;
            GameData.Value = aceO.Value;
            GameData.Type = (ushort)aceO.AceObjectId;
            GameData.TargetType = aceO.TargetTypeId;
            GameData.Usable = (Usable?)aceO.ItemUseable;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public abstract void OnCollide(Player player);
    }
}
