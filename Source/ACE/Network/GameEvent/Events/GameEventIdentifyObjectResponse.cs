
namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, uint objectID) 
            : base(GameEventType.IdentifyObjectResponse, 0x9, session)
        {
            // TODO

            Writer.Write(objectID);
            Writer.Write(0u);
            Writer.Write(1u);
        }
    }
}
