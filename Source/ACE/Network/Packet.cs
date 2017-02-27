using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using ACE.Common.Cryptography;
using ACE.Common.Extensions;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network
{
    public abstract class Packet
    {
        public static uint MaxPacketSize { get; } = 484u;

        public static uint MaxPacketDataSize { get; } = 464u;

        public PacketHeader Header { get; protected set; }
        public MemoryStream Data { get; protected set; }
        public PacketDirection Direction { get; protected set; } = PacketDirection.None;
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();
    }
}
