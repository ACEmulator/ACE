namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicRemoveSpellId : GameEventMessage
    {
        public GameEventMagicRemoveSpellId(Session session, uint spellId)
            : base(GameEventType.MagicRemoveSpell, GameMessageGroup.Group09, session)
        {
            Writer.Write(spellId);
            Writer.Align();
        }
    }
}
