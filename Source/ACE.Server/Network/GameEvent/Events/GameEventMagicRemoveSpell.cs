namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Remove a spell from the player's spellbook
    /// </summary>
    public class GameEventMagicRemoveSpell : GameEventMessage
    {
        public GameEventMagicRemoveSpell(Session session, ushort spellId, ushort layer = 0)
            : base(GameEventType.MagicRemoveSpell, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellId);
            Writer.Write(layer);    // unused?
        }
    }
}
