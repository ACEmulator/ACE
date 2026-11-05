using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.Network;

namespace ACE.Server.Tests
{
    [TestClass]
    public class SimpleProxyProtocolTests
    {
        private static readonly IPAddress proxyAddress = IPAddress.Parse("198.51.100.7");

        private static readonly HashSet<IPAddress> trustedProxyAddresses = new HashSet<IPAddress> { proxyAddress };

        // Cloudflare's example layout: magic, client address, proxy address, client port, proxy port, all network byte order
        private static byte[] BuildHeader(string clientAddress, ushort clientPort, string proxy, ushort proxyPort)
        {
            var bytes = new byte[SimpleProxyProtocolHeader.HeaderSize];

            bytes[0] = 0x56;
            bytes[1] = 0xEC;
            IPAddress.Parse(clientAddress).GetAddressBytes().CopyTo(bytes, 2);
            IPAddress.Parse(proxy).GetAddressBytes().CopyTo(bytes, 18);
            bytes[34] = (byte)(clientPort >> 8);
            bytes[35] = (byte)clientPort;
            bytes[36] = (byte)(proxyPort >> 8);
            bytes[37] = (byte)proxyPort;

            return bytes;
        }

        private static byte[] BuildServerPacket(int start)
        {
            var packet = new ServerPacket();
            packet.Header.Sequence = 0xEC56;
            packet.Header.Flags = PacketHeaderFlags.AckSequence;
            packet.Header.Id = 3;
            packet.InitializeDataWriter();
            packet.DataWriter.Write(1234u);

            var buffer = new byte[start + 64];
            packet.CreateReadyToSendPacket(buffer, out var size, start);

            var result = new byte[size];
            Buffer.BlockCopy(buffer, start, result, 0, size);
            return result;
        }

        [TestMethod]
        public void HeaderRoundTrip_IPv4MappedClient()
        {
            var bytes = BuildHeader("::ffff:203.0.113.5", 51234, "::ffff:198.51.100.7", 9000);

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnpack(bytes, out var header));

            Assert.AreEqual(IPAddress.Parse("203.0.113.5"), header.ClientAddress);
            Assert.AreEqual(AddressFamily.InterNetwork, header.ClientAddress.AddressFamily);
            Assert.AreEqual("203.0.113.5", header.ClientAddress.ToString());
            Assert.AreEqual(proxyAddress, header.ProxyAddress);
            Assert.AreEqual(51234, header.ClientPort);
            Assert.AreEqual(9000, header.ProxyPort);

            var packed = new byte[SimpleProxyProtocolHeader.HeaderSize];
            header.Pack(packed);
            CollectionAssert.AreEqual(bytes, packed);
        }

        [TestMethod]
        public void HeaderRoundTrip_IPv6Client()
        {
            var bytes = BuildHeader("2001:db8::1", 40000, "2001:db8::2", 9001);

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnpack(bytes, out var header));

            Assert.AreEqual(IPAddress.Parse("2001:db8::1"), header.ClientAddress);
            Assert.AreEqual(AddressFamily.InterNetworkV6, header.ClientAddress.AddressFamily);
            Assert.AreEqual(IPAddress.Parse("2001:db8::2"), header.ProxyAddress);
            Assert.AreEqual(40000, header.ClientPort);
            Assert.AreEqual(9001, header.ProxyPort);

            var packed = new byte[SimpleProxyProtocolHeader.HeaderSize];
            header.Pack(packed);
            CollectionAssert.AreEqual(bytes, packed);
        }

        [TestMethod]
        public void HeaderPack_IPv4AddressesAreWrittenIPv4Mapped()
        {
            var header = new SimpleProxyProtocolHeader(IPAddress.Parse("203.0.113.5"), 51234, proxyAddress, 9000);

            var packed = new byte[SimpleProxyProtocolHeader.HeaderSize];
            header.Pack(packed);

            CollectionAssert.AreEqual(BuildHeader("::ffff:203.0.113.5", 51234, "::ffff:198.51.100.7", 9000), packed);
        }

        [TestMethod]
        public void TryUnpack_RejectsTooShortAndBadMagic()
        {
            var bytes = BuildHeader("::ffff:203.0.113.5", 51234, "::ffff:198.51.100.7", 9000);

            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnpack(bytes.AsSpan(0, SimpleProxyProtocolHeader.HeaderSize - 1), out var header));
            Assert.IsNull(header);

            bytes[1] = 0xED;
            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnpack(bytes, out header));
            Assert.IsNull(header);

            // byte order matters: 0xEC56 is not the magic
            bytes[0] = 0xEC;
            bytes[1] = 0x56;
            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnpack(bytes, out header));
        }

        [TestMethod]
        public void TryUnwrap_TrustedProxyWithHeader_ReturnsHeader()
        {
            var datagram = new byte[ClientPacket.MaxPacketSize + SimpleProxyProtocolHeader.HeaderSize];
            BuildHeader("::ffff:203.0.113.5", 51234, "::ffff:198.51.100.7", 9000).CopyTo(datagram, 0);

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnwrap(datagram, SimpleProxyProtocolHeader.HeaderSize + 20, proxyAddress, trustedProxyAddresses, out var header));
            Assert.IsNotNull(header);
            Assert.AreEqual(IPAddress.Parse("203.0.113.5"), header.ClientAddress);

            // a full size packet behind the header is accepted, one byte more is not
            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, proxyAddress, trustedProxyAddresses, out header));
            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnwrap(new byte[datagram.Length + 1], datagram.Length + 1, proxyAddress, trustedProxyAddresses, out header));
        }

        [TestMethod]
        public void TryUnwrap_TrustedProxyWithoutHeader_IsDropped()
        {
            // a normal client packet, sequence 1
            var datagram = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, proxyAddress, trustedProxyAddresses, out var header));
            Assert.IsNull(header);

            // the magic alone is not enough
            datagram[0] = 0x56;
            datagram[1] = 0xEC;
            Assert.IsFalse(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, proxyAddress, trustedProxyAddresses, out header));
        }

        [TestMethod]
        public void TryUnwrap_UntrustedSourceStartingWithMagic_IsUntouched()
        {
            // an ACE packet whose little-endian sequence number starts with 0x56 0xEC, or a spoofed header from a client
            var datagram = BuildHeader("::ffff:10.0.0.1", 1, "::ffff:198.51.100.7", 9000);
            var original = (byte[])datagram.Clone();

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, IPAddress.Parse("203.0.113.5"), trustedProxyAddresses, out var header));
            Assert.IsNull(header);
            CollectionAssert.AreEqual(original, datagram);

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, proxyAddress, new HashSet<IPAddress>(), out header));
            Assert.IsNull(header);
            CollectionAssert.AreEqual(original, datagram);
        }

        [TestMethod]
        public void ParseTrustedProxyAddresses_NormalizesAndSkipsInvalid()
        {
            var addresses = SimpleProxyProtocolHeader.ParseTrustedProxyAddresses(new[] { "198.51.100.7", "::ffff:192.0.2.1", "2001:db8::2", "not an address" });

            Assert.AreEqual(3, addresses.Count);
            Assert.IsTrue(addresses.Contains(proxyAddress));
            Assert.IsTrue(addresses.Contains(IPAddress.Parse("192.0.2.1")));
            Assert.IsTrue(addresses.Contains(IPAddress.Parse("2001:db8::2")));
            Assert.IsFalse(addresses.Contains(IPAddress.Parse("203.0.113.5")));

            Assert.AreEqual(0, SimpleProxyProtocolHeader.ParseTrustedProxyAddresses(null).Count);
        }

        [TestMethod]
        public void ServerPacket_WrittenAfterProxyHeader_IsUnchanged()
        {
            CollectionAssert.AreEqual(BuildServerPacket(0), BuildServerPacket(SimpleProxyProtocolHeader.HeaderSize));
        }

        [TestMethod]
        public void ClientPacket_UnpacksAfterProxyHeader()
        {
            var packet = BuildServerPacket(0);

            var datagram = new byte[SimpleProxyProtocolHeader.HeaderSize + packet.Length];
            BuildHeader("::ffff:203.0.113.5", 51234, "::ffff:198.51.100.7", 9000).CopyTo(datagram, 0);
            packet.CopyTo(datagram, SimpleProxyProtocolHeader.HeaderSize);

            Assert.IsTrue(SimpleProxyProtocolHeader.TryUnwrap(datagram, datagram.Length, proxyAddress, trustedProxyAddresses, out var header));

            var clientPacket = new ClientPacket();
            Assert.IsTrue(clientPacket.Unpack(datagram, datagram.Length - SimpleProxyProtocolHeader.HeaderSize, SimpleProxyProtocolHeader.HeaderSize));
            Assert.AreEqual(0xEC56u, clientPacket.Header.Sequence);
            Assert.AreEqual((ushort)3, clientPacket.Header.Id);
            Assert.AreEqual(1234u, clientPacket.HeaderOptional.AckSequence);
            Assert.IsTrue(clientPacket.VerifyCRC(null));

            var directPacket = new ClientPacket();
            Assert.IsTrue(directPacket.Unpack(packet, packet.Length));
            Assert.AreEqual(clientPacket.Header.Checksum, directPacket.Header.Checksum);
        }
    }
}
