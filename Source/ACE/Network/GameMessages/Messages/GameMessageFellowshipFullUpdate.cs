using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common.Extensions;
using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageFellowshipFullUpdate : GameMessage
    {
        public GameMessageFellowshipFullUpdate(Session session, Fellowship fellowship)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipFullUpdate);

            // the current number of fellowship members
            Writer.Write((UInt16)fellowship.FellowshipMembers.Count);

            // ????
            Writer.Write((byte)0x10);
            Writer.Write((byte)0x00);

            // --- FellowInfo ---
            foreach (Player fellow in fellowship.FellowshipMembers)
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

                // todo: share loot?
                Writer.Write((byte)0x01);
                Writer.Write((byte)0x00);
                Writer.Write((byte)0x00);
                Writer.Write((byte)0x00);

                Writer.WriteString16L(session.Player.Name);
            }

            Writer.WriteString16L(fellowship.FellowshipName);
            
            // guid of fellowship leader
            Writer.Write(fellowship.FellowshipLeaderGuid);

            // todo: xp share?
            Writer.Write((byte)0x01);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);

            // todo: open fellow?
            Writer.Write((byte)0x01);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);

            // todo: fellows departed?
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);

            Writer.Write(0u);

            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x20);
            Writer.Write((byte)0x00);

            Writer.Write((byte)0x00);
            Writer.Write((byte)0x00);
            Writer.Write((byte)0x20);
            Writer.Write((byte)0x00);
        }
    }
}
