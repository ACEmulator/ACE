
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.KeyMap)]
    public class KeyMap : FileType
    {
        public string Name { get; private set; }
        public Guid MapGuid { get; private set; }

        public List<KeyMapDevice> Devices { get; } =
            new List<KeyMapDevice>();

        public List<KeyMapControl> MetaKeys { get; } =
            new List<KeyMapControl>();

        public Dictionary<uint, List<KeyMapBinding>> InputMaps { get; } =
            new Dictionary<uint, List<KeyMapBinding>>();

        public override void Unpack(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            // Build the record in temporary collections so a failed
            // unpack never leaves a partially populated KeyMap.
            uint id = reader.ReadUInt32();
            string name = reader.ReadString();
            Guid mapGuid = ReadGuid(reader);

            var devices = new List<KeyMapDevice>();
            var metaKeys = new List<KeyMapControl>();
            var inputMaps =
                new Dictionary<uint, List<KeyMapBinding>>();

            // Each device consists of a byte and a GUID.
            uint deviceCount = reader.ReadUInt32();
            ValidateCount(reader, deviceCount, 17, "Devices");

            for (uint i = 0; i < deviceCount; i++)
            {
                devices.Add(new KeyMapDevice
                {
                    Type = reader.ReadByte(),
                    Id = ReadGuid(reader)
                });
            }

            // Each modifier definition has two UInt32 values.
            uint metaKeyCount = reader.ReadUInt32();
            ValidateCount(reader, metaKeyCount, 8, "MetaKeys");

            for (uint i = 0; i < metaKeyCount; i++)
            {
                metaKeys.Add(ReadControl(reader));
            }

            // Each input map has an ID and a binding count,
            // followed by its individual binding records.
            uint inputMapCount = reader.ReadUInt32();
            ValidateCount(reader, inputMapCount, 8, "InputMaps");

            for (uint i = 0; i < inputMapCount; i++)
            {
                uint mapId = reader.ReadUInt32();
                uint bindingCount = reader.ReadUInt32();

                // A qualified control contains four UInt32 values.
                ValidateCount(
                    reader, bindingCount, 16, "Bindings");

                var bindings = new List<KeyMapBinding>();

                for (uint j = 0; j < bindingCount; j++)
                {
                    bindings.Add(new KeyMapBinding
                    {
                        Control = ReadControl(reader),
                        Activation = reader.ReadUInt32(),
                        Unknown = reader.ReadUInt32()
                    });
                }

                if (inputMaps.ContainsKey(mapId))
                {
                    throw new InvalidDataException(
                        $"Duplicate KeyMap input map ID: 0x{mapId:X8}");
                }

                inputMaps.Add(mapId, bindings);
            }

            // Commit the successfully decoded record.
            Id = id;
            Name = name;
            MapGuid = mapGuid;

            Devices.Clear();
            Devices.AddRange(devices);

            MetaKeys.Clear();
            MetaKeys.AddRange(metaKeys);

            InputMaps.Clear();
            foreach (var entry in inputMaps)
                InputMaps.Add(entry.Key, entry.Value);
        }

        private static Guid ReadGuid(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(16);

            if (bytes.Length != 16)
            {
                throw new EndOfStreamException(
                    "Incomplete KeyMap GUID.");
            }

            return new Guid(bytes);
        }

        private static KeyMapControl ReadControl(
            BinaryReader reader)
        {
            return new KeyMapControl
            {
                Key = reader.ReadUInt32(),
                Modifier = reader.ReadUInt32()
            };
        }

        private static void ValidateCount(
            BinaryReader reader,
            uint count,
            int minimumBytesPerEntry,
            string sectionName)
        {
            Stream stream = reader.BaseStream;

            if (!stream.CanSeek)
            {
                throw new NotSupportedException(
                    "KeyMap unpacking requires a seekable stream.");
            }

            long remaining = stream.Length - stream.Position;

            if (remaining < 0 ||
                (ulong)count * (ulong)minimumBytesPerEntry >
                (ulong)remaining)
            {
                throw new InvalidDataException(
                    $"Invalid KeyMap {sectionName} count: {count}.");
            }
        }
    }

    public class KeyMapDevice
    {
        public byte Type { get; set; }
        public Guid Id { get; set; }
    }

    public class KeyMapControl
    {
        public uint Key { get; set; }
        public uint Modifier { get; set; }
    }

    public class KeyMapBinding
    {
        public KeyMapControl Control { get; set; }
        public uint Activation { get; set; }
        public uint Unknown { get; set; }
    }
}
