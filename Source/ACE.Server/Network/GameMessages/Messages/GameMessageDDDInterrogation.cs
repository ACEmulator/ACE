namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDInterrogation : GameMessage
    {
        public GameMessageDDDInterrogation()
            : base(GameMessageOpcode.DDD_Interrogation, GameMessageGroup.DatabaseQueue)
        {
            Writer.Write(1u);
            Writer.Write(1u);
            Writer.Write(1u);
            Writer.Write(2u);
            Writer.Write(0u);
            Writer.Write(1u);
        }
    }
}
