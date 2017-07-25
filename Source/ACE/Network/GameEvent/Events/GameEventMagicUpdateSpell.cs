namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicUpdateSpell : GameEventMessage
    {
        public GameEventMagicUpdateSpell(Session session, uint spellId)
            : base(GameEventType.MagicUpdateSpell, GameMessageGroup.Group09, session)
        {
            Writer.Write(spellId);
            Writer.Align();
        }
    }
}
