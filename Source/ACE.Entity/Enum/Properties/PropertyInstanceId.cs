﻿using System.ComponentModel;

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
        Viewer                           = 5,
        Generator                        = 6,
        Scribe                           = 7,
        CurrentCombatTarget              = 8,
        CurrentEnemy                     = 9,
        ProjectileLauncher               = 10,
        CurrentAttacker                  = 11,
        CurrentDamager                   = 12,
        CurrentFollowTarget              = 13,
        CurrentAppraisalTarget           = 14,
        CurrentFellowshipAppraisalTarget = 15,
        ActivationTarget                 = 16,
        Creator                          = 17,
        Victim                           = 18,
        Killer                           = 19,
        Vendor                           = 20,
        Customer                         = 21,
        Bonded                           = 22,
        Wounder                          = 23,
        Allegiance                       = 24,
        Patron                           = 25,
        Monarch                          = 26,
        CombatTarget                     = 27,
        HealthQueryTarget                = 28,
        LastUnlocker                     = 29,
        CrashAndTurnTarget               = 30,
        AllowedActivator                 = 31,
        HouseOwner                       = 32,
        House                            = 33,
        Slumlord                         = 34,
        ManaQueryTarget                  = 35,
        CurrentGame                      = 36,
        RequestedAppraisalTarget         = 37,
        AllowedWielder                   = 38,
        AssignedTarget                   = 39,
        LimboSource                      = 40,
        Snooper                          = 41,
        TeleportedCharacter              = 42,
        Pet                              = 43,
        PetOwner                         = 44,
        PetDevice                        = 45,

        // values over 9000 are ones that we have added and should not be sent to the client
        Subscription                     = 9001,
        Friend                           = 9002
    }

    public static class PropertyInstanceIdExtensions
    {
        public static string GetDescription(this PropertyInstanceId prop)
        {
            var description = EnumHelper.GetAttributeOfType<DescriptionAttribute>(prop);
            return description?.Description ?? prop.ToString();
        }
    }
}
