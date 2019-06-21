using System;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /*public enum CastingState
    {
        WindupTurn,
        WindupGesture,
        CastGesture,
        CastTurn,
        Ready
    };*/

    public class MagicState
    {
        //public CastingState CastingState { get; set; }

        /// <summary>
        /// A reference to the Player for this MagicState
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Returns TRUE is player is currently in the process of casting a spell
        /// </summary>
        public bool IsCasting { get; set; }

        /// <summary>
        /// Returns TRUE if the first half of the 'launch spell' motion
        /// has made its way through the motion queue
        /// </summary>
        public bool CastMotionDone { get; set; }

        public bool WindupTurn { get; set; }

        public bool CastTurn { get; set; }

        public bool CastTurnStarted { get; set; }

        public bool Launched { get; set; }

        public CastSpellParams CastSpellParams { get; set; }

        /// <summary>
        /// The 'launch spell' motion for the current cast
        /// </summary>
        public MotionCommand CastGesture { get; set; }

        public MagicState(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Called when the player begins casting a spell
        /// </summary>
        public void OnCastStart()
        {
            Player.IsBusy = true;
            IsCasting = true;
            CastMotionDone = false;
            Launched = false;
            WindupTurn = false;
            CastTurn = false;
            CastTurnStarted = false;

            if (Player.UnderLifestoneProtection)
                Player.LifestoneProtectionDispel();
        }

        /// <summary>
        /// Called when the player finishes casting a spell
        /// </summary>
        public void OnCastDone()
        {
            Player.IsBusy = false;
            IsCasting = false;
            CastMotionDone = false;
            Launched = false;
            WindupTurn = false;
            CastTurn = false;
            CastTurnStarted = false;

            CastSpellParams = null;
        }

        public void SetCastParams(Spell spell, bool isWeaponSpell, uint manaUsed, WorldObject target, Player.CastingPreCheckStatus status)
        {
            CastSpellParams = new CastSpellParams(spell, isWeaponSpell, manaUsed, target, status);
        }
    }
}
