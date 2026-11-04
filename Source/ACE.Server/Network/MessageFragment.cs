using System;

using ACE.Server.Network.GameMessages;

using log4net;

namespace ACE.Server.Network
{
    internal class MessageFragment
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        private readonly ReadOnlyMemory<byte> data;

        public GameMessage Message { get; private set; }

        public uint Sequence { get; set; }

        public ushort Index { get; set; }

        public ushort Count { get; set; }

        public int DataLength => data.Length;

        public int DataRemaining { get; private set; }

        public int NextSize
        {
            get
            {
                var dataSize = DataRemaining;
                if (dataSize > PacketFragment.MaxFragmentDataSize)
                    dataSize = PacketFragment.MaxFragmentDataSize;
                return PacketFragmentHeader.HeaderSize + dataSize;
            }
        }

        public int TailSize 
        {
            get
            {
                var tailDataSize = DataLength % PacketFragment.MaxFragmentDataSize;

                if (DataLength > 0 && tailDataSize == 0)
                    tailDataSize = PacketFragment.MaxFragmentDataSize;

                return PacketFragmentHeader.HeaderSize + tailDataSize;
            }
        }

        public bool TailSent { get; private set; }

        public MessageFragment(GameMessage message, uint sequence)
        {
            Message = message;
            data = message.GetFrozenData();
            DataRemaining = DataLength;
            Sequence = sequence;
            Count = (ushort)(Math.Ceiling((double)DataLength / PacketFragment.MaxFragmentDataSize));
            Index = 0;
            if (Count == 1)
                TailSent = true;
            if (packetLog.IsDebugEnabled)
                packetLog.DebugFormat("Sequence {0}, count {1}, DataRemaining {2}", sequence, Count, DataRemaining);
        }

        public ServerPacketFragment GetTailFragment()
        {
            var index = (ushort)(Count - 1);
            TailSent = true;
            return CreateServerFragment(index);
        }

        public ServerPacketFragment GetNextFragment()
        {
            return CreateServerFragment(Index++);
        }

        private ServerPacketFragment CreateServerFragment(ushort index)
        {
            if (packetLog.IsDebugEnabled)
                packetLog.DebugFormat("Creating ServerFragment for index {0}", index);
            if (index >= Count)
                throw new ArgumentOutOfRangeException("index", index, "Passed index is greater then computed count");

            var position = index * PacketFragment.MaxFragmentDataSize;
            if (position > DataLength)
                throw new ArgumentOutOfRangeException("index", index, "Passed index computes to invalid position size");

            if (DataRemaining <= 0)
                throw new InvalidOperationException("There is no data remaining");

            var dataToSend = DataLength - position;
            if (dataToSend > PacketFragment.MaxFragmentDataSize)
                dataToSend = PacketFragment.MaxFragmentDataSize;

            if (DataRemaining < dataToSend)
                throw new InvalidOperationException("More data to send then data remaining!");

            // Create a read-only view into the frozen message data.
            // No shared MemoryStream cursor and no per-fragment byte[] allocation.
            var fragmentData = data.Slice(position, dataToSend);

            // Build ServerPacketFragment structure
            ServerPacketFragment fragment = new ServerPacketFragment(fragmentData);
            fragment.Header.Sequence = Sequence;
            fragment.Header.Id = 0x80000000;
            fragment.Header.Count = Count;
            fragment.Header.Index = index;
            fragment.Header.Queue = (ushort)Message.Group;

            DataRemaining -= dataToSend;
            if (packetLog.IsDebugEnabled)
                packetLog.DebugFormat("Done creating ServerFragment for index {0}. After reading {1} DataRemaining {2}", index, dataToSend, DataRemaining);
            return fragment;
        }
    }
}
