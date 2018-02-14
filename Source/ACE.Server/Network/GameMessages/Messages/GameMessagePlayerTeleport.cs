using ACE.Server.Entity.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerTeleport : GameMessage
    {
        public GameMessagePlayerTeleport(Player player)
            : base(GameMessageOpcode.PlayerTeleport, GameMessageGroup.SmartboxQueue)
        {
            Writer.Write(player.Sequences.GetNextSequence(Sequence.SequenceType.ObjectTeleport));
            // Don't see these in traces or protocol spec:
            // Writer.Write(0u);
            // Writer.Write(0u);
            // Writer.Write((ushort)0);
        }
    }
}
