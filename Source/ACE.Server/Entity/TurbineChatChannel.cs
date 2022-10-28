namespace ACE.Server.Entity
{
    public static class TurbineChatChannel
    {
        public static uint Allegiance           = 1;

        public static uint General              = 2;
        public static uint Trade                = 3;
        public static uint LFG                  = 4;
        public static uint Roleplay             = 5;

        public static uint Society              = 6;

        public static uint SocietyCelestialHand = 7;
        public static uint SocietyEldrytchWeb   = 8;
        public static uint SocietyRadiantBlood  = 9;

        public static uint Olthoi               = 10;
    }

    public enum TurbineChatChannel_Enum : uint
    {
        Allegiance = 1,
        General = 2,
        Trade = 3,
        LFG = 4,
        Roleplay = 5,
        Olthoi = 6,
        Society = 7,
        SocietyCelestialHand = 8,
        SocietyEldrytchWeb = 9,
        SocietyRadiantBlood = 10
    }
}
