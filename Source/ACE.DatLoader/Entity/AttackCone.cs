namespace ACE.DatLoader.Entity
{
    public class AttackCone
    {
        public uint PartIndex { get; set; }
        
        // these Left and Right are technically Vec2D types
        public float LeftX { get; set; }
        public float LeftY { get; set; }
        public float RightX { get; set; }
        public float RightY { get; set; }

        public float Radius { get; set; }
        public float Height { get; set; }
    }
}
