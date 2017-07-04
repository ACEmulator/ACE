using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public enum SequenceType
    {
        ObjectPosition = 0,
        ObjectMovement = 1,
        ObjectState = 2,
        ObjectVector = 3,
        ObjectTeleport = 4,
        ObjectServerControl = 5,
        ObjectForcePosition = 6,
        ObjectVisualDesc = 7,
        ObjectInstance = 8,
        PrivateUpdateAttribute = 9,
        PrivateUpdateAttribute2ndLevel = 10,
        PrivateUpdateSkill = 11,
        PrivateUpdatePropertyInt64 = 12,
        PrivateUpdatePropertyInt = 13,
        PrivateUpdatePropertyString = 14,
        PrivateUpdatePropertyBool = 15,
        PrivateUpdatePropertyDouble = 16,
        Motion = 17,
        PrivateUpdatePropertyDataID = 18,
    }
}
