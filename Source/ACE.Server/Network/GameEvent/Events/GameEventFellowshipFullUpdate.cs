using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipFullUpdate : GameEventMessage
    {
        public GameEventFellowshipFullUpdate(Session session)
            : base(GameEventType.FellowshipFullUpdate, GameMessageGroup.UIQueue, session)
        {
            // This is a naive, bare-bones implementation of 0x02BE, FullFellowshipUpdate.
            // 0x02BE is fairly complicated, so the following code is at least valuable as an example of a valid server response.

            // todo: The current implementation has race conditions,
            // and there are questions that must be answered before it can be fixed.
            // We need to figure out who "owns" the fellowship data.
            // Does everyone get a turn to read from and modify the fellowship data, and if so, how is this managed?

            // Currently, creating and leaving a fellowship is supported.
            // Any other fellowship function is not yet supported.

            var fellowship = session.Player.Fellowship;

            // the current number of fellowship members
            Writer.Write((ushort)fellowship.FellowshipMembers.Count);

            Writer.Write(64);

            // --- FellowInfo ---

            var fellowChain = new ActionChain();
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

            Writer.Write(Convert.ToUInt32(false)); //TODO: locked

            // TODO PackableHashTable of fellows departed - fellowsDeparted  -<ObjectID,int>
            Writer.Write((uint)0x00200000);
            Writer.Write((uint)0x00200000);
        }

        public void WriteFellow(Player fellow)
        {
            Writer.WriteGuid(fellow.Guid);

            Writer.Write(0u); // TODO: cpCached - Perhaps cp stored up before distribution?
            Writer.Write(0u); // TODO: lumCached - Perhaps lum stored up before distribution?

            Writer.Write(fellow.Level ?? 1);

            Writer.Write(fellow.Health.MaxValue);
            Writer.Write(fellow.Stamina.MaxValue);
            Writer.Write(fellow.Mana.MaxValue);

            Writer.Write(fellow.Health.Current);
            Writer.Write(fellow.Stamina.Current);
            Writer.Write(fellow.Mana.Current);

            // todo: share loot with this fellow?
            Writer.Write((uint)0x1); // TODO: shareLoot - if 0 then noSharePhatLoot, if 16(0x0010) then sharePhatLoot

            Writer.WriteString16L(fellow.Name);
        }
    }
}
