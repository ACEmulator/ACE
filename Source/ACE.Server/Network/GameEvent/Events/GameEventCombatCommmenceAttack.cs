using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCombatCommmenceAttack : GameEventMessage
    {
        public GameEventCombatCommmenceAttack(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.CombatCommenceAttack, GameMessageGroup.UIQueue, session)
        {
        }
    }
}
