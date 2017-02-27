using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePlayerTeleport : GameMessage
    {
        public GameMessagePlayerTeleport(uint teleportIndex) 
            : base(GameMessageOpcode.PlayerTeleport, GameMessageGroup.Group0A)
        {
            Writer.Write(teleportIndex);
            //Don't see these in traces or protocol spec:
            //Writer.Write(0u);
            //Writer.Write(0u);
            //Writer.Write((ushort)0);
        }
    }
}