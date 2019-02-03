using System;
using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;

namespace ACE.Server.Network.Structure
{
    public class SquelchInfo
    {
        public List<ChatMessageType> Filters;
        public string PlayerName;
        public bool Account;

        public SquelchInfo()
        {
            Filters = new List<ChatMessageType>();
        }

        public SquelchInfo(ChatMessageType filter, string playerName, bool account)
        {
            Filters = new List<ChatMessageType>() { filter };
            PlayerName = playerName;
            Account = account;
        }

        public SquelchInfo(List<ChatMessageType> filters, string playerName, bool account)
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

        public static void Write(this BinaryWriter writer, List<ChatMessageType> filters)
        {
            writer.Write(filters.Count);
            foreach (var filter in filters)
                writer.Write((uint)filter);
        }
    }
}
