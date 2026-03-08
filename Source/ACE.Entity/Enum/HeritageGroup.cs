namespace ACE.Entity.Enum
 {
     public enum HeritageGroup
     {
         Invalid     = 0,
         Aluvian     = 1,
         Gharundim   = 2,
         Sho         = 3,
         Viamontian  = 4,
         Shadowbound = 5,
         Gearknight  = 6,
         Tumerok     = 7,
         Lugian      = 8,
         Empyrean    = 9,
         Penumbraen  = 10,
         Undead      = 11,
         Olthoi      = 12,
         OlthoiAcid  = 13
     }

    public static class HeritageGroupExtensions
    {
        public static string ToSentence(this HeritageGroup heritageGroup)
        {
            switch (heritageGroup)
            {
                case HeritageGroup.Gharundim:
                    return "Gharu'ndim";
                case HeritageGroup.Shadowbound:
                    return "Umbraen";
                case HeritageGroup.OlthoiAcid:
                    return "Olthoi";
                default:
                    return heritageGroup.ToString();
            }
        }
    }
}
