using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageUpdateInstanceId : GameMessage
    {
        public GameMessageUpdateInstanceId(ObjectGuid containerGuid, ObjectGuid itemGuid)
            : base(GameMessageOpcode.UpdateInstanceId, GameMessageGroup.Group09)
        {
            Writer.Write((ushort)1);
            Writer.Write(itemGuid.Full);
            Writer.Write((uint)containerGuid.Full);
            Writer.Align();
        }
    }
}
