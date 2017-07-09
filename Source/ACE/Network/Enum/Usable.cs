namespace ACE.Network.Enum
{
    public enum Usable : int
    {
        Undef       = 0x00,
        No          = 0x01,
        Self        = 0x02,
        Wielded     = 0x04,
        Contained   = 0x08,
        Viewed      = 0x10,
        Remote      = 0x20,
        NeverWalk   = 0x40,
        ObjSelf     = 0x80,

        ContainedViewed                 = 24,
        ContainedViewedRemote           = 56,
        ContainedViewedRemoteNeverWalk  = 120,

        ViewedRemote            = 48,
        ViewedRemoteNeverWalk   = 112,

        RemoteNeverWalk         = 96,

        SourceWieldedTargetWielded          = 262148,
        SourceWieldedTargetContained        = 524292,
        SourceWieldedTargetViewed           = 1048580,
        SourceWieldedTargetRemote           = 2097156,
        SourceWieldedTargetRemoteNeverWalk  = 6291460,

        SourceContainedTargetWielded            = 262152,
        SourceContainedTargetContained          = 524296,
        SourceContainedTargetObjselfOrContained = 8912904,
        SourceContainedTargetSelfOrContained    = 655368,
        SourceContainedTargetViewed             = 1048584,
        SourceContainedTargetRemote             = 2097160,
        SourceContainedTargetRemoteNeverWalk    = 6291464,
        SourceContainedTargetRemoteOrSelf       = 2228232,

        SourceViewedTargetWielded   = 262160,
        SourceViewedTargetContained = 524304,
        SourceViewedTargetViewed    = 1048592,
        SourceViewedTargetRemote    = 2097168,

        SourceRemoteTargetWielded           = 262176,
        SourceRemoteTargetContained         = 524320,
        SourceRemoteTargetViewed            = 1048608,
        SourceRemoteTargetRemote            = 2097184,
        SourceRemoteTargetRemoteNeverWalk   = 6291488,

        SourceMask = 65535,
        TargetMask = -65536,
    }
}
