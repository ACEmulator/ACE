using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using log4net;

namespace ACE.Server.Network
{
    /// <summary>
    /// Simple Proxy Protocol (SPP) header, prepended to every UDP datagram by a trusted proxy to identify the real client.<para />
    /// Reference: https://developers.cloudflare.com/spectrum/reference/simple-proxy-protocol-header/
    /// </summary>
    public class SimpleProxyProtocolHeader
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int HeaderSize = 38;

        public const ushort Magic = 0x56EC;

        public IPAddress ClientAddress { get; }
        public IPAddress ProxyAddress { get; }
        public ushort ClientPort { get; }
        public ushort ProxyPort { get; }

        public SimpleProxyProtocolHeader(IPAddress clientAddress, ushort clientPort, IPAddress proxyAddress, ushort proxyPort)
        {
            ClientAddress = clientAddress;
            ClientPort = clientPort;
            ProxyAddress = proxyAddress;
            ProxyPort = proxyPort;
        }

        /// <summary>
        /// Reads a header from the start of buffer. Addresses are network byte order IPv6, IPv4 clients are encoded as IPv4-mapped IPv6 and are returned as IPv4.
        /// </summary>
        public static bool TryUnpack(ReadOnlySpan<byte> buffer, out SimpleProxyProtocolHeader header)
        {
            header = null;

            if (buffer.Length < HeaderSize || BinaryPrimitives.ReadUInt16BigEndian(buffer) != Magic)
                return false;

            var clientAddress = UnpackAddress(buffer.Slice(2, 16));
            var proxyAddress = UnpackAddress(buffer.Slice(18, 16));
            var clientPort = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(34));
            var proxyPort = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(36));

            header = new SimpleProxyProtocolHeader(clientAddress, clientPort, proxyAddress, proxyPort);

            return true;
        }

        public void Pack(Span<byte> buffer)
        {
            BinaryPrimitives.WriteUInt16BigEndian(buffer, Magic);
            PackAddress(ClientAddress, buffer.Slice(2, 16));
            PackAddress(ProxyAddress, buffer.Slice(18, 16));
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(34), ClientPort);
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(36), ProxyPort);
        }

        private static IPAddress UnpackAddress(ReadOnlySpan<byte> buffer)
        {
            var address = new IPAddress(buffer);

            return address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address;
        }

        private static void PackAddress(IPAddress address, Span<byte> buffer)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
                address = address.MapToIPv6();

            address.TryWriteBytes(buffer, out _);
        }

        /// <summary>
        /// Decides what to do with a received datagram.<para />
        /// Datagrams from a trusted proxy must start with a valid header, which is returned in header. The packet starts after the header.<para />
        /// Datagrams from any other address are left untouched and header is null, even if they start with the magic value.
        /// </summary>
        /// <returns>false if the datagram must be dropped</returns>
        public static bool TryUnwrap(byte[] buffer, int dataSize, IPAddress source, HashSet<IPAddress> trustedProxyAddresses, out SimpleProxyProtocolHeader header)
        {
            header = null;

            if (trustedProxyAddresses.Count == 0 || !trustedProxyAddresses.Contains(source))
                return dataSize <= ClientPacket.MaxPacketSize;

            if (!TryUnpack(buffer.AsSpan(0, dataSize), out header))
                return false;

            return dataSize - HeaderSize <= ClientPacket.MaxPacketSize;
        }

        /// <summary>
        /// Parses the TrustedProxyAddresses setting. Invalid entries are logged and ignored.
        /// </summary>
        public static HashSet<IPAddress> ParseTrustedProxyAddresses(IEnumerable<string> addresses)
        {
            var results = new HashSet<IPAddress>();

            if (addresses == null)
                return results;

            foreach (var entry in addresses)
            {
                if (IPAddress.TryParse(entry, out var address))
                    results.Add(address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address);
                else
                    log.ErrorFormat("Invalid TrustedProxyAddresses entry \"{0}\" ignored.", entry);
            }

            return results;
        }

        public override string ToString()
        {
            return $"{ClientAddress}:{ClientPort} via proxy {ProxyAddress}:{ProxyPort}";
        }
    }
}
