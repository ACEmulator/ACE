using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum Usable : long
    {
        Undef       = 0x00000,
        No          = 0x00001,
        Self        = 0x00002,
        Wielded     = 0x00004,
        Contained   = 0x00008,
        Viewed      = 0x00010,
        Remote      = 0x00020,
        NeverWalk   = 0x00040,
        ObjSelf     = 0x00080,
        SourceMask  = 0x0FFFF,
        TargetMask  = (0x10000 * -1),

        ContainedViewed                 = Contained | Viewed,
        ContainedViewedRemote           = Contained | Viewed | Remote,
        ContainedViewedRemoteNeverWalk  = Contained | Viewed | Remote | NeverWalk,

        ViewedRemote                    = Viewed | Remote,
        ViewedRemoteNeverWalk           = Viewed | Remote | NeverWalk,

        RemoteNeverWalk                 = Remote | NeverWalk,

        SourceWieldedTargetWielded              = 262148,
        SourceWieldedTargetContained            = 524292,
        SourceWieldedTargetViewed               = 1048580,
        SourceWieldedTargetRemote               = 2097156,
        SourceWieldedTargetRemoteNeverWalk      = 6291460,

        SourceContainedTargetWielded            = 262152,
        SourceContainedTargetContained          = 524296,
        SourceContainedTargetObjselfOrContained = 8912904,
        SourceContainedTargetSelfOrContained    = 655368,
        SourceContainedTargetViewed             = 1048584,
        SourceContainedTargetRemote             = 2097160,
        SourceContainedTargetRemoteNeverWalk    = 6291464,
        SourceContainedTargetRemoteOrSelf       = 2228232,

        SourceViewedTargetWielded               = 262160,
        SourceViewedTargetContained             = 524304,
        SourceViewedTargetViewed                = 1048592,
        SourceViewedTargetRemote                = 2097168,

        SourceRemoteTargetWielded               = 262176,
        SourceRemoteTargetContained             = 524320,
        SourceRemoteTargetViewed                = 1048608,
        SourceRemoteTargetRemote                = 2097184,
        SourceRemoteTargetRemoteNeverWalk       = 6291488,
    }
}
