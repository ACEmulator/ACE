using ACE.Common.Extensions;
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
        public List<Enchantment> Multipliers;
        public List<Enchantment> Additives;
        public List<Enchantment> Cooldowns;
        public Enchantment Vitae;

        public EnchantmentRegistry(Player player)
        {
            var enchantments = player.Biota.GetEnchantments(player.BiotaDatabaseLock);

            var multipliers = GetEntries(enchantments, EnchantmentMask.Multiplier);
            var additives = GetEntries(enchantments, EnchantmentMask.Additive);
            var cooldowns = GetEntries(enchantments, EnchantmentMask.Cooldown);
            var vitae = GetEntries(enchantments, EnchantmentMask.Vitae);

            Multipliers = BuildList(multipliers, player);
            Additives = BuildList(additives, player);
            Cooldowns = BuildList(cooldowns, player);
            Vitae = BuildList(vitae, player).FirstOrDefault();

            if (Vitae != null && (Vitae.StatModValue.EpsilonEquals(1.0f) || Vitae.StatModValue > 1.0f))
            {
                player.EnchantmentManager.Dispel(vitae.First());
                Vitae = null;
            }

            SetEnchantMask();
        }

        private static IEnumerable<BiotaPropertiesEnchantmentRegistry> GetEntries(ICollection<BiotaPropertiesEnchantmentRegistry> registry, EnchantmentMask enchantmentMask)
        {
            // TODO: improve this code, giving each Enchantment an EnchantmentMask,
            // classifying Vitae / Cooldown first, and then Multiplier / Additive
            switch (enchantmentMask)
            {
                case EnchantmentMask.Multiplier:
                    return registry.Where(e => (e.StatModType & (int)EnchantmentTypeFlags.Multiplicative) != 0 && e.SpellId != (int)SpellId.Vitae && e.SpellId <= 0x8000);

                case EnchantmentMask.Additive:
                default:
                    return registry.Where(e => (e.StatModType & (int)EnchantmentTypeFlags.Additive) != 0 && e.SpellId != (int)SpellId.Vitae && e.SpellId <= 0x8000);

                case EnchantmentMask.Vitae:
                    return registry.Where(e => e.SpellId == (int)SpellId.Vitae);

                case EnchantmentMask.Cooldown:
                    return registry.Where(e => e.SpellId > 0x8000);
            }
        }

        private static List<Enchantment> BuildList(IEnumerable<BiotaPropertiesEnchantmentRegistry> registry, Player player)
        {
            var enchantments = new List<Enchantment>();

            foreach (var entry in registry)
                enchantments.Add(new Enchantment(player, entry));

            return enchantments;
        }

        private void SetEnchantMask()
        {
            EnchantmentMask = 0;

            if (Multipliers != null && Multipliers.Count > 0)
                EnchantmentMask |= EnchantmentMask.Multiplier;
            if (Additives != null && Additives.Count > 0)
                EnchantmentMask |= EnchantmentMask.Additive;
            if (Cooldowns != null && Cooldowns.Count > 0)
                EnchantmentMask |= EnchantmentMask.Cooldown;
            if (Vitae != null)
                EnchantmentMask |= EnchantmentMask.Vitae;
        }
    }

    public static class EnchantmentRegistryExtensions
    {
        public static void Write(this BinaryWriter writer, EnchantmentRegistry registry)
        {
            var enchantmentMask = registry.EnchantmentMask;

            writer.Write((uint)enchantmentMask);
            if (enchantmentMask.HasFlag(EnchantmentMask.Multiplier))
                writer.Write(registry.Multipliers);
            if (enchantmentMask.HasFlag(EnchantmentMask.Additive))
                writer.Write(registry.Additives);
            if (enchantmentMask.HasFlag(EnchantmentMask.Cooldown))
                writer.Write(registry.Cooldowns);
            if (enchantmentMask.HasFlag(EnchantmentMask.Vitae))
                writer.Write(registry.Vitae);
        }
    }
}
