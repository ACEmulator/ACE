using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    public class MotionTable
    {
        public uint Id { get; set; }
        public uint DefaultStyle { get; set; }
        public Dictionary<uint, uint> StyleDefaults { get; set; } = new Dictionary<uint, uint>();
        public Dictionary<uint, MotionData> Cycles { get; set; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, MotionData> Modifiers { get; set; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, Dictionary<uint, MotionData>> Links { get; set; } = new Dictionary<uint, Dictionary<uint, MotionData>>();

        public static MotionTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (MotionTable)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                MotionTable m = new MotionTable();
                m.Id = datReader.ReadUInt32();

                m.DefaultStyle = datReader.ReadUInt32();

                uint numStyleDefaults = datReader.ReadUInt32();
                for (uint i = 0; i < numStyleDefaults; i++)
                    m.StyleDefaults.Add(datReader.ReadUInt32(), datReader.ReadUInt32());

                uint numCycles = datReader.ReadUInt32();
                for (uint i = 0; i < numCycles; i++)
                {
                    uint key = datReader.ReadUInt32();
                    MotionData md = MotionData.Read(datReader);
                    m.Cycles.Add(key, md);
                }

                uint numModifiers = datReader.ReadUInt32();
                for (uint i = 0; i < numModifiers; i++)
                {
                    uint key = datReader.ReadUInt32();
                    MotionData md = MotionData.Read(datReader);
                    m.Modifiers.Add(key, md);
                }

                uint numLinks = datReader.ReadUInt32();
                for (uint i = 0; i < numLinks; i++)
                {
                    uint firstKey = datReader.ReadUInt32();
                    uint numSubLinks = datReader.ReadUInt32();
                    Dictionary<uint, MotionData> links = new Dictionary<uint, MotionData>();
                    for (uint j = 0; j < numSubLinks; j++)
                    {
                        uint subKey = datReader.ReadUInt32();
                        MotionData md = MotionData.Read(datReader);
                        links.Add(subKey, md);
                    }
                    m.Links.Add(firstKey, links);
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = m;

                return m;
            }
        }
    }
}
