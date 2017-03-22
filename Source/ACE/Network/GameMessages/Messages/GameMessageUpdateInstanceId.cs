using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageUpdateInstanceId : GameMessage
    {
        public GameMessageUpdateInstanceId(ObjectGuid containerGuid, ObjectGuid itemGuid)
            : base(GameMessageOpcode.UpdateInstanceId, GameMessageGroup.Group09)
        {
            // TODO: research - could these types of sends be generalized by payload type?   for example GameMessageInt 
            Writer.Write((byte)1);  // wts
            Writer.Write(containerGuid.Full); // sender
            Writer.Write((uint)2);
            Writer.Write(itemGuid.Full); // new value of the container id
            Writer.Align(); // not sure that I need this - can someone explain when to use this?
        }
    }
}
