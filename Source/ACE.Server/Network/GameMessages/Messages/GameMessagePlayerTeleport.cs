using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerTeleport : GameMessage
    {
        public GameMessagePlayerTeleport(Player player)
            : base(GameMessageOpcode.PlayerTeleport, GameMessageGroup.SmartboxQueue)
        {
            Writer.Write((ushort)0);
            Writer.Write(player.Sequences.GetNextSequence(Sequence.SequenceType.ObjectTeleport));
        }
    }
}
