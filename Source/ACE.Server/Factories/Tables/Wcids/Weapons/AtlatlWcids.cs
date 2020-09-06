using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AtlatlWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            (WeenieClassName.atlatl,         0.5f ),
            (WeenieClassName.atlatlroyal,    0.5f ),
            (WeenieClassName.atlatlslashing, 0f ),
            (WeenieClassName.atlatlpiercing, 0f ),
            (WeenieClassName.atlatlblunt,    0f ),
            (WeenieClassName.atlatlacid,     0f ),
            (WeenieClassName.atlatlfire,     0f ),
            (WeenieClassName.atlatlfrost,    0f ),
            (WeenieClassName.atlatlelectric, 0f ),
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
            ( WeenieClassName.atlatl,         0f ),
            ( WeenieClassName.atlatlroyal,    0f ),
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
