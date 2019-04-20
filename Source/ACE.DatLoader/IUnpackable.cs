using System.IO;

namespace ACE.DatLoader
{
    public interface IUnpackable
    {
        void Unpack(BinaryReader reader);
    }
}
