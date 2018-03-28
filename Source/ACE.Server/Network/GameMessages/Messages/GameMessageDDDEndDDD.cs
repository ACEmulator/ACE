namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDEndDDD : GameMessage
    {
        public GameMessageDDDEndDDD()
            : base(GameMessageOpcode.DDD_EndDDD, GameMessageGroup.DatabaseQueue)
        {
        }
    }
}
