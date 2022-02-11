// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Defines the Positions types that we need to track.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ACE.Entity.Enum.Properties
{
    /// <summary>
    /// The enumerations for the different positions we want to track.  This code provided by Ripley via pastebin
    /// </summary>
    public enum PositionType : ushort
    {
        // S_CONSTANT: Type:             0x108E, Value: 11, CRASH_AND_TURN_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 17, SAVE_1_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 14, LAST_OUTSIDE_DEATH_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 10, PORTAL_STORM_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 18, SAVE_2_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 19, SAVE_3_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 20, SAVE_4_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 1, LOCATION_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 13, HOUSE_BOOT_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 21, SAVE_5_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 3, INSTANTIATION_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 9, LAST_PORTAL_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 16, LINKED_PORTAL_TWO_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 2, DESTINATION_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 22, SAVE_6_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 7, TARGET_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 23, SAVE_7_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 26, RELATIVE_DESTINATION_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 24, SAVE_8_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 25, SAVE_9_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 0, UNDEF_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 15, LINKED_LIFESTONE_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 27, TELEPORTED_CHARACTER_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 12, PORTAL_SUMMON_LOC_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 6, ACTIVATION_MOVE_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 4, SANCTUARY_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 5, HOME_POSITION
        // S_CONSTANT: Type:             0x108E, Value: 8, LINKED_PORTAL_ONE_POSITION

        /// <summary>
        /// I got nothing for you.
        /// </summary>
        Undef = 0,

        /// <summary>
        /// Current Position
        /// </summary>
        Location = 1,

        /// <summary>
        /// May be used to store where we are headed when we teleport (?)
        /// </summary>
        Destination = 2,

        /// <summary>
        /// Where will we pop into the world (?)
        /// </summary>
        Instantiation = 3,

        /// <summary>
        ///  Last Lifestone Used? (@ls)? | @home | @save | @recall
        /// </summary>
        Sanctuary = 4,

        /// <summary>
        /// This is the home, starting, or base position of an object.
        /// It's usually the position the object first spawned in at.
        /// </summary>
        [Ephemeral]
        Home = 5,

        /// <summary>
        /// The need to research
        /// </summary>
        ActivationMove = 6,

        /// <summary>
        /// The the position of target.
        /// </summary>
        Target = 7,

        /// <summary>
        /// Primary Portal Recall | Summon Primary Portal | Primary Portal Tie
        /// </summary>
        LinkedPortalOne = 8,

        /// <summary>
        /// Portal Recall (Last Used Portal that can be recalled to)
        /// </summary>
        LastPortal = 9,

        /// <summary>
        /// The portal storm - need research - maybe where you were portaled from or to - to does not seem likely to me.
        /// </summary>
        PortalStorm = 10,

        /// <summary>
        /// The crash and turn - I can't wait to find out.
        /// </summary>
        CrashAndTurn = 11,

        /// <summary>
        /// We are tracking what the portal ties are - could this be the physical location of the portal you summoned?   More research needed.
        /// </summary>
        PortalSummonLoc = 12,

        /// <summary>
        /// That little spot you get sent to just outside the barrier when the slum lord evicts you (??)
        /// </summary>
        HouseBoot = 13,

        /// <summary>
        /// The last outside death. --- boy would I love to extend this to cover deaths in dungeons as well.
        /// </summary>
        LastOutsideDeath = 14, // Location of Corpse

        /// <summary>
        /// The linked lifestone - Lifestone Recall | Lifestone Tie
        /// </summary>
        LinkedLifestone = 15,

        /// <summary>
        /// Secondary Portal Recall | Summon Secondary Portal | Secondary Portal Tie
        /// </summary>
        LinkedPortalTwo = 16,

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save1 = 17, // @save 1 | @home 1 | @recall 1

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save2 = 18, // @save 2 | @home 2 | @recall 2

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save3 = 19, // @save 3 | @home 3 | @recall 3

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save4 = 20, // @save 4 | @home 4 | @recall 4

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save5 = 21, // @save 5 | @home 5 | @recall 5

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save6 = 22, // @save 6 | @home 6 | @recall 6

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save7 = 23, // @save 7 | @home 7 | @recall 7

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save8 = 24, // @save 8 | @home 8 | @recall 8

        /// <summary>
        /// Admin Quick Recall Positions
        /// </summary>
        Save9 = 25, // @save 9 | @home 9 | @recall 9

        /// <summary>
        /// Position data is relative to Location
        /// </summary>
        RelativeDestination = 26,

        /// <summary>
        /// Admin - Position to return player to when using @telereturn which is where a character was at time of admin using @teletome
        /// </summary>
        TeleportedCharacter = 27,

        [ServerOnly]
        PCAPRecordedLocation = 8040
    }
}
