using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class AmbientSTBDesc : IUnpackable
    {
        public uint STBId { get; private set; }
        public List<AmbientSoundDesc> AmbientSounds { get; } = new List<AmbientSoundDesc>();

        public void Unpack(BinaryReader reader)
        {
            STBId = reader.ReadUInt32();

            AmbientSounds.Unpack(reader);
        }
    }
}
