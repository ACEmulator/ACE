namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCombatCommmenceAttack : GameEventMessage
    {
        public GameEventCombatCommmenceAttack(Session session)
            : base(GameEventType.CombatCommenceAttack, GameMessageGroup.UIQueue, session)
        {
        }
    }
}
