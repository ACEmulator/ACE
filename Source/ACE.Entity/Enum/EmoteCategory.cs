namespace ACE.Entity.Enum
{
    /// <summary>
    /// exported from the decompiled client.  actual usage of these is 100% speculative.
    /// </summary>
    public enum EmoteCategory
    {
        Invalid                   = 0,
        /// <summary>
        /// Not used as a direct refusal to accept item but as a mechanism to "examine" item but allow player to keep
        /// Supported by pcap data
        /// </summary>
        Refuse                    = 1,
        Vendor                    = 2,
        Death                     = 3,
        Portal                    = 4,
        HeartBeat                 = 5,
        Give                      = 6,
        Use                       = 7,
        Activation                = 8,
        Generation                = 9,
        PickUp                    = 10,
        Drop                      = 11,
        QuestSuccess              = 12,
        QuestFailure              = 13,
        Taunt                     = 14,
        WoundedTaunt              = 15,
        KillTaunt                 = 16,
        NewEnemy                  = 17,
        Scream                    = 18,
        Homesick                  = 19,
        ReceiveCritical           = 20,
        ResistSpell               = 21,
        TestSuccess               = 22,
        TestFailure               = 23,
        HearChat                  = 24,
        Wield                     = 25,
        UnWield                   = 26,
        EventSuccess              = 27,
        EventFailure              = 28,
        TestNoQuality             = 29,
        QuestNoFellow             = 30,
        TestNoFellow              = 31,
        GotoSet                   = 32,
        NumFellowsSuccess         = 33,
        NumFellowsFailure         = 34,
        NumCharacterTitlesSuccess = 35,
        NumCharacterTitlesFailure = 36,
        ReceiveLocalSignal        = 37,
        ReceiveTalkDirect         = 38
    }
}
