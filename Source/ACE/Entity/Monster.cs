using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Database;
using ACE.Network.Enum;
using ACE.Managers;

namespace ACE.Entity
{
    public class Monster : MutableWorldObject
    {
        public uint templateid
        {
            get  { return creature.Id; }
        }

        private Creature creature;

        public Monster(string name, Position position) : base(ObjectType.Creature, new ObjectGuid(GuidManager.NewMonsterGuid()))
        {
            creature = DatabaseManager.World.GetCreatureByName(name);
            this.Name = creature.Name;

            uint tmpWcid;
            if (creature.Wcid < 0x8000)
                tmpWcid = creature.Wcid;
            else
                tmpWcid = creature.Wcid - 0x8000;
            this.WeenieClassid = (ushort) tmpWcid;

            this.DescriptionFlags = (ObjectDescriptionFlag) creature.ObjectDescription;
            this.WeenieFlags = WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.TargetType | WeenieHeaderFlag.ItemCapacity;
            this.Position = position;

            this.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.CSetup;
            this.PhysicsData.Stable = creature.STableId;
            this.PhysicsData.Petable = creature.PhsTableId;
            this.PhysicsData.CSetup = creature.SetupId;

            uint tmpIconId = creature.IconId - 0x6000000u;
            this.GameData.Icon = (ushort) tmpIconId;
            this.GameData.ItemCapacity = (byte)creature.ItemsCapacity;
            this.GameData.ContainerCapacity = (byte)creature.ContainersCapacity;
            this.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            this.GameData.RadarColour = RadarColor.Creature;
            this.GameData.Usable = Usable.UsableNo;

            // fill model, palette and texture data
            creature.Model.ForEach(x => this.ModelData.AddModel(x.Index, (ushort) (x.ResourceId - 0x01000000)));
            creature.Palette.ForEach(x => this.ModelData.AddPallet((ushort) (x.PaletteID- 0x04000000), x.Offset, (byte) (x.Length / 8)));
            creature.Texture.ForEach(x => this.ModelData.AddTexture(x.Index, (ushort) (x.OldTexture - 0x05000000), (ushort) (x.NewTexture - 0x05000000)));

        }

        public uint Level
        {
            get { return creature.Level; }
        }

        public uint Strength
        {
            get { return creature.Strength; }
        }

        public uint Endurance
        {
            get { return creature.Endurance; }
        }

        public uint Coordination
        {
            get { return creature.Coordination; }
        }

        public uint Quickness
        {
            get { return creature.Quickness; }
        }

        public uint Focus
        {
            get { return creature.Quickness; }
        }

        public uint Self
        {
            get { return creature.Self; }
        }

        public uint Health
        {
            get { return creature.Health; }
        }

        public uint Stamina
        {
            get { return creature.Stamina; }
        }

        public uint Mana
        {
            get { return creature.Mana; }
        }

        public uint XPGranted
        {
            get { return creature.XPGranted; }
        }

        public byte Luminance
        {
            get { return creature.Luminance; }
        }

        public byte LootTier
        {
            get { return creature.LootTier; }
        }
    }
}
