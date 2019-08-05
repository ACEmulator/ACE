
namespace ACE.Entity.Enum
{
    public enum EnvironChangeType
    {
        Clear               = 0x00,
        RedFog              = 0x01,
        BlueFog             = 0x02,
        WhiteFog            = 0x03,
        GreenFog            = 0x04,
        BlackFog            = 0x05,
        BlackFog2           = 0x06,
        RoarSound           = 0x65,
        BellSound           = 0x66,
        Chant1Sound         = 0x67,
        Chant2Sound         = 0x68,
        DarkWhispers1Sound  = 0x69,
        DarkWhispers2Sound  = 0x6A,
        DarkLaughSound      = 0x6B,
        DarkWindSound       = 0x6C,
        DarkSpeechSound     = 0x6D,
        DrumsSound          = 0x6E,
        GhostSpeakSound     = 0x6F,
        BreathingSound      = 0x70,
        HowlSound           = 0x71,
        LostSoulsSound      = 0x72,
        SquealSound         = 0x75,
        Thunder1Sound       = 0x76,
        Thunder2Sound       = 0x77,
        Thunder3Sound       = 0x78,
        Thunder4Sound       = 0x79,
        Thunder5Sound       = 0x7A,
        Thunder6Sound       = 0x7B
    }

    public static class EnvironChangeTypeExtensions
    {
        public static bool IsFog(this EnvironChangeType type)
        {
            return type <= EnvironChangeType.BlackFog2;
        }

        public static bool IsSound(this EnvironChangeType type)
        {
            return type >= EnvironChangeType.RoarSound;
        }
    }
}
