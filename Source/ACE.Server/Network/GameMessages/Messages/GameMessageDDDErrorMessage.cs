namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDErrorMessage : GameMessage
    {
        public GameMessageDDDErrorMessage(uint resourceType, uint dataId, uint errorType)
            : base(GameMessageOpcode.DDD_ErrorMessage, GameMessageGroup.DatabaseQueue)
        {
            Writer.Write(resourceType);
            Writer.Write(dataId);
            Writer.Write(errorType);
        }
    }
}
