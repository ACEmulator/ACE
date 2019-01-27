using System;
using System.Collections.Generic;
using ACE.Database.Models.Shard;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The result of adding an enchantment,
    /// and where it fits into the current stack
    /// </summary>
    public class AddEnchantmentResult
    {
        /// <summary>
        ///  The resulting enchantment that was added or refreshed
        ///  This is set in EnchantmentManager.Add()
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry Enchantment;

        /// <summary>
        /// Determines how this enchantment relates
        /// to the most powerful spell in this category
        /// for the surpassing / refreshing / surpassed by message
        /// </summary>
        public StackType StackType;

        /// <summary>
        /// A list of existing enchantments in this stack 
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> Surpass;
        public List<BiotaPropertiesEnchantmentRegistry> Refresh;
        public List<BiotaPropertiesEnchantmentRegistry> Surpassed;

        /// <summary>
        /// The most powerful spells in this stack,
        /// for each stack type
        /// </summary>
        public Spell SurpassSpell;
        public Spell RefreshSpell;
        public Spell SurpassedSpell;

        /// <summary>
        /// This handles situations where the same spell can come
        /// from both a creature and item source
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry RefreshCaster { get; set; }

        public ushort TopLayerId;

        public ushort NextLayerId => (ushort)(TopLayerId + 1);

        public AddEnchantmentResult() { }

        public AddEnchantmentResult(StackType stackType)
        {
            StackType = stackType;
        }

        public void BuildStack(List<BiotaPropertiesEnchantmentRegistry> entries, Spell spell, WorldObject caster)
        {
            Surpass = new List<BiotaPropertiesEnchantmentRegistry>();
            Refresh = new List<BiotaPropertiesEnchantmentRegistry>();
            Surpassed = new List<BiotaPropertiesEnchantmentRegistry>();

            var powerLevel = spell.Power;

            foreach (var entry in entries)
            {
                if (powerLevel > entry.PowerLevel)
                {
                    // surpassing existing spell
                    Surpass.Add(entry);
                }
                else if (powerLevel == entry.PowerLevel)
                {
                    // refreshing existing spell
                    Refresh.Add(entry);

                    // this could be valid
                    // consider the case: i equip an item that casts strength 6 on me
                    // then i cast strength 6 on myself
                    // the self-cast would find the existing spell from the item, but it wouldn't refresh that one
                    // it should cast to its own layer?

                    //if (Refresh.Count > 1)
                        //Console.WriteLine($"AddEnchantmentResult.BuildStack(): multiple refresh entries");
                }
                else if (powerLevel < entry.PowerLevel)
                {
                    // surpassed by existing spell
                    Surpassed.Add(entry);
                }

                if (entry.LayerId > TopLayerId)
                    TopLayerId = entry.LayerId;
            }

            SetStackType();
            SetSpell();

            if (Refresh.Count > 0)
                SetRefreshCaster(caster);
        }

        public void SetStackType()
        {
            if (Surpassed.Count > 0)
                StackType = StackType.Surpassed;
            else if (Refresh.Count > 0)
                StackType = StackType.Refresh;
            else if (Surpass.Count > 0)
                StackType = StackType.Surpass;
        }

        public void SetSpell()
        {
            if (Surpass.Count > 0)
                SurpassSpell = new Spell(Surpass[0].SpellId, false);

            if (Refresh.Count > 0)
                RefreshSpell = new Spell(Refresh[0].SpellId, false);

            if (Surpassed.Count > 0)
                SurpassedSpell = new Spell(Surpassed[0].SpellId, false);
        }

        public void SetRefreshCaster(WorldObject caster)
        {
            foreach (var refresh in Refresh)
            {
                if (refresh.CasterObjectId == caster.Guid.Full)
                    RefreshCaster = refresh;
            }
        }
    }
}
