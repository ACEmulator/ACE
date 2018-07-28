using System.IO;
using System.Numerics;
using System.Text;

namespace ACE.Server.Network.Structure
{
    public class JumpPack
    {
        public float Extent;    // jump power 0-1
        public Vector3 Velocity;
        public ushort InstanceSequence;
        public ushort ServerControlSequence;
        public ushort TeleportSequence;
        public ushort ForcePositionSequence;

        public JumpPack() { }

        public JumpPack(BinaryReader reader)
        {
            Extent = reader.ReadSingle();
            Velocity = reader.ReadVector3();
            InstanceSequence = reader.ReadUInt16();
            ServerControlSequence = reader.ReadUInt16();
            TeleportSequence = reader.ReadUInt16();
            ForcePositionSequence = reader.ReadUInt16();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Extent: " + Extent);
            sb.AppendLine("Velocity: " + Velocity);
            sb.AppendLine("InstanceSequence: " + InstanceSequence);
            sb.AppendLine("ServerControlSequence: " + ServerControlSequence);
            sb.AppendLine("TeleportSequence: " + TeleportSequence);
            sb.AppendLine("ForcePositionSequence: " + ForcePositionSequence);

            return sb.ToString();
        }
    }
}
