using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class CrossbowWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowlight,    0.50f ),
            ( WeenieClassName.crossbowheavy,    0.50f ),
            ( WeenieClassName.crossbowslashing, 0.00f ),
            ( WeenieClassName.crossbowpiercing, 0.00f ),
            ( WeenieClassName.crossbowblunt,    0.00f ),
            ( WeenieClassName.crossbowacid,     0.00f ),
            ( WeenieClassName.crossbowfire,     0.00f ),
            ( WeenieClassName.crossbowfrost,    0.00f ),
            ( WeenieClassName.crossbowelectric, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowlight,    0.25f ),
            ( WeenieClassName.crossbowheavy,    0.26f ),
            ( WeenieClassName.crossbowslashing, 0.07f ),
            ( WeenieClassName.crossbowpiercing, 0.07f ),
            ( WeenieClassName.crossbowblunt,    0.07f ),
            ( WeenieClassName.crossbowacid,     0.07f ),
            ( WeenieClassName.crossbowfire,     0.07f ),
            ( WeenieClassName.crossbowfrost,    0.07f ),
            ( WeenieClassName.crossbowelectric, 0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowlight,    0.00f ),
            ( WeenieClassName.crossbowheavy,    0.00f ),
            ( WeenieClassName.crossbowslashing, 0.15f ),
            ( WeenieClassName.crossbowpiercing, 0.15f ),
            ( WeenieClassName.crossbowblunt,    0.14f ),
            ( WeenieClassName.crossbowacid,     0.14f ),
            ( WeenieClassName.crossbowfire,     0.14f ),
            ( WeenieClassName.crossbowfrost,    0.14f ),
            ( WeenieClassName.crossbowelectric, 0.14f ),
        };
    }
}
