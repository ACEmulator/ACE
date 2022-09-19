using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public class CAllIterationList
    {
        public List<PTaggedIterationList> Lists = new();

        public CAllIterationList() { }
    }


    public static class CAllIterationListExtensions
    {
        public static CAllIterationList ReadCAllIterationList(this BinaryReader reader)
        {
            var obj = new CAllIterationList();
            var numElements = reader.ReadInt32();
            for (var i = 0; i < numElements; i++)
                obj.Lists.Add(reader.ReadPTaggedIterationList());
            return obj;
        }
    }
}
