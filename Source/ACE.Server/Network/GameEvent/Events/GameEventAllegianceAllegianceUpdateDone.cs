using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceAllegianceUpdateDone : GameMessage
    {
        public GameEventAllegianceAllegianceUpdateDone(Session session)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.UIQueue)
        {
            //Writer.Write(session.Player.Guid.Full);
            //Writer.Write(session.GameEventSequence++);
            //Writer.Write((uint)GameEvent.GameEventType.AllegianceAllegianceUpdateDone);
            Writer.Write(0x00);
        }
    }
}
