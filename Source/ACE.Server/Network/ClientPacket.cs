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
        public bool IsValid { get; private set; } = false;
        public bool CRCVerified { get; private set; } = false;

        public ClientPacket(byte[] data)
        {
            data = NetworkSyntheticTesting.SyntheticCorruption_C2S(data);

            Unpack(data);

            if (IsValid)
                ReadFragments();
        }

        private void Unpack(byte[] data)
        {
            try
            {
                if (data.Length < PacketHeader.HeaderSize)
                {
                    IsValid = false;
                    return;
                }

                Header.Unpack(data);

                if (Header.Size > data.Length - PacketHeader.HeaderSize)
                {
                    IsValid = false;
                    return;
                }

                Data = new MemoryStream(data, PacketHeader.HeaderSize, Header.Size, false, true);
                DataReader = new BinaryReader(Data);
                HeaderOptional.Unpack(DataReader, Header);

                if (!HeaderOptional.IsValid)
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                packetLog.Error("Invalid packet data", ex);
            }
        }

        private void ReadFragments()
        {
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
            {
                while (DataReader.BaseStream.Position != DataReader.BaseStream.Length)
                {
                    try
                    {
                        var fragment = new ClientPacketFragment(DataReader); // TODO: Improve the ClientPacketFragment ctor to take a Span<byte>, or a byte[]

                        Fragments.Add(fragment);
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

        private bool VerifyChecksum(uint issacXor)
        {
            return headerChecksum + (payloadChecksum ^ issacXor) == Header.Checksum;
        }

        private bool VerifyEncryptedCRC(CryptoSystem fq, out string keyOffsetForLogging, bool rangeAdvance)
        {
            var verifiedKey = new Tuple<int, uint>(0, 0);

            Func<Tuple<int, uint>, bool> cbSearch = new Func<Tuple<int, uint>, bool>((pair) =>
            {
                if (VerifyChecksum(pair.Item2))
                {
                    verifiedKey = pair;
                    return true;
                }

                return false;
            });

            if (fq.Search(cbSearch, rangeAdvance))
            {
                keyOffsetForLogging = verifiedKey.Item1.ToString();
                return true;
            }

            keyOffsetForLogging = "???";
            return false;
        }

        private bool VerifyEncryptedCRCAndLogResult(CryptoSystem fq, bool rangeAdvance)
        {
            bool result = VerifyEncryptedCRC(fq, out string key, rangeAdvance);

            key = (key == "") ? "" : $" Key: {key}";
            packetLog.DebugFormat("{0} {1}{2}", fq, this, key);

            return result;
        }
        public bool VerifyCRC(CryptoSystem fq, bool rangeAdvance)
        {
            if (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                if (VerifyEncryptedCRCAndLogResult(fq, rangeAdvance))
                {
                    CRCVerified = true;
                    return true;
                }
            }
            else
            {
                if (Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
                {
                    // discard retransmission request with cleartext CRC
                    // client sends one encrypted version and one non encrypted version of each retransmission request
                    // honoring these causes client to drop because it's only expecting one of the two retransmission requests to be honored
                    // and it's more secure to only accept the trusted version
                    return false;
                }

                if (VerifyChecksum(0))
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
