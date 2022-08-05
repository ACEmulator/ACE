using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class HeavyWeaponWcids
    {
        private static ChanceTable<WeenieClassName> BattleAxes = new ChanceTable<WeenieClassName>()
        {
            // heavy - axe
            ( WeenieClassName.axebattle,         0.40f ),
            ( WeenieClassName.axebattleacid,     0.15f ),
            ( WeenieClassName.axebattleelectric, 0.15f ),
            ( WeenieClassName.axebattlefire,     0.15f ),
            ( WeenieClassName.axebattlefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> LugianHammers = new ChanceTable<WeenieClassName>()
        {
            // heavy - axe
            ( WeenieClassName.ace31764_lugianhammer,          0.40f ),
            ( WeenieClassName.ace31765_acidlugianhammer,      0.15f ),
            ( WeenieClassName.ace31766_lightninglugianhammer, 0.15f ),
            ( WeenieClassName.ace31767_flaminglugianhammer,   0.15f ),
            ( WeenieClassName.ace31763_frostlugianhammer,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Silifis = new ChanceTable<WeenieClassName>()
        {
            // heavy - axe
            ( WeenieClassName.silifi,         0.40f ),
            ( WeenieClassName.silifiacid,     0.15f ),
            ( WeenieClassName.silifielectric, 0.15f ),
            ( WeenieClassName.silififire,     0.15f ),
            ( WeenieClassName.silififrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> WarAxes = new ChanceTable<WeenieClassName>()
        {
            // heavy - axe
            ( WeenieClassName.ace31769_waraxe,          0.40f ),
            ( WeenieClassName.ace31770_acidwaraxe,      0.15f ),
            ( WeenieClassName.ace31771_lightningwaraxe, 0.15f ),
            ( WeenieClassName.ace31772_flamingwaraxe,   0.15f ),
            ( WeenieClassName.ace31768_frostwaraxe,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Dirks = new ChanceTable<WeenieClassName>()
        {
            // heavy - dagger
            ( WeenieClassName.dirk,         0.40f ),
            ( WeenieClassName.dirkacid,     0.15f ),
            ( WeenieClassName.dirkelectric, 0.15f ),
            ( WeenieClassName.dirkfire,     0.15f ),
            ( WeenieClassName.dirkfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Jambiyas = new ChanceTable<WeenieClassName>()
        {
            // heavy - dagger (multi-strike)
            ( WeenieClassName.jambiya,         0.40f ),
            ( WeenieClassName.jambiyaacid,     0.15f ),
            ( WeenieClassName.jambiyaelectric, 0.15f ),
            ( WeenieClassName.jambiyafire,     0.15f ),
            ( WeenieClassName.jambiyafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Stilettos = new ChanceTable<WeenieClassName>()
        {
            // heavy - dagger (multi-strike)
            ( WeenieClassName.daggerstiletto,         0.40f ),
            ( WeenieClassName.daggerstilettoacid,     0.15f ),
            ( WeenieClassName.daggerstilettoelectric, 0.15f ),
            ( WeenieClassName.daggerstilettofire,     0.15f ),
            ( WeenieClassName.daggerstilettofrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> FlangedMaces = new ChanceTable<WeenieClassName>()
        {
            // heavy - mace
            ( WeenieClassName.maceflanged,         0.40f ),
            ( WeenieClassName.maceflangedacid,     0.15f ),
            ( WeenieClassName.maceflangedelectric, 0.15f ),
            ( WeenieClassName.maceflangedfire,     0.15f ),
            ( WeenieClassName.maceflangedfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Maces = new ChanceTable<WeenieClassName>()
        {
            // heavy - mace
            ( WeenieClassName.mace,         0.40f ),
            ( WeenieClassName.maceacid,     0.15f ),
            ( WeenieClassName.maceelectric, 0.15f ),
            ( WeenieClassName.macefire,     0.15f ),
            ( WeenieClassName.macefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Mazules = new ChanceTable<WeenieClassName>()
        {
            // heavy - mace
            ( WeenieClassName.macemazule,         0.40f ),
            ( WeenieClassName.macemazuleacid,     0.15f ),
            ( WeenieClassName.macemazuleelectric, 0.15f ),
            ( WeenieClassName.macemazulefire,     0.15f ),
            ( WeenieClassName.macemazulefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> MorningStars = new ChanceTable<WeenieClassName>()
        {
            // heavy - mace
            ( WeenieClassName.morningstar,         0.40f ),
            ( WeenieClassName.morningstaracid,     0.15f ),
            ( WeenieClassName.morningstarelectric, 0.15f ),
            ( WeenieClassName.morningstarfire,     0.15f ),
            ( WeenieClassName.morningstarfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Partizans = new ChanceTable<WeenieClassName>()
        {
            // heavy - spear
            ( WeenieClassName.spearpartizan,         0.40f ),
            ( WeenieClassName.spearpartizanacid,     0.15f ),
            ( WeenieClassName.spearpartizanelectric, 0.15f ),
            ( WeenieClassName.spearpartizanfire,     0.15f ),
            ( WeenieClassName.spearpartizanfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> SpineGlaives = new ChanceTable<WeenieClassName>()
        {
            // heavy - spear
            ( WeenieClassName.ace31779_spineglaive,         0.40f ),
            ( WeenieClassName.ace31780_acidspineglaive,     0.15f ),
            ( WeenieClassName.ace31781_electricspineglaive, 0.15f ),
            ( WeenieClassName.ace31782_firespineglaive,     0.15f ),
            ( WeenieClassName.ace31778_frostspineglaive,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Tridents = new ChanceTable<WeenieClassName>()
        {
            // heavy - spear
            ( WeenieClassName.trident,         0.40f ),
            ( WeenieClassName.tridentacid,     0.15f ),
            ( WeenieClassName.tridentelectric, 0.15f ),
            ( WeenieClassName.tridentfire,     0.15f ),
            ( WeenieClassName.tridentfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Nabuts = new ChanceTable<WeenieClassName>()
        {
            // heavy - staff
            ( WeenieClassName.nabutnew,         0.40f ),
            ( WeenieClassName.nabutacidnew,     0.15f ),
            ( WeenieClassName.nabutelectricnew, 0.15f ),
            ( WeenieClassName.nabutfirenew,     0.15f ),
            ( WeenieClassName.nabutfrostnew,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Sticks = new ChanceTable<WeenieClassName>()
        {
            // heavy - staff
            ( WeenieClassName.ace31788_stick,          0.40f ),
            ( WeenieClassName.ace31789_acidstick,      0.15f ),
            ( WeenieClassName.ace31790_lightningstick, 0.15f ),
            ( WeenieClassName.ace31791_flamingstick,   0.15f ),
            ( WeenieClassName.ace31792_froststick,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Flamberges = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.swordflamberge,         0.40f ),
            ( WeenieClassName.swordflambergeacid,     0.15f ),
            ( WeenieClassName.swordflambergeelectric, 0.15f ),
            ( WeenieClassName.swordflambergefire,     0.15f ),
            ( WeenieClassName.swordflambergefrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Kens = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.ken,         0.40f ),
            ( WeenieClassName.kenacid,     0.15f ),
            ( WeenieClassName.kenelectric, 0.15f ),
            ( WeenieClassName.kenfire,     0.15f ),
            ( WeenieClassName.kenfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> LongSwords = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.swordlong,         0.40f ),
            ( WeenieClassName.swordlongacid,     0.15f ),
            ( WeenieClassName.swordlongelectric, 0.15f ),
            ( WeenieClassName.swordlongfire,     0.15f ),
            ( WeenieClassName.swordlongfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Schlagers = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.ace45108_schlager,          0.40f ),
            ( WeenieClassName.ace45109_acidschlager,      0.15f ),
            ( WeenieClassName.ace45110_lightningschlager, 0.15f ),
            ( WeenieClassName.ace45111_flamingschlager,   0.15f ),
            ( WeenieClassName.ace45112_frostschlager,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Tachis = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.tachi,         0.40f ),
            ( WeenieClassName.tachiacid,     0.15f ),
            ( WeenieClassName.tachielectric, 0.15f ),
            ( WeenieClassName.tachifire,     0.15f ),
            ( WeenieClassName.tachifrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Takubas = new ChanceTable<WeenieClassName>()
        {
            // heavy - sword
            ( WeenieClassName.takuba,         0.40f ),
            ( WeenieClassName.takubaacid,     0.15f ),
            ( WeenieClassName.takubaelectric, 0.15f ),
            ( WeenieClassName.takubafire,     0.15f ),
            ( WeenieClassName.takubafrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Cestus = new ChanceTable<WeenieClassName>()
        {
            // heavy - unarmed
            ( WeenieClassName.cestus,         0.40f ),
            ( WeenieClassName.cestusacid,     0.15f ),
            ( WeenieClassName.cestuselectric, 0.15f ),
            ( WeenieClassName.cestusfire,     0.15f ),
            ( WeenieClassName.cestusfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> Nekodes = new ChanceTable<WeenieClassName>()
        {
            // heavy - unarmed
            ( WeenieClassName.nekode,         0.40f ),
            ( WeenieClassName.nekodeacid,     0.15f ),
            ( WeenieClassName.nekodeelectric, 0.15f ),
            ( WeenieClassName.nekodefire,     0.15f ),
            ( WeenieClassName.nekodefrost,    0.15f ),
        };

        private static readonly List<(ChanceTable<WeenieClassName> table, TreasureWeaponType weaponType)> heavyWeaponsTables = new List<(ChanceTable<WeenieClassName>, TreasureWeaponType)>()
        {
            ( BattleAxes,    TreasureWeaponType.Axe ),
            ( LugianHammers, TreasureWeaponType.Axe ),
            ( Silifis,       TreasureWeaponType.Axe ),
            ( WarAxes,       TreasureWeaponType.Axe ),
            ( Dirks,         TreasureWeaponType.Dagger ),
            ( Jambiyas,      TreasureWeaponType.DaggerMS ),
            ( Stilettos,     TreasureWeaponType.DaggerMS ),
            ( FlangedMaces,  TreasureWeaponType.Mace ),
            ( Maces,         TreasureWeaponType.Mace ),
            ( Mazules,       TreasureWeaponType.Mace ),
            ( MorningStars,  TreasureWeaponType.Mace ),
            ( Partizans,     TreasureWeaponType.Spear ),
            ( SpineGlaives,  TreasureWeaponType.Spear ),
            ( Tridents,      TreasureWeaponType.Spear ),
            ( Nabuts,        TreasureWeaponType.Staff ),
            ( Sticks,        TreasureWeaponType.Staff ),
            ( Flamberges,    TreasureWeaponType.Sword ),
            ( Kens,          TreasureWeaponType.Sword ),
            ( LongSwords,    TreasureWeaponType.Sword ),
            ( Schlagers,     TreasureWeaponType.SwordMS ),
            ( Tachis,        TreasureWeaponType.Sword ),
            ( Takubas,       TreasureWeaponType.Sword ),
            ( Cestus,        TreasureWeaponType.Unarmed ),
            ( Nekodes,       TreasureWeaponType.Unarmed ),
        };

        public static WeenieClassName Roll(out TreasureWeaponType weaponType)
        {
            // even chance of selecting each weapon table
            var weaponTable = heavyWeaponsTables[ThreadSafeRandom.Next(0, heavyWeaponsTables.Count - 1)];

            weaponType = weaponTable.weaponType;

            return weaponTable.table.Roll();
        }

        private static readonly Dictionary<WeenieClassName, TreasureWeaponType> _combined = new Dictionary<WeenieClassName, TreasureWeaponType>();

        static HeavyWeaponWcids()
        {
            foreach (var heavyWeaponsTable in heavyWeaponsTables)
            {
                foreach (var wcid in heavyWeaponsTable.table)
                    _combined.TryAdd(wcid.result, heavyWeaponsTable.weaponType);
            }
        }

        public static bool TryGetValue(WeenieClassName wcid, out TreasureWeaponType weaponType)
        {
            return _combined.TryGetValue(wcid, out weaponType);
        }
    }
}
