using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class SpellComponentWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   1.00f ),
            ( WeenieClassName.peascarabiron,   0.00f ),
            ( WeenieClassName.peascarabcopper, 0.00f ),
            ( WeenieClassName.peascarabsilver, 0.00f ),
            ( WeenieClassName.peascarabgold,   0.00f ),
            ( WeenieClassName.peascarabpyreal, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.50f ),
            ( WeenieClassName.peascarabiron,   0.50f ),
            ( WeenieClassName.peascarabcopper, 0.00f ),
            ( WeenieClassName.peascarabsilver, 0.00f ),
            ( WeenieClassName.peascarabgold,   0.00f ),
            ( WeenieClassName.peascarabpyreal, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.25f ),
            ( WeenieClassName.peascarabiron,   0.50f ),
            ( WeenieClassName.peascarabcopper, 0.25f ),
            ( WeenieClassName.peascarabsilver, 0.00f ),
            ( WeenieClassName.peascarabgold,   0.00f ),
            ( WeenieClassName.peascarabpyreal, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.00f ),
            ( WeenieClassName.peascarabiron,   0.25f ),
            ( WeenieClassName.peascarabcopper, 0.50f ),
            ( WeenieClassName.peascarabsilver, 0.25f ),
            ( WeenieClassName.peascarabgold,   0.00f ),
            ( WeenieClassName.peascarabpyreal, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.00f ),
            ( WeenieClassName.peascarabiron,   0.00f ),
            ( WeenieClassName.peascarabcopper, 0.25f ),
            ( WeenieClassName.peascarabsilver, 0.50f ),
            ( WeenieClassName.peascarabgold,   0.25f ),
            ( WeenieClassName.peascarabpyreal, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.00f ),
            ( WeenieClassName.peascarabiron,   0.00f ),
            ( WeenieClassName.peascarabcopper, 0.00f ),
            ( WeenieClassName.peascarabsilver, 0.25f ),
            ( WeenieClassName.peascarabgold,   0.50f ),
            ( WeenieClassName.peascarabpyreal, 0.25f ),
        };
    }
}
