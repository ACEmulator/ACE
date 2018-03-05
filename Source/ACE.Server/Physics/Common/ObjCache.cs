using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public static class ObjCache
    {
        public static Dictionary<int, MotionTable> MotionTables;

        static ObjCache()
        {
            MotionTables = new Dictionary<int, MotionTable>();
        }

        public static MotionTable GetMotionTable(int id)
        {
            MotionTable mtable = null;
            MotionTables.TryGetValue(id, out mtable);
            return mtable;
        }
    }
}
