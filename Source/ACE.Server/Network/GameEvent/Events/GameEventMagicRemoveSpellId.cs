namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicRemoveSpellId : GameEventMessage
    {
        public GameEventMagicRemoveSpellId(Session session, uint spellId)
            : base(GameEventType.MagicRemoveSpell, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellId);
            Writer.Align();
        }
    }
}
