using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CharacterPositionExtensions
    {
        public static Position StartingPosition(uint characterId)
        {
            var startingPosition = new Position(characterId, PositionType.Location, 2130903469, 12.3199f, -28.482f, 0.0049999995f, 0.0f, 0.0f, -0.9408059f, -0.3389459f);
            return startingPosition;
        }

        public static Position InvalidPosition(uint characterId, PositionType type)
        {
            var invalidPosition = new Position();
            invalidPosition.CharacterId = characterId;
            invalidPosition.PositionType = type;
            invalidPosition.LandblockId = new LandblockId();
            return invalidPosition;
        }
    }
}