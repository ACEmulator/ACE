using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class StarterArea : IUnpackable
    {
        public string Name { get; private set; }
        public List<Position> Locations { get; } = new List<Position>();

        public void Unpack(BinaryReader reader)
        {
            Name = reader.ReadString();

            Locations.UnpackSmartArray(reader);
        }
    }
}
