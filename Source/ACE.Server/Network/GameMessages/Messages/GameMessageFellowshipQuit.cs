namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageFellowshipQuit : GameMessage
    {
        public GameMessageFellowshipQuit(Session session, uint playerId)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.UIQueue)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipQuit);
            Writer.Write(playerId);
        }
    }
}
