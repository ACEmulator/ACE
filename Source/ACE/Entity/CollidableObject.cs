using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public abstract class CollidableObject : WorldObject
    {
        public CollidableObject(ObjectType type, ObjectGuid guid)
            : base(type, guid)
        {
        }

        public CollidableObject(ObjectType type, ObjectGuid guid, ushort weenieClassId, Position position)
            : base(type, guid)
        {
        }

        public CollidableObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Location = position;
            this.WeenieClassid = weenieClassId;
        }

        public CollidableObject(AceObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            this.Name = aceO.Name;
            this.DescriptionFlags = (ObjectDescriptionFlag)aceO.WdescBitField;
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;
            this.WeenieFlags = (WeenieHeaderFlag)aceO.WeenieFlags;

            this.PhysicsData.MTableResourceId = aceO.MotionTableId;
            this.PhysicsData.Stable = aceO.SoundTableId;
            this.PhysicsData.CSetup = aceO.ModelTableId;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsBitField;
            // Creating from weenie - the pcap may have had a container or a position
            // but if we are creating new that will be sent when we place ground or container not at create
            this.PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Parent;
            this.PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            // game data min required flags;
            this.Icon = aceO.IconId;

            this.GameData.AmmoType = (AmmoType)aceO.AmmoType;
            this.GameData.Burden = (ushort)aceO.Burden;
            this.GameData.CombatUse = (CombatUse)aceO.CombatUse;
            this.GameData.ContainerCapacity = aceO.ContainersCapacity;
            this.GameData.Cooldown = aceO.CooldownId;
            this.GameData.CooldownDuration = (decimal)aceO.CooldownDuration;
            this.GameData.HookItemTypes = aceO.HookItemTypes;
            this.GameData.HookType = (ushort)aceO.HookTypeId;
            this.GameData.IconOverlay = (ushort)aceO.IconOverlayId;
            this.GameData.IconUnderlay = (ushort)aceO.IconUnderlayId;
            this.GameData.ItemCapacity = aceO.ItemsCapacity;
            this.GameData.Material = (Material)aceO.MaterialType;
            this.GameData.MaxStackSize = aceO.MaxStackSize;
            this.GameData.MaxStructure = aceO.MaxStructure;
            // GameData.Name = aceO.Name;  I think this is redundant and should be removed from Game Data or from world object
            this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            this.GameData.RadarColour = (RadarColor)aceO.Radar;
            this.GameData.UseRadius = aceO.UseRadius;
            this.GameData.Spell = (Spell)aceO.SpellId;
            this.GameData.Script = aceO.PScript;
            this.GameData.ValidLocations = (EquipMask)aceO.ValidLocations;
            this.GameData.StackSize = aceO.StackSize;
            this.GameData.Struture = aceO.Structure;
            this.GameData.Value = aceO.Value;
            this.GameData.Type = (ushort)aceO.AceObjectId;
            this.GameData.TargetType = aceO.TargetTypeId;
            this.GameData.Usable = (Usable)aceO.Usability;

            aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public abstract void OnCollide(Player player);
    }
}
