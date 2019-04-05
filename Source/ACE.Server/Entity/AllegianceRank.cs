using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public static class AllegianceTitle
    {
        public static string GetTitle(HeritageGroup heritage, Gender gender, uint rank)
        {
            switch (gender)
            {
                case Gender.Male:
                    return GetMaleTitle(heritage, rank);
                case Gender.Female:
                    return GetFemaleTitle(heritage, rank);
                default:
                    return "";
            }
        }

        public static string GetMaleTitle(HeritageGroup heritage, uint rank)
        {
            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return GetAluvianMaleTitle(rank);
                case HeritageGroup.Gharundim:
                    return GetGharundimMaleTitle(rank);
                case HeritageGroup.Sho:
                    return GetShoMaleTitle(rank);
                case HeritageGroup.Viamontian:
                    return GetViamontianMaleTitle(rank);
                case HeritageGroup.Shadowbound:
                case HeritageGroup.Penumbraen:
                    return GetShadowboundMaleTitle(rank);
                case HeritageGroup.Tumerok:
                    return GetTumerokTitle(rank);
                case HeritageGroup.Gearknight:
                    return GetGearknightTitle(rank);
                case HeritageGroup.Lugian:
                    return GetLugianTitle(rank);
                case HeritageGroup.Empyrean:
                    return GetEmpyreanMaleTitle(rank);
                case HeritageGroup.Undead:
                    return GetUndeadMaleTitle(rank);
                default:
                    return "";
            }
        }

        public static string GetFemaleTitle(HeritageGroup heritage, uint rank)
        {
            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return GetAluvianFemaleTitle(rank);
                case HeritageGroup.Gharundim:
                    return GetGharundimFemaleTitle(rank);
                case HeritageGroup.Sho:
                    return GetShoFemaleTitle(rank);
                case HeritageGroup.Viamontian:
                    return GetViamontianFemaleTitle(rank);
                case HeritageGroup.Shadowbound:
                case HeritageGroup.Penumbraen:
                    return GetShadowboundFemaleTitle(rank);
                case HeritageGroup.Tumerok:
                    return GetTumerokTitle(rank);
                case HeritageGroup.Gearknight:
                    return GetGearknightTitle(rank);
                case HeritageGroup.Lugian:
                    return GetLugianTitle(rank);
                case HeritageGroup.Empyrean:
                    return GetEmpyreanFemaleTitle(rank);
                case HeritageGroup.Undead:
                    return GetUndeadFemaleTitle(rank);
                default:
                    return "";
            }
        }

        public static string GetAluvianMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1:  return "Yeoman";
                case 2:  return "Baronet";
                case 3:  return "Baron";
                case 4:  return "Reeve";
                case 5:  return "Thane";
                case 6:  return "Ealdor";
                case 7:  return "Duke";
                case 8:  return "Aetheling";
                case 9:  return "King";
                case 10: return "High King";
                default: return "";
            }
        }

        public static string GetAluvianFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Yeoman";
                case 2: return "Baronet";
                case 3: return "Baroness";
                case 4: return "Reeve";
                case 5: return "Thane";
                case 6: return "Ealdor";
                case 7: return "Duchess";
                case 8: return "Aetheling";
                case 9: return "Queen";
                case 10: return "High Queen";
                default: return "";
            }
        }

        public static string GetGharundimMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Sayyid";
                case 2: return "Shayk";
                case 3: return "Maulan";
                case 4: return "Mu'allim";
                case 5: return "Naquib";
                case 6: return "Qadi";
                case 7: return "Mushir";
                case 8: return "Amir";
                case 9: return "Malik";
                case 10: return "Sultan";
                default: return "";
            }
        }

        public static string GetGharundimFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Sayyida";
                case 2: return "Shayka";
                case 3: return "Maulana";
                case 4: return "Mu'allima";
                case 5: return "Naquiba";
                case 6: return "Qadiya";
                case 7: return "Mushira";
                case 8: return "Amira";
                case 9: return "Malika";
                case 10: return "Sultana";
                default: return "";
            }
        }

        public static string GetShoMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Jinin";
                case 2: return "Jo-chueh";
                case 3: return "Nan-chueh";
                case 4: return "Shi-chueh";
                case 5: return "Ta-chueh";
                case 6: return "Kun-chueh";
                case 7: return "Kou";
                case 8: return "Taikou";
                case 9: return "Ou";
                case 10: return "Koutei";
                default: return "";
            }
        }

        public static string GetShoFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Jinin";
                case 2: return "Jo-chueh";
                case 3: return "Nan-chueh";
                case 4: return "Shi-chueh";
                case 5: return "Ta-chueh";
                case 6: return "Kun-chueh";
                case 7: return "Kou";
                case 8: return "Taikou";
                case 9: return "Jo-ou";
                case 10: return "Koutei";
                default: return "";
            }
        }

        public static string GetViamontianMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Squire";
                case 2: return "Banner";
                case 3: return "Baron";
                case 4: return "Viscount";
                case 5: return "Count";
                case 6: return "Marquis";
                case 7: return "Duke";
                case 8: return "Grand Duke";
                case 9: return "King";
                case 10: return "High King";
                default: return "";
            }
        }

        public static string GetViamontianFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Dame";
                case 2: return "Banner";
                case 3: return "Baroness";
                case 4: return "Viscountess";
                case 5: return "Countess";
                case 6: return "Marquise";
                case 7: return "Duchess";
                case 8: return "Grand Duchess";
                case 9: return "Queen";
                case 10: return "High Queen";
                default: return "";
            }
        }

        public static string GetShadowboundMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Tenebrous";
                case 2: return "Shade";
                case 3: return "Squire";
                case 4: return "Knight";
                case 5: return "Void Knight";
                case 6: return "Void Lord";
                case 7: return "Duke";
                case 8: return "Archduke";
                case 9: return "Highborn";
                case 10: return "King";
                default: return "";
            }
        }

        public static string GetShadowboundFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Tenebrous";
                case 2: return "Shade";
                case 3: return "Squire";
                case 4: return "Knight";
                case 5: return "Void Knight";
                case 6: return "Void Lady";
                case 7: return "Duchess";
                case 8: return "Archduchess";
                case 9: return "Highborn";
                case 10: return "Queen";
                default: return "";
            }
        }

        public static string GetTumerokTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Xutua";
                case 2: return "Tuona";
                case 3: return "Ona";
                case 4: return "Nuona";
                case 5: return "Turea";
                case 6: return "Rea";
                case 7: return "Nurea";
                case 8: return "Kauh";
                case 9: return "Sutah";
                case 10: return "Tah";
                default: return "";
            }
        }

        public static string GetGearknightTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Tribunus";
                case 2: return "Praefectus";
                case 3: return "Optio";
                case 4: return "Centurion";
                case 5: return "Principes";
                case 6: return "Legatus";
                case 7: return "Consul";
                case 8: return "Dux";
                case 9: return "Secondus";
                case 10: return "Primus";
                default: return "";
            }
        }

        public static string GetLugianTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Laigus";
                case 2: return "Raigus";
                case 3: return "Amploth";
                case 4: return "Arintoth";
                case 5: return "Obeloth";
                case 6: return "Lithos";
                case 7: return "Kantos";
                case 8: return "Gigas";
                case 9: return "Extas";
                case 10: return "Tiatus";
                default: return "";
            }
        }

        public static string GetEmpyreanMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Ensign";
                case 2: return "Corporal";
                case 3: return "Lieutenant";
                case 4: return "Commander";
                case 5: return "Captain";
                case 6: return "Commodore";
                case 7: return "Admiral";
                case 8: return "Warlord";
                case 9: return "Ipharsin";
                case 10: return "Aulin";
                default: return "";
            }
        }

        public static string GetEmpyreanFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Ensign";
                case 2: return "Corporal";
                case 3: return "Lieutenant";
                case 4: return "Commander";
                case 5: return "Captain";
                case 6: return "Commodore";
                case 7: return "Admiral";
                case 8: return "Warlord";
                case 9: return "Ipharsia";
                case 10: return "Aulia";
                default: return "";
            }
        }

        public static string GetUndeadMaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Neophyte";
                case 2: return "Acolyte";
                case 3: return "Adept";
                case 4: return "Esquire";
                case 5: return "Squire";
                case 6: return "Knight";
                case 7: return "Count";
                case 8: return "Viscount";
                case 9: return "Highness";
                case 10: return "Annointed";
                default: return "";
            }
        }

        public static string GetUndeadFemaleTitle(uint rank)
        {
            switch (rank)
            {
                case 1: return "Neophyte";
                case 2: return "Acolyte";
                case 3: return "Adept";
                case 4: return "Esquire";
                case 5: return "Squire";
                case 6: return "Knight";
                case 7: return "Countess";
                case 8: return "Viscountess";
                case 9: return "Highness";
                case 10: return "Annointed";
                default: return "";
            }
        }
    }
}
