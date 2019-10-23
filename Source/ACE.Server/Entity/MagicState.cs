using System;

using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages.Messages;
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

        public DateTime StartTime { get; set; }

        public int CastNum { get; set; }

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
            StartTime = DateTime.UtcNow;

            if (Player.UnderLifestoneProtection)
                Player.LifestoneProtectionDispel();

            CastNum++;

            if (Player.RecordCast.Enabled)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Cast #: {CastNum}", ChatMessageType.Broadcast));
                Player.RecordCast.Log($"MagicState.OnCastStart({CastNum})");
            }
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
            CastGesture = MotionCommand.Invalid;

            CastSpellParams = null;

            if (Player.RecordCast.Enabled)
            {
                Player.RecordCast.Log($"MagicState.OnCastDone()\n================================================================================");
                Player.RecordCast.Flush();
            }
        }

        public void SetCastParams(Spell spell, bool isWeaponSpell, uint manaUsed, WorldObject target, Player.CastingPreCheckStatus status)
        {
            CastSpellParams = new CastSpellParams(spell, isWeaponSpell, manaUsed, target, status);
        }

        public override string ToString()
        {
            var str = $"Player: {Player.Name} ({Player.Guid})\n";
            str += $"IsCasting: {IsCasting}\n";
            str += $"CastMotionDone: {CastMotionDone}\n";
            str += $"WindupTurn: {WindupTurn}\n";
            str += $"CastTurn: {CastTurn}\n";
            str += $"CastTurnStarted: {CastTurnStarted}\n";
            str += $"Launched: {Launched} \n";
            str += $"CastSpellParams: {CastSpellParams}\n";
            str += $"CastGesture: {CastGesture}";
            return str;
        }
    }
}
