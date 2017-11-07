using ACE.Entity.Enum;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class BSPNode
    {
        public BSPNodeType Type { get; set; }
        public Plane SplittingPlane { get; set; }
        public BSPNode PosNode { get; set; }
        public BSPNode NegNode { get; set; }
        public CSphere Sphere { get; set; }
        public List<ushort> InPolys { get; set; } = new List<ushort>(); // List of PolygonIds

        public static BSPNode Read(DatReader datReader, BSPType treeType)
        {
            BSPNode obj = new BSPNode {Type = (BSPNodeType) datReader.ReadUInt32()};

            switch (obj.Type)
            {
                case BSPNodeType.PORT:
                    return BSPPortal.ReadPortal(datReader, treeType);
                case BSPNodeType.LEAF:
                    return BSPLeaf.ReadLeaf(datReader, treeType);
            }

            obj.SplittingPlane = Plane.Read(datReader);

            switch (obj.Type)
            {
                case BSPNodeType.BPnn:
                case BSPNodeType.BPIn:
                    obj.PosNode = BSPNode.Read(datReader, treeType);
                    break;
                case BSPNodeType.BpIN:
                case BSPNodeType.BpnN:
                    obj.NegNode = BSPNode.Read(datReader, treeType);
                    break;
                case BSPNodeType.BPIN:
                case BSPNodeType.BPnN:
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
