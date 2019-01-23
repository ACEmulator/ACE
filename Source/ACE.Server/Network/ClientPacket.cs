using System;
using System.IO;

using log4net;

namespace ACE.Server.Network
{
    public class ClientPacket : Packet
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        public static int MaxPacketSize { get; } = 1024;

        public BinaryReader Payload { get; private set; }
        public PacketHeaderOptional HeaderOptional { get; private set; }
        public bool IsValid { get; private set; } = false;

        public ClientPacket(byte[] data)
        {
#if NETDIAG
            data = NetworkSyntheticTesting.SyntheticCorruption_C2S(data);
#endif
            ParsePacketData(data);
            if (IsValid)
                ReadFragments();
        }

        private void ParsePacketData(byte[] data)
        {
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        Header = new PacketHeader(reader);
                        if (Header.Size > data.Length - reader.BaseStream.Position)
                        {
                            IsValid = false;
                            return;
                        }
                        Data = new MemoryStream(reader.ReadBytes(Header.Size), 0, Header.Size, false, true);
                        Payload = new BinaryReader(Data);
                        HeaderOptional = new PacketHeaderOptional(Payload, Header);
                        if (!HeaderOptional.IsValid)
                        {
                            IsValid = false;
                            return;
                        }
                    }
                }
                IsValid = true;
            }
            catch(Exception ex)
            {
                IsValid = false;
                log.Error("Invalid packet data", ex);
            }
        }

        private void ReadFragments()
        {
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
            {
                while (Payload.BaseStream.Position != Payload.BaseStream.Length)
                {
                    try
                    {
                        Fragments.Add(new ClientPacketFragment(Payload));
                    }
                    catch (Exception)
                    {
                        // corrupt packet
                        IsValid = false;
                        break;
                    }
                }
            }
        }

        public bool VerifyChecksum(uint issacXor)
        {
            uint fragmentChecksum = 0u;

            foreach (ClientPacketFragment fragment in Fragments)
                fragmentChecksum += fragment.CalculateHash32();

            uint payloadChecksum = HeaderOptional.CalculateHash32() + fragmentChecksum;

            uint finalChecksum = Header.CalculateHash32() + (payloadChecksum ^ issacXor);
            packetLog.DebugFormat("Checksum is calculated as {0} and is {1} in header", finalChecksum, Header.Checksum);
            return finalChecksum == Header.Checksum;
        }
    }
}
