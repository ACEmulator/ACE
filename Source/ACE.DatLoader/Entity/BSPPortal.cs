using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class BSPPortal : BSPNode
    {
        public List<PortalPoly> InPortals { get; set; } = new List<PortalPoly>();

        public static BSPPortal ReadPortal(DatReader datReader, BSPType treeType)
        {
            BSPPortal obj = new BSPPortal();
            obj.Type = 0x504F5254; // PORT
            obj.SplittingPlane = Plane.Read(datReader);
            obj.PosNode = BSPNode.Read(datReader, treeType);
            obj.NegNode = BSPNode.Read(datReader, treeType);

            if (treeType == BSPType.Drawing)
            {
                obj.Sphere = CSphere.Read(datReader);

                uint numPolys = datReader.ReadUInt32();
                uint numPortals = datReader.ReadUInt32();

                for (uint i = 0; i < numPolys; i++)
                    obj.InPolys.Add(datReader.ReadUInt16());

                for (uint i = 0; i < numPortals; i++)
                    obj.InPortals.Add(PortalPoly.Read(datReader));
            }

            return obj;
        }
    }
}