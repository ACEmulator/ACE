using System;
using System.IO;

using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class BSPTree : IUnpackable
    {
        public BSPNode RootNode { get; private set; } = new BSPNode();

        /// <summary>
        /// You must use the Unpack(BinaryReader reader, BSPType treeType) method.
        /// </summary>
        /// <exception cref="NotSupportedException">You must use the Unpack(BinaryReader reader, BSPType treeType) method.</exception>
        public void Unpack(BinaryReader reader)
        {
            throw new NotSupportedException();
        }

        public void Unpack(BinaryReader reader, BSPType treeType)
        {
            RootNode = BSPNode.ReadNode(reader, treeType);
        }
    }
}
