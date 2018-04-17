using System.Collections.Generic;
using System.IO;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class Enchantment
    {
        public WorldObject Target;
        public SpellBase SpellBase;
        public Spell Spell;
        public uint Layer;
        public EnchantmentMask EnchantmentMask;
        public float? StatMod;

        public Enchantment(WorldObject target, uint spellId, uint layer, uint? enchantmentMask, float? statMod = null)
        {
            Target = target;
            SpellBase = DatManager.PortalDat.SpellTable.Spells[spellId];
            Spell = DatabaseManager.World.GetCachedSpell(spellId);
            Layer = layer;
            EnchantmentMask = (EnchantmentMask)(enchantmentMask ?? 0);
            StatMod = statMod;
        }

        public Enchantment(WorldObject target, SpellBase spellBase, uint layer, uint? enchantmentMask, float? statMod = null)
        {
            Target = target;
            SpellBase = spellBase;
            Layer = layer;
            EnchantmentMask = (EnchantmentMask)(enchantmentMask ?? 0);
            StatMod = statMod;
        }

        public Enchantment(WorldObject target, BiotaPropertiesEnchantmentRegistry entry)
        {
            Target = target;
            SpellBase = DatManager.PortalDat.SpellTable.Spells[(uint)entry.SpellId];
            Spell = DatabaseManager.World.GetCachedSpell((uint)entry.SpellId);
            Layer = entry.LayerId;
            EnchantmentMask = (EnchantmentMask)entry.EnchantmentCategory;
            StatMod = entry.StatModValue;
        }
    }

    public static class EnchantmentExtentions
    {
        public static readonly double StartTime = 0;
        public static readonly double LastTimeDegraded = 0;
        public static readonly float DefaultStatMod = 35.0f;

        public static readonly ushort HasSpellSetId = 1;
        public static readonly uint SpellSetId = 0;

        public static void Write(this BinaryWriter writer, List<Enchantment> enchantments)
        {
            writer.Write(enchantments.Count);
            foreach (var enchantment in enchantments)
                writer.Write(enchantment);
        }

        public static void Write(this BinaryWriter writer, Enchantment enchantment)
        {
            var spell = enchantment.Spell;
            var statModType = spell != null ? spell.StatModType ?? 0 : 0;
            var statModKey = spell != null ? spell.StatModKey ?? 0 : 0;

            writer.Write((ushort)enchantment.SpellBase.MetaSpellId);
            writer.Write((ushort)enchantment.Layer);
            writer.Write((ushort)enchantment.SpellBase.Category);
            writer.Write(HasSpellSetId);
            writer.Write(enchantment.SpellBase.Power);
            writer.Write(StartTime);            // FIXME: this needs to be passed in
            writer.Write(enchantment.SpellBase.Duration);
            writer.Write(enchantment.Target.Guid.Full);
            writer.Write(enchantment.SpellBase.DegradeModifier);
            writer.Write(enchantment.SpellBase.DegradeLimit);
            writer.Write(LastTimeDegraded);     // This needs timer updates to work correctly
            writer.Write(statModType);
            writer.Write(statModKey);
            writer.Write(enchantment.StatMod ?? DefaultStatMod);
            writer.Write(SpellSetId);
        }
    }
}
