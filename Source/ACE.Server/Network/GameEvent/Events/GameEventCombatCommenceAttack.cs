namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCombatCommenceAttack : GameEventMessage
    {
        public GameEventCombatCommenceAttack(Session session)
            : base(GameEventType.CombatCommenceAttack, GameMessageGroup.UIQueue, session)
        {
        }
    }
}
