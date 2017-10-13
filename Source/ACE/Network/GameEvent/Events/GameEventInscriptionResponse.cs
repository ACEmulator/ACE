namespace ACE.Network.GameEvent.Events
{
    public class GameEventInscriptionResponse : GameEventMessage
    {
        /// <summary>
        /// Sends our response message to the client once we recieve a Game Event (F7B0) Writing_SetInscription (0x00BF) Og II
        /// </summary>
        /// <param name="session">Player Session - used by the base for squence and guid to target</param>
        /// <param name="objectID">This is the object we are inscribing</param>
        /// <param name="inscriptionText">This is the inscription - I am sure it is something profound.</param>
        /// <param name="scribeName">Who is inscribing the object.</param>
        /// <param name="scribeAccount">This is the scribe account - not sure how it works and passing empty string if null</param>
        public GameEventInscriptionResponse(Session session, uint objectID, string inscriptionText, string scribeName, string scribeAccount)
                : base(GameEventType.GetInscriptionResponse, GameMessageGroup.Group09, session)
        {
            //TODO: This message is not currently described correctly in our XML.   Need to let @Zegeger know the correct format. Og II
            Writer.Write(objectID);
            Writer.WriteString16L(inscriptionText);
            Writer.Write(session.Player.Guid.Full);
            Writer.WriteString16L(scribeName);
            Writer.WriteString16L(scribeAccount);
            Writer.Align();
        }
    }
}