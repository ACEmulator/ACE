using System.IO;
using System.Numerics;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x32. 
    /// </summary>
    [DatFileType(DatFileType.ParticleEmitter)]
    public class ParticleEmitterInfo : IUnpackable
    {
        public uint Id { get; private set; }
        public int EmitterType { get; private set; }
        public int ParticleType { get; private set; }
        public uint GfxObjId { get; private set; }
        public uint HwGfxObjId { get; private set; }
        public double Birthrate { get; private set; }
        public int MaxParticles { get; private set; }
        public int InitialParticles { get; private set; }
        public int TotalParticles { get; private set; }
        public double TotalSeconds { get; private set; }
        public double LifespanRand { get; private set; }
        public double Lifespan { get; private set; }
        public uint SortingSphere { get; private set; }
        public Vector3 OffsetDir { get; private set; }
        public float MinOffset { get; private set; }
        public float MaxOffset { get; private set; }
        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }
        public float MinA { get; private set; }
        public float MaxA { get; private set; }
        public float MinB { get; private set; }
        public float MaxB { get; private set; }
        public float MinC { get; private set; }
        public float MaxC { get; private set; }
        public float ScaleRand { get; private set; }
        public float StartScale { get; private set; }
        public float FinalScale { get; private set; }
        public float TransRand { get; private set; }
        public float StartTrans { get; private set; }
        public float FinalTrans { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            /*uint unknown = */reader.ReadUInt32();

            EmitterType     = reader.ReadInt32();
            ParticleType    = reader.ReadInt32();

            GfxObjId    = reader.ReadUInt32();
            HwGfxObjId  = reader.ReadUInt32();

            Birthrate   = reader.ReadDouble();

            MaxParticles        = reader.ReadInt32();
            InitialParticles    = reader.ReadInt32();
            TotalParticles      = reader.ReadInt32();

            TotalSeconds    = reader.ReadDouble();
            LifespanRand    = reader.ReadDouble();
            Lifespan        = reader.ReadDouble();

            SortingSphere = reader.ReadUInt32();

            OffsetDir = reader.ReadVector3();
            MinOffset = reader.ReadSingle();
            MaxOffset = reader.ReadSingle();

            A = reader.ReadVector3();
            B = reader.ReadVector3();
            C = reader.ReadVector3();

            MinA = reader.ReadSingle();
            MaxA = reader.ReadSingle();
            MinB = reader.ReadSingle();
            MaxB = reader.ReadSingle();
            MinC = reader.ReadSingle();
            MaxC = reader.ReadSingle();

            ScaleRand   = reader.ReadSingle();
            StartScale  = reader.ReadSingle();
            FinalScale  = reader.ReadSingle();
            TransRand   = reader.ReadSingle();
            StartTrans  = reader.ReadSingle();
            FinalTrans  = reader.ReadSingle();
        }

        public static ParticleEmitterInfo ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(fileId, out var result))
                return (ParticleEmitterInfo)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new ParticleEmitterInfo();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
