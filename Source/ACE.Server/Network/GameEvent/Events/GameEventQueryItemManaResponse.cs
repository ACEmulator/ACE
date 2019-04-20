namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventQueryItemManaResponse : GameEventMessage
    {
        public GameEventQueryItemManaResponse(Session session, uint target, float mana, uint success)
            : base(GameEventType.QueryItemManaResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(target);
            Writer.Write(mana);
            Writer.Write(success);
        }
    }
}
