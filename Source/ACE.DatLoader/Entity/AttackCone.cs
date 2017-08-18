namespace ACE.DatLoader.Entity
{
    public class AttackCone
    {
        public uint PartIndex { get; set; }
        
        // these Left and Right are technically Vec2D types
        public float LeftX { get; private set; }
        public float LeftY { get; private set; }
        public float RightX { get; private set; }
        public float RightY { get; private set; }

        public float Radius { get; private set; }
        public float Height { get; private set; }

        public static AttackCone Read(DatReader datReader)
        {
            AttackCone a = new AttackCone();

            a.PartIndex = datReader.ReadUInt32();

            a.LeftX = datReader.ReadSingle();
            a.LeftY = datReader.ReadSingle();

            a.RightX = datReader.ReadSingle();
            a.RightY = datReader.ReadSingle();

            a.Radius = datReader.ReadSingle();
            a.Height = datReader.ReadSingle();
            return a;
        }
    }
}
