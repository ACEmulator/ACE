using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;

namespace ACE.Server.Physics.Util
{
    /// <summary>
    /// Some dungeons require position adjustments, as well as cell adjustments
    /// </summary>
    public class AdjustPos
    {
        public static Dictionary<uint, AdjustPosProfile> DungeonProfiles;

        static AdjustPos()
        {
            DungeonProfiles = new Dictionary<uint, AdjustPosProfile>();
        }

        public static bool Adjust(uint dungeonID, Position pos)
        {
            if (!DungeonProfiles.TryGetValue(dungeonID, out var profile))
                return false;

            pos.Pos += profile.GoodPosition - profile.BadPosition;
            //pos.Rotation *= profile.GoodRotation * Quaternion.Inverse(profile.BadRotation);

            return true;
        }
    }
}
