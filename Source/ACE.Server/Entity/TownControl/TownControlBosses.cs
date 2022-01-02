using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity.TownControl
{
    public static class TownControlBosses
    {
        private static Dictionary<uint, TownControlBoss> _tcBossMap;
        public static Dictionary<uint, TownControlBoss> TownControlBossMap
        {
            get
            {
                if(_tcBossMap == null)
                {
                    _tcBossMap = new Dictionary<uint, TownControlBoss>();

                    //Shoushi
                    _tcBossMap.Add(
                        42153365,
                        new TownControlBoss()
                        {
                            WeenieID = 42153365,
                            TownID = 91,
                            TownName = "Shoushi",
                            BossType = TownControlBossType.InitiationBoss
                        }
                    );

                    _tcBossMap.Add(
                        42153366,
                        new TownControlBoss()
                        {
                            WeenieID = 42153366,
                            TownID = 91,
                            TownName = "Shoushi",
                            BossType = TownControlBossType.ConflictBoss
                        }
                    );


                    //Holtburg
                    _tcBossMap.Add(
                        13345,
                        new TownControlBoss()
                        {
                            WeenieID = 13345,
                            TownID = 72,
                            TownName = "Holtburg",
                            BossType = TownControlBossType.InitiationBoss
                        }
                    );

                    _tcBossMap.Add(
                        13346, //TODO
                        new TownControlBoss()
                        {
                            WeenieID = 12346, //TODO
                            TownID = 72,
                            TownName = "Holtburg",
                            BossType = TownControlBossType.ConflictBoss
                        }
                    );

                    //Yaraq
                    _tcBossMap.Add(
                        14345, //TODO
                        new TownControlBoss()
                        {
                            WeenieID = 14345, //TODO
                            TownID = 102,
                            TownName = "Yaraq",
                            BossType = TownControlBossType.InitiationBoss
                        }
                    );

                    _tcBossMap.Add(
                        14346, //TODO
                        new TownControlBoss()
                        {
                            WeenieID = 14346, //TODO
                            TownID = 102,
                            TownName = "Yaraq",
                            BossType = TownControlBossType.ConflictBoss
                        }
                    );
                }

                return _tcBossMap;
            }
        }

        public static bool IsTownControlBoss(uint weenieId)
        {
            return TownControlBosses.TownControlBossMap.ContainsKey(weenieId);
        }

        public static bool IsTownControlConflictBoss(uint weenieId)
        {
            if (TownControlBosses.TownControlBossMap.ContainsKey(weenieId))
            {
                if(TownControlBosses.TownControlBossMap[weenieId].BossType.Equals(TownControlBossType.ConflictBoss))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class TownControlBoss
    {
        public uint WeenieID { get; set; }

        public TownControlBossType BossType { get; set; }

        public uint TownID { get; set; }

        public string TownName { get; set; }
    }

    public enum TownControlBossType
    {
        InitiationBoss = 0,
        ConflictBoss = 1
    }    
}
