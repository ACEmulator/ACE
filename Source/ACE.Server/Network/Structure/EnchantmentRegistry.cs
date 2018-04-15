using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class EnchantmentRegistry
    {
        public EnchantmentMask EnchantmentMask;
        public List<Enchantment> LifeSpells;
        public List<Enchantment> CreatureSpells;
        public List<Enchantment> Cooldowns;
        public Enchantment Vitae;

        public EnchantmentRegistry(Player player)
        {
            var registry = player.Biota.BiotaPropertiesEnchantmentRegistry;

            var lifeSpells = GetEntries(registry, EnchantmentMask.LifeSpells);
            var creatureSpells = GetEntries(registry, EnchantmentMask.CreatureSpells);
            var cooldowns = GetEntries(registry, EnchantmentMask.Cooldown);
            var vitae = GetEntries(registry, EnchantmentMask.Vitae);

            LifeSpells = BuildList(lifeSpells, player);
            CreatureSpells = BuildList(creatureSpells, player);
            Cooldowns = BuildList(cooldowns, player);
            Vitae = BuildList(vitae, player).FirstOrDefault();

            SetEnchantMask();
        }

        public void SetEnchantMask()
        {
            EnchantmentMask = 0;

            if (LifeSpells != null && LifeSpells.Count > 0)
                EnchantmentMask |= EnchantmentMask.LifeSpells;
            if (CreatureSpells != null && CreatureSpells.Count > 0)
                EnchantmentMask |= EnchantmentMask.CreatureSpells;
            if (Cooldowns != null && Cooldowns.Count > 0)
                EnchantmentMask |= EnchantmentMask.Cooldown;
            if (Vitae != null)
                EnchantmentMask |= EnchantmentMask.Vitae;
        }

        public static IEnumerable<BiotaPropertiesEnchantmentRegistry> GetEntries(ICollection<BiotaPropertiesEnchantmentRegistry> registry, EnchantmentMask enchantmentMask)
        {
            return registry.Where(e => ((EnchantmentMask)e.EnchantmentCategory).HasFlag(enchantmentMask));
        }

        public static List<Enchantment> BuildList(IEnumerable<BiotaPropertiesEnchantmentRegistry> registry, Player player)
        {
            var enchantments = new List<Enchantment>();

            foreach (var entry in registry)
                enchantments.Add(new Enchantment(player, entry));

            return enchantments;
        }
    }

    public static class EnchantmentRegistryExtensions
    {
        public static void Write(this BinaryWriter writer, EnchantmentRegistry registry)
        {
            var enchantmentMask = registry.EnchantmentMask;

            writer.Write((uint)enchantmentMask);
            if (enchantmentMask.HasFlag(EnchantmentMask.LifeSpells))
                writer.Write(registry.LifeSpells);
            if (enchantmentMask.HasFlag(EnchantmentMask.CreatureSpells))
                writer.Write(registry.CreatureSpells);
            if (enchantmentMask.HasFlag(EnchantmentMask.Cooldown))
                writer.Write(registry.Cooldowns);
            if (enchantmentMask.HasFlag(EnchantmentMask.Vitae))
                writer.Write(registry.Vitae);
        }
    }
}
