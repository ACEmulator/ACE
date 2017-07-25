namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicRemoveSpellId : GameEventMessage
    {
        public GameEventMagicRemoveSpellId(Session session, uint spellId)
            : base(GameEventType.RemoveSpellC2S, GameMessageGroup.Group09, session)
        {
            Writer.Write(spellId);
            Writer.Align();
        }
    }
}
