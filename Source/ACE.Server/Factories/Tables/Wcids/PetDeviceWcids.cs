using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class PetDeviceWcids
    {
        // necromancer - skeletons

        private static readonly List<WeenieClassName> fireSkeletons = new List<WeenieClassName>()
        {
            WeenieClassName.ace48942_fireskeletonminionessence50,
            WeenieClassName.ace48944_fireskeletonminionessence80,
            WeenieClassName.ace48945_fireskeletonminionessence100,
            WeenieClassName.ace48946_fireskeletonbushiessence125,
            WeenieClassName.ace48947_fireskeletonbushiessence150,
            WeenieClassName.ace48948_fireskeletonbushiessence180,
            WeenieClassName.ace48956_fireskeletonsamuraiessence200,
        };

        private static readonly List<WeenieClassName> acidSkeletons = new List<WeenieClassName>()
        {
            WeenieClassName.ace49213_acidskeletonminionessence50,
            WeenieClassName.ace49214_acidskeletonminionessence80,
            WeenieClassName.ace49215_acidskeletonminionessence100,
            WeenieClassName.ace49216_acidskeletonbushiessence125,
            WeenieClassName.ace49217_acidskeletonbushiessence150,
            WeenieClassName.ace49218_acidskeletonbushiessence180,
            WeenieClassName.ace49219_acidskeletonsamuraiessence200,
        };

        private static readonly List<WeenieClassName> lightningSkeletons = new List<WeenieClassName>()
        {
            WeenieClassName.ace49220_lightningskeletonminionessence50,
            WeenieClassName.ace49221_lightningskeletonminionessence80,
            WeenieClassName.ace49222_lightningskeletonminionessence100,
            WeenieClassName.ace49223_lightningskeletonbushiessence125,
            WeenieClassName.ace49224_lightningskeletonbushiessence150,
            WeenieClassName.ace49225_lightningskeletonbushiessence180,
            WeenieClassName.ace49226_lightningskeletonsamuraiessence200,
        };

        private static readonly List<WeenieClassName> frostSkeletons = new List<WeenieClassName>()
        {
            WeenieClassName.ace49227_frostskeletonminionessence50,
            WeenieClassName.ace49228_frostskeletonminionessence80,
            WeenieClassName.ace49229_frostskeletonminionessence100,
            WeenieClassName.ace49230_frostskeletonbushiessence125,
            WeenieClassName.ace49231_frostskeletonbushiessence150,
            WeenieClassName.ace49232_frostskeletonbushiessence180,
            WeenieClassName.ace49212_frostskeletonsamuraiessence200,
        };

        // necromancer - zombies

        private static readonly List<WeenieClassName> acidZombies = new List<WeenieClassName>()
        {
            WeenieClassName.ace48972_acidzombieessence50,
            WeenieClassName.ace49234_acidzombieessence80,
            WeenieClassName.ace49235_acidzombieessence100,
            WeenieClassName.ace49236_acidzombieessence125,
            WeenieClassName.ace49237_acidzombieessence150,
            WeenieClassName.ace49238_acidzombieessence180,
            WeenieClassName.ace49239_blisteredzombieessence200,
        };

        private static readonly List<WeenieClassName> lightningZombies = new List<WeenieClassName>()
        {
            WeenieClassName.ace49240_lightningzombieessence50,
            WeenieClassName.ace49241_lightningzombieessence80,
            WeenieClassName.ace49242_lightningzombieessence100,
            WeenieClassName.ace49243_lightningzombieessence125,
            WeenieClassName.ace49244_lightningzombieessence150,
            WeenieClassName.ace49245_lightningzombieessence180,
            WeenieClassName.ace49246_shockedzombieessence200,
        };

        private static readonly List<WeenieClassName> fireZombies = new List<WeenieClassName>()
        {
            WeenieClassName.ace49247_firezombieessence50,
            WeenieClassName.ace49248_firezombieessence80,
            WeenieClassName.ace49249_firezombieessence100,
            WeenieClassName.ace49250_firezombieessence125,
            WeenieClassName.ace49251_firezombieessence150,
            WeenieClassName.ace49252_firezombieessence180,
            WeenieClassName.ace49253_charredzombieessence200,
        };

        private static readonly List<WeenieClassName> frostZombies = new List<WeenieClassName>()
        {
            WeenieClassName.ace49254_frostzombieessence50,
            WeenieClassName.ace49255_frostzombieessence80,
            WeenieClassName.ace49256_frostzombieessence100,
            WeenieClassName.ace49257_frostzombieessence125,
            WeenieClassName.ace49258_frostzombieessence150,
            WeenieClassName.ace49259_frostzombieessence180,
            WeenieClassName.ace49233_frigidzombieessence200,
        };

        // necromancer - spectres

        private static readonly List<WeenieClassName> acidSpectres = new List<WeenieClassName>()
        {
            WeenieClassName.ace49421_acidspectreessence50,
            WeenieClassName.ace49422_acidspectreessence80,
            WeenieClassName.ace49423_acidspectreessence100,
            WeenieClassName.ace49424_acidspectreessence125,
            WeenieClassName.ace49425_acidspectreessence150,
            WeenieClassName.ace49426_acidspectreessence180,
            WeenieClassName.ace49427_acidmaidenessence200,
        };

        private static readonly List<WeenieClassName> lightningSpectres = new List<WeenieClassName>()
        {
            WeenieClassName.ace49428_lightningspectreessence50,
            WeenieClassName.ace49429_lightningspectreessence80,
            WeenieClassName.ace49430_lightningspectreessence100,
            WeenieClassName.ace49431_lightningspectreessence125,
            WeenieClassName.ace49432_lightningspectreessence150,
            WeenieClassName.ace49433_lightningspectreessence180,
            WeenieClassName.ace49434_lightningmaidenessence200,
        };

        private static readonly List<WeenieClassName> fireSpectres = new List<WeenieClassName>()
        {
            WeenieClassName.ace49435_firespectreessence50,
            WeenieClassName.ace49436_firespectreessence80,
            WeenieClassName.ace49437_firespectreessence100,
            WeenieClassName.ace49438_firespectreessence125,
            WeenieClassName.ace49439_firespectreessence150,
            WeenieClassName.ace49440_firespectreessence180,
            WeenieClassName.ace49441_firemaidenessence200,
        };

        private static readonly List<WeenieClassName> frostSpectres = new List<WeenieClassName>()
        {
            WeenieClassName.ace49442_frostspectreessence50,
            WeenieClassName.ace49443_frostspectreessence80,
            WeenieClassName.ace49444_frostspectreessence100,
            WeenieClassName.ace49445_frostspectreessence125,
            WeenieClassName.ace49446_frostspectreessence150,
            WeenieClassName.ace49447_frostspectreessence180,
            WeenieClassName.ace49448_frostmaidenessence200,
        };

        // primalist - elementals

        private static readonly List<WeenieClassName> fireElementals = new List<WeenieClassName>()
        {
            WeenieClassName.ace48959_fireelementalessence50,
            WeenieClassName.ace48961_fireelementalessence80,
            WeenieClassName.ace48963_fireelementalessence100,
            WeenieClassName.ace48965_firechildessence125,
            WeenieClassName.ace48967_firechildessence150,
            WeenieClassName.ace48969_firechildessence180,
            WeenieClassName.ace48957_incendiaryknightessence200,
        };

        private static readonly List<WeenieClassName> acidElementals = new List<WeenieClassName>()
        {
            WeenieClassName.ace49261_acidelementalessence50,
            WeenieClassName.ace49262_acidelementalessence80,
            WeenieClassName.ace49263_acidelementalessence100,
            WeenieClassName.ace49264_acidchildessence125,
            WeenieClassName.ace49265_acidchildessence150,
            WeenieClassName.ace49266_acidchildessence180,
            WeenieClassName.ace49267_causticknightessence200,
        };

        private static readonly List<WeenieClassName> lightningElementals = new List<WeenieClassName>()
        {
            WeenieClassName.ace49268_lightningelementalessence50,
            WeenieClassName.ace49269_lightningelementalessence80,
            WeenieClassName.ace49270_lightningelementalessence100,
            WeenieClassName.ace49271_lightningchildessence125,
            WeenieClassName.ace49272_lightningchildessence150,
            WeenieClassName.ace49273_lightningchildessence180,
            WeenieClassName.ace49274_galvanicknightessence200,
        };

        private static readonly List<WeenieClassName> frostElementals = new List<WeenieClassName>()
        {
            WeenieClassName.ace49275_frostelementalessence50,
            WeenieClassName.ace49276_frostelementalessence80,
            WeenieClassName.ace49277_frostelementalessence100,
            WeenieClassName.ace49278_frostchildessence125,
            WeenieClassName.ace49279_frostchildessence150,
            WeenieClassName.ace49280_frostchildessence180,
            WeenieClassName.ace49260_glacialknightessence200,
        };

        // naturalist - k'naths

        private static readonly List<WeenieClassName> acidKnaths = new List<WeenieClassName>()
        {
            WeenieClassName.ace49282_acidknathessence50,
            WeenieClassName.ace49283_acidknathessence80,
            WeenieClassName.ace49284_acidknathessence100,
            WeenieClassName.ace49285_acidknathessence125,
            WeenieClassName.ace49286_acidknathessence150,
            WeenieClassName.ace49287_acidknathessence180,
            WeenieClassName.ace49281_knathrajedessence200,
        };

        private static readonly List<WeenieClassName> lightningKnaths = new List<WeenieClassName>()
        {
            WeenieClassName.ace49289_lightningknathessence50,
            WeenieClassName.ace49290_lightningknathessence80,
            WeenieClassName.ace49291_lightningknathessence100,
            WeenieClassName.ace49292_lightningknathessence125,
            WeenieClassName.ace49293_lightningknathessence150,
            WeenieClassName.ace49294_lightningknathessence180,
            WeenieClassName.ace49288_knathyndaessence200,
        };

        private static readonly List<WeenieClassName> fireKnaths = new List<WeenieClassName>()
        {
            WeenieClassName.ace49296_fireknathessence50,
            WeenieClassName.ace49297_fireknathessence80,
            WeenieClassName.ace49298_fireknathessence100,
            WeenieClassName.ace49299_fireknathessence125,
            WeenieClassName.ace49300_fireknathessence150,
            WeenieClassName.ace49301_fireknathessence180,
            WeenieClassName.ace49295_knathtsoctessence200,
        };

        private static readonly List<WeenieClassName> frostKnaths = new List<WeenieClassName>()
        {
            WeenieClassName.ace49303_frostknathessence50,
            WeenieClassName.ace49304_frostknathessence80,
            WeenieClassName.ace49305_frostknathessence100,
            WeenieClassName.ace49306_frostknathessence125,
            WeenieClassName.ace49307_frostknathessence150,
            WeenieClassName.ace49308_frostknathessence180,
            WeenieClassName.ace49302_knathborretessence200,
        };

        // primalist - wisps

        private static readonly List<WeenieClassName> acidWisps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49310_acidwispessence50,
            WeenieClassName.ace49311_acidwispessence80,
            WeenieClassName.ace49312_acidwispessence100,
            WeenieClassName.ace49313_acidwispessence125,
            WeenieClassName.ace49314_acidwispessence150,
            WeenieClassName.ace49315_acidwispessence180,
            WeenieClassName.ace49316_corrosionwispessence200,
        };

        private static readonly List<WeenieClassName> lightningWisps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49317_lightningwispessence50,
            WeenieClassName.ace49318_lightningwispessence80,
            WeenieClassName.ace49319_lightningwispessence100,
            WeenieClassName.ace49320_lightningwispessence125,
            WeenieClassName.ace49321_lightningwispessence150,
            WeenieClassName.ace49322_lightningwispessence180,
            WeenieClassName.ace49323_voltiacwispessence200,
        };

        private static readonly List<WeenieClassName> fireWisps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49324_firewispessence50,
            WeenieClassName.ace49325_firewispessence80,
            WeenieClassName.ace49326_firewispessence100,
            WeenieClassName.ace49327_firewispessence125,
            WeenieClassName.ace49328_firewispessence150,
            WeenieClassName.ace49329_firewispessence180,
            WeenieClassName.ace49330_incendiarywispessence200,
        };

        private static readonly List<WeenieClassName> frostWisps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49331_frostwispessence50,
            WeenieClassName.ace49332_frostwispessence80,
            WeenieClassName.ace49333_frostwispessence100,
            WeenieClassName.ace49334_frostwispessence125,
            WeenieClassName.ace49335_frostwispessence150,
            WeenieClassName.ace49336_frostwispessence180,
            WeenieClassName.ace49309_blizzardwispessence200,
        };

        // naturalist - moars

        private static readonly List<WeenieClassName> acidMoars = new List<WeenieClassName>()
        {
            WeenieClassName.ace49338_acidmoaressence50,
            WeenieClassName.ace49339_acidmoaressence80,
            WeenieClassName.ace49340_acidmoaressence100,
            WeenieClassName.ace49341_acidmoaressence125,
            WeenieClassName.ace49342_acidmoaressence150,
            WeenieClassName.ace49343_acidmoaressence180,
            WeenieClassName.ace49344_blisteringmoaressence200,
        };

        private static readonly List<WeenieClassName> lightningMoars = new List<WeenieClassName>()
        {
            WeenieClassName.ace49345_lightningmoaressence50,
            WeenieClassName.ace49346_lightningmoaressence80,
            WeenieClassName.ace49347_lightningmoaressence100,
            WeenieClassName.ace49348_lightningmoaressence125,
            WeenieClassName.ace49349_lightningmoaressence150,
            WeenieClassName.ace49350_lightningmoaressence180,
            WeenieClassName.ace49351_electrifiedmoaressence200,
        };

        private static readonly List<WeenieClassName> fireMoars = new List<WeenieClassName>()
        {
            WeenieClassName.ace49352_firemoaressence50,
            WeenieClassName.ace49353_firemoaressence80,
            WeenieClassName.ace49354_firemoaressence100,
            WeenieClassName.ace49355_firemoaressence125,
            WeenieClassName.ace49356_firemoaressence150,
            WeenieClassName.ace49357_firemoaressence180,
            WeenieClassName.ace49358_volcanicmoaressence200,
        };

        private static readonly List<WeenieClassName> frostMoars = new List<WeenieClassName>()
        {
            WeenieClassName.ace49359_frostmoaressence50,
            WeenieClassName.ace49360_frostmoaressence80,
            WeenieClassName.ace49361_frostmoaressence100,
            WeenieClassName.ace49362_frostmoaressence125,
            WeenieClassName.ace49363_frostmoaressence150,
            WeenieClassName.ace49364_frostmoaressence180,
            WeenieClassName.ace49337_freezingmoaressence200,
        };

        // naturalist - grievvers

        private static readonly List<WeenieClassName> acidGrievvers = new List<WeenieClassName>()
        {
            WeenieClassName.ace49366_acidgrievveressence50,
            WeenieClassName.ace49367_acidgrievveressence80,
            WeenieClassName.ace49368_acidgrievveressence100,
            WeenieClassName.ace49369_acidgrievveressence125,
            WeenieClassName.ace49370_acidgrievveressence150,
            WeenieClassName.ace49371_acidgrievveressence180,
            WeenieClassName.ace49372_causticgrievveressence200,
        };

        private static readonly List<WeenieClassName> lightningGrievvers = new List<WeenieClassName>()
        {
            WeenieClassName.ace49373_lightninggrievveressence50,
            WeenieClassName.ace49374_lightninggrievveressence80,
            WeenieClassName.ace49375_lightninggrievveressence100,
            WeenieClassName.ace49376_lightninggrievveressence125,
            WeenieClassName.ace49377_lightninggrievveressence150,
            WeenieClassName.ace49378_lightninggrievveressence180,
            WeenieClassName.ace49379_excitedgrievveressence200,
        };

        private static readonly List<WeenieClassName> fireGrievvers = new List<WeenieClassName>()
        {
            WeenieClassName.ace49380_firegrievveressence50,
            WeenieClassName.ace49381_firegrievveressence80,
            WeenieClassName.ace49382_firegrievveressence100,
            WeenieClassName.ace49383_firegrievveressence125,
            WeenieClassName.ace49384_firegrievveressence150,
            WeenieClassName.ace49385_firegrievveressence180,
            WeenieClassName.ace49386_scorchedgrievveressence200,
        };

        private static readonly List<WeenieClassName> frostGrievvers = new List<WeenieClassName>()
        {
            WeenieClassName.ace49387_frostgrievveressence50,
            WeenieClassName.ace49388_frostgrievveressence80,
            WeenieClassName.ace49389_frostgrievveressence100,
            WeenieClassName.ace49390_frostgrievveressence125,
            WeenieClassName.ace49391_frostgrievveressence150,
            WeenieClassName.ace49392_frostgrievveressence180,
            WeenieClassName.ace49365_arcticgrievveressence200,
        };

        // naturalist - phyntos wasps

        private static readonly List<WeenieClassName> acidWasps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49524_acidphyntoswaspessence50,
            WeenieClassName.ace49525_acidphyntoswaspessence80,
            WeenieClassName.ace49526_acidphyntoswaspessence100,
            WeenieClassName.ace49527_acidphyntoswaspessence125,
            WeenieClassName.ace49528_acidphyntoswaspessence150,
            WeenieClassName.ace49529_acidphyntoswaspessence180,
            WeenieClassName.ace49530_acidphyntosswarmessence200,
        };

        private static readonly List<WeenieClassName> fireWasps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49531_firephyntoswaspessence50,
            WeenieClassName.ace49532_firephyntoswaspessence80,
            WeenieClassName.ace49533_firephyntoswaspessence100,
            WeenieClassName.ace49534_firephyntoswaspessence125,
            WeenieClassName.ace49535_firephyntoswaspessence150,
            WeenieClassName.ace49536_firephyntoswaspessence180,
            WeenieClassName.ace49537_firephyntosswarmessence200,
        };

        private static readonly List<WeenieClassName> frostWasps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49538_frostphyntoswaspessence50,
            WeenieClassName.ace49539_frostphyntoswaspessence80,
            WeenieClassName.ace49540_frostphyntoswaspessence100,
            WeenieClassName.ace49541_frostphyntoswaspessence125,
            WeenieClassName.ace49542_frostphyntoswaspessence150,
            WeenieClassName.ace49543_frostphyntoswaspessence180,
            WeenieClassName.ace49544_frostphyntosswarmessence200,
        };

        private static readonly List<WeenieClassName> lightningWasps = new List<WeenieClassName>()
        {
            WeenieClassName.ace49545_lightningphyntoswaspessence50,
            WeenieClassName.ace49546_lightningphyntoswaspessence80,
            WeenieClassName.ace49547_lightningphyntoswaspessence100,
            WeenieClassName.ace49548_lightningphyntoswaspessence125,
            WeenieClassName.ace49549_lightningphyntoswaspessence150,
            WeenieClassName.ace49550_lightningphyntoswaspessence180,
            WeenieClassName.ace49551_lightningphyntosswarmessence200,
        };

        private static readonly List<List<WeenieClassName>> Necromancer_PetDevices = new List<List<WeenieClassName>>()
        {
            fireSkeletons,
            acidSkeletons,
            lightningSkeletons,
            frostSkeletons,

            acidZombies,
            lightningZombies,
            fireZombies,
            frostZombies,

            acidSpectres,
            lightningSpectres,
            fireSpectres,
            frostSpectres,
        };

        private static readonly List<List<WeenieClassName>> Primalist_PetDevices = new List<List<WeenieClassName>>()
        {
            fireElementals,
            acidElementals,
            lightningElementals,
            frostElementals,

            acidKnaths,
            lightningKnaths,
            fireKnaths,
            frostKnaths,

            acidWisps,
            lightningWisps,
            fireWisps,
            frostWisps,
        };

        private static readonly List<List<WeenieClassName>> Naturalist_PetDevices = new List<List<WeenieClassName>>()
        {
            acidMoars,
            lightningMoars,
            fireMoars,
            frostMoars,

            acidGrievvers,
            lightningGrievvers,
            fireGrievvers,
            frostGrievvers,

            acidWasps,
            fireWasps,
            frostWasps,
            lightningWasps,
        };

        private static readonly Dictionary<int, int> petLevelIndexes = new Dictionary<int, int>()
        {
            { 50, 0 },
            { 80, 1 },
            { 100, 2 },
            { 125, 3 },
            { 150, 4 },
            { 180, 5 },
            { 200, 6 }
        };

        private static readonly List<List<WeenieClassName>> petDevices = Necromancer_PetDevices.Union(Primalist_PetDevices).Union(Naturalist_PetDevices).ToList();

        public static WeenieClassName Roll(TreasureDeath profile)
        {
            // roll for pet level
            var petLevel = PetDeviceChance.Roll(profile);

            // even chance of rolling each pet device type
            var rng = ThreadSafeRandom.Next(0, petDevices.Count - 1);

            var table = petDevices[rng];

            var petLevelIdx = petLevelIndexes[petLevel];

            return table[petLevelIdx];
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static PetDeviceWcids()
        {
            foreach (var petDevice in petDevices)
            {
                foreach (var wcid in petDevice)
                    _combined.Add(wcid);
            }
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
