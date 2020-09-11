using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AtlatlWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatl,         0.50f ),
            ( WeenieClassName.atlatlroyal,    0.50f ),
            ( WeenieClassName.atlatlslashing, 0.00f ),
            ( WeenieClassName.atlatlpiercing, 0.00f ),
            ( WeenieClassName.atlatlblunt,    0.00f ),
            ( WeenieClassName.atlatlacid,     0.00f ),
            ( WeenieClassName.atlatlfire,     0.00f ),
            ( WeenieClassName.atlatlfrost,    0.00f ),
            ( WeenieClassName.atlatlelectric, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatl,         0.25f ),
            ( WeenieClassName.atlatlroyal,    0.26f ),
            ( WeenieClassName.atlatlslashing, 0.07f ),
            ( WeenieClassName.atlatlpiercing, 0.07f ),
            ( WeenieClassName.atlatlblunt,    0.07f ),
            ( WeenieClassName.atlatlacid,     0.07f ),
            ( WeenieClassName.atlatlfire,     0.07f ),
            ( WeenieClassName.atlatlfrost,    0.07f ),
            ( WeenieClassName.atlatlelectric, 0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatl,         0.00f ),
            ( WeenieClassName.atlatlroyal,    0.00f ),
            ( WeenieClassName.atlatlslashing, 0.15f ),
            ( WeenieClassName.atlatlpiercing, 0.15f ),
            ( WeenieClassName.atlatlblunt,    0.14f ),
            ( WeenieClassName.atlatlacid,     0.14f ),
            ( WeenieClassName.atlatlfire,     0.14f ),
            ( WeenieClassName.atlatlfrost,    0.14f ),
            ( WeenieClassName.atlatlelectric, 0.14f ),
        };
    }
}
