using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PortalBitmask : byte
    {
        Undef           = 0x00,
        Unrestricted    = 0x01,
        NoPk            = 0x02,
        NoPKLite        = 0x04,
        NoNPK           = 0x08,
        NoSummon        = 0x10,
        NoRecall        = 0x20

        // todo: These came from ACLogView. I'm not sure what the source is.
        // If we are to transition ACLogView (and other tools) to ue ACE.Entity, we may need these.
        // Mag-nus 2017-07-18

        /*
        Undef_PortalEnum = 0,
        Not_Passable_PortalEnum = 0,
        Player_Passable_PortalEnum = 1,
        PK_Banned_PortalEnum = 2,
        PKLite_Banned_PortalEnum = 4,
        Player_NPK_Only_PortalEnum = 7,
        NPK_Banned_PortalEnum = 8,
        Player_PK_PKL_Only_PortalEnum = 9,
        Not_Summonable_PortalEnum = 16,
        Player_NotSummonable_PortalEnum = 17,
        Player_PK_PKL_Only_NotSummonable_PortalEnum = 25,
        Not_Recallable_Nor_Linkable_PortalEnum = 32,
        Player_NotRecallable_NotLinkable_PortalEnum = 33,
        Player_PK_PKL_Only_NotRecallable_NotLinkable_PortalEnum = 41,
        Player_NotRecallable_NotLinkable_NotSummonable_PortalEnum = 49,
        Player_PK_PKL_Only_NotSummonable_NotRecallable_NotLinkable_PortalEnum = 57,
        Only_Olthoi_PCs_PortalEnum = 64,
        No_Olthoi_PCs_PortalEnum = 128,
        No_Vitae_PortalEnum = 256,
        No_New_Accounts_PortalEnum = 512
        */
    }
}
