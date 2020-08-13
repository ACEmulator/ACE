using System;
using System.IO;

using log4net;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ClientPacket : Packet
    {
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        public static int MaxPacketSize { get; } = 1024;

        public BinaryReader DataReader { get; private set; }
        public PacketHeaderOptional HeaderOptional { get; } = new PacketHeaderOptional();

        /// <summary>
        /// If you pass in a shared buffer, be sure to ReleaseBuffer() after you've processed the packet, and before you use the buffer again for the next job.
        /// </summary>
        public bool Unpack(byte[] buffer, int bufferSize)
        {
            try
            {
                if (bufferSize < PacketHeader.HeaderSize)
                    return false;

                Header.Unpack(buffer);

                if (Header.Size > bufferSize - PacketHeader.HeaderSize)
                    return false;

                Data = new MemoryStream(buffer, PacketHeader.HeaderSize, Header.Size, false, true);
                DataReader = new BinaryReader(Data);
                HeaderOptional.Unpack(DataReader, Header);

                if (!HeaderOptional.IsValid)
                    return false;

                if (!ReadFragments())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                packetLog.Error("Invalid packet data", ex);

                return false;
            }
        }

        private bool ReadFragments()
        {
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
            {
                while (DataReader.BaseStream.Position != DataReader.BaseStream.Length)
                {
                    try
                    {
                        var fragment = new ClientPacketFragment();
                        if (!fragment.Unpack(DataReader)) return false;

                        Fragments.Add(fragment);
                    }
                    catch (Exception)
                    {
                        // corrupt packet
                        return false;
                    }
                }
            }

            return true;
        }

        public void ReleaseBuffer()
        {
            Data = null;
            DataReader = null;
        }

        private uint? _fragmentChecksum;
        private uint fragmentChecksum
        {
            get
            {
                if (_fragmentChecksum == null)
                {
                    uint fragmentChecksum = 0u;

                    foreach (ClientPacketFragment fragment in Fragments)
                        fragmentChecksum += fragment.CalculateHash32();

                    _fragmentChecksum = fragmentChecksum;
                }

                return _fragmentChecksum.Value;
            }
        }

        private uint? _headerChecksum;
        private uint headerChecksum
        {
            get
            {
                if (_headerChecksum == null)
                    _headerChecksum = Header.CalculateHash32();

                return _headerChecksum.Value;
            }
        }

        private uint? _headerOptionalChecksum;
        private uint headerOptionalChecksum
        {
            get
            {
                if (_headerOptionalChecksum == null)
                    _headerOptionalChecksum = HeaderOptional.CalculateHash32();

                return _headerOptionalChecksum.Value;
            }
        }

        private uint? _payloadChecksum;
        private uint payloadChecksum
        {
            get
            {
                if (_payloadChecksum == null)
                    _payloadChecksum = headerOptionalChecksum + fragmentChecksum;

                return _payloadChecksum.Value;
            }
        }
        public bool VerifyCRC(CryptoSystem fq)
        {
            if (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                var key = ((Header.Checksum - headerChecksum) ^ payloadChecksum);
                if (fq.Search(key))
                {
                    fq.ConsumeKey(key);
                    return true;
                }
            }
            else
            {
                if (headerChecksum + payloadChecksum == Header.Checksum)
                {
                    packetLog.DebugFormat("{0}", this);
                    return true;
                }

                packetLog.DebugFormat("{0}, Checksum Failed", this);
            }

            NetworkStatistics.C2S_CRCErrors_Aggregate_Increment();

            return false;
        }

        public override string ToString()
        {
            return $"<<< {Header} {HeaderOptional}".TrimEnd();
        }
    }
}
