
namespace ACE.Network.GameEvent.Events
{
    public class GameEventUpdateTitle : GameEventMessage
    {
        private uint title;

        public GameEventUpdateTitle(Session session, uint title)
            : base(GameEventType.UpdateTitle, session)
        {
            this.title = title;
            writer.Write(this.title);
            writer.Write(1u);
        }
    }
}
