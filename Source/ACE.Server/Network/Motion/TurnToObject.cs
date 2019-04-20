using System.IO;
using ACE.Entity;
using ACE.Server.Entity;

namespace ACE.Server.Network.Structure
{
    public class TurnToObject
    {
        public ObjectGuid Target;
        public float DesiredHeading;               // heading of the object to turn to.
                                                   // this is used instead of the DesiredHeading in the TurnToParameters
        public TurnToParameters TurnToParameters;  // set of turning parameters

        public TurnToObject(Motion motion)
        {
            Target = motion.TargetGuid;
            DesiredHeading = motion.DesiredHeading;

            TurnToParameters = new TurnToParameters(motion);
        }
    }

    public static class TurnToObjectExtensions
    {
        public static void Write(this BinaryWriter writer, TurnToObject turnTo)
        {
            writer.WriteGuid(turnTo.Target);
            writer.Write(turnTo.DesiredHeading);
            writer.Write(turnTo.TurnToParameters);
        }
    }
}
