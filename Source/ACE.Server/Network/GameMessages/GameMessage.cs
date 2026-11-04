using System;
using System.Threading;

namespace ACE.Server.Network.GameMessages
{
    public abstract class GameMessage
    {
        public GameMessageOpcode Opcode { get; private set; }

        public GameMessageGroup Group { get; private set; }

        public System.IO.MemoryStream Data { get; private set; }

        protected System.IO.BinaryWriter Writer { get; private set; }
        // Once a GameMessage enters the outbound network path, its serialized
        // representation is frozen here. Multiple sessions can safely share
        // this snapshot because the network path only exposes it as read-only.
        private byte[] frozenData;

        internal ReadOnlyMemory<byte> GetFrozenData()
        {
            var snapshot = Volatile.Read(ref frozenData);

            if (snapshot != null)
                return snapshot;

            // Writer already exists for every GameMessage, so use it as the
            // synchronization object instead of allocating another lock object.
            lock (Writer)
            {
                snapshot = frozenData;

                if (snapshot == null)
                {
                    Writer.Flush();

                    // ToArray() does NOT change MemoryStream.Position.
                    // This is the only payload copy made for this GameMessage,
                    // regardless of how many sessions receive it.
                    snapshot = Data.ToArray();

                    Volatile.Write(ref frozenData, snapshot);
                }
            }

            return snapshot;
        }

        protected GameMessage(GameMessageOpcode opCode, GameMessageGroup group)
        {
            Opcode = opCode;

            Group = group;

            Data = new System.IO.MemoryStream();

            Writer = new System.IO.BinaryWriter(Data);

            if (Opcode != GameMessageOpcode.None)
                Writer.Write((uint)Opcode);
        }

        /// <param name="dataInitialCapacity">
        /// This is an optimization to help us seed the Data MemoryStream with an initial capacity.<para />
        /// MemoryStream starts off as 0 capacity, then the first use it initailizes an array of 256 bytes, then doubles each type capacity is reached.<para />
        /// By using the out of the box method for all, we can be over allocating for some opcodes, and re-allocating (via array doubling) for others<para />
        /// We're only helping Data with an initial capacity. If the MemoryStream needs more, it will still double itself and work as intended.
        /// </param>
        protected GameMessage(GameMessageOpcode opCode, GameMessageGroup group, int dataInitialCapacity)
        {
            Opcode = opCode;

            Group = group;

            Data = new System.IO.MemoryStream(dataInitialCapacity);

            Writer = new System.IO.BinaryWriter(Data);

            if (Opcode != GameMessageOpcode.None)
                Writer.Write((uint)Opcode);
        }
    }
}
