namespace ACE.Server.Physics.Common
{
    public class QualifiedDataID
    {
        public uint Type;
        public uint ID;

        public QualifiedDataID(uint type, uint id)
        {
            // todo: use ACE data structures
            Type = type;
            ID = id;
        }
    }
}
