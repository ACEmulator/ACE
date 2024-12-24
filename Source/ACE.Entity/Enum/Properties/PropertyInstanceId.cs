using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // No properties are sent to the client unless they featured an attribute.
    // SendOnLogin gets sent to players in the PlayerDescription event
    // AssessmentProperty gets sent in successful appraisal
    public enum PropertyInstanceId : ushort
    {
        Undef                            = 0,
        Owner                            = 1,
        Container                        = 2,
        Wielder                          = 3,
        Freezer                          = 4,
        [Ephemeral]
        Viewer                           = 5,
        [Ephemeral]
        Generator                        = 6,
        Scribe                           = 7,
        [Ephemeral]
        CurrentCombatTarget              = 8,
        [Ephemeral]
        CurrentEnemy                     = 9,
        ProjectileLauncher               = 10,
        [Ephemeral]
        CurrentAttacker                  = 11,
        [Ephemeral]
        CurrentDamager                   = 12,
        [Ephemeral]
        CurrentFollowTarget              = 13,
        [Ephemeral]
        CurrentAppraisalTarget           = 14,
        [Ephemeral]
        CurrentFellowshipAppraisalTarget = 15,
        ActivationTarget                 = 16,
        Creator                          = 17,
        Victim                           = 18,
        Killer                           = 19,
        Vendor                           = 20,
        Customer                         = 21,
        Bonded                           = 22,
        Wounder                          = 23,
        [SendOnLogin]
        Allegiance                       = 24,
        [SendOnLogin]
        Patron                           = 25,
        Monarch                          = 26,
        [Ephemeral]
        CombatTarget                     = 27,
        [Ephemeral]
        HealthQueryTarget                = 28,
        [Ephemeral]
        LastUnlocker                     = 29,
        CrashAndTurnTarget               = 30,
        AllowedActivator                 = 31,
        HouseOwner                       = 32,
        House                            = 33,
        Slumlord                         = 34,
        [Ephemeral]
        ManaQueryTarget                  = 35,
        CurrentGame                      = 36,
        [Ephemeral]
        RequestedAppraisalTarget         = 37,
        AllowedWielder                   = 38,
        AssignedTarget                   = 39,
        LimboSource                      = 40,
        Snooper                          = 41,
        TeleportedCharacter              = 42,
        [Ephemeral]
        Pet                              = 43,
        PetOwner                         = 44,
        [Ephemeral]
        PetDevice                        = 45,

        /* Custom Properties */
        PCAPRecordedObjectIID            = 8000,
        PCAPRecordedParentIID            = 8008,
    }
}
