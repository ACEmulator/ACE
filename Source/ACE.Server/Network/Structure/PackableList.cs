using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public static class PackableList
    {
        public static List<uint> ReadListUInt32(this BinaryReader reader)
        {
            var size = reader.ReadUInt32();

            var list = new List<uint>();

            for (var i = 0; i < size; i++)
                list.Add(reader.ReadUInt32());

            return list;
        }

        public static void Write(this BinaryWriter writer, List<uint> list)
        {
            writer.Write(list.Count);
            foreach (var item in list)
                writer.Write(item);
        }
    }
}
