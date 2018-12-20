namespace ACE.Server.Network.Sequence
{
    public enum SequenceType
    {
        ObjectPosition                        = 0,
        ObjectMovement                        = 1,
        ObjectState                           = 2,
        ObjectVector                          = 3,
        ObjectTeleport                        = 4,
        ObjectServerControl                   = 5,
        ObjectForcePosition                   = 6,
        ObjectVisualDesc                      = 7,
        ObjectInstance                        = 8,

        Motion,

        PrivateUpdateAttribute,
        PrivateUpdateAttribute2ndLevel,
        PrivateUpdateSkill,

        PrivateUpdatePropertyInt,
        PrivateUpdatePropertyInt64,        
        PrivateUpdatePropertyBool,
        PrivateUpdatePropertyDouble,        
        PrivateUpdatePropertyDataID,
        PrivateUpdatePropertyInstanceID,
        PrivateUpdatePropertyString,
        PrivateUpdatePosition,

        PublicUpdatePropertyInt,
        PublicUpdatePropertyInt64,
        PublicUpdatePropertyBool,
        PublicUpdatePropertyDouble,
        PublicUpdatePropertyDataID,
        PublicUpdatePropertyInstanceId,
        PublicUpdatePropertyString,
        PublicUpdatePosition,

        SetStackSize,

        Confirmation,
    }
}
