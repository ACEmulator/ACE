
namespace ACE.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public GameEventCharacterTitle(Session session) : base(GameEventType.CharacterTitle, 0x9, session)
        {
            Writer.Write(1u);
            Writer.Write(1u); // TODO: get current title from database
            Writer.Write(10u); // TODO: get player's title list from database
            for (uint i = 1; i <= 10; i++)
                Writer.Write(i);
        }
    }
}
