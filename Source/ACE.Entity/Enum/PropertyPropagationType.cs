using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity.Enum
{
    public enum PropertyPropagationType
    {
        NetPredictedSharedVisually = 0x0,
        NetPredictedSharedPrivately = 0x1,
        NetSharedVisually = 0x2,
        NetSharedPrivately = 0x3,
        NetNotShared = 0x4,
        WorldSharedWithServers = 0x5,
        WorldSharedWithServersAndClients = 0x6,
    }
}
