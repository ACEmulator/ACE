using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public class CMostlyConsecutiveIntSet
    {
        public int DatFileType;
        public int DatFileId;
        public int Iterations;
        public Dictionary<int, int> Ints = new();

        public CMostlyConsecutiveIntSet() { }
    }

    public static class CMostlyConsecutiveIntSetExtensions
    {
        public static CMostlyConsecutiveIntSet ReadCMostlyConsecutiveIntSet(this BinaryReader reader)
        {
            var newObj = new CMostlyConsecutiveIntSet();
            newObj.DatFileType = reader.ReadInt32();
            newObj.DatFileId = reader.ReadInt32();
            newObj.Iterations = reader.ReadInt32();
            var iterations = newObj.Iterations;
            while (iterations > 0)
            {
                var consecutiveIterations = reader.ReadInt32();
                var startingIteration = reader.ReadInt32();
                newObj.Ints.TryAdd(startingIteration, consecutiveIterations);
                iterations += consecutiveIterations;
            }
            return newObj;
        }
    }
}
