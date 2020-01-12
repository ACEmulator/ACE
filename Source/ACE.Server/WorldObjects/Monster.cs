using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
        public bool IsMonster { get; set; }

        public bool IsChessPiece { get; set; }

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
            Awake,
            Return
        };

        /// <summary>
        /// Determines if this creature runs combat ai,
        /// and caches into IsMonster
        /// </summary>
        public void SetMonsterState()
        {
            if (this is Player) return;

            IsChessPiece = this is GamePiece;

            // includes CombatPets
            IsMonster = Attackable || TargetingTactic != TargetingTactic.None;
        }
    }
}
