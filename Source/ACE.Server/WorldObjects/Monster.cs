using System;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
        public bool IsMonster;

        /// <summary>
        /// The exclusive state of the monster
        /// </summary>
        public State MonsterState = State.Idle;

        /// <summary>
        /// The exclusive states the monster can be in
        /// </summary>
        public enum State
        {
            Idle,
            Awake
        };

        /// <summary>
        /// Called on monster death, before Die()
        /// </summary>
        public void OnDeath()
        {
            IsTurning = false;
            IsMoving = false;

            EmoteManager.OnDeath(DamageHistory);

            // handle summoning portals on creature death
            if (LinkedPortalOneDID != null)
                SummonPortal(LinkedPortalOneDID.Value);
        }
    }
}
