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

        public List<int> Ints { get; private set; }
        public bool Sorted { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            Ints = new List<int>();
            Ints.Add(reader.ReadInt32());
            Ints.Add(reader.ReadInt32());
            Sorted = reader.ReadBoolean();
            reader.AlignBoundary();
        }

        public override string ToString()
        {
            string s = "";
            for (var i = 0; i < Ints.Count; i++)
            {
                s += Ints[i] + ",";
            }
            if (Sorted)
                s += "1";
            else
                s += "0";
            return s;
        }
    }
}
