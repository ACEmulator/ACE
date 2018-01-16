using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common.Extensions;
using ACE.Entity.Actions;
using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageFellowshipFullUpdate : GameMessage
    {
        public GameMessageFellowshipFullUpdate(Session session)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            // This is a naive, bare-bones implementation of 0x02BE, FullFellowshipUpdate.
            // 0x02BE is fairly complicated, so the following code is at least valuable as an example of a valid server response.

            // todo: The current implementation has race conditions,
            // and there are questions that must be answered before it can be fixed.
            // We need to figure out who "owns" the fellowship data.
            // Does everyone get a turn to read from and modify the fellowship data, and if so, how is this managed?

            // Currently, creating and leaving a fellowship is supported.
            // Any other fellowship function is not yet supported.

            Fellowship fellowship = session.Player.Fellowship;

            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipFullUpdate);

            // the current number of fellowship members
            Writer.Write((UInt16)fellowship.FellowshipMembers.Count);

            // todo: figure out what these two bytes are for
            Writer.Write((byte)0x10);
            Writer.Write((byte)0x00);

            // --- FellowInfo ---

            ActionChain fellowChain = new ActionChain();
            foreach (Player fellow in fellowship.FellowshipMembers)
            {
                // Write data associated with each fellowship member
                WriteFellow(fellow);
            }

            Writer.WriteString16L(fellowship.FellowshipName);
            Writer.Write(fellowship.FellowshipLeaderGuid);
            Writer.Write(Convert.ToUInt32(fellowship.ShareXP));
            Writer.Write(Convert.ToUInt32(fellowship.EvenShare));
            Writer.Write(Convert.ToUInt32(fellowship.Open));

            Writer.Write(0u);

            // End of meaningful data?
            Writer.Write((uint)0x00200000);
            Writer.Write((uint)0x00200000);
        }

        public void WriteFellow(Player fellow)
        {
            Writer.Write(fellow.Guid.Full);

            Writer.Write(0u);
            Writer.Write(0u);

            Writer.Write(fellow.Level);

            Writer.Write(fellow.Health.MaxValue);
            Writer.Write(fellow.Stamina.MaxValue);
            Writer.Write(fellow.Mana.MaxValue);

            Writer.Write(fellow.Health.Current);
            Writer.Write(fellow.Stamina.Current);
            Writer.Write(fellow.Mana.Current);

            // todo: share loot with this fellow?
            Writer.Write((uint)0x1);

            Writer.WriteString16L(fellow.Name);
        }
    }
}
