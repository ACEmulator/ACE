using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ArmorWcids
    {
        private static readonly ChanceTable<WeenieClassName> ChainmailWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldkitelarge,      0.04f ),
            ( WeenieClassName.shieldroundlarge,     0.04f ),
            ( WeenieClassName.capmetal,             0.02f ),
            ( WeenieClassName.mailcoif,             0.02f ),
            ( WeenieClassName.coifscale,            0.02f ),
            ( WeenieClassName.basinetchainmail,     0.02f ),
            ( WeenieClassName.bootssteeltoe,        0.08f ),
            ( WeenieClassName.bracerschainmail,     0.07f ),
            ( WeenieClassName.breastplatechainmail, 0.07f ),
            ( WeenieClassName.hauberkchainmail,     0.05f ),
            ( WeenieClassName.gauntletschainmail,   0.08f ),
            ( WeenieClassName.girthchainmail,       0.07f ),
            ( WeenieClassName.greaveschainmail,     0.07f ),
            ( WeenieClassName.leggingschainmail,    0.08f ),
            ( WeenieClassName.pauldronschainmail,   0.08f ),
            ( WeenieClassName.shirtchainmail,       0.05f ),
            ( WeenieClassName.sleeveschainmail,     0.06f ),
            ( WeenieClassName.tassetschainmail,     0.08f ),
        };

        private static readonly ChanceTable<WeenieClassName> CovenantWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldcovenant,      0.10f ),
            ( WeenieClassName.helmcovenant,        0.10f ),
            ( WeenieClassName.gauntletscovenant,   0.10f ),
            ( WeenieClassName.bracerscovenant,     0.10f ),
            ( WeenieClassName.pauldronscovenant,   0.10f ),
            ( WeenieClassName.breastplatecovenant, 0.10f ),
            ( WeenieClassName.girthcovenant,       0.10f ),
            ( WeenieClassName.tassetscovenant,     0.10f ),
            ( WeenieClassName.greavescovenant,     0.10f ),
            ( WeenieClassName.bootscovenant,       0.10f ),
        };

        private static readonly ChanceTable<WeenieClassName> LoricaWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bootslorica,       0.16f ),
            ( WeenieClassName.gauntletslorica,   0.16f ),
            ( WeenieClassName.helmlorica,        0.17f ),
            ( WeenieClassName.breastplatelorica, 0.17f ),
            ( WeenieClassName.leggingslorica,    0.17f ),
            ( WeenieClassName.sleeveslorica,     0.17f ),
        };

        private static readonly ChanceTable<WeenieClassName> NariyidWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bootsnariyid,       0.14f ),
            ( WeenieClassName.gauntletsnariyid,   0.14f ),
            ( WeenieClassName.helmnariyid,        0.14f ),
            ( WeenieClassName.breastplatenariyid, 0.14f ),
            ( WeenieClassName.girthnariyid,       0.14f ),
            ( WeenieClassName.leggingsnariyid,    0.15f ),
            ( WeenieClassName.sleevesnariyid,     0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> ChiranWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.sandalschiran,   0.20f ),
            ( WeenieClassName.gauntletschiran, 0.20f ),
            ( WeenieClassName.helmchiran,      0.20f ),
            ( WeenieClassName.coatchiran,      0.20f ),
            ( WeenieClassName.leggingschiran,  0.20f ),
        };

        private static readonly ChanceTable<WeenieClassName> CeldonWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.girthceldon,       0.25f ),
            ( WeenieClassName.breastplateceldon, 0.25f ),
            ( WeenieClassName.leggingsceldon,    0.25f ),
            ( WeenieClassName.sleevesceldon,     0.25f ),
        };

        private static readonly ChanceTable<WeenieClassName> AmuliWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.coatamullian,     0.50f ),
            ( WeenieClassName.leggingsamullian, 0.50f ),
        };

        private static readonly ChanceTable<WeenieClassName> KoujiaWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.breastplatekoujia, 0.33f ),
            ( WeenieClassName.leggingskoujia,    0.34f ),
            ( WeenieClassName.sleeveskoujia,     0.33f ),
        };

        private static readonly ChanceTable<WeenieClassName> LeatherWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.buckler,                 0.07f ),
            ( WeenieClassName.capleather,              0.02f ),
            ( WeenieClassName.cowlleathernew,          0.02f ),
            ( WeenieClassName.basinetleathernew,       0.03f ),
            ( WeenieClassName.bootsleathernew,         0.06f ),
            ( WeenieClassName.bracersleathernew,       0.06f ),
            ( WeenieClassName.breastplateleathernew,   0.06f ),
            ( WeenieClassName.coatleathernew,          0.03f ),
            ( WeenieClassName.cuirassleathernew,       0.06f ),
            ( WeenieClassName.gauntletsleathernew,     0.06f ),
            ( WeenieClassName.girthleathernew,         0.06f ),
            ( WeenieClassName.greavesleathernew,       0.05f ),
            ( WeenieClassName.leggingsleathernew,      0.05f ),
            ( WeenieClassName.longgauntletsleathernew, 0.06f ),
            ( WeenieClassName.pantsleathernew,         0.05f ),
            ( WeenieClassName.pauldronsleathernew,     0.06f ),
            ( WeenieClassName.shirtleathernew,         0.04f ),
            ( WeenieClassName.shortsleathernew,        0.05f ),
            ( WeenieClassName.sleevesleathernew,       0.06f ),
            ( WeenieClassName.tassetsleathernew,       0.05f ),
        };

        private static readonly ChanceTable<WeenieClassName> PlatemailWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldtower,          0.08f ),
            ( WeenieClassName.helmhorned,           0.02f ),
            ( WeenieClassName.helmet,               0.02f ),
            ( WeenieClassName.armet,                0.02f ),
            ( WeenieClassName.heaumenew,            0.02f ),
            ( WeenieClassName.sollerets,            0.08f ),
            ( WeenieClassName.vambracesplatemail,   0.06f ),
            ( WeenieClassName.breastplateplatemail, 0.08f ),
            ( WeenieClassName.cuirassplatemail,     0.08f ),
            ( WeenieClassName.gauntletsplatemail,   0.08f ),
            ( WeenieClassName.girthplatemail,       0.05f ),
            ( WeenieClassName.greavesplatemail,     0.07f ),
            ( WeenieClassName.tassetsplatemail,     0.07f ),
            ( WeenieClassName.hauberkplatemail,     0.06f ),
            ( WeenieClassName.leggingsplatemail,    0.08f ),
            ( WeenieClassName.pauldronsplatemail,   0.05f ),
            ( WeenieClassName.sleevesplatemail,     0.08f ),
        };

        private static readonly ChanceTable<WeenieClassName> ScalemailWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldtower,          0.07f ),
            ( WeenieClassName.baigha,               0.02f ),
            ( WeenieClassName.helmet,               0.02f ),
            ( WeenieClassName.armet,                0.02f ),
            ( WeenieClassName.heaumenew,            0.02f ),
            ( WeenieClassName.basinetscalemail,     0.01f ),
            ( WeenieClassName.sollerets,            0.07f ),
            ( WeenieClassName.bracersscalemail,     0.07f ),
            ( WeenieClassName.breastplatescalemail, 0.06f ),
            ( WeenieClassName.cuirassscalemail,     0.06f ),
            ( WeenieClassName.gauntletsscalemail,   0.07f ),
            ( WeenieClassName.girthscalemail,       0.06f ),
            ( WeenieClassName.greavesscalemail,     0.07f ),
            ( WeenieClassName.tassetsscalemail,     0.07f ),
            ( WeenieClassName.hauberkscalemail,     0.06f ),
            ( WeenieClassName.leggingsscalemail,    0.07f ),
            ( WeenieClassName.pauldronsscalemail,   0.06f ),
            ( WeenieClassName.sleevesscalemail,     0.06f ),
            ( WeenieClassName.shirtscalemail,       0.06f ),
        };

        private static readonly ChanceTable<WeenieClassName> YoroiWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldtower,        0.08f ),
            ( WeenieClassName.kabuton,            0.02f ),
            ( WeenieClassName.helmet,             0.02f ),
            ( WeenieClassName.armet,              0.02f ),
            ( WeenieClassName.heaumenew,          0.02f ),
            ( WeenieClassName.sollerets,          0.08f ),
            ( WeenieClassName.kote,               0.08f ),
            ( WeenieClassName.breastplateyoroi,   0.08f ),
            ( WeenieClassName.cuirassyoroi,       0.08f ),
            ( WeenieClassName.gauntletsplatemail, 0.06f ),
            ( WeenieClassName.girthyoroi,         0.08f ),
            ( WeenieClassName.greavesyoroi,       0.08f ),
            ( WeenieClassName.tassetsyoroi,       0.08f ),
            ( WeenieClassName.leggingsyoroi,      0.07f ),
            ( WeenieClassName.pauldronsyoroi,     0.08f ),
            ( WeenieClassName.sleevesyoroi,       0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> StuddedLeatherWcids = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shieldkite,                0.04f ),
            ( WeenieClassName.shieldround,               0.04f ),
            ( WeenieClassName.cowlstuddedleather,        0.04f ),
            ( WeenieClassName.basinetstuddedleather,     0.04f ),
            ( WeenieClassName.bootsreinforcedleather,    0.08f ),
            ( WeenieClassName.bracersstuddedleather,     0.07f ),
            ( WeenieClassName.breastplatestuddedleather, 0.07f ),
            ( WeenieClassName.coatstuddedleather,        0.03f ),
            ( WeenieClassName.cuirassstuddedleather,     0.06f ),
            ( WeenieClassName.gauntletsstuddedleather,   0.08f ),
            ( WeenieClassName.girthstuddedleather,       0.07f ),
            ( WeenieClassName.greavesstuddedleather,     0.07f ),
            ( WeenieClassName.leggingsstuddedleather,    0.07f ),
            ( WeenieClassName.pauldronsstuddedleather,   0.07f ),
            ( WeenieClassName.shirtstuddedleather,       0.04f ),
            ( WeenieClassName.sleevesstuddedleather,     0.06f ),
            ( WeenieClassName.tassetsstuddedleather,     0.07f ),
        };
    }
}
