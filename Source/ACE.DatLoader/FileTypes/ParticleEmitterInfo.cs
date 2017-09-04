using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x32. 
    /// </summary>
    public class ParticleEmitterInfo
    {
        public uint Id { get; set; }
        public EmitterType EmitterType { get; set; }
        public ParticleType ParticleType { get; set; }
        public uint GfxObjId { get; set; }
        public uint HwGfxObjId { get; set; }
        public double Birthrate { get; set; }
        public int MaxParticles { get; set; }
        public int InitialParticles { get; set; }
        public int TotalParticles { get; set; }
        public double TotalSeconds { get; set; }
        public double LifespanRand { get; set; }
        public double Lifespan { get; set; }
        public uint SortingSphere { get; set; }
        public Position OffsetDir { get; set; }
        public float MinOffset { get; set; }
        public float MaxOffset { get; set; }
        public Position A { get; set; }
        public Position B { get; set; }
        public Position C { get; set; }
        public float MinA { get; set; }
        public float MaxA { get; set; }
        public float MinB { get; set; }
        public float MaxB { get; set; }
        public float MinC { get; set; }
        public float MaxC { get; set; }
        public float ScaleRand { get; set; }
        public float StartScale { get; set; }
        public float FinalScale { get; set; }
        public float TransRand { get; set; }
        public float StartTrans { get; set; }
        public float FinalTrans { get; set; }

        public static ParticleEmitterInfo ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (ParticleEmitterInfo)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                ParticleEmitterInfo obj = new ParticleEmitterInfo();

                obj.Id = datReader.ReadUInt32();

                uint unknown = datReader.ReadUInt32();

                obj.EmitterType = (EmitterType)datReader.ReadInt32();
                obj.ParticleType = (ParticleType)datReader.ReadInt32();

                obj.GfxObjId = datReader.ReadUInt32();
                obj.HwGfxObjId = datReader.ReadUInt32();

                obj.Birthrate = datReader.ReadDouble();

                obj.MaxParticles = datReader.ReadInt32();
                obj.InitialParticles = datReader.ReadInt32();
                obj.TotalParticles = datReader.ReadInt32();

                obj.TotalSeconds = datReader.ReadDouble();
                obj.LifespanRand = datReader.ReadDouble();
                obj.Lifespan = datReader.ReadDouble();

                obj.SortingSphere = datReader.ReadUInt32();

                obj.OffsetDir = PositionExtensions.ReadPositionFrame(datReader);
                obj.MinOffset = datReader.ReadSingle();
                obj.MaxOffset = datReader.ReadSingle();

                obj.A = PositionExtensions.ReadPositionFrame(datReader);
                obj.B = PositionExtensions.ReadPositionFrame(datReader);
                obj.C = PositionExtensions.ReadPositionFrame(datReader);

                obj.MinA = datReader.ReadSingle();
                obj.MaxA = datReader.ReadSingle();
                obj.MinB = datReader.ReadSingle();
                obj.MaxB = datReader.ReadSingle();
                obj.MinC = datReader.ReadSingle();
                obj.MaxC = datReader.ReadSingle();

                obj.ScaleRand = datReader.ReadSingle();
                obj.StartScale = datReader.ReadSingle();
                obj.FinalScale = datReader.ReadSingle();
                obj.TransRand = datReader.ReadSingle();
                obj.StartTrans = datReader.ReadSingle();
                obj.FinalTrans = datReader.ReadSingle();

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
