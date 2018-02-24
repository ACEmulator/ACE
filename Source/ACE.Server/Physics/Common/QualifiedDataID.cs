namespace ACE.Server.Physics.Common
{
    public class QualifiedDataID
    {
        public int Type;
        public int ID;

        public QualifiedDataID(int type, int id)
        {
            Type = type;
            ID = id;
        }
    }
}
