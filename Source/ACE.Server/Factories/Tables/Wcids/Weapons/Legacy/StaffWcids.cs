using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class StaffWcids
    {
        private static readonly ChanceTable<WeenieClassName> StaffWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.quarterstaffnew,         0.40f ),
            ( WeenieClassName.quarterstaffacidnew,     0.15f ),
            ( WeenieClassName.quarterstaffelectricnew, 0.15f ),
            ( WeenieClassName.quarterstaffflamenew,    0.15f ),
            ( WeenieClassName.quarterstafffrostnew,    0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> StaffWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.nabutnew,         0.40f ),
            ( WeenieClassName.nabutacidnew,     0.15f ),
            ( WeenieClassName.nabutelectricnew, 0.15f ),
            ( WeenieClassName.nabutfirenew,     0.15f ),
            ( WeenieClassName.nabutfrostnew,    0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> StaffWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jonew,         0.40f ),
            ( WeenieClassName.joacidnew,     0.15f ),
            ( WeenieClassName.joelectricnew, 0.15f ),
            ( WeenieClassName.jofirenew,     0.15f ),
            ( WeenieClassName.jofrostnew,    0.15f ),
        };

        public static WeenieClassName Roll(HeritageGroup heritage)
        {
            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return StaffWcids_Aluvian.Roll();

                case HeritageGroup.Gharundim:
                    return StaffWcids_Gharundim.Roll();

                case HeritageGroup.Sho:
                    return StaffWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
