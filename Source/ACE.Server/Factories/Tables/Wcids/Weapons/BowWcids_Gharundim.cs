using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class BowWcids_Gharundim
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.yag,         0.50f ),
            ( WeenieClassName.nayin,       0.50f ),
            ( WeenieClassName.bowslashing, 0.00f ),
            ( WeenieClassName.bowpiercing, 0.00f ),
            ( WeenieClassName.bowblunt,    0.00f ),
            ( WeenieClassName.bowacid,     0.00f ),
            ( WeenieClassName.bowfire,     0.00f ),
            ( WeenieClassName.bowfrost,    0.00f ),
            ( WeenieClassName.bowelectric, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.yag,         0.25f ),
            ( WeenieClassName.nayin,       0.26f ),
            ( WeenieClassName.bowslashing, 0.07f ),
            ( WeenieClassName.bowpiercing, 0.07f ),
            ( WeenieClassName.bowblunt,    0.07f ),
            ( WeenieClassName.bowacid,     0.07f ),
            ( WeenieClassName.bowfire,     0.07f ),
            ( WeenieClassName.bowfrost,    0.07f ),
            ( WeenieClassName.bowelectric, 0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.yag,         0.00f ),
            ( WeenieClassName.nayin,       0.00f ),
            ( WeenieClassName.bowslashing, 0.15f ),
            ( WeenieClassName.bowpiercing, 0.15f ),
            ( WeenieClassName.bowblunt,    0.14f ),
            ( WeenieClassName.bowacid,     0.14f ),
            ( WeenieClassName.bowfire,     0.14f ),
            ( WeenieClassName.bowfrost,    0.14f ),
            ( WeenieClassName.bowelectric, 0.14f ),
        };
    }
}
