using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventWeenieErrorWithString : GameEventMessage
    {
        public GameEventWeenieErrorWithString(Session session, WeenieErrorWithString errorType, string message)
            : base(GameEventType.WeenieErrorWithString, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)errorType);
            Writer.WriteString16L(message);
        }
    }
}
