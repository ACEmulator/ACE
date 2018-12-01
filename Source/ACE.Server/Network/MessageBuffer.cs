using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.Server.Network
{
    internal class MessageBuffer
    {
        private List<ClientPacketFragment> fragments = new List<ClientPacketFragment>();

        public uint Sequence { get; }
        public int Count => fragments.Count;
        public uint TotalFragments { get; }
        public bool Complete => fragments.Count == TotalFragments;

        public MessageBuffer(uint sequence, uint totalFragments)
        {
            Sequence = sequence;
            TotalFragments = totalFragments;
        }

        public void AddFragment(ClientPacketFragment fragment)
        {
            lock (fragments)
            {
                if (!Complete && fragments.All(x => x.Header.Index != fragment.Header.Index))
                {
                    fragments.Add(fragment);
                }
            }
        }

        public ClientMessage GetMessage()
        {
            fragments.Sort(delegate (ClientPacketFragment x, ClientPacketFragment y) { return x.Header.Index - y.Header.Index; });
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (ClientPacketFragment fragment in fragments)
            {
                writer.Write(fragment.Data);
            }
            stream.Seek(0, SeekOrigin.Begin);
            return new ClientMessage(stream);
        }
    }
}
