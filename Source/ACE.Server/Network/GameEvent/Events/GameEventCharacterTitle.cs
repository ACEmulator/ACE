namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public GameEventCharacterTitle(Session session)
            : base(GameEventType.CharacterTitle, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(1u);
            Writer.Write(session.Player.CharacterTitleId ?? 0);
            session.Player.NumCharacterTitles = session.Player.Biota.BiotaPropertiesTitleBook.Count;
            Writer.Write(session.Player.NumCharacterTitles ?? 0);
            foreach (var title in session.Player.Biota.BiotaPropertiesTitleBook)
                Writer.Write(title.TitleId);
        }
    }
}
