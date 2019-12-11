using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum Usable: uint
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

        ContainedViewed                 = Contained | Viewed,
        ContainedViewedRemote           = Contained | Viewed | Remote,
        ContainedViewedRemoteNeverWalk  = Contained | Viewed | Remote | NeverWalk,

        ViewedRemote                    = Viewed | Remote,
        ViewedRemoteNeverWalk           = Viewed | Remote | NeverWalk,

        RemoteNeverWalk                 = Remote | NeverWalk,

        SourceWieldedTargetWielded              = 0x040004,
        SourceWieldedTargetContained            = 0x080004,
        SourceWieldedTargetViewed               = 0x100004,
        SourceWieldedTargetRemote               = 0x200004,
        SourceWieldedTargetRemoteNeverWalk      = 0x600004,

        SourceContainedTargetWielded            = 0x040008,
        SourceContainedTargetContained          = 0x080008,
        SourceContainedTargetObjselfOrContained = 0x880008,
        SourceContainedTargetSelfOrContained    = 0x0A0008,
        SourceContainedTargetViewed             = 0x100008,
        SourceContainedTargetRemote             = 0x200008,
        SourceContainedTargetRemoteNeverWalk    = 0x600008,
        SourceContainedTargetRemoteOrSelf       = 0x220008,

        SourceViewedTargetWielded               = 0x040010,
        SourceViewedTargetContained             = 0x080010,
        SourceViewedTargetViewed                = 0x100010,
        SourceViewedTargetRemote                = 0x200010,

        SourceRemoteTargetWielded               = 0x040020,
        SourceRemoteTargetContained             = 0x080020,
        SourceRemoteTargetViewed                = 0x100020,
        SourceRemoteTargetRemote                = 0x200020,
        SourceRemoteTargetRemoteNeverWalk       = 0x600020,

        SourceMask = 0xFFFF,
        TargetMask = 0xFFFF0000,
    }

    public static class UsableExtensions
    {
        public static Usable GetSourceFlags(this Usable usable)
        {
            return usable & Usable.SourceMask;
        }

        public static Usable GetTargetFlags(this Usable usable)
        {
            return (Usable)((uint)usable >> 16);
        }
    }
}
