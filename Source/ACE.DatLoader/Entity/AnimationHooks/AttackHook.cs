using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class AttackHook : AnimationHook
    {
        public AttackCone AttackCone { get; } = new AttackCone();

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            AttackCone.Unpack(reader);
        }
    }
}
