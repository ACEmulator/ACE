using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using ACE.Server.Network;
using ACE.Server.Network.GameMessages;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Network
{
    [TestClass]
    public class MessageFragmentTests
    {
        [TestMethod]
        [DataRow(PacketFragment.MaxFragmentDataSize)]
        [DataRow(PacketFragment.MaxFragmentDataSize * 2)]
        public void TailSize_ExactMultipleOfMaxFragmentDataSize_UsesFullFragment(
            int payloadLength)
        {
            var message = new TestGameMessage(payloadLength);

            Type messageFragmentType =
                typeof(ServerPacketFragment).Assembly.GetType(
                    "ACE.Server.Network.MessageFragment");

            Assert.IsNotNull(messageFragmentType);

            object fragment = CreateMessageFragment(
                messageFragmentType,
                message,
                1);

            Assert.IsNotNull(fragment);

            PropertyInfo dataLengthProperty =
                messageFragmentType.GetProperty("DataLength");

            PropertyInfo tailSizeProperty =
                messageFragmentType.GetProperty("TailSize");

            Assert.IsNotNull(dataLengthProperty);
            Assert.IsNotNull(tailSizeProperty);

            int dataLength =
                (int)dataLengthProperty.GetValue(fragment);

            int tailSize =
                (int)tailSizeProperty.GetValue(fragment);

            Assert.AreEqual(
                payloadLength,
                dataLength);

            Assert.AreEqual(
                PacketFragmentHeader.HeaderSize
                    + PacketFragment.MaxFragmentDataSize,
                tailSize);
        }

        [TestMethod]
        public async Task SharedGameMessage_ConcurrentFragmentation_UsesFrozenData()
        {
            const int workerCount = 8;

            int payloadLength =
                (PacketFragment.MaxFragmentDataSize * 32) + 137;

            byte[] expectedPayload = new byte[payloadLength];

            new Random(12345).NextBytes(expectedPayload);

            var message =
                new TestGameMessage(expectedPayload);

            Type messageFragmentType =
                typeof(ServerPacketFragment).Assembly.GetType(
                    "ACE.Server.Network.MessageFragment");

            Assert.IsNotNull(messageFragmentType);

            Task<object>[] createTasks =
                new Task<object>[workerCount];

            for (int i = 0; i < workerCount; i++)
            {
                int worker = i;

                createTasks[i] = Task.Run(() =>
                    CreateMessageFragment(
                        messageFragmentType,
                        message,
                        (uint)(worker + 1)));
            }

            object[] fragments =
                await Task.WhenAll(createTasks);

            foreach (object fragment in fragments)
                Assert.IsNotNull(fragment);

            // Every MessageFragment should now own a read-only view of the
            // frozen snapshot. Fragmentation must no longer depend on this
            // MemoryStream or its shared Position.
            message.Data.Dispose();

            Task<byte[]>[] fragmentTasks =
                new Task<byte[]>[workerCount];

            for (int i = 0; i < workerCount; i++)
            {
                object fragment = fragments[i];

                fragmentTasks[i] = Task.Run(() =>
                    ReconstructPayload(
                        messageFragmentType,
                        fragment));
            }

            byte[][] reconstructedPayloads =
                await Task.WhenAll(fragmentTasks);

            foreach (byte[] reconstructedPayload in reconstructedPayloads)
            {
                CollectionAssert.AreEqual(
                    expectedPayload,
                    reconstructedPayload);
            }
        }

        private static object CreateMessageFragment(
            Type messageFragmentType,
            GameMessage message,
            uint sequence)
        {
            return Activator.CreateInstance(
                messageFragmentType,
                BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic,
                binder: null,
                args: new object[] { message, sequence },
                culture: null);
        }

        private static byte[] ReconstructPayload(
            Type messageFragmentType,
            object fragment)
        {
            MethodInfo getNextFragmentMethod =
                messageFragmentType.GetMethod(
                    "GetNextFragment");

            PropertyInfo dataRemainingProperty =
                messageFragmentType.GetProperty(
                    "DataRemaining");

            Assert.IsNotNull(getNextFragmentMethod);
            Assert.IsNotNull(dataRemainingProperty);

            using var reconstructed =
                new MemoryStream();

            while ((int)dataRemainingProperty.GetValue(fragment) > 0)
            {
                var serverFragment =
                    (ServerPacketFragment)
                    getNextFragmentMethod.Invoke(
                        fragment,
                        null);

                Assert.IsNotNull(serverFragment);

                byte[] packedFragment =
                    new byte[serverFragment.Length];

                int offset = 0;

                serverFragment.PackAndReturnHash32(
                    packedFragment,
                    ref offset);

                Assert.AreEqual(
                    serverFragment.Length,
                    offset);

                reconstructed.Write(
                    packedFragment,
                    PacketFragmentHeader.HeaderSize,
                    serverFragment.Length
                        - PacketFragmentHeader.HeaderSize);
            }

            return reconstructed.ToArray();
        }

        private class TestGameMessage : GameMessage
        {
            public TestGameMessage(int payloadLength)
                : this(new byte[payloadLength])
            {
            }

            public TestGameMessage(byte[] payload)
                : base(
                    GameMessageOpcode.None,
                    GameMessageGroup.UIQueue)
            {
                Writer.Write(payload);
            }
        }
    }
}
