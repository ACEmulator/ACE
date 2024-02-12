using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerTeleport : GameMessage
    {
        public GameMessagePlayerTeleport(Player player)
            : base(GameMessageOpcode.PrivateUpdateAttribute, GameMessageGroup.SmartboxQueue, 21)
        {
            Writer.Write(player.Sequences.GetNextSequence(Sequence.SequenceType.ObjectTeleport));
            Writer.Align();
        }
    }
}
