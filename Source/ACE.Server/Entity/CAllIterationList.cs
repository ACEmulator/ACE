using ACE.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.Server.Entity
{
    public class CAllIterationList
    {
        public List<CMostlyConsecutiveIntSet> Lists = new List<CMostlyConsecutiveIntSet>();

        public static CAllIterationList Read(BinaryReader reader)
        {
            CAllIterationList obj = new CAllIterationList();
            var numElements = reader.ReadInt32();
            for(var i = 0;i < numElements; i++)
                obj.Lists.Add(CMostlyConsecutiveIntSet.Read(reader));
            return obj;
        }

        public class CMostlyConsecutiveIntSet
        {
            public int DatFileType;
            public int DatFileId;
            public List<int> Ints = new List<int>();
            public bool Sorted;

            public static CMostlyConsecutiveIntSet Read(BinaryReader reader)
            {
                CMostlyConsecutiveIntSet newObj = new CMostlyConsecutiveIntSet();
                newObj.DatFileType = reader.ReadInt32();
                newObj.DatFileId = reader.ReadInt32();
                newObj.Ints.Add(reader.ReadInt32());
                newObj.Ints.Add(reader.ReadInt32());
                newObj.Sorted = reader.ReadBoolean();
                reader.Align();
                return newObj;
            }
        }


    }
}
