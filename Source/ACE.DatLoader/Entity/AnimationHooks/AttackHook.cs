namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class AttackHook : IHook
    {
        public AttackCone AttackCone { get; set; }

        public static AttackHook ReadHookType(DatReader datReader)
        {
            AttackHook a = new AttackHook();
            AttackCone ac = new AttackCone();

            ac.PartIndex = datReader.ReadUInt32();
            ac.LeftX = datReader.ReadSingle();
            ac.LeftY = datReader.ReadSingle();
            ac.RightX = datReader.ReadSingle();
            ac.RightY = datReader.ReadSingle();
            ac.Radius = datReader.ReadSingle();
            ac.Height = datReader.ReadSingle();

            a.AttackCone = ac;

            return a;
        }
    }
}
