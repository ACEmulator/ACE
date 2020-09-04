using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Network.Structure
{
    public class EnchantmentRegistry
    {
        public EnchantmentMask EnchantmentMask;
        public Dictionary<EnchantmentMask, List<Enchantment>> Enchantments;

        public EnchantmentRegistry(Player player)
        {
            var enchantments = player.Biota.PropertiesEnchantmentRegistry.Clone(player.BiotaDatabaseLock);

            Enchantments = BuildCategories(player, enchantments);

            var vitae = Enchantments[EnchantmentMask.Vitae].FirstOrDefault();

            if (vitae != null && (vitae.StatModValue.EpsilonEquals(1.0f) || vitae.StatModValue > 1.0f))
            {
                player.EnchantmentManager.Dispel(enchantments.FirstOrDefault(e => e.SpellId == (int)SpellId.Vitae));

                Enchantments[EnchantmentMask.Vitae].Clear();
            }
            SetEnchantMask();
        }

        private static Dictionary<EnchantmentMask, List<Enchantment>> BuildCategories(Player player, ICollection<PropertiesEnchantmentRegistry> registry)
        {
            var categories = new Dictionary<EnchantmentMask, List<Enchantment>>();

            categories.Add(EnchantmentMask.Multiplicative, new List<Enchantment>());
            categories.Add(EnchantmentMask.Additive, new List<Enchantment>());
            categories.Add(EnchantmentMask.Vitae, new List<Enchantment>());
            categories.Add(EnchantmentMask.Cooldown, new List<Enchantment>());

            foreach (var entry in registry)
            {
                var enchantment = new Enchantment(player, entry);

                if (enchantment.SpellID == (int)SpellId.Vitae)
                {
                    categories[EnchantmentMask.Vitae].Add(enchantment);
                }
                else if (enchantment.SpellID > EnchantmentManager.SpellCategory_Cooldown)
                {
                    categories[EnchantmentMask.Cooldown].Add(enchantment);
                }
                else if ((enchantment.StatModType & EnchantmentTypeFlags.Multiplicative) != 0)
                {
                    categories[EnchantmentMask.Multiplicative].Add(enchantment);
                }
                else
                {
                    if ((enchantment.StatModType & EnchantmentTypeFlags.Additive) == 0)
                        Console.WriteLine($"EnchantmentRegistry.BuildCategories(): unknown enchantment {enchantment.SpellID} StatModType {enchantment.StatModType}");

                    categories[EnchantmentMask.Additive].Add(enchantment);
                }
            }
            return categories;
        }

        private void SetEnchantMask()
        {
            EnchantmentMask = 0;
            foreach (var kvp in Enchantments.Where(e => e.Value.Count > 0))
                EnchantmentMask |= kvp.Key;
        }
    }

    public static class EnchantmentRegistryExtensions
    {
        public static void Write(this BinaryWriter writer, EnchantmentRegistry registry)
        {
            var enchantmentMask = registry.EnchantmentMask;

            writer.Write((uint)enchantmentMask);
            if (enchantmentMask.HasFlag(EnchantmentMask.Multiplicative))
                writer.Write(registry.Enchantments[EnchantmentMask.Multiplicative]);
            if (enchantmentMask.HasFlag(EnchantmentMask.Additive))
                writer.Write(registry.Enchantments[EnchantmentMask.Additive]);
            if (enchantmentMask.HasFlag(EnchantmentMask.Cooldown))
                writer.Write(registry.Enchantments[EnchantmentMask.Cooldown]);
            if (enchantmentMask.HasFlag(EnchantmentMask.Vitae))
                writer.Write(registry.Enchantments[EnchantmentMask.Vitae].FirstOrDefault());
        }
    }
}
