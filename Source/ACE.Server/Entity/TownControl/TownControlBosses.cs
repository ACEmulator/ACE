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
                        42132032,
                        new TownControlBoss()
                        {
                            WeenieID = 42132032,
                            TownID = 91,
                            TownName = "Shoushi",
                            BossType = TownControlBossType.ConflictBoss
                        }
                    );


                    //Holtburg
                    _tcBossMap.Add(
                        4200001,
                        new TownControlBoss()
                        {
                            WeenieID = 4200001,
                            TownID = 72,
                            TownName = "Holtburg",
                            BossType = TownControlBossType.InitiationBoss
                        }
                    );

                    _tcBossMap.Add(
                        4200007,
                        new TownControlBoss()
                        {
                            WeenieID = 4200007,
                            TownID = 72,
                            TownName = "Holtburg",
                            BossType = TownControlBossType.ConflictBoss
                        }
                    );

                    //Yaraq
                    _tcBossMap.Add(
                        4200003,
                        new TownControlBoss()
                        {
                            WeenieID = 4200003, //TODO
                            TownID = 102,
                            TownName = "Yaraq",
                            BossType = TownControlBossType.InitiationBoss
                        }
                    );

                    _tcBossMap.Add(
                        4200008,
                        new TownControlBoss()
                        {
                            WeenieID = 4200008,
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

        public static bool IsTownControlInitBoss(uint weenieId)
        {
            if (TownControlBosses.TownControlBossMap.ContainsKey(weenieId))
            {
                return TownControlBosses.TownControlBossMap[weenieId].BossType.Equals(TownControlBossType.InitiationBoss);
            }

            return false;
        }

        public static bool IsTownControlConflictBoss(uint weenieId)
        {
            if (TownControlBosses.TownControlBossMap.ContainsKey(weenieId))
            {
                return TownControlBosses.TownControlBossMap[weenieId].BossType.Equals(TownControlBossType.ConflictBoss);                
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
