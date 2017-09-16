using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class BSPLeaf : BSPNode
    {
        public int LeafIndex { get; set; }
        public int Solid { get; set; }

        public static BSPLeaf ReadLeaf(DatReader datReader, BSPType treeType)
        {
            BSPLeaf obj = new BSPLeaf();
            obj.Type = 0x4C454146; // LEAF
            obj.LeafIndex = datReader.ReadInt32();

            if (treeType == BSPType.Physics)
            {
                obj.Solid = datReader.ReadInt32();
                // Note that if Solid is equal to 0, these values will basically be null. Still read them, but they don't mean anything.
                obj.Sphere = CSphere.Read(datReader);

                uint numPolys = datReader.ReadUInt32();
                for (uint i = 0; i < numPolys; i++)
                    obj.InPolys.Add(datReader.ReadUInt16());
            }

            return obj;
        }
    }
}
