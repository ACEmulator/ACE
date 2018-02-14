using ACE.Server.Entity.WorldObjects;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageConfirmationDone : GameMessage
    {
        public GameMessageConfirmationDone(Player player, ConfirmationType confirmationType, uint contextId)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.UIQueue)
        {
            Writer.Write(player.Guid.Full);
            Writer.Write(player.Session.GameEventSequence++);
            Writer.Write((uint)GameEventType.CharacterConfirmationDone);
            Writer.Write((uint)confirmationType);
            Writer.Write(contextId);
        }
    }
}
