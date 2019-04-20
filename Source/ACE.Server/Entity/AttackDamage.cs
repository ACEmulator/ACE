using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// An attackable objects keeps track of its damage sources
    /// </summary>
    public class AttackDamage
    {
        public WorldObject Source;
        public uint Amount;
        public DateTime Time;
        public bool IsCritical;

        /// <summary>
        /// Constructs a new attack damage
        /// </summary>
        /// <param name="source">The attacker or source of damage</param>
        /// <param name="amount">The amount of hit damage</param>
        /// <param name="criticalHit">Flag indicates critical hit</param>
        public AttackDamage(WorldObject source, uint amount, bool criticalHit)
        {
            Source = source;
            Amount = amount;
            Time = DateTime.UtcNow;
            IsCritical = criticalHit;
        }

        /// <summary>
        /// Returns the total damage from the source attacker
        /// </summary>
        /// <param name="attacks">The list of attacks performed on a target</param>
        /// <param name="source">The attacker to add up the damage for</param>
        public static ulong GetTotalDamage(List<AttackDamage> attacks, WorldObject source)
        {
            return (ulong)attacks.Where(a => a.Source == source).Sum(a => a.Amount);
        }

        /// <summary>
        /// Returns TRUE if last attack was critical hit
        /// </summary>
        /// <param name="attacks">The list of attacks performed on a target</param>
        public static bool LastHitCritical(List<AttackDamage> attacks)
        {
            var lastHit = attacks.LastOrDefault();
            if (lastHit != null)
                return lastHit.IsCritical;
            else
                return false;
        }

        /// <summary>
        /// Returns the top damager on creature death
        /// </summary>
        public static WorldObject GetTopDamager(List<AttackDamage> attacks)
        {
            // build the attack list
            return new AttackList(attacks).TopDamager;
        }
    }
}
