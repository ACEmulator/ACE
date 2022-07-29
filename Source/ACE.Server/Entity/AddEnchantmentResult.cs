using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Models;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;

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
        public PropertiesEnchantmentRegistry Enchantment;

        /// <summary>
        /// Determines how this enchantment relates
        /// to the most powerful spell in this category
        /// for the surpassing / refreshing / surpassed by message
        /// </summary>
        public StackType StackType;

        /// <summary>
        /// A list of existing enchantments in this stack 
        /// </summary>
        public List<PropertiesEnchantmentRegistry> Surpass;
        public List<PropertiesEnchantmentRegistry> Refresh;
        public List<PropertiesEnchantmentRegistry> Surpassed;

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
        public PropertiesEnchantmentRegistry RefreshCaster { get; set; }

        public ushort TopLayerId { get; set; }

        public ushort NextLayerId => (ushort)(TopLayerId + 1);

        public AddEnchantmentResult() { }

        public AddEnchantmentResult(StackType stackType)
        {
            StackType = stackType;
        }

        public void BuildStack(List<PropertiesEnchantmentRegistry> entries, Spell spell, WorldObject caster, bool equip = false)
        {
            Surpass = new List<PropertiesEnchantmentRegistry>();
            Refresh = new List<PropertiesEnchantmentRegistry>();
            Surpassed = new List<PropertiesEnchantmentRegistry>();

            var powerLevel = spell.Power;

            foreach (var entry in entries.OrderByDescending(i => i.PowerLevel))
            {
                if (powerLevel > entry.PowerLevel)
                {
                    // surpassing existing spell
                    Surpass.Add(entry);
                }
                else if (powerLevel == entry.PowerLevel)
                {
                    // refreshing existing spell
                    if (spell.Id == entry.SpellId)
                    {
                        Refresh.Add(entry);

                        // this could be valid
                        // consider the case: i equip an item that casts strength 6 on me
                        // then i cast strength 6 on myself
                        // the self-cast would find the existing spell from the item, but it wouldn't refresh that one
                        // it should cast to its own layer?

                        //if (Refresh.Count > 1)
                            //Console.WriteLine($"AddEnchantmentResult.BuildStack(): multiple refresh entries");
                    }
                    else
                    {
                        // handle special case to prevent message: Pumpkin Shield casts Web of Defense on you, refreshing Aura of Defense
                        var spellDuration = equip ? double.PositiveInfinity : spell.Duration;

                        if (!equip && caster is Player player && player.AugmentationIncreasedSpellDuration > 0)
                            spellDuration *= 1.0f + player.AugmentationIncreasedSpellDuration * 0.2f;

                        var entryDuration = entry.Duration == -1 ? double.PositiveInfinity : entry.Duration;

                        if (spellDuration > entryDuration || spellDuration == entryDuration && !SpellSet.SetSpells.Contains(entry.SpellId))
                            Surpass.Add(entry);
                        else if (spellDuration < entryDuration)
                            Surpassed.Add(entry);
                        else
                        {
                            // fallback on spell id, for overlapping set spells in multiple sets, where the different 'level' names each have the same spellLevel and powerLevel?
                            // ie. for Gauntlet Damage Boost I and II
                            // this bug still exists in acclient visual enchantment display, unknown whether this bug existed on retail server
                            if (spell.Id > entry.SpellId)
                                Surpass.Add(entry);
                            else
                                Surpassed.Add(entry);
                        }
                    }
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
            // the same spells from different casters should definitely be written to separate layers,
            // but it's questionable if retail sent 'refreshing' or 'surpassing' here
            // these messages were seen in retail pcaps:

            // - Acid Tachi cast Aura of Incantation of Blood Drinker Self on you, surpassing Aura of Incantation of Blood Drinker Self
            // - Scalemail Leggings cast Minor Light Weapon Aptitude on you, surpassing Minor Light Weapon Aptitude
            // - Little Thor cast Aura of Incantation of Spirit Drinker Other on you, surpassing Aura of Incantation of Spirit Drinker Other

            // which seems to indicate the surpass verb was tied directly to whether or not a new layer was added
            // however, at the same time, i would expect to see a ton of references in the pcaps for multiple monsters casting the same spells,
            // such as 'Monster cast Imperil VI on you, surpassing Imperil VI' when 2 monsters have both cast imperil
            // i am seeing a total of 0 references for this monster scenario

            foreach (var refresh in Refresh)
            {
                if (refresh.CasterObjectId == caster.Guid.Full)
                    RefreshCaster = refresh;
            }
        }
    }
}
