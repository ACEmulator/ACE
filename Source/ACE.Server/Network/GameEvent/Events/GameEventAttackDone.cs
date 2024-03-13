using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAttackDone : GameEventMessage
    {
        public GameEventAttackDone(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.AttackDone, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.Write((uint)errorType);
        }
    }
}
