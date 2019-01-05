using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.Shard;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class Enchantment
    {
        public WorldObject Target;
        public uint CasterGuid;
        public Spell Spell;
        public ushort Layer;
        public EnchantmentMask EnchantmentMask;
        public double StartTime;
        public double Duration;
        public float? StatMod;

        public Enchantment(WorldObject target, uint casterGuid, uint spellId, double duration, ushort layer, EnchantmentMask enchantmentMask, float? statMod = null)
        {
            Target = target;
            CasterGuid = casterGuid;
            Spell = new Spell(spellId);
            Layer = layer;
            Duration = duration;
            EnchantmentMask = enchantmentMask;
            StatMod = statMod ?? Spell.StatModVal;
        }

        public Enchantment(WorldObject target, uint casterGuid, SpellBase spellBase, double duration, ushort layer, EnchantmentMask enchantmentMask, float? statMod = null)
        {
            Target = target;
            CasterGuid = casterGuid;
            Spell = new Spell(spellBase.MetaSpellId);
            Layer = layer;
            Duration = duration;
            EnchantmentMask = enchantmentMask;
            StatMod = statMod;
        }

        public Enchantment(WorldObject target, BiotaPropertiesEnchantmentRegistry entry)
        {
            Target = target;
            CasterGuid = entry.CasterObjectId;
            Spell = new Spell((uint)entry.SpellId);
            Layer = entry.LayerId;
            StartTime = entry.StartTime;
            Duration = entry.Duration;
            EnchantmentMask = (EnchantmentMask)entry.EnchantmentCategory;
            StatMod = entry.StatModValue;
        }
    }

    public static class EnchantmentExtensions
    {
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
            var statModType = spell.StatModType;
            var statModKey = spell.StatModKey;

            writer.Write((ushort)enchantment.Spell.Id);
            var layer = (spell.Id == (uint)SpellId.Vitae) ? (ushort)0 : enchantment.Layer; // this line is to force vitae to be layer 0 to match retail pcaps. We save it as layer 1 to make EF Core happy.
            writer.Write(layer);
            writer.Write((ushort)enchantment.Spell.Category);
            writer.Write(HasSpellSetId);
            writer.Write(enchantment.Spell.Power);
            writer.Write(enchantment.StartTime);
            writer.Write(enchantment.Duration);
            writer.Write(enchantment.CasterGuid);
            writer.Write(enchantment.Spell.DegradeModifier);
            writer.Write(enchantment.Spell.DegradeLimit);
            writer.Write(LastTimeDegraded);     // always 0 / spell economy?
            writer.Write((uint)statModType);
            writer.Write(statModKey);
            writer.Write(enchantment.StatMod ?? DefaultStatMod);
            writer.Write(SpellSetId);
        }
    }
}
