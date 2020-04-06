using System.Collections.Generic;
using System.IO;

using ACE.Entity.Models;

namespace ACE.Server.Network.Structure
{
    public class LayeredSpell
    {
        public ushort SpellId;
        public ushort Layer;

        public LayeredSpell() { }

        public LayeredSpell(ushort spellId, ushort layer)
        {
            SpellId = spellId;
            Layer = layer;
        }

        public LayeredSpell(PropertiesEnchantmentRegistry enchantment)
        {
            SpellId = (ushort)enchantment.SpellId;
            Layer = enchantment.LayerId;
        }
    }

    public static class LayeredSpellExtensions
    {
        public static LayeredSpell ReadLayeredSpell(this BinaryReader reader)
        {
            var layeredSpell = new LayeredSpell();
            layeredSpell.SpellId = reader.ReadUInt16();
            layeredSpell.Layer = reader.ReadUInt16();

            return layeredSpell;
        }

        public static void Write(this BinaryWriter writer, LayeredSpell spell)
        {
            writer.Write(spell.SpellId);
            writer.Write(spell.Layer);
        }

        public static void Write(this BinaryWriter writer, List<LayeredSpell> spells)
        {
            writer.Write(spells.Count);
            foreach (var spell in spells)
                writer.Write(spell);
        }

        public static void Write(this BinaryWriter writer, PropertiesEnchantmentRegistry enchantment)
        {
            writer.Write((ushort)enchantment.SpellId);
            writer.Write(enchantment.LayerId);
        }

        public static void Write(this BinaryWriter writer, List<PropertiesEnchantmentRegistry> enchantments)
        {
            writer.Write(enchantments.Count);
            foreach (var enchantment in enchantments)
                writer.Write(enchantment);
        }
    }
}
