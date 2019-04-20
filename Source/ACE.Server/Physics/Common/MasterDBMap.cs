namespace ACE.Server.Physics.Common
{
    public class MasterDBMap
    {
        public static uint DivineType(uint dataDID)
        {
            var type = dataDID >> 24;
            if (type == 0x01)
                return 6;
            else if (type == 0x02)
                return 7;
            else
                return 0;
        }
    }
}
