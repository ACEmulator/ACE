using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class FinesseWeaponWcids
    {
        // hammers?

        private static ChanceTable<WeenieClassName> Hatchets = new ChanceTable<WeenieClassName>()
        {
            // finesse - axe
            ( WeenieClassName.axehatchet,         0.40f ),
            ( WeenieClassName.axehatchetacid,     0.15f ),
            ( WeenieClassName.axehatchetelectric, 0.15f ),
            ( WeenieClassName.axehatchetfire,     0.15f ),
            ( WeenieClassName.axehatchetfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Shouonos = new ChanceTable<WeenieClassName>()
        {
            // finesse - axe
            ( WeenieClassName.shouono,         0.40f ),
            ( WeenieClassName.shouonoacid,     0.15f ),
            ( WeenieClassName.shouonoelectric, 0.15f ),
            ( WeenieClassName.shouonofire,     0.15f ),
            ( WeenieClassName.shouonofrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Tungis = new ChanceTable<WeenieClassName>()
        {
            // finesse - axe
            ( WeenieClassName.tungi,         0.40f ),
            ( WeenieClassName.tungiacid,     0.15f ),
            ( WeenieClassName.tungielectric, 0.15f ),
            ( WeenieClassName.tungifire,     0.15f ),
            ( WeenieClassName.tungifrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Knives = new ChanceTable<WeenieClassName>()
        {
            // finesse - dagger (multi-strike)
            ( WeenieClassName.knife,         0.40f ),
            ( WeenieClassName.knifeacid,     0.15f ),
            ( WeenieClassName.knifeelectric, 0.15f ),
            ( WeenieClassName.knifefire,     0.15f ),
            ( WeenieClassName.knifefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Lancets = new ChanceTable<WeenieClassName>()
        {
            // finesse - dagger (multi-strike)
            ( WeenieClassName.ace31794_lancet,          0.40f ),
            ( WeenieClassName.ace31795_acidlancet,      0.15f ),
            ( WeenieClassName.ace31796_lightninglancet, 0.15f ),
            ( WeenieClassName.ace31797_flaminglancet,   0.15f ),
            ( WeenieClassName.ace31793_frostlancet,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Poniards = new ChanceTable<WeenieClassName>()
        {
            // finesse - dagger
            ( WeenieClassName.daggerponiard,         0.40f ),
            ( WeenieClassName.daggerponiardacid,     0.15f ),
            ( WeenieClassName.daggerponiardelectric, 0.15f ),
            ( WeenieClassName.daggerponiardfire,     0.15f ),
            ( WeenieClassName.daggerponiardfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> BoardsWithNails = new ChanceTable<WeenieClassName>()
        {
            // finesse - mace
            ( WeenieClassName.ace31774_boardwithnail,         0.40f ),
            ( WeenieClassName.ace31775_acidboardwithnail,     0.15f ),
            ( WeenieClassName.ace31776_electricboardwithnail, 0.15f ),
            ( WeenieClassName.ace31777_fireboardwithnail,     0.15f ),
            ( WeenieClassName.ace31773_frostboardwithnail,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Dabus = new ChanceTable<WeenieClassName>()
        {
            // finesse - mace
            ( WeenieClassName.dabus,         0.40f ),
            ( WeenieClassName.dabusacid,     0.15f ),
            ( WeenieClassName.dabuselectric, 0.15f ),
            ( WeenieClassName.dabusfire,     0.15f ),
            ( WeenieClassName.dabusfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Jittes = new ChanceTable<WeenieClassName>()
        {
            // finesse - mace
            ( WeenieClassName.jitte,         0.40f ),
            ( WeenieClassName.jitteacid,     0.15f ),
            ( WeenieClassName.jitteelectric, 0.15f ),
            ( WeenieClassName.jittefire,     0.15f ),
            ( WeenieClassName.jittefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Tofuns = new ChanceTable<WeenieClassName>()
        {
            // finesse - mace
            ( WeenieClassName.tofun,         0.40f ),
            ( WeenieClassName.tofunacid,     0.15f ),
            ( WeenieClassName.tofunelectric, 0.15f ),
            ( WeenieClassName.tofunfire,     0.15f ),
            ( WeenieClassName.tofunfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Budiaqs = new ChanceTable<WeenieClassName>()
        {
            // finesse - spear
            ( WeenieClassName.budiaq,         0.40f ),
            ( WeenieClassName.budiaqacid,     0.15f ),
            ( WeenieClassName.budiaqelectric, 0.15f ),
            ( WeenieClassName.budiaqfire,     0.15f ),
            ( WeenieClassName.budiaqfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Naginatas = new ChanceTable<WeenieClassName>()
        {
            // finesse - spear
            ( WeenieClassName.swordstaff,         0.40f ),
            ( WeenieClassName.swordstaffacid,     0.15f ),
            ( WeenieClassName.swordstaffelectric, 0.15f ),
            ( WeenieClassName.swordstafffire,     0.15f ),
            ( WeenieClassName.swordstafffrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Bastones = new ChanceTable<WeenieClassName>()
        {
            // finesse - staff
            ( WeenieClassName.staffmeleebastone,         0.40f ),
            ( WeenieClassName.staffmeleebastoneacid,     0.15f ),
            ( WeenieClassName.staffmeleebastoneelectric, 0.15f ),
            ( WeenieClassName.staffmeleebastonefire,     0.15f ),
            ( WeenieClassName.staffmeleebastonefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Jos = new ChanceTable<WeenieClassName>()
        {
            // finesse - staff
            ( WeenieClassName.jonew,         0.40f ),
            ( WeenieClassName.joacidnew,     0.15f ),
            ( WeenieClassName.joelectricnew, 0.15f ),
            ( WeenieClassName.jofirenew,     0.15f ),
            ( WeenieClassName.jofrostnew,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Sabras = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword
            ( WeenieClassName.swordsabra,         0.40f ),
            ( WeenieClassName.swordsabraacid,     0.15f ),
            ( WeenieClassName.swordsabraelectric, 0.15f ),
            ( WeenieClassName.swordsabrafire,     0.15f ),
            ( WeenieClassName.swordsabrafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Scimitars = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword
            ( WeenieClassName.scimitar,         0.40f ),
            ( WeenieClassName.scimitaracid,     0.15f ),
            ( WeenieClassName.scimitarelectric, 0.15f ),
            ( WeenieClassName.scimitarfire,     0.15f ),
            ( WeenieClassName.scimitarfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> ShortSwords = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword
            ( WeenieClassName.swordshort,         0.40f ),
            ( WeenieClassName.swordshortacid,     0.15f ),
            ( WeenieClassName.swordshortelectric, 0.15f ),
            ( WeenieClassName.swordshortfire,     0.15f ),
            ( WeenieClassName.swordshortfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Simis = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword
            ( WeenieClassName.simi,         0.40f ),
            ( WeenieClassName.simiacid,     0.15f ),
            ( WeenieClassName.simielectric, 0.15f ),
            ( WeenieClassName.simifire,     0.15f ),
            ( WeenieClassName.simifrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Rapiers = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword (multi-strike)
            ( WeenieClassName.swordrapier,              0.40f ),
            ( WeenieClassName.ace45104_acidrapier,      0.15f ),
            ( WeenieClassName.ace45105_lightningrapier, 0.15f ),
            ( WeenieClassName.ace45106_flamingrapier,   0.15f ),
            ( WeenieClassName.ace45107_frostrapier,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Yaojis = new ChanceTable<WeenieClassName>()
        {
            // finesse - sword
            ( WeenieClassName.yaoji,         0.40f ),
            ( WeenieClassName.yaojiacid,     0.15f ),
            ( WeenieClassName.yaojielectric, 0.15f ),
            ( WeenieClassName.yaojifire,     0.15f ),
            ( WeenieClassName.yaojifrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Claws = new ChanceTable<WeenieClassName>()
        {
            // finesse - unarmed
            ( WeenieClassName.ace31784_claw,          0.40f ),
            ( WeenieClassName.ace31785_acidclaw,      0.15f ),
            ( WeenieClassName.ace31786_lightningclaw, 0.15f ),
            ( WeenieClassName.ace31787_flamingclaw,   0.15f ),
            ( WeenieClassName.ace31783_frostclaw,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> HandWraps = new ChanceTable<WeenieClassName>()
        {
            // finesse - unarmed
            ( WeenieClassName.ace45118_handwraps,          0.40f ),
            ( WeenieClassName.ace45119_acidhandwraps,      0.15f ),
            ( WeenieClassName.ace45120_lightninghandwraps, 0.15f ),
            ( WeenieClassName.ace45121_flaminghandwraps,   0.15f ),
            ( WeenieClassName.ace45122_frosthandwraps,     0.15f ),
        };

        private static readonly List<(ChanceTable<WeenieClassName> table, TreasureWeaponType weaponType)> finesseWeaponsTables = new List<(ChanceTable<WeenieClassName>, TreasureWeaponType)>()
        {
            ( Hatchets,        TreasureWeaponType.Axe ),
            ( Shouonos,        TreasureWeaponType.Axe ),
            ( Tungis,          TreasureWeaponType.Axe ),
            ( Knives,          TreasureWeaponType.DaggerMS ),
            ( Lancets,         TreasureWeaponType.DaggerMS ),
            ( Poniards,        TreasureWeaponType.Dagger ),
            ( BoardsWithNails, TreasureWeaponType.Mace ),
            ( Dabus,           TreasureWeaponType.Mace ),
            ( Jittes,          TreasureWeaponType.MaceJitte ),
            ( Tofuns,          TreasureWeaponType.Mace ),
            ( Budiaqs,         TreasureWeaponType.Spear ),
            ( Naginatas,       TreasureWeaponType.Spear ),
            ( Bastones,        TreasureWeaponType.Staff ),
            ( Jos,             TreasureWeaponType.Staff ),
            ( Sabras,          TreasureWeaponType.Sword ),
            ( Scimitars,       TreasureWeaponType.Sword ),
            ( ShortSwords,     TreasureWeaponType.Sword ),
            ( Simis,           TreasureWeaponType.Sword ),
            ( Rapiers,         TreasureWeaponType.SwordMS ),
            ( Yaojis,          TreasureWeaponType.Sword ),
            ( Claws,           TreasureWeaponType.Unarmed ),
            ( HandWraps,       TreasureWeaponType.Unarmed ),
        };

        public static WeenieClassName Roll(out TreasureWeaponType weaponType)
        {
            // even chance of selecting each weapon table
            var weaponTable = finesseWeaponsTables[ThreadSafeRandom.Next(0, finesseWeaponsTables.Count - 1)];

            weaponType = weaponTable.weaponType;

            return weaponTable.table.Roll();
        }

        private static readonly Dictionary<WeenieClassName, TreasureWeaponType> _combined = new Dictionary<WeenieClassName, TreasureWeaponType>();

        static FinesseWeaponWcids()
        {
            foreach (var finesseWeaponsTable in finesseWeaponsTables)
            {
                foreach (var wcid in finesseWeaponsTable.table)
                    _combined.TryAdd(wcid.result, finesseWeaponsTable.weaponType);
            }
        }

        public static bool TryGetValue(WeenieClassName wcid, out TreasureWeaponType weaponType)
        {
            return _combined.TryGetValue(wcid, out weaponType);
        }
    }
}
