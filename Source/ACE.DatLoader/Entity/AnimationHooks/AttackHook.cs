namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class AttackHook : IHook
    {
        public AttackCone AttackCone { get; private set; }

        public static AttackHook ReadHookType(DatReader datReader)
        {
            AttackHook a = new AttackHook();

            a.AttackCone = AttackCone.Read(datReader);

            return a;
        }
    }
}
