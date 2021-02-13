using ACE.Database.Models.Shard;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public GameEventCharacterTitle(Session session)
            : base(GameEventType.CharacterTitle, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(1u);
            Writer.Write(session.Player.CharacterTitleId ?? 0);

            var titles = session.Player.Character.GetTitles(session.Player.CharacterDatabaseLock);

            session.Player.NumCharacterTitles = titles.Count;
            Writer.Write(session.Player.NumCharacterTitles ?? 0);
            foreach (var title in titles)
                Writer.Write(title.TitleId);
        }
    }
}
