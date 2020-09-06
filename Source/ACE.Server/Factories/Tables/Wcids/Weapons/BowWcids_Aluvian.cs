using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class BowWcids_Aluvian
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowshort,    0.5f ),
            ( WeenieClassName.bowlong,     0.5f ),
            ( WeenieClassName.bowslashing, 0f ),
            ( WeenieClassName.bowpiercing, 0f ),
            ( WeenieClassName.bowblunt,    0f ),
            ( WeenieClassName.bowacid,     0f ),
            ( WeenieClassName.bowfire,     0f ),
            ( WeenieClassName.bowfrost,    0f ),
            ( WeenieClassName.bowelectric, 0f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowshort,    0.25f ),
            ( WeenieClassName.bowlong,     0.26f ),
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
            ( WeenieClassName.bowshort,    0f ),
            ( WeenieClassName.bowlong,     0f ),
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
