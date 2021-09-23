using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class LightWeaponWcids
    {
        private static ChanceTable<WeenieClassName> Dolabras = new ChanceTable<WeenieClassName>()
        {
            // light - axe
            ( WeenieClassName.axedolabra,         0.40f ),
            ( WeenieClassName.axedolabraacid,     0.15f ),
            ( WeenieClassName.axedolabraelectric, 0.15f ),
            ( WeenieClassName.axedolabrafire,     0.15f ),
            ( WeenieClassName.axedolabrafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> HandAxes = new ChanceTable<WeenieClassName>()
        {
            // light - axe
            ( WeenieClassName.axehand,         0.40f ),
            ( WeenieClassName.axehandacid,     0.15f ),
            ( WeenieClassName.axehandelectric, 0.15f ),
            ( WeenieClassName.axehandfire,     0.15f ),
            ( WeenieClassName.axehandfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Onos = new ChanceTable<WeenieClassName>()
        {
            // light - axe
            ( WeenieClassName.ono,         0.40f ),
            ( WeenieClassName.onoacid,     0.15f ),
            ( WeenieClassName.onoelectric, 0.15f ),
            ( WeenieClassName.onofire,     0.15f ),
            ( WeenieClassName.onofrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> WarHammers = new ChanceTable<WeenieClassName>()
        {
            // light - axe
            ( WeenieClassName.warhammer,         0.40f ),
            ( WeenieClassName.warhammeracid,     0.15f ),
            ( WeenieClassName.warhammerelectric, 0.15f ),
            ( WeenieClassName.warhammerfire,     0.15f ),
            ( WeenieClassName.warhammerfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Daggers = new ChanceTable<WeenieClassName>()
        {
            // light - dagger (multi-strike)
            ( WeenieClassName.dagger,         0.40f ),
            ( WeenieClassName.daggeracid,     0.15f ),
            ( WeenieClassName.daggerelectric, 0.15f ),
            ( WeenieClassName.daggerfire,     0.15f ),
            ( WeenieClassName.daggerfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Khanjars = new ChanceTable<WeenieClassName>()
        {
            // light - dagger
            ( WeenieClassName.khanjar,         0.40f ),
            ( WeenieClassName.khanjaracid,     0.15f ),
            ( WeenieClassName.khanjarelectric, 0.15f ),
            ( WeenieClassName.khanjarfire,     0.15f ),
            ( WeenieClassName.khanjarfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Clubs = new ChanceTable<WeenieClassName>()
        {
            // light - mace
            ( WeenieClassName.club,         0.40f ),
            ( WeenieClassName.clubacid,     0.15f ),
            ( WeenieClassName.clubelectric, 0.15f ),
            ( WeenieClassName.clubfire,     0.15f ),
            ( WeenieClassName.clubfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Kasrullahs = new ChanceTable<WeenieClassName>()
        {
            // light - mace
            ( WeenieClassName.kasrullah,         0.40f ),
            ( WeenieClassName.kasrullahacid,     0.15f ),
            ( WeenieClassName.kasrullahelectric, 0.15f ),
            ( WeenieClassName.kasrullahfire,     0.15f ),
            ( WeenieClassName.kasrullahfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> SpikedClubs = new ChanceTable<WeenieClassName>()
        {
            // light - mace
            ( WeenieClassName.clubspiked,         0.40f ),
            ( WeenieClassName.clubspikedacid,     0.15f ),
            ( WeenieClassName.clubspikedelectric, 0.15f ),
            ( WeenieClassName.clubspikedfire,     0.15f ),
            ( WeenieClassName.clubspikedfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Spears = new ChanceTable<WeenieClassName>()
        {
            // light - spear
            ( WeenieClassName.spear,         0.40f ),
            ( WeenieClassName.spearacid,     0.15f ),
            ( WeenieClassName.spearelectric, 0.15f ),
            ( WeenieClassName.spearflame,    0.15f ),
            ( WeenieClassName.spearfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Yaris = new ChanceTable<WeenieClassName>()
        {
            // light - spear
            ( WeenieClassName.yari,         0.40f ),
            ( WeenieClassName.yariacid,     0.15f ),
            ( WeenieClassName.yarielectric, 0.15f ),
            ( WeenieClassName.yarifire,     0.15f ),
            ( WeenieClassName.yarifrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> QuarterStaffs = new ChanceTable<WeenieClassName>()
        {
            // light - staff
            ( WeenieClassName.quarterstaffnew,         0.40f ),
            ( WeenieClassName.quarterstaffacidnew,     0.15f ),
            ( WeenieClassName.quarterstaffelectricnew, 0.15f ),
            ( WeenieClassName.quarterstaffflamenew,    0.15f ),
            ( WeenieClassName.quarterstafffrostnew,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> BroadSwords = new ChanceTable<WeenieClassName>()
        {
            // light - sword
            ( WeenieClassName.swordbroad,         0.40f ),
            ( WeenieClassName.swordbroadacid,     0.15f ),
            ( WeenieClassName.swordbroadelectric, 0.15f ),
            ( WeenieClassName.swordbroadfire,     0.15f ),
            ( WeenieClassName.swordbroadfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> DericostBlades = new ChanceTable<WeenieClassName>()
        {
            // light - sword
            ( WeenieClassName.ace31759_dericostblade,          0.40f ),
            ( WeenieClassName.ace31760_aciddericostblade,      0.15f ),
            ( WeenieClassName.ace31761_lightningdericostblade, 0.15f ),
            ( WeenieClassName.ace31762_flamingdericostblade,   0.15f ),
            ( WeenieClassName.ace31758_frostdericostblade,     0.15f ),
        };

        // epee?

        private static ChanceTable<WeenieClassName> Kaskaras = new ChanceTable<WeenieClassName>()
        {
            // light - sword
            ( WeenieClassName.kaskara,         0.40f ),
            ( WeenieClassName.kaskaraacid,     0.15f ),
            ( WeenieClassName.kaskaraelectric, 0.15f ),
            ( WeenieClassName.kaskarafire,     0.15f ),
            ( WeenieClassName.kaskarafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Shamshirs = new ChanceTable<WeenieClassName>()
        {
            // light - sword
            ( WeenieClassName.shamshir,         0.40f ),
            ( WeenieClassName.shamshiracid,     0.15f ),
            ( WeenieClassName.shamshirelectric, 0.15f ),
            ( WeenieClassName.shamshirfire,     0.15f ),
            ( WeenieClassName.shamshirfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Spadas = new ChanceTable<WeenieClassName>()
        {
            // light - sword
            ( WeenieClassName.swordspada,         0.40f ),
            ( WeenieClassName.swordspadaacid,     0.15f ),
            ( WeenieClassName.swordspadaelectric, 0.15f ),
            ( WeenieClassName.swordspadafire,     0.15f ),
            ( WeenieClassName.swordspadafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Katars = new ChanceTable<WeenieClassName>()
        {
            // light - unarmed
            ( WeenieClassName.katar,         0.40f ),
            ( WeenieClassName.kataracid,     0.15f ),
            ( WeenieClassName.katarelectric, 0.15f ),
            ( WeenieClassName.katarfire,     0.15f ),
            ( WeenieClassName.katarfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Knuckles = new ChanceTable<WeenieClassName>()
        {
            // light - unarmed
            ( WeenieClassName.knuckles,         0.40f ),
            ( WeenieClassName.knucklesacid,     0.15f ),
            ( WeenieClassName.knuckleselectric, 0.15f ),
            ( WeenieClassName.knucklesfire,     0.15f ),
            ( WeenieClassName.knucklesfrost,    0.15f ),
        };

        private static readonly List<(ChanceTable<WeenieClassName> table, TreasureWeaponType weaponType)> lightWeaponsTables = new List<(ChanceTable<WeenieClassName>, TreasureWeaponType)>()
        {
            ( Dolabras,       TreasureWeaponType.Axe ),
            ( HandAxes,       TreasureWeaponType.Axe ),
            ( Onos,           TreasureWeaponType.Axe ),
            ( WarHammers,     TreasureWeaponType.Axe ),
            ( Daggers,        TreasureWeaponType.DaggerMS ),
            ( Khanjars,       TreasureWeaponType.Dagger ),
            ( Clubs,          TreasureWeaponType.Mace ),
            ( Kasrullahs,     TreasureWeaponType.Mace ),
            ( SpikedClubs,    TreasureWeaponType.Mace ),
            ( Spears,         TreasureWeaponType.Spear ),
            ( Yaris,          TreasureWeaponType.Spear ),
            ( QuarterStaffs,  TreasureWeaponType.Staff ),
            ( BroadSwords,    TreasureWeaponType.Sword ),
            ( DericostBlades, TreasureWeaponType.Sword ),
            ( Kaskaras,       TreasureWeaponType.Sword ),
            ( Shamshirs,      TreasureWeaponType.Sword ),
            ( Spadas,         TreasureWeaponType.Sword ),
            ( Katars,         TreasureWeaponType.Unarmed ),
            ( Knuckles,       TreasureWeaponType.Unarmed ),
        };

        public static WeenieClassName Roll(out TreasureWeaponType weaponType)
        {
            // even chance of selecting each weapon table
            var weaponTable = lightWeaponsTables[ThreadSafeRandom.Next(0, lightWeaponsTables.Count - 1)];

            weaponType = weaponTable.weaponType;

            return weaponTable.table.Roll();
        }

        private static readonly Dictionary<WeenieClassName, TreasureWeaponType> _combined = new Dictionary<WeenieClassName, TreasureWeaponType>();

        static LightWeaponWcids()
        {
            foreach (var lightWeaponsTable in lightWeaponsTables)
            {
                foreach (var wcid in lightWeaponsTable.table)
                    _combined.TryAdd(wcid.result, lightWeaponsTable.weaponType);
            }
        }

        public static bool TryGetValue(WeenieClassName wcid, out TreasureWeaponType weaponType)
        {
            return _combined.TryGetValue(wcid, out weaponType);
        }
    }
}
