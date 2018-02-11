namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicUpdateSpell : GameEventMessage
    {
        public GameEventMagicUpdateSpell(Session session, uint spellId)
            : base(GameEventType.MagicUpdateSpell, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellId);
            Writer.Align();
        }
    }
}
