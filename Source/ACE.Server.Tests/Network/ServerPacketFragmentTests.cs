using System;

using ACE.Common.Cryptography;
using ACE.Server.Network;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Network
{
    [TestClass]
    public class ServerPacketFragmentTests
    {
        [TestMethod]
        public void PackAndReturnHash32_MaxPayload_PacksExpectedBytesAndHash()
        {
            byte[] payload = new byte[PacketFragment.MaxFragmentDataSize];

            for (int i = 0; i < payload.Length; i++)
                payload[i] = (byte)(i % 256);

            var fragment = new ServerPacketFragment(
                new ReadOnlyMemory<byte>(payload));

            fragment.Header.Sequence = 1234;
            fragment.Header.Id = 0x80000000;
            fragment.Header.Count = 1;
            fragment.Header.Index = 0;
            fragment.Header.Queue = 1;

            byte[] buffer = new byte[fragment.Length];
            int offset = 0;

            uint actualHash =
                fragment.PackAndReturnHash32(buffer, ref offset);

            Assert.AreEqual(fragment.Length, offset);

            Assert.AreEqual(
                (ushort)fragment.Length,
                fragment.Header.Size);

            byte[] packedPayload = buffer.AsSpan(
                PacketFragmentHeader.HeaderSize,
                payload.Length).ToArray();

            CollectionAssert.AreEqual(
                payload,
                packedPayload);

            uint expectedHash =
                Hash32.Calculate(
                    buffer,
                    0,
                    PacketFragmentHeader.HeaderSize)
                + Hash32.Calculate(
                    payload,
                    payload.Length);

            Assert.AreEqual(
                expectedHash,
                actualHash);
        }
    }
}
