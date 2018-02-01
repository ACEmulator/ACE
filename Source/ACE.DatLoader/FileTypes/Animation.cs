using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x03. 
    /// Special thanks to Dan Skorupski for his work on Bael'Zharon's Respite, which helped fill in some of the gaps https://github.com/boardwalk/bzr
    /// </summary>
    public class Animation : IUnpackable
    {
        public uint AnimationId { get; private set; }
        public uint NumParts { get; private set; }
        public uint NumFrames { get; private set; }
        public List<Position> PosFrames { get; } = new List<Position>();
        public List<AnimationFrame> Frames { get; } = new List<AnimationFrame>();

        public void Unpack(BinaryReader reader)
        {
            AnimationId = reader.ReadUInt32();
            var flags   = reader.ReadUInt32();
            NumParts    = reader.ReadUInt32();
            NumFrames   = reader.ReadUInt32();

            if ((flags & 1) != 0)
            {
                for (uint i = 0; i < NumFrames; i++)
                {
                    // Origin
                    var position = new Position();
                    position.Read(reader);
                    PosFrames.Add(position);
                }
            }

            for (uint i = 0; i < NumFrames; i++)
            {
                var animationFrame = new AnimationFrame();
                animationFrame.Unpack(reader, NumParts);
                Frames.Add(animationFrame);
            }
        }

        public static Animation ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (Animation)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            Animation animation = new Animation();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                animation.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = animation;

            return animation;
        }
    }
}
