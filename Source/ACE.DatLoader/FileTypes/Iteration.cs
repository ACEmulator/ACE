using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are stored in the client_cell.dat, client_portal.dat, and client_local_English.dat files with the index 0xFFFF0001
    ///
    /// This is essentially the dat "versioning" system.
    /// This is used when first connecting to the server to compare the client dat files with the server dat files and any subsequent patching that may need to be done.
    /// 
    /// Special thanks to the GDLE team for pointing me the right direction on how/where to find this info in the dat files- OptimShi
    /// </summary>
    public class Iteration : FileType
    {
        internal const uint FILE_ID = 0xFFFF0001;

        public int TotalIterations { get; private set; }
        public Dictionary<int, int> Ints { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            TotalIterations = reader.ReadInt32();

            var iterationCount = TotalIterations;
            Ints = new Dictionary<int, int>();
            while (iterationCount > 0)
            {
                var consecutiveIterations = reader.ReadInt32();
                var startingIteration = reader.ReadInt32();
                Ints.Add(startingIteration, consecutiveIterations);
                iterationCount += consecutiveIterations;
            }
        }

        public override string ToString()
        {
            return TotalIterations.ToString();
        }
    }
}
