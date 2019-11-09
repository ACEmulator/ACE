using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventInscriptionResponse : GameEventMessage
    {
        /// <summary>
        /// THIS EVENT IS DEPRECIATED AND HAS NO HANDLER IN ACCLIENT
        /// </summary>
        public GameEventInscriptionResponse(Session session, WorldObject worldObject)
                : base(GameEventType.GetInscriptionResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.WriteString16L(worldObject.Inscription);
            Writer.WriteGuid(session.Player.Guid);
            Writer.WriteString16L(worldObject.ScribeName);
            Writer.WriteString16L(worldObject.ScribeAccount);
            Writer.Align();
        }
    }
}
