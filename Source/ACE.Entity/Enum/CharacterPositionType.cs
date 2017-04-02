namespace ACE.Entity.Enum
{
    public enum PositionType
    {
        /// <summary>
        /// Position of the bound lifestone
        /// </summary>
        LifestoneUsed = 0x04,

        /// <summary>
        /// Position of the spell tied lifestone
        /// </summary>
        LifestoneTied = 0x15,

        /// <summary>
        /// Portal recall postion
        /// </summary>
        PortalRecall = 0x09,

        /// <summary>
        /// Primary Portal Recal position
        /// </summary>
        PrimaryPortalRecall = 0x08,

        /// <summary>
        /// Secondary Portal Recal position
        /// </summary>
        SecondaryPortalRecall = 0x16,

        /// <summary>
        /// Allegiance hometown recall position - This is currently a placeholder/invalid position until
        /// the allegiance code has been built out.
        /// </summary>
        AllegianceHometown = 0x06,

        /// <summary>
        /// Mansion recall position - This is currently a placeholder/invalid position until
        /// the allegiance code has been built out.
        /// </summary>
        MansionRecall = 0x07,

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

        Undef = 0,
        Location = 1, // Current Position
        Destination = 2,
        Instantiation = 3,
        Sanctuary = 4, // Last Lifestone Used? (@ls)? | @home | @save | @recall
        Home = 5,
        ActivationMove = 6,
        Target = 7,
        LinkedPortalOne = 8, // Primary Portal Recall | Summon Primary Portal | Primary Portal Tie
        LastPortal = 9, // Portal Recall (Last Used Portal that can be recalled to)
        PortalStorm = 10,
        CrashAndTurn = 11,
        PortalSummonLOC = 12,
        HouseBoot = 13,
        LastOutsideDeath = 14, // Location of Corpse
        LinkedLifestone = 15, // Lifestone Recall | Lifestone Tie
        LinkedPortalTwo = 16, // Secondary Portal Recall | Summon Secondary Portal | Secondary Portal Tie
        Save1 = 17, // @save 1 | @home 1 | @recall 1
        Save2 = 18, // @save 2 | @home 2 | @recall 2
        Save3 = 19, // @save 3 | @home 3 | @recall 3
        Save4 = 20, // @save 4 | @home 4 | @recall 4
        Save5 = 21, // @save 5 | @home 5 | @recall 5
        Save6 = 22, // @save 6 | @home 6 | @recall 6
        Save7 = 23, // @save 7 | @home 7 | @recall 7
        Save8 = 24, // @save 8 | @home 8 | @recall 8
        Save9 = 25, // @save 9 | @home 9 | @recall 9
        RelativeDestination = 26,
        TeleportedCharacter = 27
    }

    public class CharacterPositionExtensions
    {
        public static CharacterPosition StartingPosition(uint characterId)
        {

            var startingPosition = new CharacterPosition();

            startingPosition.character_id = characterId;
            startingPosition.cell = 2130969005;
            startingPosition.positionType = (int)PositionType.Location;
            startingPosition.positionX = 12.3199f;
            startingPosition.positionY = -28.482f;
            startingPosition.positionZ = 0.0049999995f;
            startingPosition.rotationX = 0.0f;
            startingPosition.rotationY = 0.0f;
            startingPosition.rotationZ = -0.9408059f;
            startingPosition.rotationW = -0.3389459f;

            return startingPosition;
            
        }

        public static CharacterPosition InvalidPosition(uint characterId, PositionType type)
        {
            var invalidPosition = new CharacterPosition();

            invalidPosition.character_id = characterId;
            invalidPosition.cell = 0;
            invalidPosition.positionType = (uint) type;
            invalidPosition.positionX = 0.0f;
            invalidPosition.positionY = 0.0f;
            invalidPosition.positionZ = 0.0f;
            invalidPosition.rotationX = 0.0f;
            invalidPosition.rotationY = 0.0f;
            invalidPosition.rotationZ = 0.0f;
            invalidPosition.rotationW = 0.0f;

            return invalidPosition;
        }

        /// <summary>
        /// Returns a CharacterPosition from a Position and characterId.
        /// </summary>
        public static CharacterPosition positionToCharacterPosition(uint characterId, Position position, PositionType type)
        {

            var newCharacterPosition = new CharacterPosition();

            newCharacterPosition.character_id = characterId;
            newCharacterPosition.cell = position.LandblockId.Raw;
            newCharacterPosition.positionType = (uint)type;

            newCharacterPosition.positionX = position.Offset.X;
            newCharacterPosition.positionY = position.Offset.Y;
            newCharacterPosition.positionZ = position.Offset.Z;

            newCharacterPosition.rotationX = position.Facing.X;
            newCharacterPosition.rotationY = position.Facing.Y;
            newCharacterPosition.rotationZ = position.Facing.Z;
            newCharacterPosition.rotationW = position.Facing.W;

            return newCharacterPosition;
        }

        public static Position characterPositionToPosition(CharacterPosition characterPosition)
        {
            return new Position(characterPosition.cell, characterPosition.positionX, characterPosition.positionY,
                characterPosition.positionZ, characterPosition.rotationX, characterPosition.rotationY,
                characterPosition.rotationZ, characterPosition.rotationW);
        }
    }
}
