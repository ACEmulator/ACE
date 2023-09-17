using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public class PTaggedIterationList
    {
        public int DatFileType;
        public int DatFileId;
        public CMostlyConsecutiveIntSet List = new();

        public PTaggedIterationList() { }

        public override string ToString()
        {
            var str = "";

            str += $"DatFileType: {DatFileType}" + Environment.NewLine;
            str += $"DatFileId:   {DatFileId}" + Environment.NewLine;
            str += List;

            return str;
        }
    }

    public static class PTaggedIterationListExtensions
    {
        public static PTaggedIterationList ReadPTaggedIterationList(this BinaryReader reader)
        {
            var newObj = new PTaggedIterationList();
            newObj.DatFileType = reader.ReadInt32();
            newObj.DatFileId = reader.ReadInt32();
            newObj.List = reader.ReadCMostlyConsecutiveIntSet();
            return newObj;
        }
    }
}
