namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public GameEventCharacterTitle(Session session)
            : base(GameEventType.CharacterTitle, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(1u);
            Writer.Write(session.Player.CharacterTitleId ?? 0);
            session.Player.NumCharacterTitles = session.Player.Character.CharacterPropertiesTitleBook.Count;
            Writer.Write(session.Player.NumCharacterTitles ?? 0);
            foreach (var title in session.Player.Character.CharacterPropertiesTitleBook)
                Writer.Write(title.TitleId);
        }
    }
}
