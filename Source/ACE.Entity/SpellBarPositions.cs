
namespace ACE.Entity
{
    public class SpellBarPositions
    {
        public uint SpellBarId { get; set; }

        public uint SpellBarPositionId { get; set; }

        public uint SpellId { get; set; }

        public SpellBarPositions(uint spellBarId, uint spellBarPositionId, uint spellId)
        {
            SpellBarId = spellBarId - 1;
            SpellBarPositionId = spellBarPositionId - 1;
            SpellId = spellId;
        }
    }
}
