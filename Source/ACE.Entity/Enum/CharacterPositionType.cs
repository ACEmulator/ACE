namespace ACE.Entity.Enum
{
    public enum CharacterPositionType
    {
        /// <summary>
        /// Physical position of the player
        /// </summary>
        PhysicalLocation = 0x01,

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
        /// Allegiance hometown recall position
        /// </summary>
        AllegianceHometown = 0x06,

        /// <summary>
        /// Mansion recall position
        /// </summary>
        MansionRecall = 0x07
    }

    public static class CharacterPositionExtensions
    {
        public static CharacterPosition StartingPosition
        {
            get
            {
                var startingPosition = new CharacterPosition();

                startingPosition.cell = 2130969005;
                startingPosition.positionType = (int)CharacterPositionType.PhysicalLocation;
                startingPosition.positionX = 12.3199f;
                startingPosition.positionY = -28.482f;
                startingPosition.positionZ = 0.0049999995f;
                startingPosition.rotationX = 0.0f;
                startingPosition.rotationY = 0.0f;
                startingPosition.rotationZ = -0.9408059f;
                startingPosition.rotationW = -0.3389459f;

                return startingPosition;
            }
        }

        public static CharacterPosition InvalidPosition
        {
            get
            {
                var invalidPosition = new CharacterPosition();

                invalidPosition.cell = 0;
                invalidPosition.positionX = 0.0f;
                invalidPosition.positionY = 0.0f;
                invalidPosition.positionZ = 0.0f;
                invalidPosition.rotationX = 0.0f;
                invalidPosition.rotationY = 0.0f;
                invalidPosition.rotationZ = 0.0f;
                invalidPosition.rotationW = 0.0f;

                return invalidPosition;
            }
        }

        /// <summary>
        /// Returns a CharacterPosition from a Position and characterId.
        /// </summary>
        public static CharacterPosition positionToCharacterPosition(uint characterId, Position position, Positions type)
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

        public static CharacterPosition positionToCharacterPosition(uint characterId, Position position, CharacterPositionType type)
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
