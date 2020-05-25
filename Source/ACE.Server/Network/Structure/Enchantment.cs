using System;
using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Network.Structure
{
    public class Enchantment
    {
        public ushort SpellID;
        public ushort Layer;
        public ushort HasSpellSetID = 1; // default true?
        public ushort SpellCategory;
        public uint PowerLevel;
        public double StartTime;
        public double Duration;
        public uint CasterGuid;     // can be from items
        public float DegradeModifier;
        public float DegradeLimit;
        public double LastTimeDegraded;
        public EnchantmentTypeFlags StatModType;
        public uint StatModKey;
        public float StatModValue;
        public uint SpellSetID;     // only sent if HasSpellSetID = true

        // not sent in network structure
        public WorldObject Target;
        public Spell Spell;
        public EnchantmentMask EnchantmentMask;

        public Enchantment(WorldObject target, uint casterGuid, uint spellId, ushort layer, EnchantmentMask enchantmentMask, float? statModVal = null)
        {
            // 2 references left, can this use BiotaPropertiesEnchantment?
            Init(new Spell(spellId));

            Layer = layer;
            CasterGuid = casterGuid;
            StatModValue = statModVal ?? Spell.StatModVal;

            Target = target;
            EnchantmentMask = enchantmentMask;
        }

        public Enchantment(WorldObject target, SpellBase spellBase, ushort layer, EnchantmentMask enchantmentMask, float? statModVal = null)
        {
            // should be able to replace this with cooldown constructor
            Init(new Spell(spellBase.MetaSpellId));

            Layer = layer;
            CasterGuid = target.Guid.Full;
            StatModValue = statModVal ?? 35.0f;

            Target = target;
            EnchantmentMask = enchantmentMask;
        }

        public Enchantment(WorldObject target, PropertiesEnchantmentRegistry entry)
        {
            if (entry.SpellCategory == (SpellCategory)EnchantmentManager.SpellCategory_Cooldown)
            {
                InitCooldown(target, entry);
                return;
            }

            Init(new Spell((uint)entry.SpellId));

            Layer = entry.LayerId;
            StartTime = entry.StartTime;
            Duration = entry.Duration;      // item spells can have -1, overriding the spell duration
            CasterGuid = entry.CasterObjectId;
            StatModValue = entry.StatModValue;
            SpellSetID = (uint)entry.SpellSetId;

            Target = target;
            EnchantmentMask = (EnchantmentMask)entry.EnchantmentCategory;
        }

        public void Init(Spell spell)
        {
            Spell = spell;
            SpellID = (ushort)spell.Id;
            SpellCategory = (ushort)spell.Category;
            PowerLevel = spell.Power;
            Duration = spell.Duration;
            DegradeModifier = spell.DegradeModifier;
            DegradeLimit = spell.DegradeLimit;

            if (spell._spell != null)
            {
                StatModType = spell.StatModType;
                StatModKey = spell.StatModKey;
            }
        }

        public void InitCooldown(WorldObject target, PropertiesEnchantmentRegistry entry)
        {
            SpellID = (ushort)entry.SpellId;
            Layer = entry.LayerId;
            SpellCategory = (ushort)entry.SpellCategory;
            StartTime = entry.StartTime;
            Duration = entry.Duration;
            CasterGuid = entry.CasterObjectId;
            DegradeModifier = entry.DegradeModifier;
            DegradeLimit = entry.DegradeLimit;
            LastTimeDegraded = entry.LastTimeDegraded;
            StatModType = (EnchantmentTypeFlags)entry.StatModType;
            StatModKey = entry.StatModKey;
            StatModValue = entry.StatModValue;
        }

        public string GetInfo()
        {
            var spell = new Spell(SpellID);

            var info = $"Spell: {spell.Name} ({SpellID})\n";
            info += $"Target: {Target.Name}\n";
            info += $"Layer: {Layer}\n";
            info += $"SpellCategory: {(SpellCategory)SpellCategory}\n";
            info += $"Power: {PowerLevel}\n";
            info += $"StartTime: {StartTime}\n";
            info += $"Duration: {Duration}\n";
            info += $"CasterGuid: {CasterGuid:X8}\n";
            info += $"StatModType: {StatModType}\n";
            info += $"StatModKey: {StatModKey}\n";
            info += $"StatModValue: {StatModValue}\n";
            info += "---------";

            return info;
        }
    }

    public static class EnchantmentExtensions
    {
        public static void Write(this BinaryWriter writer, List<Enchantment> enchantments)
        {
            writer.Write(enchantments.Count);
            foreach (var enchantment in enchantments)
                writer.Write(enchantment);
        }

        public static void Write(this BinaryWriter writer, Enchantment enchantment)
        {
            writer.Write(enchantment.SpellID);
            var layer = enchantment.SpellID == (ushort)SpellId.Vitae ? (ushort)0 : enchantment.Layer; // this line is to force vitae to be layer 0 to match retail pcaps. We save it as layer 1 to make EF Core happy.
            writer.Write(layer);
            writer.Write(enchantment.SpellCategory);
            writer.Write(enchantment.HasSpellSetID);
            writer.Write(enchantment.PowerLevel);
            writer.Write(enchantment.StartTime);
            writer.Write(enchantment.Duration);
            writer.Write(enchantment.CasterGuid);
            writer.Write(enchantment.DegradeModifier);
            writer.Write(enchantment.DegradeLimit);
            writer.Write(enchantment.LastTimeDegraded);     // always 0 / spell economy?
            writer.Write((uint)enchantment.StatModType);
            writer.Write(enchantment.StatModKey);
            writer.Write(enchantment.StatModValue);
            if (enchantment.HasSpellSetID != 0)
                writer.Write(enchantment.SpellSetID);
        }
    }
}
