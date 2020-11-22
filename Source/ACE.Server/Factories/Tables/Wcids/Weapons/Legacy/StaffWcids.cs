using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class StaffWcids
    {
        private static ChanceTable<WeenieClassName> StaffWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.quarterstaffnew,         0.40f ),
            ( WeenieClassName.quarterstaffacidnew,     0.15f ),
            ( WeenieClassName.quarterstaffelectricnew, 0.15f ),
            ( WeenieClassName.quarterstaffflamenew,    0.15f ),
            ( WeenieClassName.quarterstafffrostnew,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> StaffWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.nabutnew,         0.40f ),
            ( WeenieClassName.nabutacidnew,     0.15f ),
            ( WeenieClassName.nabutelectricnew, 0.15f ),
            ( WeenieClassName.nabutfirenew,     0.15f ),
            ( WeenieClassName.nabutfrostnew,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> StaffWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jonew,         0.40f ),
            ( WeenieClassName.joacidnew,     0.15f ),
            ( WeenieClassName.joelectricnew, 0.15f ),
            ( WeenieClassName.jofirenew,     0.15f ),
            ( WeenieClassName.jofrostnew,    0.15f ),
        };

        public static WeenieClassName Roll(TreasureHeritageGroup heritage)
        {
            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return StaffWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return StaffWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return StaffWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
