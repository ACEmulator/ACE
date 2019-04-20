using System.IO;
using ACE.Server.Entity;

namespace ACE.Server.Network.Structure
{
    public class TurnToHeading
    {
        public TurnToParameters TurnToParameters;  // set of turning parameters

        public TurnToHeading(Motion motion)
        {
            TurnToParameters = new TurnToParameters(motion);
        }
    }

    public static class TurnToHeadingExtensions
    {
        public static void Write(this BinaryWriter writer, TurnToHeading turnTo)
        {
            writer.Write(turnTo.TurnToParameters);
        }
    }
}
