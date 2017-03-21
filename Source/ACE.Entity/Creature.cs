using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Creature
    {
        // These have the data types corresponding to DB
        // Need to add explicit casts when necessary to the Datatypes defined in ACE.Entity
        // i.e. for Model, ModelPalette, ModelTexture, etc.

        public uint Id { get; set; }

        public uint Wcid { get; set; }

        public string Name { get; set; }

        public uint IconId { get; set; }

        public uint SetupId { get; set; }

        public uint PhsTableId { get; set; }

        public uint STableId { get; set; }

        public byte ItemsCapacity { get; set; }

        public uint ObjectDescription { get; set; }

        public uint PhysicsState { get; set; }

        public uint ContainersCapacity { get; set; }

        public uint PaletteId { get; set; }

        public List<ObjectModel> Model;

        public List<ObjectPalette> Palette;

        public List<ObjectTexture> Texture;

        // Attributes
        public uint Level { get; set; }

        public uint Strength { get; set; }

        public uint Endurance { get; set; }

        public uint Coordination { get; set; }

        public uint Quickness { get; set; }

        public uint Focus { get; set; }

        public uint Self { get; set; }

        public uint Health { get; set; }

        public uint Stamina { get; set; }

        public uint Mana { get; set; }

        public uint XPGranted { get; set; }

        public byte Luminance { get; set; }

        public byte LootTier { get; set; }

        public Creature()
        {
            Model = new List<ObjectModel>();
            Palette = new List<ObjectPalette>();
            Texture = new List<ObjectTexture>();
        }

        // TODO:
        // Trophies dropped by this creature
        // Common Attacks: i.e. Bludgeon, Slash
        // Weaknesses: i.e. Fire
        // Attack Skills: Melee, Magic, Missile
        // Defense Skills: Melee, Magic, Missile
        // Leveling Up and Spending XP from killing players
    }
}
