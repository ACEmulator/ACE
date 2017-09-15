using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class BSPTree
    {
        public BSPNode RootNode { get; set; }

        public static BSPTree Read(DatReader datReader, BSPType treeType)
        {
            BSPTree obj = new BSPTree();
            obj.RootNode = BSPNode.Read(datReader, treeType);
            return (obj);
        }
    }
}
