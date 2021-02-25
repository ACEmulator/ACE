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

        public bool IsPassivePet { get; set; }

        public bool IsFactionMob { get; set; }

        public bool HasFoeType { get; set; }

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

            IsPassivePet = WeenieType == WeenieType.Pet;
            IsChessPiece = WeenieType == WeenieType.GamePiece;

            // includes CombatPets
            IsMonster = Attackable || TargetingTactic != TargetingTactic.None;

            IsFactionMob = IsMonster && WeenieType != WeenieType.CombatPet && Faction1Bits != null;

            HasFoeType = IsMonster && FoeType != null;
        }
    }
}
