using System;
using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;

namespace ACE.Server.Network.Structure
{
    public class SquelchInfo
    {
        public List<SquelchMask> Filters;
        public string PlayerName;
        public bool Account;

        public SquelchInfo()
        {
            Filters = new List<SquelchMask>();
        }

        public SquelchInfo(SquelchMask filter, string playerName, bool account)
        {
            // not sure why this is sent 4x..
            // if not sent 4x, then the 'checkbox' in the chat menu doesn't toggle

            // there doesn't appear to be any pcaps of players performing per-channel character squelches,
            // so not sure how those were handled for this packet.

            Filters = new List<SquelchMask>() { filter, filter, filter, filter };
            PlayerName = playerName;
            Account = account;
        }

        public SquelchInfo(List<SquelchMask> filters, string playerName, bool account)
        {
            Filters = filters;
            PlayerName = playerName;
            Account = account;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public SquelchInfo(SquelchInfo info)
        {
            Filters = info.Filters;
            PlayerName = info.PlayerName;
            Account = info.Account;
        }
    }

    public static class SquelchInfoExtensions
    {
        public static void Write(this BinaryWriter writer, SquelchInfo info)
        {
            writer.Write(info.Filters);
            writer.WriteString16L(info.PlayerName);
            writer.Write(Convert.ToUInt32(info.Account));
        }

        public static void Write(this BinaryWriter writer, List<SquelchMask> filters)
        {
            writer.Write(filters.Count);
            foreach (var filter in filters)
                writer.Write((uint)filter);
        }
    }
}
