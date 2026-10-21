
using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using KeyMap = ACE.DatLoader.FileTypes.KeyMap;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class KeyMapTests
    {
        private static readonly Guid TestGuid =
            new Guid("451f8ccc-a7e9-4df4-9a6b-f4a7c706f300");

        // TEST 1: Decode a complete valid KeyMap.
        [TestMethod]
        public void KeyMap_ValidRecord_ParsesCompletely()
        {
            byte[] data = CreateValidKeyMap();

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var keyMap = new KeyMap();
            keyMap.Unpack(reader);

            Assert.AreEqual(0x14000000u, keyMap.Id);
            Assert.AreEqual("TestMap", keyMap.Name);
            Assert.AreEqual(TestGuid, keyMap.MapGuid);

            Assert.AreEqual(2, keyMap.Devices.Count);
            Assert.AreEqual((byte)0, keyMap.Devices[0].Type);
            Assert.AreEqual((byte)2, keyMap.Devices[1].Type);

            Assert.AreEqual(1, keyMap.MetaKeys.Count);
            Assert.AreEqual(0x002A0000u, keyMap.MetaKeys[0].Key);
            Assert.AreEqual(0x80000000u,
                keyMap.MetaKeys[0].Modifier);

            Assert.AreEqual(2, keyMap.InputMaps.Count);
            Assert.AreEqual(2, keyMap.InputMaps[10].Count);
            Assert.AreEqual(1, keyMap.InputMaps[20].Count);

            Assert.AreEqual(3,
                keyMap.InputMaps.Values.Sum(x => x.Count));

            var firstBinding = keyMap.InputMaps[10][0];

            Assert.AreEqual(0x001E0000u,
                firstBinding.Control.Key);
            Assert.AreEqual(0u,
                firstBinding.Control.Modifier);
            Assert.AreEqual(3u, firstBinding.Activation);
            Assert.AreEqual(0u, firstBinding.Unknown);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        // TEST 2: Reject truncated binary records.
        [TestMethod]
        public void KeyMap_TruncatedRecord_Throws()
        {
            byte[] data = CreateValidKeyMap();

            // Remove the final byte to simulate corruption.
            Array.Resize(ref data, data.Length - 1);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var keyMap = new KeyMap();

            // ValidateCount detects insufficient bytes before
            // BinaryReader attempts to read the incomplete record.
            Assert.ThrowsExactly<InvalidDataException>(
                () => keyMap.Unpack(reader));
        }

        // TEST 3: Reject impossible collection counts.
        [TestMethod]
        public void KeyMap_InvalidDeviceCount_Throws()
        {
            using var stream = new MemoryStream();

            using (var writer = new BinaryWriter(
                stream, System.Text.Encoding.UTF8, true))
            {
                writer.Write(0x14000000u);
                writer.Write("InvalidMap");
                writer.Write(TestGuid.ToByteArray());

                // This count cannot fit in the remaining data.
                writer.Write(uint.MaxValue);
            }

            stream.Position = 0;

            using var reader = new BinaryReader(stream);
            var keyMap = new KeyMap();

            Assert.ThrowsExactly<InvalidDataException>(
                () => keyMap.Unpack(reader));
        }

        // TEST 4: Reject duplicate input map identifiers.
        [TestMethod]
        public void KeyMap_DuplicateInputMap_Throws()
        {
            byte[] data = CreateValidKeyMap(
                duplicateMapIds: true);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var keyMap = new KeyMap();

            Assert.ThrowsExactly<InvalidDataException>(
                () => keyMap.Unpack(reader));
        }

        // TEST 5: Reusing the parser replaces previous data.
        [TestMethod]
        public void KeyMap_RepeatedUnpack_ReplacesCollections()
        {
            byte[] data = CreateValidKeyMap();
            var keyMap = new KeyMap();

            for (int i = 0; i < 2; i++)
            {
                using var stream = new MemoryStream(data);
                using var reader = new BinaryReader(stream);

                keyMap.Unpack(reader);

                Assert.AreEqual(2, keyMap.Devices.Count);
                Assert.AreEqual(1, keyMap.MetaKeys.Count);
                Assert.AreEqual(2, keyMap.InputMaps.Count);
                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        // Generate a valid KeyMap record in memory.
        // No client DAT files are required for these tests.
        private static byte[] CreateValidKeyMap(
            bool duplicateMapIds = false)
        {
            using var stream = new MemoryStream();

            using (var writer = new BinaryWriter(
                stream, System.Text.Encoding.UTF8, true))
            {
                // Header
                writer.Write(0x14000000u);
                writer.Write("TestMap");
                writer.Write(TestGuid.ToByteArray());

                // Two devices
                writer.Write(2u);

                writer.Write((byte)0);
                writer.Write(TestGuid.ToByteArray());

                writer.Write((byte)2);
                writer.Write(TestGuid.ToByteArray());

                // One modifier key definition
                writer.Write(1u);
                writer.Write(0x002A0000u);
                writer.Write(0x80000000u);

                // Two input map groups
                writer.Write(2u);

                // Map 10: two bindings
                writer.Write(10u);
                writer.Write(2u);

                WriteBinding(writer,
                    0x001E0000u, 0u, 3u, 0u);

                WriteBinding(writer,
                    0x00300000u, 0u, 3u, 0u);

                // Map 20, or duplicate map 10
                writer.Write(duplicateMapIds ? 10u : 20u);
                writer.Write(1u);

                WriteBinding(writer,
                    0x002E0000u, 0x20000000u, 3u, 0u);
            }

            return stream.ToArray();
        }

        // Each binding contains four unsigned 32-bit values.
        private static void WriteBinding(
            BinaryWriter writer,
            uint key,
            uint modifier,
            uint activation,
            uint unknown)
        {
            writer.Write(key);
            writer.Write(modifier);
            writer.Write(activation);
            writer.Write(unknown);
        }
    }
}
