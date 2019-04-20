using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyInstanceId : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi

        // description attributes are used by the weenie editor for a cleaner display name

        Undef = 0,
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
        [ServerOnly][Ephemeral]
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
        Pet                              = 43,
        PetOwner                         = 44,
        PetDevice                        = 45,

        [ServerOnly]
        PCAPRecordedObjectIID            = 8000,
        [ServerOnly]
        PCAPRecordedParentIID            = 8008
    }

    public static class PropertyInstanceIdExtensions
    {
        public static string GetDescription(this PropertyInstanceId prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }
    }
}
