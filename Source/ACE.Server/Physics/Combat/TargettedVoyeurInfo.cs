using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Combat
{
    public class TargettedVoyeurInfo
    {
        public uint ObjectID;
        public double Quantum;
        public float Radius;
        public Position LastSentPosition;

        public TargettedVoyeurInfo() { }

        public TargettedVoyeurInfo(uint objectID, float radius, double quantum)
        {
            ObjectID = objectID;
            Quantum = quantum;
            Radius = radius;
        }
    }
}
