using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SkyTimeOfDay : IUnpackable
    {
        public float Begin { get; private set; }

        public float DirBright { get; private set; }
        public float DirHeading { get; private set; }
        public float DirPitch { get; private set; }
        public uint DirColor { get; private set; }

        public float AmbBright { get; private set; }
        public uint AmbColor { get; private set; }

        public float MinWorldFog { get; private set; }
        public float MaxWorldFog { get; private set; }
        public uint WorldFogColor { get; private set; }
        public uint WorldFog { get; private set; }

        public List<SkyObjectReplace> SkyObjReplace { get; } = new List<SkyObjectReplace>();

        public void Unpack(BinaryReader reader)
        {
            Begin       = reader.ReadSingle();

            DirBright   = reader.ReadSingle();
            DirHeading  = reader.ReadSingle();
            DirPitch    = reader.ReadSingle();
            DirColor    = reader.ReadUInt32();

            AmbBright   = reader.ReadSingle();
            AmbColor    = reader.ReadUInt32();

            MinWorldFog     = reader.ReadSingle();
            MaxWorldFog     = reader.ReadSingle();
            WorldFogColor   = reader.ReadUInt32();
            WorldFog        = reader.ReadUInt32();

            reader.AlignBoundary();

            SkyObjReplace.Unpack(reader);
        }
    }
}
