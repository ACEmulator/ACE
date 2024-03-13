using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventConfirmationDone : GameEventMessage
    {
        public GameEventConfirmationDone(Session session, ConfirmationType confirmationType, uint contextId)
            : base(GameEventType.CharacterConfirmationDone, GameMessageGroup.UIQueue, session, 12)
        {
            Writer.Write((uint)confirmationType);
            Writer.Write(contextId);
        }
    }
}
