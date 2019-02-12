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

        UpdatePropertyInt,
        UpdatePropertyInt64,        
        UpdatePropertyBool,
        UpdatePropertyDouble,        
        UpdatePropertyDataID,
        UpdatePropertyInstanceID,
        UpdatePropertyString,
        UpdateRestrictionDB,

        UpdateAttribute,
        UpdateAttribute2ndLevel,
        UpdatePosition,
        UpdateSkill,
    }
}
