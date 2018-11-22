using System;
using System.IO;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class StringTableData : IUnpackable
    {
        public uint Id { get; private set; }
        public List<string> VarNames { get; } = new List<string>();
        public List<string> Vars { get; } = new List<string>();
        public List<string> Strings { get; } = new List<string>();
        public List<uint> Comments { get; } = new List<uint>();

        public byte Unknown { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            var num_varnames = reader.ReadUInt16();
            for (uint i = 0; i < num_varnames; i++)
                VarNames.Add(reader.ReadUnicodeString());

            var num_vars = reader.ReadUInt16();
            for (uint i = 0; i < num_vars; i++)
                Vars.Add(reader.ReadUnicodeString());

            var num_strings = reader.ReadUInt32();
            for (uint i = 0; i < num_strings; i++)
                Strings.Add(reader.ReadUnicodeString());

            var num_comments = reader.ReadUInt32();
            for (uint i = 0; i < num_comments; i++)
                Comments.Add(reader.ReadUInt32());

            Unknown = reader.ReadByte();
        }
    }
}
