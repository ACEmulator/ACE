using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CharacterPositionExtensions
    {
        public static Position StartingPosition()
        {
            var startingPosition = new Position(2130903469, 12.3199f, -28.482f, 0.0049999995f, 0.0f, 0.0f, -0.9408059f, -0.3389459f);

            return startingPosition;
        }

        public static Position InvalidPosition(uint characterId)
        {
            var invalidPosition = new Position();
            invalidPosition.LandblockId = new LandblockId();
            return invalidPosition;
        }
    }
}