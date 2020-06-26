using System;
using System.Collections.Generic;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipFullUpdate : GameEventMessage
    {
        public static FellowComparer FellowComparer = new FellowComparer();

        public GameEventFellowshipFullUpdate(Session session)
            : base(GameEventType.FellowshipFullUpdate, GameMessageGroup.UIQueue, session)
        {
            var fellowship = session.Player.Fellowship;

            #region PackableHashTable of fellowship table - <ObjectID,Fellow>
            // the current number of fellowship members
            Writer.Write((ushort)fellowship.FellowshipMembers.Count); //count - number of items in the table
            Writer.Write(FellowComparer.TableSize);    // static table size from retail pcaps

            // --- FellowInfo ---

            var fellowshipMembers = new SortedDictionary<uint, Player>(fellowship.GetFellowshipMembers(), FellowComparer);
            foreach (Player fellow in fellowshipMembers.Values)
            {
                // Write data associated with each fellowship member
                WriteFellow(fellow);
            }
            #endregion

            Writer.WriteString16L(fellowship.FellowshipName);
            Writer.Write(fellowship.FellowshipLeaderGuid);
            Writer.Write(Convert.ToUInt32(fellowship.ShareXP));
            Writer.Write(Convert.ToUInt32(fellowship.EvenShare));
            Writer.Write(Convert.ToUInt32(fellowship.Open));

            Writer.Write(Convert.ToUInt32(fellowship.IsLocked));

            Writer.Write(fellowship.DepartedMembers);

            Writer.Write(fellowship.FellowshipLocks);
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
            Writer.Write((uint)0x10); // TODO: shareLoot - if 0 then noSharePhatLoot, if 16(0x0010) then sharePhatLoot

            Writer.WriteString16L(fellow.Name);
        }
    }

    public class FellowComparer : IComparer<uint>
    {
        public static ushort TableSize = 16;

        public int Compare(uint a, uint b)
        {
            var keyA = a % TableSize;
            var keyB = b % TableSize;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }
}
