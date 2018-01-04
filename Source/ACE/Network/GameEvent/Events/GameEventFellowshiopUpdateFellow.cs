using ACE.Entity;
using ACE.Network.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventFellowshiopUpdateFellow : GameMessage
    {
        public GameEventFellowshiopUpdateFellow(Session session, Player player)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipUpdateFellow);
            

            // Fellow information
            Writer.Write(player.Guid.Full);

            Writer.Write(0u);
            Writer.Write(0u);

            Writer.Write(player.Level);

            Writer.Write(player.Health.MaxValue);
            Writer.Write(player.Stamina.MaxValue);
            Writer.Write(player.Mana.MaxValue);

            Writer.Write(player.Health.Current);
            Writer.Write(player.Stamina.Current);
            Writer.Write(player.Mana.Current);

            // todo: share loot with this fellow?
            Writer.Write((uint)0x1);

            Writer.WriteString16L(player.Name);

            Writer.Write(1u);

        }
    }
}
