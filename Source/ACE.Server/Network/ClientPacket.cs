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

        public bool Unpack(byte[] data)
        {
            try
            {
                if (data.Length < PacketHeader.HeaderSize)
                    return false;

                Header.Unpack(data);

                if (Header.Size > data.Length - PacketHeader.HeaderSize)
                    return false;

                Data = new MemoryStream(data, PacketHeader.HeaderSize, Header.Size, false, true);
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
                        fragment.Unpack(DataReader);

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

        private bool VerifyChecksum()
        {
            return headerChecksum + payloadChecksum == Header.Checksum;
        }

        private bool VerifyEncryptedCRC(CryptoSystem fq, out string keyOffsetForLogging)
        {
            var verifiedKey = new Tuple<int, uint>(0, 0);

            uint receivedKey = (Header.Checksum - headerChecksum) ^ payloadChecksum;

            Func<Tuple<int, uint>, bool> cbSearch = new Func<Tuple<int, uint>, bool>((pair) =>
            {
                if (receivedKey == pair.Item2)
                {
                    verifiedKey = pair;
                    return true;
                }

                return false;
            });

            if (fq.Search(cbSearch))
            {
                keyOffsetForLogging = verifiedKey.Item1.ToString();
                return true;
            }

            keyOffsetForLogging = "???";
            return false;
        }

        private bool VerifyEncryptedCRCAndLogResult(CryptoSystem fq)
        {
            bool result = VerifyEncryptedCRC(fq, out string key);

            key = (key == "") ? "" : $" Key: {key}";
            packetLog.DebugFormat("{0} {1}{2}", fq, this, key);

            return result;
        }
        public bool VerifyCRC(CryptoSystem fq)
        {
            if (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                if (VerifyEncryptedCRCAndLogResult(fq))
                    return true;
            }
            else
            {
                if (VerifyChecksum())
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
