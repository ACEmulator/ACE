using ACE.DatLoader.Entity;
using ACE.Entity;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x03. 
    /// Special thanks to Dan Skorupski for his work on Bael'Zharon's Respite, which helped fill in some of the gaps https://github.com/boardwalk/bzr
    /// </summary>
    public class Animation
    {
        public uint AnimationId { get; set; }
        public uint NumParts { get; set; }
        public uint NumFrames { get; set; }
        public List<Position> PosFrames { get; set; } = new List<Position>();
        public List<AnimationFrame> Frames { get; set; } = new List<AnimationFrame>();

        public static Animation ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Animation)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                Animation a = new Animation();
                a.AnimationId = datReader.ReadUInt32();

                uint flags = datReader.ReadUInt32();

                a.NumParts = datReader.ReadUInt32();
                a.NumFrames = datReader.ReadUInt32();

                bool hasPosFrames = ((flags & 1) > 0);
                if (hasPosFrames && a.NumFrames > 0)
                { 
                    for (uint i = 0; i < a.NumFrames; i++)
                    {
                        // Origin
                        a.PosFrames.Add(PositionExtensions.ReadPosition(datReader));
                    }
                }

                for (uint i = 0; i < a.NumFrames; i++)
                {
                    AnimationFrame f = AnimationFrame.Read(a.NumParts, datReader);
                    a.Frames.Add(f);
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = a;

                return a;
            }
        }
    }
}
