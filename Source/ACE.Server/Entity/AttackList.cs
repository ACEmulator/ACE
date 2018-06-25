using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Tracks top damager
    /// </summary>
    public class AttackList
    {
        public Dictionary<WorldObject, uint> Damagers;

        public AttackList()
        {
            Init();
        }

        public AttackList(List<AttackDamage> attackDamages)
        {
            Init();

            foreach (var attackDamage in attackDamages)
                Add(attackDamage.Source, attackDamage.Amount);
        }

        public void Init()
        {
            Damagers = new Dictionary<WorldObject, uint>();
        }

        public void Add(WorldObject damager, uint amount)
        {
            if (Damagers.ContainsKey(damager))
                Damagers[damager] += amount;
            else
                Damagers.Add(damager, amount);
        }

        public WorldObject TopDamager
        {
            get
            {
                var sorted = Damagers.OrderByDescending(wo => wo.Value);
                return sorted.FirstOrDefault().Key;
            }
        }

        /// <summary>
        /// Called when an AttackTarget regains health
        /// </summary>
        /// <param name="healAmount">The amount of health restored</param>
        /// <param name="missingHealth">The amount of health that was missing before healing</param>
        public void OnHeal(int healAmount, int missingHealth)
        {
            // on heal, scale the damage from each source by 1 - healAmount / missingHealth
            var scalar = 1.0f - healAmount / missingHealth;

            foreach (var damager in Damagers.Keys)
                Damagers[damager] *= (uint)Math.Round(scalar);
        }
    }
}
