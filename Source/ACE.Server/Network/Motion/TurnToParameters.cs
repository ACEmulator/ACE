using System.IO;
using ACE.Entity.Enum;
using ACE.Server.Entity;

namespace ACE.Server.Network.Structure
{
    public class TurnToParameters
    {
        public MovementParams MovementParams;
        public float Speed;                     // speed of the turn
        public float DesiredHeading;            // the angle to turn to

        public TurnToParameters(Motion motion)
        {
            MovementParams = motion.MoveToParameters.MovementParameters;
            Speed = motion.MoveToParameters.Speed;
            DesiredHeading = motion.DesiredHeading;
        }
    }

    public static class TurnToParametersExtensions
    {
        public static void Write(this BinaryWriter writer, TurnToParameters turnTo)
        {
            writer.Write((uint)turnTo.MovementParams);
            writer.Write(turnTo.Speed);
            writer.Write(turnTo.DesiredHeading);
        }
    }
}
