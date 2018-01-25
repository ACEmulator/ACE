using System.IO;

namespace ACE.DatLoader
{
    interface IUnpackable
    {
        void Unpack(BinaryReader reader);
    }
}
