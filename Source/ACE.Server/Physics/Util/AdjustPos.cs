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

            // Burial Temple // No longer needed as of 11/24/19
            //var burialTemple = new AdjustPosProfile();
            //burialTemple.BadPosition = new Vector3(30.389999f, -37.439999f, 0.000000f);
            //burialTemple.BadRotation = new Quaternion(-0f, 0, 0, -1f);
            //burialTemple.GoodPosition = new Vector3(30f, -146.30799865723f, 0.0049999998882413f);
            //burialTemple.GoodRotation = new Quaternion(1, 0, 0, 0);

            //DungeonProfiles.Add(0x13e, burialTemple); // No longer needed as of 11/24/19

            // North Glenden Prison // No longer needed as of 09/22/19
            //var glendenPrison = new AdjustPosProfile();
            //glendenPrison.BadPosition = new Vector3(38.400002f, -18.600000f, 6.000000f);
            //glendenPrison.BadRotation = new Quaternion(-0.782608f, 0, 0, -0.622514f);
            //glendenPrison.GoodPosition = new Vector3(61, -20, -17.995000839233f);
            //glendenPrison.GoodRotation = new Quaternion(-0.70710700750351f, 0, 0, -0.70710700750351f);

            //DungeonProfiles.Add(0x1e4, glendenPrison); // No longer needed as of 09/22/19

            // Nuhmudira's Dungeon // No longer needed as of 11/24/19
            //var nuhmudirasDungeon = new AdjustPosProfile();
            //nuhmudirasDungeon.BadPosition = new Vector3(149.242996f, -49.946301f, -5.995000f);
            //nuhmudirasDungeon.BadRotation = new Quaternion(-0.707107f, 0, 0, -0.707107f);
            //nuhmudirasDungeon.GoodPosition = new Vector3(149.24299621582f, -129.94599914551f, -5.9949998855591f);
            //nuhmudirasDungeon.GoodRotation = new Quaternion(0.6967059969902f, 0, 0, 0.71735697984695f);

            //DungeonProfiles.Add(0x536d, nuhmudirasDungeon); // No longer needed as of 11/24/19
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
