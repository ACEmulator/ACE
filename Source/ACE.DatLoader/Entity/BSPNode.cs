using ACE.Entity.Enum;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class BSPNode
    {
        // These constants are actually strings in the dat file
        private const uint PORT = 1347375700; // 0x504F5254
        private const uint LEAF = 1279607110; // 0x4C454146
        private const uint BPnn = 1112567406; // 0x42506E6E
        private const uint BPIn = 1112557934; // 0x4250496E
        private const uint BpIN = 1114655054; // 0x4270494E
        private const uint BpnN = 1114664526; // 0x42706E4E
        private const uint BPIN = 1112557902; // 0x4250494E
        private const uint BPnN = 1112567374; // 0x42506E4E
        
        public uint Type { get; set; }
        public Plane SplittingPlane { get; set; }
        public BSPNode PosNode { get; set; }
        public BSPNode NegNode { get; set; }
        public CSphere Sphere { get; set; }
        public List<ushort> InPolys { get; set; } = new List<ushort>(); // List of PolygonIds

        public static BSPNode Read(DatReader datReader, BSPType treeType)
        {
            BSPNode obj = new BSPNode();

            obj.Type = datReader.ReadUInt32();
            
            switch (obj.Type)
            {
                case PORT:
                    return BSPPortal.ReadPortal(datReader, treeType);
                case LEAF:
                    return BSPLeaf.ReadLeaf(datReader, treeType);
            }

            obj.SplittingPlane = Plane.Read(datReader);

            switch (obj.Type)
            {
                case BPnn:
                case BPIn:
                    obj.PosNode = BSPNode.Read(datReader, treeType);
                    break;
                case BpIN:
                case BpnN:
                    obj.NegNode = BSPNode.Read(datReader, treeType);
                    break;
                case BPIN:
                case BPnN:
                    obj.PosNode = BSPNode.Read(datReader, treeType);
                    obj.NegNode = BSPNode.Read(datReader, treeType);
                    break;
            }


            if (treeType == BSPType.Cell)
                return obj;

            obj.Sphere = CSphere.Read(datReader);

            if (treeType == BSPType.Physics)
                return obj;

            uint numPolys = datReader.ReadUInt32();
            for (uint i = 0; i < numPolys; i++)
                obj.InPolys.Add(datReader.ReadUInt16());
            
            return obj;
        }
    }
}
