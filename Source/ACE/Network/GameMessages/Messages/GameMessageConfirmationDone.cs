using ACE.Entity;
using ACE.Network.Enum;
using ACE.Network.GameEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageConfirmationDone : GameMessage
    {
        public GameMessageConfirmationDone(Player player, ConfirmationType confirmationType, uint contextId)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(player.Guid.Full);
            Writer.Write(player.Session.GameEventSequence++);
            Writer.Write((uint)GameEventType.CharacterConfirmationDone);
            Writer.Write((uint)confirmationType);
            Writer.Write(contextId);
        }
    }
}
