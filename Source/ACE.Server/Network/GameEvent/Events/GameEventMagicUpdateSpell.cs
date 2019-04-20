namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Adds a spell to the player's spellbook.
    /// </summary>
    public class GameEventMagicUpdateSpell : GameEventMessage
    {
        public GameEventMagicUpdateSpell(Session session, ushort spellId, ushort layer = 0)
            : base(GameEventType.MagicUpdateSpell, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellId);
            Writer.Write(layer);    // unused?
        }
    }
}
