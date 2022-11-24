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
        /// This flag indicates if player is currently casting a spell
        /// //
        /// It gets set to TRUE when they press the cast button,
        /// and becomes FALSE again when their recoil animation has completed.
        ///
        /// If server is running with spellcast_recoil_queue = true,
        /// it becomes FALSE when their recoil animation has started.
        /// </summary>
        public bool IsCasting { get; set; }

        /// <summary>
        /// Returns TRUE if the first half of the 'launch spell' motion
        /// has made its way through the motion queue
        /// </summary>
        public bool CastMotionDone { get; set; }

        /// <summary>
        /// Returns TRUE if player has started turning for either the windup or cast launch
        /// After turning has completed, this will still be true
        /// </summary>
        public bool TurnStarted { get; set; }

        /// <summary>
        /// Returns TRUE if current player animation frame is turning
        /// </summary>
        public bool IsTurning { get; set; }

        /// <summary>
        /// Information required for performing the windup
        /// </summary>
        public WindupParams WindupParams { get; set; }

        /// <summary>
        /// Information required for launching the spell
        /// </summary>
        public CastSpellParams CastSpellParams { get; set; }

        /// <summary>
        /// The 'launch spell' motion for the current cast
        /// </summary>
        public MotionCommand CastGesture { get; set; }

        /// <summary>
        /// The time when the player pressed the key to begin spell casting
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// If a player interrupts a TurnTo during casting,
        /// the TurnTo resumes when the player is no longer holding any Turn keys
        /// </summary>
        public bool PendingTurnRelease { get; set; }

        /// <summary>
        /// Tracks the cast # for /recordcast
        /// </summary>
        public int CastNum { get; set; }

        /// <summary>
        /// If TRUE, the casting efficiency meter has been enabled with /castmeter
        /// This shows the player information about how quickly they interrupted the cast motion
        /// 
        /// - 0% would be a standard cast, with no additional buttons pressed
        /// - 50% would mean they interrupted the cast motion 1/2 way through
        /// - 100% could theoretically be possible, and would indicate the cast motion was completely avoided
        /// - Negative % indicates the player pressed additional keys which actually slowed down the cast
        /// </summary>
        public bool CastMeter { get; set; }

        /// <summary>
        /// The time when the player started performing the 'launch spell' casting gesture
        /// This is used for CastMeter measurement
        /// </summary>
        public DateTime CastGestureStartTime { get; set; }

        /// <summary>
        /// This is only used if server option spellcast_recoil_queue = true
        /// Allows the player to queue the next spellcast as soon as the previous spell is released
        /// </summary>
        public bool CanQueue;
        public CastQueue CastQueue;

        /// <summary>
        /// By default, MoveToManager waits for the player to be in Ready state
        /// before beginning a TurnTo. For some motions, such as immmediately after the CastGesture,
        /// this can produce unnecessary delays.
        /// This will be set to TRUE only for the first turn after the CastGesture
        /// </summary>
        public bool AlwaysTurn;

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
            TurnStarted = false;
            IsTurning = false;
            PendingTurnRelease = false;
            CanQueue = false;
            CastQueue = null;
            AlwaysTurn = false;

            StartTime = DateTime.UtcNow;
            CastGestureStartTime = DateTime.MinValue;

            if (Player.UnderLifestoneProtection)
                Player.LifestoneProtectionDispel();

            CastNum++;

            if (Player.RecordCast.Enabled)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Cast #: {CastNum}", ChatMessageType.Broadcast));
                Player.RecordCast.Log($"MagicState.OnCastStart({CastNum})");
                Player.RecordCast.Log($"Player Location: {Player.Location.ToLOCString()}");
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
            TurnStarted = false;
            IsTurning = false;
            PendingTurnRelease = false;
            Player.TurnTarget = null;
            CanQueue = false;
            CastQueue = null;
            AlwaysTurn = false;

            CastGesture = MotionCommand.Invalid;
            CastGestureStartTime = DateTime.MinValue;

            if (Player.RecordCast.Enabled)
            {
                Player.RecordCast.Log($"MagicState.OnCastDone()");
                Player.RecordCast.Log($"Player Location: {Player.Location.ToLOCString()}");
                if (CastSpellParams?.Target != null)
                    Player.RecordCast.Log($"Target Location: {CastSpellParams.Target.Location.ToLOCString()}");
                Player.RecordCast.Log("================================================================================");
                Player.RecordCast.Flush();
            }

            CastSpellParams = null;
            WindupParams = null;
        }

        public void SetCastParams(Spell spell, WorldObject caster, uint magicSkill, uint manaUsed, WorldObject target, Player.CastingPreCheckStatus status)
        {
            CastSpellParams = new CastSpellParams(spell, caster, magicSkill, manaUsed, target, status);

            if (Player.RecordCast.Enabled && CastSpellParams.Target != null)
                Player.RecordCast.Log($"Target Location: {CastSpellParams.Target.Location.ToLOCString()}");
        }

        public void SetWindupParams(uint targetGuid, uint spellId, WorldObject casterItem)
        {
            WindupParams = new WindupParams(targetGuid, spellId, casterItem);
        }

        public override string ToString()
        {
            var str = $"Player: {Player.Name} ({Player.Guid})\n";
            str += $"IsCasting: {IsCasting}\n";
            str += $"CastMotionStarted: {CastMotionDone}\n";
            str += $"CastMotionDone: {CastMotionDone}\n";
            str += $"TurnStarted: {TurnStarted}\n";
            str += $"IsTurning: {IsTurning}\n";
            str += $"WindupParams: {WindupParams}\n";
            str += $"CastSpellParams: {CastSpellParams}\n";
            str += $"CastGesture: {CastGesture}\n";
            str += $"StartTime: {StartTime}";
            return str;
        }
    }
}
