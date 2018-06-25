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
    }
}
