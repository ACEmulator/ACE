namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
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
        /// Called every ~1 second
        /// </summary>
        public void DoTick()
        {
            Think();
            QueueNextTick();
        }

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        public void Think()
        {
            if (!IsAwake || IsDead) return;

            if (!IsTurning && !IsMoving && !IsMeleeRange())
                StartTurn();
            else if (MeleeReady())
                MeleeAttack();
            else
                Movement();
        }
    }
}
