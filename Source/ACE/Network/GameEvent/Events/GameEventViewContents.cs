namespace ACE.Network.GameEvent.Events
{
    public class GameEventViewContents : GameEventMessage
    {
        public GameEventViewContents(Session session, uint objectId)
            : base(GameEventType.ViewContents, GameMessageGroup.Group09, session)
        {
            // TODO: build this out with container contents list.    Stubbing for now.   Og II
            Writer.Write(objectId);
            Writer.Write(0u);
        }
    }
}
