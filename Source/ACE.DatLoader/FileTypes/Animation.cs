using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x03. 
    /// Special thanks to Dan Skorupski for his work on Bael'Zharon's Respite, which helped fill in some of the gaps https://github.com/boardwalk/bzr
    /// </summary>
    [DatFileType(DatFileType.Animation)]
    public class Animation : IUnpackable
    {
        public uint AnimationId { get; private set; }
        public uint Bitfield { get; private set; }
        public uint NumParts { get; private set; }
        public uint NumFrames { get; private set; }
        public List<Frame> PosFrames { get; } = new List<Frame>();
        public List<AnimationFrame> PartFrames { get; } = new List<AnimationFrame>();

        public void Unpack(BinaryReader reader)
        {
            AnimationId = reader.ReadUInt32();
            Bitfield    = reader.ReadUInt32();
            NumParts    = reader.ReadUInt32();
            NumFrames   = reader.ReadUInt32();

            if ((Bitfield & 1) != 0)
                PosFrames.Unpack(reader, NumFrames);

            for (uint i = 0; i < NumFrames; i++)
            {
                var animationFrame = new AnimationFrame();
                animationFrame.Unpack(reader, NumParts);
                PartFrames.Add(animationFrame);
            }
        }

        public static Animation ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (Animation)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new Animation();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
