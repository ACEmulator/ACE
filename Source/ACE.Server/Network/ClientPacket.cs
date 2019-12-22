using ACE.Common.Cryptography;
using log4net;
using System;
using System.IO;

namespace ACE.Server.Network
{
    public class ClientPacket : Packet
    {
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        public static int MaxPacketSize { get; } = 1024;

        public BinaryReader Payload { get; private set; }
        public PacketHeaderOptional HeaderOptional { get; private set; }
        public bool IsValid { get; private set; } = false;
        public bool CRCVerified { get; private set; } = false;

        public ClientPacket(byte[] data)
        {
            data = NetworkSyntheticTesting.SyntheticCorruption_C2S(data);

            ParsePacketData(data);
            if (IsValid)
            {
                ReadFragments();
            }
        }

        private void ParsePacketData(byte[] data)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
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

        private uint? _fragmentChecksum;
        private uint fragmentChecksum
        {
            get
            {
                if (_fragmentChecksum == null)
                {
                    uint fragmentChecksum = 0u;
                    foreach (ClientPacketFragment fragment in Fragments)
                    {
                        fragmentChecksum += fragment.CalculateHash32();
                    }
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
                {
                    _headerChecksum = Header.CalculateHash32();
                }
                return _headerChecksum.Value;
            }
        }
        private uint? _headerOptionalChecksum;
        private uint headerOptionalChecksum
        {
            get
            {
                if (_headerOptionalChecksum == null)
                {
                    _headerOptionalChecksum = HeaderOptional.CalculateHash32();
                }
                return _headerOptionalChecksum.Value;
            }
        }
        private uint? _payloadChecksum;
        private uint payloadChecksum
        {
            get
            {
                if (_payloadChecksum == null)
                {
                    _payloadChecksum = headerOptionalChecksum + fragmentChecksum;
                }
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
                else
                {
                    return false;
                }
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
            key = (key == "") ? $"" : $" Key: {key}";
            packetLog.Debug($"{fq} {this}{key}");
            return result;
        }
        public bool VerifyCRC(CryptoSystem fq)
        {
            if (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                if (VerifyEncryptedCRCAndLogResult(fq))
                {
                    CRCVerified = true;
                    return true;
                }
            }
            else
            {
                if (VerifyChecksum())
                {
                    packetLog.Debug($"{this}");
                    return true;
                }
                else
                {
                    packetLog.Debug($"{this}, Checksum Failed");
                }
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
