
namespace ACE.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public GameEventCharacterTitle(Session session) : base(GameEventType.CharacterTitle, session)
        {
            writer.Write(1u);
            writer.Write(1u); // TODO: get current title from database
            writer.Write(10u); // TODO: get player's title list from database
            for (uint i = 1; i <= 10; i++)
                writer.Write(i);
        }
    }
}
